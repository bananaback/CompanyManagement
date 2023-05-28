using CompanyManagement.EF;
using CuoiKi.States;

namespace CompanyManagement.ViewModels
{
    public class ManagerViewInfomationViewModel : ViewModelBase
    {
        private Employee _currentEmployee;
        public ManagerViewInfomationViewModel()
        {
            _currentEmployee = TaskAssignmentState.SelectedEmployee;
        }
        public string ID
        {
            get => _currentEmployee.ID;
        }
        public string Name
        {
            get => _currentEmployee.Name;
        }
        public string Gender
        {
            get => _currentEmployee.Gender.ToString();
        }
        public string Address
        {
            get => _currentEmployee.Address;
        }
        public string Birthday
        {
            get => _currentEmployee.Birth.ToString("d");
        }
        public string Status
        {
            get => _currentEmployee.EmployeeStatus;
        }
        public string Role
        {
            get => _currentEmployee.Role.ToString();
        }
    }
}
