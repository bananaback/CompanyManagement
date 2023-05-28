using CompanyManagement.EF;
using System;

namespace CompanyManagement.Factories
{
    public class WorkSessionFactory
    {
        public static WorkSession CreateWorkSession(string employeeID)
        {
            WorkSession workSession = new WorkSession();
            workSession.ID = employeeID + DateTime.Now.ToShortDateString().Replace("/", "") + new Random().Next(1000, 9999);
            workSession.EmployeeID = employeeID;
            workSession.StartingTime = DateTime.Now;
            workSession.EndingTime = null;
            return workSession;
        }
    }
}
