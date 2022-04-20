using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multicare_pharmacy.Forms
{
    public partial class SalesPanel : Form
    {
        public SalesPanel(string sessionCode)
        {
            InitializeComponent();
            this.sessionCode.Text = sessionCode;
            TabQuantity.Enabled = false;
        }

        private void TabQuan_CheckedChanged(object sender, EventArgs e)
        {
            if (TabQuan.Checked)
            {
                Packs.Enabled = false;
                TabQuantity.Enabled = true;
            }
            else
            {
                Packs.Enabled = true;
                TabQuantity.Enabled = false;
            }
        }
    }
}