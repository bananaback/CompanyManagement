using CompanyManagement.ViewModels;
using System.Windows;

namespace CompanyManagement.UI.Forms
{
    /// <summary>
    /// Interaction logic for ManagerTaskAssignmentForm.xaml
    /// </summary>
    public partial class ManagerTaskAssignmentForm : Window
    {
        public ManagerTaskAssignmentForm(TeamMemberPageViewModel tmpvm)
        {
            InitializeComponent();
            this.DataContext = tmpvm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
