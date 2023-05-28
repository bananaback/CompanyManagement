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
                Employee employee = EmployeeFactory.Create("mng1", "Tin", "Binh Dinh", Enums.EmployeeStatus.Active, "123", Enums.Gender.Male, Enums.Role.Manager);
                db.Employees.AddOrUpdate(employee);
                db.SaveChanges();
            }
            base.OnStartup(e);
        }
    }
}