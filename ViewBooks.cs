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
    public partial class ViewBooks : Form
    {
        private Timer timer;
        private SqlConnection con;
        private SqlDataAdapter adapter;
        private DataTable dt;

        public ViewBooks()
        {
            InitializeComponent();
            InitializeTimer();
            PopulateDataGridView();
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 2000; // Refresh every 5 seconds
            timer.Tick += timer1_Tick;
            timer.Start();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        private void PopulateDataGridView()
        {
            try
            {
                panel2.Visible = false;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Books";
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            panel2.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void ViewBooks_Load(object sender, EventArgs e)
        {


        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }
        int bid;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value!=null)
            //{
            //    bid = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            //}
            //panel2.Visible = true;
            //SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;

            //cmd.CommandText = "SELECT * FROM Books WHERE BookID";
            //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //adapter.Fill(dt);

            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                // Retrieve the BookID of the selected book
                bid = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }

            // Make panel2 visible
            panel2.Visible = true;

            try
            {
                // Create a new SQL connection
                using (SqlConnection con = new SqlConnection("Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True"))
                {
                    // Open the connection
                    con.Open();

                    // Create a SQL command to select the details of the book with the specified BookID
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Books WHERE BookID = @BookID", con);
                    cmd.Parameters.AddWithValue("@BookID", bid);

                    // Create a SQL data adapter to execute the command
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    // Create a DataTable to store the results
                    DataTable dt = new DataTable();

                    // Fill the DataTable with the results of the command
                    adapter.Fill(dt);

                    // Display the details of the selected book in panel2
                    if (dt.Rows.Count > 0)
                    {
                        textBox2.Text = dt.Rows[0]["BookID"].ToString();
                        textBox3.Text = dt.Rows[0]["Title"].ToString();
                        textBox4.Text = dt.Rows[0]["Author"].ToString();
                        textBox5.Text = dt.Rows[0]["ISBN"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Book not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //if(textBox1.Text!="")
            //{
            //    SqlConnection con = new SqlConnection();
            //    con.ConnectionString = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;

            //    cmd.CommandText = "SELECT * FROM Books WHERE Title";
            //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable();
            //    adapter.Fill(dt);

            //    dataGridView1.DataSource = dt;
            //}
            //else
            //{
            //    SqlConnection con = new SqlConnection();
            //    con.ConnectionString = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;

            //    cmd.CommandText = "SELECT * FROM Books";
            //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable();
            //    adapter.Fill(dt);

            //    dataGridView1.DataSource = dt;
            //}
            string titleFilter = textBox1.Text.Trim(); // Get the text from textBox1 and remove leading/trailing spaces

            using (SqlConnection con = new SqlConnection("Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True"))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                if (!string.IsNullOrEmpty(titleFilter))
                {
                    // If textBox1 is not empty, filter records based on the Title column
                    cmd.CommandText = "SELECT * FROM Books WHERE Title LIKE @Title";
                    cmd.Parameters.AddWithValue("@Title", "%" + titleFilter + "%"); // Use '%' for partial matching
                }
                else
                {
                    // If textBox1 is empty, fetch all records
                    cmd.CommandText = "SELECT * FROM Books";
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input values from text boxes
                int bid = Convert.ToInt32(textBox2.Text);
                string bname = textBox3.Text;
                string bauthor = textBox4.Text;
                int bisbn = Convert.ToInt32(textBox5.Text);

                // Create a connection to the database
                using (SqlConnection con = new SqlConnection("Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True"))
                {
                    con.Open();

                    // Construct the SQL UPDATE statement
                    string query = "UPDATE Books SET Title = @Title, Author = @Author, ISBN = @ISBN WHERE BookID = @BookID";

                    // Create a SqlCommand object
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Set parameter values
                        cmd.Parameters.AddWithValue("@Title", bname);
                        cmd.Parameters.AddWithValue("@Author", bauthor);
                        cmd.Parameters.AddWithValue("@ISBN", bisbn);
                        cmd.Parameters.AddWithValue("@BookID", bid);

                        // Execute the SQL command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh the DataGridView to reflect the changes
                            RefreshDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("No matching book found for the provided Book ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RefreshDataGridView()
        {
            // Refresh the DataGridView to reflect the changes in the database
            using (SqlConnection con = new SqlConnection("Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT * FROM Books";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the Book ID from the TextBox
                int bid = Convert.ToInt32(textBox2.Text);

                // Create a connection to the database
                using (SqlConnection con = new SqlConnection("Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True"))
                {
                    con.Open();

                    // Construct the SQL DELETE statement
                    string query = "DELETE FROM Books WHERE BookID = @BookID";

                    // Create a SqlCommand object
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Set the parameter value
                        cmd.Parameters.AddWithValue("@BookID", bid);

                        // Execute the SQL command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh the DataGridView to reflect the changes
                            RefreshDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("No matching book found for the provided Book ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ConfirmExit();
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

        private void ViewBooks_Load_1(object sender, EventArgs e)
        {

        }
    }
}
