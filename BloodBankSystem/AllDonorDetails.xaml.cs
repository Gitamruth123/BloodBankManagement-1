using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BloodBankSystem
{
    public partial class AllDonorDetails : Window
    {
        private readonly XDocument _xmldoc;
        private readonly string URL_XML_FILE = "../../../BloodBankData.xml";

        public AllDonorDetails()
        {
            InitializeComponent();
            _xmldoc = XDocument.Load(URL_XML_FILE);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDonorData();
        }

        private void LoadDonorData()
        {
            try
            {
                List<Donor> donors = new List<Donor>();

                foreach (var donorElement in _xmldoc.Descendants("Donor"))
                {
                    Donor donor = new Donor
                    {
                        ID = (int?)donorElement.Attribute("ID") ?? 0,
                        Name = (string)donorElement.Element("Fullname") ?? string.Empty,
                        FatherName = (string)donorElement.Element("FatherName") ?? string.Empty,
                        DOB = (string)donorElement.Element("DOB") ?? string.Empty,
                        Mobile = (string)donorElement.Element("Mobile") ?? string.Empty,
                        Gender = (string)donorElement.Element("Gender") ?? string.Empty,
                        Email = (string)donorElement.Element("Email") ?? string.Empty,
                        BloodGroup = (string)donorElement.Element("BloodGroup") ?? string.Empty,
                        City = (string)donorElement.Element("City") ?? string.Empty,
                        Address = (string)donorElement.Element("Address") ?? string.Empty
                    };
                    donors.Add(donor);
                }

                dataGrid.ItemsSource = donors;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDataGrid(dataGrid);
        }

        private void PrintDataGrid(DataGrid grid)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                grid.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                grid.Arrange(new Rect(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight)));

                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    VisualBrush visualBrush = new VisualBrush(grid);
                    drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(0, 0), new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight)));
                }

                printDialog.PrintVisual(drawingVisual, "Printing DataGrid");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
