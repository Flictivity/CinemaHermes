using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaHermes.Components
{
    public partial class Client
    {
        public int VisitCount => Visit.Count;
        public DateTime VisitDate => Visit.OrderBy(x => x.Date).Last().Date;
        public List<ClientTag> Tags => ClientTag.Where(x => x.ClientId == Id).ToList();
    }
}
