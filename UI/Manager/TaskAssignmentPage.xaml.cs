using CompanyManagement.ViewModels;
using CuoiKi.States;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CompanyManagement.UI.Manager
{
    /// <summary>
    /// Interaction logic for TaskAssignmentPage.xaml
    /// </summary>
    public partial class TaskAssignmentPage : Page
    {
        public TaskAssignmentPage()
        {
            InitializeComponent();
            TeamsPageViewModel viewModel = new TeamsPageViewModel();
            this.DataContext = viewModel;
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            TaskAssignmentState.SelectedStage = null;
            NavigationService.GoBack();
        }

        public void Team_Click(object sender, RoutedEventArgs e)
        {
            if (sender as Button is null)
            {
                return;
            }
            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
            NavigationService.Navigate(new AssignTaskPage());
        }

        private void BtnTeamItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
        }

        private void ViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AssignTaskPage());
        }
    }
}
