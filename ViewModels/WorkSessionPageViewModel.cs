using CompanyManagement.Controllers;
using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.HelperClasses;
using CompanyManagement.States;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CompanyManagement.ViewModels
{
    public class WorkSessionPageViewModel : ViewModelBase
    {
        private readonly WorkSessionController workSessionController;
        private string _currentUserId = "";
        private ObservableCollection<WorkSession> _workSessionsInSelectedMonth;
        public WorkSessionPageViewModel()
        {
            workSessionController = new WorkSessionController();
            _workSessionsInSelectedMonth = new ObservableCollection<WorkSession>();
            InitializeInfo();
        }
        private void InitializeInfo()
        {
            _currentUserId = LoginInfoState.Id;
            CurrentStatus = EnumMapper.mapToString(workSessionController.GetWorkSessionStatus(_currentUserId));
            UpdateLastestWorkSessionPanelVariables();
            UpdateWorkSessionsInSelectedMonth(_currentUserId, DateTime.Now);
        }

        #region Binding work session list in month
        public ObservableCollection<WorkSession> WorkSessionsInSelectedMonth
        {
            get { return _workSessionsInSelectedMonth; }
            set
            {
                _workSessionsInSelectedMonth = value;
                OnPropertyChanged(nameof(WorkSessionsInSelectedMonth));
            }
        }
        #endregion

        private void UpdateWorkSessionsInSelectedMonth(string employeeID, DateTime dateInMonth)
        {
            _workSessionsInSelectedMonth.Clear();
            List<WorkSession> workSessions =
                workSessionController.GetAllWorkSessionOfAnEmployeeInSelectedMonth(employeeID, dateInMonth);
            foreach (var item in workSessions)
            {
                _workSessionsInSelectedMonth.Add(item);
            }
            WorkSessionsInSelectedMonth = new ObservableCollection<WorkSession>(_workSessionsInSelectedMonth);
        }

        #region Binding calendar current selected date
        private DateTime _calendarSelectedDate = DateTime.Today;
        public DateTime CalendarSelectedDate
        {
            get { return _calendarSelectedDate; }
            set
            {
                _calendarSelectedDate = value;
                OnPropertyChanged(nameof(CalendarSelectedDate));
                UpdateWorkSessionsInSelectedMonth(_currentUserId, value);
            }
        }
        #endregion

        #region Binding CurrentDate
        private string _currentDate = DateTime.Now.Date.ToString();
        public string CurrentDate
        {
            get { return _currentDate; }
            set
            {
                string[] tokens = value.Split(new Char[] { '/', ' ', '.' });
                _currentDate = tokens[1] + "/" + tokens[0] + "/" + tokens[2];
                OnPropertyChanged(nameof(CurrentDate));
            }
        }
        #endregion

        #region Binding CurrentStatus
        private string _currentStatus = "";
        public string CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                _currentStatus = value;
                OnPropertyChanged(nameof(CurrentStatus));
            }
        }
        #endregion

        #region Binding and update work session visible variables
        private void UpdateLastestWorkSessionPanelVariables()
        {
            CurrentDate = DateTime.Now.ToString();

            WorkSession ws = workSessionController.GetLastestWorkSession(_currentUserId);
            if (ws != null)
            {
                ShowEmptyWorkSession = false;
                ShowCurrentWorkSession = true;
                WorkSessionID = ws.ID;
                WorkSessionStartingTime = ws.StartingTime.ToString("F");
                if (ws.EndingTime == DateTime.MinValue)
                {
                    WorkSessionEndingTime = "You haven't checked out yet";
                }
                else
                {
                    WorkSessionEndingTime = ws.EndingTime?.ToString("F");
                }
            }
            else
            {
                ShowEmptyWorkSession = true;
                ShowCurrentWorkSession = false;
            }
        }
        private string _workSessionID = "";
        public string WorkSessionID
        {
            get { return _workSessionID; }
            set
            {
                _workSessionID = value;
                OnPropertyChanged(nameof(WorkSessionID));
            }
        }
        private string _workSessionStartingTime = "";
        public string WorkSessionStartingTime
        {
            get { return _workSessionStartingTime; }
            set
            {
                _workSessionStartingTime = value;
                OnPropertyChanged(nameof(WorkSessionStartingTime));
            }
        }
        private string _workSessionEndingTime = "";
        public string WorkSessionEndingTime
        {
            get { return _workSessionEndingTime; }
            set
            {
                _workSessionEndingTime = value;
                OnPropertyChanged(nameof(WorkSessionEndingTime));
            }
        }
        private bool _showEmptyWorkSession = true;
        public bool ShowEmptyWorkSession
        {
            get { return _showEmptyWorkSession; }
            set
            {
                _showEmptyWorkSession = value;
                OnPropertyChanged(nameof(ShowEmptyWorkSession));
            }
        }
        private bool _showCurrentWorkSession = false;
        public bool ShowCurrentWorkSession
        {
            get { return _showCurrentWorkSession; }
            set
            {
                _showCurrentWorkSession = value;
                OnPropertyChanged(nameof(ShowCurrentWorkSession));
            }
        }
        #endregion

        #region Show check in/out button properties
        private bool _showCheckInButton = true;
        public bool ShowCheckInButton
        {
            get { return _showCheckInButton; }
            set
            {
                _showCheckInButton = value;
                OnPropertyChanged(nameof(ShowCheckInButton));
            }
        }
        private bool _showCheckOutButton = true;
        public bool ShowCheckOutButton
        {
            get { return _showCheckOutButton; }
            set
            {
                _showCheckOutButton = value;
                OnPropertyChanged(nameof(ShowCheckOutButton));
            }
        }
        #endregion

        #region Handle check in command
        private ICommand _checkInCommand;
        private void CheckIn()
        {
            workSessionController.CheckInAndReturnSuccessOrNot(LoginInfoState.Id);
            CurrentStatus = EnumMapper.mapToString(workSessionController.GetWorkSessionStatus(_currentUserId));
            UpdateLastestWorkSessionPanelVariables();
        }
        private readonly bool canCheckIn = true;
        public ICommand CheckInCommand
        {
            get
            {
                if (_checkInCommand == null)
                {
                    _checkInCommand = new RelayCommand(
                        p => this.canCheckIn,
                        p => this.CheckIn());
                }
                return _checkInCommand;
            }
        }
        #endregion

        #region Handle check out command
        private ICommand _checkOutCommand;
        private void CheckOut()
        {
            workSessionController.CheckOutAndReturnSuccessOrNot(LoginInfoState.Id);
            CurrentStatus = EnumMapper.mapToString(workSessionController.GetWorkSessionStatus(_currentUserId));
            UpdateLastestWorkSessionPanelVariables();
            UpdateWorkSessionsInSelectedMonth(_currentUserId, DateTime.Now);
        }
        private readonly bool canCheckOut = true;
        public ICommand CheckOutCommand
        {
            get
            {
                if (_checkOutCommand == null)
                {
                    _checkOutCommand = new RelayCommand(
                        p => this.canCheckOut,
                        p => this.CheckOut());
                }
                return _checkOutCommand;
            }
        }
        #endregion
    }
}
