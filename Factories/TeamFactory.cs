using CompanyManagement.EF;
using System;

namespace CompanyManagement.Factories
{
    public class TeamFactory
    {
        public static Team CreateNewTeam(string StageID, string TechLeadID, string Name)
        {
            string ID = "Team-" + new Random().Next().ToString();
            Team team = new Team();
            team.ID = ID;
            team.Name = Name;
            team.StageID = StageID;
            team.TechLeadID = TechLeadID;
            return team;
        }
    }
}
