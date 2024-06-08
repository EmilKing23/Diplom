using DiplomKarakuyumjyan.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static DiplomKarakuyumjyan.Pages.ClientsPage;

namespace DiplomKarakuyumjyan
{
    public partial class ReportsPage : Page
    {
        ДипломEntities Context = new ДипломEntities();

        public ReportsPage()
        {
            InitializeComponent();
            DropFilterButtonBorder.Visibility = Visibility.Hidden;
            ServicesCollection = new List<ВидыРабот>();
            OrderDateFilter.SelectedDate = DateTime.Now;
            foreach (var item in Context.ВидыРабот)
            {
                ServicesCollection.Add(item);
            }
            ServicesComboBox.ItemsSource = ServicesCollection;
            Заявки searchOrder;
            Клиенты client;
            Работники employer;

            ReportsList = new ObservableCollection<Reports>();
            foreach (var item in Context.Отчеты)
            {
                searchOrder = Context.Заявки.First(_ => _.IDЗаявки.Equals(item.IDЗаявки));
                client = Context.Клиенты.First(_ => _.IDКлиента.Equals(item.Заявки.IDКлиента));
                employer = Context.Работники.First(_ => _.IDРаботника.Equals(item.Заявки.IDРаботника));

                ReportsList.Add(new Reports()
                {
                    Id = item.IDОтчета,
                    Description = item.ОписаниеРабот,
                    Employer = $"{item.Заявки.Работники.Фамилия} {item.Заявки.Работники.Имя} {item.Заявки.Работники.Отчество}",

                    Client = new Clients()
                    {
                        Name = $"{client.Фамилия} {client.Имя} {client.Отчество}",
                        Adress = item.Заявки.Адрес,
                        Phone = $"{client.НомерТелефона}",
                    },
                    Order = new Orders
                    {
                        DateEnd = (DateTime)searchOrder.ПлановаяДатаОкончанияРабот,
                        DateStart = (DateTime)searchOrder.ПлановаяДатаНачалаРабот,
                        Status = searchOrder.СтатусРаботы.Наименование,
                        Service = searchOrder.ВидыРабот.Наименование,
                        EmployerName = $"{searchOrder.Работники.Фамилия} {searchOrder.Работники.Имя} {searchOrder.Работники.Отчество}",
                        ManagerName = $"{searchOrder.Пользователи.Фамилия} {searchOrder.Пользователи.Имя} {searchOrder.Пользователи.Отчество}",
                        Email = searchOrder.Клиенты.Почта
                    }
                });
            }
            ListBoxClients.ItemsSource = ReportsList.ToList();
        }
        public List<ВидыРабот> ServicesCollection { get; set; }

        public Reports SelectedReport { get; set; }

        public ObservableCollection<Reports> DisplaySelectedReportCollection { get; set; }

        public ObservableCollection<Reports> ReportsList { get; set; }

        public class Reports
        {
            public string Service { get; set; }
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

        private void TextBoxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxSearch.Text)) ListBoxClients.ItemsSource = ReportsList;
            ListBoxClients.ItemsSource = ReportsList.Where(_ =>
            _.Client.Name.Contains(TextBoxSearch.Text) ||
            _.Client.Name.Contains(TextBoxSearch.Text) ||
            _.Order.EmployerName.Contains(TextBoxSearch.Text) ||
            _.Order.Email.Contains(TextBoxSearch.Text));
            DropFilterButtonBorder.Visibility = Visibility.Visible;
        }

        private void ServicesComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ServicesComboBox.SelectedItem is null) return;
            ListBoxClients.ItemsSource = ReportsList.Where(_ => _.Order.Service.Equals(((DiplomKarakuyumjyan.ВидыРабот)ServicesComboBox.SelectedValue).Наименование));
            DropFilterButtonBorder.Visibility = Visibility.Visible;
        }

        private void DropFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ServicesComboBox.SelectedItem = null;
            TextBoxSearch.Text = string.Empty;
            OrderDateFilter.SelectedDate = null;
            ListBoxClients.ItemsSource = ReportsList;
            DropFilterButtonBorder.Visibility = Visibility.Hidden;
        }

        private void OrderDateFilter_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ReportsList is null) return;
            if (OrderDateFilterEnd.SelectedDate is null)
            {
                ListBoxClients.ItemsSource = ReportsList.Where(_ => _.Order.DateStart.Equals(OrderDateFilter.SelectedDate.ToString()));
                DropFilterButtonBorder.Visibility = Visibility.Visible;
            }
            else
            {
                List<Reports> list = ReportsList.Where(_ => _.Order.DateStart >= OrderDateFilter.SelectedDate || _.Order.DateStart.Equals(OrderDateFilter.SelectedDate) && _.Order.DateEnd <= OrderDateFilterEnd.SelectedDate || _.Order.DateEnd.Equals(OrderDateFilterEnd.SelectedDate)).ToList();
                DropFilterButtonBorder.Visibility = Visibility.Visible;
                ListBoxClients.ItemsSource = list;
            }
        }

        private void OrderDateFilterEnd_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ReportsList is null) return;
            if (OrderDateFilterEnd.SelectedDate is null)
            {
                ListBoxClients.ItemsSource = ReportsList.Where(_ => _.Order.DateStart.Equals(OrderDateFilter.SelectedDate.ToString()));
                DropFilterButtonBorder.Visibility = Visibility.Visible;
            }
            else
            {
                List<Reports> list = ReportsList.Where(_ => _.Order.DateStart >= OrderDateFilter.SelectedDate || _.Order.DateStart.Equals(OrderDateFilter.SelectedDate) && _.Order.DateEnd <= OrderDateFilterEnd.SelectedDate || _.Order.DateEnd.Equals(OrderDateFilterEnd.SelectedDate)).ToList();
                DropFilterButtonBorder.Visibility = Visibility.Visible;
                ListBoxClients.ItemsSource = list;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(SelectedReport.Description);
            }
            catch
            {
                MessageBox.Show("Файл не найден по указанному пути!");
            }

        }
    }
}