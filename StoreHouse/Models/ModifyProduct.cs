using StoreHouse.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreHouse.Models
{
    public class ModifyProduct
    {
        public static bool createImport(Import import,ApplicationDbContext context)
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

        public static bool createExport(Export export, ApplicationDbContext context)
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

        public static bool deleteImport(Import import, ApplicationDbContext context)
        {

            var modifyDb = import.quantity;

            var modifyId = import.ProductID;
            var product = context.Product
                                 .SingleOrDefault(s => s.ProductID == modifyId);
            product.stock -= modifyDb;
            context.Update(product);
            context.SaveChanges();
            return true;
        }

        public static bool deleteExport(Export export, ApplicationDbContext context)
        {

            var modifyDb = export.quantity;

            var modifyId = export.ProductID;
            var product = context.Product
                                 .SingleOrDefault(s => s.ProductID == modifyId);
            product.stock += modifyDb;
            context.Update(product);
            context.SaveChanges();
            return true;
        }

        public static bool editImport(Import import, int modifiesDb, ApplicationDbContext context)
        {
            var modifyDb = modifiesDb - import.quantity ;
            var modifyId = import.ProductID;
            var product = context.Product
                                 .SingleOrDefault(s => s.ProductID == modifyId);
            product.stock -= modifyDb;
            context.Update(product);
            context.SaveChanges();
            return true;

        }

        internal static bool editExport(Export export, int modifiesDb, ApplicationDbContext context)
        {
            var modifyDb = modifiesDb - export.quantity;
            var modifyId = export.ProductID;
            var product = context.Product
                                 .SingleOrDefault(s => s.ProductID == modifyId);
            product.stock += modifyDb;
            context.Update(product);
            context.SaveChanges();
            return true;
        }
    }
}
