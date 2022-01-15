using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TeamGenerator.Controls
{
    public partial class MainMenuControl : UserControl
    {
        public MainMenuControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SwitchViewCommandProperty = DependencyProperty.Register("SwitchViewCommand", typeof(ICommand), typeof(TopBarControl), new PropertyMetadata(null));

        public ICommand SwitchViewCommand
        {
            get { return (ICommand)GetValue(SwitchViewCommandProperty); }
            set { SetValue(SwitchViewCommandProperty, value); }
        }
    }
}
