using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TeamGenerator.Shell.Controls
{
    public partial class TopBarControl : UserControl
    {
        public TopBarControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ApplicationTitleProperty = DependencyProperty.Register("ApplicationTitle", typeof(string), typeof(TopBarControl), new PropertyMetadata(null));

        public string ApplicationTitle
        {
            get { return (string)GetValue(ApplicationTitleProperty); }
            set { SetValue(ApplicationTitleProperty, value); }
        }

        public static readonly DependencyProperty MinimizeApplicationCommandProperty = DependencyProperty.Register("MinmizeApplicationCommand", typeof(ICommand), typeof(TopBarControl), new PropertyMetadata(null));

        public ICommand MinimizeApplicationCommand
        {
            get { return (ICommand)GetValue(MinimizeApplicationCommandProperty); }
            set { SetValue(MinimizeApplicationCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseApplicationCommandProperty = DependencyProperty.Register("CloseApplicationCommand", typeof(ICommand), typeof(TopBarControl), new PropertyMetadata(null));

        public ICommand CloseApplicationCommand
        {
            get { return (ICommand)GetValue(CloseApplicationCommandProperty); }
            set { SetValue(CloseApplicationCommandProperty, value); }
        }
    }
}
