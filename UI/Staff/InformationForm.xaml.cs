using CompanyManagement.ViewModels;
using System.Windows.Controls;

namespace CompanyManagement.UI.Staff
{
    /// <summary>
    /// Interaction logic for InformationForm.xaml
    /// </summary>
    public partial class InformationForm : Page
    {
        public InformationForm()
        {
            InitializeComponent();
            DataContext = new UserInformationViewModel();
        }
    }
}
