using Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace TeamGenerator.Controls
{
    /// <summary>
    /// Interaction logic for AddPlayerControl.xaml
    /// </summary>
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
    }
}
