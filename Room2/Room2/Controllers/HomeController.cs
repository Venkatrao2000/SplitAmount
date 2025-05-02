using Microsoft.AspNetCore.Mvc;
using Room2.Models;
using System.Collections.Generic;
using System.Linq;

namespace Room2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static List<Bill> Bills = new List<Bill>();
        private static List<string> Members = new List<string>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Calculate the total and per-share amounts
            decimal totalAmount = Bills.Sum(b => b.Amount);
            decimal perMemberAmount = Members.Count > 0 ? totalAmount / Members.Count : 0;

            // Round to two decimal places
            ViewBag.TotalAmount = Math.Round(totalAmount, 2);
            ViewBag.PerMemberAmount = Math.Round(perMemberAmount, 2);

            ViewBag.Bills = Bills;
            ViewBag.Members = Members;

            return View();
        }

        // Add a bill only
        [HttpPost]
        public IActionResult AddBill(Bill newBill)
        {
            if (newBill != null && newBill.Amount > 0)
            {
                Bills.Add(newBill);
            }

            return RedirectToAction("Index");
        }

        // Set members once
        [HttpPost]
        public IActionResult SetMembers(string memberNames)
        {
            if (!string.IsNullOrEmpty(memberNames))
            {
                Members = memberNames.Split(',')
                                     .Select(m => m.Trim())
                                     .Where(m => !string.IsNullOrWhiteSpace(m))
                                     .ToList();
            }

            return RedirectToAction("Index");
        }
    }
}
