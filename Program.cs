using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using WindowsFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            PrintOptions();

            int choice = 0;
            string name = "";
            int age = 0;
            int contact = 0;
            int memberId = 0;
            string title = "";
            string author = "";
            int isbn = 0;
            bool availability = true;
            Dictionary<int, Member> members = new Dictionary<int, Member>();

            while (true)
            {
                Console.Write("Enter Number: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid Input please try again " + ex.Message);
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Member person = new Member(name, age, contact, memberId);
                        members.Add(members.Count + 1, person);
                        person.AddMember(name, age, contact, memberId);
                        break;
                    case 2:
                        //SearchMember(members);
                        ViewMembers();
                        break;
                    case 3:
                        Book borrowbook = new Book(title, author, isbn, availability);
                        borrowbook.BorrowBooks();
                        break;
                    case 4:
                        Book returnBook = new Book(title, author, isbn, availability);
                        returnBook.returnBook();
                        break;
                    case 5:
                        Form1 homepg = new Form1();
                        homepg.ShowDialog();
                        break;
                    case 6:
                        Console.WriteLine("Program Terminated Thank you!!! ");
                        return;
                    default:
                        Console.WriteLine("Invalid Number Please Try Again");
                        break;
                }
            }
        }

        public static void PrintOptions()
        {
            Console.WriteLine("Library System");
            Console.WriteLine("--------------");
            Console.WriteLine("Enter 1 to Add Members");
            Console.WriteLine("Enter 2 to Search member information ");
            Console.WriteLine("Enter 3 to Borrow Book");
            Console.WriteLine("Enter 4 to Return Book");
            Console.WriteLine("Enter 5 to Open GUI");
            Console.WriteLine("Enter 6 to Exit");
        }

        //static void SearchMember(Dictionary<int, Member> members)
        //{
        //    while (true)
        //    {
        //        Console.Write("Enter member id : ");
        //        int checkkey = Convert.ToInt32(Console.ReadLine());
        //        if (members.ContainsKey(checkkey))
        //        {
        //            members[checkkey].PersonInfo();
        //        }
        //        else
        //        {
        //            Console.WriteLine("Key doesn't exist");
        //        }
        //        Console.Write("Do you wish to continue y/n : ");
        //        string choice2 = Console.ReadLine().ToLower();
        //        if (choice2 == "n")
        //        {
        //            break;
        //        }
        //    }
        //}
        static void ViewMembers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True"))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Members";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Member List:");
                            while (reader.Read())
                            {
                                Console.WriteLine($"Member ID: {reader["MemberId"]}");
                                Console.WriteLine($"Name: {reader["Name"]}");
                                Console.WriteLine($"Age: {reader["Age"]}");
                                Console.WriteLine($"Contact Number: {reader["ContactNo"]}");
                                //Console.WriteLine($"Borrowed Books: {reader["Borrowed_Books"]}");
                                Console.WriteLine("--------------------");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}

