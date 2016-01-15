using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Utility;
using TitaniumColector.Classes.Model;
using TitaniumColector.Forms;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace TitaniumColector.Classes.Procedimentos
{
    class ProcedimentosAlocacao : IDisposable
    {
        public List<Etiqueta> ListEtiquetas { get; set; }
        public  List<Etiqueta> ListEtiquetasAlocadas { get; set; }
        public List<String> ListXmlProcedimento { get; set; }
        public Form FormPrincipal { get; set; }
        public int TotalItensAlocados { get { return this.ListEtiquetasAlocadas.Count; } }
        public int TotalItens { get { return this.ListEtiquetas.Count; } }
        private Etiqueta etiquetaProduto;
        private Array inputStringToEtiqueta;
        
    #region "SingleTon"

        private static ProcedimentosAlocacao instancia;

        private ProcedimentosAlocacao() 
        {
            ListEtiquetas = new List<Etiqueta>();
            ListEtiquetasAlocadas = new List<Etiqueta>();
            ListXmlProcedimento = new List<string>();
        }

        public static ProcedimentosAlocacao Instanciar
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ProcedimentosAlocacao();
                }
                return instancia;
            }
        }

     #endregion

        public void clear() 
        {
            this.ListEtiquetas = new List<Etiqueta>();
            this.ListXmlProcedimento = new List<string>();
            this.ListEtiquetasAlocadas = new List<Etiqueta>();
            this.etiquetaProduto = null;
            this.inputStringToEtiqueta = null;
            this.FormPrincipal = null;
        }

        /// <summary>
        /// Define o tipo de ação a ser realizada após a leitura da etiqueta.
        /// </summary>
        /// <param name="etiqueta">Etiqueta lida</param>
        private void trabalhaEtiqueta(Etiqueta etiqueta)
        {

            try
            {
                if (etiquetaJaAlocada(etiqueta))
                {
                    throw new InvalidOperationException();
                }

                if (!etiquetaJaValidada(etiqueta))
                {
                    this.ListEtiquetas.Add(etiqueta);
                }
                else
                {
                    if (!etiquetaJaAlocada(etiqueta))
                    {
                        this.FormPrincipal.Enabled = false;
                        FrmInputAlocacao frmInput = new FrmInputAlocacao((EtiquetaAlocacao)etiqueta, this.FormPrincipal);
                        frmInput.Show();
                    }
                    else
                    {
                        throw new InvalidOperationException();  
                    }
                }
            }
            catch( InvalidOperationException)
            {
                mostrarMensagem("Volume já alocado!", "Guardar Volumes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception)
            {   
                throw;
            }
            
        }

        /// <summary>
        /// Verifica se a etiqueta já existe na lista de Etiquetas
        /// </summary>
        /// <param name="etiqueta">Etiqueta a ser validada</param>
        /// <returns>true se ela existe.</returns>
        private bool etiquetaJaValidada(Etiqueta etiqueta)
        {
            foreach (var item in ListEtiquetas )
            {
                if (item.Equals(etiqueta))
                {
                    return true;
                }
            }
            return false;
        }

        private bool etiquetaJaAlocada(Etiqueta etiqueta)
        {
            foreach (var item in ListEtiquetasAlocadas)
            {
                if (item.Equals(etiqueta))
                {
                    return true;
                }
            }
            return false;
        }

        private void ordenarList(List<EtiquetaAlocacao> list)
        {
            list.Sort();
        }

        /// <summary>
        /// Monta o Xml referente a cada item do pedido  o mesmo será gravado na tabela tb1403_Itens_Pedidos ao seu referido item
        /// </summary>
        private void montarXmlProcedimento()
        {
            var trabalhadoItem = 0;
            this.ListXmlProcedimento = new List<string>();

            foreach (EtiquetaAlocacao item in ListEtiquetasAlocadas)
            {
                if (trabalhadoItem != item.CodigoItemAlocacao)
                {
                    trabalhadoItem = item.CodigoItemAlocacao;

                    //apenas realizo um cast para List<EtiquetaAlocacao>
                    List<EtiquetaAlocacao> list = ListEtiquetasAlocadas.Cast<EtiquetaAlocacao>().ToList<EtiquetaAlocacao>();


                    //Seleciono apenas os itens que possuem o CODIGO DO ITEM a ser trabalhado
                    var itemSelecionado = from c in list
                                          where c.CodigoItemAlocacao == trabalhadoItem
                                          orderby c.VolumeItemAlocacao
                                          select c;

                    //MONTAGEM XML
                    //Inicia o processo para montagem da string de Xml do Item do pedido
                    System.IO.StringWriter str = new System.IO.StringWriter();
                    //Variável que irá receber o Xml na forma de String.
                    XmlTextWriter writer = new XmlTextWriter(str);
                    //inicia o documento xml
                    writer.WriteStartDocument();
                    //define a indentação do arquivo
                    writer.Formatting = Formatting.Indented;
                    //

                    var count = 0;
                    //trabalho as informações nescessárias para montar  a string  Xml;
                    foreach (var itens in itemSelecionado)
                    {
                        if (count == 0)
                        {
                            //Escreve o elemento raiz
                            writer.WriteStartElement("ItemPedido");
                            //Escrever o atributo para o Elemento Raiz CodigoItemPedido
                            writer.WriteAttributeString("ID", itens.CodigoItemAlocacao.ToString());

                            //Escreve o elemento raiz Produto Pedido
                            writer.WriteStartElement("prodPedido");
                            //escrever o atributo para o Elemento Raiz Produto Pedido
                            writer.WriteAttributeString("ID", itens.CodigoProduto.ToString());
                            writer.WriteAttributeString("DESCRICAO", itens.DescricaoCompletaProduto.ToString());

                            //Escreve o elemento raiz Usuario Liberacao
                            writer.WriteStartElement("idUsuarioLiberacao");
                            //escrever o atributo para o Elemento Usuario Liberacao
                            writer.WriteAttributeString("ID", itens.UsuarioAlocacao.Codigo.ToString());

                            //Escreve Elemento Raiz Informações 
                            writer.WriteStartElement("Inf");
                        }

                        //Escreve o elemento raiz Volumes
                        writer.WriteStartElement("volume");
                        //Escreve atributos Do Elelmento Volume
                        writer.WriteAttributeString("ID", itens.VolumeItemAlocacao.ToString());

                        //Escreve elementos entre a tag inf
                        //informações do item do pedido
                        writer.WriteElementString("idLote", itens.CodigoLote.ToString());
                        writer.WriteElementString("descLote", itens.LoteEtiqueta.ToString());
                        writer.WriteElementString("idLocalAlocacao", itens.CodigoLocalAlocacao.ToString());
                        writer.WriteElementString("descLocalAlocacao", itens.LocalAlocacao.ToString());
                        writer.WriteElementString("datahoraLiberacao", itens.DataHoraValidacao.ToString());

                        //Encerra o elemento Volumes
                        writer.WriteEndElement();

                        count++;
                    }

                    //Encerra o elemento Inf
                    writer.WriteEndElement();

                    //Encerra o elemento Item
                    writer.WriteEndDocument();

                    this.ListXmlProcedimento.Add(str.ToString().Replace("\r\n", "").Remove(0, 39));
                }
            }
        }

        /// <summary>
        /// Insert na Base Engine as informações de Lote e Local Lote de cada Volume.
        /// </summary>
        private void persistirLocalLote()
        {
            using(var dao = new TitaniumColector.Classes.Dao.DaoEtiqueta())
            {
                dao.finalizarAlocacao(this.ListEtiquetasAlocadas.Cast<EtiquetaAlocacao>().ToList<EtiquetaAlocacao>());
            }

        }

        private int recuperaCodigoItemEmXml(string strXml)
        {
            if (strXml.Contains("ItemPedido ID"))
            {
                var startIndex = strXml.IndexOf("ItemPedido ID=\"") + ("ItemPedido ID=\"").Length;
                var lenStr = strXml.IndexOf("\">", startIndex) - 16;
                return Convert.ToInt32(strXml.Substring(startIndex, lenStr));
            }

            return -1;
        }

        private void gravarXmlItemPedido()
        {
            foreach (var item in this.ListXmlProcedimento)
            {
                var codigo = this.recuperaCodigoItemEmXml(item);
                if (codigo > 0)
                {
                    using (var dao = new TitaniumColector.Classes.Dao.DaoEtiqueta())
                    {
                        dao.gravarXmlItemPedido(codigo, item);
                    }
                }
            }
        }

        public void alocarProduto(EtiquetaAlocacao etiquetaAlocar)
        {
            if (etiquetaAlocar.JaAlocado)
            {
                if (this.etiquetaJaValidada(etiquetaAlocar))
                {
                    etiquetaAlocar.montarXml();
                    this.ListEtiquetasAlocadas.Add(etiquetaAlocar);
                    this.ListEtiquetas.Remove(etiquetaAlocar);
                }
            }
        }

        public void atualizarListView()
        {
            FrmAlocacao.carregarListVolumes();
        }

        /// <summary>
        /// Mostra uma mensagem ao usuario,(retorna um dialogResult dependente o tipo de Buttons definido com parâmetro)
        /// </summary>
        /// <param name="mensagem">Mensagem ao usuário</param>
        /// <param name="caption">Cabeçalho</param>
        /// <param name="msgButton">Buttons</param>
        /// <param name="msgIcon"> Icone da caixa de texto.</param>
        /// <returns></returns>
        public static DialogResult mostrarMensagem(string mensagem, string caption, MessageBoxButtons msgButton, MessageBoxIcon msgIcon)
        {
            return MessageBox.Show(mensagem, caption, msgButton, msgIcon, MessageBoxDefaultButton.Button2);
        }

        /// <summary>
        /// Ações para finalizar o processo para os itens já alocados.
        /// </summary>
        public void finalizarProcesso()
        {
            try
            {
                if (!temItensFinalizar())
                {
                    throw new InvalidOperationException();
                }

                Cursor.Current = Cursors.WaitCursor;
                this.montarXmlProcedimento();
                this.persistirLocalLote();
                this.gravarXmlItemPedido();
                this.limparItensAlocados();
                this.atualizarListView();
                Cursor.Current = Cursors.Default;
            }
            catch (InvalidOperationException)
            {
                mostrarMensagem("Não existe itens a serem finalizados!", "Guarda de Volumes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception e) 
            {
                throw e;
            }
            
        }

        /// <summary>
        /// Verifica se a lista de etiquetas alocados possue itens
        /// </summary>
        /// <returns>true se sim</returns>
        public bool temItensFinalizar()
        {
            return this.ListEtiquetasAlocadas.Count > 0;
        }

        /// <summary>
        /// Verifica se a lista de etiquetas lidas possue itens.
        /// </summary>
        /// <returns>true se sim</returns>
        public bool temItensConferidos()
        {
            return this.ListEtiquetas.Count > 0;
        }

        public void limparItensAlocados()
        {
            if (this.temItensFinalizar()) 
            {
                this.ListEtiquetasAlocadas.Clear();
                this.ListXmlProcedimento.Clear();
            }
        }

        /// <summary>
        /// Define que ação será feita após a leitura da etiqueta.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="tipoEtiqueta"></param>
        public void realizarAcao(string inputText, Etiqueta.TipoCode tipoEtiqueta)
        {
            try
            {
                switch (tipoEtiqueta)
                {
                    case Etiqueta.TipoCode.INVALID:

                        inputText = string.Empty;
                        mostrarMensagem(" Tipo de Etiqueta inválida!!!", "Guardar Volumes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;

                    case Etiqueta.TipoCode.QRCODE:

                        //MONTA UM ARRAY DE STRING COM AS INFORMACOES PASSADAS NO INPUTTEXT
                        inputStringToEtiqueta = FileUtility.arrayOfTextFile(inputText, FileUtility.splitType.PIPE);
                        //GERA UM OBJETO ETIQUETA DO TIPO QUE FOI PASSADO NO PRIMEIRO PÂRAMETRO 
                        etiquetaProduto = Leitor.gerarEtiqueta(new EtiquetaAlocacao(), inputStringToEtiqueta, tipoEtiqueta);
                        //VALIDA A INCLUSÃO OU ALOCAÇÃO DA ETIQUETA;
                        this.trabalhaEtiqueta(etiquetaProduto);
                        inputText = string.Empty;
                        break;

                    default:

                        inputText = string.Empty;
                        mostrarMensagem("Não é possível validar a etiqueta lida!!!", "Guardar Volumes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }

            }
            catch (Exception)
            {
                throw;
            }
           
        }

    #region "Idisposable"

        private Stream _resource;
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these 
            // operations, as well as in your methods that use the resource.
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_resource != null)
                        _resource.Dispose();
                }

                // Indicate that the instance has been disposed.
                _resource = null;
                _disposed = true;
            }
        }

    #endregion

    }
}
