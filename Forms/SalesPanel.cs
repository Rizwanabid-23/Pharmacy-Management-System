using Bunifu.UI.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Multicare_pharmacy.Forms
{
    public partial class SalesPanel : Form
    {
        DataTable dt = new DataTable();
        Forms.AddCustomer addCustomerInstance = Forms.AddCustomer.instance();
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
            dt.Clear();
        }

        private void tabQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (productID.Text != String.Empty && (packs.Text != String.Empty || tabQuantity.Text != String.Empty))
                {
                    int PId;
                    int Quantity;
                    if (int.TryParse(productID.Text, out PId) && (int.TryParse(packs.Text, out Quantity) || int.TryParse(tabQuantity.Text, out Quantity)))
                    {
                        try
                        {
                            dt.Columns.Add("ID");
                            dt.Columns.Add("Quantity");
                            DataRow dr = dt.NewRow();
                            dr["ID"] = PId;
                            dr["Quantity"] = Quantity;
                            dt.Rows.Add(dr);
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        var connection = Configuration.getInstance().getConnection();
                        try
                        {
                            SqlDataAdapter dataAdapter = new SqlDataAdapter("Select * FROM Product WHERE ID='" + PId + "'AND Quantity>='" + Quantity + "'", connection);
                            DataTable dataTable = new DataTable();
                            dataAdapter.Fill(dataTable);
                            DataTable mergedDataTable = mergeDataTable(dataTable, dt);
                            DataView dataView = new DataView(mergedDataTable);
                            DataTable filteredTable = dataView.ToTable(false, "ID", "ProductName", "SalePrice", "Quantity", "Discount", "Total");
                            //filteredTable.Columns["Marks"].ColumnName = "SubjectMarks";
                            detailsDGV.DataSource = filteredTable;
                            totalProducts.Text = "Total Products: " + detailsDGV.Rows.Count.ToString();
                            grandTotal.Text = "Grand Total: " + filteredTable.AsEnumerable().Sum(dr => dr.Field<Decimal>("Total")).ToString();
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter correct numeric Product ID\n OR Please check the Quantity value");
                    }
                }
            }
        }

        private void addCustBTN_Click(object sender, EventArgs e)
        {
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
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Customer WHERE ID='" + Id + "'", connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    if (dataTable.Rows.Count == 1)
                    {
                        DataRow dataRow = dataTable.Rows[0];
                        CName.Text = dataRow.ItemArray[1].ToString();
                        CAddress.Text = dataRow.ItemArray[3].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Customer does not exist.\nPlease enter correct numeric ID or create a new customer.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
                MessageBox.Show("Please enter correct numeric ID");
            }
        }

        public DataTable mergeDataTable(DataTable dt1, DataTable dt2)
        {
            dt1.Columns.Add("Quantity", typeof(Decimal));
            dt1.Columns.Add("Total", typeof(Decimal));
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (dt1.Rows[i]["ID"] == dt2.Rows[i]["ID"])
                {
                    dt1.Rows[i]["Quantity"] = dt2.Rows[i]["Quantity"];
                    dt1.Rows[i]["Total"] = int.Parse(dt1.Rows[i]["SalePrice"].ToString()) * int.Parse(dt2.Rows[i]["Quantity"].ToString());
                }
            }
            return dt1;
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
            grandTotal.Text = "Grand Total: 0";
            totalProducts.Text = "Total Products: 0";
        }
    }
}