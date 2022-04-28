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
        private static loginForm loginFormInstance;
        
        public loginForm()
        {
            InitializeComponent();
        }
        public static loginForm instance()
        {
            if (loginFormInstance == null)
            {
                loginFormInstance = new loginForm();
            }
            return loginFormInstance;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {

                string username = TextBox1.Text;
                string pin = TextBox2.Text;
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
                    this.Hide();
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
            this.TextBox1.Text = String.Empty;
            this.TextBox2.Text = String.Empty;
        }

        private void closeBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                bunifuButton1_Click(sender, e);
            }
        }
    }
}
