using CMCS.Data;
using TaskOneDraft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace CMCS.Controllers
{
    public class AdminController : Controller
    {
        private readonly CMCSContext _context;

        public AdminController(CMCSContext context)
        {
            _context = context;
        }

        // Admin Dashboard - View Pending Claims
        public async Task<IActionResult> Dashboard()
        {
            var pendingClaims = await _context.Claims
                .Where(cl => cl.ApprovalStatus == "Pending")
                .Include(cl => cl.Contract)
                .Include(cl => cl.Lecturer)
                .ToListAsync();

            return View(pendingClaims);
        }

        // GET: Admin/ApproveClaim/5
        public async Task<IActionResult> ApproveClaim(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            claim.ApprovalStatus = "Approved";
            claim.AdminID = 1; // Simulate logged-in admin
            
            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        // GET: Admin/RejectClaim/5
        public async Task<IActionResult> RejectClaim(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            claim.ApprovalStatus = "Rejected";
            claim.AdminID = 1; // Simulate logged-in admin
            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        // Payment Processing
        public async Task<IActionResult> PaymentProcessing()
        {
            var approvedClaims = await _context.Claims
                .Where(cl => cl.ApprovalStatus == "Approved" && cl.Payment == null)
                .Include(cl => cl.Lecturer)
                .ToListAsync();

            return View(approvedClaims);
        }

        // POST: Admin/ProcessPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int claimID, decimal paymentAmount)
        {
            var claim = await _context.Claims.FindAsync(claimID);
            if (claim == null)
            {
                return NotFound();
            }

            var payment = new Payment
            {
                ClaimID = claimID,
                PaymentAmount = paymentAmount,
                PaymentDate = DateTime.Now
            };

            _context.Add(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PaymentProcessing));
        }
    }
}
