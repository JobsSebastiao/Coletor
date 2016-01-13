using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Classes.Model;
using TitaniumColector.Classes;
using TitaniumColector.Classes.Procedimentos;
using System.Globalization;
using TitaniumColector.Classes.Utility;

namespace TitaniumColector.Forms
{
    public partial class FrmAlocacao : Form
    {
        private string inputText;
        private ProcedimentosAlocacao procedimentos;
        private static int totalVolumes;
        private static int totalVolumesAlocados;

        public FrmAlocacao()
        {
            InitializeComponent();
            this.inicializarForm();
        }

        void FrmAlocacao_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try 
            {

                if (e.KeyChar == Convert.ToChar(13))
                {
                    if (inputText != "" && inputText != null)
                    {
                        //CRIAR OU RECUPERA A INSTANCIA PARA UM OBJETO PROCEDIMENTALOCACAO (USO DE SINGLETON)
                        procedimentos = ProcedimentosAlocacao.Instanciar;
                        procedimentos.FormPrincipal = this;
                        //VALIDA O TIPO DE ETIQUETA
                        Etiqueta.Tipo tipoEtiqueta = Leitor.validaInputValueEtiqueta(inputText, new EtiquetaAlocacao());
                        //INICIA O PROCEDIMENTO
                        procedimentos.realizarAcao(inputText, tipoEtiqueta);
                        procedimentos.atualizarListView();
                        //carregarListVolumes(procedimentos.ListEtiquetas.Cast<EtiquetaAlocacao>().ToList<EtiquetaAlocacao>());
                        inputText = "";
                    }
                }
                else
                {
                    inputText += e.KeyChar.ToString();
                }

            }catch(Exception ex)
            {
                MessageBox.Show("Problemas durante a leitura da Etiqueta!!\n"+ ex, "Processos de Guarda", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
            }           
        }

