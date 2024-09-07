using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Transactions : Form
    {
        private Timer timer;
        private object ex;

        public Transactions()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 2000; // Refresh every 5 seconds
            timer.Tick += timer1_Tick;
            timer.Start();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 bookForm = new Form3();
            bookForm.ShowDialog();
        }
        private void RefreshDataGridView()
        {
            try
            {
                string connectionString = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Select all transactions
                    string query = "SELECT * FROM Transactions";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Execute query and fill DataTable
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Bind DataGridView to the DataTable
                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 bookForm = new Form3();
            bookForm.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
