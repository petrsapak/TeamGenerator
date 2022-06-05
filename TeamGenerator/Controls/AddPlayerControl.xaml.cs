using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TeamGenerator.Model;

namespace TeamGenerator.Controls
{
    public partial class AddPlayerControl : UserControl
    {
        public AddPlayerControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty AddAvailablePlayerCommandProperty = DependencyProperty.Register("AddAvailablePlayerCommand", typeof(DelegateCommand), typeof(AddPlayerControl), new PropertyMetadata(null));

        public DelegateCommand AddAvailablePlayerCommand
        {
            get { return (DelegateCommand)GetValue(AddAvailablePlayerCommandProperty); }
            set { SetValue(AddAvailablePlayerCommandProperty, value); }
        }

        public static readonly DependencyProperty NewPlayerNameProperty = DependencyProperty.Register("NewPlayerName", typeof(string), typeof(AddPlayerControl), new PropertyMetadata(null));

        public string NewPlayerName
        {
            get { return (string)GetValue(NewPlayerNameProperty); }
            set { SetValue(NewPlayerNameProperty, value); }
        }

        public static readonly DependencyProperty NewPlayerRankProperty = DependencyProperty.Register("NewPlayerRank", typeof(Rank), typeof(AddPlayerControl), new PropertyMetadata(null));

        public Rank NewPlayerRank
        {
            get { return (Rank)GetValue(NewPlayerRankProperty); }
            set { SetValue(NewPlayerRankProperty, value); }
        }

        public static readonly DependencyProperty RanksProperty = DependencyProperty.Register("Ranks", typeof(ObservableCollection<Rank>), typeof(AddPlayerControl), new PropertyMetadata(null));

        public ObservableCollection<Rank> Ranks
        {
            get { return (ObservableCollection<Rank>)GetValue(RanksProperty); }
            set { SetValue(RanksProperty, value); }
        }
    }
}
