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
    public partial class ViewStudentInformation : Form
    {
        public ViewStudentInformation()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }


        int stuid;
        Int64 rowId;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                stuid = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            panel1.Visible = true;

            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; User Id=MyUser; Password=MyPassword;";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;

            cmd.CommandText = "select * from newStudentTable where stuid = @stuid";
            cmd.Parameters.AddWithValue("@stuid", stuid);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);  // Associate the SqlCommand with SqlDataAdapter
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            rowId = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            txtStName.Text = ds.Tables[0].Rows[0][1].ToString();
            txtErollment.Text = ds.Tables[0].Rows[0][2].ToString();
            txtDepartment.Text = ds.Tables[0].Rows[0][3].ToString();
            txtStSem.Text = ds.Tables[0].Rows[0][4].ToString();
            txtStCont.Text = ds.Tables[0].Rows[0][5].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0][6].ToString();
        }

        private void ViewStudentInformation_Load(object sender, EventArgs e)
        {
            panel1.Visible = false; 
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; User Id=MyUser; Password=MyPassword;";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;

            cmd.CommandText = "select * from newStudentTable";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);  // Associate the SqlCommand with SqlDataAdapter
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }

        private void txtRefresh_TextChanged(object sender, EventArgs e)
        {
            if (txtRefresh.Text != "")
            {
                pictureBox1.Image = Properties.Resources.StudentSearshImage;
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; User Id=MyUser; Password=MyPassword;";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection;

                cmd.CommandText = "select * from newStudentTable where sname LIKE '" + txtRefresh.Text + "%'";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);  // Associate the SqlCommand with SqlDataAdapter
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];
            }
            else
            {
                pictureBox1.Image = Properties.Resources.ViewStudentImage;
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; User Id=MyUser; Password=MyPassword;";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection;

                cmd.CommandText = "select * from newStudentTable";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);  // Associate the SqlCommand with SqlDataAdapter
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtRefresh.Clear();
            panel1.Visible = false;
        }

        private void btnUpdata_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Data Will Be Updated. Confirm!", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string stName = txtStName.Text;
                string erollment = txtErollment.Text;
                string department = txtDepartment.Text;
                string stSem = txtStSem.Text;
                Int64 stCont = Int64.Parse(txtStCont.Text);
                string email = txtEmail.Text;

                string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; User Id=MyUser; Password=MyPassword;";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    string query = "UPDATE newStudentTable SET sname = @stName, enroll = @erollment, dep = @department, sem = @stSem, contact = @stCont, email = @email WHERE stuid = @stuid";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        // Use parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@stName", stName);
                        cmd.Parameters.AddWithValue("@erollment", erollment);
                        cmd.Parameters.AddWithValue("@department", department);
                        cmd.Parameters.AddWithValue("@stSem", stSem);
                        cmd.Parameters.AddWithValue("@stCont", stCont);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@stuid", rowId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Data Will Be Updated. Confirm!", "Confirmation Dialog!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                using (SqlConnection sqlConnection = new SqlConnection(@"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; Integrated Security=True"))
                {
                    sqlConnection.Open();

                    string query = "DELETE FROM newStudentTable WHERE stuid = @stuid";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        // Parameterize the query to prevent SQL injection
                        cmd.Parameters.AddWithValue("@stuid", rowId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book record deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Delete failed. Please check if the record exists.");
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("UnSaved Data Will be Lost.", "Are You Sure!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
