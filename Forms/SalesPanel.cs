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
        DataTable mergedDataTable = new DataTable();
        Forms.AddCustomer addCustomerInstance = Forms.AddCustomer.instance();
        public SalesPanel(string sessionCode, string EID, string EName)
        {
            InitializeComponent();
            this.sessionCode.Text = sessionCode;
            this.employeeID.Text = EID;
            this.employeeName.Text = EName;
            tabQuantity.Enabled = false;
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Quantity", typeof(Decimal));
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
            Int32 billID = 0;
            var connection = Configuration.getInstance().getConnection();
            if (productID.Text != String.Empty && amountRecieved.Text != String.Empty && (packs.Text != String.Empty || tabQuantity.Text != String.Empty) && (cashRB.Checked != true || cardRB.Checked != true))
            {
                int PIdCheck;
                int QuantityCheck;
                int amountRecievedCheck;
                if (int.TryParse(productID.Text, out PIdCheck) && int.TryParse(amountRecieved.Text, out amountRecievedCheck) && (int.TryParse(packs.Text, out QuantityCheck) || int.TryParse(tabQuantity.Text, out QuantityCheck)))
                {
                    try
                    {
                        for (int i = 0; i < detailsDGV.Rows.Count; i++)
                        {
                            int PId = int.Parse(detailsDGV.Rows[i].Cells[0].Value.ToString());
                            int Quantity = int.Parse(detailsDGV.Rows[i].Cells[3].Value.ToString());

                            SqlCommand beginCommand = new SqlCommand("BEGIN TRANSACTION", connection);
                            beginCommand.ExecuteNonQuery();

                            SqlCommand command = new SqlCommand("UPDATE Product SET Packs = Packs - '" + Quantity + "' WHERE ID='" + PId + "'", connection);
                            command.ExecuteNonQuery();

                            SqlCommand commitCommand = new SqlCommand("COMMIT TRANSACTION", connection);
                            commitCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Here01");
                    }
                    try
                    {
                        SqlCommand beginCommand = new SqlCommand("BEGIN TRANSACTION", connection);
                        beginCommand.ExecuteNonQuery();

                        SqlCommand command = new SqlCommand("INSERT INTO Bill VALUES (@CustomerID, @EmployeeID, @OrderDate)", connection);
                        command.Parameters.AddWithValue("@CustomerID", CID.Text);
                        command.Parameters.AddWithValue("@EmployeeID", employeeID.Text);
                        command.Parameters.AddWithValue("@OrderDate", DateTime.Today);
                        command.ExecuteNonQuery();

                        SqlCommand commitCommand = new SqlCommand("COMMIT TRANSACTION", connection);
                        commitCommand.ExecuteNonQuery();
                        clearFields();
                        MessageBox.Show("Customer added to the system successfully");

                        SqlCommand getBillID = new SqlCommand("SELECT TOP(1) ID FROM Bill ORDER BY 1 DESC", connection);
                        billID = (Int32)getBillID.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Here02");
                    }
                    try
                    {

                        int paymentType = 5;
                        if (cardRB.Checked == true)
                        {
                            paymentType = 5;
                        }
                        else if (cashRB.Checked == true)
                        {
                            paymentType = 6;
                        }

                        for (int i = 0; i < detailsDGV.Rows.Count; i++)
                        {

                            int PId = int.Parse(detailsDGV.Rows[i].Cells[0].Value.ToString());
                            int Quantity = int.Parse(detailsDGV.Rows[i].Cells[3].Value.ToString());
                            int Total = int.Parse(detailsDGV.Rows[i].Cells[5].Value.ToString());

                            SqlCommand beginCommand = new SqlCommand("BEGIN TRANSACTION", connection);
                            beginCommand.ExecuteNonQuery();

                            SqlCommand command = new SqlCommand("INSERT INTO BillDetails VALUES (@BillID, @ProductID, @Quantity, @TotalAmount, @PaymentType)", connection);
                            command.Parameters.AddWithValue("@BillID", billID);
                            command.Parameters.AddWithValue("@ProductID", PId);
                            command.Parameters.AddWithValue("@Quantity", Quantity);
                            command.Parameters.AddWithValue("@TotalAmount", Total);
                            command.Parameters.AddWithValue("@PaymentType", paymentType);
                            command.ExecuteNonQuery();

                            SqlCommand commitCommand = new SqlCommand("COMMIT TRANSACTION", connection);
                            commitCommand.ExecuteNonQuery();
                            clearFields();
                            MessageBox.Show("Customer added to the system successfully");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Here02");
                    }

                }
                else
                {
                    MessageBox.Show("Please enter correct numeric Product ID\n OR Please check the Quantity value");
                }
            }
            else
            {
                MessageBox.Show("Error");
            }
            //detailsDGV.Rows.Clear();
            mergedDataTable.Clear();
            clearFields();
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
                            SqlDataAdapter dataAdapter = new SqlDataAdapter("Select * FROM Product WHERE ID='" + PId + "'AND Packs>='" + Quantity + "'", connection);
                            DataTable dataTable = new DataTable();
                            dataAdapter.Fill(dataTable);
                            mergedDataTable.Merge(mergeDataTable(dataTable, dt));
                            DataView dataView = new DataView(mergedDataTable);
                            DataTable filteredTable = dataView.ToTable(false, "ID", "ProductName", "SalePrice", "Quantity", "Discount", "Total");
                            //filteredTable.Columns["Marks"].ColumnName = "SubjectMarks";
                            if (filteredTable.Rows.Count != 0)
                            {
                                detailsDGV.DataSource = filteredTable;
                                totalProducts.Text = "Total Products: " + detailsDGV.Rows.Count.ToString();
                                grandTotal.Text = "Grand Total: " + filteredTable.AsEnumerable().Sum(dr => dr.Field<Decimal>("Total")).ToString();
                            }
                            else
                            {
                                MessageBox.Show("Product out of Stock");
                            }
                            dt.Clear();
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
                dt1.Rows[i]["Quantity"] = dt2.Rows[i]["Quantity"];
                dt1.Rows[i]["Total"] = int.Parse(dt1.Rows[i]["SalePrice"].ToString()) * int.Parse(dt2.Rows[i]["Quantity"].ToString());
            }
            return dt1;
        }
        private void addCustBTN_Click(object sender, EventArgs e)
        {
            addCustomerInstance.Show();
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