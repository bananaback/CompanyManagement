using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.HelperClasses;
using CompanyManagement.States;
using CuoiKi.States;
using CUOIKI_EF.Controllers;
using System.Collections.Generic;
using System.Windows.Input;

namespace CompanyManagement.ViewModels
{
    public class ViewTaskPageViewModel : ViewModelBase
    {
        private string employeeName = string.Empty;
        private string employeeID = string.Empty;
        private List<Task> _assignedTasks;
        private DbController _dbController;
        private Task _selectedTask;
        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }
        public List<Task> AssignedTasks
        {
            get => _assignedTasks;
            set
            {
                _assignedTasks = value;
                OnPropertyChanged(nameof(AssignedTasks));
            }
        }
        public string EmployeeName
        {
            get => employeeName;
            set
            {
                employeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
            }
        }
        public string EmployeeID
        {
            get => employeeID;
            set
            {
                employeeID = value;
                OnPropertyChanged(nameof(EmployeeID));
            }
        }
        public ViewTaskPageViewModel()
        {
            EmployeeName = TaskAssignmentState.SelectedEmployee.Name;
            EmployeeID = TaskAssignmentState.SelectedEmployee.ID;
            _assignedTasks = new List<Task>();
            _dbController = new DbController();
            FetchAssignedTasks();
        }
        void FetchAssignedTasks()
        {
            AssignedTasks.Clear();
            AssignedTasks = _dbController.GetAssignedJobsByManagerToEmployee(LoginInfoState.Id, employeeID);
        }
        private ICommand _CmdEdit { get; set; }
        public ICommand CmdEdit
        {
            get
            {
                if (_CmdEdit == null)
                {
                    _CmdEdit = new RelayCommand(
                    p => true,
                    p => EditTask());
                }
                return _CmdEdit;
            }
        }
        private void EditTask()
        {

        }

        private ICommand _CmdDone { get; set; }
        public ICommand CmdDone
        {
            get
            {
                if (_CmdDone == null)
                {
                    _CmdDone = new RelayCommand(
                    p => true,
                    p => MarkAsDone());
                }
                return _CmdDone;
            }
        }
        private void MarkAsDone()
        {
            SelectedTask.Status = EnumMapper.mapToString(Enums.TaskStatus.Done);
            _dbController.Save(SelectedTask);
            FetchAssignedTasks();
        }

    }
}
