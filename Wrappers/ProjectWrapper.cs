using CompanyManagement.EF;

namespace CompanyManagement.Wrappers
{
    public class ProjectWrapper : Wrapper
    {
        private readonly Project _project;
        public ProjectWrapper(Project project) : base()
        {
            _project = project;
        }
        public string ID => _project.ID;
        public string Name => _project.Name;
        public string Description => _project.Description;
    }
}
