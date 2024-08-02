using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace BloodBankSystem
{
    public partial class Searchbygroup : Window
    {
        private XDocument xmldoc;
        private string URL_XML_FILE = "../../../BloodBankData.xml";
        private List<Donor> donors;

        public Searchbygroup()
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

            // Bind the list to DataGrid
            dataGrid.ItemsSource = donors;
        }

        private void txtBloodgroup_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtBloodgroup.Text.ToLower();

            // Filter the list based on search text
            var filteredDonors = donors.Where(donor => donor.BloodGroup?.ToLower().Contains(searchText) == true).ToList();

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
    }
}
