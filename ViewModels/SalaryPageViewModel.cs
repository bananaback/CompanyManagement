using CompanyManagement.EF;
using CompanyManagement.States;
using CUOIKI_EF.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CompanyManagement.ViewModels
{
    public class SalaryPageViewModel : ViewModelBase
    {
        private readonly DbController db;
        public SalaryPageViewModel()
        {
            db = DbController.Instance;
            FetchSalary();
        }
        public string EmployeeID
        {
            get => LoginInfoState.Id;
            set { }
        }

        public string EmployeeName
        {
            get => LoginInfoState.Name;
            set { }
        }

        private DateTime _PickedDate = DateTime.Now;
        private string _PickedDateMessage
        {
            get => string.Format("Viewing Salary of {0}/{1}", PickedDate.Month + 1, PickedDate.Year);
            set { }
        }
        public DateTime PickedDate
        {
            get => _PickedDate;
            set
            {
                _PickedDate = value;
                OnPropertyChanged(nameof(PickedDate));
                MessageBox.Show(_PickedDateMessage);
            }
        }

        private int _BasicPay;
        public int BasicPay
        {
            get => _BasicPay;
            set
            {
                _BasicPay = value;
                OnPropertyChanged(nameof(BasicPay));
            }
        }

        private int kpiCost;

        private int _DelayedTasksCnt;
        public int DelayedTasksCnt
        {
            get => _DelayedTasksCnt;
            set
            {
                _DelayedTasksCnt = value;
                OnPropertyChanged(nameof(DelayedTasksCnt));
            }
        }

        public int TotalPay
        {
            get => BasicPay - DelayedTasksCnt * kpiCost;
            set { }
        }


        private void FetchSalary()
        {
            DateTime firstDayOfMonth = new DateTime(PickedDate.Year, PickedDate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            Salary s = db.GetSalaryOfEmployee(LoginInfoState.Id);
            List<Task> delayedTasks = db.GetDelayedTasksOfEmployee(LoginInfoState.Id, firstDayOfMonth, lastDayOfMonth);
            BasicPay = s.basicPay;
            kpiCost = s.kpiCost;
            DelayedTasksCnt = delayedTasks?.Count ?? 0;
        }
    }
}
