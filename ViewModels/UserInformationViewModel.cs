using CompanyManagement.EF;
using CompanyManagement.Enums;
using CompanyManagement.States;
using CUOIKI_EF.Controllers;
using System.Collections.Generic;

namespace CompanyManagement.ViewModels
{
    public class UserInformationViewModel : ViewModelBase
    {
        private readonly DbController db;
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserBirth { get; set; }
        public string UserRole { get; set; }
        public string UserGender { get; set; }
        public string UserStatus { get; set; }
        public UserInformationViewModel()
        {
            db = DbController.Instance;
            Employee e = db.GetByID<Employee>(LoginInfoState.Id);
            UserId = e.ID;
            UserName = e.Name;
            UserAddress = e.Address;
            UserBirth = e.Birth.ToShortDateString();
            UserRole = e.Role.ToString();
            UserGender = e.Gender.ToString();
            UserStatus = e.EmployeeStatus.ToString();
        }

        private List<string> genderList = new List<string>()
        {
            nameof(Gender.Male), nameof(Gender.Female)
        };

        public List<string> GenderList
        {
            get => genderList;
            set => genderList = value;
        }

        private List<string> roleList = new List<string>()
        {
            nameof(Role.Dev), nameof(Role.Designer), nameof(Role.Tester), nameof(Role.TechLead), nameof(Role.Manager), nameof(Role.Hr), nameof(Role.Staff)
        };

        public List<string> RoleList
        {
            get => roleList;
            set => roleList = value;
        }
    }
}
