using CompanyManagement.DTO;
using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.Factories;
using CompanyManagement.HelperClasses;
using CompanyManagement.States;
using CompanyManagement.UI.Forms;
using CompanyManagement.Wrappers;
using CuoiKi.States;
using CUOIKI_EF.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CompanyManagement.ViewModels
{
    public class TeamMemberPageViewModel : ViewModelBase
    {
        private DbController _controller;
        private string _CurrentManagerName = "";
        private string _CurrentManagerID = "";
        private string _CurrentEmployeeName = "";
        private string _CurrentTeamName = "";
        private List<Employee> _teamMembers;
        private List<EmployeeWrapper> _employeeWrappers;
        public List<EmployeeWrapper> EmployeeWrappers
        {
            get => _employeeWrappers;
            set
            {
                _employeeWrappers = value;
                OnPropertyChanged(nameof(EmployeeWrappers));
            }
        }

        public string CurrentTeamName
        {
            get => _CurrentTeamName;
            set
            {
                _CurrentTeamName = value;
                OnPropertyChanged(nameof(CurrentTeamName));
            }
        }

        public TeamMemberPageViewModel()
        {
            _controller = new DbController();
            _teamMembers = new List<Employee>();
            _employeeWrappers = new List<EmployeeWrapper>();
            EmployeeWrappers = new List<EmployeeWrapper>();
            InitializeVariables();
            UpdateTeamMemberList();
        }

        private void InitializeVariables()
        {
            _CurrentManagerName = LoginInfoState.Name;
            _CurrentManagerID = LoginInfoState.Id;
            _CurrentTeamName = _controller.GetTeamName(TaskAssignmentState.SelectedTeam.ID);
        }
        #region Team member list
        private void UpdateTeamMemberList()
        {
            _teamMembers.Clear();
            _employeeWrappers.Clear();
            List<TeamMember> teamMembers = _controller.GetAllMembersOfTeam(TaskAssignmentState.SelectedTeam);
            foreach (TeamMember member in teamMembers)
            {
                _teamMembers.Add(_controller.GetTeamMemberDetails(member));
            }
            for (int i = 0; i < _teamMembers.Count; i++)
            {
                EmployeeWrapper employeeWrapper = new EmployeeWrapper(_teamMembers[i]);
                List<EF.Task> teamTasks = _controller.GetAllTaskOfTeam(TaskAssignmentState.SelectedTeam.ID);
                List<EF.Task> employeeTasks = teamTasks.Where(task => task.Assignee == employeeWrapper.ID).ToList();
                int percentDone = 0;
                if (employeeTasks != null && employeeTasks.Count != 0) percentDone = (employeeTasks.Where(t => t.Status == EnumMapper.mapToString(Enums.TaskStatus.Done)).Count() * 100 / employeeTasks.Count);
                employeeWrapper.InitializeUI(percentDone);
                _employeeWrappers.Add(employeeWrapper);
            }
            EmployeeWrappers = new List<EmployeeWrapper>(_employeeWrappers);
        }

        #endregion
        #region Assignee and assigner information property
        public string CurrentEmployeeName
        {
            get
            {
                return _CurrentEmployeeName;
            }
            set
            {
                _CurrentEmployeeName = value;
                OnPropertyChanged(nameof(CurrentEmployeeName));
            }
        }
        public string CurrentManagerName
        {
            get { return _CurrentManagerName; }
            set
            {
                _CurrentManagerName = value;
                OnPropertyChanged(nameof(CurrentManagerName));
            }
        }
        #endregion

        #region Open task assigment form command
        private ICommand _cmdOpenTaskAssignmentForm;
        public ICommand CmdOpenTaskAssignmentForm
        {
            get
            {
                if (_cmdOpenTaskAssignmentForm == null)
                {
                    _cmdOpenTaskAssignmentForm = new RelayCommand(
                    p => true,
                    p => OpenTaskAssignmentForm());
                }
                return _cmdOpenTaskAssignmentForm;
            }
        }
        private void OpenTaskAssignmentForm()
        {
            var taskAssignmentForm = new ManagerTaskAssignmentForm(this);
            taskAssignmentForm.Show();
        }
        #endregion
        #region Form input
        private string _ToBeSavedTaskTitle = "";
        private string _ToBeSavedTaskDescription = "";
        private DateTime? _ToBeSavedTaskStartingTime = DateTime.Now;
        private DateTime? _ToBeSavedTaskEndingTime = DateTime.Now;
        public string ToBeSavedTaskTitle
        {
            get { return _ToBeSavedTaskTitle; }
            set
            {
                _ToBeSavedTaskTitle = value;
                OnPropertyChanged(nameof(ToBeSavedTaskTitle));
                CheckValidTaskInput();
            }
        }
        public string ToBeSavedTaskDescription
        {
            get { return _ToBeSavedTaskDescription; }
            set
            {
                _ToBeSavedTaskDescription = value;
                OnPropertyChanged(nameof(ToBeSavedTaskDescription));
                CheckValidTaskInput();
            }
        }
        public DateTime ToBeSavedTaskStartingTime
        {
            get { return _ToBeSavedTaskStartingTime ?? DateTime.Now; }
            set
            {
                _ToBeSavedTaskStartingTime = value;
                OnPropertyChanged(nameof(ToBeSavedTaskStartingTime));
            }
        }
        public DateTime ToBeSavedTaskEndingTime
        {
            get { return _ToBeSavedTaskEndingTime ?? DateTime.Now; }
            set
            {
                _ToBeSavedTaskEndingTime = value;
                OnPropertyChanged(nameof(ToBeSavedTaskEndingTime));
            }
        }
        #endregion
        #region Save task command
        private ICommand _cmdSaveTask;
        public ICommand CmdSaveTask
        {
            get
            {
                if (_cmdSaveTask == null)
                {
                    _cmdSaveTask = new RelayCommand(
                    p => _CanSaveTask,
                    p => SaveTask());
                }
                return _cmdSaveTask;
            }
        }
        private bool _CanSaveTask = false;
        private void SaveTask()
        {
            EF.Task task = TaskFactory.CreateNewTask(TaskAssignmentState.SelectedEmployee.ID, _CurrentManagerID, TaskAssignmentState.SelectedTeam.ID, ToBeSavedTaskDescription, ToBeSavedTaskTitle, ToBeSavedTaskStartingTime, ToBeSavedTaskEndingTime); ;
            _controller.Save(task);
            Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive == true).Close();
        }
        private void CheckValidTaskInput()
        {
            _CanSaveTask = !string.IsNullOrEmpty(ToBeSavedTaskTitle);
            _CanSaveTask = !string.IsNullOrEmpty(ToBeSavedTaskDescription);
        }
        #endregion

        #region Assign member's task command
        private ICommand _cmdAssignMemberTask;
        public ICommand CmdAssignMemberTask
        {
            get
            {
                if (_cmdAssignMemberTask == null)
                {
                    _cmdAssignMemberTask = new RelayCommand(
                    p => true,
                    p => AssignTaskToMember(p));
                }
                return _cmdAssignMemberTask;
            }
        }
        private void AssignTaskToMember(object p)
        {
            EmployeeWrapper ew = (EmployeeWrapper)p;
            Employee e = ew.Employee;
            TaskAssignmentState.SelectedEmployee = e;
            CurrentEmployeeName = e.Name;
            var taskForm = new ManagerTaskAssignmentForm(this);
            taskForm.Show();
        }
        #endregion

        #region View member's task
        private ICommand _cmdViewMemberTask;
        public ICommand CmdViewMemberTask
        {
            get
            {
                if (_cmdViewMemberTask == null)
                {
                    _cmdViewMemberTask = new RelayCommand(
                    p => true,
                    p => ViewMemberTask());
                }
                return _cmdViewMemberTask;
            }
        }
        private void ViewMemberTask()
        {

        }
        #endregion

        #region View member's information
        private ICommand _cmdViewMemberInformation;
        public ICommand CmdViewMemberInformation
        {
            get
            {
                if (_cmdViewMemberInformation == null)
                {
                    _cmdViewMemberInformation = new RelayCommand(
                    p => true,
                    p => ViewMemberInformation(p));
                }
                return _cmdViewMemberInformation;
            }
        }
        private void ViewMemberInformation(object p)
        {
            EmployeeWrapper ew = (EmployeeWrapper)p;
            Employee e = ew.Employee;
            TaskAssignmentState.SelectedEmployee = e;
            //MessageBox.Show("View member's information" + e.ID.ToString());
        }
        #endregion

        #region Open team member management form
        private ICommand _cmdOpenTeamMemberManagementForm;
        public ICommand CmdOpenTeamMemberManagementForm
        {
            get
            {
                if (_cmdOpenTeamMemberManagementForm == null)
                {
                    _cmdOpenTeamMemberManagementForm = new RelayCommand(
                    p => true,
                    p => OpenTeamMemberManagementForm());
                }
                return _cmdOpenTeamMemberManagementForm;
            }
        }
        private void OpenTeamMemberManagementForm()
        {
            UpdateWorkerList();
            var teamMemberManagementForm = new ManageTeamMemberForm(this);
            teamMemberManagementForm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            teamMemberManagementForm.Show();
        }
        #endregion

        #region Team member management logic
        private List<WorkerDTO> _workerList;
        public List<WorkerDTO> WorkerList
        {
            get => _workerList;
            set
            {
                _workerList = value;
                OnPropertyChanged(nameof(WorkerList));
            }
        }

        private void UpdateWorkerList()
        {
            List<TeamMember> teamMembers = _controller.GetAllMembersOfTeam(TaskAssignmentState.SelectedTeam);
            List<Employee> allWorkers = _controller.GetAllWorkers();
            _workerList = allWorkers.Select(x => new WorkerDTO()
            {
                Name = x.Name,
                EmployeeID = x.ID,
                EmployeeRole = x.Role.ToString(),
                IsSelected = teamMembers.Any(tm => tm.EmployeeID == x.ID)
            })
            .ToList();
        }
        #endregion
        #region Save changes command
        private ICommand _cmdSaveChanges;
        public ICommand CmdSaveChanges
        {
            get
            {
                if (_cmdSaveChanges == null)
                {
                    _cmdSaveChanges = new RelayCommand(
                    p => true,
                    p => SaveTeamMemberChanges());
                }
                return _cmdSaveChanges;
            }
        }
        private void SaveTeamMemberChanges()
        {
            List<TeamMember> currentTeamMembers = _controller.GetAllMembersOfTeam(TaskAssignmentState.SelectedTeam);
            // Get the IDs of selected workers who are not already team members
            List<string> newMemberIDs = _workerList
                .Where(w => w.IsSelected && !currentTeamMembers.Any(m => m.EmployeeID == w.EmployeeID))
                .Select(w => w.EmployeeID)
                .ToList();
            // Get the IDs of deselected workers who are already team members
            List<string> toBeRemovedTeamMemberIDs = _workerList
                .Where(w => !w.IsSelected && currentTeamMembers.Any(m => m.EmployeeID == w.EmployeeID))
                .Select(w => currentTeamMembers.First(m => m.EmployeeID == w.EmployeeID).ID)
                .ToList();
            _controller.AddTeamMembersToTeam(TaskAssignmentState.SelectedTeam.ID, newMemberIDs);
            _controller.RemoveTeamMembers(toBeRemovedTeamMemberIDs);
            UpdateWorkerList();
            UpdateTeamMemberList();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive == true).Close();
        }
        #endregion

        #region Cancel command
        private ICommand _cmdCancelTeamManagementForm;
        public ICommand CmdCancelTeamMenagementForm
        {
            get
            {
                if (_cmdCancelTeamManagementForm == null)
                {
                    _cmdCancelTeamManagementForm = new RelayCommand(
                    p => true,
                    p => CancelTeamManagementForm());
                }
                return _cmdCancelTeamManagementForm;
            }
        }
        private void CancelTeamManagementForm()
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive == true).Close();

        }
        #endregion
        #region Save current employee to state
        private ICommand _cmdSaveEmployeeToCurrentState;
        public ICommand CmdSaveEmployeeToCurrentState
        {
            get
            {
                if (_cmdSaveEmployeeToCurrentState == null)
                {
                    _cmdSaveEmployeeToCurrentState = new RelayCommand(
                    p => true,
                    p => SaveEmployeeToCurrentState(p));
                }
                return _cmdSaveEmployeeToCurrentState;
            }
        }
        private void SaveEmployeeToCurrentState(object parameter)
        {
            EmployeeWrapper ew = (EmployeeWrapper)parameter;
            Employee e = ew.Employee;
            TaskAssignmentState.SelectedEmployee = e;
        }
        #endregion

        private ICommand _reloadCommand;
        public ICommand ReloadCommand
        {
            get
            {
                if (_reloadCommand == null)
                {
                    _reloadCommand = new RelayCommand(
                    p => true,
                    p => Reload());
                }
                return _reloadCommand;
            }
        }

        private void Reload()
        {
            UpdateTeamMemberList();
        }
    }
}
