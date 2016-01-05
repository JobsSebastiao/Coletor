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
            if (e.KeyChar == Convert.ToChar(13))
            {
                if (inputText != "" && inputText != null)
                {
                    //criar ou recupera a instancia para um objeto ProcedimentAlocacao (Uso De SingleTon)
                    procedimentos = ProcedimentosAlocacao.Instanciar;

                    //VALIDA O TIPO DE ETIQUETA
                    Etiqueta.Tipo tipoEtiqueta = Leitor.validaInputValueEtiqueta(inputText, new EtiquetaAlocacao());
                    
                    //INICIA O PROCEDIMENTO
                    procedimentos.realizarAcao(inputText, tipoEtiqueta);
                }
            }
            else
            {
                inputText += e.KeyChar.ToString();
            }
           
        }

        private void btnSair_Click(object sender, System.EventArgs e)
        {
            this.Close();
            this.Dispose(true);
            FrmAcao frm = new FrmAcao();
            frm.Show();
        }
    }
}