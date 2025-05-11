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

namespace Kutuphane_Otomasyonu
{
    public partial class AddStudent : Form
    {
        public AddStudent()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtStudentName.Text != "" && txtErollmentNo.Text != "" && txtDepartment.Text != "" && txtStudentSem.Text != "" && txtStudentCont.Text != "" && txtEmail.Text != "")
            {
                string sname = txtStudentName.Text;
                string enroll = txtErollmentNo.Text;
                string dep = txtDepartment.Text;
                string sem = txtStudentSem.Text;
                Int64 mobile = Int64.Parse(txtStudentCont.Text);
                string email = txtEmail.Text;
                string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; User Id=MyUser; Password=MyPassword;";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO newStudentTable (sname, enroll, dep, sem, contact, email) VALUES (@sname, @enroll, @dep, @sem, @mobile, @email)", sqlConnection);

                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@sname", sname);
                    cmd.Parameters.AddWithValue("@enroll", enroll);
                    cmd.Parameters.AddWithValue("@dep", dep);
                    cmd.Parameters.AddWithValue("@sem", sem);
                    cmd.Parameters.AddWithValue("@mobile", mobile);
                    cmd.Parameters.AddWithValue("@email", email);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                }

                MessageBox.Show("Data Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtStudentName.Clear();
                txtStudentSem.Clear();
                txtStudentCont.Clear();
                txtErollmentNo.Clear();
                txtDepartment.Clear();
                txtEmail.Text = "";
            }
            else
            {
                MessageBox.Show("Please Fill Empty Fields", "Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtStudentName.Clear();
            txtStudentSem.Clear();
            txtStudentCont.Clear();
            txtErollmentNo.Clear();
            txtDepartment.Clear();
            txtEmail.Text = "";
        }

        private void AddStudent_Load(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {

        }
    }
}
