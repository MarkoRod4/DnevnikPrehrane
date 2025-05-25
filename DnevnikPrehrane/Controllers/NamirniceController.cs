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

    public class NamirniceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public NamirniceController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Namirnice
        public async Task<IActionResult> Index(string? searchString, int? kategorijaId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser?.Id;
            var roles = await _userManager.GetRolesAsync(currentUser);

            // Kategorije i namirnice filtrirane prema useru

            IQueryable<Kategorija> kategorijeQuery = _context.Kategorije;

            if (roles.Contains("USER"))
            {
                kategorijeQuery = kategorijeQuery
                    .Where(k => k.UserId == null || k.UserId == userId);
            }
            else
            {
                kategorijeQuery = kategorijeQuery
                    .Where(k => k.UserId == null);
            }

            var kategorije = await kategorijeQuery
                .OrderBy(k => k.Name)
                .ToListAsync();

            ViewData["KategorijaId"] = new SelectList(kategorije, "KategorijaId", "Name", kategorijaId);

            var namirnice = _context.Namirnice
                .Include(n => n.Kategorija)
                .Include(n => n.User)
                .AsQueryable();

            if (roles.Contains("USER"))
            {
                namirnice = namirnice.Where(n => n.UserId == null || n.UserId == userId);
            }
            else
            {
                namirnice = namirnice.Where(n => n.UserId == null);
            }

            // Search params

            if (!string.IsNullOrEmpty(searchString))
            {
                namirnice = namirnice.Where(n => n.Name.Contains(searchString));
            }

            if (kategorijaId.HasValue)
            {
                namirnice = namirnice.Where(n => n.KategorijaId == kategorijaId.Value);
            }

            return View(await namirnice.ToListAsync());
        }


        // GET: Namirnice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var namirnica = await _context.Namirnice
                .Include(n => n.Kategorija)
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.NamirnicaId == id);


            if (namirnica == null)
            {
                return NotFound();
            }

            return View(namirnica);
        }

        // GET: Namirnice/Create
        public async Task<IActionResult> Create(int? kategorijaId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser?.Id;
            var roles = await _userManager.GetRolesAsync(currentUser);

            // filter mogucih kategorija
            var kategorijeQuery = _context.Kategorije.AsQueryable();

            if (roles.Contains("USER"))
            {
                kategorijeQuery = kategorijeQuery.Where(k => k.UserId == null || k.UserId == userId);
            }
            else
            {
                kategorijeQuery = kategorijeQuery.Where(k => k.UserId == null);
            }

            var kategorije = await kategorijeQuery.OrderBy(k => k.Name).ToListAsync();
            ViewData["KategorijaId"] = new SelectList(kategorije, "KategorijaId", "Name", kategorijaId);

            return View();
        }


        // POST: Namirnice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NamirnicaId,Name,KategorijaId,Kalorije,Protein,Ugljikohidrati,Masti")] Namirnica namirnica)
        {
            double sum = namirnica.Protein + namirnica.Masti + namirnica.Ugljikohidrati;
            if (sum > 100) 
            {
                ModelState.AddModelError(string.Empty, "Zbroj proteina, masti i ugljikohidrata ne smije biti veći od 100g.");
                return View(namirnica);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser?.Id;
            var roles = await _userManager.GetRolesAsync(currentUser);

            ModelState.Remove("UserId");
            ModelState.Remove("User");
            ModelState.Remove("Kategorija");

            if (ModelState.IsValid)
            {
                namirnica.UserId = roles.Contains("USER") ? userId : null;
                _context.Add(namirnica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var kategorijeQuery = _context.Kategorije.AsQueryable();
            if (roles.Contains("USER"))
            {
                kategorijeQuery = kategorijeQuery.Where(k => k.UserId == null || k.UserId == userId);
            }
            else
            {
                kategorijeQuery = kategorijeQuery.Where(k => k.UserId == null);
            }
            var kategorije = await kategorijeQuery.OrderBy(k => k.Name).ToListAsync();
            ViewData["KategorijaId"] = new SelectList(kategorije, "KategorijaId", "Name", namirnica.KategorijaId);

            return View(namirnica);
        }


        // GET: Namirnice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) 
            { 
                return NotFound(); 
            }

            var namirnica = await _context.Namirnice.FindAsync(id);
            if (namirnica == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser?.Id;
            var roles = await _userManager.GetRolesAsync(currentUser);

            if (!roles.Contains("ADMIN") && !roles.Contains("TRENER") && namirnica.UserId.IsNullOrEmpty()) 
            {
                return Unauthorized();
            }

            var kategorijeQuery = _context.Kategorije.AsQueryable();
            if (roles.Contains("USER"))
            {
                kategorijeQuery = kategorijeQuery.Where(k => k.UserId == null || k.UserId == userId);
            }
            else
            {
                kategorijeQuery = kategorijeQuery.Where(k => k.UserId == null);
            }

            var kategorije = await kategorijeQuery.OrderBy(k => k.Name).ToListAsync();
            ViewData["KategorijaId"] = new SelectList(kategorije, "KategorijaId", "Name", namirnica.KategorijaId);

            return View(namirnica);
        }


        // POST: Namirnice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NamirnicaId,Name,KategorijaId,Kalorije,Protein,Ugljikohidrati,Masti")] Namirnica namirnica)
        {
            double sum = namirnica.Protein + namirnica.Masti + namirnica.Ugljikohidrati;
            if (sum > 100)
            {
                ModelState.AddModelError(string.Empty, "Zbroj proteina, masti i ugljikohidrata ne smije biti veći od 100g.");
                return View(namirnica);
            }

            if (id != namirnica.NamirnicaId) 
            {
                return NotFound(); 
            }

            ModelState.Remove("UserId");
            ModelState.Remove("User");
            ModelState.Remove("Kategorija");


            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Namirnice.AsNoTracking().FirstOrDefaultAsync(n => n.NamirnicaId == id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    
                    namirnica.UserId = existing.UserId;

                    _context.Update(namirnica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NamirnicaExists(namirnica.NamirnicaId)) 
                    { 
                        return NotFound(); 
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser?.Id;
            var roles = await _userManager.GetRolesAsync(currentUser);

            var kategorijeQuery = _context.Kategorije.AsQueryable();
            if (roles.Contains("USER"))
            {
                kategorijeQuery = kategorijeQuery.Where(k => k.UserId == null || k.UserId == userId);
            }
            else
            {
                kategorijeQuery = kategorijeQuery.Where(k => k.UserId == null);
            }

            var kategorije = await kategorijeQuery.OrderBy(k => k.Name).ToListAsync();
            ViewData["KategorijaId"] = new SelectList(kategorije, "KategorijaId", "Name", namirnica.KategorijaId);

            return View(namirnica);
        }


        // GET: Namirnice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var namirnica = await _context.Namirnice
                .Include(n => n.Kategorija)
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.NamirnicaId == id);
            if (namirnica == null)
            {
                return NotFound();
            }



            if (namirnica.UserId != null)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var userId = currentUser?.Id;
                if (namirnica.UserId != userId)
                {
                    return Forbid();
                }
                return View(namirnica);


            }
            else 
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var userId = currentUser?.Id;
                var roles = await _userManager.GetRolesAsync(currentUser);

                if (!roles.Contains("ADMIN") && !roles.Contains("TRENER") && namirnica.UserId.IsNullOrEmpty())
                {
                    return Unauthorized();
                }
                return View(namirnica);
            }
        }

        // POST: Namirnice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var namirnica = await _context.Namirnice.FindAsync(id);
            if (namirnica == null) 
            {
                return NotFound();
            }

            if (namirnica.UserId != null)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var userId = currentUser?.Id;
                if (namirnica.UserId != userId)
                {
                    return Forbid();
                }
            }

            if (namirnica != null)
            {
                _context.Namirnice.Remove(namirnica);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NamirnicaExists(int id)
        {
            return _context.Namirnice.Any(e => e.NamirnicaId == id);
        }
    }
}
