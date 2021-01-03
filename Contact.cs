using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager
{
    public class Contact
    {
        private string fname;
        private string lname;
        private string email;
        private string address;
        private string notes;
        private DateTime birthday;
        private int id;

        public Contact(string fname, string lname)
        {
            this.fname = fname;
            this.lname = lname;
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public string getFname()
        {
            return fname;
        }

        public void setFname(string fname)
        {
            this.fname = fname;
        }

        public string getLname()
        {
            return lname;
        }

        public void setLname(string lname)
        {
            this.lname = lname;
        }

        public string getEmail()
        {
            return email;
        }

        public void setEmail(string email)
        {
            this.email = email;
        }

        public string getAddress()
        {
            return address;
        }

        public void setAddress(string address)
        {
            this.address = address;
        }

        public string getNotes()
        {
            return notes;
        }

        public void setNotes(string notes)
        {
            this.notes = notes;
        }

        public DateTime getBirthday()
        {
            return birthday;
        }

        public void setBirthday(DateTime birthday)
        {
            this.birthday = birthday;
        }

    }
}
