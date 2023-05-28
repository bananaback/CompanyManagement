using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.States;
using CuoiKi.States;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace CUOIKI_EF.Controllers
{
    public class DbController
    {
        private static DbController instance;
        private static CompanyContext db;
        public DbController()
        {
            db = new CompanyContext();
        }

        public static DbController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DbController();
                }
                return instance;
            }
        }
        public void Save<T>(T entry)
        {
            if (entry is Project)
            {
                db.Projects.AddOrUpdate(entry as Project);
            }
            else if (entry is Stage)
            {
                db.Stages.AddOrUpdate(entry as Stage);
            }
            else if (entry is Team)
            {
                db.Teams.AddOrUpdate(entry as Team);
            }
            else if (entry is TeamMember)
            {
                db.TeamMembers.AddOrUpdate(entry as TeamMember);
            }
            else if (entry is Task)
            {
                db.Tasks.AddOrUpdate(entry as Task);
            }
            else if (entry is WorkSession)
            {
                db.WorkSessions.AddOrUpdate(entry as WorkSession);
            }
            else if (entry is WorkLeaf)
            {
                db.WorkLeaves.AddOrUpdate(entry as WorkLeaf);
            }

            db.SaveChanges();
        }

        public void Delete<T>(T entry)
        {
            if (entry is Project)
            {
                Project pe = (entry as Project);
                var y = (from x in db.Projects where x.ID == pe.ID select x).First();
                db.Projects.Remove(y);
            }
            else if (entry is Stage)
            {
                Stage se = (entry as Stage);
                var y = (from x in db.Stages where x.ID == se.ID select x).First();
                db.Stages.Remove(y);
            }
            else if (entry is Team)
            {
                Team te = (entry as Team);
                var y = (from x in db.Teams where x.ID == te.ID select x).First();
                db.Teams.Remove(y);
            }
            else if (entry is TeamMember)
            {
                db.TeamMembers.Remove(entry as TeamMember);
            }
            else if (entry is Task)
            {
                db.Tasks.Remove(entry as Task);
            }
            else if (entry is WorkSession)
            {
                db.WorkSessions.Remove(entry as WorkSession);
            }
            db.SaveChanges();
        }

        public List<Project> GetProjectsOfCurrentUser()
        {
            if (LoginInfoState.Role == Role.Manager)
            {
                return db.Projects.Where(x => x.ManagerID == LoginInfoState.Id).ToList();
            }
            else if (LoginInfoState.Role == Role.TechLead)
            {
                var query = from prj in db.Projects
                            join stg in db.Stages on prj.ID equals stg.ProjectID
                            join tm in db.Teams on stg.ID equals tm.StageID
                            where tm.TechLeadID == LoginInfoState.Id
                            select prj;
                return query.ToList();
            }
            else
            {
                var query = from prj in db.Projects
                            join stg in db.Stages on prj.ID equals stg.ProjectID
                            join tm in db.Teams on stg.ID equals tm.StageID
                            join member in db.TeamMembers on tm.ID equals member.ID
                            join employee in db.Employees on member.EmployeeID equals employee.ID
                            where employee.ID == LoginInfoState.Id
                            select prj;
                return query.ToList();
            }
        }

        public T GetByID<T>(string id)
        {
            if (typeof(T) == typeof(Project))
            {
                return (T)(object)db.Projects.Where(x => x.ID == id).FirstOrDefault();
            }
            else if (typeof(T) == typeof(Stage))
            {
                return (T)(object)db.Stages.Where(x => x.ID == id).FirstOrDefault();
            }
            else if (typeof(T) == typeof(Team))
            {
                return (T)(object)db.Teams.Where(x => x.ID == id).FirstOrDefault();
            }
            else if (typeof(T) == typeof(Task))
            {
                return (T)(object)db.Tasks.Where(x => x.ID == id).FirstOrDefault();
            }
            else if (typeof(T) == typeof(WorkSession))
            {
                return (T)(object)db.WorkSessions.Where(x => x.ID == id).FirstOrDefault();
            }
            else return (T)(object)db.Employees.Where(x => x.ID == id).FirstOrDefault();
        }

        public List<Stage> GetStagesOfCurrentProject()
        {
            return db.Stages.Where(x => x.ProjectID == TaskAssignmentState.SelectedProject.ID).ToList();
        }

        public List<Team> GetTeamsOfCurrentStage()
        {
            return db.Teams.Where(x => x.StageID == TaskAssignmentState.SelectedStage.ID).ToList();
        }

        public List<Employee> GetEmployeesByRole(Role role)
        {
            string r = EnumMapper.mapToString(role);
            return db.Employees.Where(x => x.Role == r).ToList();
        }

        public List<WorkLeaf> GetAllWorkLeavesOfEmployee(string employeeID)
        {
            return db.WorkLeaves.Where(x => x.EmployeeID == employeeID).ToList();
        }

        public List<Task> GetAllTasksOfEmployee(string employeeID)
        {
            return db.Tasks.Where(x => x.Assignee == employeeID).ToList();
        }

        public List<Project> GetProjectsOfAccount(string ID)
        {
            using (var dbContext = new CompanyContext())
            {
                var projects = dbContext.Projects
                    .Where(p => p.ManagerID == ID)
                    .ToList();
                return projects;
            }
        }

        public List<Task> GetAllTaskOfProject(string ID)
        {
            using (var dbContext = new CompanyContext())
            {
                var tasks = dbContext.Tasks
                    .Join(dbContext.Teams, t => t.TeamID, team => team.ID, (t, team) => new { Task = t, Team = team })
                    .Join(dbContext.Stages, tt => tt.Team.StageID, stage => stage.ID, (tt, stage) => new { tt.Task, Team = tt.Team, Stage = stage })
                    .Join(dbContext.Projects, ttt => ttt.Stage.ProjectID, project => project.ID, (ttt, project) => new { ttt.Task, Team = ttt.Team, Stage = ttt.Stage, Project = project })
                    .Where(ptsp => ptsp.Project.ID == ID)
                    .Select(ptst => ptst.Task)
                    .ToList();
                return tasks;
            }
        }

        public Salary GetSalaryOfEmployee(string employeeID)
        {
            return db.Salaries.Where(x => x.ID == employeeID).FirstOrDefault();
        }

        public List<Task> GetDelayedTasksOfEmployee(string employeeID, DateTime startTime, DateTime endTime)
        {
            string unfinishedTaskStatus = EnumMapper.mapToString(TaskStatus.WIP);
            return db.Tasks.Where(x => x.Assignee == employeeID && x.Status == unfinishedTaskStatus && x.EndingTime < endTime && x.StartingTime > startTime).ToList();
        }

        public List<Stage> GetStagesOfProject(Project selectedProject)
        {
            using (var dbContext = new CompanyContext())
            {
                var stages = db.Stages.Where(p => p.ProjectID == selectedProject.ID).ToList();
                return stages;
            }
        }

        public List<Task> GetAllTaskOfStage(string ID)
        {
            using (var dbContext = new CompanyContext())
            {
                var tasks = dbContext.Tasks
                    .Join(dbContext.Teams, t => t.TeamID, team => team.ID, (t, team) => new { Task = t, Team = team })
                    .Join(dbContext.Stages, tt => tt.Team.StageID, stage => stage.ID, (tt, stage) => new { tt.Task, Team = tt.Team, Stage = stage })
                    .Where(st => st.Stage.ID == ID)
                    .Select(stt => stt.Task)
                    .ToList();
                return tasks;
            }
        }
        public List<Employee> GetAllEmployeeOfARole(Role role)
        {
            string roleAsString = EnumMapper.mapToString(role);
            using (var dbContext = new CompanyContext())
            {
                var employees = dbContext.Employees
                    .Where(e => e.Role == roleAsString)
                    .ToList();
                return employees;
            }
        }
        public List<Team> GetTeamsOfAStage(Stage stage)
        {
            using (var dbContext = new CompanyContext())
            {
                var teams = dbContext.Teams
                    .Where(t => t.StageID == stage.ID)
                    .ToList();

                return teams;
            }
        }
        public List<Task> GetAllTaskOfTeam(string teamID)
        {
            using (var dbContext = new CompanyContext())
            {
                var tasks = dbContext.Tasks
                    .Where(t => t.TeamID == teamID)
                    .ToList();

                return tasks;
            }
        }
        public void AddTeamMembersToTeam(string teamID, List<string> employeeIDs)
        {
            using (var dbContext = new CompanyContext())
            {
                var team = dbContext.Teams.FirstOrDefault(t => t.ID == teamID);
                if (team == null)
                {
                    // Handle the case when the team does not exist
                    return;
                }

                foreach (string eID in employeeIDs)
                {
                    var employee = dbContext.Employees.FirstOrDefault(e => e.ID == eID);
                    if (employee != null)
                    {
                        var teamMember = new TeamMember
                        {
                            ID = "TeamMember" + teamID + new Random().Next(1000, int.MaxValue).ToString(),
                            TeamID = teamID,
                            EmployeeID = eID
                        };
                        dbContext.TeamMembers.AddOrUpdate(teamMember);
                    }
                }

                dbContext.SaveChanges();
            }
        }
        public void RemoveTeamMembers(List<string> toBeRemovedTeamMemberIDs)
        {
            using (var dbContext = new CompanyContext())
            {
                var teamMembers = dbContext.TeamMembers
                    .Where(tm => toBeRemovedTeamMemberIDs.Contains(tm.ID))
                    .ToList();

                dbContext.TeamMembers.RemoveRange(teamMembers);
                dbContext.SaveChanges();
            }
        }
        public List<TeamMember> GetAllMembersOfTeam(Team team)
        {
            using (var dbContext = new CompanyContext())
            {
                var teamMembers = dbContext.TeamMembers
                    .Where(tm => tm.TeamID == team.ID)
                    .ToList();

                return teamMembers;
            }
        }
        public List<Employee> GetAllWorkers()
        {
            using (var dbContext = new CompanyContext())
            {
                var roles = new List<string>
                {
                    EnumMapper.mapToString(Role.TechLead),
                    EnumMapper.mapToString(Role.Dev),
                    EnumMapper.mapToString(Role.Designer),
                    EnumMapper.mapToString(Role.Tester),
                    EnumMapper.mapToString(Role.Staff)
                };

                var workers = dbContext.Employees
                    .Where(e => roles.Contains(e.Role))
                    .ToList();

                return workers;
            }
        }

        public string GetTeamName(string teamID)
        {
            using (var dbContext = new CompanyContext())
            {
                var team = dbContext.Teams.FirstOrDefault(t => t.ID == teamID);
                return team?.Name;
            }
        }
        public Employee GetTeamMemberDetails(TeamMember teamMember)
        {
            using (var dbContext = new CompanyContext())
            {
                var employee = dbContext.Employees.FirstOrDefault(e => e.ID == teamMember.EmployeeID);
                return employee;
            }
        }
        public List<Task> GetAssignedJobsByManagerToEmployee(string assignerID, string assigneeID)
        {
            using (var dbContext = new CompanyContext())
            {
                var tasks = dbContext.Tasks
                    .Where(t => t.Assigner == assignerID && t.Assignee == assigneeID)
                    .ToList();

                return tasks;
            }
        }
    }
}