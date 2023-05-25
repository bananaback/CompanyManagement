namespace CompanyManagement.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Task
    {
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Assignee { get; set; }

        [Required]
        [StringLength(50)]
        public string Assigner { get; set; }

        [Required]
        [StringLength(50)]
        public string TeamID { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public DateTime StartingTime { get; set; }

        public DateTime EndingTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Employee Employee1 { get; set; }

        public virtual Team Team { get; set; }
    }
}
