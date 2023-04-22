using CinemaHermes.Components;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CinemaHermes.Pages
{
    /// <summary>
    /// Interaction logic for ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        private int _itemsCount = 0;
        private int _pageNumber = 0;
        private List<Client> _clients;
        private List<Client> _clientsInDb;
        private bool _showBirthday = false;

        private Predicate<Client> _filter = x => true;
        private Predicate<Client> _birthdayFilter = x => true;
        private Func<Client, object> _sort = x => x.Id;
        public ClientsPage()
        {
            InitializeComponent();
            _clientsInDb = App.Connection.Client.Where(x => x.IsDeleted == false).ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbItemsCount.ItemsSource = PageItemsCount.ItemsCount;
            cbItemsCount.SelectedIndex = 0;

            cbFilter.ItemsSource = FilterMethods.Methods;
            cbFilter.SelectedIndex = 0;

            cbSorting.ItemsSource = SortingMethods.Methods;
            cbSorting.SelectedIndex = 0;

            PageChange(0);
            UpdateClients();
        }

        /// <summary>
        /// Обработка изменения количества элементов на одной странице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbItemsCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbItemsCount.SelectedIndex)
            {
                case 0:
                    _itemsCount = 10;
                    break;
                case 1:
                    _itemsCount = 50;
                    break;
                case 2:
                    _itemsCount = 200;
                    break;
                case 3:
                    _itemsCount = _clientsInDb.Count;
                    break;
            }
            UpdateClients();
        }

        /// <summary>
        /// Обработка изменения метода сортировки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbSorting.SelectedIndex)
            {
                case 0:
                    _sort = x => x.Firstname;
                    break;
                case 1:
                    _sort = x => x.VisitDate;
                    break;
                case 2:
                    _sort = x => x.VisitCount;
                    break;
                case 3:
                    _sort = x => x.Id;
                    break;
                default:
                    _sort = x => x.Id;
                    return;
            }
            UpdateClients();
        }

        /// <summary>
        /// Обработка изменения метода фильтрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbFilter.SelectedIndex)
            {
                case 0:
                    _filter = x => x.Gender1.ID == "м";
                    break;
                case 1:
                    _filter = x => x.Gender1.ID == "ж";
                    break;
                case 2:
                    _filter = x => true;
                    break;
                default:
                    _filter = x => true;
                    return;
            }
            UpdateClients();
        }

        /// <summary>
        /// Обработка нажатия кнопки для переклюяения на предыдущую старницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackInPageBtnClick(object sender, RoutedEventArgs e)
        {
            PageChange(-1);
            UpdateClients();
        }

        /// <summary>
        /// Обработка нажатия кнопки для переклюяения на следующую старницу
        /// </summary>w
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextBtnClick(object sender, RoutedEventArgs e)
        {
            PageChange(1);
            UpdateClients();
        }

        /// <summary>
        /// Полусение клиентов относительно заданной страницы
        /// </summary>
        /// <returns></returns>
        private List<Client> GetClientsToPage()
        {
            FilterAndSort();
            return _clients.Skip(_pageNumber * _itemsCount).Take(_itemsCount).ToList();
        }

        /// <summary>
        /// Фильтрация и сортировка списка клиентов
        /// </summary>
        private void FilterAndSort()
        {
            _clientsInDb = App.Connection.Client.Where(x => x.IsDeleted == false).ToList();
            _clients = _clientsInDb.Where(x => (_filter(x) && (_showBirthday ? _birthdayFilter(x) : true))).OrderBy(_sort).ToList();
            if (tbSearch.Text != "") Search();
        }

        private void UpdateClients()
        {
            _clients = GetClientsToPage();
            lvClients.ItemsSource = null;
            lvClients.ItemsSource = _clients;
            tbItemsCount.Text = $"{_clients.Count} из {_clientsInDb.Count}";
        }

        private void Search()
        {
            lvClients.ItemsSource = _clients
                .Where(x => string.Join(" ", x.Lastname, x.Firstname, x.Patronymic,
                x.Email, x.PhoneNumber).ToLower()
                .Contains(tbSearch.Text.ToLower()))
                .ToList();
            tbItemsCount.Text = $"{_clients.Count} из {_clientsInDb.Count}";
        }

        /// <summary>
        /// Метод для изменения номера страницы
        /// </summary>
        /// <param name="n"></param>
        private void PageChange(int n)
        {
            var a = _clientsInDb.Count % _itemsCount;
            int totalPages = 0;
            if (a == 0)
                totalPages = _clientsInDb.Count / _itemsCount;
            else
                totalPages = _clientsInDb.Count / _itemsCount + 1;

            _pageNumber += n;

            if (_pageNumber <= 0)
            {
                _pageNumber = 0;
                btnBack.IsEnabled = false;
            }
            else
            {
                btnBack.IsEnabled = true;
            }

            if (_pageNumber >= totalPages - 1)
            {
                btnNext.IsEnabled = false;
            }
            else
            {
                btnNext.IsEnabled = true;
            }

            tbPageNum.Text = $"{_pageNumber + 1} из {totalPages}";
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        /// <summary>
        /// Обработка нажатия кнопки, которая включает отборку клиентов, у которыз день рождения в этом месяце
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowBirthDaysBtnClick(object sender, RoutedEventArgs e)
        {
            _showBirthday = !_showBirthday;
            _birthdayFilter = x => x.BirthDate.Month == DateTime.Now.Month;
            btnBirthday.Background = _showBirthday ? Brushes.Green : (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE0DD46"); ;
            UpdateClients();
        }

        private void ExitBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void EditBtnClick(object sender, RoutedEventArgs e)
        {
            var client = (Client)(((Button)sender).Tag);
            NavigationService.Navigate(new ClientPage(client));
        }

        private void DeleteBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var clientId = ((Client)(((Button)sender).Tag)).Id;
                var client = App.Connection.Client.FirstOrDefault(x => x.Id == clientId);
                var clientVisits = App.Connection.Visit.Where(x => x.ClientId == clientId).ToList();

                if (clientVisits.Any())
                {
                    MessageBox.Show("Удаление клиента невозможно.\nВ системе все еще хранится информация о его посещениях.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                client.IsDeleted = true;

                App.Connection.Client.AddOrUpdate(client);
                App.Connection.SaveChanges();
                MessageBox.Show("Клиент успешно удален",
                    "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateClients();
            }
            catch
            {
                MessageBox.Show("Ошибка удаления",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lvClients_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var client = ((ListView)sender).SelectedItem as Client;
            NavigationService.Navigate(new ClientVisitPage(client));
        }

        private void CreateClientBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientPage(null));
        }
    }
}