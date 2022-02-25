namespace Kursovaya.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Suppliers")]
    public partial class Suppliers
    {
        [Key]
        [Column("Suppliers")]
        public int ID { get; set; }

        public DateTime data { get; set; }

        public int Pricetovar { get; set; }

        public decimal Koltovar { get; set; }

        public int TovarID { get; set; }

        public int MenegerID { get; set; }

        public virtual Managers Managers { get; set; }

        public virtual Tovar Tovar { get; set; }
    }
}
