using System;
using System.Collections.Generic;
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

namespace DiplomKarakuyumjyan
{
    /// <summary>
    /// Логика взаимодействия для SelectAService.xaml
    /// </summary>
    public partial class SelectAService : Window
    {
        public SelectAService()
        {
            InitializeComponent();
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            ReportsPage page = new ReportsPage();
            MainWindowFrame.Content = page;
        }

        private void btnCreatRequest_Click(object sender, RoutedEventArgs e)
        {
            CreateRequestPage page = new CreateRequestPage();
            MainWindowFrame.Content = page;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Winwow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Authorization zat = new Authorization();
            zat.Show();
            Close();
        }
    }
}