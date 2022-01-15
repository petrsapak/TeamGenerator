using Prism.Ioc;
using Prism.Regions;
using System.Windows;

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
            regionManager.RegisterViewWithRegion("MainMenuRegion", typeof(MainMenuView));
        }

        private void TopBarControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
