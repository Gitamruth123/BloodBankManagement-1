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

        private void LoadDataBloodBank()
        {
            try
            {
                xmldoc = XDocument.Load(URL_XML_FILE);

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
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(UtxtID.Text, out int targetId))
            {
                var targetElement = xmldoc.Descendants("Donor")
                    .FirstOrDefault(element => (int)element.Attribute("ID") == targetId);

                if (targetElement != null)
                {
                    txtName.Text = targetElement.Element("Fullname")?.Value;
                    txtFather.Text = targetElement.Element("FatherName")?.Value;
                    txtDOB.Text = targetElement.Element("DOB")?.Value;
                    txtMobile.Text = targetElement.Element("Mobile")?.Value;
                    txtGender.Text = targetElement.Element("Gender")?.Value;
                    txtEmail.Text = targetElement.Element("Email")?.Value;
                    txtBloodGroup.Text = targetElement.Element("BloodGroup")?.Value;
                    txtCity.Text = targetElement.Element("City")?.Value;

                    string address = targetElement.Element("Address")?.Value;
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
                var targetElement = xmldoc.Descendants("Donor")
                    .FirstOrDefault(element => (int)element.Attribute("ID") == targetId);

                if (targetElement != null)
                {
                    try
                    {
                        targetElement.Element("Fullname")?.SetValue(txtName.Text);
                        targetElement.Element("FatherName")?.SetValue(txtFather.Text);
                        targetElement.Element("DOB")?.SetValue(txtDOB.SelectedDate?.ToString("dd/MM/yyyy") ?? string.Empty);
                        targetElement.Element("Mobile")?.SetValue(txtMobile.Text);
                        targetElement.Element("Gender")?.SetValue(txtGender.Text);
                        targetElement.Element("Email")?.SetValue(txtEmail.Text);
                        targetElement.Element("BloodGroup")?.SetValue(txtBloodGroup.Text);
                        targetElement.Element("City")?.SetValue(txtCity.Text);

                        string addressText = new TextRange(txtAddress.Document.ContentStart, txtAddress.Document.ContentEnd).Text.Trim();
                        targetElement.Element("Address")?.SetValue(addressText);

                        xmldoc.Save(URL_XML_FILE);

                        MessageBox.Show("Donor details updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadDataBloodBank();
                        ResetFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating donor details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
