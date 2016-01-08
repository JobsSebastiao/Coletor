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
                        carregarListEmbalagens(procedimentos.ListEtiquetas.Cast<EtiquetaAlocacao>().ToList<EtiquetaAlocacao>());
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
            procedimentos.clear();
            FrmAlocacao.TotalVolumes = 0;
            FrmAlocacao.TotalVolumesAlocados = 0;
            this.Close();
            this.Dispose(true);
            FrmAcao frm = new FrmAcao();
            frm.Show();
        }

        private void btnFinalizar_Click(object sender,System.EventArgs e) 
        {
            //CRIAR OU RECUPERA A INSTANCIA PARA UM OBJETO PROCEDIMENTALOCACAO (USO DE SINGLETON)
            procedimentos = ProcedimentosAlocacao.Instanciar;
            procedimentos.FormPrincipal = this;
            procedimentos.montarXmlProcedimento();
        }

        /// <summary>
        /// Carrega a listView de etiquetas lidas
        /// </summary>
        public static void carregarListEmbalagens()
        {
            listVolumes.Items.Clear();
            //Ordena baseado no metodo CompareTo da classe 
            ProcedimentosAlocacao.Instanciar.ListEtiquetas.Sort();
            //carrega o listview com as informações carregadas do banco de dados.
            foreach (var item in ProcedimentosAlocacao.Instanciar.ListEtiquetas.Cast<EtiquetaAlocacao>().ToList<EtiquetaAlocacao>())
            {
                CultureInfo ptBr = CultureInfo.CreateSpecificCulture("pt-BR");
                //string peso = string.Format(item.Peso.ToString("00.000", ptBr));

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

        public static void carregarListEmbalagens(List<EtiquetaAlocacao> listEtiquetas)
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


    }
}