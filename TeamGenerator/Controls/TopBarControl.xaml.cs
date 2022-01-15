using Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace TeamGenerator.Controls
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

        public static readonly DependencyProperty MinimizeApplicationCommandProperty = DependencyProperty.Register("MinmizeApplicationCommand", typeof(DelegateCommand), typeof(TopBarControl), new PropertyMetadata(null));

        public DelegateCommand MinimizeApplicationCommand
        {
            get { return (DelegateCommand)GetValue(MinimizeApplicationCommandProperty); }
            set { SetValue(MinimizeApplicationCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseApplicationCommandProperty = DependencyProperty.Register("CloseApplicationCommand", typeof(DelegateCommand), typeof(TopBarControl), new PropertyMetadata(null));

        public DelegateCommand CloseApplicationCommand
        {
            get { return (DelegateCommand)GetValue(CloseApplicationCommandProperty); }
            set { SetValue(CloseApplicationCommandProperty, value); }
        }
    }
}
