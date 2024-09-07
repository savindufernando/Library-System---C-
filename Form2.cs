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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowForm3();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Transactions bookForm = new Transactions();
            bookForm.ShowDialog();
        }
        private void ShowForm3()
        {
            // Hide Form1
            this.Hide();

            // Show Form3
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

    }
}
