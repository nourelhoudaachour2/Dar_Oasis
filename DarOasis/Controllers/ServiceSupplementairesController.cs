using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DarOasis.Data;
using DarOasis.Models;

namespace DarOasis.Controllers
{
    public class ServiceSupplementairesController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceSupplementairesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ServicesSupplementaires.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var serviceSupplementaire = await _context.ServicesSupplementaires
                .FirstOrDefaultAsync(m => m.IdService == id);

            if (serviceSupplementaire == null)
                return NotFound();

            return View(serviceSupplementaire);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdService,NomService,PrixService")]
            ServiceSupplementaire serviceSupplementaire)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceSupplementaire);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(serviceSupplementaire);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var serviceSupplementaire = await _context.ServicesSupplementaires.FindAsync(id);

            if (serviceSupplementaire == null)
                return NotFound();

            return View(serviceSupplementaire);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("IdService,NomService,PrixService")]
            ServiceSupplementaire serviceSupplementaire)
        {
            if (id != serviceSupplementaire.IdService)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceSupplementaire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceSupplementaireExists(serviceSupplementaire.IdService))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(serviceSupplementaire);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var serviceSupplementaire = await _context.ServicesSupplementaires
                .FirstOrDefaultAsync(m => m.IdService == id);

            if (serviceSupplementaire == null)
                return NotFound();

            return View(serviceSupplementaire);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceSupplementaire = await _context.ServicesSupplementaires
                .FindAsync(id);

            if (serviceSupplementaire == null)
                return RedirectToAction(nameof(Index));

            var reservationServices = await _context.ReservationServices
                .Where(rs => rs.ServiceId == id)
                .ToListAsync();

            _context.ReservationServices.RemoveRange(reservationServices);
            _context.ServicesSupplementaires.Remove(serviceSupplementaire);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ServiceSupplementaireExists(int id)
        {
            return _context.ServicesSupplementaires.Any(e => e.IdService == id);
        }
    }
}