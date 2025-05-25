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
using Microsoft.IdentityModel.Tokens;

namespace DnevnikPrehrane.Controllers
{
    [Authorize]

    public class KategorijeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public KategorijeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Kategorije
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

            var kategorije = _context.Kategorije
                .Include(k => k.User)
                .AsQueryable();

            if (userRoles.Contains("USER"))
            {
                kategorije = kategorije
                    .Where(k => k.UserId == null || k.UserId == userId);
            }
            else
            {
                kategorije = kategorije
                    .Where(k => k.UserId == null);
            }

            var kategorijeList = await kategorije
                .OrderBy(k => k.Name)
                .ToListAsync();

            return View(kategorijeList);
        }

        // GET: Kategorije/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorije
                .Include(k => k.User)
                .Include(k => k.Namirnice)
                .FirstOrDefaultAsync(m => m.KategorijaId == id);

            if (kategorija == null)
            {
                return NotFound();
            }

            if (kategorija.UserId != null) 
            {
                var currentUserId = _userManager.GetUserId(User);
                if (kategorija.UserId != currentUserId)
                {
                    return Forbid();
                }
            }

            return View(kategorija);
        }

        // GET: Kategorije/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kategorije/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KategorijaId,Name")] Kategorija kategorija)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            if (!userRoles.Contains("ADMIN") && !userRoles.Contains("TRENER"))
            {
                kategorija.UserId = currentUser?.Id;
            }
            else
            {
                kategorija.UserId = null;
            }

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                _context.Add(kategorija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(kategorija);
        }

        // GET: Kategorije/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorije
                .Include(k => k.Namirnice)
                .FirstOrDefaultAsync(k => k.KategorijaId == id);

            if (kategorija == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            if (kategorija.UserId != null)
            {
                // Privatne kategorije
                if (kategorija.UserId != currentUser?.Id)
                {
                    return Forbid();
                }
            }
            else
            {
                // Globalne kategorije uredjuju admin i treneri
                if (!(userRoles.Contains("ADMIN") || userRoles.Contains("TRENER")))
                {
                    return Forbid();
                }
            }

            return View(kategorija);
        }


        // POST: Kategorije/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KategorijaId,Name")] Kategorija kategorija)
        {
            if (id != kategorija.KategorijaId)
            {
                return NotFound();
            }

            var existing = await _context.Kategorije.AsNoTracking().FirstOrDefaultAsync(k => k.KategorijaId == id);
            if (existing == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            // Provjera pristupa
            if (existing.UserId != null)
            {
                if (existing.UserId != currentUser?.Id)
                {
                    return Forbid();
                }

                kategorija.UserId = existing.UserId; // Samo vlasnik moze editati
            }
            else
            {
                if (!(userRoles.Contains("ADMIN") || userRoles.Contains("TRENER")))
                {
                    return Forbid();
                }

                kategorija.UserId = null; // Globalne kategorije
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategorija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategorijaExists(kategorija.KategorijaId))
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

            return View(kategorija);
        }

        // GET: Kategorije/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorije
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.KategorijaId == id);

            if (kategorija == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            if (kategorija.UserId != null)
            {
                // Privatna kategorija
                if (kategorija.UserId != currentUser?.Id)
                {
                    return Forbid();
                }
            }
            else
            {
                // Globalna kategorija
                if (!(userRoles.Contains("ADMIN") || userRoles.Contains("TRENER")))
                {
                    return Forbid();
                }
            }

            return View(kategorija);
        }


        // POST: Kategorije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategorija = await _context.Kategorije
                .Include(k => k.Namirnice)
                .FirstOrDefaultAsync(k => k.KategorijaId == id);

            if (kategorija == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            if (kategorija.UserId != null)
            {
                if (kategorija.UserId != currentUser?.Id)
                {
                    return Forbid();
                }
            }
            else
            {
                if (!(userRoles.Contains("ADMIN") || userRoles.Contains("TRENER")))
                {
                    return Forbid();
                }
            }

            // Kategorija ne smije sadrzavati namirnice
            if (kategorija.Namirnice != null && kategorija.Namirnice.Any())
            {
                ModelState.AddModelError(string.Empty, "Kategorija sadrzi namirnice!");
                return View(kategorija);
            }

            _context.Kategorije.Remove(kategorija);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool KategorijaExists(int id)
        {
            return _context.Kategorije.Any(e => e.KategorijaId == id);
        }
    }
}
