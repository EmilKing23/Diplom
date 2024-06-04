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
            FormCollections();

        }

        private void FormCollections()
        {
            foreach (var item in context.Пользователи.Where(_=>_.Роли.Наименование.Equals("Сотрудник")))
            {
                var count = context.Заявки.Where(_ => _.IDПользователя.Equals(item.IDПользователя)).Count();
                EmployersCollection.Add(new Employers
                {
                    Id = item.IDПользователя,
                    Name = $"{item.Фамилия} {item.Имя}",
                    OrdersCount = count
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
            try
            {
                var service = ServicesComboBox.SelectedItem as Services;
                var employer = EmployersComboBox.SelectedItem as Employers;
                if( clientsPage.SelectedClient is null || DateEndCombo.SelectedDate is null || DateStartCombo.SelectedDate is null || CityTextBox.Text == string.Empty || service.Id == 0 || employer.Id == 0 ) {
                    MessageBox.Show("Присутствуют пустые поля!");
                    return;
                }
                Заявки order = new Заявки()
                {
                    IDКлиента = clientsPage.SelectedClient.IDКлиента,
                    ПлановаяДатаНачалаРабот = DateStartCombo.SelectedDate,
                    ПлановаяДатаОкончанияРабот = DateEndCombo.SelectedDate,
                    Адрес = $"{CityTextBox.Text} {StreetTextBox.Text} {LiterTextBox.Text}",
                    IDВида = service.Id,
                    IDПользователя = UserConfiguration.UserInfo.Id,
                    IDСтатуса = 1,
                    IDРаботника = employer.Id
                };
                context.Заявки.Add(order);
                context.SaveChanges();
                MessageBox.Show("Заявка добавлена!");
                FormCollections();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка при добавлении заявки: "+ex.ToString());
            }
           
        }

        public ObservableCollection<Services> ServicesCollection { get; set; } = new ObservableCollection<Services>();
        public ObservableCollection<Employers> EmployersCollection { get; set; } = new ObservableCollection<Employers>();
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
            public int OrdersCount { get; set; }
        }

        private void EmployersComboBox_SelectionChanged()
        {

        }
    }
}