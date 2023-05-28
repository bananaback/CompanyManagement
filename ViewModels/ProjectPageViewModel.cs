using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.Factories;
using CompanyManagement.HelperClasses;
using CompanyManagement.States;
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
    public class ProjectPageViewModel : ViewModelBase
    {
        private readonly DbController dbController;
        private List<Project> _projectList;
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
        private string _projectID = "";
        private Visibility _VisID = Visibility.Collapsed;
        private string _currentManagerID = "";
        public ProjectPageViewModel()
        {
            dbController = new DbController();
            _currentManagerID = LoginInfoState.Id;
            _projectList = new List<Project>();
            _projectWrappers = new List<ProjectWrapper>();
            ProjectWrappers = new List<ProjectWrapper>();
            fetchProjectList();
        }
        private void fetchProjectList()
        {
            _projectList.Clear();
            _projectList = dbController.GetProjectsOfAccount(LoginInfoState.Id);
            _projectWrappers.Clear();
            ProjectWrappers.Clear();
            for (int i = 0; i < _projectList.Count; i++)
            {
                ProjectWrapper projectWrapper = new ProjectWrapper(_projectList[i]);
                List<Task> tasks = dbController.GetAllTaskOfProject(projectWrapper.ID);
                int percentDone = 0;
                if (tasks != null && tasks.Count != 0) percentDone = (tasks.Where(t => t.Status == EnumMapper.mapToString(Enums.TaskStatus.Done)).Count() * 100 / tasks.Count);
                projectWrapper.InitializeUI(percentDone);
                _projectWrappers.Add(projectWrapper);
            }
            ProjectWrappers = new List<ProjectWrapper>(_projectWrappers);
        }
        public string ProjectID
        {
            get { return _projectID; }
            set
            {
                _projectID = value;
                if (value.Length > 0)
                {
                    _VisID = Visibility.Visible;
                }
                else _VisID = Visibility.Collapsed;
                OnPropertyChanged(nameof(ProjectID));
            }
        }
        public Visibility VisID
        {
            get { return _VisID; }
            set { _VisID = value; OnPropertyChanged(nameof(VisID)); }
        }
        public string CurrentManagerID
        {
            get => _currentManagerID;
            set
            {
                _currentManagerID = value;
                OnPropertyChanged(nameof(CurrentManagerID));
            }
        }

        #region Save project logic
        private string _toBeSavedProjectName = "";
        public string ToBeSavedProjectName
        {
            get { return _toBeSavedProjectName; }
            set
            {
                _toBeSavedProjectName = value;
                OnPropertyChanged(nameof(ToBeSavedProjectName));
                CheckValidProjectInput();
            }
        }
        private string _toBeSavedProjectDescription = "";
        public string ToBeSavedProjectDescription
        {
            get { return _toBeSavedProjectDescription; }
            set
            {
                _toBeSavedProjectDescription = value;
                OnPropertyChanged(nameof(ToBeSavedProjectDescription));
                CheckValidProjectInput();
            }
        }
        public void SaveProject()
        {
            Project project = TaskAssignmentState.SelectedProject ?? ProjectFactory.CreateNewProject(ToBeSavedProjectName, ToBeSavedProjectDescription);
            project.Name = ToBeSavedProjectName;
            project.Description = ToBeSavedProjectDescription;
            dbController.Save(project);
            fetchProjectList();
        }

        private bool canSaveProject = false;
        private ICommand _saveProjectCommand;
        public ICommand SaveProjectCommand
        {
            get
            {
                if (_saveProjectCommand == null)
                {
                    _saveProjectCommand = new RelayCommand(
                        p => this.canSaveProject,
                        p => this.SaveProject());
                }
                return _saveProjectCommand;
            }
        }
        public void CheckValidProjectInput()
        {
            canSaveProject = !string.IsNullOrEmpty(ToBeSavedProjectName);
            canSaveProject = !string.IsNullOrEmpty(ToBeSavedProjectDescription);
            // TODO: check if project name existed...
        }
        #endregion

        #region Project click logic
        private bool canProjectItemClick = true;
        private ICommand _projectItemClickCommand;
        public ICommand ProjectItemClickCommand
        {
            get
            {
                if (_projectItemClickCommand == null)
                {
                    _projectItemClickCommand = new RelayCommand(
                        obj => canProjectItemClick,
                        obj => SaveProjectIdToState(obj)
                    );
                }
                return _projectItemClickCommand;
            }
        }

        private void SaveProjectIdToState(object parameter)
        {
            if (parameter == null) { return; }
            var projectId = parameter as string;
            TaskAssignmentState.SelectedProject = _projectList.Where(x => x.ID == projectId).ElementAt(0);
            ToBeSavedProjectDescription = TaskAssignmentState.SelectedProject.Description;
            ToBeSavedProjectName = TaskAssignmentState.SelectedProject.Name;
            ProjectID = TaskAssignmentState.SelectedProject.ID;
        }
        #endregion

        #region Add Project Click logic
        private ICommand _addProjectClickCommand;

        public ICommand AddProjectClickCommand
        {
            get
            {
                if (_addProjectClickCommand == null)
                {
                    _addProjectClickCommand = new RelayCommand(
                        p => true,
                        p => OpenAddProjectForm()
                    );
                }
                return _addProjectClickCommand;
            }
        }

        private void OpenAddProjectForm()
        {
            TaskAssignmentState.SelectedProject = null;
            ToBeSavedProjectName = "";
            ToBeSavedProjectDescription = "";
            ProjectID = "";
            var prjForm = new ProjectForm(this);
            prjForm.Show();
        }
        #endregion

        #region Context menu command functions
        // Can execute variables
        private bool canViewProject = true;
        private bool canEditProject = true;
        private bool canDeleteProject = true;
        private void ViewProject()
        {
            TaskAssignmentState.SelectedProject = _projectList.Where(x => x.ID == ProjectID).ElementAt(0);
        }
        private void EditProject()
        {
            var prjForm = new ProjectForm(this);
            prjForm.Show();
        }
        private void DeleteProject()
        {
            MessageBox.Show(TaskAssignmentState.SelectedProject.ID.ToString());
            dbController.Delete(TaskAssignmentState.SelectedProject);
            fetchProjectList();
        }
        #endregion

        #region Context menu commands
        private ICommand _cmdViewProject;
        public ICommand CmdViewProject
        {
            get
            {
                if (_cmdViewProject == null)
                {
                    _cmdViewProject = new RelayCommand(
                        p => canViewProject,
                        p => ViewProject()
                    );
                }
                return _cmdViewProject;
            }
        }
        private ICommand _cmdEditProject;
        public ICommand CmdEditProject
        {
            get
            {
                if (_cmdEditProject == null)
                {
                    _cmdEditProject = new RelayCommand(
                        p => canEditProject,
                        p => EditProject());
                }
                return _cmdEditProject;
            }
        }
        private ICommand _cmdDeleteProject;
        public ICommand CmdDeleteProject
        {
            get
            {
                if (_cmdDeleteProject == null)
                {
                    _cmdDeleteProject = new RelayCommand(
                        p => canDeleteProject,
                        p => DeleteProject());
                }
                return _cmdDeleteProject;
            }
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
            fetchProjectList();
        }
    }
}
