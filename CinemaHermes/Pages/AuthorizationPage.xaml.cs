using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CinemaHermes.Pages
{
    /// <summary>
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        private int _passwordWrongEnterCount;
        private int _maxPasswordWrongEnterCount = 3;
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void AuthorizationBtnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbLogin.Text))
            {
                MessageBox.Show("Поле логин не заполнено!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(pbPassword.Password))
            {
                MessageBox.Show("Поле пароль не заполнено!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dbUser = App.Connection.User.FirstOrDefault(x => x.Login == tbLogin.Text);

            if (dbUser == null)
            {
                MessageBox.Show("Пользователя с таким логином не существует!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(Properties.Settings.Default.Blocker < DateTime.Now))
            {
                MessageBox.Show("Действует блокировка ввода пароля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dbUser.Password != pbPassword.Password)
            {
                _passwordWrongEnterCount++;
                if (_passwordWrongEnterCount >= _maxPasswordWrongEnterCount)
                {
                    Properties.Settings.Default.Blocker = DateTime.Now.AddMinutes(1);
                    Properties.Settings.Default.Save();
                    _passwordWrongEnterCount = 0;
                }
                MessageBox.Show("Неверный пароль!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (chbRemember.IsChecked == true)
            {
                Properties.Settings.Default.Login = tbLogin.Text;
                Properties.Settings.Default.Save();
            }

            NavigationService.Navigate(new ClientsPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tbLogin.Text = Properties.Settings.Default.Login;
        }

        private void RegistrationBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
