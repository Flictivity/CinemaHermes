using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CinemaHermes.Pages
{
    /// <summary>
    /// Interaction logic for ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var clients = App.Connection.Client.ToList();
            lvClients.ItemsSource = clients;
        }
    }
}