        private void btnSair_Click(object sender, System.EventArgs e)
        {
            var podeSair = false;

            using(procedimentos = ProcedimentosAlocacao.Instanciar)
            {
                //Verifica se o processo possui itens já alocados
                if (procedimentos.temItensFinalizar())
                {
                    var result = ProcedimentosAlocacao.mostrarMensagem("Existem itens já alocados, o processo de Alocação deve ser finalizado!", "Finalizar Processo", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                    if (result == DialogResult.OK)
                    {
                        this.btnFinalizar_Click(this, e);
                    }
                    else
                    {
                        this.nextControl(false);
                        return;
                    }

                }
                else 
                {
                    //verifica se existem itens conferidos na list
                    if (!procedimentos.temItensConferidos())
                    {
                        podeSair = true;
                    }
                    else 
                    {
                        var result = ProcedimentosAlocacao.mostrarMensagem("O processo possui itens conferidos,caso deixe o processo os dados serão perdido!\nDeseja realmente sair?","Sair",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                        
                        if (result == DialogResult.Yes)
                        {
                            podeSair = true;
                        }
                    }

                    if (podeSair)
                    {
                        procedimentos.clear();
                        FrmAlocacao.TotalVolumes = 0;
                        FrmAlocacao.TotalVolumesAlocados = 0;
                        this.Close();
                        this.Dispose(true);
                        FrmAcao frm = new FrmAcao();
                        frm.Show();
                    }
                    else 
                    {
                        this.nextControl(false);
                    }
                }
            }
           
        }

        private void btnFinalizar_Click(object sender,System.EventArgs e) 
        {
            //CRIAR OU RECUPERA A INSTANCIA PARA UM OBJETO PROCEDIMENTALOCACAO (USO DE SINGLETON)
            procedimentos = ProcedimentosAlocacao.Instanciar;

            if (procedimentos.temItensFinalizar())
            {
                var resul = ProcedimentosAlocacao.mostrarMensagem("Deseja finalizar o processo de Alocação para os itens alocados?", "Guardar de Volumes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if(resul == DialogResult.Yes)
                {
                    procedimentos.finalizarProcesso();
                }
            }
            else 
            {
                ProcedimentosAlocacao.mostrarMensagem("Não existem itens alocados para o processo!", "Guarda de Volumes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            this.nextControl(false);

        }

        /// <summary>
        /// Carrega a listView de etiquetas lidas
        /// </summary>
        public static void carregarListVolumes()
        {
            try
            {
                listVolumes.Items.Clear();

                //Ordena baseado no metodo CompareTo da classe 
                ProcedimentosAlocacao.Instanciar.ListEtiquetas.Sort();
                //carrega o listview com as informações carregadas do banco de dados.
                foreach (var item in ProcedimentosAlocacao.Instanciar.ListEtiquetas.Cast<EtiquetaAlocacao>().ToList<EtiquetaAlocacao>())
                {
                    CultureInfo ptBr = CultureInfo.CreateSpecificCulture("pt-BR");
                    listItem = new System.Windows.Forms.ListViewItem();
                    listItem.Tag = item.CodigoItemAlocacao.ToString();
                    listItem.Text = item.CodigoItemAlocacao.ToString(); item.LocaisLote.ToString();
                    listItem.SubItems.Add(item.LocaisLote.ToString());
                    listItem.SubItems.Add(item.DescricaoCompletaProduto.ToString());
                    listItem.SubItems.Add(item.VolumeItemAlocacao.ToString());
                    listItem.SubItems.Add(item.LoteEtiqueta.ToString());
                    listVolumes.Items.Add(listItem);
                }
                txtTotal.Text = TotalVolumes.ToString(" 0000", CultureInfo.CreateSpecificCulture("pt-BR"));
                lbQtdTotalAlocado.Text = TotalVolumesAlocados.ToString(" 000", CultureInfo.CreateSpecificCulture("pt-BR"));
            }
            catch (Exception)
            {
                throw new Exception("Problemas durante a atualização do formulário!\n MÉTODO : carregarListVolumes() ");
            }
            
        }

        public static void carregarListVolumes(List<EtiquetaAlocacao> listEtiquetas)
        {
            listVolumes.Items.Clear();
            //Ordena baseado no metodo CompareTo da classe 
            listEtiquetas.Sort();
            //carrega o listview com as informações carregadas do banco de dados.
            foreach (var item in listEtiquetas)
            {
                CultureInfo ptBr = CultureInfo.CreateSpecificCulture("pt-BR");
                //string peso = string.Format(item.Peso.ToString("00.000", ptBr));

                listItem = new System.Windows.Forms.ListViewItem();
                listItem.Tag = item.CodigoItemAlocacao.ToString();
                listItem.Text = item.CodigoItemAlocacao.ToString();  item.LocaisLote.ToString();
                listItem.SubItems.Add(item.LocaisLote.ToString());
                listItem.SubItems.Add(item.DescricaoCompletaProduto.ToString());
                listItem.SubItems.Add(item.VolumeItemAlocacao.ToString());
                listItem.SubItems.Add(item.LoteEtiqueta.ToString());
                listVolumes.Items.Add(listItem);
            }

            txtTotal.Text = TotalVolumes.ToString(" 0000", CultureInfo.CreateSpecificCulture("pt-BR"));
            lbQtdTotalAlocado.Text = TotalVolumesAlocados.ToString(" 000",CultureInfo.CreateSpecificCulture("pt-BR"));
        }

        public static int TotalVolumes
        {
            get { return listVolumes.Items.Count; }
            private set { totalVolumes = value; }
        }

        public static int TotalVolumesAlocados
        {
            get { return ProcedimentosAlocacao.Instanciar.ListEtiquetasAlocadas.Count; }
            private set { totalVolumesAlocados = value; }
        }

    #region "Tira Focus Controle"

        void txtCabecalho_GotFocus(object sender, System.EventArgs e)
        {
            this.nextControl();
        }

        void txtTotal_GotFocus(object sender, System.EventArgs e)
        {
            this.nextControl();
        }

        void txtDescricaoTotal_GotFocus(object sender, System.EventArgs e)
        {
            this.nextControl();
        }

        /// <summary>
        /// Passo o foco para o próximo controle que possui TabStop == true
        /// </summary>
        private void nextControl()
        {
            this.SelectNextControl(this, true, true, true, false);
        }

        /// <summary>
        /// Passo o foco para o próximo controle que possui TabStop == true
        /// </summary>
        /// <param name="proximo">Define de irá para o proximo controle ou ao controle anterior</param>
        private void nextControl(bool proximo) 
        {
            this.SelectNextControl(this, proximo, true,true,false);
        }

    #endregion 
    }
}