using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.Model
{
    class SuppliersModel
    {
        public int ID { get; set; }

        public DateTime data { get; set; }

        public int Pricetovar { get; set; }

        public decimal Koltovar { get; set; }

        public int TovarID { get; set; }

        public int MenegerID { get; set; }
    }
}
