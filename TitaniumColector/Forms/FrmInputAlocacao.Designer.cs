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
            // FrmInputAlocacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(254, 335);
            this.Menu = this.mainMenu1;
            this.Name = "FrmInputAlocacao";
            this.Text = "Definir Local Armazenagem";
            this.ResumeLayout(false);

        }

        private void configLayout() 
        {
            this.lbDescricao = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.lbDescricao.Text = "Selecionar_local_para_alocação_do_produto!";
            this.lbDescricao.Font = MainConfig.FontMediaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lbDescricao.Text, this.lbDescricao.Font);
            this.lbDescricao.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lbDescricao.Location = new Point(10, 10);
            this.lbDescricao.Text = String.Format("Selecionar local para alocação do produto!");


            this.Size = new System.Drawing.Size(250, 100);
            this.Location = new System.Drawing.Point(40,100);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lbDescricao;
        private System.Drawing.SizeF fontStringSize;

        #endregion
    }
}