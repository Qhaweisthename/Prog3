using CMCS.Data;
using TaskOneDraft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMCS.Controllers
{
    public class LecturerController : Controller
    {
        private readonly CMCSContext _context;

        public LecturerController(CMCSContext context)
        {
            _context = context;
        }

        // Lecturer Dashboard - View Contracts and Claims
        public async Task<IActionResult> Dashboard()
        {
            var lecturerID = 1; // This would be dynamically obtained from the logged-in user
            var contracts = await _context.Contracts
                .Where(c => c.LecturerID == lecturerID)
                .ToListAsync();

            var claims = await _context.Claims
                .Where(cl => cl.LecturerID == lecturerID)
                .ToListAsync();

            ViewBag.Claims = claims;

            return View(contracts);
        }

        // GET: Lecturer/SubmitClaim
        public async Task<IActionResult> SubmitClaim(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            ViewBag.Contracts = new SelectList(await _context.Contracts.ToListAsync(), "ContractID", "ContractName");
            return View();
        }

        // POST: Lecturer/SubmitClaim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaim([Bind("ContractID,HoursWorked")] Claim claim, IFormFile document)
        {
            if (ModelState.IsValid)
            {
                claim.LecturerID = 1; // Simulate logged-in lecturer
                claim.ClaimAmount = claim.HoursWorked * _context.Contracts.Find(claim.ContractID).RatePerHour;
                claim.SubmissionDate = DateTime.Now;
                claim.ApprovalStatus = "Pending";

                // Handle document upload if necessary
                if (document != null && document.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/documents", document.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(stream);
                    }
                }

                _context.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Contracts = new SelectList(await _context.Contracts.ToListAsync(), "ContractID", "ContractName");
            return View(claim);
        }
    }
}
