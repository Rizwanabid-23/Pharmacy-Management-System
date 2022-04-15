using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multicare_pharmacy
{
    public partial class admins : Form
    {
        public admins()
        {
            InitializeComponent();
        }

        private void admins_Load(object sender, EventArgs e)
        {

        }

        private void addEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //uc_addEmployee.Visible = true;
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            foreach (var c in this.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = String.Empty;
                }
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //int id = int.Parse(bunifuTextBox1.Text);
                //string uname = bunifuTextBox2.Text;
                //string pin = bunifuTextBox3.Text;
                //string adress = bunifuTextBox4.Text;
                //string firstname = bunifuTextBox5.Text;
                //string lastname = bunifuTextBox6.Text;
                //string cnic = bunifuTextBox7.Text;
                //string phone = bunifuTextBox8.Text;
                //string email = bunifuTextBox9.Text;
            }
            catch
            {
                MessageBox.Show("Unable to add data");
            }
        }

        private void uc_addemployee1_Load(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch
            {

            }
        }
    }
}
