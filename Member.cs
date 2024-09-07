using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class Member : Person
    {
        private int member_id;

        public int Member_id
        {
            get { return member_id; }
            set { member_id = value; }
        }
        public Member(string name, int age, int contact, int member_id) : base(name, age, contact)
        {
            this.Member_id = Member_id;
        }
        public override void PersonInfo()
        {
            Console.WriteLine("Member ID : " + Member_id);
            Console.WriteLine("Member Name : " + Name);
            Console.WriteLine("Member Age : " + Age);
            Console.WriteLine("Member Contact Number : " + Contact);
        }
        public void AddMember(string name, int age, int contact, int member_id)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter  Your Name : ");
                    name = Console.ReadLine();

                    Console.WriteLine("Enter Your Age : ");
                    age = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter Your Contact Number : ");
                    contact = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter Your CB Number [CB]: ");
                    member_id = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error Occured " + e.Message);
                }
                Console.Write("Confirm Add Member : y/n");
                string save = Console.ReadLine();

                if (save == "y")
                {
                    databaseinputs(member_id, name, age, contact);
                }
                else
                {
                    Console.WriteLine("Member Information Not Added");
                    continue;
                }
                Console.Write("Do you want to Add a another one y/n : ");

                string addnew = Console.ReadLine().ToLower();
                if (addnew == "n")
                {
                    break;
                }
            }
        }
        private void databaseinputs(int memberid, string name, int age, int contact)
        {
            try
            {
                int numBorrowedBooks = 0;
                string cons = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";
                SqlConnection con = new SqlConnection(cons);
                con.Open();

                SqlCommand cmd;
                string sql;

                sql = "INSERT INTO Members(MemberId,Name,Age,ContactNo,Borrowed_Books) VALUES (@memberId,@name,@age,@contact,@borrowedbooks)";

                using (cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@MemberId", memberid);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@contact", contact);
                    cmd.Parameters.AddWithValue("@borrowedbooks", numBorrowedBooks);

                    int rownum = cmd.ExecuteNonQuery();
                    Console.WriteLine($"Added to {rownum} Database Successfully");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void ViewMemberDetails(int memberId)
        {
            try
            {
                string cons = "Data Source=SAVINDU;Initial Catalog=LibrarySystem;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(cons))
                {
                    con.Open();
                    string sql = "SELECT * FROM Members WHERE MemberId = @memberId";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@memberId", memberId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Console.WriteLine($"Member ID: {reader["MemberId"]}");
                                Console.WriteLine($"Name: {reader["Name"]}");
                                Console.WriteLine($"Age: {reader["Age"]}");
                                Console.WriteLine($"Contact Number: {reader["ContactNo"]}");
                                Console.WriteLine($"Borrowed Books: {reader["Borrowed_Books"]}");
                            }
                            else
                            {
                                Console.WriteLine("Member not found.");
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

