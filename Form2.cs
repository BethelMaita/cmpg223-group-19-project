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

namespace cmpg223_project
{
    public partial class FrmDeveloper : Form
    {
        private string conStr = "YourConnectionString"; // Replace with your actual connection string
        SqlConnection conn;
        SqlDataAdapter adap;
        SqlCommand comm;
        DataSet ds;

        public FrmDeveloper()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExit2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int phaseIdToUpdate;
            if (!int.TryParse(tbProjectDateToUpdate.Text, out phaseIdToUpdate))
            {
                MessageBox.Show("Invalid Phase ID. Please enter a valid Phase ID.");
                return;
            }

            DateTime startDate = dtmStart.Value;
            DateTime dueDate = dtmDue.Value;

            // Ask for confirmation before updating
            DialogResult result = MessageBox.Show($"Are you sure you want to update dates for Phase ID {phaseIdToUpdate}?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return; // User canceled the update
            }

            try
            {
                SqlConnection conn = new SqlConnection(conStr);
                conn.Open();
                 // Define the SQL update query
                 string updateQuery = "UPDATE PROJECTSCHEDULES SET ScheduleStartDate = @StartDate, ScheduleDueDate = @DueDate " +
                                         "WHERE PhaseID = @PhaseID";

                        SqlCommand cmd = new SqlCommand(updateQuery, conn);
                        // Add parameters to the SQL query
                        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate;
                        cmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = dueDate;
                        cmd.Parameters.Add("@PhaseID", SqlDbType.Int).Value = phaseIdToUpdate;

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Update successful!");
                        }
                        else
                        {
                            MessageBox.Show("No records updated. Phase ID not found.");
                        }
                        
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
        
    

