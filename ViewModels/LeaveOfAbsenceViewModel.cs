using CompanyManagement.EF;
using CompanyManagement.Factories;
using CompanyManagement.HelperClasses;
using CompanyManagement.States;
using CompanyManagement.UI.Forms;
using CuoiKi.Enum;
using CUOIKI_EF.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace CompanyManagement.ViewModels
{
    public class LeaveOfAbsencePageViewModel : ViewModelBase
    {
        private DbController _controller;
        private List<WorkLeaf> _workLeaves;
        private FilterChain<WorkLeaf> _filterChain;
        private List<FilterName> _filterOptions;
        public List<FilterName> FilterOptions
        {
            get => _filterOptions;
            set
            {
                _filterOptions = value;
                OnPropertyChanged(nameof(FilterOptions));
            }
        }
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
        private List<WorkLeaf> _filteredWorkLeave;
        public List<WorkLeaf> FilteredWorkLeave
        {
            get => _filteredWorkLeave;
            set
            {
                _filteredWorkLeave = value;
                OnPropertyChanged(nameof(FilteredWorkLeave));
            }
        }
        public LeaveOfAbsencePageViewModel()
        {
            _controller = DbController.Instance;
            _workLeaves = new List<WorkLeaf>();
            _filterChain = new FilterChain<WorkLeaf>();
            _filterList = new List<FilterName>();
            _filterOptions = new List<FilterName>() { FilterName.InThisYear, FilterName.InThisMonth };
            _seletedFilter = (FilterName)0;
            fetchWorkLeaves();

            _filteredWorkLeave = _workLeaves;
            FilteredWorkLeave = _workLeaves;
        }

        private void fetchWorkLeaves()
        {
            _workLeaves.Clear();
            _workLeaves = _controller.GetAllWorkLeavesOfEmployee(_employeeID);
        }

        #region UI Binding
        private string _employeeID = LoginInfoState.Id;
        public string EmployeeID
        {
            get { return _employeeID; }
            set { }
        }

        private DateTime _fromDate = DateTime.Now;
        public DateTime FromDate
        {
            get
            {
                return _fromDate;
            }
            set
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
            }
        }

        private DateTime _toDate = DateTime.Now;
        public DateTime ToDate
        {
            get
            {
                return _toDate;
            }
            set
            {
                _toDate = value;
                OnPropertyChanged(nameof(ToDate));
            }
        }

        private string _reason = string.Empty;
        public string Reason
        {
            get
            {
                return _reason;
            }
            set
            {
                _reason = value;
                OnPropertyChanged(nameof(Reason));
            }
        }

        private RelayCommand _CmdSaveWorkLeave;
        public RelayCommand CmdSaveWorkLeave
        {
            get
            {
                if (_CmdSaveWorkLeave == null)
                {
                    _CmdSaveWorkLeave = new RelayCommand(
                        p => true,
                        p => SaveWorkLeave()
                    );
                }
                return _CmdSaveWorkLeave;
            }
        }
        #endregion

        private RelayCommand _CmdAddWorkLeave;
        public RelayCommand CmdAddWorkLeave
        {
            get
            {
                if (_CmdAddWorkLeave == null)
                {
                    _CmdAddWorkLeave = new RelayCommand(
                        p => true,
                        p => OpenAddWorkLeaveForm()
                    );
                }
                return _CmdAddWorkLeave;
            }
        }

        private void OpenAddWorkLeaveForm()
        {
            var form = new WorkLeaveForm(this);
            form.Show();
        }

        private void SaveWorkLeave()
        {
            if (!(FromDate >= DateTime.Today && ToDate > FromDate && Reason.Length > 0))
            {
                return;
            }
            _controller.Save(WorkLeaveFactory.Create(EmployeeID, FromDate, ToDate, Reason));
            fetchWorkLeaves();
            FilteredWorkLeave = _workLeaves;
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
                case FilterName.InThisYear:
                    RemoveTimeFilter();
                    _filterChain.AddPredicate(filterName, FilterLogicType.And, p =>
                        p.FromDate.Year == DateTime.Now.Year);
                    break;
                case FilterName.InThisMonth:
                    RemoveTimeFilter();
                    _filterChain.AddPredicate(filterName, FilterLogicType.And, p =>
                        p.FromDate.Month == DateTime.Now.Month);
                    break;
                default:
                    break;
            }
            FilterList = _filterList.Concat(new[] { filterName }).ToList();
            FilterOptions = _filterOptions.Except(_filterList).ToList();
            SelectedFilter = FilterOptions.FirstOrDefault();

            ApplyFilter();
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
            _filterChain.RemovePredicate(currName);

            ApplyFilter();
        }
        private void RemoveTimeFilter()
        {
            _filterChain.RemovePredicate(FilterName.InThisYear);
            _filterChain.RemovePredicate(FilterName.InThisMonth);
            _filterList.RemoveAll(filter => filter == FilterName.InThisYear || filter == FilterName.InThisMonth);
            FilterList = _filterList;
            if (!FilterOptions.Contains(FilterName.InThisYear))
            {
                FilterOptions.Add(FilterName.InThisYear);
            }

            if (!FilterOptions.Contains(FilterName.InThisMonth))
            {
                FilterOptions.Add(FilterName.InThisMonth);
            }
        }
        private void ApplyFilter()
        {
            FilteredWorkLeave = _filterChain.ApplyAndLogic(_workLeaves).ToList();
        }
    }
}
