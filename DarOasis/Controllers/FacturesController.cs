using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DarOasis.Data;
using DarOasis.Models;

namespace DarOasis.Controllers
{
    public class FacturesController : Controller
    {
        private readonly AppDbContext _context;

        public FacturesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Factures
        public async Task<IActionResult> Index()
        {
            var factures = await _context.Factures
                .Include(f => f.Reservation)
                    .ThenInclude(r => r.Client)
                .ToListAsync();

            return View(factures);
        }

        // GET: Factures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var facture = await _context.Factures
                .Include(f => f.Reservation)
                    .ThenInclude(r => r.Client)

                .Include(f => f.Reservation)
                    .ThenInclude(r => r.Chambre)

                .Include(f => f.Reservation)
                    .ThenInclude(r => r.ReservationServices)
                        .ThenInclude(rs => rs.ServiceSupplementaire)

                .FirstOrDefaultAsync(f => f.IdFacture == id);

            if (facture == null)
                return NotFound();

            if (facture.Reservation == null
                || facture.Reservation.Client == null
                || facture.Reservation.Chambre == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(facture);
        }

        // GET: Factures/Create
        public IActionResult Create()
        {
            ViewData["ReservationId"] =
                new SelectList(_context.Reservations,
                    "IdReservation",
                    "IdReservation");

            return View();
        }

        // POST: Factures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdFacture,NumeroFacture,DateFacture,DateEcheance,Statut,MontantSejour,MontantServices,Promotion,PenaliteRetard,MontantTotal,ReservationId")]
            Facture facture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facture);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ReservationId"] =
                new SelectList(_context.Reservations,
                    "IdReservation",
                    "IdReservation",
                    facture.ReservationId);

            return View(facture);
        }

        // GET: Factures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var facture = await _context.Factures.FindAsync(id);

            if (facture == null)
                return NotFound();

            return View(facture);
        }

        // POST: Factures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Facture model)
        {
            var facture = await _context.Factures.FindAsync(id);

            if (facture == null)
                return NotFound();

            facture.DateEcheance = model.DateEcheance;
            facture.Statut = model.Statut;

            if (facture.Statut != "Payé"
                && facture.DateEcheance.Date < DateTime.Today)
            {
                facture.PenaliteRetard =
                    facture.MontantTotal * 0.05m;
            }
            else
            {
                facture.PenaliteRetard = 0;
            }

            facture.MontantTotal =
                facture.MontantSejour
                + facture.MontantServices
                - facture.Promotion
                + facture.PenaliteRetard;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Factures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var facture = await _context.Factures
                .Include(f => f.Reservation)
                    .ThenInclude(r => r.Client)
                .FirstOrDefaultAsync(m => m.IdFacture == id);

            if (facture == null)
                return NotFound();

            return View(facture);
        }

        // POST: Factures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facture = await _context.Factures.FindAsync(id);

            if (facture != null)
            {
                _context.Factures.Remove(facture);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool FactureExists(int id)
        {
            return _context.Factures.Any(e => e.IdFacture == id);
        }
    }
}