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
        public bool permissaoVendas { get; set; }
        public bool permissaoAlocacao { get; set; }
        public FrmAcao()
        {
            InitializeComponent();
            this.controlsConfig();
        }
        //
        public FrmAcao(bool test) { }

        void FrmAcao_Load(object sender, System.EventArgs e)
        {

            try
            {
                if (!MainConfig.UserOn.temPermissoes())
                {
                    throw new Exception("Usuario não possui as permissões nescessárias para utilização da aplicação!");
                }

                //Recupera a lista de permissoes definadas para o usuário
                foreach (var item in MainConfig.UserOn.Permissoes)
                {
                    if (item.MetodoMetodo.Equals("Liberacao Vendas Mobile"))
                    {
                        permissaoVendas = (item.ValorUsuarioMetodo == 1 ? true : false);
                        continue;
                    }

                    if (item.MetodoMetodo.Equals("Guarda Volumes Mobile"))
                    {
                        permissaoAlocacao = (item.ValorUsuarioMetodo == 1 ? true : false);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.btnAlocacao.Enabled = false;
                this.btnVenda.Enabled = false;
            }
            finally 
            {
                this.btnAlocacao.Enabled = this.permissaoAlocacao;
                this.lblPermissaoAlocacao.Visible = !this.permissaoAlocacao;
                this.btnVenda.Enabled = this.permissaoVendas;
                this.lblPermissaoVenda.Visible = !this.permissaoVendas;
            }
        }

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
            proposta.Show();
            this.Hide();
            this.Enabled = true;
        }

        private void btnCompra_Click(object sender, EventArgs e)
        {
            FrmAlocacao frm = new FrmAlocacao();
            frm.Show();
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