using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreHouse.Models
{
    public enum currency
    {
        Forint, Euró ,Dollár
    }

    public class Product
    {
        [Required]
        [DisplayName("Id")]
        public int ProductID { get; set; }

        [Required]
        [DisplayName("Termék")]
        public string name { get; set; }

        
        [StringLength(40)]
        [DisplayName("Megjegyzés")]
        public string description { get; set; }

        [DisplayName("Készleten")]
        public int stock { get; set; }

        [DisplayName("Vételi Ár")]
        public int purchase_price { get; set; }

        [DisplayName("Eladási Ár")]
        public int sale_price { get; set; }

        [DisplayFormat(NullDisplayText ="Nincs kiválasztva pénznem")]
        [DisplayName("Pénznem")]
        public currency? currency { get; set; }


        [UrlAttribute]
        public string url { get; set; }
    }
}
