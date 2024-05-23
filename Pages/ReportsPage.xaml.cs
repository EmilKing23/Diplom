using Avalonia.Controls;
using DiplomKarakuyumjyan.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static DiplomKarakuyumjyan.Pages.ClientsPage;

namespace DiplomKarakuyumjyan
{
    public partial class ReportsPage : Page
    {
        ДипломEntities Context = new ДипломEntities();

        public ReportsPage()
        {
            InitializeComponent();
            ServicesCollection = new List<ВидыРабот>();
            OrderDateFilter.SelectedDate = DateTime.Now; 
            foreach (var item in Context.ВидыРабот)
            {
                ServicesCollection.Add(item);
            }
            ServicesComboBox.ItemsSource = ServicesCollection;
             SelectedReport = new Reports
            {
                Id = 23,
                Client = new Clients
                {
                    Adress = "Ул. Пушкина дом 14",
                    Name = "Остапенко Анатолий",
                    Phone = "+79882034455"
                },
                Order = new Orders
                {
                    DateEnd="10.02.2024",
                    DateStart = "2.02.2024",
                    Status = "Выполнено",
                    ManagerName = "Валентина Стрыкало",
                    EmployerName = "Максим Коржев",
                    Service = "Межевание земельного участка"
                }

                 
            };

            Заявки searchOrder;
            Клиенты client;
            Работники employer;

            ReportsList = new ObservableCollection<Reports>();
            ReportsList.Add(SelectedReport);
            foreach (var item in Context.Отчеты)
            {
                searchOrder = Context.Заявки.First(_ => _.IDЗаявки.Equals(item.IDЗаявки));
                client = Context.Клиенты.First(_ => _.IDКлиента.Equals(item.Заявки.IDКлиента));
                employer = Context.Работники.First(_ => _.IDРаботника.Equals(item.Заявки.IDРаботника));

                ReportsList.Add(new Reports()
                {
                    Id = item.IDОтчета,
                    Description = item.ОписаниеРабот,
                    Client = new Clients()
                    {
                        Name = $"{client.Фамилия} {client.Имя} {client.Отчество}",

                        Phone = $"{client.НомерТелефона}"
                    },
                    Order = new Orders
                    {
                        DateEnd = searchOrder.ПлановаяДатаОкончанияРабот.Value.ToString(),
                        DateStart = searchOrder.ПлановаяДатаНачалаРабот.Value.ToString(),
                        Status = searchOrder.СтатусРаботы.Наименование,
                    }

                });
            }
            ListBoxClients.ItemsSource = ReportsList.ToList();
        }
        public List<ВидыРабот> ServicesCollection { get; set; }

        public Reports SelectedReport { get; set; }

        public ObservableCollection<Reports> DisplaySelectedReportCollection {get;set;}

        public ObservableCollection<Reports> ReportsList { get; set; }

        public class Reports
        {

            public int Id { get; set; }
            public string Description { get; set; }
            public Orders Order { get; set; }
            public string Employer { get; set; }
            public string Adress { get; set; }
            public Clients Client { get; set; }
        }

        private void ListBoxClients_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedReport = ListBoxClients.SelectedItem as Reports;
            SelectedReportDisplay.ItemsSource = DisplaySelectedReportCollection = new ObservableCollection<Reports> { SelectedReport };

        }
    }
}