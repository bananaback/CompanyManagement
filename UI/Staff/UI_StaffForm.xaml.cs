using CompanyManagement.UI.Forms;
using System.Windows;
using System.Windows.Input;

namespace CompanyManagement.UI.Staff
{
    /// <summary>
    /// Interaction logic for UI_StaffForm.xaml
    /// </summary>
    public partial class UI_StaffForm : Window
    {
        public UI_StaffForm()
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

        private void btn_CalcSalary_click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new SalaryPage());
        }

        private void btn_Information_click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new InformationForm());
        }

        private void btn_WorkSession_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new WorkSessionForm());
        }

        private void btn_LeaveOfAbsence_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new LeaveOfAbsenceForm());
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_ViewTask_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new StaffTaskManagementForm());
        }
    }
}
