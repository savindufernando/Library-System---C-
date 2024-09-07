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
    public partial class AddBook : Form
    {
        public AddBook()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveBookData();  
        }
        private void SaveBookData()
        {
            int bookId = Convert.ToInt32(bookid.Text);
            string bookTitle = bookname.Text;
            string bauthor = author.Text;
            int isbn = Convert.ToInt32(bisbn.Text);
            bool availability = true;

            Library librarylist=new Library(bookId,bookTitle,bauthor,isbn,availability);
            librarylist.AddBook(bookId,bookTitle, bauthor,isbn,availability);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {
            // Reset the text of input fields to empty strings
            bookid.Text = "";
            bookname.Text = "";
            author.Text = "";
            bisbn.Text = "";
        }
    }
}
