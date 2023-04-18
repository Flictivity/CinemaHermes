using System.Windows;
using System.Windows.Controls;

namespace CinemaHermes.Pages
{
    /// <summary>
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
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

            if (chbRemember.IsChecked == true)
            {
                Properties.Settings.Default.Login = tbLogin.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tbLogin.Text = Properties.Settings.Default.Login;
        }
    }
}
