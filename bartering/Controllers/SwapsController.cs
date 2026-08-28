
using bartering.Data;
using bartering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static bartering.Models.Enum;

public class SwapsController : Controller
{
    private readonly ApplicationDbContext _context;

    public SwapsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: SWAPOFFERS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.SwapOffers.ToListAsync());
    }

    // GET: SWAPOFFERS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var swapoffer = await _context.SwapOffers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (swapoffer == null)
        {
            return NotFound();
        }

        return View(swapoffer);
    }

    // GET: SWAPOFFERS/Create
    public async Task<IActionResult> Create(int requestedItemId)
    {
        var requestedItem = await _context.Items
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(i => i.Id == requestedItemId);

        if (requestedItem == null)
        {
            return NotFound();
        }

        var swapOffer = new SwapOffer
        {
            RequestedItemId = requestedItem.Id,
            ToUserId = requestedItem.OwnerId
        };

        return View(swapOffer);
    }



    //public IActionResult Create()
    //{
    //     return View();
    //}

    // POST: SWAPOFFERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SwapOffer swapoffer)
    {
        var fromUserId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(fromUserId))
        {
            return Unauthorized();
        }

        var requestedItem = await _context.Items
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(i => i.Id == swapoffer.RequestedItemId);

        if (requestedItem == null)
        {
            return NotFound();
        }

        if (requestedItem.OwnerId == fromUserId)
        {
            ModelState.AddModelError("", "You cannot propose a swap for your own item.");
            return View(swapoffer);
        }

        swapoffer.FromUserId = fromUserId;
        swapoffer.ToUserId = requestedItem.OwnerId;
        swapoffer.Status = SwapOfferStatus.Pending;
        swapoffer.CreatedAt = DateTime.UtcNow;

        if (!ModelState.IsValid)
        {
            return View(swapoffer);
        }

        _context.SwapOffers.Add(swapoffer);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    //public async Task<IActionResult> Create([Bind("Id,FromUserId,FromUser,ToUserId,ToUser,OfferedItemId,OfferedItem,RequestedItemId,RequestedItem,Status,Message,CreatedAt,RespondedAt,CompletedAt")] SwapOffer swapoffer)
    //{
    //  if (ModelState.IsValid)
    //{
    //  _context.Add(swapoffer);
    //await _context.SaveChangesAsync();
    //return RedirectToAction(nameof(Index));
    //}
    //return View(swapoffer);
    // }

    // GET: SWAPOFFERS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var swapoffer = await _context.SwapOffers.FindAsync(id);
        if (swapoffer == null)
        {
            return NotFound();
        }
        return View(swapoffer);
    }

    // POST: SWAPOFFERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,FromUserId,FromUser,ToUserId,ToUser,OfferedItemId,OfferedItem,RequestedItemId,RequestedItem,Status,Message,CreatedAt,RespondedAt,CompletedAt")] SwapOffer swapoffer)
    {
        if (id != swapoffer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(swapoffer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SwapOfferExists(swapoffer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(swapoffer);
    }

    // GET: SWAPOFFERS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var swapoffer = await _context.SwapOffers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (swapoffer == null)
        {
            return NotFound();
        }

        return View(swapoffer);
    }

    // POST: SWAPOFFERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var swapoffer = await _context.SwapOffers.FindAsync(id);
        if (swapoffer != null)
        {
            _context.SwapOffers.Remove(swapoffer);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SwapOfferExists(int? id)
    {
        return _context.SwapOffers.Any(e => e.Id == id);
    }
}
