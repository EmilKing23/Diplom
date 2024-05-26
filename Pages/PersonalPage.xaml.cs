using DiplomKarakuyumjyan.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiplomKarakuyumjyan.Pages
{
    /// <summary>
    /// Логика взаимодействия для PersonalPage.xaml
    /// </summary>
    public partial class PersonalPage : Page
    {
        ДипломEntities entities = new ДипломEntities();
        public PersonalPage()
        {
            InitializeComponent();
            OrdersList = new List<Orders>();
            SetItemSources();

        }



        private void SetItemSources()
        {
            TODOList = new List<Orders>();
            InProgressList = new List<Orders>();
            WaitReportOrdersList = new List<Orders>();
            DoneOrdersList = new List<Orders>();
            OrdersList = new List<Orders>();
            foreach (var item in entities.Заявки.Where(_ => _.IDРаботника.Equals(UserConfiguration.UserInfo.Id)))
            {
                var service = entities.ВидыРабот.FirstOrDefault(_ => _.IDВида.Equals(item.IDВида)).Наименование;
                var status = entities.СтатусРаботы.FirstOrDefault(_ => _.IDСтатуса.Equals(item.IDСтатуса)).Наименование;
                var client = entities.Клиенты.FirstOrDefault(_ => _.IDКлиента.Equals(item.IDКлиента));
                OrdersList.Add(new Orders
                {
                    Id = item.IDЗаявки,
                    StatusId = item.IDСтатуса,
                    Adress = item.Адрес,
                    Service = service,
                    Status = status,
                    ClientName = $"{client.Фамилия} {client.Имя} {client.Отчество}",
                    DateStart = item.ПлановаяДатаНачалаРабот.ToString(),
                    DateEnd = item.ПлановаяДатаОкончанияРабот.ToString(),
                    Phone = client.НомерТелефона.ToString(),
                    Email = client.Почта.ToString(),
                });


            }
            TODOList = OrdersList.FindAll(_ => _.StatusId.Equals(1)).ToList();
            InProgressList = OrdersList.FindAll(_ => _.StatusId.Equals(2)).ToList();
            WaitReportOrdersList = OrdersList.FindAll(_ => _.StatusId.Equals(3)).ToList();
            DoneOrdersList = OrdersList.FindAll(_ => _.StatusId.Equals(4)).ToList();
            StatusChangeComboBox.ItemsSource = entities.СтатусРаботы.ToList();
            TODOListBox.ItemsSource = TODOList;
            InProgressListBox.ItemsSource = InProgressList;
            WaitReportListBox.ItemsSource = WaitReportOrdersList;
            DoneListBox.ItemsSource = DoneOrdersList;

        }
        public List<Orders> OrdersList { get; set; } = new List<Orders>();
        public List<Orders> TODOList { get; set; } = new List<Orders>();
        public List<Orders> InProgressList { get; set; } = new List<Orders>();
        public List<Orders> WaitReportOrdersList { get; set; } = new List<Orders>();
        public List<Orders> DoneOrdersList { get; set; } = new List<Orders>();
        public List<СтатусРаботы> StatusList { get; set; } = new List<СтатусРаботы>();

        private void StatusChangeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedOrder is null) return;
            StatusChange(SelectedOrder, StatusChangeComboBox.SelectedItem as СтатусРаботы);
            StatusChangeComboBox.SelectedItem = null;
        }

        private void InProgressListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedOrder = InProgressListBox.SelectedItem as Orders;
        }

        private void WaitReportListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedOrder = WaitReportListBox.SelectedItem as Orders;
        }

        private void DoneListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedOrder = DoneListBox.SelectedItem as Orders;
        }

        private void TODOListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedOrder = TODOListBox.SelectedItem as Orders;

        }

        public Orders SelectedOrder { get; set; }

        private void StatusChange(Orders orders, СтатусРаботы value)
        {
            entities.Заявки.First(_ => _.IDЗаявки.Equals(orders.Id)).СтатусРаботы = value;
            entities.SaveChanges();
            SetItemSources();
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrder is null) return;
            Отчеты duplicateOrder = entities.Отчеты.FirstOrDefault(_ => _.IDЗаявки.Equals(SelectedOrder.Id));
            if (duplicateOrder != null)
            {
                MessageBox.Show("К этой заявке уже добавлен отчёт!");
                return;
            }
            FileDialog fileDialog = new OpenFileDialog();
            bool file = (bool)fileDialog.ShowDialog();
            if (file)
            {
                entities.Отчеты.Add(new Отчеты
                {
                    IDЗаявки = SelectedOrder.Id,
                    ОписаниеРабот = fileDialog.FileName,
                });
                entities.SaveChanges();
            }
            if (MessageBox.Show("Отметить заявку как выполненнную?", "Отчёт добавлен", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                entities.Заявки.First(_ => _.IDЗаявки.Equals(SelectedOrder.Id)).IDСтатуса = 4;
                entities.SaveChanges();
                SetItemSources();
            }
        }

        private void RemoveReport_Click(object sender, RoutedEventArgs e)
        {
            Отчеты report = entities.Отчеты.FirstOrDefault(_ => _.IDОтчета.Equals(SelectedOrder.Id));
            entities.Отчеты.Remove(report);

        }
    }
}

