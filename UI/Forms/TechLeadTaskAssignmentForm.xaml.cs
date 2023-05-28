using CompanyManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CompanyManagement.UI.Forms
{
    /// <summary>
    /// Interaction logic for TechLeadTaskAssignmentForm.xaml
    /// </summary>
    public partial class TechLeadTaskAssignmentForm : Window
    {
        public TechLeadTaskAssignmentForm(TechLeadTeamMemberViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
