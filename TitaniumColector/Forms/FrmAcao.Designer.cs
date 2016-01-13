namespace TitaniumColector.Forms
{
    partial class FrmAcao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu menuFrmAcao;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnVenda = new System.Windows.Forms.Button();
            this.btnAlocacao = new System.Windows.Forms.Button();
            this.painelFrmAcao = new System.Windows.Forms.Panel();
            this.lblPermissaoAlocacao = new System.Windows.Forms.Label();
            this.lblPermissaoVenda = new System.Windows.Forms.Label();
            this.painelFrmAcao.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnVenda
            // 
            this.btnVenda.Location = new System.Drawing.Point(41, 39);
            this.btnVenda.Name = "btnVenda";
            this.btnVenda.Size = new System.Drawing.Size(265, 174);
            this.btnVenda.TabIndex = 0;
            this.btnVenda.Text = "Liberação Vendas";
            this.btnVenda.Click += new System.EventHandler(this.btnVenda_Click);
            // 
            // btnAlocacao
            // 
            this.btnAlocacao.Location = new System.Drawing.Point(41, 242);
            this.btnAlocacao.Name = "btnAlocacao";
            this.btnAlocacao.Size = new System.Drawing.Size(265, 174);
            this.btnAlocacao.TabIndex = 1;
            this.btnAlocacao.Text = "Guardar Volumes";
            this.btnAlocacao.Click += new System.EventHandler(this.btnCompra_Click);
            // 
            // painelFrmAcao
            // 
            this.painelFrmAcao.Controls.Add(this.lblPermissaoVenda);
            this.painelFrmAcao.Controls.Add(this.lblPermissaoAlocacao);
            this.painelFrmAcao.Controls.Add(this.btnVenda);
            this.painelFrmAcao.Controls.Add(this.btnAlocacao);
            this.painelFrmAcao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.painelFrmAcao.Location = new System.Drawing.Point(0, 0);
            this.painelFrmAcao.Name = "painelFrmAcao";
            this.painelFrmAcao.Size = new System.Drawing.Size(346, 455);
            // 
            // lblPermissaoAlocacao
            // 
            this.lblPermissaoAlocacao.ForeColor = System.Drawing.Color.Crimson;
            this.lblPermissaoAlocacao.Location = new System.Drawing.Point(114, 274);
            this.lblPermissaoAlocacao.Name = "lblPermissaoAlocacao";
            this.lblPermissaoAlocacao.Size = new System.Drawing.Size(123, 20);
            this.lblPermissaoAlocacao.Text = "Solicite Permissão";
            this.lblPermissaoAlocacao.Visible = false;
            // 
            // lblPermissaoVenda
            // 
            this.lblPermissaoVenda.ForeColor = System.Drawing.Color.Crimson;
            this.lblPermissaoVenda.Location = new System.Drawing.Point(114, 67);
            this.lblPermissaoVenda.Name = "lblPermissaoVenda";
            this.lblPermissaoVenda.Size = new System.Drawing.Size(123, 20);
            this.lblPermissaoVenda.Text = "Solicite Permissão";
            this.lblPermissaoVenda.Visible = false;
            // 
            // FrmAcao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(346, 455);
            this.ControlBox = false;
            this.Controls.Add(this.painelFrmAcao);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmAcao";
            this.Text = "FrmAcao";
            this.Load += new System.EventHandler(this.FrmAcao_Load);
            this.painelFrmAcao.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Configura todos os controles do formulário
        /// </summary>
        private void controlsConfig()
        {

            //
            //FrmAcao
            //
            this.Size = new System.Drawing.Size(MainConfig.ScreenSize.Width, MainConfig.ScreenSize.Height);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Text = "Próxima ação";

            //Menus 

            ////menuItem Opções
            this.mnuAcao_Opcoes = new System.Windows.Forms.MenuItem();
            this.mnuAcao_Opcoes.Text = "Opção";
            this.mnuAcao_Opcoes.Enabled = true;

            ////menuItem Exit
            this.mnuAcao_Exit = new System.Windows.Forms.MenuItem();
            this.mnuAcao_Exit.Text = "Exit";
            this.mnuAcao_Exit.Enabled = true;
            this.mnuAcao_Exit.Click += new System.EventHandler(mnuAcao_Exit_Click);
           

            ////MenuItem Logout
            this.mnuAcao_Logout = new System.Windows.Forms.MenuItem();
            this.mnuAcao_Logout.Text = "Logout";
            this.mnuAcao_Logout.Enabled = true;
            this.mnuAcao_Logout.Click += new System.EventHandler(mnuAcao_Logout_Click);
            //
            ////Adiciona os menus ao MenuPrincipal.
            //
            this.menuFrmAcao = new System.Windows.Forms.MainMenu();
            this.menuFrmAcao.MenuItems.Add(mnuAcao_Opcoes);
            this.mnuAcao_Opcoes.MenuItems.Add(this.mnuAcao_Exit);
            this.mnuAcao_Opcoes.MenuItems.Add(this.mnuAcao_Logout);
            this.Menu = this.menuFrmAcao;

            //
            //btnVenda
            //
            this.btnVenda.Location = new System.Drawing.Point(MainConfig.intPositionX + 20, MainConfig.intPositionY + 50);
            this.btnVenda.Size = new System.Drawing.Size(MainConfig.ScreenSize.Width - 40, MainConfig.ScreenSize.Height / 3);
            this.btnVenda.Text = "Liberação Vendas";
            this.btnVenda.Font = MainConfig.FontGrandeBold;
            //
            //btnSaida
            //
            this.btnAlocacao.Location = new System.Drawing.Point(MainConfig.intPositionX + 20, btnVenda.Location.Y + btnVenda.Size.Height + 10);
            this.btnAlocacao.Size = new System.Drawing.Size(MainConfig.ScreenSize.Width - 40, MainConfig.ScreenSize.Height / 3);
            this.btnAlocacao.Text = "Guardar Volumes";
            this.btnAlocacao.BackColor = System.Drawing.SystemColors.Control;
            this.btnAlocacao.Font = MainConfig.FontGrandeBold;
            // 
            // lblPermissaoVenda
            // 
            this.lblPermissaoVenda.ForeColor = System.Drawing.Color.Crimson;
            this.lblPermissaoVenda.Location = new System.Drawing.Point(this.btnVenda.Location.X + (this.btnVenda.Size.Width / 2) - (this.lblPermissaoVenda.Size.Width / 2), this.btnVenda.Location.Y + 20);
            this.lblPermissaoVenda.Name = "lblPermissaoVenda";
            this.lblPermissaoVenda.Size = new System.Drawing.Size(123, 20);
            this.lblPermissaoVenda.Text = "Solicite Permissão";
            // 
            // lblPermissaoAlocacao
            //// 
            this.lblPermissaoAlocacao.ForeColor = System.Drawing.Color.Crimson;
            this.lblPermissaoAlocacao.Location = new System.Drawing.Point(this.btnAlocacao.Location.X + (this.btnVenda.Size.Width / 2) - (this.lblPermissaoAlocacao.Size.Width / 2), this.btnAlocacao.Location.Y + 20 );
            this.lblPermissaoAlocacao.Name = "lblPermissaoAlocacao";
            this.lblPermissaoAlocacao.Size = new System.Drawing.Size(123, 20);
            this.lblPermissaoAlocacao.Text = "Solicite Permissão";

        }


        #endregion

        private System.Windows.Forms.MenuItem mnuAcao_Opcoes;
        private System.Windows.Forms.MenuItem mnuAcao_Exit;
        private System.Windows.Forms.MenuItem mnuAcao_Logout;
        private System.Windows.Forms.Button btnVenda;
        private System.Windows.Forms.Button btnAlocacao;
        private System.Windows.Forms.Panel painelFrmAcao;
        private System.Windows.Forms.Label lblPermissaoVenda;
        private System.Windows.Forms.Label lblPermissaoAlocacao;
    }
}