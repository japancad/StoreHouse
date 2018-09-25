using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreHouse.Data;
using StoreHouse.Models;

namespace StoreHouse.Controllers.api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Export")]
    public class ExportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get(IDataTablesRequest request)
        {
            var exports = _context.Export.ToList();
            var vm = new List<Export>();
            foreach (var export in exports)
            {
                vm.Add(new Export
                {
                    ExportID = export.ExportID,
                    date = export.date,
                    ProductID = export.ProductID,
                    quantity = export.quantity,
                    PartnerID = export.PartnerID,
                    sale_price = export.sale_price
                });
            }

            var response = DataTablesResponse.Create(request, vm.Count, vm.Count, vm);
            return new DataTablesJsonResult(response, true);
        }

        [HttpPost]
        public IActionResult Post(IDataTablesRequest request)
        {
            var exports = _context.Export.ToList();
            var vm = new List<Export>();
            foreach (var export in exports)
            {
                vm.Add(new Export
                {
                    ExportID = export.ExportID,
                    date = export.date,
                    ProductID = export.ProductID,
                    quantity = export.quantity,
                    PartnerID = export.PartnerID,
                    sale_price = export.sale_price
                });
            }
            var products = _context.Product.ToList();
            var vmp = new List<Product>();
            foreach (var product in products)
            {
                vmp.Add(new Product
                {
                    ProductID = product.ProductID,
                    name = product.name,
                    description = product.description,
                    stock = product.stock,
                    currency = product.currency,
                    purchase_price = product.purchase_price,
                    sale_price = product.sale_price,
                    url = product.url
                });
            }

            var partners = _context.Partner.ToList();
            var vmpa = new List<Partner>();
            foreach (var partner in partners)
            {
                vmpa.Add(new Partner
                {
                    PartnerID = partner.PartnerID,
                    name = partner.name,
                    description = partner.description,
                    email = partner.email,
                    phone = partner.phone
                });
            }

            //szürés keresöre    
            var filteredVmpa = string.IsNullOrWhiteSpace(request?.Search.Value)
                                            ? vmpa
                                            : vmpa.Where(x => x.name.ToLowerInvariant().Contains(request.Search.Value.ToLowerInvariant()))
                                            ;
            var searchPartnerId = filteredVmpa.
            var filteredVm = string.IsNullOrWhiteSpace(request?.Search.Value)
                                            ? vmp
                                            : vmp.Where(x => x.name.ToLowerInvariant().Contains(request.Search.Value.ToLowerInvariant()))
                                            ;

            //Sorbarendezés
            //itt megnézük minél van beállitva rendezési paraméter
            var sortColumns = request.Columns
                                     .Where(c => c.Sort != null)
                                     .OrderBy(c => c.Sort.Order)
                                     .ToList();
            //rendezük az kapott paraméter alapján
            foreach (var column in sortColumns)
            {

                if (column.Sort.Direction == SortDirection.Ascending)
                {
                    if (column.Field.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                        filteredVm = filteredVm.OrderBy(c => c.name);
                    }
                    if (column.Field.Equals("Stock", StringComparison.OrdinalIgnoreCase))
                    {
                        filteredVm = filteredVm.OrderBy(c => c.stock);
                    }
                }
                else
                {
                    if (column.Field.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                        filteredVm = filteredVm.OrderByDescending(c => c.name);
                    }
                    if (column.Field.Equals("Stock", StringComparison.OrdinalIgnoreCase))
                    {
                        filteredVm = filteredVm.OrderByDescending(c => c.stock);
                    }
                }

            }

            //lapozas

            var vmPage = filteredVm.Skip(request.Start).Take(request.Length).ToList();


            var response = DataTablesResponse.Create(request, vm.Count, filteredVm.Count(), vmPage);
            return new DataTablesJsonResult(response);
        }
    }
}