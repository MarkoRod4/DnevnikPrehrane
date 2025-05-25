using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DnevnikPrehrane.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace DnevnikPrehrane.Controllers
{
    [Authorize(Roles = "ADMIN,TRENER")]
    public class KorisniciController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public KorisniciController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: Korisnici
        public async Task<IActionResult> Index()
        {
            var allUsers = _userManager.Users.ToList();
            var userList = new List<IdentityUser>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("USER"))
                {
                    userList.Add(user);
                }
            }

            return View(userList);
        }

        // GET: Korisnici/Details/id
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }
    }
}
