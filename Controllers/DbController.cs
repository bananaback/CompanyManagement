using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.States;
using CuoiKi.States;
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

            db.SaveChanges();
        }

        public void Delete<T>(T entry)
        {
            if (entry is Project)
            {
                db.Projects.Remove(entry as Project);
            }
            else if (entry is Stage)
            {
                db.Stages.Remove(entry as Stage);
            }
            else if (entry is Team)
            {
                db.Teams.Remove(entry as Team);
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

    }
}