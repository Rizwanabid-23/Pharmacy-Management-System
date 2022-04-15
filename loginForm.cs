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
                string check = checker(username, pin);
                if( check== "admin")
                {
                        admins admin = new admins();
                        admin.Show();
                        this.Hide();   
                }
                else if(check=="employee")
                {

                    employee employee = new employee();
                    employee.Show();
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
        }
        private string checker(string uname,string pin)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(local);Initial Catalog=MultiCarePharmacy;Integrated Security=True");

            SqlDataAdapter cmd = new SqlDataAdapter("select count(*) from Admin where Username='"+uname+"' and PIN='"+pin+"'",con);

            DataTable dt = new DataTable();
            cmd.Fill(dt);

            if (dt.Rows[0][0].ToString() == "1")
            {
                return "admin";
            }
            else
            {
                SqlDataAdapter cmd1 = new SqlDataAdapter("select count(*) from Employee where Username='" + uname + "' and PIN='" + pin + "'", con);

                DataTable dt1 = new DataTable();
                cmd1.Fill(dt1);

                if (dt1.Rows[0][0].ToString() == "1")
                {
                    return "employee";
                }
            }

            return "false";

        }
    }
}
