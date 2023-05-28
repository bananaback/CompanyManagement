using CompanyManagement.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CompanyManagement.UI.Manager
{
    /// <summary>
    /// Interaction logic for ManagerViewInfomationPage.xaml
    /// </summary>
    public partial class ManagerViewInfomationPage : Page
    {
        public ManagerViewInfomationPage()
        {
            InitializeComponent();
            this.DataContext = new ManagerViewInfomationViewModel();
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
