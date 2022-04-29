using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Multicare_pharmacy
{
    public partial class admins : Form
    {
        List<Panel> listpanel = new List<Panel>();
        DataTable dt = new DataTable();
        DataTable mergedDataTable = new DataTable();
        DataTable searchedDataTable = new DataTable();
        DataTable nameTable = new DataTable();
        DataTable pname_change = new DataTable();
        DataTable pname_change1 = new DataTable();
        DataTable pname_change2 = new DataTable();
        public admins()
        {
            InitializeComponent();
        }

        private void admins_Load(object sender, EventArgs e)
        {
            //this.productTableAdapter.Fill(this.multiCarePharmacyDataSet.Product);
            //this.employeeTableAdapter.Fill(this.multiCarePharmacyDataSet.Employee);
            //this.supplierTableAdapter.Fill(this.multiCarePharmacyDataSet.Supplier);
            listpanel.Add(panel1);
            listpanel.Add(panel2);
            listpanel.Add(panel3);
            listpanel.Add(panel4);
            listpanel.Add(panel5);
            listpanel.Add(panel6);
            listpanel.Add(panel7);
            listpanel.Add(panel8);
            listpanel.Add(panel9);

            for (int i = 0; i < listpanel.Count; i++)
            {
                if (listpanel[i].Name == "panel2")
                {
                    listpanel[i].Show();
                    listpanel[i].BringToFront();
                }
                else
                {
                    listpanel[i].Hide();
                }
            }

            var connection = Configuration.getInstance().getConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("Select ID, Username,PIN,FirstName,LastName,CNIC,Address,Phone,Email FROM Employee", connection);
            dataAdapter.Fill(searchedDataTable);
            dataGridView1.DataSource = searchedDataTable;



            var connection2 = Configuration.getInstance().getConnection();
            SqlDataAdapter dataAdapter2 = new SqlDataAdapter("Select ID, Username,PIN,FirstName,LastName,CNIC,Address,Phone,Email FROM Employee", connection2);
            dataAdapter2.Fill(nameTable);
            dataGridView2.DataSource = nameTable;


            var connection3 = Configuration.getInstance().getConnection();
            SqlDataAdapter dataAdapter3 = new SqlDataAdapter("Select P.ID,P.ProductName,P.SalePrice,P.PurchasePrice,P.Category,P.Packs,P.QuantityPerPack,P.Legality,P.Potency,P.Discount,PS.SupplierID FROM Product P join ProductSupplier PS on P.ID=PS.ProductID", connection3);
            dataAdapter3.Fill(pname_change);
            dataGridView3.DataSource = pname_change;


            var connection4 = Configuration.getInstance().getConnection();
            SqlDataAdapter dataAdapter4 = new SqlDataAdapter("Select P.ID,P.ProductName,P.SalePrice,P.PurchasePrice,P.Category,P.ManufacturingDate,P.ExpiryDate,P.Packs,P.QuantityPerPack,P.Legality,P.Potency,P.Discount FROM Product P", connection4);
            dataAdapter4.Fill(pname_change1);
            dataGridView4.DataSource = pname_change1;


            var connection5 = Configuration.getInstance().getConnection();
            SqlDataAdapter dataAdapter5 = new SqlDataAdapter("Select P.ID,P.ProductName,P.Packs,P.QuantityPerPack FROM Product P", connection5);
            dataAdapter5.Fill(pname_change2);
            dataGridView5.DataSource = pname_change2;
        }

        private void bunifuButton2_Click_1(object sender, EventArgs e)
        {
            foreach (var c in panel1.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = String.Empty;
                }
            }
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            foreach (var c in panel2.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = String.Empty;
                }
                if (c is ComboBox)
                {
                    ((ComboBox)c).Text = string.Empty;
                }
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            // int id = int.Parse(textBox1.Text);
            string uname = textBox2.Text;
            string pin = textBox3.Text;
            string fname = textBox4.Text;
            string lname = textBox5.Text;
            string cnic = (textBox6.Text);
            string adress = textBox7.Text;
            string phone = textBox8.Text;
            string email = textBox9.Text;

            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("exec addEmployee '" + uname + "','" + pin + "','" + fname + "','" + lname + "','" + cnic + "','" + adress + "','" + phone + "','" + email + "' ", con);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            int leg = 0;
            float potency = 0;
            string legality = "";
            float discount = 0;
            int sid = int.Parse(comboBox3.Text);
            string name = textBox10.Text;
            string category = comboBox1.Text;
            float sprice = float.Parse(textBox11.Text);
            float pprice = float.Parse(textBox12.Text);
            string mfg = Convert.ToDateTime(bunifuDatePicker1.Text).ToString();
            string exp = Convert.ToDateTime(bunifuDatePicker2.Text).ToString();

            if (textBox16.Text != "")
            {
                potency = float.Parse(textBox16.Text);
            }

            string manufacturer = textBox15.Text;
            if (comboBox2.Text != "")
            {
                legality = comboBox2.Text;
                if (legality == "Allowed")
                {
                    leg = 3;
                }
                else if (legality == "Not Allowed")
                {
                    leg = 4;
                }
            }

            float nop = float.Parse(textBox13.Text);
            int qp = int.Parse(textBox14.Text);
            if (textBox17.Text != "")
            {
                discount = float.Parse(textBox17.Text);
            }

            int cat = 0;
            if (category == "Medicine")
            {
                cat = 1;
            }
            else if (category == "Day-to-Day")
            {
                cat = 2;
            }


            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Insert into ProductManufacturer values (@ProductName,@Manufacturer)", con);
            cmd1.Parameters.AddWithValue("@ProductName", name);
            cmd1.Parameters.AddWithValue("@Manufacturer", manufacturer);
            cmd1.ExecuteNonQuery();

            SqlCommand cmd = new SqlCommand("exec add_product '"+name+"' ,'"+sprice+"' ,'"+pprice+"'  ,'"+cat+"','"+Convert.ToDateTime( mfg)+"','"+Convert.ToDateTime( exp)+"','"+nop+"','"+qp+"','"+leg+"','"+potency+"','"+discount+"','"+ ParameterDirection.Output+"'   ", con);
            
            //cmd.Parameters.AddWithValue("@SalePrice", sprice);
            //cmd.Parameters.AddWithValue("@PurchasePrice", pprice);
            //cmd.Parameters.AddWithValue("@Category", cat);
            //cmd.Parameters.AddWithValue("@ManufacturingDate", mfg);
            //cmd.Parameters.AddWithValue("@ExpiryDate", exp);
            //cmd.Parameters.AddWithValue("@Packs", nop);
            //cmd.Parameters.AddWithValue("@QuantityPerPack", qp);
            //cmd.Parameters.AddWithValue("@Legality", leg);
            //cmd.Parameters.AddWithValue("@Potency", potency);
            //cmd.Parameters.AddWithValue("@Discount", discount);
            //cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            
            string id = cmd.Parameters["@id"].Value.ToString();

            SqlCommand cmd2 = new SqlCommand("Insert into ProductSupplier values (@ProductID,@SupplierID)", con);
            cmd2.Parameters.AddWithValue("@ProductID",id );
            cmd2.Parameters.AddWithValue("@SupplierID", sid);
            cmd2.ExecuteNonQuery();


            MessageBox.Show("Successfully saved");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            loginForm.instance().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listpanel.Count; i++)
            {
                if (listpanel[i].Name == "panel1")
                {
                    listpanel[i].Show();
                }
                else
                {
                    listpanel[i].Hide();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listpanel.Count; i++)
            {
                if (listpanel[i].Name == "panel2")
                {
                    listpanel[i].Show();
                    listpanel[i].BringToFront();
                }
                else
                {
                    listpanel[i].Hide();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listpanel.Count; i++)
            {
                if (listpanel[i].Name == "panel3")
                {
                    listpanel[i].Show();
                    listpanel[i].BringToFront();
                }
                else
                {
                    listpanel[i].Hide();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listpanel.Count; i++)
            {
                if (listpanel[i].Name == "panel4")
                {
                    listpanel[i].Show();
                    listpanel[i].BringToFront();
                }
                else
                {
                    listpanel[i].Hide();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < listpanel.Count; i++)
            {
                if (listpanel[i].Name == "panel5")
                {
                    listpanel[i].Show();
                    listpanel[i].BringToFront();
                }
                else
                {
                    listpanel[i].Hide();
                }
            }
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            string id = comboBox9.Text;
            string qty = textBox34.Text;


            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UPDATE Product SET Packs='" + qty + "'+(Select Packs from Product where ID='" + id + "') where ID='" + id + "' ", con);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Stock updated successfuly");
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            int pid = int.Parse(comboBox8.Text);
            string pname = textBox33.Text;
            float sprice = float.Parse(textBox32.Text);
            float pprice = float.Parse(textBox31.Text);
            string category = comboBox7.Text;
            int cat = 0;
            if (category == "Medicine" || category=="1")
            {
                cat = 1;
            }
            else if (category == "Day-to-Day" || category=="2")
            {
                cat = 2;
            }

            //DateTime mfg = Convert.ToDateTime(bunifuDatePicker4.Text);
            //DateTime exp = Convert.ToDateTime(bunifuDatePicker3.Text);
            float nop = float.Parse(textBox30.Text);
            int qty = int.Parse(textBox29.Text);
            int legal = int.Parse(comboBox6.Text);
            float potency = float.Parse(textBox28.Text);
            float disc = float.Parse(textBox27.Text);
            int sid = int.Parse(comboBox5.Text);
            

            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("exec sp_updateProduct '"+pid+"','"+pname+"','"+sprice+"','"+pprice+"','"+cat+"','"+nop+"','"+qty+"','"+legal+"','"+potency+"','"+disc+"' ", con);

            cmd.ExecuteNonQuery();
            SqlCommand cmd1 = new SqlCommand("UPDATE ProductSupplier SET SupplierID='" + sid + "'  where ProductID='" + pid + "' ", con);

            cmd1.ExecuteNonQuery();
            MessageBox.Show("Product Data updated Successfully");
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            try
            {
                int eid = int.Parse(comboBox4.Text);
                string uname = textBox25.Text;
                string pin = textBox24.Text;
                string fname = textBox23.Text;
                string lname = textBox22.Text;
                string cnic = (textBox21.Text);
                string adress = textBox20.Text;
                string phone = textBox19.Text;
                string email = textBox18.Text;

                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("UPDATE Employee SET Username='" + uname + "',PIN='" + pin + "',FirstName='" + fname + "',LastName='" + lname + "',CNIC='" + cnic + "',Address='" + adress + "',Phone='" + phone + "',Email='" + email + "'  where ID='" + eid + "' ", con);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Succesfully updated employee data");
            }
            catch
            {
                MessageBox.Show("Error while updating employee data!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listpanel.Count; i++)
            {
                if (listpanel[i].Name == "panel6")
                {
                    listpanel[i].Show();
                    listpanel[i].BringToFront();
                }
                else
                {
                    listpanel[i].Hide();
                }
            }
        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(comboBox10.Text);
                string name = textBox36.Text;

                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Delete from Employee where ID='" + id + "' ", con);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee deleted successfully");
            }
            catch
            {
                MessageBox.Show("Error occured while deleting employee!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listpanel.Count; i++)
            {
                if (listpanel[i].Name == "panel7")
                {
                    listpanel[i].Show();
                    listpanel[i].BringToFront();
                }
                else
                {
                    listpanel[i].Hide();
                }
            }
        }

        private void bunifuButton9_Click(object sender, EventArgs e)
        {
            int id = int.Parse(comboBox11.Text);
            string name = textBox37.Text;

            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("exec delete_product'" + id + "' ", con);

            cmd.ExecuteNonQuery();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listpanel.Count; i++)
            {
                if (listpanel[i].Name == "panel8")
                {
                    listpanel[i].Show();
                    listpanel[i].BringToFront();
                }
                else
                {
                    listpanel[i].Hide();
                }
            }
        }

        private void bunifuPictureBox1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listpanel.Count; i++)
            {
                if (listpanel[i].Name == "panel9")
                {
                    listpanel[i].Show();
                    listpanel[i].BringToFront();
                }
                else
                {
                    listpanel[i].Hide();
                    listpanel[i].SendToBack();
                }
            }
        }

        private void bunifuButton14_Click(object sender, EventArgs e)
        {
            string name = textBox38.Text;
            string type = comboBox14.Text;
            string phone = textBox39.Text;
            string adress = textBox40.Text;

            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Exec add_supplier '" + name + "','" + type + "','" + phone + "','" + adress + "' ", con);

            cmd.ExecuteNonQuery();


            panel9.Hide();
            panel2.Show();
            panel2.BringToFront();
        }

        private void bunifuButton13_Click(object sender, EventArgs e)
        {
            report4 report4 = new report4();
            report4.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuButton12_Click(object sender, EventArgs e)
        {
            report3 report3 = new report3();
            report3.Show();
        }

        private void bunifuButton11_Click(object sender, EventArgs e)
        {
            report2 report2 = new report2();
            report2.Show();
        }

        private void bunifuButton10_Click(object sender, EventArgs e)
        {
            string months = comboBox12.Text;
            string year = comboBox13.Text;
            int month=0;
            
            ArrayList month_list = new ArrayList();
            month_list.Add("January");
            month_list.Add("February");
            month_list.Add("March");
            month_list.Add("April");
            month_list.Add("May");
            month_list.Add("June");
            month_list.Add("July");
            month_list.Add("August");
            month_list.Add("September");
            month_list.Add("October");
            month_list.Add("November");
            month_list.Add("December");

            for(int i=1;i<month_list.Count;i++)
            {
                if((string)month_list[i]==months)
            for (int i = 1; i < month_list.Count; i++)
            {
                if ((string)month_list[i] == months)
                {
                    month = i;
                }
            }

            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Exec calculate_profit '" + month + "','" +year + "' ", con);
            SqlCommand cmd = new SqlCommand("Exec calculate_profit '" + month + "','" + year + "' ", con);

            cmd.ExecuteNonQuery();
            report1 report1 = new report1();
            report1.Show();

        }

        private void check_username(object sender, EventArgs e)
        {
            DataView dv = searchedDataTable.DefaultView;
            dv.RowFilter = "Username LIKE '" + textBox25.Text + "%'";
            dataGridView1.DataSource = dv;
        }

        private void dgv_cellclick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            comboBox4.Text = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            textBox25.Text = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
            textBox24.Text= dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            textBox23.Text = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
            textBox22.Text = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
            textBox21.Text = dataGridView1.Rows[rowIndex].Cells[5].Value.ToString();
            textBox20.Text = dataGridView1.Rows[rowIndex].Cells[6].Value.ToString();
            textBox19.Text = dataGridView1.Rows[rowIndex].Cells[7].Value.ToString();
            textBox18.Text = dataGridView1.Rows[rowIndex].Cells[8].Value.ToString();

        }

        private void search_name(object sender, EventArgs e)
        {
            DataView dv2 = nameTable.DefaultView;
            dv2.RowFilter = "Username LIKE '" + textBox36.Text + "%'";
            dataGridView2.DataSource = dv2;
        }

        private void search_user(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = dataGridView2.CurrentCell.RowIndex;
            comboBox10.Text = dataGridView2.Rows[rowIndex].Cells[0].Value.ToString();
            textBox36.Text = dataGridView2.Rows[rowIndex].Cells[1].Value.ToString();
        }

        private void pName_changed(object sender, EventArgs e)
        {
           
            DataView dv1 = pname_change.DefaultView;
            dv1.RowFilter = "ProductName LIKE '" + textBox33.Text + "%'";
            dataGridView3.DataSource = dv1;
        }

        private void pName_search(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = dataGridView3.CurrentCell.RowIndex;
            comboBox8.Text = dataGridView3.Rows[rowIndex].Cells[0].Value.ToString();
            textBox33.Text = dataGridView3.Rows[rowIndex].Cells[1].Value.ToString();
            textBox32.Text = dataGridView3.Rows[rowIndex].Cells[2].Value.ToString();
            textBox31.Text = dataGridView3.Rows[rowIndex].Cells[3].Value.ToString();
            comboBox7.Text = dataGridView3.Rows[rowIndex].Cells[4].Value.ToString();
            //bunifuDatePicker4.Value = (DateTime)dataGridView3.Rows[rowIndex].Cells[5].Value;
            //bunifuDatePicker3.Value = (DateTime)dataGridView3.Rows[rowIndex].Cells[6].Value;
            textBox30.Text = dataGridView3.Rows[rowIndex].Cells[5].Value.ToString();
            textBox29.Text = dataGridView3.Rows[rowIndex].Cells[6].Value.ToString();
            comboBox6.Text = dataGridView3.Rows[rowIndex].Cells[7].Value.ToString();
            textBox28.Text = dataGridView3.Rows[rowIndex].Cells[8].Value.ToString();
            textBox27.Text = dataGridView3.Rows[rowIndex].Cells[9].Value.ToString();
            comboBox5.Text= dataGridView3.Rows[rowIndex].Cells[10].Value.ToString();
        }

        private void pName_Changed(object sender, EventArgs e)
        {
            DataView dv2 = pname_change1.DefaultView;
            dv2.RowFilter = "ProductName LIKE '" + textBox37.Text + "%'";
            dataGridView4.DataSource = dv2;
        }

        private void dg_click(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = dataGridView4.CurrentCell.RowIndex;
            comboBox11.Text = dataGridView4.Rows[rowIndex].Cells[0].Value.ToString();
            textBox37.Text = dataGridView4.Rows[rowIndex].Cells[1].Value.ToString();
        }

        private void name_change(object sender, EventArgs e)
        {
            DataView dv3 = pname_change2.DefaultView;
            dv3.RowFilter = "ProductName LIKE '" + textBox35.Text + "%'";
            dataGridView4.DataSource = dv3;
        }

        private void cell_click(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = dataGridView5.CurrentCell.RowIndex;
            comboBox9.Text = dataGridView5.Rows[rowIndex].Cells[0].Value.ToString();
            textBox35.Text = dataGridView5.Rows[rowIndex].Cells[1].Value.ToString();
           // textBox34.Text = dataGridView5.Rows[rowIndex].Cells[2].Value.ToString();
        }
    }
}
