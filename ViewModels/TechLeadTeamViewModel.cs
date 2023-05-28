using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.HelperClasses;
using CompanyManagement.Wrappers;
using CuoiKi.States;
using CUOIKI_EF.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CompanyManagement.ViewModels
{
    public class TechLeadTeamViewModel : ViewModelBase
    {
        private readonly DbController _dbController;
        private List<Team> _teams;
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
        public TechLeadTeamViewModel()
        {
            _dbController = DbController.Instance;
            _teams = new List<Team>();
            _teamWrappers = new List<TeamWrapper>();
            UpdateTeams();
        }
        private void UpdateTeams()
        {
            _teams.Clear();
            _teamWrappers.Clear();
            _teams = _dbController.GetTeamsOfStage(TaskAssignmentState.SelectedStage);
            for (int i = 0; i < _teams.Count; i++)
            {
                TeamWrapper teamWrapper = new TeamWrapper(_teams[i]);
                List<Task> tasks = _dbController.GetAllTaskOfTeam(teamWrapper.ID);
                int percentDone = 0;
                if (tasks != null && tasks.Count != 0) percentDone = (tasks.Where(t => t.Status == EnumMapper.mapToString(TaskStatus.Done)).Count() * 100 / tasks.Count);
                teamWrapper.InitializeUI(percentDone);
                _teamWrappers.Add(teamWrapper);
            }
            TeamWrappers = new List<TeamWrapper>(_teamWrappers);
        }
        private ICommand _cmdTeamItemClick;
        public ICommand TeamItemClickCommand
        {
            get
            {
                if (_cmdTeamItemClick == null )
                {
                    _cmdTeamItemClick = new RelayCommand(
                    p => true,
                    p => SaveTeamToState(p));
                }
                return _cmdTeamItemClick;
            }
        }
        private void SaveTeamToState(object p)
        {
            if (p is null) return;
            string teamID = p as string;
            TaskAssignmentState.SelectedTeam = _teams.Where(team => team.ID == teamID).FirstOrDefault();
        }
    }
}
