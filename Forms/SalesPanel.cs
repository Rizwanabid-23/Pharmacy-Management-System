using Bunifu.UI.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Multicare_pharmacy.Forms
{
    public partial class SalesPanel : Form
    {
        public SalesPanel(string sessionCode, string EID, string EName)
        {
            InitializeComponent();
            this.sessionCode.Text = sessionCode;
            this.employeeID.Text = EID;
            this.employeeName.Text = EName;
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

        private void tabQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //if (productID.)
            }
        }

        private void addCustBTN_Click(object sender, EventArgs e)
        {
            Forms.AddCustomer addCustomerInstance = new Forms.AddCustomer();
            addCustomerInstance.Show();
        }

        private void getDetailsBTN_Click(object sender, EventArgs e)
        {
            int Id;
            if (int.TryParse(CID.Text, out Id))
            {
                var connection = Configuration.getInstance().getConnection();
                try
                {
                    SqlDataAdapter command = new SqlDataAdapter("select * from Customer where ID='" + Id + "'", connection);
                    DataTable dataTable = new DataTable();
                    command.Fill(dataTable);
                    if (dataTable.Rows.Count == 1)
                    {
                        DataRow dataRow = dataTable.Rows[0];
                        CName.Text = dataRow.ItemArray[1].ToString();
                        CAddress.Text = dataRow.ItemArray[3].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Customer does not exist.\nPlease enter appropriate ID");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
                MessageBox.Show("Please enter appropriate ID");
            }
        }

        private void SignOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}