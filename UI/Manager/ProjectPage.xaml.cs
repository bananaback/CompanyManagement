using CompanyManagement.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CompanyManagement.UI.Manager
{
    /// <summary>
    /// Interaction logic for ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        public ProjectPage()
        {
            InitializeComponent();
            this.DataContext = new ProjectPageViewModel();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //var projectEditorWindow = new ProjectForm(this.DataContext as ProjectsPageViewModel);
            //projectEditorWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
            //NavigationService.Navigate(new StagesPage());
        }

        private void ViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new StagesPage());
        }

        private void Button_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
        }
    }
}
