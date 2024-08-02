
using System.Xml.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;   

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static BloodBankSystem.DeleteDonor;
using System.Collections.ObjectModel;


namespace BloodBankSystem
{
    public partial class DeleteDonor : Window
    {
        private readonly XDocument _xmldoc;
        private readonly string URL_XML_FILE = "../../../BloodBankData.xml";
        
        public DeleteDonor()
        {
            InitializeComponent();
            _xmldoc = XDocument.Load(URL_XML_FILE);

          
            dataGrid.ItemsSource = LoadDonors();
        }

        private List<Donor> LoadDonors()
        {
            var donors = new List<Donor>();
            try
            {
                foreach (var donorElement in _xmldoc.Descendants("Donor"))
                {
                    donors.Add(new Donor
                    {
                        ID = (int?)donorElement.Attribute("ID") ?? 0,
                        Name = (string)donorElement.Element("Fullname") ?? string.Empty,
                        FatherName = (string)donorElement.Element("FatherName") ?? string.Empty,
                        DOB = (string)donorElement.Element("DOB") ?? string.Empty,
                        Mobile = (string)donorElement.Element("Mobile") ?? string.Empty,
                        Gender = (string)donorElement.Element("Gender") ?? string.Empty,
                        Email= (string)donorElement.Element("Email") ?? string.Empty,
                        BloodGroup = (string)donorElement.Element("BloodGroup") ?? string.Empty,
                        City= (string)donorElement.Element("City") ?? string.Empty,
                        Address = (string)donorElement.Element("Address") ?? string.Empty,
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading donors: " + ex.Message);
            }
            return donors;
        }

        private void DeleteDonor_Click(object sender, RoutedEventArgs e)
        {
            var selectedDonor = dataGrid.SelectedItem as Donor;


            if (selectedDonor != null)
            {
                if (MessageBox.Show($"Are you sure you want to delete donor {selectedDonor.Name}?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    
                        var donorElement = _xmldoc.Descendants("Donor")
                            .FirstOrDefault(d => (int)d.Attribute("ID") == selectedDonor.ID);

                        if (donorElement != null)
                        {
                            donorElement.Remove();
                            _xmldoc.Save(URL_XML_FILE);
                           
                            MessageBox.Show("Donor deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Donor not found.");
                        }
                        dataGrid.ItemsSource = LoadDonors();
                }            }
            else
            {
                MessageBox.Show("Please select a donor to delete.");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class Donor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }

        public string Email { get; set; }
        public string BloodGroup { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

    }
}