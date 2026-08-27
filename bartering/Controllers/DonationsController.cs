
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bartering.Models;
using bartering.Data;

public class DonationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DonationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Donations/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Donations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Donation donation)
    {
        if (ModelState.IsValid)
        {
            donation.DonationDate = DateTime.UtcNow;

            if (donation.IsAnonymous)
            {
                donation.DonorName = "Anonymous";
            }

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Confirmation),
                new { id = donation.Id });
        }

        return View(donation);
    }

    // GET: Donations/Confirmation/5
    [HttpGet]
    public async Task<IActionResult> Confirmation(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donation = await _context.Donations
            .FirstOrDefaultAsync(d => d.Id == id);

        if (donation == null)
        {
            return NotFound();
        }

        return View(donation);
    }

    // GET: Donations
    public async Task<IActionResult> Index()
    {
        var donations = await _context.Donations
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync();

        return View(donations);
    }

    // GET: Donations/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donation = await _context.Donations
            .FirstOrDefaultAsync(d => d.Id == id);

        if (donation == null)
        {
            return NotFound();
        }

        return View(donation);
    }

    // POST: Donations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var donation = await _context.Donations.FindAsync(id);

        if (donation != null)
        {
            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
