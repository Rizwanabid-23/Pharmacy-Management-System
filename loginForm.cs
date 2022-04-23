using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace Multicare_pharmacy
{
    public partial class loginForm : Form
    {
        public loginForm()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {

                string username = textBox1.Text;
                string pin = textBox2.Text;
                List<string> check = checker(username, pin);
                if (check[0] == "admin")
                {
                    admins admin = new admins();
                    
                    admin.Show();
                    
                    this.Hide();
                }
                else if (check[0] == "employee")
                {
                    Sales sales = new Sales(check[1], check[2]);
                    sales.Show();
                }
                else
                {
                    MessageBox.Show("Login parameters not correct");
                }
            }
            catch
            {
                MessageBox.Show("Error occured while logging in. Try again");
            }
            clearFields();
        }
        private List<string> checker(string uname, string pin)
        {
            List<string> credentials = new List<string>();
            var con = Configuration.getInstance().getConnection();
            SqlDataAdapter cmd = new SqlDataAdapter("select count(*) from Admin where Username='" + uname + "' and PIN='" + pin + "'", con);
            DataTable dt = new DataTable();
            cmd.Fill(dt);

            if (dt.Rows[0][0].ToString() == "1")
            {
                credentials.Add("admin");
                credentials.Add("admin");
                credentials.Add("123");
                return credentials;
            }

            else
            {
                SqlDataAdapter cmd1 = new SqlDataAdapter("select * from Employee where Username='" + uname + "' and PIN='" + pin + "'", con);

                DataTable dt1 = new DataTable();
                cmd1.Fill(dt1);

                if (dt1.Rows.Count == 1)
                {
                    DataRow dataRow = dt1.Rows[0];
                    credentials.Add("employee");
                    credentials.Add(dataRow.ItemArray[0].ToString());
                    credentials.Add(dataRow.ItemArray[3].ToString());
                    return credentials;
                }
            }
            credentials.Add("false");
            credentials.Add("null");
            credentials.Add("null");
            return credentials;
        }

        public void clearFields()
        {
            this.textBox1.Text = String.Empty;
            this.textBox2.Text = String.Empty;
        }
    }
}
