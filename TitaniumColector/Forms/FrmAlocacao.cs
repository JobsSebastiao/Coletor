using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TitaniumColector.Forms
{
    public partial class FrmAlocacao : Form
    {
        public FrmAlocacao()
        {
            InitializeComponent();
            this.inicializarForm();
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