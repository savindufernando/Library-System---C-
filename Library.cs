using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    internal class Library
    {
        private int bookId;
        private string bookTitle;
        private string bauthor;
        private int isbn;
        private bool availability;
        public List<Book> books;

        public int BookId
        {
            get { return bookId; }
            set { bookId = value; }
        }
        public string BookTitle
        {
            get { return bookTitle; }
            set { bookTitle = value; }
        }   
        public string Author
        {
            get { return bauthor; }
            set { bauthor = value; }
        }
        public int Isbn
        {
            get { return isbn; }
            set { isbn = value; }
        }
        public bool Availability
        {
            get { return availability; }
            set { availability = value; } 
        }
        public Library(int bookId,string bookTitle, string bauthor, int isbn, bool availability)
        {
            this.BookId = bookId;
            this.BookTitle = bookTitle;
            this.Author = bauthor;
            this.Isbn = isbn;
        }
        public void AddBook(int bookId,string bookTitle,string bauthor, int isbn, bool availability)
        {
            string connectionString = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO BOOKS(BookID, Title, Author, ISBN, Availability) VALUES(@bid, @bname, @bauthor, @isbn, 1)"; // 1 represents true
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@bid", bookId);
                    cmd.Parameters.AddWithValue("@bname", bookTitle);
                    cmd.Parameters.AddWithValue("@bauthor", bauthor);
                    cmd.Parameters.AddWithValue("@isbn", isbn);
                    cmd.Parameters.AddWithValue("@availability", availability);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void RemoveBook(int bookId, DataGridView dataGridView)
        {
            try
            {
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
                        cmd.Parameters.AddWithValue("@BookID", bookId);

                        // Execute the SQL command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh the DataGridView to reflect the changes
                            RefreshDataGridView(dataGridView);
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
        private void RefreshDataGridView(DataGridView dataGridView)
        {
            // Refresh the DataGridView to reflect the changes in the database
            using (SqlConnection con = new SqlConnection("Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT * FROM Books";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView.DataSource = dt;
            }
        }
    }
}
