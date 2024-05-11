using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiplomKarakuyumjyan
{
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            InitializeComponent();
        }
        
        private void buttonНазад_Click(object sender, RoutedEventArgs e)
        {
            SelectAService Menu = new SelectAService();
            Menu.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            SelectAService Menu = new SelectAService();
            Menu.Show();
        }
    }
}