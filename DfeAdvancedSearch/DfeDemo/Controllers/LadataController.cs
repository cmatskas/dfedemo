using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DfeDemo.Data;
using Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DfeDemo.Models;

namespace DfeDemo.Controllers
{
	[Authorize]
    public class LadataController : Controller
    {
        private readonly DataDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public LadataController(DataDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
			_userManager = userManager;
        }

        // GET: Ladata
        public async Task<IActionResult> Index()
        {
			var user = await _userManager.GetUserAsync(HttpContext.User);
			return View(await _context.Ladata.ToListAsync());
        }

        // GET: Ladata/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ladata = await _context.Ladata
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ladata == null)
            {
                return NotFound();
            }

            return View(ladata);
        }

        // GET: Ladata/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ladata/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Ladata ladata)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ladata);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ladata);
        }

        // GET: Ladata/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ladata = await _context.Ladata.SingleOrDefaultAsync(m => m.Id == id);
            if (ladata == null)
            {
                return NotFound();
            }
            return View(ladata);
        }

        // POST: Ladata/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Ladata ladata)
        {
            if (id != ladata.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ladata);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LadataExists(ladata.Id))
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
            return View(ladata);
        }

        // GET: Ladata/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ladata = await _context.Ladata
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ladata == null)
            {
                return NotFound();
            }

            return View(ladata);
        }

        // POST: Ladata/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ladata = await _context.Ladata.SingleOrDefaultAsync(m => m.Id == id);
            _context.Ladata.Remove(ladata);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LadataExists(int id)
        {
            return _context.Ladata.Any(e => e.Id == id);
        }
    }
}
