using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.Factories;
using CompanyManagement.HelperClasses;
using CompanyManagement.UI.Forms;
using CompanyManagement.Wrappers;
using CuoiKi.States;
using CUOIKI_EF.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CompanyManagement.ViewModels
{
    public class TeamsPageViewModel : ViewModelBase
    {
        private DbController _controller;
        public TeamsPageViewModel()
        {
            StageID = TaskAssignmentState.SelectedStage.ID;
            _controller = new DbController();
            _Title = "";
            _TeamID = "";
            _ShowID = Visibility.Collapsed;
            _TechLead = null;
            _TeamName = "";
            _techLeadsFromDB = _controller.GetAllEmployeeOfARole(Enums.Role.TechLead) ?? new List<Employee>();
            _teamList = new List<Team>();
            _teamWrappers = new List<TeamWrapper>();
            TeamWrappers = new List<TeamWrapper>();
            FetchTeamList();
        }

        #region List Team Binding
        private List<Team> _teamList;
        private List<TeamWrapper> _teamWrappers;
        public List<TeamWrapper> TeamWrappers
        {
            get => _teamWrappers;
            set
            {
                _teamWrappers = value;
                OnPropertyChanged(nameof(TeamWrappers));
            }
        }
        private void FetchTeamList()
        {
            _teamList = _controller.GetTeamsOfAStage(TaskAssignmentState.SelectedStage) ?? new List<Team>();
            _teamWrappers.Clear();
            for (int i = 0; i < _teamList.Count; i++)
            {
                TeamWrapper teamWrapper = new TeamWrapper(_teamList[i]);
                List<EF.Task> tasks = _controller.GetAllTaskOfTeam(teamWrapper.ID);
                int percentDone = 0;
                if (tasks != null && tasks.Count != 0) percentDone = (tasks.Where(t => t.Status == EnumMapper.mapToString(Enums.TaskStatus.Done)).Count() * 100 / tasks.Count);
                teamWrapper.InitializeUI(percentDone);
                _teamWrappers.Add(teamWrapper);
            }
            TeamWrappers = new List<TeamWrapper>(_teamWrappers);
        }

        #endregion

        #region Add/Edit Team Command Binding
        private ICommand _cmdAddTeam;
        public ICommand CmdAddTeam
        {
            get
            {
                if (_cmdAddTeam == null)
                {
                    _cmdAddTeam = new RelayCommand(
                        p => true,
                        p => OpenAddTeamForm());
                }
                return _cmdAddTeam;
            }
        }

        private ICommand _cmdEditTeam;
        public ICommand CmdEditTeam
        {
            get
            {
                if (_cmdEditTeam == null)
                {
                    _cmdEditTeam = new RelayCommand(
                        p => true,
                        p => OpenEditTeamForm());
                }
                return _cmdEditTeam;
            }
        }

        private ICommand _cmdSave;
        public ICommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                {
                    _cmdSave = new RelayCommand(
                        p => TechLead?.ID.Length > 0 && TeamName.Length > 0,
                        p => SaveTeam()
                    );
                }
                return _cmdSave;
            }
        }

        private void SaveTeam()
        {
            Team team = TaskAssignmentState.SelectedTeam ?? TeamFactory.CreateNewTeam(StageID, TechLead.ID, TeamName);
            team.TechLeadID = TechLead.ID;
            team.Name = TeamName;
            _controller.Save(team);
            FetchTeamList();
        }

        private void OpenEditTeamForm()
        {
            Title = "Edit Team";
            ShowID = Visibility.Visible;

            StageID = TaskAssignmentState.SelectedTeam.StageID;
            TechLead = TechLeadsFromDB.Where(x => x.ID == TaskAssignmentState.SelectedTeam.TechLeadID).First();
            TeamName = TaskAssignmentState.SelectedTeam.Name;

            var f = new TeamForm(this);
            f.Show();
        }

        private void OpenAddTeamForm()
        {
            Title = "Add New Team";
            TeamID = "";
            ShowID = Visibility.Collapsed;
            StageID = TaskAssignmentState.SelectedStage.ID;
            TechLead = null;
            TeamName = "";
            var f = new TeamForm(this);
            f.Show();
        }

        #endregion

        #region Team Form Bindings
        public string StageID { get; set; }
        private string _TeamID;
        public string TeamID
        {
            get { return _TeamID; }
            set
            {
                _TeamID = value;
                OnPropertyChanged(nameof(TeamID));
            }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private Visibility _ShowID;
        public Visibility ShowID
        {
            get { return _ShowID; }
            set
            {
                _ShowID = value;
                OnPropertyChanged(nameof(ShowID));
            }
        }

        private List<Employee> _techLeadsFromDB;
        public List<Employee> TechLeadsFromDB
        {
            get { return _techLeadsFromDB; }
            set
            {
                _techLeadsFromDB = value;
                OnPropertyChanged(nameof(TechLeadsFromDB));
            }
        }

        private Employee _TechLead;
        public Employee TechLead
        {
            get { return _TechLead; }
            set
            {
                _TechLead = value;
                OnPropertyChanged(nameof(TechLead));
            }
        }

        private string _TeamName;
        public string TeamName
        {
            get { return _TeamName; }
            set
            {
                _TeamName = value;
                OnPropertyChanged(nameof(TeamName));
            }
        }

        private ICommand _cmdUpdateTechLead;
        public ICommand CmdUpdateTechLead
        {
            get
            {
                if (_cmdUpdateTechLead == null)
                {
                    _cmdUpdateTechLead = new RelayCommand(
                        p => true,
                        p =>
                        {
                            TechLead = TechLeadsFromDB.Where(x => x.ID == p.ToString()).First();
                        }
                    );
                }
                return _cmdUpdateTechLead;
            }
        }

        #endregion

        #region Context Menu Bindings
        private ICommand _cmdSaveTeamToState;
        public ICommand CmdSaveTeamToState
        {
            get
            {
                if (_cmdSaveTeamToState == null)
                {
                    _cmdSaveTeamToState = new RelayCommand(
                        p => true,
                        p => SaveTeamToState(p)
                    );
                }
                return _cmdSaveTeamToState;
            }
        }

        private void SaveTeamToState(object param)
        {
            if (!(param is string id))
            {
                return;
            }
            TaskAssignmentState.SelectedTeam = _teamList.Where(x => x.ID == id).FirstOrDefault();
            TeamID = id;
        }

        private ICommand _cmdDeleteTeam;
        public ICommand CmdDeleteTeam
        {
            get
            {
                if (_cmdDeleteTeam == null)
                {
                    _cmdDeleteTeam = new RelayCommand(
                    p => true,
                    p => DeleteTeam());
                }
                return _cmdDeleteTeam;
            }
        }

        private void DeleteTeam()
        {
            _controller.Delete(TaskAssignmentState.SelectedTeam);
            FetchTeamList();
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
            FetchTeamList();
        }
    }
}
