using CompanyManagement.EF;
using CompanyManagement.Factories;
using CompanyManagement.HelperClasses;
using CompanyManagement.States;
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
        private DbController _dbController;
        private List<Project> _projectList;
        private string _projectID = "";
        private Visibility _VisID = Visibility.Collapsed;

        public ProjectPageViewModel()
        {
            _dbController = DbController.Instance;
            _projectList = new List<Project>();
            fetchProjectList();
        }

        private void fetchProjectList()
        {
            _projectList.Clear();
            ProjectList = _dbController.GetProjectsOfCurrentUser();
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
        public List<Project> ProjectList
        {
            get { return _projectList; }
            set
            {
                _projectList = value;
                OnPropertyChanged(nameof(ProjectList));
            }
        }

        public string CurrentManagerID
        {
            get => LoginInfoState.Id;
            set { }
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
            Project project
                = TaskAssignmentState.SelectedProject ?? ProjectFactory.Create(CurrentManagerID, ToBeSavedProjectName, ToBeSavedProjectDescription);
            project.Name = ToBeSavedProjectName;
            project.Description = ToBeSavedProjectDescription;

            _dbController.Save(project);
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
            TaskAssignmentState.SelectedProject = _projectList.Where(x => x.ID == projectId).First();
            ToBeSavedProjectDescription = TaskAssignmentState.SelectedProject.Description;
            ToBeSavedProjectName = TaskAssignmentState.SelectedProject.Name;
            ProjectID = TaskAssignmentState.SelectedProject.ID;
        }
        #endregion

        #region Add Project Click logic
        private ICommand _AddProjectClickCommand;
        public ICommand AddProjectClickCommand
        {
            get
            {
                if (_AddProjectClickCommand == null)
                {
                    _AddProjectClickCommand = new RelayCommand(
                        p => true,
                        p => OpenAddProjectForm()
                    );
                }
                return _AddProjectClickCommand;
            }
        }

        private void OpenAddProjectForm()
        {
            TaskAssignmentState.SelectedProject = null;
            ToBeSavedProjectName = "";
            ToBeSavedProjectDescription = "";
            ProjectID = "";
            //var prjForm = new ProjectForm(this);
            //prjForm.Show();
        }
        #endregion

        #region Context menu command functions
        // Can execute variables
        private bool canEditProject = true;
        private bool canDeleteProject = true;
        private void EditProject()
        {
            //var prjForm = new ProjectForm(this);
            //prjForm.Show();
        }
        private void DeleteProject()
        {
            MessageBox.Show("Delete project");
        }
        #endregion

        #region Context menu commands
        private ICommand _cmdEditProject;
        public ICommand CmdEditProject
        {
            get
            {
                if (_cmdEditProject == null)
                {
                    _cmdEditProject = new RelayCommand(
                        p => canEditProject,
                        p => EditProject()
                    );
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
    }
}
