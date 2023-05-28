using CompanyManagement.UI.Forms;
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

namespace CompanyManagement.UI.TechLead
{
    /// <summary>
    /// Interaction logic for UI_TechLeadForm.xaml
    /// </summary>
    public partial class UI_TechLeadForm : Window
    {
        public UI_TechLeadForm()
        {
            InitializeComponent();
        }
        public void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void MyProjectsClick(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new TechLeadProjectPage());
        }
        private void LogOut_click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btn_CalcSalary_click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new SalaryPage());
        }
    }
}
