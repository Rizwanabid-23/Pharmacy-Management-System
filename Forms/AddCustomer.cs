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

namespace Multicare_pharmacy.Forms
{
    public partial class AddCustomer : Form
    {
        public AddCustomer()
        {
            InitializeComponent();
        }

        private void closeBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddBTN_Click(object sender, EventArgs e)
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand("INSERT INTO Customer VALUES (@Name, @Phone, @Address)", connection);
            try
            {
                command.Parameters.AddWithValue("@Name", custName.Text);
                command.Parameters.AddWithValue("@Phone", custPhone.Text);
                command.Parameters.AddWithValue("@Address", custAddress.Text);
                command.ExecuteNonQuery();
                clearFields();
                MessageBox.Show("Customer added to the system successfully");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void clearFields()
        {
            custName.Text = String.Empty;
            custPhone.Text = String.Empty;
            custAddress.Text = String.Empty;
        }
    }
}
