
using bartering.Data;
using bartering.Models;
using bartering.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static bartering.Models.Enum;

public class ItemsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ItemsController(ApplicationDbContext context)
    {
        _context = context;
    }


    // GET: ITEMS
    public async Task<IActionResult> Index()
    {
        var items = await _context.Items
            .Include(i => i.Owner)
            .ToListAsync();

        var viewModel = new BrowseViewModel
        {
            Items = items
        };

        return View(viewModel);
    }



    // GET: ITEMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var item = await _context.Items
    .Include(i => i.Owner)
    .FirstOrDefaultAsync(m => m.Id == id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    // GET: ITEMS/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new ItemFormViewModel());
    }

    //public IActionResult Create()
    //{
    //  return View();
    //}

    // POST: ITEMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ItemFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var item = new Item
        {
            Title = viewModel.Title,
            Description = viewModel.Description,
            Category = viewModel.Category,
            Condition = viewModel.Condition,
            OwnerId = userId,
            CreatedAt = DateTime.UtcNow,
            Status = ItemStatus.Available
        };

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: ITEMS/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.Items.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        var viewModel = new ItemFormViewModel
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Category = item.Category,
            Condition = item.Condition,
            ExistingImageUrl = item.ImageUrl
        };

        return View(viewModel);  
    }

    //public async Task<IActionResult> Edit(int? id)
    //{
    //if (id == null)
    // {
    //   return NotFound();
    // }

    //var item = await _context.Items.FindAsync(id);
    //if (item == null)
    // {
    //    return NotFound();
    // }
    // return View(item);
    //}

    // POST: ITEMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ItemFormViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel); 
        }

        var item = await _context.Items.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        item.Title = viewModel.Title;
        item.Description = viewModel.Description;
        item.Category = viewModel.Category;
        item.Condition = viewModel.Condition;

        // Only replace the image if a new image was uploaded
        if (viewModel.Image != null && viewModel.Image.Length > 0)
        {
            // Add your image-saving code here
            // Example:
            // item.ImageUrl = ...
        }

        try
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ItemExists(item.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    //public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Description,Category,Condition,Status,ImageUrl,CreatedAt,OwnerId,Owner")] Item item)
    // {
    //   if (id != item.Id)
    //  {
    //     return NotFound();
    //  }

    //   if (ModelState.IsValid)
    //  {
    //     try
    //      {
    //          _context.Update(item);
    //  await _context.SaveChangesAsync();
    //      }
    //     catch (DbUpdateConcurrencyException)
    //   {
    //       if (!ItemExists(item.Id))
    //      {
    //  return NotFound();
    //     }
    //     else
    //     {
    //        throw;
    //    }
    //  }
    //  return RedirectToAction(nameof(Index));
    // }
    //  return View(item);
    //  }

    // GET: ITEMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var item = await _context.Items
            .FirstOrDefaultAsync(m => m.Id == id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    // POST: ITEMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item != null)
        {
            _context.Items.Remove(item);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ItemExists(int? id)
    {
        return _context.Items.Any(e => e.Id == id);
    }
}
