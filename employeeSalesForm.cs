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
    public partial class employeeSalesForm : Form
    {
        int employeeID;
        public employeeSalesForm(int empID)
        {
            InitializeComponent();
            this.employeeID = empID;
        }

        private void report6_Load(object sender, EventArgs e)
        {
            employeeSales EmployeeSales = new employeeSales();
            EmployeeSales.SetParameterValue("employeeID", employeeID);
            crystalReportViewer1.ReportSource = EmployeeSales;
            crystalReportViewer1.Refresh();
        }
    }
}
