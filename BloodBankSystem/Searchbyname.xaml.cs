using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace BloodBankSystem
{
    public partial class Searchbyname : Window
    {
        private XDocument xmldoc;
        private string URL_XML_FILE = "../../../BloodBankData.xml";
        private List<Donor> donors;

        public Searchbyname()
        {
            InitializeComponent();
            LoadData(); 
        }

        private void LoadData()
        {
            xmldoc = XDocument.Load(URL_XML_FILE);

            // Populate the list with donor data
            donors = xmldoc.Descendants("Donor")
                            .Select(donorElement => new Donor
                            {
                                ID = (int)donorElement.Attribute("ID"),
                                Name = (string)donorElement.Element("Fullname"),
                                FatherName = (string)donorElement.Element("FatherName"),
                                DOB = (string)donorElement.Element("DOB"),
                                Mobile = (string)donorElement.Element("Mobile"),
                                Gender = (string)donorElement.Element("Gender"),
                                Email = (string)donorElement.Element("Email"),
                                BloodGroup = (string)donorElement.Element("BloodGroup"),
                                City = (string)donorElement.Element("City"),
                                Address = (string)donorElement.Element("Address")
                            }).ToList();

            dataGrid.ItemsSource = donors;
        }

        private void txtsName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtsName.Text.ToLower();

            var filteredDonors = donors.Where(donor => donor.Name?.ToLower().Contains(searchText) == true).ToList();

            if (filteredDonors.Any())
            {
                dataGrid.ItemsSource = filteredDonors;
            }
            else
            {
                dataGrid.ItemsSource = null;
                MessageBox.Show("No records found.");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
