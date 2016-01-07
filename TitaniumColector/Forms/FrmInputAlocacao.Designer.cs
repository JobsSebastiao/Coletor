using System;
using System.Drawing;
namespace TitaniumColector.Forms
{
    partial class FrmInputAlocacao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        //private System.Windows.Forms.MainMenu mainMenu1;

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
            this.SuspendLayout();
            // 
            // FrmInputAlocacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(254, 335);
            this.Name = "FrmInputAlocacao";
            this.Text = "Definir Local Armazenagem";
            this.ResumeLayout(false);

        }

        private void configLayout() 
        {
            this.lbProduto = new System.Windows.Forms.Label();
            this.lbLote = new System.Windows.Forms.Label();
            this.lbDescricaoProduto = new System.Windows.Forms.Label();
            this.lbDescricaoLote = new System.Windows.Forms.Label();
            this.lbLocalSugerido = new System.Windows.Forms.Label();
            this.tbLocalSugerido  = new System.Windows.Forms.TextBox();
            this.tbLocalAlocacao = new System.Windows.Forms.TextBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            //
            //Produto :
            //
            this.lbProduto.Text = "Produto_: ";
            this.lbProduto.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lbProduto.Text, this.lbProduto.Font);
            this.lbProduto.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lbProduto.Location = new Point(1,2);
            this.lbProduto.Text = "Produto : ";
            //
            // Descricao Produto
            //
            this.lbDescricaoProduto.Text = "________________________";
            this.lbDescricaoProduto.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lbDescricaoProduto.Text, this.lbDescricaoProduto.Font);
            this.lbDescricaoProduto.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lbDescricaoProduto.Location = new Point(this.lbProduto.Location.X + this.lbProduto.Size.Width + 2, 2);
            this.lbDescricaoProduto.Text = "Produto a ser alocado";
            //
            // Lote :
            //
            this.lbLote.Text = "Lote_: ";
            this.lbLote.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lbLote.Text, this.lbLote.Font);
            this.lbLote.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lbLote.Location = new Point(this.lbProduto.Location.X, this.lbProduto.Location.Y + this.lbProduto.Size.Height + 3);
            this.lbLote.Text = String.Format("Lote :");
            //
            // Descricao Lote
            //
            this.lbDescricaoLote.Text = "________________________";
            this.lbDescricaoLote.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lbDescricaoLote.Text, this.lbDescricaoLote.Font);
            this.lbDescricaoLote.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lbDescricaoLote.Location = new Point(this.lbLote.Location.X + this.lbLote.Size.Width + 2, this.lbLote.Location.Y);
            this.lbDescricaoLote.Text = "Lote a ser Alocado";
            //
            //Sugestao :
            //
            this.lbLocalSugerido.Text = "Sugestão_: ";
            this.lbLocalSugerido.Font = MainConfig.FontMediaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lbLocalSugerido.Text, this.lbLocalSugerido.Font);
            this.lbLocalSugerido.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lbLocalSugerido.Location = new Point(this.lbProduto.Location.X, this.lbLote.Location.Y + this.lbLote.Size.Height + 7);
            this.lbLocalSugerido.Text = String.Format("Sugestão :");

            this.tbLocalSugerido.Location = new System.Drawing.Point(lbLocalSugerido.Location.X + this.lbLocalSugerido.Size.Width, lbLocalSugerido.Location.Y - 1);
            this.tbLocalSugerido.Multiline = false;
            this.tbLocalSugerido.Font = MainConfig.FontMediaBold;
            this.tbLocalSugerido.Name = "tbLocalSugerido";
            this.tbLocalSugerido.Size = new System.Drawing.Size(this.Size.Width - this.lbLocalSugerido.Size.Width + 10, 60);
            this.tbLocalSugerido.TabStop = false;
            this.tbLocalSugerido.ReadOnly = true;
            this.tbLocalSugerido.BackColor = Color.White;
            this.tbLocalSugerido.Text = "L2A";

            this.tbLocalAlocacao.Location = new System.Drawing.Point(60, this.tbLocalSugerido.Location.Y + this.tbLocalSugerido.Size.Height + 5);
            this.tbLocalAlocacao.Multiline = false;
            this.tbLocalAlocacao.Name = "tbLocalAlocacao";
            this.tbLocalAlocacao.Size = new System.Drawing.Size(160,200);
            this.tbLocalAlocacao.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbLocalAlocacao.Font = MainConfig.FontMuitoGrandeBold;
            this.tbLocalAlocacao.BackColor = Color.White;
            this.tbLocalAlocacao.TabStop = true;
            this.tbLocalAlocacao.TabIndex = 0;
            this.tbLocalAlocacao.ReadOnly = false;
            this.tbLocalAlocacao.Text = "   L2A";
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(this.tbLocalAlocacao.Location.X - 20, this.tbLocalAlocacao.Location.Y + this.tbLocalAlocacao.Size.Height + 5 );
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 50);
            this.btOK.Font = MainConfig.FontMediaBold;
            this.btOK.TabIndex = 0;
            this.btOK.TabStop = false;
            this.btOK.Text = "OK";
            this.btOK.Click += new EventHandler(btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(this.btOK.Location.X + 120, this.btOK.Location.Y );
            this.btCancel.Name = "btCancel";
            this.btCancel.Font = MainConfig.FontMediaBold;
            this.btCancel.Size = new System.Drawing.Size(80, 50);
            this.btOK.TabIndex = 0;
            this.btOK.TabStop = false;
            this.btCancel.Text = "CANCEL";
            this.btCancel.Click += new EventHandler(btCancel_Click);

            this.Size = new System.Drawing.Size(280, 210);
            this.Location = new System.Drawing.Point(20,70);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(FrmInputAlocacao_Closing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FrmInputAlocacao_KeyPress);
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Controls.Add(this.lbProduto);
            this.Controls.Add(this.lbLote);
            this.Controls.Add(this.lbDescricaoProduto);
            this.Controls.Add(this.lbDescricaoLote);
            this.Controls.Add(this.lbLocalSugerido);
            this.Controls.Add(this.tbLocalAlocacao);
            this.Controls.Add(this.tbLocalSugerido);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lbProduto;
        private System.Windows.Forms.Label lbLote;
        private System.Windows.Forms.Label lbDescricaoProduto;
        private System.Windows.Forms.Label lbDescricaoLote;
        private System.Windows.Forms.Label lbLocalSugerido;
        private System.Windows.Forms.TextBox tbLocalSugerido;
        private System.Windows.Forms.TextBox tbLocalAlocacao;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Drawing.SizeF fontStringSize;

        #endregion
    }
}