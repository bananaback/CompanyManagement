using System;

namespace CompanyManagement.Enums
{
    public class EnumMapper
    {
        public static String mapToString(EmployeeStatus status)
        {
            return status == EmployeeStatus.Active
                    ? nameof(EmployeeStatus.Active)
                    : nameof(EmployeeStatus.Disabled);
        }
        public static String mapToString(Gender gender)
        {
            return gender == Gender.Male
                    ? nameof(Gender.Male)
                    : nameof(Gender.Female);
        }
        public static String mapToString(Role role)
        {
            return role == Role.Dev
                ? nameof(Role.Dev)
                : role == Role.Designer
                ? nameof(Role.Designer)
                : role == Role.Tester
                ? nameof(Role.Tester)
                : role == Role.TechLead
                ? nameof(Role.TechLead)
                : role == Role.Manager
                ? nameof(Role.Manager)
                : nameof(Role.Hr);
        }
        public static String mapToString(WorkSessionStatus status)
        {
            return status == WorkSessionStatus.CheckedIn
                    ? "CHECKED IN"
                    : "CHECKED OUT";
        }
        public static String mapToString(TaskStatus status)
        {
            return status == TaskStatus.WIP
                ? nameof(TaskStatus.WIP)
                : status == TaskStatus.NeedsReview
                ? nameof(TaskStatus.NeedsReview)
                : nameof(TaskStatus.Done);
        }
    }
}
