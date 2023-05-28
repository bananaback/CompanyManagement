using CompanyManagement.ViewModels;
using CuoiKi.States;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CompanyManagement.UI.Manager
{
    /// <summary>
    /// Interaction logic for AssignTaskPage.xaml
    /// </summary>
    public partial class AssignTaskPage : Page
    {
        public AssignTaskPage()
        {
            InitializeComponent();
            this.DataContext = new TeamMemberPageViewModel();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            TaskAssignmentState.SelectedTeam = null;
            NavigationService.GoBack();
        }

        private void btn_ViewTask_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
            NavigationService.Navigate(new ViewTask());
        }
        private void ViewInformationClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
            NavigationService.Navigate(new ManagerViewInfomationPage());
        }
    }
}
