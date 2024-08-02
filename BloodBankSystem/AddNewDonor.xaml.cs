using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Xml;

namespace BloodBankSystem
{
    public partial class AddNewDonor : Window
    {
        private readonly string URL_XML_FILE = "../../../BloodBankData.xml";

        public AddNewDonor()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            int latestId = GetLatestId();
            txtID.Content = (latestId + 1).ToString();
        }

        private int GetLatestId()
        {
            int latestId = 1;

            if (File.Exists(URL_XML_FILE))
            {
                using (XmlReader reader = XmlReader.Create(URL_XML_FILE))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Donor")
                        {
                            if (reader.HasAttributes)
                            {
                                int id = int.Parse(reader.GetAttribute("ID"));
                                if (id >= latestId)
                                {
                                    latestId = id;
                                }
                            }
                        }
                    }
                }
            }

            return latestId;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidInput())
            {
                string addressText = new TextRange(txtAddress.Document.ContentStart, txtAddress.Document.ContentEnd).Text.Trim();
                AddDonorToXml(txtID.Content.ToString(), txtName.Text, txtFather.Text, txtDOB.Text, txtMobile.Text, txtGender.Text, txtEmail.Text, txtBloodGroup.Text, txtCity.Text, addressText);
                MessageBox.Show("Successfully added donor", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetFields();
            }
            else
            {
                MessageBox.Show("Please fill all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddDonorToXml(string id, string fullname, string fatherName, string dob, string mobile, string gender, string email, string bloodGroup, string city, string address)
        {
            string tempFileName = URL_XML_FILE + ".tmp";

            using (XmlReader reader = XmlReader.Create(URL_XML_FILE))
            using (XmlWriter writer = XmlWriter.Create(tempFileName, new XmlWriterSettings { Indent = true }))
            {
                bool rootWritten = false;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "BloodBank")
                    {
                        if (!rootWritten)
                        {
                            writer.WriteStartElement("BloodBank");
                            rootWritten = true;
                        }
                        writer.WriteNode(reader, true);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "BloodBank")
                    {
                        writer.WriteNode(reader, true);
                    }
                }

                if (!rootWritten)
                {
                    writer.WriteStartElement("BloodBank");
                }

                writer.WriteStartElement("Donor");
                writer.WriteAttributeString("ID", id);
                writer.WriteElementString("Fullname", fullname);
                writer.WriteElementString("FatherName", fatherName);
                writer.WriteElementString("DOB", dob);
                writer.WriteElementString("Mobile", mobile);
                writer.WriteElementString("Gender", gender);
                writer.WriteElementString("Email", email);
                writer.WriteElementString("BloodGroup", bloodGroup);
                writer.WriteElementString("City", city);
                writer.WriteElementString("Address", address);
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

        private bool IsValidInput()
        {
            string addressText = new TextRange(txtAddress.Document.ContentStart, txtAddress.Document.ContentEnd).Text.Trim();

            return !string.IsNullOrWhiteSpace(txtName.Text) &&
                   !string.IsNullOrWhiteSpace(txtFather.Text) &&
                   !string.IsNullOrWhiteSpace(txtDOB.Text) &&
                   !string.IsNullOrWhiteSpace(txtMobile.Text) &&
                   !string.IsNullOrWhiteSpace(txtGender.Text) &&
                   !string.IsNullOrWhiteSpace(txtEmail.Text) &&
                   !string.IsNullOrWhiteSpace(txtBloodGroup.Text) &&
                   !string.IsNullOrWhiteSpace(txtCity.Text) &&
                   !string.IsNullOrWhiteSpace(addressText);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();
        }

        private void ResetFields()
        {
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
