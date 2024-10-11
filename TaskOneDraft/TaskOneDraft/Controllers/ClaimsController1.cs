using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using TaskOneDraft.Areas.Identity.Data;
using TaskOneDraft.Models;

namespace TaskOneDraft.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClaimsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this._webHostEnvironment = webHostEnvironment;
        }

        // Display the form to make a claim (GET request)
        [HttpGet]
        public IActionResult Claims()
        {
            return View(); // Ensure you have a Claims.cshtml view for this action
        }

        // Handle the form submission to make a claim (POST request)
        [HttpPost]
        public async Task<IActionResult> Claims(Claims claims)
        {
            if (ModelState.IsValid)
            {
                if (claims.SupportingDocuments != null && claims.SupportingDocuments.Any())
                {
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    foreach (var file in claims.SupportingDocuments)
                    {
                        if (file.Length > 0)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string filePath = Path.Combine(uploadPath, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                    }
                }

                // Calculate total hours
                claims.TotalAmount = claims.HoursWorked * claims.RatePerHour;
                _context.Add(claims);
                await _context.SaveChangesAsync();
                ViewBag.Message = "Your claim has been successfully submitted.";
                return RedirectToAction(nameof(claims));
              //  return View(claims);
            }
            return View(claims);
        }

        // Display the list of claims (GET request)
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var claims = await _context.Claims.ToListAsync();
            return View(claims); // Ensure you have a List.cshtml view for this action
        }

        // Approve a claim
        public IActionResult Approve(int id)
        {
            var claim = _context.Claims.Find(id);
            if (claim == null)
            {
                TempData["Message"] = "Claim not found";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }

            claim.Status = "Approved";
            _context.SaveChanges();
            TempData["Message"] = "Your claim has been approved";
            TempData["MessageType"] = "success";
            return RedirectToAction("List");
        }

        // Reject a claim
        public IActionResult Reject(int id)
        {
            var claim = _context.Claims.Find(id);
            if (claim == null)
            {
                TempData["Message"] = "Claim not found";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }

            claim.Status = "Rejected";
            _context.SaveChanges();
            TempData["Message"] = "Your claim has been rejected - contact HR";
            TempData["MessageType"] = "error";
            return RedirectToAction("List");
        }
    }
}
