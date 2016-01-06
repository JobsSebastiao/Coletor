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

namespace TitaniumColector.Classes.Testes
{
    public partial class FrmTeste : Form
    {
        public FrmTeste()
        {
            InitializeComponent();
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
                    Etiqueta.Tipo tipoEtiquetaVenda = Leitor.validaInputValueEtiqueta(inputText,new EtiquetaVenda());
                    Etiqueta.Tipo tipoEtiquetaAlocacao = Leitor.validaInputValueEtiqueta(inputText, new EtiquetaAlocacao());
                    inputText = "";
                }
            }
            else
            {
                inputText += e.KeyChar.ToString();
            }
        }
    }
}