using CompanyManagement.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CompanyManagement.UI.Manager
{
    /// <summary>
    /// Interaction logic for StagePage.xaml
    /// </summary>
    public partial class StagePage : Page
    {
        public StagePage()
        {
            InitializeComponent();
            DataContext = new StagePageViewModel();
        }
        private void back_click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnStageItem_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
            NavigationService.Navigate(new TaskAssignmentPage());
        }

        private void BtnStageItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
        }

        private void ViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskAssignmentPage());
        }
    }
}
