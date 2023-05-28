using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.HelperClasses;
using CompanyManagement.States;
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
    public class TechLeadProjectViewModel : ViewModelBase
    {
        private readonly DbController _dbController;
        private List<Project> _projects;
        private List<ProjectWrapper> _projectWrappers;
        public List<ProjectWrapper> ProjectWrappers
        {
            get => _projectWrappers;
            set
            {
                _projectWrappers = value;
                OnPropertyChanged(nameof(ProjectWrappers));
            }
        }

        public TechLeadProjectViewModel()
        {
            _dbController = DbController.Instance;
            _projects = new List<Project>();
            _projectWrappers = new List<ProjectWrapper>();
            UpdateProjects();
        }
        private void UpdateProjects()
        {
            _projects.Clear();
            _projectWrappers.Clear();
            _projects = _dbController.GetAllProjectsOfTechLead(LoginInfoState.Id);
            if (_projects is null) return;
            for (int i = 0; i < _projects.Count; i++)
            {
                ProjectWrapper projectWrapper = new ProjectWrapper(_projects[i]);
                List<Task> tasks = _dbController.GetAllTaskOfProject(projectWrapper.ID);
                int percentDone = 0;
                if (tasks != null && tasks.Count != 0) percentDone = (tasks.Where(t => t.Status == EnumMapper.mapToString(TaskStatus.Done)).Count() * 100 / tasks.Count);
                projectWrapper.InitializeUI(percentDone);
                _projectWrappers.Add(projectWrapper);
            }
            ProjectWrappers = new List<ProjectWrapper>(_projectWrappers);
        }
        private ICommand _cmdProjectItem;
        public ICommand ProjectItemClickCommand
        {
            get
            {
                if (_cmdProjectItem == null )
                {
                    _cmdProjectItem = new RelayCommand(
                    p => true,
                    p => ProjectItemClick(p));
                }
                return _cmdProjectItem;
            }
        }
        private void ProjectItemClick(object p)
        {
            if (p is null) return;
            string projectID = p as string;
            TaskAssignmentState.SelectedProject = _projects.Where(x => x.ID == projectID).ElementAt(0);
        }

    }
}
