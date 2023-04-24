using CinemaHermes.Components;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace CinemaHermes.Pages
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        private Client _client = new Client();
        private bool _isEdit = false;
        public ClientPage(Client client)
        {
            InitializeComponent();
            if (client != null)
            {
                _isEdit = true;
                _client = client;
            }


            cbGender.ItemsSource = App.Connection.Gender.ToList();
            DataContext = _client;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tbId.Visibility = _isEdit ? Visibility.Visible : Visibility.Collapsed;
            tblId.Visibility = _isEdit ? Visibility.Visible : Visibility.Collapsed;
            ShowFreeTags();
        }

        private void BackBtnClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void SaveBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var fullnameRegex = "^[А-Яа-я A-Za-z -]+$";

                if (_client.Lastname.Length > 50 || _client.Firstname.Length > 50 || _client.Patronymic.Length > 50)
                {
                    MessageBox.Show("Фамилия, имя или отчество не могут быть длиннее 50 символов", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!Regex.IsMatch(_client.Firstname, fullnameRegex)
                    || !Regex.IsMatch(_client.Lastname, fullnameRegex)
                    || !Regex.IsMatch(_client.Patronymic, fullnameRegex))
                {
                    MessageBox.Show("Неверное ФИО", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var allowedSymbols = new List<int> { '(', ')', ' ', '+', '-' }.Concat(Enumerable.Range('0', '9').ToList()).ToList();
                if (!_client.PhoneNumber.All(x => allowedSymbols.Contains(x)))
                {
                    MessageBox.Show("Неверный номер телефона", "Ошибка",
                       MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var splitEmail = _client.Email.Split('@');
                if (splitEmail.Length != 2 || !splitEmail.All(x => x.Length >= 1))
                {
                    MessageBox.Show("Неверный email", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!_isEdit)
                {
                    _client.AddedDate = DateTime.Now;
                }
                _client.IsDeleted = false;
                App.Connection.Client.AddOrUpdate(_client);
                App.Connection.SaveChanges();
                MessageBox.Show("Успешно", "Сообщение", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch
            {
                MessageBox.Show($"Ошибка!",
        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lvTags_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var tag = ((ListView)sender).SelectedItem as Tag;

                if (tag == null)
                {
                    return;
                }

                var clientTag = new ClientTag()
                {
                    Tag = tag,
                    Client = _client
                };

                _client.ClientTag.Add(clientTag);

                UpdateUserTags();
            }
            catch
            {
                MessageBox.Show($"Ошибка добавления тега!",
        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowFreeTags()
        {
            var freeTags = new List<Tag>();
            var allTags = App.Connection.Tag.ToList();

            foreach (var clientTag in _client.ClientTag)
            {
                allTags.Remove(clientTag.Tag);
            }

            freeTags = allTags;
            lvTags.ItemsSource = freeTags;
        }

        private void UpdateUserTags()
        {
            lvClientTags.Items.Refresh();
            ShowFreeTags();
        }

        private void DeleteBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var tag = (ClientTag)((Button)sender).Tag;

                if (tag == null)
                {
                    return;
                }

                _client.ClientTag.Remove(tag);

                if (tag.Id != 0)
                {
                    App.Connection.ClientTag.Remove(tag);
                    App.Connection.SaveChanges();
                }

                UpdateUserTags();
            }
            catch
            {
                MessageBox.Show($"Ошибка удаления тега!",
        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddImageBtnClick(object sender, RoutedEventArgs e)
        {
            var window = new OpenFileDialog();
            window.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";

            if (window.ShowDialog() != true)
            {
                MessageBox.Show($"не выбрано изоражение!",
        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var byteArray = File.ReadAllBytes(window.FileName);
            _client.Photo = byteArray;

            BindingOperations.GetBindingExpressionBase(imgClientPhoto, Image.SourceProperty).UpdateTarget();
        }
    }
}
