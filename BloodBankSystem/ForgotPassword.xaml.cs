using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BloodBankSystem
{
    public partial class ForgotPassword : Window
    {
        private XDocument xmldoc;
        private readonly string URL_XML_FILE = "../../../AdminRegistration.xml";
        public ForgotPassword()
        {
            InitializeComponent();
            xmldoc = XDocument.Load(URL_XML_FILE);
        }
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string email = textBox.Text;

            if (IsValidEmail(email))
            {

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

            }
        }
            private bool IsValidEmail(string email)
            {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern);
            }
        
        private void btnEmailSubit_Click(object sender, RoutedEventArgs e)
        {
           
            var targetemail = txtEmail.Text;
            

            var checkexistingUser = xmldoc.Descendants("Admin")
                .FirstOrDefault(admin => (string)admin.Element("Email") == targetemail);

            if (checkexistingUser != null)
            {
                ResetPass rP = new ResetPass(txtEmail.Text);
                rP.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Email is not Exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtEmail.Clear();
            }

        }

        private void btnExist_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
