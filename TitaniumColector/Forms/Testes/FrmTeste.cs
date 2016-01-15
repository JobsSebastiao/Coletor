using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Classes.Model;
using TitaniumColector.Classes.Procedimentos;
using TitaniumColector.Forms;

namespace TitaniumColector.Classes.Testes
{
    public partial class FrmTeste : Form
    {
        private Form FormPrincipal{get; set; }

        public FrmTeste()
        {
            InitializeComponent();
            this.FormPrincipal = new FrmAlocacao();
            
        }

        public string inputText= "";

        private void FrmTeste_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Teste pra validação do tipo de etiqueta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FrmTeste_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                if (inputText != "" && inputText != null)
                {
                    Etiqueta.TipoCode tipoEtiquetaVenda = Leitor.validaInputValueEtiqueta(inputText,new EtiquetaVenda());
                    Etiqueta.TipoCode tipoEtiquetaAlocacao = Leitor.validaInputValueEtiqueta(inputText, new EtiquetaAlocacao());
                    inputText = "";
                }
            }
            else
            {
                inputText += e.KeyChar.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmAlocacao frm= new FrmAlocacao();
            //frm.iniciarProcesso();
        }
    }
}