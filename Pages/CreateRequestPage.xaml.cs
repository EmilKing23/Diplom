using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiplomKarakuyumjyan
{
    public partial class CreateRequestPage : Page
    {
        
        public CreateRequestPage()
        {
            InitializeComponent();
           
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnCreatRequest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

        }

        // private void btnMinimize_Click(object sender, RoutedEventArgs e)
        // {
        //    
        // }
        // private void Winwow_MouseDown(object sender, MouseButtonEventArgs e)
        // {
        //     if (e.LeftButton == MouseButtonState.Pressed)
        //         DragMove();
        // }
    }
}