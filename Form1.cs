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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pnlCreateNewAcount.Visible = false;
            panel2.Visible = false;
            panel3.Location = new System.Drawing.Point(655, 55);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtUserName.Text !="" && txtPassword.Text != "")
            {
                string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=Login; User Id=MyUser; Password=MyPassword;";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();

                        string query = "SELECT giren FROM Users WHERE name = @username AND password = @password";
                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.Add(new SqlParameter("@username", txtUserName.Text));
                            cmd.Parameters.Add(new SqlParameter("@password", txtPassword.Text));

                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                string role = result.ToString();
                                this.Hide();

                                if (role == "admin")
                                {
                                    Dashboard dashboard = new Dashboard();
                                    dashboard.Show();
                                }
                                else if (role == "user")
                                {
                                    ReturnBook returnBook = new ReturnBook();
                                    returnBook.Show();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Wrong Username or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else
            {
                MessageBox.Show("Empty Field NOT Allowed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            if(txtFullName.Text !="" && txtEmail.Text !="" && txtCreatePassword.Text != "")
            {
                string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=Login; User Id=MyUser; Password=MyPassword;";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE name = @username";
                    using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, sqlConnection))
                    {
                        checkCmd.Parameters.AddWithValue("@username", txtFullName.Text);

                        int userExists = (int)checkCmd.ExecuteScalar();

                        if (userExists > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose another one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }


                    string insertUserQuery = "INSERT INTO Users (name,email, password,giren) VALUES (@username,@email, @password,@giren)";
                    using (SqlCommand insertCmd = new SqlCommand(insertUserQuery, sqlConnection))
                    {
                        insertCmd.Parameters.AddWithValue("@username", txtFullName.Text);
                        insertCmd.Parameters.AddWithValue("@password", txtCreatePassword.Text);
                        insertCmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        insertCmd.Parameters.AddWithValue("@giren","admin");


                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Account created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to create account. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    sqlConnection.Close();
                }
                pnlCreateNewAcount.Visible = false;
                panel3.Visible = true;
                panel3.Location = new System.Drawing.Point(690, 100);
            }
            else
            {
                MessageBox.Show("Empty Field NOT Allowed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            txtFullName.Clear();    
            txtCreatePassword.Clear();
            txtEmail.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(txtResetPassword.Text != "")
            {
                string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=Login; User Id=MyUser; Password=MyPassword;";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    string query = "SELECT password FROM Users WHERE email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Email", txtResetPassword.Text);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string password = result.ToString();
                            MessageBox.Show($"The password for the entered email is: ***{password}***", "Password Retrieved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("The email does not exist in the system.", "Email Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    sqlConnection.Close();
                }
            }
            else
            {
                MessageBox.Show("Empty Field NOT Allowed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtUserName.Text == "UserName")
            {
                txtUserName.Clear();
            }
        }

        private void txtPassword_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Clear();
                txtPassword.PasswordChar = '*';
            }
        }

        private void pictureBoxFacebook_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/abdulkarim.hajamin?mibextid=LQQJ4d");
        }

        private void pictureBoxInstagram_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/");
        }

        private void pictureBoxLinkedin_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/abdulkarim-haj-amin-656a7b294?utm_source=share&utm_campaign=share_via&utm_content=profile&utm_medium=ios_app");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel3.Visible=false;
            panel2.Visible =true;
            panel2.Location = new System.Drawing.Point(690, 100);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel3.Visible = false;
            pnlCreateNewAcount.Visible = true;
            pnlCreateNewAcount.Location = new System.Drawing.Point(690, 100);
            txtUserName.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            txtCreatePassword.Clear();
            txtEmail.Clear();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)

        {
            pnlCreateNewAcount.Visible=false;
            panel3.Visible= true;
            panel3.Location = new System.Drawing.Point(690, 100);
            txtUserName.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            txtCreatePassword.Clear();
            txtEmail.Clear();
            
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel2.Visible=false;
            panel3.Visible =true;
            txtResetPassword.Clear();
        }

        private void pnlCreateNewAcount_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnSignUp2_Click(object sender, EventArgs e)
        {

            if (txtFullName.Text != "" && txtEmail.Text != "" && txtCreatePassword.Text != "")
            {
                string connectionString = @"Data Source=ABDULKARIM\SQLEXPRESS; Database=Login; User Id=MyUser; Password=MyPassword;";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE name = @username";
                    using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, sqlConnection))
                    {
                        checkCmd.Parameters.AddWithValue("@username", txtFullName.Text);

                        int userExists = (int)checkCmd.ExecuteScalar();

                        if (userExists > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose another one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }


                    string insertUserQuery = "INSERT INTO Users (name,email, password,giren) VALUES (@username,@email, @password,@giren)";
                    using (SqlCommand insertCmd = new SqlCommand(insertUserQuery, sqlConnection))
                    {
                        insertCmd.Parameters.AddWithValue("@username", txtFullName.Text);
                        insertCmd.Parameters.AddWithValue("@password", txtCreatePassword.Text);
                        insertCmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        insertCmd.Parameters.AddWithValue("@giren", "user");


                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Account created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to create account. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    sqlConnection.Close();
                }
                pnlCreateNewAcount.Visible = false;
                panel3.Visible = true;
                panel3.Location = new System.Drawing.Point(690, 100);
            }
            else
            {
                MessageBox.Show("Empty Field NOT Allowed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            txtFullName.Clear();
            txtCreatePassword.Clear();
            txtEmail.Clear();
        }
    }
}
