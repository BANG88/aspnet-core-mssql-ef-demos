using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using aspnet_core_mssql_ef_demos.Models;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_core_mssql_ef_demos.Controllers
{
    public class HomeController : Controller
    {
        private readonly NorthWindContext _context;

        public HomeController(NorthWindContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Take(5).AsNoTracking().ToListAsync();

            return View(employees);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            var orders = _context.Orders.Take(100).Select(o => new Orders { OrderId = o.OrderId, ShipName = o.ShipName }).ToList();
            return View(orders);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
