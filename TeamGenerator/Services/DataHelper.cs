using System.Collections.Generic;
using System.Collections.ObjectModel;
using TeamGenerator.Model;

namespace TeamGenerator.Services
{
    internal class DataHelper
    {
        public ObservableCollection<Player> PlayerPool { get; set; }
        public List<Rank> Ranks { get; set; }
    }
}
