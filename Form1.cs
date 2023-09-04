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
using System.Data.SqlClient;

namespace cmpg223_project
{
    public partial class Login : Form
    {



        SqlConnection conn;
        SqlDataAdapter adapter;
        SqlDataReader reader;
        SqlCommand command;

        public static int LoggedInEmployeeID { get; private set; }
        // Method to set the logged-in employee ID when authentication succeeds
        private void SetLoggedInEmployeeID(int employeeID)
        {
            LoggedInEmployeeID = employeeID;
        }
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (adminCheckBox.Checked || userIDTextBox.Text == "1992" || passTextBox.Text == "DevTracker")
            {
                Form3 admin = new Form3();
                admin.ShowDialog();
            }
            else if (client.Checked)
            {

            }
            else if (Employee.Checked)
            {
                FrmDeveloper employee = new FrmDeveloper();
                employee.ShowDialog();
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
            registration reg = new registration();
            reg.ShowDialog();
            this.Close();
        }
    }
}
