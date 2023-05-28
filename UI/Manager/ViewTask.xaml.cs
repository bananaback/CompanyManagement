using CompanyManagement.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CompanyManagement.UI.Manager
{
    /// <summary>
    /// Interaction logic for ViewTask.xaml
    /// </summary>
    public partial class ViewTask : Page
    {
        public ViewTask()
        {
            InitializeComponent();
            this.DataContext = new ViewTaskPageViewModel();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
