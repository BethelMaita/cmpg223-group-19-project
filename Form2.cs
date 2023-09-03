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
        SqlDataReader read;
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

        private void btnLogOut_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int loggedInEmployeeID = Login.LoggedInEmployeeID;
            // Get the current system month
            int currentMonth = DateTime.Now.Month;

            try
            {
                SqlConnection conn = new SqlConnection(conStr);                
                    conn.Open();
                    // Define a SQL query to retrieve upcoming projects for the logged-in employee
                    string reportQuery = "SELECT P.ProjectID, P.ProjectDescription, C.ClientCompanyName, " +
                                         "PH.PhaseName, PS.ScheduleStartDate, PS.ScheduleDueDate " +
                                         "FROM PROJECTS P " +
                                         "INNER JOIN CLIENTS C ON P.ClientID = C.ClientID " +
                                         "INNER JOIN PROJECTSCHEDULES PS ON P.ScheduleID = PS.ScheduleID " +
                                         "INNER JOIN PHASES PH ON P.PhaseID = PH.PhaseID " +
                                         "INNER JOIN PROJECTASSIGNMENTS PA ON P.AssignmentID = PA.AssignmentID " +
                                         "WHERE MONTH(PS.ScheduleDueDate) = @CurrentMonth " +
                                         "AND PA.EmployeeID = @EmployeeID";

                SqlCommand cmd = new SqlCommand(reportQuery, conn);
                        cmd.Parameters.AddWithValue("@CurrentMonth", currentMonth);
                        cmd.Parameters.AddWithValue("@EmployeeID", loggedInEmployeeID);
                SqlDataReader read = cmd.ExecuteReader();
                  
                            // Clear the RichTextBox
                            rtbReport.Clear();
                            // Build and display the report header
                            rtbReport.AppendText($"UPCOMING PROJECTS FOR Employee: {loggedInEmployeeID}\n");
                            rtbReport.AppendText("----------------------------------------------------------------\n");
                            rtbReport.AppendText($"Dear {GetEmployeeName(loggedInEmployeeID)}, here is a list of your upcoming due projects for {DateTime.Now.ToString("MMMM")}:\n\n");
                            // Iterate through the data and add projects to the report
                            int projectCounter = 1;
                            while (read.Read())
                            {
                                int projectID = read.GetInt32(0);
                                string projectDescription = read.GetString(1);
                                string clientCompanyName = read.GetString(2);
                                string phaseName = read.GetString(3);
                                DateTime startDate = read.GetDateTime(4);
                                DateTime dueDate = read.GetDateTime(5);

                                string projectInfo = $"{projectCounter}.) Project: {projectID}\n" +
                                                     $"\t{projectDescription} for {clientCompanyName}.\n" +
                                                     $"\tCurrently in phase {phaseName}.\n" +
                                                     $"\tDate started: {startDate}.\n" +
                                                     $"\tDate due: {dueDate}.\n\n";

                                rtbReport.AppendText(projectInfo);
                                projectCounter++;
                            }
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string GetEmployeeName(int employeeID)
        {
            try
            {
                SqlConnection conn = new SqlConnection(conStr);
                    conn.Open();
                    string query = "SELECT employeeFirstName, employeeLastName FROM EMPLOYEES WHERE employeeID = @EmployeeID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                SqlDataReader read = cmd.ExecuteReader();
                            if (read.Read())
                            {
                                string firstName = read.GetString(0);
                                string lastName = read.GetString(1);
                                return $"{firstName} {lastName}";
                            }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching the employee name: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return "EmployeeNamePlaceholder";
        }
    }
}
    

        
    

