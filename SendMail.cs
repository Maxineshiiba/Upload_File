using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonsend_Click(object sender, EventArgs e)
        {
            bool validForm = true;

            //reset error labels, call resetLabels() function
            resetLabels();

            //field error checking
            if (textBoxsubject.TextLength <= 0)
            {
                SubjectErrorLabel.Text = "Please add a subject!";
                validForm = false;
            }

            if(TextBoxbody.TextLength <= 0)
            {
                BodyErrorLabel.Text = "Please add a body!";
                validForm = false;
            }

            if (!ValidEmail(textBoxto.Text))
            {
                ToErrorLabel.Text = "Please enter a valid email address!";
                validForm = false;
            }

            if (!ValidEmail(textBoxfrom.Text))
            {
                FromErrorLabel.Text = "Please enter a valid gmail address!";
                validForm = false;
            }
            else
            { 
               //make sure the host is gmail.com
                MailAddress addr = new MailAddress(textBoxfrom.Text);
                if (addr.Host != "gmail.com")
                {
                    FromErrorLabel.Text = "Please enter a valid gmail address!";
                    validForm = false;
                }
            }
            if (!validForm)
            {
                //if this is empty field then do nothing
                return;
            }
            else 
            { 
               //create an instance of the password form
                Password frm = new Password(textBoxto.Text, textBoxfrom.Text, textBoxsubject.Text, TextBoxbody.Text);
                frm.Show();
            }
        }

        private void resetLabels()
        {
            //resetting the error labels
            SubjectErrorLabel.Text = "";
            ToErrorLabel.Text = "";
            FromErrorLabel.Text = "";
            BodyErrorLabel.Text = "";
        }

        //check if the email address is valid or not
        private bool ValidEmail(string emailaddress)
        {
            try
            {
                //if the email address is valid
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

/////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Password : Form
    {
        string to;
        string from;
        string subject;
        string body;

        public Password(string toText, string fromText, string subjectText, string bodyText)
        {
            InitializeComponent();
            to = toText;
            from = fromText;
            subject = subjectText;
            body = bodyText;
            //protect the password when typing
            PasswordBox.Text = "";
            PasswordBox.PasswordChar = '*';  
        }

        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            //setting variables
            var fromAddress = new MailAddress(from);
            var toAddress = new MailAddress(to);
            string pw = PasswordBox.Text;

            if (PasswordBox.TextLength <= 0)
            {
                MessageBox.Show("Please enter password!");
                return;
            }

            //Source: stackoverflow.com/questions/32260/sending-email-in-net-through-gmail
            //trys to send the email
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, pw)
            };
            using (var msg = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                try
                {
                    smtp.Send(msg);
                    this.Close();
                    MessageBox.Show("E-mail has been sent successfully!!!");
                }
                catch (Exception ex)
                {
                    //error catching
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}


