using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DarOasis.Data;
using DarOasis.Models;

namespace DarOasis.Controllers
{
    public class EmployesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EmployesController(
            AppDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employes.ToListAsync());
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var employe = await _context.Employes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employe == null)
                return NotFound();

            return View(employe);
        }

        // CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Nom,Prenom,Telephone,Email,Poste,ImagePath,DateNaissance,DateEmbauche,Adresse,Experience,Salaire")]
            Employe employe,
            IFormFile? imageFile,
            bool CreerCompte)
        {
            if (ModelState.IsValid)
            {
                // Upload image
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(
                        _environment.WebRootPath,
                        "images",
                        "employes");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName =
                        Guid.NewGuid().ToString()
                        + Path.GetExtension(imageFile.FileName);

                    string filePath = Path.Combine(
                        uploadsFolder,
                        uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    employe.ImagePath =
                        "/images/employes/" + uniqueFileName;
                }
                else
                {
                    employe.ImagePath =
                        "/images/employes/default.png";
                }

                _context.Add(employe);

                await _context.SaveChangesAsync();

                // Création compte réceptionniste
                if (CreerCompte && employe.Poste == "Réceptionniste")
                {
                    bool utilisateurExiste =
                        await _context.Utilisateurs
                        .AnyAsync(u => u.Email == employe.Email);

                    if (!utilisateurExiste)
                    {
                        var utilisateur = new Utilisateur
                        {
                            Nom = employe.Nom + " " + employe.Prenom,
                            Email = employe.Email,
                            MotDePasse = BCrypt.Net.BCrypt.HashPassword("123456"),
                            Role = "receptionniste",
                            EmployeId = employe.Id
                        };

                        _context.Utilisateurs.Add(utilisateur);

                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(employe);
        }

        // EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var employe = await _context.Employes.FindAsync(id);

            if (employe == null)
                return NotFound();

            return View(employe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Nom,Prenom,Telephone,Email,Poste,ImagePath,DateNaissance,DateEmbauche,Adresse,Experience,Salaire")]
            Employe employe,
            IFormFile? imageFile)
        {
            if (id != employe.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var oldEmploye = await _context.Employes
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);

                    if (oldEmploye == null)
                        return NotFound();

                    // Upload nouvelle image
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(
                            _environment.WebRootPath,
                            "images",
                            "employes");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName =
                            Guid.NewGuid().ToString()
                            + Path.GetExtension(imageFile.FileName);

                        string filePath = Path.Combine(
                            uploadsFolder,
                            uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        employe.ImagePath =
                            "/images/employes/" + uniqueFileName;
                    }
                    else
                    {
                        employe.ImagePath = oldEmploye.ImagePath;
                    }

                    _context.Update(employe);

                    await _context.SaveChangesAsync();

                    // Update utilisateur lié
                    var utilisateur = await _context.Utilisateurs
                        .FirstOrDefaultAsync(u => u.EmployeId == employe.Id);

                    if (utilisateur != null)
                    {
                        utilisateur.Nom =
                            employe.Nom + " " + employe.Prenom;

                        utilisateur.Email = employe.Email;

                        if (employe.Poste == "Réceptionniste")
                        {
                            utilisateur.Role = "receptionniste";
                        }

                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeExists(employe.Id))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(employe);
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employe = await _context.Employes
                .FindAsync(id);

            if (employe != null)
            {
                // Affectations liées
                var affectations = await _context.Affectations
                    .Where(a => a.EmployeId == id)
                    .ToListAsync();

                _context.Affectations.RemoveRange(affectations);

                // Utilisateur lié
                var utilisateur = await _context.Utilisateurs
                    .FirstOrDefaultAsync(u => u.EmployeId == employe.Id);

                if (utilisateur != null)
                {
                    _context.Utilisateurs.Remove(utilisateur);
                }

                // Employé
                _context.Employes.Remove(employe);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EmployeExists(int id)
        {
            return _context.Employes
                .Any(e => e.Id == id);
        }
    }
}