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
    public class TechLeadStageViewModel : ViewModelBase
    {
        private readonly DbController _dbController;
        private List<Stage> _stages;
        private List<StageWrapper> _stageWrappers;
        public List<StageWrapper> StageWrappers
        {
            get => _stageWrappers;
            set
            {
                _stageWrappers = value;
                OnPropertyChanged(nameof(StageWrappers));
            }
        }
        public TechLeadStageViewModel()
        {
            _dbController = DbController.Instance;
            _stages = new List<Stage>();
            _stageWrappers = new List<StageWrapper>();
            UpdateStages();
        }
        private void UpdateStages()
        {
            _stages.Clear();
            _stageWrappers.Clear();
            _stages = _dbController.GetStagesOfProject(TaskAssignmentState.SelectedProject);
            for (int i = 0; i < _stages.Count; i++)
            {
                StageWrapper stageWrapper = new StageWrapper(_stages[i]);
                List<Task> tasks = _dbController.GetAllTaskOfStage(stageWrapper.ID);
                int percentDone = 0;
                if (tasks != null && tasks.Count != 0) percentDone = (tasks.Where(t => t.Status == EnumMapper.mapToString(TaskStatus.Done)).Count() * 100 / tasks.Count);
                stageWrapper.InitializeUI(percentDone);
                _stageWrappers.Add(stageWrapper);
            }
            StageWrappers = new List<StageWrapper>(_stageWrappers);
        }
        private ICommand _cmdStageItemClick;
        public ICommand StageItemClickCommand
        {
            get
            {
                if (_cmdStageItemClick == null)
                {
                    _cmdStageItemClick = new RelayCommand(
                    p => true,
                    p => SaveCurrentStageToState(p));
                }
                return _cmdStageItemClick;
            }
        }
        private void SaveCurrentStageToState(object p)
        {
            if (p is null) return;
            string stageID = p as string;
            TaskAssignmentState.SelectedStage = _stages.Where(stage => stage.ID == stageID).FirstOrDefault();
        }
    }
}
