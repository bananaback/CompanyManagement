using CompanyManagement.EF;
using System.Linq;

namespace CompanyManagement.Controllers
{
    public class AuthenticationController
    {
        public AuthenticationController() { }

        public Employee Login(string id, string password)
        {
            using (var db = new CompanyContext())
            {
                Employee foundEmployee = db.Employees.Where(x => x.ID == id).FirstOrDefault();
                return foundEmployee != null && password == foundEmployee.Password ? foundEmployee : null;
            }
        }
    }
}
