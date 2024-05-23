using DiplomKarakuyumjyan.Pages;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiplomKarakuyumjyan
{
    public partial class CreateRequestPage : Page
    {
        ClientsPage clientsPage = new ClientsPage();
        ДипломEntities context = new ДипломEntities();

        public CreateRequestPage()
        {
            InitializeComponent();
      
            ClientsFrame.Content = clientsPage;
            ServicesCollection = new ObservableCollection<Services>() ;
            EmployersCollection = new ObservableCollection<Employers>();
            foreach (var item in context.Работники)
            {
                EmployersCollection.Add(new Employers
                {
                    Id = item.IDРаботника,
                    Name = $"{item.Фамилия} {item.Имя}",
                    Vaccancy = item.Должность
                });
            }
            foreach (var item in context.ВидыРабот)
            {
                ServicesCollection.Add(new Services
                {
                    Id = item.IDВида,
                    Name = $"{item.Наименование}",
                    Price = decimal.ToDouble(item.Цена)
                });
            }

            ServicesComboBox.ItemsSource = ServicesCollection;
                EmployersComboBox.ItemsSource = EmployersCollection;

        }

        private void btnCreatRequest_Click(object sender, RoutedEventArgs e)
        {

        }

        public ObservableCollection<Services> ServicesCollection { get; set; }
        public ObservableCollection<Employers> EmployersCollection { get; set; }
        public class Services
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
        }

        public class Employers
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Vaccancy { get; set; }
        }

        private void EmployersComboBox_SelectionChanged()
        {

        }
    }
}