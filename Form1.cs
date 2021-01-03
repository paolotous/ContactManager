using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;

namespace ContactManager
{
    public partial class Form1 : Form
    {
        ContactManagement cm = new ContactManagement(100);
        Contact c;
        int counter = 0;
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Contacts.mdf;Integrated Security=True");
        static string provider = ConfigurationManager.AppSettings["provider"];
        string connectionString = ConfigurationManager.AppSettings["connectionString"];
        DbProviderFactory factory = DbProviderFactories.GetFactory(provider);

        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        //this will fetch the data from the database and load
        public void showContacts()
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                if (connection == null)
                {
                    MessageBox.Show("Connection Error");
                    return;
                }

                connection.ConnectionString = connectionString;
                connection.Open();
                DbCommand command = factory.CreateCommand();
                DbCommand commandNums = factory.CreateCommand();

                if (command == null)
                {
                    MessageBox.Show("Command Error");
                    return;
                }

                command.Connection = connection;
                command.CommandText = "SELECT * FROM tblContacts";
                commandNums.Connection = connection;
                commandNums.CommandText = "SELECT Id FROM tblContacts";


                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //cm.getContacts()[counter].setFname($"{dataReader["FirstName"]}");
                        //cm.getContacts()[counter].setLname($"{dataReader["LastName"]}");
                        cm.addContact($"{dataReader["FirstName"]}", $"{dataReader["LastName"]}", $"{dataReader["Email"]}",
                            $"{dataReader["StreetAddress"]}", Convert.ToDateTime(dataReader["Birthday"]), $"{dataReader["Notes"]}");
                        //listView1.Items.Add($"{dataReader["FirstName"]}" + " " + $"{dataReader["LastName"]}");
                        listView1.Items.Add(cm.getFullName(counter));
                        counter++;
                    }
                }
            }
        }

        //this will add new contacts to the database
        public void addContacts(Contact c)
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                if (connection == null)
                {
                    MessageBox.Show("Connection Error");
                    return;
                }

                connection.ConnectionString = connectionString;
                connection.Open();
                DbCommand command = factory.CreateCommand();

                if (command == null)
                {
                    MessageBox.Show("Command Error");
                    return;
                }

                //MessageBox.Show(c.getFname());
                command.Connection = connection;
                command.CommandText = "Insert into tblContacts (Id, FirstName, LastName, Email, StreetAddress, " +
                    "Birthday, Notes) values ('" +c.getId()+ "','" +c.getFname()+ "','" +c.getLname()+
                    "','" +c.getEmail()+ "','" +c.getAddress()+ "','" +c.getBirthday().Date+ "','" +c.getNotes()+ "');";
                command.ExecuteNonQuery();
                connection.Close();
                 
            } 

        }

        //a different way to do insert queries
        public void insertContacts(Contact c)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into tblContacts (Id, FirstName, LastName, Email, StreetAddress, " +
                    "Birthday, Notes) values ('" + c.getId() + "','" + c.getFname() + "','" + c.getLname() +
                    "','" + c.getEmail() + "','" + c.getAddress() + "','" + c.getBirthday().Date + "','" + c.getNotes() + "');";
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //this will update a contact in the database
        public void updateContacts(Contact c)
        {
            using(DbConnection connection = factory.CreateConnection())
            {
                if(connection == null)
                {
                    MessageBox.Show("Connection Error");
                    return;
                }

                connection.ConnectionString = connectionString;
                connection.Open();
                DbCommand command = factory.CreateCommand();

                if(command == null)
                {
                    MessageBox.Show("Command Error");
                    return;
                }

                command.Connection = connection;
                command.CommandText = "UPDATE tblContacts" +
                    " SET FirstName = '"  +c.getFname()+ "', LastName ='" + c.getLname() + "', Email ='" + c.getEmail()
                    + "', StreetAddress = '" + c.getAddress() + "', Birthday='" + c.getBirthday().Date +
                    "', Notes ='" + c.getNotes() + "' WHERE Id='" + c.getId()+ "';";
                
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        //this will delete a contact in the database
        public void deleteContacts(Contact c)
        {
            using(DbConnection connection = factory.CreateConnection())
            {
                if(connection == null)
                {
                    MessageBox.Show("Connection Error");
                    return;
                }

                connection.ConnectionString = connectionString;
                connection.Open();
                DbCommand command = factory.CreateCommand();

                if(command == null)
                {
                    MessageBox.Show("Command error");
                    return;
                }

                command.Connection = connection;
                command.CommandText = "DELETE FROM tblContacts WHERE Id = " + c.getId();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        //First thing we wanna do when the form loads is to load and fetch the data
        private void Form1_Load(object sender, EventArgs e)
        {

            showContacts();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {    int id;
            if (checkFields())
            {
                id = cm.addContact(txtFirst.Text, txtLast.Text, txtEmail.Text, txtAddress.Text, dateBirth.Value, txtExtra.Text);
                //MessageBox.Show(id.ToString());
                ListViewItem fullName = new ListViewItem(cm.getFullName(id-1));
                listView1.Items.Add(fullName);
                addContacts(cm.getContacts()[id-1]);
                //MessageBox.Show(id.ToString());
                clear();
                
            }
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                c = cm.getContacts()[listView1.SelectedItems[0].Index];
                txtFirst.Text = c.getFname();
                txtLast.Text = c.getLname();
                txtEmail.Text = c.getEmail();
                txtAddress.Text = c.getAddress();
                txtExtra.Text = c.getNotes();
                dateBirth.Value = c.getBirthday();
            }
            catch(ArgumentOutOfRangeException ex)
            {
                //this is just to prevent the program from crashing
                //listview issue unrelated to the code logic
            }
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            remove();
        }

        //this will perform the functions/steps in charge of deleting a contact
        public void remove()
        {
            try
            {
                c = cm.getContacts()[listView1.SelectedItems[0].Index];
                cm.deleteContact(c.getId());
                //deleteContacts(cm.getContacts()[c.getId() - 1]);
                deleteContacts(c);
                listView1.Items.Remove(listView1.SelectedItems[0]);
                clear();
            }

            catch (ArgumentOutOfRangeException ex)
            {
                //same thing
            }
        }
        
        //this will clear the form
        public void clear()
        {
            txtFirst.Clear();
            txtLast.Clear();
            txtExtra.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            dateBirth.Value = DateTime.Now;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            remove();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location))
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (checkFields())
            {
                try
                {
                    c = cm.getContacts()[listView1.SelectedItems[0].Index];
                    c.setFname(txtFirst.Text);
                    c.setLname(txtLast.Text);
                    c.setEmail(txtEmail.Text);
                    c.setAddress(txtAddress.Text);
                    c.setBirthday(dateBirth.Value);
                    c.setNotes(txtExtra.Text);
                    //MessageBox.Show(c.getId().ToString());
                    updateContacts(c);
                    listView1.SelectedItems[0].Text = cm.getFullName(c.getId()-1);

                }
                catch (ArgumentOutOfRangeException ex)
                {

                }
            }
        }

        //checks whether a field that requires letters is not empty and whether all characters are letters
        public bool isAlphabet(string txt)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return false;
            }

            for(int i = 0; i < txt.Length; i++)
            {
                if (!char.IsLetter(txt[i]))
                {
                    return false;
                }
            }

            return true;
        }

        //regular expression for email
        public static Regex emailRegex()
        {
            string emailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(emailPattern, RegexOptions.IgnoreCase);
        }
        public bool validateEmail(string emailTxt)
        {
            if (string.IsNullOrEmpty(emailTxt))
            {
                return false;
            }

            Regex emailReg = emailRegex();
            if (emailReg.IsMatch(emailTxt))
            {
                return true;
            }

            return false;

        }

        //this will validate all fields and gets the final result
        public bool checkFields()
        {
            if(isAlphabet(txtFirst.Text) && isAlphabet(txtLast.Text) && validateEmail(txtEmail.Text))
            {
                return true;
            }

            if (!isAlphabet(txtFirst.Text))
            {
                showLimited(lblErrFirst, "Must contain letters");
            }

             if (!isAlphabet(txtLast.Text))
            {
                showLimited(lblErrLast, "Must contain letters");
            }
            if (!validateEmail(txtEmail.Text))
            {
                showLimited(lblErrEmail, "Invalid Email");
            }
            

            return false;
        }

        //this will activate if input error occured, a label will be temporarily be displayed
        void showLimited(Label lbl, string message)
        {
            Timer t = new Timer();
            t.Interval = 3000;
            lbl.Text = message;
            lbl.Visible = true;
            t.Tick += (s, e) =>
            {
                lbl.Visible = false;
                t.Stop();
            };
            t.Start();
           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        //this will refresh a form and clear any selection if there is any
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            clear();
            listView1.SelectedItems.Clear();
        }
    }
}
