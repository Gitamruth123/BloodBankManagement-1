using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class StockDecrease : Window
    {
        private XDocument xmldoc;
        private string URL_XML_FILE = "../../../StockData.xml";
        public StockDecrease()
        {
            InitializeComponent();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                xmldoc = XDocument.Load(URL_XML_FILE);

                List<Stock> stocks = new List<Stock>();
                foreach (var stockElement in xmldoc.Descendants("Stock"))
                {
                    stocks.Add(new Stock
                    {
                        ID = (int)stockElement.Attribute("ID") ,
                        BloodGroup = (string)stockElement.Element("BloodGroup"),
                        Quantity = (int)stockElement.Element("Quantity") 
                    });
                }

                dataGrid.ItemsSource = stocks; 
                dataGrid.Columns.Clear();
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "BloodGroup", Binding = new Binding("BloodGroup"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Quantity", Binding = new Binding("Quantity"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public class Stock  
        {
            public int ID { get; set; }
            public string BloodGroup { get; set; }
            public int Quantity { get; set; }
        }

        private void btnDecrease_Click(object sender, RoutedEventArgs e)
        {
            var Bgroup = txtGroup.Text;
            XElement pat = xmldoc.Descendants("Stock").FirstOrDefault(e => e.Descendants().Any(c => c.Value == Bgroup));
            if (pat != null)
            {
                int Quantityv = int.Parse(pat.Element("Quantity").Value);
                int Total = Quantityv - int.Parse(txtQuantity.Text);
                pat.Element("Quantity").Value = Total.ToString();
                xmldoc.Save(URL_XML_FILE);
                Grid_Loaded(this, null);
                ResetFields();

            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ResetFields()
        {

            txtGroup.Text = string.Empty; ;
            txtQuantity.Text = string.Empty;

        }
    }
}
