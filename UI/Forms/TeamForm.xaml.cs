using CompanyManagement.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CompanyManagement.UI.Forms
{
    /// <summary>
    /// Interaction logic for TeamForm.xaml
    /// </summary>
    public partial class TeamForm : Window
    {
        public TeamForm(TeamsPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CbbTechLead_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox cbb))
            {
                return;
            }
            (DataContext as TeamsPageViewModel).CmdUpdateTechLead.Execute(cbb.SelectedValue?.ToString());
        }
    }
}
