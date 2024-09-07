using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class Book
    {
        private string title;
        private string author;
        private int isbn;
        private bool availability;
        private object transactiontype;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Author
        {
            get { return author; }
            set { author = value; }
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
        public Book(string title, string author, int isbn, bool availability)
        {
            this.title = title;
            this.author = author;
            this.isbn = isbn;
            this.availability = availability;
        }
        public void BorrowBooks()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter Book Id : ");
                    int bookId = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter CB No : ");
                    int borrow_memberid = Convert.ToInt32(Console.ReadLine());

                    DateTime now = DateTime.Now;
                    string borrowedDate = now.ToString("yyyy-MM-dd HH:mm:ss");

                    DateTime returnDate = now.AddDays(14);

                    bool bookAvailability = true;
                    string txnType = "Borrowed";

                    string con = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";
                    using (SqlConnection connect = new SqlConnection(con))
                    {
                        connect.Open();
                        if (checkMember(connect, borrow_memberid))
                        {
                            if (checkAvailable(connect, bookId, ref bookAvailability))
                            {
                                Console.Write("Book Available DO YOU WANT TO CONTINUE y/n : ");
                                string borrowch = Console.ReadLine().ToLower();

                                if (borrowch.Equals("y"))
                                {
                                    bool borrowed = false;

                                    string s1 = "UPDATE Books SET Availability = @borrowed WHERE BookID = @bookId";
                                    string s2 = "UPDATE Members SET Borrowed_Books = Borrowed_Books + 1 WHERE MemberId  = @borrowed_memberid";
                                    string s3 = "INSERT INTO Transactions (BookId, MemberId, [Borrowed Data], [Return Data], [Transaction Type]) VALUES (@bookid, @borrow_MemberId, @borrowedDate, @returnDate, @transactiontype)";

                                    using (SqlCommand cmd = new SqlCommand(s1, connect))
                                    {
                                        cmd.Parameters.AddWithValue("@bookid", bookId);
                                        cmd.Parameters.AddWithValue("@borrowed", borrowed);
                                        cmd.ExecuteNonQuery();
                                    }
                                    using (SqlCommand cmd = new SqlCommand(s2, connect))
                                    {
                                        cmd.Parameters.AddWithValue("@borrowed_memberid", borrow_memberid);
                                        cmd.ExecuteNonQuery();
                                        Console.WriteLine("Book Borrowed and Updated Successfully");
                                    }
                                    using (SqlCommand cmd = new SqlCommand(s3, connect))
                                    {
                                        cmd.Parameters.AddWithValue("@bookid", bookId);
                                        cmd.Parameters.AddWithValue("@borrow_MemberId", borrow_memberid);
                                        cmd.Parameters.AddWithValue("@borrowedDate", borrowedDate);
                                        cmd.Parameters.AddWithValue("@returnDate", returnDate);
                                        cmd.Parameters.AddWithValue("@transactiontype", txnType);

                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Book is Unavailable");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Member not found");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    break;
                }
            }
        }

        public bool checkMember(SqlConnection connect, int borrow_memberid)
        {
            bool memberAvailability = false;
            int numBorrowedBooks = 0;

            try
            {
                string sql3 = "SELECT MemberId FROM Members WHERE MemberId = @borrow_MemberId";
                string sql4 = "SELECT Borrowed_Books FROM Members WHERE MemberId = @borrow_MemberId";

                using (SqlCommand cmd = new SqlCommand(sql3, connect))
                {
                    cmd.Parameters.AddWithValue("@borrow_MemberId", borrow_memberid);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int member = reader.GetInt32(0);
                        if (member > 0)
                        {
                            memberAvailability = true;
                        }
                    }
                    reader.Close();
                }
                using (SqlCommand cmd = new SqlCommand(sql4, connect))
                {
                    cmd.Parameters.AddWithValue("@borrow_MemberId", borrow_memberid);
                    SqlDataReader reader2 = cmd.ExecuteReader();

                    if (reader2.Read())
                    {
                        numBorrowedBooks = reader2.GetInt32(0);
                        if (numBorrowedBooks >= 6)
                        {
                            memberAvailability = false;
                        }
                    }
                    reader2.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return memberAvailability;
        }

        public bool checkAvailable(SqlConnection connect, int bookId, ref bool bookAvailability)
        {
            string sql3 = "SELECT Availability FROM Books WHERE BookId = @bookId";

            using (SqlCommand cmd = new SqlCommand(sql3, connect))
            {
                cmd.Parameters.AddWithValue("@bookId", bookId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    bookAvailability = reader.GetBoolean(0);
                }
                reader.Close();
            }
            return bookAvailability;
        }

        public void returnBook()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter Book Id : ");
                    int bookId = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter Member Id : ");
                    int borrow_MemberId = Convert.ToInt32(Console.ReadLine());

                    DateTime now = DateTime.Now;
                    string returnedDate = now.ToString("yyyy-MM-dd HH:mm:ss");

                    string txnType = "Returned";

                    Console.WriteLine("Do you wish to Return Book y/n : ");
                    string borrowchoice = Console.ReadLine().ToLower();

                    if (borrowchoice.Equals("y"))
                    {
                        bool returned = true;

                        string constring = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";

                        using (SqlConnection connection3 = new SqlConnection(constring))
                        {
                            connection3.Open();

                            string sql3 = "UPDATE Books SET Availability = @returned WHERE BookID = @bookId";
                            string sql5 = "UPDATE Members SET Borrowed_Books = Borrowed_Books - 1 WHERE MemberId = @borrow_MemberId";
                            string sql4 = "INSERT INTO Transactions (BookId, MemberId, [Borrowed Data], [Return Data], [Transaction Type]) VALUES(@bookId, @borrow_MemberId, @borrowedDate, @returnDate, @txnType)";

                            using (SqlCommand cmd = new SqlCommand(sql3, connection3))
                            {
                                cmd.Parameters.AddWithValue("@bookId", bookId);
                                cmd.Parameters.AddWithValue("@returned", returned);
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("Book Returned and updated successfully");
                            }
                            using (SqlCommand cmd = new SqlCommand(sql5, connection3))
                            {
                                cmd.Parameters.AddWithValue("@borrow_MemberId", borrow_MemberId);
                                cmd.ExecuteNonQuery();
                            }
                            using (SqlCommand cmd = new SqlCommand(sql4, connection3))
                            {
                                cmd.Parameters.AddWithValue("@bookId", bookId);
                                cmd.Parameters.AddWithValue("@borrow_MemberId", borrow_MemberId);
                                cmd.Parameters.AddWithValue("@borrowedDate", returnedDate);
                                cmd.Parameters.AddWithValue("@returnDate", returnedDate);
                                cmd.Parameters.AddWithValue("@txnType", txnType);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Book is Unavailable");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    break;
                }
            }
        }


        //    public void BorrowBooks()
        //    {
        //        while(true) 
        //        {
        //            try
        //            {
        //                Console.Write("Enter Book Id : ");
        //                int bookId = Convert.ToInt32(Console.ReadLine());

        //                Console.Write("Enter CB No : ");
        //                int borrow_memberid = Convert.ToInt32(Console.ReadLine());

        //                DateTime now = DateTime.Now;
        //                string borrowedDate = now.ToString("yyyy-MM-dd HH:mm:ss");

        //                DateTime returnDate = now.AddDays(14);

        //                bool bookAvailability = true;
        //                string txnType = "Borrowed";

        //                string con = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";
        //                SqlConnection connect = new SqlConnection(con);
        //                connect.Open();
        //                if (checkMember(borrow_memberid))
        //                {
        //                    if (checkAvailable(bookId, bookAvailability))
        //                    {
        //                        Console.Write("Book Available DO YOU WANT TO CONTINUE y/n : ");
        //                        string borrowch = Console.ReadLine().ToLower();

        //                        if (borrowch.Equals("y"))
        //                        {
        //                            bool borrowed = false;

        //                            string s1 = "UPDATE Books SET Availability = @borrowed WHERE BookID = @bookId";
        //                            string s2 = "UPDATE Members SET Borrowed_Books = @borrowedbooks + 1  WHERE MemberId  = @borrowed_memberid";
        //                            string s3 = "INSERT INTO Transactions (BookId, MemberId, [Borrowed Data], [Return Data], [Transaction Type])";

        //                            using (SqlCommand cmd = new SqlCommand(s1, connect))
        //                            {
        //                                cmd.Parameters.AddWithValue("@bookid", bookId);
        //                                cmd.Parameters.AddWithValue("borrowed", borrowed);
        //                                cmd.ExecuteNonQuery();
        //                            }
        //                            using (SqlCommand cmd = new SqlCommand(s2, connect))
        //                            {
        //                                cmd.Parameters.AddWithValue("@borrow_Member_Id", borrow_memberid);
        //                                cmd.ExecuteNonQuery();
        //                                Console.WriteLine("Book Borrowed and Updated Successfully");
        //                            }
        //                            using (SqlCommand cmd = new SqlCommand(s3, connect))
        //                            {
        //                                cmd.Parameters.AddWithValue("@bookid", bookId);
        //                                cmd.Parameters.AddWithValue("borrow_MemberId", borrow_memberid);
        //                                cmd.Parameters.AddWithValue("borrowedDate", borrowedDate);
        //                                cmd.Parameters.AddWithValue("returnDate", returnDate);
        //                                cmd.Parameters.AddWithValue("@transactiontype", txnType);

        //                                cmd.ExecuteNonQuery();
        //                            }
        //                            break;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("Book is UNavailable");
        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("Member not found");
        //                    break;
        //                }
        //                connect.Close();
        //            }
        //            catch (Exception ex) 
        //            {
        //                Console.WriteLine(ex.Message); 
        //                break; 
        //            } 

        //        }
        //    }
        //    public bool checkMember(int borrow_memberid)
        //    {
        //        bool memberAvailability = false;
        //        int numBorrowedBooks = 0;

        //        try
        //        {
        //            string connc = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";
        //            SqlConnection connect = new SqlConnection(connc);
        //            connect.Open();

        //            string sql3 = "SELECT MemberId FROM Members WHERE MemberId = @borrow_MemberId";
        //            string sql4 = "SELECT Borrowed Books FROM Members WHERE MemberId = @borrow_MemberId";


        //            using (SqlCommand cmd = new SqlCommand(sql3, connect))
        //            {

        //                cmd.Parameters.AddWithValue("@borrow_MemberId", borrow_memberid);

        //                SqlDataReader reader = cmd.ExecuteReader();

        //                if (reader.Read())
        //                {
        //                    int member = reader.GetInt32(0);
        //                    if (member > 0)
        //                    {
        //                        memberAvailability = true;
        //                    }
        //                }
        //                reader.Close();
        //            }
        //            using (SqlCommand cmd = new SqlCommand(sql4, connect))
        //            {

        //                cmd.Parameters.AddWithValue("@borrow_MemberId", borrow_memberid);

        //                SqlDataReader reader2 = cmd.ExecuteReader();

        //                if (reader2.Read())
        //                {
        //                    numBorrowedBooks = reader2.GetInt32(0);
        //                    if (numBorrowedBooks >= 6)
        //                    {
        //                        memberAvailability = false;
        //                    }
        //                }

        //                reader2.Close();

        //            }
        //            connect.Close();

        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Book is not Available or Member already borrowed 6 books please contatc the Librarian", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        return memberAvailability;

        //    }
        //    public bool checkAvailable(int bookId, bool bookAvailability)
        //    {


        //        string constring = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";
        //        SqlConnection connection3 = new SqlConnection(constring);
        //        connection3.Open();

        //        string sql3 = "SELECT Availability FROM Books WHERE BookId = @bookId";

        //        using (SqlCommand cmd = new SqlCommand(sql3, connection3))
        //        {

        //            cmd.Parameters.AddWithValue("@bookId", bookId);

        //            SqlDataReader reader = cmd.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                bookAvailability = reader.GetBoolean(0);
        //            }

        //            reader.Close();
        //            connection3.Close();
        //        }
        //        return bookAvailability;
        //    }
        //    public void returnBook()
        //    {
        //        while (true)
        //        {

        //            try
        //            {
        //                Console.Write("Enter Book Id : ");
        //                int bookId = Convert.ToInt32(Console.ReadLine());

        //                Console.Write("Enter Member Id : ");
        //                int borrow_MemberId = Convert.ToInt32(Console.ReadLine());

        //                DateTime now = DateTime.Now;
        //                string returnedDate = now.ToString("yyyy-MM-dd HH:mm:ss");

        //                bool bookAvailability = true;

        //                Console.WriteLine("Do you wish to Return Book y/n : ");
        //                string borrowchoice = Console.ReadLine().ToLower();

        //                string txnType = "Returned";

        //                if (borrowchoice.Equals("y"))
        //                {
        //                    bool returned = true;

        //                    string constring = "Data Source=Subie\\MSSQLSERVER01; Initial catalog=Library;Integrated Security=True";
        //                    SqlConnection connection3 = new SqlConnection(constring);
        //                    connection3.Open();

        //                    string sql3 = "UPDATE Books SET Availability = @returned WHERE [Book Id] = @bookId";
        //                    string sql5 = "UPDATE Members SET Borrowed_Books = Borrowed_Books - 1 WHERE Member_Id] = @borrow_MemberId";
        //                    string sql4 = "INSERT INTO Transactions (BookId, MemberId, [Borrowed Data], [Return Data], [Transaction Type]) VALUES(@bookId, @borrow_MemberId, @borrowedDate, @returnDate, @txnType)";

        //                    using (SqlCommand cmd = new SqlCommand(sql3, connection3))
        //                    {
        //                        cmd.Parameters.AddWithValue("@bookId", bookId);
        //                        cmd.Parameters.AddWithValue("@returned", returned);

        //                        cmd.ExecuteNonQuery();
        //                        Console.WriteLine("Book Returned and updated successfully");
        //                    }
        //                    using (SqlCommand cmd = new SqlCommand(sql5, connection3))
        //                    {

        //                        cmd.Parameters.AddWithValue("@borrow_MemberId", borrow_MemberId);

        //                    }
        //                    using (SqlCommand cmd = new SqlCommand(sql4, connection3))
        //                    {
        //                        cmd.Parameters.AddWithValue("@bookId", bookId);
        //                        cmd.Parameters.AddWithValue("@borrow_MemberId", borrow_MemberId);
        //                        cmd.Parameters.AddWithValue("@borrowedDate", returnedDate);
        //                        cmd.Parameters.AddWithValue("@returnDate", returnedDate);
        //                        cmd.Parameters.AddWithValue("@txnType", txnType);

        //                        cmd.ExecuteNonQuery();
        //                    }
        //                    connection3.Close();
        //                    break;

        //                }
        //                else
        //                {
        //                    Console.WriteLine("Book is Unavailable");
        //                    break;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //                break;
        //            }
        //        }
        //    }

        //}
    }
    }
 
