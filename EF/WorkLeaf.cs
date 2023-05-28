namespace CompanyManagement.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class WorkLeaf
    {
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeID { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [Required]
        [StringLength(255)]
        public string ReasonOfLeave { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
