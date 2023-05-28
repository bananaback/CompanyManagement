using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;

namespace CompanyManagement.Controllers
{
    public class WorkSessionController
    {
        public WorkSessionController()
        {

        }
        // Thinking a better name for these function...
        public bool CheckInAndReturnSuccessOrNot(string employeeId)
        {
            using (var dbContext = new CompanyContext())
            {
                var foundEmployee = dbContext.Employees.FirstOrDefault(e => e.ID == employeeId);
                if (foundEmployee == null)
                {
                    MessageBox.Show("EmployeeId not found");
                    return false;
                }

                var workSessionStatus = GetWorkSessionStatus(employeeId);
                if (workSessionStatus == WorkSessionStatus.CheckedIn)
                {
                    MessageBox.Show("You already checked in");
                    return false;
                }

                var newWorkSession = WorkSessionFactory.CreateWorkSession(employeeId);
                dbContext.WorkSessions.Add(newWorkSession);
                dbContext.SaveChanges();

                MessageBox.Show("Check in success");
                return true;
            }
        }
        public bool CheckOutAndReturnSuccessOrNot(string employeeId)
        {
            using (var dbContext = new CompanyContext())
            {
                var foundEmployee = dbContext.Employees.FirstOrDefault(e => e.ID == employeeId);
                if (foundEmployee == null)
                {
                    MessageBox.Show("EmployeeId not found");
                    return false;
                }

                var unfinishedWorkSession = GetUnfinishedWorkSession(employeeId);
                if (unfinishedWorkSession == null)
                {
                    MessageBox.Show("You already checked out");
                    return false;
                }

                unfinishedWorkSession.EndingTime = DateTime.Now;
                dbContext.WorkSessions.AddOrUpdate(unfinishedWorkSession);
                dbContext.SaveChanges();

                MessageBox.Show("Check out success");
                return true;
            }
        }
        public WorkSessionStatus GetWorkSessionStatus(string employeeId)
        {
            if (GetUnfinishedWorkSession(employeeId) != null)
            {
                return WorkSessionStatus.CheckedIn;
            }
            return WorkSessionStatus.CheckedOut;
        }
        public WorkSession GetUnfinishedWorkSession(string employeeId)
        {
            using (var dbContext = new CompanyContext())
            {
                var foundEmployee = dbContext.Employees.Where(e => e.ID == employeeId).FirstOrDefault();
                if (foundEmployee == null)
                {
                    MessageBox.Show("EmployeeId not found");
                    return null;
                }
                var unfinishedWorkSession = dbContext.WorkSessions.FirstOrDefault(ws => ws.EmployeeID == employeeId && (ws.EndingTime == null || ws.EndingTime == DateTime.MinValue));
                return unfinishedWorkSession;
            }
        }
        public List<WorkSession> GetAllWorkSessions()
        {
            using (var dbContext = new CompanyContext())
            {
                var workSessions = dbContext.WorkSessions.ToList();
                return workSessions;
            }
        }

        public List<WorkSession> GetAllWorkSessionsOfAnEmployee(string employeeID)
        {
            using (var dbContext = new CompanyContext())
            {
                var workSessions = dbContext.WorkSessions.Where(ws => ws.EmployeeID == employeeID).ToList();
                return workSessions;
            }
        }

        public List<WorkSession> GetAllWorkSessionOfAnEmployeeInSelectedMonth(string employeeID, DateTime dateInMonth)
        {
            var startDate = new DateTime(dateInMonth.Year, dateInMonth.Month, 1);
            var endDate = startDate.AddMonths(1).AddSeconds(-1);

            using (var dbContext = new CompanyContext())
            {
                var workSessions = dbContext.WorkSessions
                    .Where(ws => ws.EmployeeID == employeeID && ws.StartingTime >= startDate && ws.StartingTime <= endDate)
                    .ToList();

                return workSessions;
            }
        }

        public WorkSession GetLastestWorkSession(string employeeID)
        {
            using (var dbContext = new CompanyContext())
            {
                var lastestWorkSession = dbContext.WorkSessions
                    .Where(ws => ws.EmployeeID == employeeID)
                    .OrderByDescending(ws => ws.StartingTime)
                    .FirstOrDefault();

                return lastestWorkSession;
            }
        }

    }
}
