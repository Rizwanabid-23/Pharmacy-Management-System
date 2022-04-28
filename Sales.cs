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
    public partial class Sales : Form
    {
        Forms.SalesPanel screen01;
        Forms.SalesPanel screen02;
        Forms.SalesPanel screen03;
        Forms.AddCustomer addCustomerInstance = Forms.AddCustomer.instance();
        private Form currentPanel;
        public Sales(string EID, string EName)
        {
            InitializeComponent();
            screen01 = new Forms.SalesPanel("Session 01", EID, EName);
            screen02 = new Forms.SalesPanel("Session 02", EID, EName);
            screen03 = new Forms.SalesPanel("Session 03", EID, EName);
        }

        private void openNextPanel(Form nextPanel, object sender)
        {
            if (currentPanel != null)
            {
                currentPanel.Hide();
            }
            currentPanel = nextPanel;
            nextPanel.TopLevel = false;
            nextPanel.FormBorderStyle = FormBorderStyle.None;
            nextPanel.Dock = DockStyle.Fill;
            mainContentPanel.Controls.Add(nextPanel);
            mainContentPanel.Tag = nextPanel;
            nextPanel.BringToFront();
            nextPanel.Show();
        }
        private void Sales_Load(object sender, EventArgs e)
        {
            openNextPanel(screen01, sender);
        }

        private void tab01_Click(object sender, EventArgs e)
        {
            openNextPanel(screen01, sender);
        }

        private void tab02_Click(object sender, EventArgs e)
        {
            openNextPanel(screen02, sender);
        }

        private void tab03_Click(object sender, EventArgs e)
        {
            openNextPanel(screen03, sender);
        }

        private void signOutButton_Click(object sender, EventArgs e)
        {
            addCustomerInstance.Hide();
            this.Close();
            loginForm.instance().Show();
        }
    }
}