namespace Kursovaya.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SustemShopDB : DbContext
    {
        public SustemShopDB()
            : base("name=SustemShopDB")
        {
        }

        public virtual DbSet<Managers> Managers { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Tovar> Tovar { get; set; }
    }
}
