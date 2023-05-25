namespace CompanyManagement.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Salary")]
    public partial class Salary
    {
        [StringLength(50)]
        public string ID { get; set; }

        public int basicPay { get; set; }

        public int kpiCost { get; set; }

        public int leaveCost { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
