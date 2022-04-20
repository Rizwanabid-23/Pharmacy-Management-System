using Bunifu.UI.WinForms;
using System;
using System.Windows.Forms;

namespace Multicare_pharmacy.Forms
{
    public partial class SalesPanel : Form
    {
        public SalesPanel(string sessionCode)
        {
            InitializeComponent();
            this.sessionCode.Text = sessionCode;
            tabQuantity.Enabled = false;
        }

        private void TabQuan_CheckedChanged(object sender, EventArgs e)
        {
            if (tabQuanCb.Checked)
            {
                packs.Enabled = false;
                packs.Text = String.Empty;
                tabQuantity.Enabled = true;
            }
            else
            {
                packs.Enabled = true;
                tabQuantity.Text = String.Empty;
                tabQuantity.Enabled = false;
            }
        }

        private void generateBillBTN_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void clearFields()
        {
            productID.Text = String.Empty;
            packs.Text = String.Empty;
            tabQuantity.Text = String.Empty;
            amountRecieved.Text = String.Empty;
            tabQuanCb.Checked = false;
            cardRB.Checked = false;
            cashRB.Checked = false;
            CID.Text = String.Empty;
            CName.Text = String.Empty;
            CAddress.Text = String.Empty;
            grandTotal.Text = "Grand Total:";
            totalProducts.Text = "Total Products";
        }
    }
}