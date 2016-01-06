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
            //fontStringSize = new SizeF();
            this.txtCabecalho = new System.Windows.Forms.TextBox();
            this.txtDescricaoTotal = new System.Windows.Forms.TextBox ();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.btnSair =  new System.Windows.Forms.Button();
            this.pnGridItens = new System.Windows.Forms.Panel();
            this.listVolumes = new System.Windows.Forms.ListView();
            //
            // txtCabecalho
            //
            this.txtCabecalho.Font = MainConfig.FontMediaBold;
            this.txtCabecalho.Size = new Size(this.Width-20,30);
            this.txtCabecalho.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCabecalho.Location = new Point((this.Right - txtCabecalho.Width)/2, 15);
            this.txtCabecalho.Text = "   Processo de Guarda de Volumes";
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
            //
            // txtTotal
            //
            this.txtTotal.Font = MainConfig.FontMediaBold;
            this.txtTotal.Size = new Size(this.pnGridItens.Size.Width - this.txtDescricaoTotal.Size.Width,txtDescricaoTotal.Size.Height);
            this.txtTotal.Location = new Point(this.txtDescricaoTotal.Location.X + this.txtDescricaoTotal.Size.Width + 1, this.txtDescricaoTotal.Location.Y);
            this.txtTotal.ReadOnly = true;
            this.txtTotal.TabStop = false;
            this.txtTotal.Text = "00000";
            //
            // btnSair
            //             
            this.btnSair.Name = "btnSair";
            this.btnSair.Text = "Sair";
            this.btnSair.Font = MainConfig.FontMediaBold;
            this.btnSair.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSair.Size = new System.Drawing.Size(60, 30);
            this.btnSair.Location = new System.Drawing.Point(txtDescricaoTotal.Location.X, txtDescricaoTotal.Location.Y + txtDescricaoTotal.Size.Height+1);
            this.btnSair.Click += new System.EventHandler(btnSair_Click);
            //
            // ListVolumes
            //
            this.listVolumes.FullRowSelect = true;
            this.listVolumes.Size = new System.Drawing.Size(this.pnGridItens.Size.Width - 2, this.pnGridItens.Size.Height- 1);
            this.listVolumes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listVolumes.BackColor = System.Drawing.Color.Beige;
            this.listVolumes.Font = MainConfig.FontPadraoBold;
            this.listVolumes.Location = new System.Drawing.Point(MainConfig.intPositionX + 1, MainConfig.intPositionY + 1);
            this.listVolumes.View = System.Windows.Forms.View.Details;
            
            //
            //Colunas ListVolumes
            //
            this.listVolumes.Columns.Add("codigoRegistro", 0, System.Windows.Forms.HorizontalAlignment.Center);
            this.listVolumes.Columns.Add("Local", 90, System.Windows.Forms.HorizontalAlignment.Center);
            this.listVolumes.Columns.Add("Produto", 140, System.Windows.Forms.HorizontalAlignment.Center);
            this.listVolumes.Columns.Add("Volume", 75, System.Windows.Forms.HorizontalAlignment.Center);
            this.listVolumes.Columns.Add("Lote", 75, System.Windows.Forms.HorizontalAlignment.Center);
            //
            //Adiciona itens ao panel
            //
            this.pnGridItens.Controls.Add(this.listVolumes);
            //
            //Adiciona controles ao Form
            //
            this.Controls.Add(this.txtCabecalho);
            this.Controls.Add(this.pnGridItens);
            this.Controls.Add(this.txtDescricaoTotal);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.btnSair);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FrmAlocacao_KeyPress);

        }

        
        private System.Windows.Forms.ListView listVolumes;
        private System.Windows.Forms.ListViewItem listItem;
        private System.Windows.Forms.TextBox txtCabecalho;
        private System.Windows.Forms.TextBox txtDescricaoTotal;
        private System.Windows.Forms.TextBox txtTotal;
        //private System.Drawing.SizeF fontStringSize;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Panel pnGridItens;
        
        #endregion

    }
}