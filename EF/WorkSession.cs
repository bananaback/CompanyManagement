namespace CompanyManagement.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class WorkSession
    {
        [StringLength(50)]
        public string ID { get; set; }

        [StringLength(50)]
        public string EmployeeID { get; set; }

        public DateTime StartingTime { get; set; }

        public DateTime? EndingTime { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
