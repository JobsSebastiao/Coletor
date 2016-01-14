using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Classes;
using TitaniumColector.Classes.Procedimentos;
using System.Globalization;
using TitaniumColector.Classes.Dao;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using TitaniumColector.Classes.Exceptions;
using TitaniumColector.Classes.Utility;
using TitaniumColector.Classes.Model;
using System.Drawing;

namespace TitaniumColector.Forms
{
    public partial class FrmProposta : Form
    {
        //OBJECTS
        private Proposta objProposta;
        private String inputText;
        private Parametro paramValidarSequencia;
        private List<String> ListInformacoesProposta { get; set; }

        //ENUM
        public enum enumCor { RED = 0, BLACK = 1, BLUE = 2 }
        public enum enumCursor { WAIT = 0, DEFAULT = 1 }


        //Contrutor.
        public FrmProposta()
        {
            InitializeComponent();
            configControls();
            ProcedimentosLiberacao.carregaBaseMobile(this);
            //this.carregaBaseMobile();
         }

    #region "EVENTOS FORM"

        private void FrmProposta_Load(object sender, System.EventArgs e)
        {
            this.clearFormulario(true, true);
            this.carregarProposta();
            //this.carregarDadosProposta();
            Cursor.Current = Cursors.Default;
        }

        void mnuOpcoes_Terminar_Click(object sender, System.EventArgs e)
        {
            try
            {
                ICall form = new FrmAcao(true);
                this.exitForm(form,true,true);
            }
            catch (Exception ex)
            {
                MainConfig.errorMessage(ex.Message, "Logout");
            }
        }
        
