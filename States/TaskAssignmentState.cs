using CompanyManagement.EF;

namespace CuoiKi.States
{
    public static class TaskAssignmentState
    {
        public static Project SelectedProject { get; set; }
        public static Stage SelectedStage { get; set; }
        public static Team SelectedTeam { get; set; }
        public static Employee SelectedEmployee { get; set; }
    }
}