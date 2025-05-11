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
    public partial class CompleteBookDetail : Form
    {
        public CompleteBookDetail()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CompleteBookDetail_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=NewStudent; User Id=MyUser; Password=MyPassword;";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection;
                cmd.CommandText = "SELECT * FROM IRBook WHERE book_return_date IS NULL";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                cmd.CommandText = "SELECT * FROM IRBook WHERE book_return_date IS NOT NULL";
                SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd);
                DataSet ds1 = new DataSet();
                dataAdapter1.Fill(ds1);
                dataGridView2.DataSource = ds1.Tables[0];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ReturnBook returnBook = new ReturnBook();
            returnBook.Show();
        }
    }
}
