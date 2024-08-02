using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace BloodBankSystem
{
    public partial class AdminReg : Window
    {
        private XDocument xmldoc;
        private readonly string URL_XML_FILE = "../../../AdminRegistration.xml";

        public AdminReg()
        {
            InitializeComponent();
            MainWindow mw = new MainWindow();
            mw.Close();
            xmldoc = XDocument.Load(URL_XML_FILE);
        }

        private void txtAdminEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string email = textBox.Text;

            if (IsValidEmail(email))
            {
                textBox.Background = System.Windows.Media.Brushes.White;
                emailmsg.Content = "Valid email";
                emailmsg.Foreground = System.Windows.Media.Brushes.Green;
            }
            else if (email == "")
            {
                emailmsg.Content = "";
            }
            else
            {
                emailmsg.Content = "Invalid email";
                textBox.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var targetUsername = txtAUsername.Text;
            var targetEmail = txtAdminEmail.Text;
            var hashedPassword = txtACpassword1.Password; 

            var existingUser = xmldoc.Descendants("Admin")
                .FirstOrDefault(admin => (string)admin.Element("Username") == targetUsername ||
                                         (string)admin.Element("Email") == targetEmail);

            if (existingUser == null && targetUsername != "" && targetEmail != "" && hashedPassword != "")
            {
                AddUser(targetUsername, hashedPassword, targetEmail);
                MessageBox.Show("Successfully User Added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetFields();
                this.Close();
                MainWindow mw = new MainWindow();
                mw.Show();
            }
            else if (existingUser != null && (string)existingUser.Element("Username") == targetUsername)
            {
                MessageBox.Show("Username Already Exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ResetFields();
            }
            else if (existingUser != null && (string)existingUser.Element("Email") == targetEmail)
            {
                MessageBox.Show("Email Already Exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ResetFields();
            }
        }

        private void AddUser(string username, string hashedPassword, string email)
        {
            string tempFileName = URL_XML_FILE + ".tmp";

            using (XmlReader reader = XmlReader.Create(URL_XML_FILE))
            using (XmlWriter writer = XmlWriter.Create(tempFileName, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartDocument();

                bool rootWritten = false;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "AdminRegistration")
                    {
                        if (!rootWritten)
                        {
                            writer.WriteStartElement("AdminRegistration");
                            rootWritten = true;
                        }
                        writer.WriteNode(reader, true);
                    }
                    else if (rootWritten)
                    {
                        writer.WriteNode(reader, true);
                    }
                }

                if (!rootWritten)
                {
                    writer.WriteStartElement("AdminRegistration");
                }

                writer.WriteStartElement("Admin");
                writer.WriteElementString("Username", username);
                writer.WriteElementString("Password", hashedPassword);
                writer.WriteElementString("Email", email);
                writer.WriteEndElement();

                if (rootWritten)
                {
                    writer.WriteEndElement(); 
                }

                writer.WriteEndDocument();
            }

            File.Delete(URL_XML_FILE);
            File.Move(tempFileName, URL_XML_FILE);
        }

        private void ResetFields()
        {
            txtAUsername.Clear();
            txtACpassword1.Clear();
            txtApassword1.Clear();
            txtAdminEmail.Clear();
        }

        private void txtACpassword1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtApassword1.Password == txtACpassword1.Password)
            {
                passwordmsg.Content = "Passwords match";
                passwordmsg.Foreground = System.Windows.Media.Brushes.Green;
            }
            else if (txtACpassword1.Password == "")
            {
                passwordmsg.Content = "";
            }
            else
            {
                passwordmsg.Content = "Passwords do not match";
            }
        }

        private void btnExist_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
