using CompanyManagement.EF;
using System;

namespace CompanyManagement.Factories
{
    public class WorkLeaveFactory
    {
        private static string CreateRandomID()
        {
            return "WL" + new Random().Next();
        }

        public static WorkLeaf Create(string EmployeeID, DateTime FromDate, DateTime ToDate, string Reason)
        {
            var entry = new WorkLeaf();
            entry.ID = CreateRandomID();
            entry.EmployeeID = EmployeeID;
            entry.FromDate = FromDate;
            entry.ToDate = ToDate;
            entry.ReasonOfLeave = Reason;
            return entry;
        }
    }
}