        /// <summary>
        /// Menu evento ao clicar em Opções/Logout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOpcoes_Logout_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (this.exitForm(new frmLogin(true),true,true )!= DialogResult.Cancel) {
                    MainConfig.UserOn.registrarAcesso(Usuario.statusLogin.NAOLOGADO);
                }
            }
            catch (Exception ex)
            {
                MainConfig.errorMessage(ex.Message, "Logout");
            }
            
        }
     
        /// <summary>
        /// Menu evento ao clicar em Opções/Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOpcoes_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     
        /// <summary>
        /// Recebe o Valor de input durante a leitura do dispositivo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmProposta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                if (inputText != "" && inputText != null)
                {
                    Etiqueta.Tipo tipoEtiqueta = Leitor.validaInputValueEtiqueta(inputText,new EtiquetaVenda());

                    switch (tipoEtiqueta)
                    {
                        case Etiqueta.Tipo.INVALID:

                            inputText = string.Empty;
                            mostrarMensagem(enumCor.RED," Tipo de Etiqueta inválida!!!",enumCursor.DEFAULT);
                            break;

                        case Etiqueta.Tipo.QRCODE:

                            this.liberarItem(inputText,tipoEtiqueta);
                            inputText = string.Empty;
                            break;

                        case Etiqueta.Tipo.BARRAS:
                            
                            paramValidarSequencia = MainConfig.Permissoes_TB1210.retornarParametro("ValidarSequencia");

                            if (paramValidarSequencia.Valor == "1")
                            {
                                this.liberarItem(inputText, tipoEtiqueta);
                                inputText = string.Empty;
                                break;
                            }
                            else
                            {
                                inputText = string.Empty;
                                //tbMensagem.Text = "As configurações atuais não permitem validar etiquetas do tipo Ean13!";
                                mostrarMensagem(enumCor.RED, "As configurações atuais não permitem validar etiquetas do tipo Ean13!", enumCursor.DEFAULT);
                                break;
                            }

                        default:

                            inputText = string.Empty;
                            //tbMensagem.Text = " Tipo de Etiqueta inválida!!!";
                            mostrarMensagem(enumCor.RED, " Tipo de Etiqueta inválida!!!", enumCursor.DEFAULT);
                            break;
                    }

                }
            }
            else
            {
                inputText += e.KeyChar.ToString();
            }
        }

        /// <summary>
        /// Valida o fechamento do form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmProposta_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            DialogResult result = exitForm(true);

            if (result == DialogResult.No || result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnVolumes_Click(object sender, System.EventArgs e)
        {
            FrmVolumes frmVolumes = new FrmVolumes();
            frmVolumes.ShowDialog();
            this.lbQtdVolumes.Text = ProcedimentosLiberacao.TotalVolumes.ToString();
            //atualiza peso total pedido
            ProcedimentosLiberacao.atualizarPesoTotalPedido();
        }

    #endregion

    #region "CARGA INICIAL DE INFORMAÇÕES DO PRODUTO A SER TRABALHADO E DO FORMULÁRIO"

        /// <summary>
        ///  Preenche um objeto proposta com todas as informações contidas na base de dados da Proposta e de todos os seus itens.
        /// </summary>
        /// <returns> Objeto Proposta </returns>
        private void carregarProposta()
        {
            try
            {
                objProposta = ProcedimentosLiberacao.carregarProposta(this);
            }
            catch (ArithmeticException ex) 
            {
                StringBuilder sbMsg = new StringBuilder();
                sbMsg.Append("Problemas durante o processamento de informações sobre a proposta.\n");
                sbMsg.AppendFormat("Error : {0}", ex.Message);
                MainConfig.errorMessage(sbMsg.ToString(), "Operação Inválida!");
            }
            catch (Exception ex)
            {
                StringBuilder sbMsg = new StringBuilder();
                sbMsg.Append("Problemas durante o processamento de informações sobre a proposta \n");
                sbMsg.AppendFormat("Error : {0}", ex.Message);
                sbMsg.Append("Contate o Administrador do sistema.");
                MainConfig.errorMessage(sbMsg.ToString(), "Sistem Error!");
            }
        }
        /// <summary>
        /// Carrega o form com as informações nescessárias para separação do próximo item.
        /// </summary>
        /// <param name="objProposta">ObjProposta já setado com as informações do seu próximo item. ITEM INDEX[0] DA LISTOBJITEMPROPOSTA</param>
        /// <param name="qtdPecas">Quantidade de peças ainda a separar</param>
        /// <param name="qtdItens">Quantidade itens ainda a liberar</param>
        /// <remarks > O objeto proposta já deve ter sido carregado com o próximo item que será trabalhado pois as informações serão retira
        ///           retiradas do item de index [0] na ListObjItemProsta
        /// </remarks>
        public void fillCamposForm(Proposta objProposta,Double qtdPecas, Double qtdItens)
        {
            lbNumeroPedido.Text = objProposta.Numero.ToString();
            lbNomeCliente.Text = objProposta.RazaoCliente;
            lbQtdPecas.Text = qtdPecas.ToString() + " Pçs";
            lbQtdItens.Text = qtdItens.ToString() + " Itens";
            tbPartNumber.Text = objProposta.ListObjItemProposta[0].Partnumber;
            tbDescricao.Text = objProposta.ListObjItemProposta[0].Descricao;
            if (objProposta.ListObjItemProposta[0].NomeLocalLote.Contains(','))
            {
                tbLocal.Font = MainConfig.FontGrandeBold;
            }
            tbLocal.Text = objProposta.ListObjItemProposta[0].NomeLocalLote;
            tbQuantidade.Text = objProposta.ListObjItemProposta[0].Quantidade.ToString();
        }

        public void fillCamposForm(Proposta prop)
        {

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");

            lbNumeroPedido.Text = prop.Numero.ToString();        //numeroProposta.ToString();
            lbNomeCliente.Text = prop.RazaoCliente.ToString();  // nomeCliente;
            lbQtdPecas.Text = MainConfig.intOrDecimal(ProcedimentosLiberacao.TotalPecas.ToString()) + " Pçs";
            lbQtdVolumes.Text = ProcedimentosLiberacao.TotalVolumes.ToString();
            lbQtdItens.Text = ProcedimentosLiberacao.TotalItens.ToString() + " Itens";
            tbPartNumber.Text = prop.ListObjItemProposta[0].Partnumber.ToString();
            tbDescricao.Text = prop.ListObjItemProposta[0].Descricao;
            if (prop.ListObjItemProposta[0].NomeLocalLote.Contains(','))
            {
                tbLocal.Font = MainConfig.FontGrandeBold;
            }
            tbLocal.Text = prop.ListObjItemProposta[0].NomeLocalLote;
            tbQuantidade.Text = MainConfig.intOrDecimal(prop.ListObjItemProposta[0].Quantidade);

        }
    
    #endregion

    #region "MÉTODOS GERAIS"
        
        /// <summary>
        /// Limpa todos os campos que possuem valores manipuláveis.
        /// </summary>
        public void clearFormulario()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType() == typeof(Panel))
                {
                    //loop nos controles do painel PRINCIPAL
                    if (ctrl.Name.ToString().ToUpper() == "PNLFRMPROPOSTA")
                    {
                        foreach (Control pnFrmCtrl in ctrl.Controls)
                        {
                            //realiza um loop nos controles do painel CENTRAL
                            if (pnFrmCtrl.Name.ToString().ToUpper() == "PNCENTRAL")
                            {
                                foreach (Control pnCentralCtrl in pnFrmCtrl.Controls)
                                {
                                    if (pnCentralCtrl.Tag.ToString() != "" && pnCentralCtrl.Tag.ToString().ToUpper() == "L")
                                    {
                                        pnCentralCtrl.Text = "";
                                    }
                                }

                            }
                            else if (pnFrmCtrl.Tag.ToString() != "" && pnFrmCtrl.Tag.ToString().ToUpper() == "L")
                            {

                                pnFrmCtrl.Text = "";
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Limpa os campos com valores manipuláveis podendo selecionar se quer limpar apenas um dos dois paineis no formulário ou os dois.
        /// </summary>
        /// <param name="boolPnPrincipal">Limpa apenas o painel Principal (TRUE)</param>
        /// <param name="boolPnCentral"> limpa apenas o painel central (TRUE)</param>
        public void clearFormulario(bool boolPnPrincipal, bool boolPnCentral)
        {
            //Entra no painelPrincipal
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType() == typeof(Panel))
                {
                    if (ctrl.Name.ToString().ToUpper() == "PNLFRMPROPOSTA")
                    {
                        foreach (Control pnFrmCtrl in ctrl.Controls)
                        {

                            if (pnFrmCtrl.Name.ToString().ToUpper() == "PNCENTRAL")
                            {   //Entra no painel Central
                                foreach (Control pnCentralCtrl in pnFrmCtrl.Controls)
                                {
                                    if (pnCentralCtrl.Tag.ToString() != "" && pnCentralCtrl.Tag.ToString().ToUpper() == "L" && (boolPnCentral == true))
                                    {
                                        pnCentralCtrl.Text = "";
                                    }
                                }

                            }

                            else if (pnFrmCtrl.Tag.ToString() != "" && pnFrmCtrl.Tag.ToString().ToUpper() == "L" && (boolPnPrincipal == true))
                            {

                                pnFrmCtrl.Text = "";
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Limpar formulário para preencher informações de um próximo item.
        /// </summary>
        public void clearParaProximoItem()
        {
            this.clearFormulario(false, true);
            this.tbProduto.Text = "";
            this.tbLote.Text = "";
            this.tbSequencia.Text = "";
            tbMensagem.Text = "";
        }

        /// <summary>
        /// Realiza todo o processo de liberação para o produto lido
        /// </summary>
        /// <param name="inputText">Valor captado pelo Leitor</param>
        /// <param name="tipoEtiqueta">Tipo de Etiqueta a ser validada</param>
        private void liberarItem(String inputText,Etiqueta.Tipo tipoEtiqueta)
        {
            try
            {
                ProcedimentosLiberacao.liberarItem(inputText, tipoEtiqueta, objProposta, this);
            }
            catch (Exception ex)
            {
                MainConfig.errorMessage(ex.Message, "Liberação!");
            }
        }

        /// <summary>
        /// Realiza os procedimentos nescessários de termino de conferência 
        /// de um item de modo que não seja perdido nenhuma informação útil.
        /// </summary>
        /// <param name="formulario">Interface responsável por abrir um próximo formulário</param>
        /// <param name="retorno">Se o usuário terá ou não um mesnsagem de informativa após a conclusão do método.</param>
        /// <param name="showQuestion">Mostrar ou não a pergunta de confirmação ao usuário.</param>
        /// <returns>resposta do Dialog Result</returns>
        private DialogResult exitForm(ICall formulario, bool retorno, bool showQuestion)
        {
            try
            {
                DialogResult resp;

                //Verifica se irá ou não apresentar a pergunta ao usuário.
                if (showQuestion)
                {
                    resp = MessageBox.Show("Deseja salvar as altereções realizadas", "Exit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    resp = DialogResult.Yes;
                }

                //Modifica o estado do cursor
                Cursor.Current = Cursors.WaitCursor;
                
                //realiza procedimentos nescessário para terminar a conferência do item
                if (resp == DialogResult.Yes)
                {
                    //Classes para trabalhar com as bases de dados.
                    var daoItemProposta = new DaoProdutoProposta();
                    var daoProposta = new DaoProposta();
                    var daoEmbalagem = new DaoEmbalagem();

                    ProcedimentosLiberacao.interromperLiberacao(objProposta);
                    daoProposta.updatePropostaTbPickingMobile(objProposta, Proposta.StatusLiberacao.NAOFINALIZADO, true, true);
                    daoItemProposta.updateItemPropostaRetorno();
                    daoEmbalagem.salvarEmbalagensSeparacao(objProposta);
                    this.Dispose();
                    this.Close();
                }
                else if (resp == DialogResult.No)
                {
                    var daoProposta = new DaoProposta();
                    ProcedimentosLiberacao.interromperLiberacao(objProposta);
                    daoProposta.updatePropostaTbPickingMobile(objProposta, Proposta.StatusLiberacao.NAOFINALIZADO, true, false);
                    this.Dispose();
                    this.Close();
                }
                return resp;
            }
            catch (Exception ex)
            {
                MainConfig.errorMessage("Não foi possível executar o comando solicitado.\n" + ex.Message,"Form Propostas");
                return DialogResult.Yes;
            }
            finally 
            {
                Cursor.Current = Cursors.Default;
                formulario.call();
            }
        }

        /// <summary>
        /// Realiza os procedimentos nescessários de termino de conferência 
        /// de um item de modo que não seja perdido nenhuma informação útil.
        /// </summary>
        /// <param name="showQuestion">Mostrar ou não a pergunta de confirmação ao usuário.</param>
        /// <returns>resposta do Dialog Result</returns>
        private DialogResult exitForm(bool showQuestion)
        {
            try
            {
                DialogResult resp;

                if (showQuestion)
                {
                    resp = MessageBox.Show("Deseja salvar as altereções realizadas", "Exit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    resp = DialogResult.Yes;
                }

                Cursor.Current = Cursors.WaitCursor;

                if (resp == DialogResult.Yes)
                {
                    var daoItemProposta = new DaoProdutoProposta();
                    var daoProposta = new DaoProposta();
                    var daoEmbalagem = new DaoEmbalagem();

                    ProcedimentosLiberacao.interromperLiberacao(objProposta);
                    daoProposta.updatePropostaTbPickingMobile(objProposta, Proposta.StatusLiberacao.NAOFINALIZADO, true, true);
                    daoItemProposta.updateItemPropostaRetorno();
                    daoEmbalagem.salvarEmbalagensSeparacao(objProposta);
                    this.Dispose();
                    this.Close();
                }
                else if (resp == DialogResult.No)
                {
                    var daoProposta = new DaoProposta();
                    ProcedimentosLiberacao.interromperLiberacao(objProposta);
                    daoProposta.updatePropostaTbPickingMobile(objProposta, Proposta.StatusLiberacao.NAOFINALIZADO, true, false);
                    this.Dispose();
                    this.Close();
                }

                return resp;
            }
            catch (Exception ex)
            {
                MainConfig.errorMessage("Não foi possível executar o comando solicitado.\n" + ex.Message, "Form Propostas");
                return DialogResult.Yes;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// método para fechar o form durante execptions
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="headForm"></param>
        public  void exitOnError(String mensagem, String headForm,bool showQuestion)
        {   
            //MOSTRA MENSAGEM DE ERRO AO USUÁRIO
            MainConfig.errorMessage(mensagem, headForm);
            Cursor.Current = Cursors.Default;
            //FINALIZA O PROCESSO E ABRE O FORM DE AÇÃO NOVAMENTE.
            ICall form = new FrmAcao(true);
            this.exitForm(form, false, showQuestion);
            //ENCERRA O FORM.
            this.Dispose();
            this.Close();
        }

        public static void mostrarMensagem(enumCor corMensagem,String mensagem,enumCursor modoCursor) 
        {
            Cursor.Current = estadoCursor[(int)modoCursor];
            tbMensagem.ForeColor = cores[(int)corMensagem];
            tbMensagem.Text = mensagem;
        }

        public bool temItensConferir() 
        {
            return ProcedimentosLiberacao.TotalItens > 0;
        }

    #endregion

    #region "Realocados para a classe ProcedimentosLiberacao"

        ////#region "GET E SET"

        ////    public List<String> ListInformacoesProposta
        ////    {
        ////        get { return ListInformacoesProposta; }
        ////        set { ListInformacoesProposta = value; }
        ////    }

        ////#endregion

        ///// <summary>
        ///// Finaliza o processo de conferência de uma proposta como um todo.
        ///// </summary>
        //private void finalizarProposta()
        //{
        //    try
        //    {
        //        mostrarMensagem(enumCor.BLUE, "Gravando informações na base de dados!", enumCursor.WAIT);
        //        daoItemProposta = new DaoProdutoProposta();
        //        daoProposta = new DaoProposta();
        //        daoEmbalagem = new DaoEmbalagem();

        //        daoEmbalagem.salvarEmbalagensSeparacao(objProposta);
        //        daoProposta.updatePropostaTbPickingMobile(objProposta, Proposta.StatusLiberacao.FINALIZADO, true, true);
        //        daoItemProposta.updateItemPropostaRetorno();
        //        daoProposta.updateVolumeProposta(objProposta.Codigo);
        //        daoProposta.retiraPropostaListaPrioridade(objProposta.Codigo, MainConfig.UserOn.Codigo);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("finalizarProposta()\n " + ex.Message);
        //    }
        //    finally
        //    {
        //        FrmAcao frm = new FrmAcao();
        //        mostrarMensagem(enumCor.RED, "", enumCursor.DEFAULT);
        //        daoItemProposta = null;
        //        daoProposta = null;
        //        this.Dispose();
        //        this.Close();
        //        frm.Show();
        //    }
        //}

        ///// <summary>
        ///// Realiza todos os procedimentos nescessários para carregar o próximo item a ser separado.
        ///// </summary>
        ///// 
        ///// <returns>
        /////          TRUE --> caso exista um próximo item a ser trabalhado
        /////          FALSE --> caso não exista mais items para serem trabalhados.
        ///// </returns>
        //private bool nextItemProposta()
        //{
        //    bool hasItem = false;
        //    daoItemProposta = new DaoProdutoProposta();
        //    daoEtiqueta = new DaoEtiqueta();
        //    objTransacoes = new BaseMobile();

        //    try
        //    {
        //        this.clearParaProximoItem();

        //        ProcedimentosLiberacao.tratarParaProximoItem(objProposta);
        //        //grava informações do item na base de dados mobile
        //        daoItemProposta.updateStatusItemProposta(objProposta.ListObjItemProposta[0]);
        //        //inseri informações das etiquetas referente ao produto liberado em formato Xml
        //        daoItemProposta.updateXmlItemProposta(ProcedimentosLiberacao.montarXmlProcedimento(), objProposta.ListObjItemProposta[0].CodigoItemProposta);

        //        //carrega próximo item
        //        if (temItensConferir())
        //        {
        //            var prod = daoItemProposta.itemATrabalhar();

        //            if (prod != null)
        //            {
        //                //Carrega informações de Embalagem para o produto que será trabalhado.
        //                prod.Embalagens = daoEmbalagem.carregarEmbalagensProduto(prod);

        //                hasItem = true;

        //                objProposta.setNextItemProposta(prod);
        //            }
        //            else
        //            {
        //                hasItem = false;
        //            }
        //        }
        //        else // CASO não tenha um próximo item 
        //        {
        //            hasItem = false;
        //        }

        //        //Se existir um próximo item
        //        if (hasItem)
        //        {
        //            //seta Parametros para iniciar leitura do próximo item
        //            ProcedimentosLiberacao.inicializarProcedimentosProximoItem(objProposta.ListObjItemProposta[0].Quantidade);

        //            //recarrega o form com as informações do próximo item.
        //            this.fillCamposForm(objProposta, ProcedimentosLiberacao.TotalPecas, ProcedimentosLiberacao.TotalItens);
        //        }
        //        else
        //        {
        //            this.clearFormulario(true, true);
        //        }
        //    }
        //    catch (SqlCeException Ex)
        //    {
        //        MessageBox.Show(Ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao carregar próximo item!", ex);
        //    }
        //    finally
        //    {
        //        daoEtiqueta = null;
        //        daoItemProposta = null;
        //    }

        //    return hasItem;
        //}

        ///// <summary>
        ///// Reliza todos os processos nescessários para efetuar a carga de dados na base Mobile.
        ///// </summary>
        //private void carregaBaseMobile()
        //{

        //    objTransacoes = new BaseMobile();
        //    objProposta = new Proposta();
        //    daoItemProposta = new DaoProdutoProposta();
        //    daoProposta = new DaoProposta();
        //    daoProduto = new DaoProduto();
        //    daoEmbalagem = new DaoEmbalagem();

        //    //LIMPA INFORMAÇÕES RESULTANTE DE OUTROS PRODUTOS JÁ CONFERIDOS
        //    ProcedimentosLiberacao.limpar();

        //    try
        //    {
        //        //Limpa a Base.
        //        objTransacoes.clearBaseMobile();

        //        //Carrega um objeto Proposta e inicia todo o procedimento.
        //        //Caso não exista propostas a serem liberadas gera um exception 
        //        //onde será feito os tratamentos para evitar o travamento do sistema.
        //        if ((objProposta = daoProposta.fillTop1PropostaServidor()) != null)
        //        {
        //            daoProposta.InsertOrUpdatePickingMobile(objProposta, MainConfig.UserOn.Codigo, Proposta.StatusLiberacao.TRABALHANDO, DateTime.Now);

        //            //recupera o codigoPickingMobile da proposta trabalhada.
        //            objProposta.CodigoPikingMobile = daoProposta.selectMaxCodigoPickingMobile(objProposta.Codigo);

        //            //Realiza o Insert na Base Mobile
        //            daoProposta.insertProposta(objProposta, MainConfig.UserOn.Codigo);

        //            //Recupera List com itens da proposta
        //            //Insert na Base Mobile tabela tb0002_ItensProposta
        //            daoItemProposta.carregarBaseMobileItens(daoItemProposta.fillListItensProposta((int)objProposta.Codigo).ToList<ProdutoProposta>());

        //            //Insert na base Mobile tabela tb0003_Produtos
        //            //Recupera informações sobre os produtos existentes na proposta
        //            daoProduto.insertProdutoBaseMobile(daoProduto.fillListProduto((int)objProposta.Codigo).ToList<Produto>());

        //            //Armazena informações de embalagens do produto na base mobile.
        //            daoEmbalagem.insertEmbalagemBaseMobile(daoEmbalagem.cargaEmbalagensProduto((int)objProposta.Codigo));

        //            //Carrega Informações das Embalagens de Separação.
        //            //Carrega Quantidade das Embalagens utilizadas nos volumes da separação
        //            ProcedimentosLiberacao.ListEmbalagensSeparacao = daoEmbalagem.carregarInformacoesEmbalagensUtilizadas((Int32)objProposta.CodigoPikingMobile, daoEmbalagem.carregarEmbalagensSeparacao());

        //        }
        //        else
        //        {
        //            throw new NoNewPropostaException("Não existem novas propostas a serem liberadas!!");
        //        }
        //    }
        //    catch (SqlQueryExceptions ex)
        //    {
        //        this.exitOnError(ex.Message, "Próxima Proposta", false);
        //    }
        //    catch (NoNewPropostaException ex)
        //    {
        //        this.exitOnError(ex.Message, "Próxima Proposta", false);
        //    }
        //    catch (SqlCeException sqlEx)
        //    {
        //        ProcedimentosLiberacao.interromperLiberacao(objProposta);
        //        daoProposta.updatePropostaTbPickingMobile(objProposta, Proposta.StatusLiberacao.NAOFINALIZADO, true, false);
        //        StringBuilder strBuilder = new StringBuilder();
        //        strBuilder.Append("O procedimento não pode ser concluído.\n");
        //        strBuilder.AppendFormat("Erro : {0}", sqlEx.Errors);
        //        strBuilder.AppendFormat("Description : {0}", sqlEx.Message);
        //        this.exitOnError(strBuilder.ToString(), "SqlException!!", false);
        //    }
        //    catch (Exception ex)
        //    {
        //        ProcedimentosLiberacao.interromperLiberacao(objProposta);
        //        daoProposta.updatePropostaTbPickingMobile(objProposta, Proposta.StatusLiberacao.NAOFINALIZADO, true, false);
        //        StringBuilder strBuilder = new StringBuilder();
        //        strBuilder.Append("O procedimento não pode ser concluído.\n");
        //        strBuilder.AppendFormat(" Descrição: {0}", ex.Message);
        //        strBuilder.Append("\nContate o Administrador do sistema.");
        //        this.exitOnError(strBuilder.ToString(), "SqlException!!", false);
        //    }
        //    finally
        //    {
        //        objTransacoes = null;
        //        objProposta = null;
        //        daoProposta = null;
        //        daoProduto = null;
        //        daoItemProposta = null;
        //        daoEmbalagem = null;
        //    }

        //}

        #endregion

    }

}