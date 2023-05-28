namespace CompanyManagement.EF
{
    using System.ComponentModel.DataAnnotations;

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
