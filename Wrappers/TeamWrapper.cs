using CompanyManagement.EF;

namespace CompanyManagement.Wrappers
{
    public class TeamWrapper : Wrapper
    {
        private readonly Team _team;
        public TeamWrapper(Team team) : base()
        {
            _team = team;
        }
        public string ID => _team.ID;
        public string Name => _team.Name;
        public string TechLeadID => _team.TechLeadID;
    }
}
