using CinemaHermes.Components;
using System;
using System.Linq;
using System.Windows.Controls;

namespace CinemaHermes.Pages
{
    /// <summary>
    /// Interaction logic for ClientVisitPage.xaml
    /// </summary>
    public partial class ClientVisitPage : Page
    {
        Client _client;

        Predicate<Visit> _filter = x => true;
        public ClientVisitPage(Client client)
        {
            InitializeComponent();

            _client = client;

            cbDayOfWeeks.SelectedIndex = 7;

            lvVisits.ItemsSource = _client.Visit.Where(x => _filter(x));
        }

        private void сbDayOfWeeksSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbDayOfWeeks.SelectedIndex)
            {
                case 0:
                    _filter = x => x.Date.DayOfWeek == DayOfWeek.Monday;
                    break;

                case 1:
                    _filter = x => x.Date.DayOfWeek == DayOfWeek.Tuesday;
                    break;

                case 2:
                    _filter = x => x.Date.DayOfWeek == DayOfWeek.Wednesday;
                    break;

                case 3:
                    _filter = x => x.Date.DayOfWeek == DayOfWeek.Thursday;
                    break;

                case 4:
                    _filter = x => x.Date.DayOfWeek == DayOfWeek.Friday;
                    break;

                case 5:
                    _filter = x => x.Date.DayOfWeek == DayOfWeek.Saturday;
                    break;

                case 6:
                    _filter = x => x.Date.DayOfWeek == DayOfWeek.Sunday;
                    break;

                default:
                    _filter = x => true;
                    break;
            }

            lvVisits.ItemsSource = _client.Visit.Where(x => _filter(x));
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
