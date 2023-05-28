using CompanyManagement.EF;
using CompanyManagement.Enums;
using System;

namespace CompanyManagement.Factories
{
    public class EmployeeFactory
    {
        public static Employee Create(string ID, string Name, string Address, EmployeeStatus Status, string Password, Gender Gender, Role Role)
        {
            var employee = new Employee();
            employee.ID = ID;
            employee.Name = Name;
            employee.Address = Address;
            employee.Birth = DateTime.Now;
            employee.EmployeeStatus = EnumMapper.mapToString(Status);
            employee.Password = Password;
            employee.Gender = EnumMapper.mapToString(Gender);
            employee.Role = EnumMapper.mapToString(Role);
            return employee;
        }
    }
}
