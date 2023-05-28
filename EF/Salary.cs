namespace CompanyManagement.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
