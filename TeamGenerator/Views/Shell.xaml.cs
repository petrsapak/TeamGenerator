using Prism.Ioc;
using Prism.Regions;
using System.Windows;
using TeamGenerator.Controls;

namespace TeamGenerator.Views
{
    public partial class Shell : Window
    {
        private readonly IRegionManager regionManager;
        private readonly IContainerExtension container;

        public Shell(IRegionManager regionManager, IContainerExtension container)
        {
            InitializeComponent();
            this.regionManager = regionManager;
            this.container = container;
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(DashboardView));
            regionManager.RegisterViewWithRegion("MainMenuRegion", typeof(MainMenuControl));
        }

        private void TopBarControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
