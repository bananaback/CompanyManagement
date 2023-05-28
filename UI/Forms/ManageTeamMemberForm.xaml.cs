using CompanyManagement.ViewModels;
using System.Windows;

namespace CompanyManagement.UI.Forms
{
    /// <summary>
    /// Interaction logic for ManageTeamMemberForm.xaml
    /// </summary>
    public partial class ManageTeamMemberForm : Window
    {
        public ManageTeamMemberForm(TeamMemberPageViewModel tmpvm)
        {
            InitializeComponent();
            this.DataContext = tmpvm;
        }
    }
}
