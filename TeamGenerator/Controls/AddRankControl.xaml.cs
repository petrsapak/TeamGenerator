using Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace TeamGenerator.Controls
{
    public partial class AddRankControl : UserControl
    {
        public AddRankControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty AddRankCommandProperty = DependencyProperty.Register("AddRankCommand", typeof(DelegateCommand), typeof(AddRankControl), new PropertyMetadata(null));

        public DelegateCommand AddRankCommand
        {
            get { return (DelegateCommand)GetValue(AddRankCommandProperty); }
            set { SetValue(AddRankCommandProperty, value); }
        }

        public static readonly DependencyProperty NewRankNameProperty = DependencyProperty.Register("NewRankName", typeof(string), typeof(AddRankControl), new PropertyMetadata(null));

        public string NewRankName
        {
            get { return (string)GetValue(NewRankNameProperty); }
            set { SetValue(NewRankNameProperty, value); }
        }

        public static readonly DependencyProperty NewRankValueProperty = DependencyProperty.Register("NewRankValue", typeof(double), typeof(AddRankControl), new PropertyMetadata(null));

        public double NewRankValue
        {
            get { return (double)GetValue(NewRankValueProperty); }
            set { SetValue(NewRankValueProperty, value); }
        }
    }
}
