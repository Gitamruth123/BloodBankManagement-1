using System;
using System.Windows;

namespace BloodBankSystem
{
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Addnew_Click(object sender, RoutedEventArgs e)
        {
            AddNewDonor addNewDonorWindow = new AddNewDonor();
            addNewDonorWindow.Show();
        }

        private void Updatedonor_Click(object sender, RoutedEventArgs e)
        {
            UpdateDonorDetails updateDonorDetailsWindow = new UpdateDonorDetails();
            updateDonorDetailsWindow.Show();
        }

        private void Alldonor_Click(object sender, RoutedEventArgs e)
        {
            AllDonorDetails allDonorDetailsWindow = new AllDonorDetails();
            allDonorDetailsWindow.Show();
        }

        private void ByName_Click(object sender, RoutedEventArgs e)
        {
            Searchbyname searchByNameWindow = new Searchbyname();
            searchByNameWindow.Show();
        }

        private void ByGroup_Click(object sender, RoutedEventArgs e)
        {
            Searchbygroup searchByGroupWindow = new Searchbygroup();
            searchByGroupWindow.Show();
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            StockIncrease stockIncreaseWindow = new StockIncrease();
            stockIncreaseWindow.Show();
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            StockDecrease stockDecreaseWindow = new StockDecrease();
            stockDecreaseWindow.Show();
        }

        private void Delete_Donor_Click(object sender, RoutedEventArgs e)
        {
            DeleteDonor dd = new DeleteDonor();
            dd.Show();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
