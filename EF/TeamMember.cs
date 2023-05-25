namespace CompanyManagement.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeamMember
    {
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [StringLength(50)]
        public string TeamID { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeID { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Team Team { get; set; }
    }
}
