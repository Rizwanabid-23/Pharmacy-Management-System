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
        DataTable searchedDataTable = new DataTable();
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
            try
            {
                SqlCommand beginCommand = new SqlCommand("BEGIN TRANSACTION", connection);
                beginCommand.ExecuteNonQuery();
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

                                SqlCommand command = new SqlCommand("spUpdateQuantity", connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@Quantity", Quantity);
                                command.Parameters.AddWithValue("@PId", PId);
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Updation Error 01");
                        }
                        try
                        {
                            int CIDCheck;
                            SqlCommand command = new SqlCommand("spUpdateBill", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            if (CID.Text == String.Empty)
                            {
                                command.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                            }
                            else if (int.TryParse(CID.Text, out CIDCheck))
                            {
                                command.Parameters.AddWithValue("@CustomerID", CIDCheck);
                            }
                            command.Parameters.AddWithValue("@EmployeeID", employeeID.Text);
                            command.Parameters.AddWithValue("@OrderDate", DateTime.Today);
                            command.ExecuteNonQuery();

                            SqlCommand getBillID = new SqlCommand("SELECT TOP(1) ID FROM Bill ORDER BY 1 DESC", connection);
                            billID = (Int32)getBillID.ExecuteScalar();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Updation Erorr 02");
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
                                SqlCommand command = new SqlCommand("spUpdateBillDetails", connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@BillID", billID);
                                command.Parameters.AddWithValue("@ProductID", PId);
                                command.Parameters.AddWithValue("@Quantity", Quantity);
                                command.Parameters.AddWithValue("@TotalAmount", Total);
                                command.Parameters.AddWithValue("@PaymentType", paymentType);
                                command.ExecuteNonQuery();
                            }
                            SqlCommand commitCommand = new SqlCommand("COMMIT TRANSACTION", connection);
                            commitCommand.ExecuteNonQuery();
                            Decimal TotalDue = mergedDataTable.AsEnumerable().Sum(dr => dr.Field<Decimal>("Total"));
                            MessageBox.Show("Net Total: " + TotalDue.ToString() + "\n" +
                                            "Cash: " + amountRecieved.Text + "\n" +
                                            "Balance: " + (int.Parse(amountRecieved.Text) - TotalDue).ToString(), "Total Bill");
                            mergedDataTable.Clear();
                            detailsDGV.DataSource = null;
                            clearFields();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Updation Error 03");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error! \nMake sure you have entered correct values.");
                    }
                }
                else
                {
                    MessageBox.Show("Error! \nMake sure you have not leave the required fields empty.");
                }
            }
            catch
            {
                SqlCommand rollBackCommand = new SqlCommand("ROLLBACK TRANSACTION", connection);
                rollBackCommand.ExecuteNonQuery();
            }
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
                        if (Quantity > 0)
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
                        }
                        else
                        {
                            MessageBox.Show("Error! \nQuantity should be greater than 0.");
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
                dt1.Rows[i]["Total"] = (int.Parse(dt1.Rows[i]["SalePrice"].ToString()) * int.Parse(dt2.Rows[i]["Quantity"].ToString())) - (int.Parse(dt1.Rows[i]["Discount"].ToString()) * int.Parse(dt2.Rows[i]["Quantity"].ToString()));
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

        private void detailsDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = detailsDGV.CurrentCell.RowIndex;
            int PId = int.Parse(detailsDGV.Rows[rowIndex].Cells[0].Value.ToString());
            for (int i = mergedDataTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = mergedDataTable.Rows[i];
                if (dr["ID"].ToString() == PId.ToString())
                    dr.Delete();
            }
            mergedDataTable.AcceptChanges();
            detailsDGV.Rows.RemoveAt(rowIndex);
            totalProducts.Text = "Total Products: " + detailsDGV.Rows.Count.ToString();
            grandTotal.Text = "Grand Total: " + mergedDataTable.AsEnumerable().Sum(dr => dr.Field<Decimal>("Total")).ToString();
        }

        private void productName_TextChange(object sender, EventArgs e)
        {
            DataView dv = searchedDataTable.DefaultView;
            dv.RowFilter = "ProductName LIKE '" + productName.Text + "%'";
            searchDGV.DataSource = dv;
        }

        private void SalesPanel_Load(object sender, EventArgs e)
        {
            var connection = Configuration.getInstance().getConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("Select ID, ProductName FROM Product", connection);
            dataAdapter.Fill(searchedDataTable);
            searchDGV.DataSource = searchedDataTable;
        }

        private void searchDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = searchDGV.CurrentCell.RowIndex;
            productID.Text = searchDGV.Rows[rowIndex].Cells[0].Value.ToString();
            productName.Text = searchDGV.Rows[rowIndex].Cells[1].Value.ToString();
        }
    }
}