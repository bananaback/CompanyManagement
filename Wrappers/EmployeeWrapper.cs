using CompanyManagement.EF;
using CompanyManagement.Enums;
using System;

namespace CompanyManagement.Wrappers
{
    public class EmployeeWrapper : Wrapper
    {
        private readonly Employee _employee;
        public EmployeeWrapper(Employee employee)
        {
            this._employee = employee;
        }
        public string ID => _employee.ID;
        public string Name => _employee.Name;
        public Role Role => (Role)Enum.Parse(typeof(Role), _employee.Role, true);

        public Employee Employee => _employee;
    }
}
