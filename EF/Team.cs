namespace CompanyManagement.EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Team
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Team()
        {
            Tasks = new HashSet<Task>();
            TeamMembers = new HashSet<TeamMember>();
        }

        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [StringLength(50)]
        public string StageID { get; set; }

        [Required]
        [StringLength(50)]
        public string TechLeadID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Stage Stage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
