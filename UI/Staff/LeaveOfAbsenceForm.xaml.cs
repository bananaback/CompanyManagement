using System.Windows.Controls;
using CompanyManagement.ViewModels;

namespace CompanyManagement.UI.Staff
{
    /// <summary>
    /// Interaction logic for LeaveOfAbsenceForm.xaml
    /// </summary>
    public partial class LeaveOfAbsenceForm : Page
    {
        public LeaveOfAbsenceForm()
        {
            InitializeComponent();
            DataContext = new LeaveOfAbsencePageViewModel();
        }
    }
}
