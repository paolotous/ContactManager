using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager
{
    class ContactManagement
    {
        private Contact[] contactList;
        private int numContacts;
        private int maxContacts;

        public ContactManagement(int maxContacts)
        {
            this.maxContacts = maxContacts;
            contactList = new Contact[maxContacts];
            numContacts = 0;
        }

        public int getNumContacts()
        {
            return numContacts;
        }

        public void setNumContacts(int numContacts)
        {
            this.numContacts = numContacts;
        }

        //I chose to return an int here so I can use the id generated right away after adding a contact
        public int addContact(string fname, string lname, string email, string address, DateTime birthday, string notes)
        {
            int id;
            if (numContacts < maxContacts)
            {
                contactList[numContacts] = new Contact(fname, lname);
                contactList[numContacts].setEmail(email);
                contactList[numContacts].setAddress(address);
                contactList[numContacts].setNotes(notes);
                contactList[numContacts].setBirthday(birthday);
                contactList[numContacts].setId(numContacts+1);
                id = contactList[numContacts].getId();
                numContacts++;
                return id;
            }

            return -1;
        }

        public string getFullName(int id)
        {
            string s = "";
            int loc = findContact(id+1);
            if (loc != -1)
            {
                s = contactList[loc].getFname() + " " + contactList[loc].getLname();
                return s;
            }
               
            return "Not found";
        }

        //returns an array of contacts
        public Contact[] getContacts()
        {
            return contactList;

        }

        public int findContact(int id)
        {
            for(int i = 0; i < numContacts; i++)
            {
                if(contactList[i].getId() == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool deleteContact(int id)
        {
            int loc = findContact(id);
            if(loc != -1)
            {
                contactList[loc] = contactList[numContacts - 1];
                numContacts--;
                return true;
            }

            return false;
        }
    }
}
