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
    public partial class IssueBooks : Form
    {
        public IssueBooks()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are Your Sure!", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)==DialogResult.OK)
            {
                this.Close();
            }
        }

        private void IssueBooks_Load(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection(@"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewBook; Integrated Security=True"))
            {
                string query = "SELECT bName FROM AddBook";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxBookName.Items.Add(reader["bName"].ToString());
                        }
                        reader.Close();
                    }
                }
                sqlConnection.Close();
            }
        }

        int count;
        private void btnSearchSt_Click(object sender, EventArgs e)
        {
            if (txtSearchSt.Text != "")
            {
                string eid = txtSearchSt.Text;
                string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; User Id=MyUser; Password=MyPassword;";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    // Query to get student details
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM newStudentTable WHERE enroll = @enroll", sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@enroll", eid);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        dataAdapter.Fill(ds);

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            txtStName.Text = ds.Tables[0].Rows[0]["sname"].ToString();
                            txtDepartment.Text = ds.Tables[0].Rows[0]["dep"].ToString();
                            txtStSem.Text = ds.Tables[0].Rows[0]["sem"].ToString();
                            txtStCont.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                            txtStEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                        }
                        else
                        {
                            txtStName.Clear();
                            txtDepartment.Clear();
                            txtStSem.Clear();
                            txtStCont.Clear();
                            txtStEmail.Clear();
                            MessageBox.Show("Invalid Enrollment No", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Query to count issued books
                    using (SqlCommand cmdCount = new SqlCommand("SELECT COUNT(std_enroll) FROM IRBook WHERE std_enroll = @enroll AND book_return_date IS NULL", sqlConnection))
                    {
                        cmdCount.Parameters.AddWithValue("@enroll", eid);
                        count = (int)cmdCount.ExecuteScalar();
                    }
                }

                MessageBox.Show($"Number of books issued: {count}", "Book Count", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnIssueBook_Click(object sender, EventArgs e)
        {
            if (txtStName.Text != "")
            {
                if (comboBoxBookName.SelectedIndex != -1 && count <= 2)
                {
                    string enroll = txtSearchSt.Text;
                    string sname = txtStName.Text;
                    string dep = txtDepartment.Text;
                    string sem = txtStSem.Text;
                    Int64 contact = Int64.Parse(txtStCont.Text);
                    string email = txtStEmail.Text;
                    string bookname = comboBoxBookName.Text;
                    string bookIssueDate = dateTimePicker1.Text;
                    string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=Login; User Id=MyUser; Password=MyPassword;";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        // Using parameterized SQL command to prevent SQL injection
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO IRBook (std_enroll, std_name, std_dep, std_sem, std_contact, std_email, book_name, book_issue_date) VALUES (@enroll, @sname, @dep, @sem, @contact, @email, @bookname, @bookIssueDate)", con))
                        {
                            // Adding parameters
                            cmd.Parameters.AddWithValue("@enroll", enroll);
                            cmd.Parameters.AddWithValue("@sname", sname);
                            cmd.Parameters.AddWithValue("@dep", dep);
                            cmd.Parameters.AddWithValue("@sem", sem);
                            cmd.Parameters.AddWithValue("@contact", contact);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@bookname", bookname);
                            cmd.Parameters.AddWithValue("@bookIssueDate", bookIssueDate);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Book Issued", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Select Book. OR Maximum number of Book Has been ISSUED", "No Book Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Enter Valid Enrollement No ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearchSt_TextChanged(object sender, EventArgs e)
        {
            if(txtSearchSt.Text == "")
            {
                txtStName.Clear();
                txtDepartment.Clear();
                txtStSem.Clear();
                txtStEmail.Clear(); 
                txtStCont.Clear();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearchSt.Clear();
        }
    }
}
