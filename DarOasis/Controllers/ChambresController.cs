using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DarOasis.Data;
using DarOasis.Models;

namespace DarOasis.Controllers
{
    public class ChambresController : Controller
    {
        private readonly AppDbContext _context;

        public ChambresController(AppDbContext context)
        {
            _context = context;
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chambres.ToListAsync());
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var chambre = await _context.Chambres
                .Include(c => c.Affectations)
                    .ThenInclude(a => a.Employe)
                .FirstOrDefaultAsync(m => m.IdChambre == id);

            if (chambre == null)
                return NotFound();

            return View(chambre);
        }

        // CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdChambre,Numero,Type,PrixParNuit,Disponible,Note,Statut")]
            Chambre chambre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chambre);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(chambre);
        }

        // EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var chambre = await _context.Chambres
                .Include(c => c.Affectations)
                    .ThenInclude(a => a.Employe)
                .FirstOrDefaultAsync(c => c.IdChambre == id);

            if (chambre == null)
                return NotFound();

            return View(chambre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("IdChambre,Numero,Type,PrixParNuit,Disponible,Note,Statut")]
            Chambre chambre)
        {
            if (id != chambre.IdChambre)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chambre);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChambreExists(chambre.IdChambre))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(chambre);
        }

        // DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chambre = await _context.Chambres
                .FindAsync(id);

            if (chambre == null)
                return RedirectToAction(nameof(Index));

            // Réservations liées
            var reservations = await _context.Reservations
                .Where(r => r.ChambreId == id)
                .ToListAsync();

            // IDs réservations
            var reservationIds = reservations
                .Select(r => r.IdReservation)
                .ToList();

            // Factures liées
            var factures = await _context.Factures
                .Where(f => reservationIds.Contains(f.ReservationId))
                .ToListAsync();

            // Affectations liées
            var affectations = await _context.Affectations
                .Where(a => a.ChambreId == id)
                .ToListAsync();

            // Delete ordre correct
            _context.Factures.RemoveRange(factures);
            _context.Reservations.RemoveRange(reservations);
            _context.Affectations.RemoveRange(affectations);
            _context.Chambres.Remove(chambre);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ChambreExists(int id)
        {
            return _context.Chambres
                .Any(e => e.IdChambre == id);
        }
    }
}