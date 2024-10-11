using CMCS.Data;
using TaskOneDraft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMCS.Controllers
{
    public class PaymentController : Controller
    {
        private readonly CMCSContext _context;

        public PaymentController(CMCSContext context)
        {
            _context = context;
        }

        // List Payments
        public async Task<IActionResult> Index()
        {
            var payments = await _context.Payments
                .Include(p => p.Claim)
                .ThenInclude(c => c.Lecturer)
                .ToListAsync();

            return View(payments);
        }
    }
}
