using CompanyManagement.Controllers;
using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.States;
using CompanyManagement.UI.Manager;
using CompanyManagement.UI.Staff;
using CompanyManagement.UI.TechLead;
using System;
using System.Windows;

namespace CompanyManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AuthenticationController _authenticationController;
        public MainWindow()
        {
            InitializeComponent();
            _authenticationController = new AuthenticationController();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            Employee foundEmployee = _authenticationController.Login(txtUsername.Text, txtPassword.Password);
            if (foundEmployee == null)
            {
                MessageBox.Show("Invalid Credentials");
                return;
            }
            LoginInfoState.Id = foundEmployee.ID;
            LoginInfoState.Name = foundEmployee.Name;
            LoginInfoState.Role = (Role)Enum.Parse(typeof(Role), foundEmployee.Role, true);

            if (foundEmployee.Role == Role.Manager.ToString())
            {
                UI_ManagerForm uI_ManagerForm = new UI_ManagerForm();
                uI_ManagerForm.Show();

            }
            else if (foundEmployee.Role == Role.Hr.ToString())
            {
                //UI_HrForm uI_HrForm = new UI_HrForm();
                //uI_HrForm.Show();
            }
            else if (foundEmployee.Role == Role.TechLead.ToString())
            {
                UI_TechLeadForm uI_TechLeadForm = new UI_TechLeadForm();
                uI_TechLeadForm.Show();
            }
            else
            {
                UI_StaffForm uI_StaffForm = new UI_StaffForm();
                uI_StaffForm.Show();
            }
            this.Close();
        }
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
