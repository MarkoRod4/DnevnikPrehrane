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
using System.Security.Claims;

namespace DnevnikPrehrane.Controllers
{
    [Authorize]
    public class BiljeskeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BiljeskeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Biljeske
        public async Task<IActionResult> Index(string? userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserId = currentUser?.Id;
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            var biljeske = _context.Biljeske
                .Include(b => b.User)
                .AsQueryable();

            if (userRoles.Contains("ADMIN") || userRoles.Contains("TRENER"))
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    biljeske = biljeske.Where(b => b.UserId == userId);
                }

                biljeske = biljeske.OrderByDescending(b => b.Date);
            }
            else
            {
                biljeske = biljeske
                    .Where(b => b.UserId == currentUserId)
                    .OrderByDescending(b => b.Date);
            }

            return View(await biljeske.ToListAsync());
        }


        // GET: Biljeske/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biljeska = await _context.Biljeske
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BiljeskaId == id);
            if (biljeska == null)
            {
                return NotFound();
            }

            return View(biljeska);
        }

        // GET: Biljeske/Create
        [Authorize(Roles = "USER")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Biljeske/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Create([Bind("BiljeskaId,Tekst")] Biljeska biljeska)
        {
            biljeska.UserId = _userManager.GetUserId(User);
            biljeska.Date = DateTime.Today;

            ModelState.Remove("UserId");
            ModelState.Remove("User");


            if (ModelState.IsValid)
            {
                _context.Add(biljeska);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(biljeska);
        }


        // GET: Biljeske/Edit/5
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biljeska = await _context.Biljeske.FindAsync(id);
            if (biljeska == null)
            {
                return NotFound();
            }

            // Provjera je li user vlasnik biljeske
            var currentUserId = _userManager.GetUserId(User);
            if (biljeska.UserId != currentUserId) 
            {
                return Forbid();
            }

            return View(biljeska);
        }

        // POST: Biljeske/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Edit(int id, [Bind("BiljeskaId,Tekst")] Biljeska biljeskaFromForm)
        {
            if (!BiljeskaExists(id))
                return NotFound();

            var biljeska = await _context.Biljeske.AsNoTracking().FirstOrDefaultAsync(b => b.BiljeskaId == id);
            if (biljeska == null)
                return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            if (biljeska.UserId != currentUserId)
                return Forbid();

            // Moguce mjenjanje samo teksta
            biljeska.Tekst = biljeskaFromForm.Tekst;

            try
            {
                _context.Update(biljeska);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BiljeskaExists(id))
                    return NotFound();
                throw;
            }
        }

        // GET: Biljeske/Delete/5
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biljeska = await _context.Biljeske
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BiljeskaId == id);

            if (biljeska == null)
            {
                return NotFound();
            }

            // Provjera je li user vlasnik biljeske
            var currentUserId = _userManager.GetUserId(User);
            if (biljeska.UserId != currentUserId)
                return Forbid();

            return View(biljeska);
        }

        // POST: Biljeske/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var biljeska = await _context.Biljeske.FindAsync(id);

            // Provjera je li user vlasnik biljeske
            var currentUserId = _userManager.GetUserId(User);
            if (biljeska.UserId != currentUserId)
                return Forbid();

            if (biljeska != null)
            {
                _context.Biljeske.Remove(biljeska);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BiljeskaExists(int id)
        {
            return _context.Biljeske.Any(e => e.BiljeskaId == id);
        }
    }
}
