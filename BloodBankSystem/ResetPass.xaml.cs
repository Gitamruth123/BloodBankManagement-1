using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
   
    public partial class ResetPass : Window
    {
        private XDocument xmldoc;
        private readonly string URL_XML_FILE = "../../../AdminRegistration.xml";
        private string receivedData;
        public ResetPass(string data)
        {
            InitializeComponent();
            xmldoc = XDocument.Load(URL_XML_FILE);
            receivedData = data;
           
            
        }

        private void PasswordBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox2.Password == PasswordBox1.Password)
            {


                passwordmsg.Content = "Passwords match";
                passwordmsg.Foreground = System.Windows.Media.Brushes.Green;
            }
            else if (PasswordBox1.Password == "")
            {
                passwordmsg.Content = "";
            }
            else
            {
                passwordmsg.Content = "Passwords do not match";
                passwordmsg.Foreground = System.Windows.Media.Brushes.Red;

            }
        }

        private void BtnRest_Click(object sender, RoutedEventArgs e)
        {
            var hashedPassword = PasswordBox2.Password;
            var existingUser = xmldoc.Descendants("Admin")
                .FirstOrDefault(admin => (string)admin.Element("Email") == receivedData);

            if (existingUser != null && PasswordBox2.Password != "" && PasswordBox1.Password != "" && PasswordBox2.Password == PasswordBox1.Password)
            {
                existingUser.Element("Password").Value = hashedPassword;

                xmldoc.Save(URL_XML_FILE);
                MessageBox.Show("Reset Password successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow Mw = new MainWindow();
                Mw.Show();
                this.Close();
            }
            else if (PasswordBox2.Password != PasswordBox1.Password)
            { 
                MessageBox.Show("Password is not matching", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetFields();
            }
            else
                {
                MessageBox.Show("Please enter your password", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetFields();
            }

        }
        private void ResetFields()
        {
            
            PasswordBox2.Clear();
            PasswordBox1.Clear();
            
        }
    }
}
