using CinemaHermes.Components;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CinemaHermes.Pages
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void RegistrationBtnClick(object sender, RoutedEventArgs e)
        {
            try
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

                var existUser = App.Connection.User.Any(x => x.Login == tbLogin.Text);
                if (existUser)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var newUser = new User
                {
                    Login = tbLogin.Text,
                    Password = pbPassword.Password
                };

                if (!CheckPasswordMatch(newUser.Password))
                {
                    MessageBox.Show("Пароль не соответсвует требованиям", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                App.Connection.User.Add(newUser);
                App.Connection.SaveChanges();
                MessageBox.Show("Усешно", "Сообщение",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new AuthorizationPage());
            }
            catch
            {
                MessageBox.Show("Ошибка", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AuthorizationBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }

        private bool CheckPasswordMatch(string password)
        {
            int specialSymbolCount = 0;
            int upperSymbolCount = 0;
            int numCount = 0;

            if (password.Length < 6)
            {
                return false;
            }
            for (int i = 0; i < password.Length; ++i)
            {
                if (password[i] == '!'
                    || password[i] == '@'
                    || password[i] == '#'
                    || password[i] == '$'
                    || password[i] == '%'
                    || password[i] == '^')
                {
                    specialSymbolCount++;
                }
                if (char.IsDigit(password[i]))
                {
                    numCount++;
                }
                if (char.IsUpper(password[i]))
                {
                    upperSymbolCount++;
                }
            }

            return specialSymbolCount > 0 && upperSymbolCount > 0 && numCount > 0;
        }
    }
}
