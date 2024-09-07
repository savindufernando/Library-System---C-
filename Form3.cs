using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res;
            res = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                // Optionally, you can keep this line if you want to continue the application
                // execution when the user clicks "No" in the exit confirmation dialog.
                this.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowAddBookForm();
        }
        private void ShowAddBookForm()
        {
            // Hide Form1
            this.Hide();

            // Show AddBook form (Form4)
            AddBook addBookForm = new AddBook();
            addBookForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowViewBooks();
        }
        private void ShowViewBooks()
        {
            // Hide Form3
            this.Hide();

            // Show ViewBook Form
            AvailableBooks viewbookForm = new AvailableBooks();
            viewbookForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2= new Form2();
            form2.ShowDialog();            
        }
        private void RemoveBook()
        {
            this.Hide();
            AvailableBooks bookForm = new AvailableBooks();
            bookForm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RemoveBook();
        }
    }
}
