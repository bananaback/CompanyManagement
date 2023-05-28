using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.HelperClasses;
using CompanyManagement.States;
using CuoiKi.Enum;
using CUOIKI_EF.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace CompanyManagement.ViewModels
{
    public class StaffTaskManagementPageViewModel : ViewModelBase
    {
        private DbController db;
        private List<Task> _taskList;
        private int _percentDone;
        public int PercentDone
        {
            get => _percentDone;
            set
            {
                _percentDone = value;
                OnPropertyChanged(nameof(PercentDone));
            }
        }
        public SolidColorBrush BarColor { get; set; }
        public List<Task> TaskList
        {
            get => _taskList;
            set
            {
                _taskList = value;
                OnPropertyChanged(nameof(TaskList));
            }
        }
        private List<Task> _filteredTaskList;
        public List<Task> FilteredTaskList
        {
            get => _filteredTaskList;
            set
            {
                _filteredTaskList = value;
                OnPropertyChanged(nameof(FilteredTaskList));
            }
        }
        public StaffTaskManagementPageViewModel()
        {
            db = DbController.Instance;
            _taskList = new List<Task>();
            _filteredTaskList = new List<Task>();
            _filterList = new List<FilterName>();
            _taskFilterChain = new FilterChain<Task>();
            _filterOptions = new List<FilterName>() { FilterName.Done, FilterName.WIP, FilterName.NeedReview, FilterName.InThisYear, FilterName.InThisMonth, FilterName.InThisDay };
            _seletedFilter = (FilterName)0;
            fetchTaskList();
            // At first, there is no filter
            FilteredTaskList = TaskList;
            BarColor = Brushes.Gray;
            UpdateProgressBar();
        }
        private void UpdateProgressBar()
        {
            PercentDone = 0;
            if (FilteredTaskList != null && FilteredTaskList.Count != 0) PercentDone = (FilteredTaskList.Where(t => t.Status == EnumMapper.mapToString(TaskStatus.Done)).Count() * 100 / FilteredTaskList.Count);
            if (PercentDone < 30)
            {
                BarColor = Brushes.Red;
            }
            else if (PercentDone < 50)
            {
                BarColor = new SolidColorBrush(Color.FromRgb(255, 172, 0));
            }
            else if (PercentDone < 80)
            {
                BarColor = new SolidColorBrush(Color.FromRgb(249, 255, 85));
            }
            else
            {
                BarColor = new SolidColorBrush(Color.FromRgb(0, 255, 25));
            }
        }
        // Use some services to get tasks from database
        private void fetchTaskList()
        {
            _taskList.Clear();
            _taskList = db.GetAllTasksOfEmployee(LoginInfoState.Id);
        }

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
            // Find the index of the task to update
            int index = _taskList.IndexOf(_taskList.FirstOrDefault(t => t.ID == _selectedTask?.ID));
            if (index == -1) return;

            // Get the task to update
            Task taskToUpdate = _taskList[index];

            // Update the task's status
            taskToUpdate.Status = EnumMapper.mapToString(TaskStatus.Done);

            // Save the updated task using the _dbController
            db.Save(taskToUpdate);

            fetchTaskList();

            // Create a new list with the updated task
            var updatedList = new List<Task>(_taskList);
            updatedList[index] = taskToUpdate;

            // Assign the new list to FilteredTaskList
            FilteredTaskList = updatedList;
            ApplyFilter();
            UpdateProgressBar();

        }



        private FilterChain<Task> _taskFilterChain;
        private FilterName? _seletedFilter;
        public FilterName? SelectedFilter
        {
            get { return _seletedFilter; }
            set
            {
                _seletedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
            }
        }
        private List<FilterName> _filterList;
        public List<FilterName> FilterList
        {
            get => _filterList;
            set
            {
                _filterList = value;
                OnPropertyChanged(nameof(FilterList));
            }
        }
        private List<FilterName> _filterOptions;
        public List<FilterName> FilterOptions
        {
            get { return _filterOptions; }
            set
            {
                _filterOptions = value;
                OnPropertyChanged(nameof(FilterOptions));
            }
        }
        private ICommand _CmdRemoveFilterItem { get; set; }
        public ICommand CmdRemoveFilterItem
        {
            get
            {
                if (_CmdRemoveFilterItem == null)
                {
                    _CmdRemoveFilterItem = new RelayCommand(
                    p => true,
                    p => RemoveFilterItem(p));
                }
                return _CmdRemoveFilterItem;
            }
        }
        private void RemoveFilterItem(object p)
        {
            FilterName currName = (FilterName)p;
            FilterList = _filterList.Where(x => x != currName).ToList();
            FilterOptions = _filterOptions.Concat(new[] { currName }).ToList();

            _taskFilterChain.RemovePredicate(currName);

            ApplyFilter();
        }
        private ICommand _CmdAddFilter;
        public ICommand CmdAddFilter
        {
            get
            {
                if (_CmdAddFilter == null)
                {
                    _CmdAddFilter = new RelayCommand(
                    p => true,
                    p => addFilter());
                }
                return _CmdAddFilter;
            }
        }
        private void addFilter()
        {
            if (SelectedFilter is null) return;
            FilterName filterName = (FilterName)SelectedFilter;
            switch (filterName)
            {
                case FilterName.WIP:
                    _taskFilterChain.AddPredicate(filterName, FilterLogicType.Or, p => p.Status == EnumMapper.mapToString(TaskStatus.WIP));
                    break;
                case FilterName.NeedReview:
                    _taskFilterChain.AddPredicate(filterName, FilterLogicType.Or, p => p.Status == EnumMapper.mapToString(TaskStatus.NeedsReview));
                    break;
                case FilterName.Done:
                    _taskFilterChain.AddPredicate(filterName, FilterLogicType.Or, p => p.Status == EnumMapper.mapToString(TaskStatus.Done));
                    break;
                case FilterName.InThisYear:
                    RemoveTimeFilter();
                    _taskFilterChain.AddPredicate(filterName, FilterLogicType.And, p =>
                        p.StartingTime.Year == DateTime.Now.Year
                        || p.EndingTime.Year == DateTime.Now.Year);
                    break;
                case FilterName.InThisMonth:
                    RemoveTimeFilter();
                    _taskFilterChain.AddPredicate(filterName, FilterLogicType.And, p =>
                        p.StartingTime.Month == DateTime.Now.Month
                        || p.EndingTime.Month == DateTime.Now.Month);
                    break;
                case FilterName.InThisDay:
                    RemoveTimeFilter();
                    _taskFilterChain.AddPredicate(filterName, FilterLogicType.And, p =>
                        p.StartingTime.Date == DateTime.Now.Date
                        || p.EndingTime.Date == DateTime.Now.Date);
                    break;
                default:
                    break;
            }
            FilterList = _filterList.Concat(new[] { filterName }).ToList();
            FilterOptions = _filterOptions.Except(_filterList).ToList();
            SelectedFilter = FilterOptions.FirstOrDefault();

            ApplyFilter();
        }
        private void RemoveTimeFilter()
        {
            _taskFilterChain.RemovePredicate(FilterName.InThisYear);
            _taskFilterChain.RemovePredicate(FilterName.InThisMonth);
            _taskFilterChain.RemovePredicate(FilterName.InThisDay);
            _filterList.RemoveAll(filter => filter == FilterName.InThisYear || filter == FilterName.InThisMonth || filter == FilterName.InThisDay);
            FilterList = _filterList;
            if (!FilterOptions.Contains(FilterName.InThisYear))
            {
                FilterOptions.Add(FilterName.InThisYear);
            }

            if (!FilterOptions.Contains(FilterName.InThisMonth))
            {
                FilterOptions.Add(FilterName.InThisMonth);
            }

            if (!FilterOptions.Contains(FilterName.InThisDay))
            {
                FilterOptions.Add(FilterName.InThisDay);
            }

        }
        private void ApplyFilter()
        {
            bool FilterByStatus = FilterList.Any(f => f == FilterName.WIP || f == FilterName.NeedReview || f == FilterName.Done);
            bool FilterByTime = FilterList.Any(f => f == FilterName.InThisYear || f == FilterName.InThisMonth);
            IEnumerable<Task> filteredTasks = _taskList;
            if (_filterList.ToList().Count > 0)
            {
                if (FilterByStatus && FilterByTime)
                {
                    filteredTasks = _taskFilterChain.ApplyOrThenAnd(_taskList);
                }
                else if (FilterByStatus)
                {
                    filteredTasks = _taskFilterChain.ApplyOrLogic(_taskList);
                }
                else
                {
                    filteredTasks = _taskFilterChain.ApplyAndLogic(_taskList);
                }
            }
            FilteredTaskList = filteredTasks.ToList();
            UpdateProgressBar();

        }
    }
}
