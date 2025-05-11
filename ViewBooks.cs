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
    public partial class ViewBooks : Form
    {
        public ViewBooks()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will CANCEL Your UnSaved DATA", "Are You Sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.Close();
                panel3.Visible = false;
            }
        }

        private void ViewBooks_Load(object sender, EventArgs e)
        {
            panel3.Visible=false;
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewBook; User Id=MyUser; Password=MyPassword;";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;

            cmd.CommandText = "select * from AddBook";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);  // Associate the SqlCommand with SqlDataAdapter
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }
        int bid;
        Int64 rowId;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                bid = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            panel3.Visible = true;

            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewBook; User Id=MyUser; Password=MyPassword;";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;

            cmd.CommandText = "select * from AddBook where bid = @bid";
            cmd.Parameters.AddWithValue("@bid", bid);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);  // Associate the SqlCommand with SqlDataAdapter
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            rowId = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            txtBookName.Text = ds.Tables[0].Rows[0][1].ToString();
            txtBookAuthor.Text = ds.Tables[0].Rows[0][2].ToString();
            txtBookPub.Text = ds.Tables[0].Rows[0][3].ToString();
            txtPurchase.Text = ds.Tables[0].Rows[0][4].ToString();
            txtBookPrice.Text = ds.Tables[0].Rows[0][5].ToString();
            txtBookQuantity.Text = ds.Tables[0].Rows[0][6].ToString();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (txtRefresh.Text != "")
            {
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewBook; User Id=MyUser; Password=MyPassword;";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection;

                cmd.CommandText = "select * from AddBook where bName LIKE '" + txtRefresh.Text + "%'";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);  // Associate the SqlCommand with SqlDataAdapter
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewBook; User Id=MyUser; Password=MyPassword;";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection;

                cmd.CommandText = "select * from AddBook";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);  // Associate the SqlCommand with SqlDataAdapter
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtRefresh.Clear();
            panel3.Visible = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Data Will Be Updated. Confirm!", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string bname = txtBookName.Text;
                string bauthor = txtBookAuthor.Text;
                string bPublication = txtBookPub.Text;
                string bData = txtPurchase.Text;
                string bPrice = txtBookPrice.Text;
                string bQuan = txtBookQuantity.Text;
                string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewBook; User Id=MyUser; Password=MyPassword;";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    string query = "UPDATE AddBook SET bName = @bName, bAuthor = @bAuthor, bPubl = @bPubl, bPDate = @bPDate, bPrice = @bPrice, bQuan = @bQuan WHERE bid = @bid";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        // Use parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@bName", bname);
                        cmd.Parameters.AddWithValue("@bAuthor", bauthor);
                        cmd.Parameters.AddWithValue("@bPubl", bPublication);
                        cmd.Parameters.AddWithValue("@bPDate", bData);
                        cmd.Parameters.AddWithValue("@bPrice", bPrice);
                        cmd.Parameters.AddWithValue("@bQuan", bQuan);
                        cmd.Parameters.AddWithValue("@bid", rowId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        panel3.Visible = false;
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Data Will Be Updated. Confirm!", "Confirmation Dialog!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewBook; User Id=MyUser; Password=MyPassword;";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    string query = "DELETE FROM AddBook WHERE bid = @bid";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        // Parameterize the query to prevent SQL injection
                        cmd.Parameters.AddWithValue("@bid", rowId);

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
