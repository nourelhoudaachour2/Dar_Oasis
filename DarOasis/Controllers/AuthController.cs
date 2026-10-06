using DarOasis.Data;
using DarOasis.Models;
using DarOasis.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DarOasis.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Login
        public IActionResult Login()
        {
            // lwken utilisateur déjà connecté
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login model)
        {
            // ytfa9ed el formulaire
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // ylwj utilisateur
            var user = _context.Utilisateurs
                .FirstOrDefault(u => u.Email == model.Email);

            // Vérification email + mot de passe
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.MotDePasse, user.MotDePasse))
            {
                ModelState.AddModelError("", "Email ou mot de passe incorrect.");
                return View(model);
            }

            // Stockage session
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserNom", user.Nom);
            HttpContext.Session.SetString("UserRole", user.Role);

            // Redirection selon rôle
            if (user.Role == "gerant")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            if (user.Role == "receptionniste")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            // Sécurité si rôle inconnu
            return RedirectToAction("Login", "Auth");
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: /Auth/Seed
        public IActionResult Seed()
        {
            if (_context.Utilisateurs.Any())
            {
                return Content("Utilisateurs déjà existants.");
            }

            _context.Utilisateurs.AddRange(
                new Utilisateur
                {
                    Nom = "Nour el houda Achour",
                    Email = "nour@daroasis.com",
                    MotDePasse = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Role = "gerant"
                }
            );

            _context.SaveChanges();

            return Content(" Utilisateurs créés avec succès !");
        }
    }
}