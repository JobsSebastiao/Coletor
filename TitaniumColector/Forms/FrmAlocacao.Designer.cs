using System.Drawing;
namespace TitaniumColector.Forms
{
    partial class FrmAlocacao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.SuspendLayout();
            // 
            // FrmAlocacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(320, 430);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FrmAlocacao";
            this.Text = "Guarda de Volumes";
            this.ResumeLayout(false);

        }

        private void inicializarForm() 
        {
            fontStringSize = new SizeF();
            this.txtCabecalho = new System.Windows.Forms.TextBox();
            this.txtDescricaoTotal = new System.Windows.Forms.TextBox ();
            this.btnSair =  new System.Windows.Forms.Button();
            this.btnFinalizar = new System.Windows.Forms.Button();
            this.pnGridItens = new System.Windows.Forms.Panel();
            this.lbTotalAlocado = new System.Windows.Forms.Label();
            lbQtdTotalAlocado = new System.Windows.Forms.Label();
            txtTotal = new System.Windows.Forms.TextBox();
            listVolumes = new System.Windows.Forms.ListView();
            //
            // txtCabecalho
            //
            this.txtCabecalho.Font = MainConfig.FontMediaBold;
            this.txtCabecalho.Size = new Size(this.Width-20,30);
            this.txtCabecalho.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCabecalho.Location = new Point((this.Right - txtCabecalho.Width)/2, 5);
            this.txtCabecalho.Text = "   Processo de Guarda de Volumes";
            this.txtCabecalho.BackColor = SystemColors.Info;
            this.txtCabecalho.ReadOnly = true;
            this.txtCabecalho.TabStop = false;
            //
            //panel Grid Volumes
            //
            this.pnGridItens.Name = "pnGridItens";
            this.pnGridItens.Location = new Point(this.txtCabecalho.Location.X,(this.txtCabecalho.Location.Y + this.txtCabecalho.Size.Height+1));
            this.pnGridItens.Size = new Size(this.txtCabecalho.Size.Width,180);
            this.pnGridItens.BackColor = Color.White;
            //
            // txtDescricaoTotal
            //
            this.txtDescricaoTotal.Font = MainConfig.FontMediaBold;
            this.txtDescricaoTotal.Size = new Size(txtCabecalho.Size.Width - 70, txtCabecalho.Size.Height);
            this.txtDescricaoTotal.Location = new Point(this.pnGridItens.Location.X, this.pnGridItens.Location.Y + this.pnGridItens.Height + 1);
            this.txtDescricaoTotal.Text = "        Total de Volumes";
            this.txtDescricaoTotal.ReadOnly = true;
            this.txtDescricaoTotal.TabStop = false;
            this.txtDescricaoTotal.BackColor = SystemColors.ControlLight;
            //
            // txtTotal
            //
            txtTotal.Font = MainConfig.FontMediaBold;
            txtTotal.Size = new Size(pnGridItens.Size.Width - txtDescricaoTotal.Size.Width,txtDescricaoTotal.Size.Height);
            txtTotal.Location = new Point(txtDescricaoTotal.Location.X + txtDescricaoTotal.Size.Width + 1, txtDescricaoTotal.Location.Y);
            txtTotal.ReadOnly = true;
            txtTotal.TabStop = false;
            txtTotal.BackColor = Color.White;
            txtTotal.Text = "  0000";
            //
            // Button Sair
            //             
            this.btnSair.Name = "btnSair";
            this.btnSair.Text = "Sair";
            this.btnSair.Font = MainConfig.FontMediaBold;
            this.btnSair.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSair.Size = new System.Drawing.Size(75, 30);
            this.btnSair.Location = new System.Drawing.Point(txtTotal.Location.X, txtDescricaoTotal.Location.Y + txtDescricaoTotal.Size.Height + 15);
            this.btnSair.Click += new System.EventHandler(btnSair_Click);
            //
            // Button Finalizar
            //             
            this.btnFinalizar.Name = "btnFinalizar";
            this.btnFinalizar.Text = "Finalizar";
            this.btnFinalizar.Font = MainConfig.FontMediaBold;
            this.btnFinalizar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFinalizar.Size = new System.Drawing.Size(75, 30);
            this.btnFinalizar.Location = new System.Drawing.Point(txtDescricaoTotal.Location.X, btnSair.Location.Y);
            this.btnFinalizar.Click += new System.EventHandler(btnFinalizar_Click); 
            //
            // Total Alocado :
            //
            this.lbTotalAlocado.Text = "Total_Vol._Alocados";
            this.lbTotalAlocado.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lbTotalAlocado.Text, this.lbTotalAlocado.Font);
            this.lbTotalAlocado.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lbTotalAlocado.Location = new Point(this.btnFinalizar.Location.X + this.btnFinalizar.Size.Width + 18, this.txtDescricaoTotal.Location.Y + this.txtDescricaoTotal.Size.Height+5);
            this.lbTotalAlocado.Text = "Total Vol. Alocados";
            //
            // Quantidade Total Alocado :
            //
            lbQtdTotalAlocado.Text = " 000 ";
            lbQtdTotalAlocado.Font = MainConfig.FontMediaBold;
            fontStringSize = MainConfig.sizeStringEmPixel(lbQtdTotalAlocado.Text, lbQtdTotalAlocado.Font);
            lbQtdTotalAlocado.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            lbQtdTotalAlocado.Location = new Point(this.Width/2 - (lbQtdTotalAlocado.Size.Width/2) , this.lbTotalAlocado.Location.Y + this.lbTotalAlocado.Size.Height + 5 );
            lbQtdTotalAlocado.Text = " 000 ";
            //
            // ListVolumes
            //
            listVolumes.FullRowSelect = true;
            listVolumes.Size = new System.Drawing.Size(this.pnGridItens.Size.Width - 2, this.pnGridItens.Size.Height- 1);
            listVolumes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listVolumes.BackColor = System.Drawing.Color.Beige;
            listVolumes.Font = MainConfig.FontPadraoBold;
            listVolumes.Location = new System.Drawing.Point(MainConfig.intPositionX + 1, MainConfig.intPositionY + 1);
            listVolumes.View = System.Windows.Forms.View.Details;
            //
            //Colunas ListVolumes
            //
            listVolumes.Columns.Add("codigoRegistro", 0, System.Windows.Forms.HorizontalAlignment.Center);
            listVolumes.Columns.Add("Local", 90, System.Windows.Forms.HorizontalAlignment.Center);
            listVolumes.Columns.Add("Produto", 140, System.Windows.Forms.HorizontalAlignment.Center);
            listVolumes.Columns.Add("Volume", 75, System.Windows.Forms.HorizontalAlignment.Center);
            listVolumes.Columns.Add("Lote", 75, System.Windows.Forms.HorizontalAlignment.Center);
            //
            //Adiciona itens ao panel
            //
            this.pnGridItens.Controls.Add(listVolumes);
            //
            //Adiciona controles ao Form
            //
            this.Controls.Add(this.txtCabecalho);
            this.Controls.Add(this.pnGridItens);
            this.Controls.Add(this.txtDescricaoTotal);
            this.Controls.Add(txtTotal);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnFinalizar);
            this.Controls.Add(lbQtdTotalAlocado);
            this.Controls.Add(lbTotalAlocado);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FrmAlocacao_KeyPress);

        }

        private static System.Windows.Forms.ListView listVolumes;
        private static System.Windows.Forms.ListViewItem listItem;
        private static System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.TextBox txtCabecalho;
        private System.Windows.Forms.TextBox txtDescricaoTotal;
        private System.Windows.Forms.Label lbTotalAlocado;
        private static System.Windows.Forms.Label lbQtdTotalAlocado;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button btnFinalizar;
        private System.Windows.Forms.Panel pnGridItens;
        private System.Drawing.SizeF fontStringSize;
        
        #endregion

    }
}