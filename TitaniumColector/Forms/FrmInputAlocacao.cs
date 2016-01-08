using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Classes;
using TitaniumColector.Classes.Model;

namespace TitaniumColector.Forms
{
    public partial class FrmInputAlocacao : Form
    {
        //PROPRIEDADES
        public EtiquetaAlocacao EtiquetaAlocar { get; set; }
        public Form FormChamador { get; set; }
        private string InputText{get;set;}
        private Array arrlocalAlocacao { get; set; }
        private string LocalEtiqueta { get; set; }
        private int CodigoLocalEtiqueta { get; set; }

        public FrmInputAlocacao() 
        {
            InitializeComponent();
            this.configLayout();
        }

        public FrmInputAlocacao(EtiquetaAlocacao etiqueta,Form formChamador)
        {
            InitializeComponent();
            this.configLayout();
            this.EtiquetaAlocar = etiqueta;
            this.FormChamador = formChamador;
            this.preencherForm();
        }

        public Etiqueta validarLocal(EtiquetaAlocacao etiqueta)
        {
            this.EtiquetaAlocar = (EtiquetaAlocacao)etiqueta;
            preencherForm();
            return this.EtiquetaAlocar;
        }

        void FrmInputAlocacao_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.FormChamador.Enabled = true;
            this.FormChamador.Show();
        }

        void FrmInputAlocacao_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {

                if (e.KeyChar == Convert.ToChar(13))
                {
                    if (this.InputText != "" && this.InputText != null)
                    {
                        if (validaInputValue(this.InputText))
                        {
                            this.defineLocal(
                                TitaniumColector.Utility.FileUtility.arrayOfTextFile(InputText
                                , TitaniumColector.Utility.FileUtility.splitType.PIPE)
                                );

                            this.preencherTbLocal();
                            definirLocalEtiqueta();
                            this.btOK_Click(sender, new EventArgs());
                            this.InputText = "";
                        }
                        else
                        {
                            this.InputText = "";
                            MessageBox.Show("Local Inválido!");
                        }
                    }
                }
                else
                {
                    InputText += e.KeyChar.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problemas durante a leitura da Etiqueta!!\n" + ex, "Processos de Guarda", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void preencherForm()
        {
            this.lbDescricaoProduto.Text = this.EtiquetaAlocar.DescricaoCompletaProduto.ToString();
            this.lbDescricaoLote.Text = this.EtiquetaAlocar.LoteEtiqueta.ToString();
            this.tbLocalSugerido.Text = this.EtiquetaAlocar.LocaisLote.ToString();
            this.tbLocalAlocacao.Text = "     ";
            this.tbLocalAlocacao.Enabled = false;
        }

        private bool validaInputValue(string inputText) 
        {
            if (inputText.StartsWith("CODIGO:"))
            {
                if (inputText.Contains("LOCAL:"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void defineLocal(Array arrayEtiqueta) 
        {
            foreach (string item in arrayEtiqueta)
            {
                string strItem = item.Substring(0, item.IndexOf(":", 0));

                if (strItem == "CODIGO")
                {
                    this.CodigoLocalEtiqueta = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
                }
                else if (strItem == "LOCAL")
                {
                    this.LocalEtiqueta = (item.Substring(item.IndexOf(":", 0) + 1)).ToString() ;
                }
            }
        }

        private void preencherTbLocal() 
        {
            this.tbLocalAlocacao.Text = String.Format("   {0}",this.LocalEtiqueta.ToString());
        }

        private void definirLocalEtiqueta() 
        {
            this.EtiquetaAlocar.CodigoLocalAlocacao = this.CodigoLocalEtiqueta;
            this.EtiquetaAlocar.LocalAlocacao = this.LocalEtiqueta;
            this.EtiquetaAlocar.JaAlocado = true;
            this.EtiquetaAlocar.UsuarioAlocacao = MainConfig.UserOn;
            this.EtiquetaAlocar.DataHoraValidacao = DateTime.Now;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.FrmInputAlocacao_Closing(sender, new CancelEventArgs());
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            using (TitaniumColector.Classes.Procedimentos.ProcedimentosAlocacao procedimentos = TitaniumColector.Classes.Procedimentos.ProcedimentosAlocacao.Instanciar) 
            {
                procedimentos.alocarProduto(this.EtiquetaAlocar);
                procedimentos.atualizarListView();
            }
        }

    }
}