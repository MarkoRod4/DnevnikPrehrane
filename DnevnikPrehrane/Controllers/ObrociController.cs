using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DnevnikPrehrane.Data;
using DnevnikPrehrane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DnevnikPrehrane.Controllers
{
    [Authorize]

    public class ObrociController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ObrociController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Obroci
        public async Task<IActionResult> Index(DateTime? date, string? userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserId = currentUser?.Id;
            var roles = await _userManager.GetRolesAsync(currentUser);

            DateTime targetDate = date?.Date ?? DateTime.Today;

            if (roles.Contains("USER"))
            {
                userId = currentUserId;
            }
            else
            {
                userId ??= currentUserId;
            }

            var obrociQuery = _context.Obroci
                .Include(o => o.User)
                .Where(o => EF.Functions.DateDiffDay(o.Date, targetDate) == 0);

            if (!string.IsNullOrEmpty(userId))
            {
                obrociQuery = obrociQuery.Where(o => o.UserId == userId);
            }

            var obroci = await obrociQuery.ToListAsync();
            return View(obroci);
        }


        // GET: Obroci/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var obrok = await _context.Obroci
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.ObrokId == id);

            if (obrok == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(currentUser);

            if (roles.Contains("USER") && obrok.UserId != currentUser.Id)
            {
                return Forbid();
            }

            return View(obrok);
        }


        // GET: Obroci/Create
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var namirnice = await _context.Namirnice
                .Where(n => n.UserId == null || n.UserId == currentUser.Id)
                .OrderBy(n => n.Name)
                .ToListAsync();

            ViewData["NamirnicaId"] = new SelectList(namirnice, "NamirnicaId", "Name");
            ViewBag.Namirnice = namirnice;

            return View(new Obrok { Date = DateTime.Now, Količina = 100 });
        }


        // POST: Obroci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Create([Bind("ObrokId,Date,ImeNamirnice,Količina,Kalorije,Protein,Ugljikohidrati,Masti")] Obrok obrok)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            obrok.UserId = currentUser.Id;

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                _context.Add(obrok);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(obrok);
        }


        // GET: Obroci/Edit/5
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var obrok = await _context.Obroci.FindAsync(id);
            if (obrok == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (obrok.UserId != currentUser.Id)
                return Forbid();

            var namirnice = await _context.Namirnice
                .Where(n => n.UserId == null || n.UserId == currentUser.Id)
                .OrderBy(n => n.Name)
                .ToListAsync();

            ViewData["NamirnicaId"] = new SelectList(namirnice, "NamirnicaId", "Name");
            ViewBag.Namirnice = namirnice;

            return View(obrok);
        }

        // POST: Obroci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Edit(int id, [Bind("ObrokId,Date,ImeNamirnice,Količina,Kalorije,Protein,Ugljikohidrati,Masti")] Obrok obrok)
        {
            if (id != obrok.ObrokId) 
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var originalObrok = await _context.Obroci.AsNoTracking().FirstOrDefaultAsync(o => o.ObrokId == id);

            if (originalObrok == null || originalObrok.UserId != currentUser.Id) 
            {
                return Forbid();
            }

            obrok.UserId = currentUser.Id;

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obrok);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObrokExists(obrok.ObrokId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(obrok);
        }

        // GET: Obroci/Delete/5
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) 
            {
                return NotFound();

            }

            var obrok = await _context.Obroci
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.ObrokId == id);

            var currentUser = await _userManager.GetUserAsync(User);
            if (obrok == null || obrok.UserId != currentUser.Id) 
            {
                return Forbid();
            }

            return View(obrok);
        }

        // POST: Obroci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obrok = await _context.Obroci.FindAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);
            if (obrok == null || obrok.UserId != currentUser.Id)
                return Forbid();

            _context.Obroci.Remove(obrok);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ObrokExists(int id)
        {
            return _context.Obroci.Any(e => e.ObrokId == id);
        }
    }
}
