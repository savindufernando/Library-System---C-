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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True");
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AuthenticateUser();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ConfirmExit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void AuthenticateUser()
        {
            // Hardcoded username and password
            string Username = "admin";
            string Password = "12345";

            string username = textBox1.Text;
            string user_password = textBox2.Text;

            try
            {
                if (username == Username && user_password == Password)
                {
                    // Login successful
                    Form2 form2 = new Form2();

                    // Pass a reference to Form1 to Form2
                    form2.Owner = this;

                    // Hide Form1
                    this.Hide();

                    // Show Form2
                    form2.ShowDialog();
                }
                else
                {
                    // Login failed
                    MessageBox.Show("Invalid login credentials, please check Username and Password and try again", "Invalid Login Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }
        private void ConfirmExit()
        {
            DialogResult res = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                // execution when the user clicks "No" in the exit confirmation dialog.
                this.Show();
            }
        }



    }
}
