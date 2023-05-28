using System.Windows;
using System.Windows.Controls;

namespace CompanyManagement.UI.Hr
{
    /// <summary>
    /// Interaction logic for MemberList.xaml
    /// </summary>
    public partial class MemberList : Page
    {
        public MemberList()
        {
            InitializeComponent();
            //this.DataContext = new SalaryOfMember();
        }

        public void ViewSalaryClick(object sender, RoutedEventArgs e)
        {
            //var btn = sender as Button;
            //NavigationService.Navigate(new EmployeeSalaryPage(btn.CommandParameter as Employee));
        }
    }
}
