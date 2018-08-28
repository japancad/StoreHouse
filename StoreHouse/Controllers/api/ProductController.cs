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
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get(IDataTablesRequest request)
        {
            var products = _context.Product.ToList();
            var vm = new List<Product>();
            foreach (var product in products)
            {
                vm.Add(new Product
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

            var response = DataTablesResponse.Create(request, vm.Count, vm.Count, vm);
            return new DataTablesJsonResult(response, true);
        }

        [HttpPost]
        public IActionResult Post(IDataTablesRequest request)
        {
            var products = _context.Product.ToList();
            var vm = new List<Product>();
            foreach (var product in products)
            {
                vm.Add(new Product
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
            //szürés keresöre    
            var filteredVm = string.IsNullOrWhiteSpace(request?.Search.Value)
                                            ? vm
                                            : vm.Where(x => x.name.ToLowerInvariant().Contains(request.Search.Value.ToLowerInvariant()))
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