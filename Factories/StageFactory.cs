using CompanyManagement.EF;
using System;

namespace CompanyManagement.Factories
{
    public class StageFactory
    {
        private static string CreateRandomID()
        {
            return "Stg" + DateTime.Now.ToShortDateString().Replace("/", "") + new Random().Next(1000, 9999);
        }

        public static Stage Create(string StageID, string Description)
        {
            var stg = new Stage();
            stg.ID = CreateRandomID();
            stg.ProjectID = StageID;
            stg.Description = Description;
            return stg;
        }

        public static Stage CreateNewStage(string ID, string ProjectID, string Description)
        {
            var stg = new Stage();
            stg.ID = ID;
            stg.ProjectID = ProjectID;
            stg.Description = Description;
            return stg;
        }

        public static Stage CreateNewStage(string StageID, string Description)
        {
            return Create(StageID, Description);
        }
    }
}
