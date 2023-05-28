using CompanyManagement.EF;
using CompanyManagement.States;
using System;

namespace CompanyManagement.Factories
{
    public class ProjectFactory
    {
        private static string CreateRandomID()
        {
            return "Prj" + DateTime.Now.ToShortDateString().Replace("/", "") + new Random().Next(1000, 9999);
        }

        public static Project Create(string ManagerID, string ProjectName, string Description)
        {
            var prj = new Project();
            prj.ID = CreateRandomID();
            prj.ManagerID = ManagerID;
            prj.CreatedAt = DateTime.Now;
            prj.Description = Description;
            prj.Name = ProjectName;
            return prj;
        }

        public static Project CreateNewProject(string ProjectName, string Description)
        {
            return Create(LoginInfoState.Id, ProjectName, Description);
        }
    }
}
