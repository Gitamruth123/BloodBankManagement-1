using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace BloodBankSystem
{
    public partial class MainWindow : Window
    {
        private XDocument xmldoc;
        private readonly string URL_XML_FILE = "../../../AdminRegistration.xml";
        public MainWindow()
        {
            InitializeComponent();
            xmldoc = XDocument.Load(URL_XML_FILE);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var targetUsername = txtUsername.Text;
            var targetPassword = txtPassword.Password;

            var user = xmldoc.Descendants("Admin")
                            .FirstOrDefault(admin => (string)admin.Element("Username") == targetUsername &&
                                                     (string)admin.Element("Password") == targetPassword);

            if (user != null)
            {
                Dashboard db = new Dashboard();
                db.Show();
                this.Hide(); 
            }
            else
            {
                
               MessageBox.Show(" Username or Password is Incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            ResetFields();
            }
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            // Enable the login button if the checkbox is checked
            if (CheckBox1.IsChecked == true)
            {
                btnLogin.IsEnabled = true;
            }
            else
            {
                btnLogin.IsEnabled = false;
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = false;
        }

        private void btnExist_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void linkReg_Click(object sender, RoutedEventArgs e)
        {
            AdminReg ar = new AdminReg();
            ar.Show();
        }

        private void ResetPass_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword fp = new ForgotPassword();
            fp.Show();
        }
        private void ResetFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            CheckBox1.IsChecked = false;

        }
    }
}
