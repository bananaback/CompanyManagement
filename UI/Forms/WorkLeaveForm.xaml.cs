using CompanyManagement.ViewModels;
using System.Windows;

namespace CompanyManagement.UI.Forms
{
    /// <summary>
    /// Interaction logic for WorkLeaveForm.xaml
    /// </summary>
    public partial class WorkLeaveForm : Window
    {
        public WorkLeaveForm(LeaveOfAbsencePageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
