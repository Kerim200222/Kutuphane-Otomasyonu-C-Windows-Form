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
    public partial class ReturnBook : Form
    {
        public ReturnBook()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string bname;
        string bDate;
        Int64 rowId;
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            panel2.Visible = true;

            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                rowId= Int64.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                bname = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                bDate = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            }

            txtBookName.Text = bname;
            txtBookIssueDate.Text = bDate;
        }

        private void ReturnBook_Load(object sender, EventArgs e)
        {
            panel2.Visible = false;
            txtSearchSt.Clear();
        }

        private void txtSearchSt_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchSt.Text == "")
            {
                panel2.Visible=false;
                dataGridView1.DataSource=null;
            }
        }
        
        private void btnSearchSt_Click(object sender, EventArgs e)
        {
            if(txtSearchSt.Text != "" )
            {
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; User Id=MyUser; Password=MyPassword;";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection;

                cmd.CommandText = "select * from IRBook where std_enroll LIKE '" + txtSearchSt.Text + "' and book_return_date IS NULL ";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);  // Associate the SqlCommand with SqlDataAdapter
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                }
                else
                {
                    MessageBox.Show("Invalid ID or No Book Issued", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearchSt.Clear();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; User Id=MyUser; Password=MyPassword;";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string query = "UPDATE IRBook SET book_return_date = @ReturnDate WHERE std_enroll = @StudentEnroll AND id = @RowId";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@ReturnDate", dateTimePicker1.Value); // Use Value instead of Text for DateTime objects
                    cmd.Parameters.AddWithValue("@StudentEnroll", txtSearchSt.Text);
                    cmd.Parameters.AddWithValue("@RowId", rowId);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }

            MessageBox.Show("Return Successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ReturnBook_Load(this, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void btnSearchSt2_Click(object sender, EventArgs e)
        {
            this.Hide();
            CompleteBookDetail completeBookDetail = new CompleteBookDetail();
            completeBookDetail.Show();
        }
    }
}
