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

                        this.carregarListEmbalagens(procedimentos.listEtiquetas.Cast<EtiquetaAlocacao>().ToList<EtiquetaAlocacao>());
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
            this.Close();
            this.Dispose(true);
            FrmAcao frm = new FrmAcao();
            frm.Show();
        }

        /// <summary>
        /// Carrega a listView de etiquetas lidas
        /// </summary>
        private void carregarListEmbalagens(List<EtiquetaAlocacao> listEtiquetas)
        {
            this.listVolumes.Items.Clear();
            //Ordena baseado no metodo CompareTo da classe 
            listEtiquetas.Sort();
            //carrega o listview com as informações carregadas do banco de dados.
            foreach (var item in listEtiquetas)
            {
                CultureInfo ptBr = CultureInfo.CreateSpecificCulture("pt-BR");
                //string peso = string.Format(item.Peso.ToString("00.000", ptBr));

                this.listItem = new System.Windows.Forms.ListViewItem();
                this.listItem.Tag = item.CodigoItemAlocacao.ToString();
                this.listItem.Text = item.CodigoItemAlocacao.ToString();  item.LocaisLote.ToString();
                this.listItem.SubItems.Add(item.LocaisLote.ToString());
                this.listItem.SubItems.Add(item.DescricaoCompletaProduto.ToString());
                this.listItem.SubItems.Add(item.VolumeItemAlocacao.ToString());
                this.listItem.SubItems.Add(item.LoteEtiqueta.ToString());
                this.listVolumes.Items.Add(this.listItem);
            }
        }
    }
}