using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreHouse.Models
{
    public class Partner
    {
        [Required]
        public int PartnerID { get; set; }

        
        [StringLength(40)]
        public string name { get; set; }

        
        [StringLength(100)]
        public string description { get; set; }

        [StringLength(40)]
        [EmailAddressAttribute]
        public string email { get; set; }

       
        [StringLength(15)]
        [PhoneAttribute]
        public string phone { get; set; }

    }
}
