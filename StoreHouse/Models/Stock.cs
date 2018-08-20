using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace StoreHouse.Models
{
    public class Stock
    {

        public int stockId { get; set; }

        public string name { get; set; }

        [DisplayName("Készleten")]
        public int stock { get; set; }

    }
}
