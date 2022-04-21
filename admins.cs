using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multicare_pharmacy
{
    public partial class admins : Form
    {
        List<Panel> listpanel = new List<Panel>();
        public admins()
        {
            InitializeComponent();
        }

        private void admins_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'multiCarePharmacyDataSet.Product' table. You can move, or remove it, as needed.
            //this.productTableAdapter.Fill(this.multiCarePharmacyDataSet.Product);
            // TODO: This line of code loads data into the 'multiCarePharmacyDataSet.Employee' table. You can move, or remove it, as needed.
            //this.employeeTableAdapter.Fill(this.multiCarePharmacyDataSet.Employee);
            //this.supplierTableAdapter.Fill(this.multiCarePharmacyDataSet.Supplier);
            listpanel.Add(panel1);
            listpanel.Add(panel2);
            listpanel.Add(panel3);
            listpanel.Add(panel4);
            listpanel.Add(panel5);

            listpanel[1].Show();
            listpanel[1].BringToFront();
        }

        private void addEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void addProductsToolStripMenuItem_Click(object sender, EventArgs e)
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
            int id = int.Parse(textBox1.Text);
            string uname = textBox2.Text;
            string pin = textBox3.Text;
            string fname = textBox4.Text;
            string lname = textBox5.Text;
            string cnic = (textBox6.Text);
            string adress = textBox7.Text;
            string phone = textBox8.Text;
            string email = textBox9.Text;

            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Insert into Employee values (@ID,@Username,@PIN,@FirstName,@LastName,@CNIC,@Address,@Phone,@Email)", con);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Username", uname);
            cmd.Parameters.AddWithValue("@PIN", pin);
            cmd.Parameters.AddWithValue("@FirstName", fname);
            cmd.Parameters.AddWithValue("@LastName", lname);
            cmd.Parameters.AddWithValue("@CNIC", cnic);
            cmd.Parameters.AddWithValue("@Address", adress);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Email", email);
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
            DateTime mfg = Convert.ToDateTime(bunifuDatePicker1.Text);
            DateTime exp = Convert.ToDateTime(bunifuDatePicker2.Text);

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
            SqlCommand cmd = new SqlCommand("Insert into Product values (@ProductName,@SalePrice,@PurchasePrice,@Category,@ManufacturingDate,@ExpiryDate,@Packs,@QuantityPerPack,@Legality,@Potency,@Discount)", con);
            cmd.Parameters.AddWithValue("@ProductName", name);
            cmd.Parameters.AddWithValue("@SalePrice", sprice);
            cmd.Parameters.AddWithValue("@PurchasePrice", pprice);
            cmd.Parameters.AddWithValue("@Category", cat);
            cmd.Parameters.AddWithValue("@ManufacturingDate", mfg);
            cmd.Parameters.AddWithValue("@ExpiryDate", exp);
            cmd.Parameters.AddWithValue("@Packs", nop);
            cmd.Parameters.AddWithValue("@QuantityPerPack", qp);
            cmd.Parameters.AddWithValue("@Legality", leg);
            cmd.Parameters.AddWithValue("@Potency", potency);
            cmd.Parameters.AddWithValue("@Discount", discount);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            loginForm loginForm = new loginForm();
            loginForm.Show();
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
    }
}
