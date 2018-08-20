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
    [Route("api/Stock")]
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// adatforrás elöállitása : kell neki paraméter, amit a Datatable küld és a visszatérési JSON a DataTables-nek tudnia kell dolgozni
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public IActionResult Get(IDataTablesRequest request)
        //{
        //    var termeklista = new List<Product>();
        //    termeklista = _context.Product.ToList();
        //    var keszletlista = new List<Stock>();
            
        //    var id = 0;
        //    foreach (var termek in termeklista)
        //    {
        //        var keszlet = new Stock();
        //        keszlet.StockId = id++;
        //        keszlet.product = _context.Product.SingleOrDefault(x => x.ProductID == termek.ProductID);
        //        var importSum = _context.Import
        //                                .Where(x => x.ProductID == termek.ProductID)
        //                                .ToArray()
        //                                .Select(xd => xd.quantity)
        //                                .Sum();
        //        var exportSum = _context.Export
        //                                .Where(x => x.ProductID == termek.ProductID)
        //                                .ToArray()
        //                                .Select(xd => xd.quantity)
        //                                .Sum();
        //        keszlet.stock = importSum - exportSum;

        //        keszletlista.Add(keszlet);

        //    }
        //    // elökészzület a visszaadásra
        //    var response = DataTablesResponse.Create(request, keszletlista.Count,keszletlista.Count,keszletlista);
        //    //var response = DataTablesResponse.Create(request, vm.Count, vm.Count(), vm);
        //    //var response = DataTablesResponse.Create(request, keszletlista.Count, keszletlista.Count, keszletlista);

        //    return new DataTablesJsonResult(response, true); 
        //}


        [HttpPost]
        public IActionResult Post(IDataTablesRequest request)
        {
            var termekLista = new List<Product>();
            termekLista = _context.Product.ToList();
            var keszletLista = new List<Stock>();

            var id = 0;
            foreach (var termek in termekLista)
            {
                var keszlet = new Stock();
                keszlet.stockId = id++;
                keszlet.name = _context.Product.SingleOrDefault(x => x.ProductID == termek.ProductID).name;
                var importSum = _context.Import
                                        .Where(x => x.ProductID == termek.ProductID)
                                        .ToArray()
                                        .Select(xd => xd.quantity)
                                        .Sum();
                var exportSum = _context.Export
                                        .Where(x => x.ProductID == termek.ProductID)
                                        .ToArray()
                                        .Select(xd => xd.quantity)
                                        .Sum();
                keszlet.stock = importSum - exportSum;

                keszletLista.Add(keszlet);

            }

            //szürés keresöre    
            var filteredKeszletLista = string.IsNullOrWhiteSpace(request?.Search.Value)
                                            ? keszletLista
                                            : keszletLista.Where(x => x.name.ToLowerInvariant().Contains(request.Search.Value.ToLowerInvariant()))
                                            ;


            //Sorbarendezés

            var sortColumns = request.Columns
                                     .Where(c => c.Sort != null)
                                     .OrderBy(c => c.Sort.Order)
                                     .ToList();

            //LINQ Expressionnal ki lehet váltani
            foreach (var column in sortColumns)
            {
                //minden oszlopnál meg kell csinálni
                if (column.Sort.Direction == SortDirection.Ascending)
                {
                    //megvizsgáljuk hog name szerepel e kis nyg betü nem számit
                    if (column.Field.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                        //TODO: c => c.product.name ez miatt nem tudja sorba rendezni
                        filteredKeszletLista = filteredKeszletLista.OrderBy(c => c.name);
                    }
                    if (column.Field.Equals("Stock", StringComparison.OrdinalIgnoreCase))
                    {
                        filteredKeszletLista = filteredKeszletLista.OrderBy(c => c.stock);
                    }
                }
                else
                {
                    if (column.Field.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                        filteredKeszletLista = filteredKeszletLista.OrderByDescending(c => c.name);
                    }
                    if (column.Field.Equals("Stock", StringComparison.OrdinalIgnoreCase))
                    {
                        filteredKeszletLista = filteredKeszletLista.OrderByDescending(c => c.stock);
                    }

                }


            }

            //Lapozas müködés
            var keszletListaPage = filteredKeszletLista.Skip(request.Start).Take(request.Length).ToList();

            // elökészzület a visszaadásra
            var response = DataTablesResponse.Create(request, keszletLista.Count, filteredKeszletLista.Count(), keszletListaPage);
            //var response = DataTablesResponse.Create(request, vm.Count, vm.Count(), vm);
            //var response = DataTablesResponse.Create(request, keszletlista.Count, keszletlista.Count, keszletlista);

            return new DataTablesJsonResult(response, true);
        }
    }
}