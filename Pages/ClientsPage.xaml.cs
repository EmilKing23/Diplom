using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static DiplomKarakuyumjyan.Pages.ClientsPage;

namespace DiplomKarakuyumjyan.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        ДипломEntities context = new ДипломEntities();
        public ClientsPage()
        {
            InitializeComponent();
            if (UserConfiguration.Usertype == UserConfiguration.UserTypes.Employer)
            {
                ClientDeleteOperation.Visibility = Visibility.Hidden;
                ClientChangeOperation.Visibility = Visibility.Hidden;
            }
            if (UserConfiguration.Usertype == UserConfiguration.UserTypes.Admin)
            {
                ClientDeleteOperation.Visibility = Visibility.Visible;
                ClientChangeOperation.Visibility = Visibility.Visible;
            }
            GetClients();
        }
        public Клиенты SelectedClient { get; set; }

        public IEnumerable<Клиенты> ClientsListSearch { get; set; }
        public ObservableCollection<Клиенты> ClientsList { get; set; }
        public class Clients
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Adress { get; set; }
        }

        private void GetClients()
        {
            ClientsList = new ObservableCollection<Клиенты>();
            foreach (var client in context.Клиенты)
            {
                ClientsList.Add(client);
            }
            ListBoxClients.ItemsSource = ClientsList;
        }


        private void ListBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedClient = ListBoxClients.SelectedItem as Клиенты;
            if (SelectedClient is null) return;
            SelectedUserNameText.Text = SelectedClient.Имя;
            SelectedUserSurnNameText.Text = SelectedClient.Фамилия;
            SelectedUserPatronymicText.Text = SelectedClient.Отчество;
            SelectedUserAdressText.Text = SelectedClient.Почта;
            SelectedUserPhoneText.Text = SelectedClient.НомерТелефона;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = TextBoxSearch.Text;
            if (string.IsNullOrEmpty(search)) ListBoxClients.ItemsSource = ClientsList;
            ClientsListSearch = ClientsList.Where(_ =>
            _.Имя.Contains(search) ||
             _.Фамилия.Contains(search) ||
              _.Отчество.Contains(search) ||
            _.НомерТелефона.Contains(search) ||
            _.Почта.Contains(search)
            );
            ClientsListSearch.ToList();

            if (ClientsListSearch.Count() == 0) ListBoxClients.ItemsSource = ClientsList;
            else
            {
                labelSearchCount.Content = $"Найдено: {ClientsListSearch.Count()}";
                ListBoxClients.ItemsSource = ClientsListSearch.ToList();

            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(ClientNameTxtBox.Text) ||
            string.IsNullOrEmpty(ClientSurNameTxtBox.Text) ||
            string.IsNullOrEmpty(ClientPatronymicNameTxtBox.Text) ||
            string.IsNullOrEmpty(ClientPhoneTxtBox.Text) ||
            string.IsNullOrEmpty(ClientEmailTxtBox.Text))
            {
                MessageBox.Show("Пустое поле!");
                return;
            }
            if (!ClientEmailTxtBox.Text.Contains('@'))
            {
                MessageBox.Show("В поле 'Электронная Почта' указано неверное значение!");
                return;
            }
            string phone = ClientPhoneTxtBox.Text;
            bool IsPhoneValid = Int64.TryParse(phone, out var validPhone);
            if (IsPhoneValid == false || phone.Length != 10)
            {
                MessageBox.Show("В поле 'Номер телефона' указано неверное значение!");
                return;
            }
            if (!_internalChange)
            {
                Клиенты clients = new Клиенты
                {
                    Имя = ClientNameTxtBox.Text,
                    Фамилия = ClientSurNameTxtBox.Text,
                    Отчество = ClientPatronymicNameTxtBox.Text,
                    НомерТелефона = $"+7{validPhone.ToString()}",
                    Почта = ClientEmailTxtBox.Text,

                };
                var existItem = context.Клиенты.FirstOrDefault(_ => _.НомерТелефона.Equals(clients.НомерТелефона) || _.Почта.Equals(ClientEmailTxtBox.Text));
                if (existItem is null)
                {
                    context.Клиенты.Add(clients);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка при добавлении клиента \nПопробуйте позже");
                        return;
                    }
                    GetClients();
                    MessageBox.Show("Добавлен клиент ");
                    return;
                }
                else
                {
                    MessageBox.Show("В базе уже есть клиент с таким номером или почтой!");
                }
            }
            else
            {
                Клиенты clients = new Клиенты
                {
                    Имя = ClientNameTxtBox.Text,
                    Фамилия = ClientSurNameTxtBox.Text,
                    Отчество = ClientPatronymicNameTxtBox.Text,
                    НомерТелефона = $"+7{validPhone.ToString()}",
                    Почта = ClientEmailTxtBox.Text,

                };
                Клиенты клиенты = ListBoxClients.SelectedItem as Клиенты;
                context.Клиенты.FirstOrDefault(_ => _.НомерТелефона.Equals(клиенты.НомерТелефона) || _.Почта.Equals(клиенты.Почта)).Фамилия = clients.Фамилия;
                context.Клиенты.FirstOrDefault(_ => _.НомерТелефона.Equals(клиенты.НомерТелефона) || _.Почта.Equals(клиенты.Почта)).Отчество = clients.Отчество;
                context.Клиенты.FirstOrDefault(_ => _.НомерТелефона.Equals(клиенты.НомерТелефона) || _.Почта.Equals(клиенты.Почта)).Имя = clients.Имя;
                context.Клиенты.FirstOrDefault(_ => _.НомерТелефона.Equals(клиенты.НомерТелефона) || _.Почта.Equals(клиенты.Почта)).Почта = clients.Почта;
                context.Клиенты.FirstOrDefault(_ => _.НомерТелефона.Equals(клиенты.НомерТелефона) || _.Почта.Equals(клиенты.Почта)).НомерТелефона = clients.НомерТелефона;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка при изменении клиента \nПопробуйте позже");
                    return;
                }
                GetClients();
                MessageBox.Show("Клиент изменен ");
                AddClientbutton.Content = "Добавить";
                _internalChange = false;
                ClientSurNameTxtBox.Text = string.Empty;
                ClientNameTxtBox.Text = string.Empty;
                ClientPhoneTxtBox.Text = string.Empty;
                ClientEmailTxtBox.Text = string.Empty;
                ClientPatronymicNameTxtBox.Text = string.Empty;
                return;

            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxClients.SelectedItem is null)
            {
                MessageBox.Show("Выберите клиента !");
                return;
            }
            var res = MessageBox.Show($"Вы действительно хотите удалить: \n{SelectedUserNameText.Text}? Отменить это действие - невозможно! ", "", MessageBoxButton.YesNo);
            try
            {
                if (res == MessageBoxResult.Yes)
                {
                    Клиенты клиенты = ListBoxClients.SelectedItem as Клиенты;
                    context.Клиенты.Remove(клиенты);
                    context.SaveChanges();
                }
                GetClients();
            }
            catch (Exception)
            {
                MessageBox.Show("Клиент не может быть удалён! Клиент содержит заявки");
            }


        }
        private bool _internalChange;
        private void ChangeClient_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedClient is null)
            {
                MessageBox.Show("Выберите клиента для изменения!");
                return;
            }
            Клиенты client = ListBoxClients.SelectedItem as Клиенты;

            ClientNameTxtBox.Text = client.Имя;
            ClientSurNameTxtBox.Text = client.Фамилия;
            ClientPatronymicNameTxtBox.Text = client.Отчество;
            ClientEmailTxtBox.Text = client.Почта;
            ClientPhoneTxtBox.Text = client.НомерТелефона.TrimStart('+', '7');
            AddClientbutton.Content = "Подтвердить";
            _internalChange = true;
        }
    }
}
