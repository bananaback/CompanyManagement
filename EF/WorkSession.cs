namespace CompanyManagement.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
