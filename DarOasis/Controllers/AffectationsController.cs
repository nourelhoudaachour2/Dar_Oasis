using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DarOasis.Data;
using DarOasis.Models;

namespace DarOasis.Controllers
{ public class AffectationsController : Controller
    {
        private readonly AppDbContext _context;

        public AffectationsController(AppDbContext context)
        {
            _context = context;
        }

        // ethy el pae index fil view
        public async Task<IActionResult> Index()
        {
            var affectations = await _context.Affectations
                .Include(a => a.Employe)
                .Include(a => a.Chambre)
                .OrderByDescending(a => a.DateAffectation)
                .ToListAsync();

            return View(affectations);
        }

        // partie eli lehya bl DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var affectation = await _context.Affectations
                .Include(a => a.Employe)
                .Include(a => a.Chambre)
                .FirstOrDefaultAsync(a => a.IdAffectation == id);

            if (affectation == null)
                return NotFound();

            return View(affectation);
        }

        // CREATE
        public IActionResult Create()
        {
            ViewBag.Employes = _context.Employes
                .Where(e => e.Poste == "Femme de ménage")
                .ToList();

            ViewBag.Chambres = _context.Chambres.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int EmployeId,
            List<int> ChambreIds,
            DateTime DateAffectation)
        {
            if (EmployeId == 0 || ChambreIds == null || ChambreIds.Count == 0)
            {
                ViewBag.Employes = _context.Employes
                    .Where(e => e.Poste == "Femme de ménage")
                    .ToList();

                ViewBag.Chambres = _context.Chambres.ToList();

                ModelState.AddModelError("",
                    "Veuillez choisir une femme de ménage et au moins une chambre.");

                return View();
            }

            foreach (var chambreId in ChambreIds)
            {
                _context.Affectations.Add(new Affectation
                {
                    EmployeId = EmployeId,
                    ChambreId = chambreId,
                    DateAffectation = DateAffectation
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var affectation = await _context.Affectations
                .Include(a => a.Employe)
                .Include(a => a.Chambre)
                .FirstOrDefaultAsync(a => a.IdAffectation == id);

            if (affectation == null)
                return NotFound();

            ViewBag.Employes = _context.Employes
                .Where(e => e.Poste == "Femme de ménage")
                .ToList();

            ViewBag.Chambres = _context.Chambres.ToList();

            ViewBag.SelectedChambres = _context.Affectations
                .Where(a => a.EmployeId == affectation.EmployeId
                         && a.DateAffectation.Date == affectation.DateAffectation.Date)
                .Select(a => a.ChambreId)
                .ToList();

            return View(affectation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int IdAffectation,
            int EmployeId,
            List<int> ChambreIds,
            DateTime DateAffectation)
        {
            if (EmployeId == 0 || ChambreIds == null || ChambreIds.Count == 0)
            {
                ViewBag.Employes = _context.Employes
                    .Where(e => e.Poste == "Femme de ménage")
                    .ToList();

                ViewBag.Chambres = _context.Chambres.ToList();

                ViewBag.SelectedChambres = ChambreIds ?? new List<int>();

                ModelState.AddModelError("",
                    "Veuillez choisir une femme de ménage et au moins une chambre.");

                return View(new Affectation
                {
                    IdAffectation = IdAffectation,
                    EmployeId = EmployeId,
                    DateAffectation = DateAffectation
                });
            }

            var oldAffectation = await _context.Affectations
                .FirstOrDefaultAsync(a => a.IdAffectation == IdAffectation);

            if (oldAffectation == null)
                return NotFound();

            var oldGroup = await _context.Affectations
    .Where(a => a.EmployeId == oldAffectation.EmployeId
             && a.DateAffectation.Date == oldAffectation.DateAffectation.Date)
    .ToListAsync();
            _context.Affectations.RemoveRange(oldGroup);

            foreach (var chambreId in ChambreIds)
            {
                _context.Affectations.Add(new Affectation
                {
                    EmployeId = EmployeId,
                    ChambreId = chambreId,
                    DateAffectation = DateAffectation
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var affectation = await _context.Affectations
                .FindAsync(id);

            if (affectation != null)
            {
                _context.Affectations.Remove(affectation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AffectationExists(int id)
        {
            return _context.Affectations
                .Any(e => e.IdAffectation == id);
        }
    }
}