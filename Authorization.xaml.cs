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
    /// Логика взаимодействия для authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        ДипломEntities entities = new ДипломEntities();

        public Authorization()
        {
            InitializeComponent();
        }

        private void Winwow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
           var user = entities.Пользователи.FirstOrDefault(_ => _.Логин.Equals(txtUser.Text) && _.Пароль.Equals(txtPass.Password));

            if(user is null)
            {
                HintLabel.Content = "Неверный логин или пароль !";
                HintLabel.Visibility = Visibility.Visible;
            }
            else
            {
                switch (user.IDРоли)
                {
                    case 0: 
                        return; 
                    case 1: {
                            UserConfiguration.Usertype = UserConfiguration.UserTypes.Admin;
                        } break;
                    case 2:
                        {
                            UserConfiguration.Usertype = UserConfiguration.UserTypes.Manager;
                        } break;
                    case 3:
                        {
                            UserConfiguration.Usertype = UserConfiguration.UserTypes.Employer;
                        } break;
                }

                UserConfiguration.UserInfo = new UserConfiguration.User()
                {
                    Id = user.IDПользователя,
                    Name = user.Имя,
                    SurName = user.Фамилия,
                    Role = UserConfiguration.Usertype
                };

                SelectAService Menu = new SelectAService();
                Menu.Show();
                this.Close();
            }
        }

        //private void btnNoLogin_Click(object sender, RoutedEventArgs e)
        //{
        //    UserConfiguration.Usertype = UserConfiguration.UserTypes.Client;
        //}
    }
}
