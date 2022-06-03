using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TeamGenerator.Model;

namespace TeamGenerator.Controls
{
    /// <summary>
    /// Interaction logic for TeamControl.xaml
    /// </summary>
    public partial class TeamControl : UserControl
    {
        public TeamControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(TeamControl), new PropertyMetadata(null));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty PlayersProperty = DependencyProperty.Register("Players", typeof(ObservableCollection<Player>), typeof(TeamControl), new PropertyMetadata(null));

        public ObservableCollection<Player> Players 
        {
            get { return (ObservableCollection<Player>)GetValue(PlayersProperty); }
            set { SetValue(PlayersProperty, value); }
        }

        public static readonly DependencyProperty WinProbabilityProperty = DependencyProperty.Register("WinProbability", typeof(int), typeof(TeamControl), new PropertyMetadata(null));

        public int WinProbability 
        {
            get { return (int)GetValue(WinProbabilityProperty); }
            set { SetValue(WinProbabilityProperty, value); }
        }
    }
}
