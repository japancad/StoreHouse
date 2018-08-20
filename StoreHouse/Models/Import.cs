using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreHouse.Models
{
    public class Import
    {
        public int ImportID { get; set; }

        [DataType(DataType.Date)]
        //[DefaultValue(type : typeof(DateTime), value: DateTime.Now.ToString("yyyy-MM-dd")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }

        public int ProductID { get; set; }

        public int quantity { get; set; }

        public int PartnerID { get; set; }

        public int sale_price { get; set; }

        
        public Product Product { get; set; }
        public Partner Partner { get; set; }
    }
}
