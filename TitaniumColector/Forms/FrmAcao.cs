using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Utility;
using TitaniumColector.Classes.Dao;
using TitaniumColector.Classes.Utility;

namespace TitaniumColector.Forms
{
    public partial class FrmAcao : Form,ICall
    {

        public FrmAcao()
        {
            InitializeComponent();
            this.controlsConfig();
        }

        public FrmAcao(bool test) { }

        private void mnuAcao_Logout_Click(object sender, EventArgs e)
        {
            MainConfig.UserOn.registrarAcesso(TitaniumColector.Classes.Usuario.statusLogin.NAOLOGADO);
            frmLogin login = new frmLogin();
            login.Show();
            this.Close();
        }

        private void mnuAcao_Exit_Click(object sender, EventArgs e)
        {
            MainConfig.UserOn.registrarAcesso(TitaniumColector.Classes.Usuario.statusLogin.NAOLOGADO);
            Application.Exit();
        }

        private void btnVenda_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            FrmProposta proposta = new FrmProposta();
            this.Close();
            proposta.Show();
        }

        private void btnCompra_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidade em desenvolvimento!!");
        }

        #region ICall Members

        /// <summary>
        /// Método da interface utilizado quando o usuário escolhe uma das opções do menu do form propostas
        /// </summary>
        public void call()
        {
            FrmAcao form = new FrmAcao();
            form.Show();
        }

        #endregion
     
    }
}