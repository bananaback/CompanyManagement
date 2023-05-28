using CompanyManagement.EF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CompanyManagement.UI.Human_Resource
{
    /// <summary>
    /// Interaction logic for MemberList.xaml
    /// </summary>
    public partial class MemberList : Page
    {
        public MemberList()
        {
            InitializeComponent();
        }

        private void ViewSalaryClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            //NavigationService.Navigate(new  EmployeeSalaryPage(btn.CommandParameter as Employee));
        }
    }
}
