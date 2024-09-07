using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal abstract class Person
    {
        private string name;
        private int age;
        private int contact;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        public int Contact
        {
            get { return contact; }
            set { contact = value; }
        }
        public Person(string name, int age, int contact)
        {
            this.name = name;
            this.age = age;
            this.contact = contact;
        }
        public abstract void PersonInfo();
    }
}
