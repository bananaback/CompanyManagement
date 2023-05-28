using CompanyManagement.EF;
using System;

namespace CompanyManagement.Factories
{
    public class TaskFactory
    {
        public static Task CreateNewTask(string Assignee, string Assigner, string TeamID, string Description, string Title, DateTime StartingTime, DateTime EndingTime)
        {
            string workId = Assignee + new Random().Next().ToString();
            Task task = new Task();
            task.ID = workId;
            task.Assignee = Assignee;
            task.Assigner = Assigner;
            task.TeamID = TeamID;
            task.Description = Description;
            task.Title = Title;
            task.StartingTime = StartingTime;
            task.EndingTime = EndingTime;
            task.Status = "WIP";
            task.CreatedAt = DateTime.Now;
            task.UpdatedAt = DateTime.Now;
            return task;
        }
    }
}
