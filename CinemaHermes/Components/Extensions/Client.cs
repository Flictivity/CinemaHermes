using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CinemaHermes.Components
{
    public partial class Client
    {
        public int VisitCount => Visit.Count;
        public DateTime VisitDate
        {
            get
            {
                if (VisitCount > 0)
                {
                    return Visit.OrderBy(x => x.Date).Last().Date;
                }
                return DateTime.Now;
            }
        }

        public Visibility HaveVisits => VisitCount > 0 ? Visibility.Visible : Visibility.Collapsed;

        public List<ClientTag> Tags => ClientTag.Where(x => x.ClientId == Id).ToList();
    }
}
