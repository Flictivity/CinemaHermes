using CinemaHermes.Components;
using System.Windows;

namespace CinemaHermes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CinemaRentalEntities Connection = new CinemaRentalEntities();
    }
}
