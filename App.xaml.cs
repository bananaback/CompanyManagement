using CompanyManagement.EF;
using CompanyManagement.Factories;
using System.Data.Entity.Migrations;
using System.Windows;

namespace CompanyManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            using (var db = new CompanyContext())
            {
                Employee mng1 = EmployeeFactory.Create("mng1", "Tin", "Binh Dinh", Enums.EmployeeStatus.Active, "123", Enums.Gender.Male, Enums.Role.Manager);
                db.Employees.AddOrUpdate(mng1);
                Employee dev1 = EmployeeFactory.Create("dev1", "Hung", "Long Xuyen", Enums.EmployeeStatus.Active, "123", Enums.Gender.Male, Enums.Role.Dev);
                db.Employees.AddOrUpdate(dev1);
                db.SaveChanges();
            }
            base.OnStartup(e);
        }
    }
}