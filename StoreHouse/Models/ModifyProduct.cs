using StoreHouse.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreHouse.Models
{
    public class ModifyProduct
    {
        public static bool modifyProductImport(Import import,ApplicationDbContext context)
        {

            var modifyDb = import.quantity;
            
            var modifyId = import.ProductID;
            var product = context.Product
                                 .SingleOrDefault(s => s.ProductID == modifyId);
            product.stock += modifyDb;
            context.Update(product);
            context.SaveChanges();
            return true;
        }

        public static bool modifyProductExport(Export export, ApplicationDbContext context)
        {

            var modifyDb = export.quantity;
            
            var modifyId = export.ProductID;
            var product = context.Product
                                 .SingleOrDefault(s => s.ProductID == modifyId);
            product.stock -= modifyDb;
            context.Update(product);
            context.SaveChanges();
            return true;
        }

    }
}
