using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DnevnikPrehrane.Data;
using DnevnikPrehrane.Models;

namespace DnevnikPrehrane.Controllers
{
    [Authorize]
    public class ZapisiMaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ZapisiMaseController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ZapisiMase
        public async Task<IActionResult> Index(string? userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserId = currentUser?.Id;
            var roles = await _userManager.GetRolesAsync(currentUser);

            if (roles.Contains("USER"))
            {
                userId = currentUserId;
            }
            else
            {
                userId ??= currentUserId;
            }

            var zapisi = _context.ZapisiMase
                .Include(z => z.User)
                .Where(z => z.UserId == userId)
                .OrderByDescending(z => z.Date);

            return View(await zapisi.ToListAsync());
        }

        // GET: ZapisiMase/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var zapisMase = await _context.ZapisiMase
                .Include(z => z.User)
                .FirstOrDefaultAsync(m => m.ZapisMaseId == id);

            if (zapisMase == null)
            { 
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(currentUser);

            if (roles.Contains("USER") && zapisMase.UserId != currentUser.Id)
            {
                return Forbid();
            }

            return View(zapisMase);
        }

        // GET: ZapisiMase/Create
        [Authorize(Roles = "USER")]
        public IActionResult Create()
        {
            return View(new ZapisMase { Date = DateTime.Now });
        }

        // POST: ZapisiMase/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Create([Bind("Masa,Date")] ZapisMase zapisMase)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            { 
                return Unauthorized();
            }

            zapisMase.UserId = currentUser.Id;

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                _context.Add(zapisMase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(zapisMase);
        }

        // GET: ZapisiMase/Edit/5
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Edit(int id)
        {
            var zapisMase = await _context.ZapisiMase.FindAsync(id);
            if (zapisMase == null) 
            { 
                return NotFound(); 
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (zapisMase.UserId != currentUser.Id)
            {
                return Forbid();
            }

            return View(zapisMase);
        }

        // POST: ZapisiMase/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Edit(int id, [Bind("ZapisMaseId,Masa,Date")] ZapisMase zapisMase)
        {
            if (id != zapisMase.ZapisMaseId) 
            { 
                return NotFound(); 
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var original = await _context.ZapisiMase.AsNoTracking().FirstOrDefaultAsync(z => z.ZapisMaseId == id);

            if (original == null || original.UserId != currentUser.Id) 
            { 
                return Forbid(); 
            }

            zapisMase.UserId = currentUser.Id;

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zapisMase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZapisMaseExists(zapisMase.ZapisMaseId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(zapisMase);
        }

        // GET: ZapisiMase/Delete/5
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Delete(int id)
        {
            var zapisMase = await _context.ZapisiMase
                .Include(z => z.User)
                .FirstOrDefaultAsync(m => m.ZapisMaseId == id);

            if (zapisMase == null) 
            { 
                return NotFound(); 
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (zapisMase.UserId != currentUser.Id)
            {
                return Forbid();
            }

            return View(zapisMase);
        }

        // POST: ZapisiMase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zapisMase = await _context.ZapisiMase.FindAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);
            if (zapisMase == null || zapisMase.UserId != currentUser.Id)
            {
                return Forbid();
            }

            _context.ZapisiMase.Remove(zapisMase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZapisMaseExists(int id)
        {
            return _context.ZapisiMase.Any(e => e.ZapisMaseId == id);
        }
    }
}
