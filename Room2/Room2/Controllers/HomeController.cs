using Microsoft.AspNetCore.Mvc;
using Room2.Models;
using System.Collections.Generic;
using System.Linq;

namespace Room2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Static lists to hold bills and members
        private static List<Bill> Bills = new List<Bill>();
        private static List<string> Members = new List<string>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Index action to display bills, members, and total amounts
        public IActionResult Index()
        {
            // Calculate total and per-share amounts
            decimal totalAmount = Bills.Sum(b => b.Amount);
            decimal perMemberAmount = Members.Count > 0 ? totalAmount / Members.Count : 0;

            // Store in ViewBag for use in the view
            ViewBag.TotalAmount = Math.Round(totalAmount, 2);
            ViewBag.PerMemberAmount = Math.Round(perMemberAmount, 2);

            ViewBag.Bills = Bills;
            ViewBag.Members = Members;

            return View();
        }

        // Add a new bill
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

        // Clear all session data and reset lists
        [HttpPost]
        public IActionResult ClearAllData()
        {
            // Clear session and reset the static lists
            HttpContext.Session.Remove("Bills");
            HttpContext.Session.Remove("Members");
            HttpContext.Session.Remove("TotalAmount");
            HttpContext.Session.Remove("PerMemberAmount");

            Bills.Clear();
            Members.Clear();

            return RedirectToAction("Index");
        }
    }
}
