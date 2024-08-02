using System;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Documents;
using System.Windows.Controls;

namespace BloodBankSystem
{
    public partial class UpdateDonorDetails : Window
    {
        private XDocument xmldoc;
        private readonly string URL_XML_FILE = "../../../BloodBankData.xml";

        public UpdateDonorDetails()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataBloodBank();
        }

        public void LoadDataBloodBank()
        {
            using (XmlReader reader = XmlReader.Create(URL_XML_FILE))
            {
                xmldoc = XDocument.Load(reader);
            }

            var data = xmldoc.Descendants("Donor").Select(p => new
            {
                Fullname = p.Element("Fullname")?.Value,
                FatherName = p.Element("FatherName")?.Value,
                DOB = p.Element("DOB")?.Value,
                Mobile = p.Element("Mobile")?.Value,
                Gender = p.Element("Gender")?.Value,
                Email = p.Element("Email")?.Value,
                BloodGroup = p.Element("BloodGroup")?.Value,
                City = p.Element("City")?.Value,
                Address = p.Element("Address")?.Value
            }).OrderBy(p => p.Fullname).ToList();

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(UtxtID.Text, out int targetId))
            {
                var targetElement = xmldoc.Descendants("Donor")
                    .FirstOrDefault(element => (int)element.Attribute("ID") == targetId);

                if (targetElement != null)
                {
                    txtName.Text = (string)targetElement.Element("Fullname");
                    txtFather.Text = (string)targetElement.Element("FatherName");
                    txtDOB.Text = (string)targetElement.Element("DOB");
                    txtMobile.Text = (string)targetElement.Element("Mobile");
                    txtGender.Text = (string)targetElement.Element("Gender");
                    txtEmail.Text = (string)targetElement.Element("Email");
                    txtBloodGroup.Text = (string)targetElement.Element("BloodGroup");
                    txtCity.Text = (string)targetElement.Element("City");

                    string address = (string)targetElement.Element("Address");
                    txtAddress.Document.Blocks.Clear();
                    txtAddress.Document.Blocks.Add(new Paragraph(new Run(address)));
                }
                else
                {
                    MessageBox.Show("Donor not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid ID", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(UtxtID.Text, out int targetId))
            {
                using (XmlReader reader = XmlReader.Create(URL_XML_FILE))
                {
                    xmldoc = XDocument.Load(reader);
                }

                var targetElement = xmldoc.Descendants("Donor")
                    .FirstOrDefault(element => (int)element.Attribute("ID") == targetId);

                if (targetElement != null)
                {
                    // Update the donor details
                    targetElement.Element("Fullname").Value = txtName.Text;
                    targetElement.Element("FatherName").Value = txtFather.Text;
                    targetElement.Element("DOB").Value = txtDOB.SelectedDate?.ToString("dd/MM/yyyy") ?? string.Empty;
                    targetElement.Element("Mobile").Value = txtMobile.Text;
                    targetElement.Element("Gender").Value = txtGender.Text;
                    targetElement.Element("Email").Value = txtEmail.Text;
                    targetElement.Element("BloodGroup").Value = txtBloodGroup.Text;
                    targetElement.Element("City").Value = txtCity.Text;

                    // Update address
                    string addressText = new TextRange(txtAddress.Document.ContentStart, txtAddress.Document.ContentEnd).Text.Trim();
                    targetElement.Element("Address").Value = addressText;

                    using (XmlWriter writer = XmlWriter.Create(URL_XML_FILE, new XmlWriterSettings { Indent = true }))
                    {
                        xmldoc.WriteTo(writer);
                    }

                    MessageBox.Show("Donor details updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadDataBloodBank();
                    ResetFields();
                }
                else
                {
                    MessageBox.Show("Donor not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid ID", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ResetFields()
        {
            UtxtID.Clear();
            txtName.Clear();
            txtFather.Clear();
            txtDOB.Text = string.Empty;
            txtMobile.Clear();
            txtGender.Text = string.Empty;
            txtEmail.Clear();
            txtBloodGroup.Text = string.Empty;
            txtCity.Clear();
            txtAddress.Document.Blocks.Clear();
        }
    }
}
