using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DarOasis.Data;
using DarOasis.Models;
using DarOasis.ViewModels;

namespace DarOasis.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        // INDEX
        public async Task<IActionResult> Index(string cin)
        {
            var reservations = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Chambre)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(cin))
            {
                reservations = reservations
                    .Where(r => r.Client.NumeroIdentite.Contains(cin));
            }

            ViewBag.CinRecherche = cin;

            return View(await reservations.ToListAsync());
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Chambre)
                .Include(r => r.Accompagnants)
                .Include(r => r.ReservationServices)
                    .ThenInclude(rs => rs.ServiceSupplementaire)
                .FirstOrDefaultAsync(r => r.IdReservation == id);

            if (reservation == null)
                return NotFound();

            return View(reservation);
        }

        // CREATE GET
        public IActionResult Create()
        {
            var model = new ReservationCreateViewModel
            {
                DateDebut = DateTime.Today,
                DateFin = DateTime.Today.AddDays(1),
                NombrePersonnes = 0,

                ServicesDisponibles =
                    _context.ServicesSupplementaires.ToList(),

                TypesChambres = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "simple",
                        Text = "Simple"
                    },

                    new SelectListItem
                    {
                        Value = "double",
                        Text = "Double"
                    },

                    new SelectListItem
                    {
                        Value = "suite",
                        Text = "Suite"
                    },

                    new SelectListItem
                    {
                        Value = "familiale",
                        Text = "Familiale"
                    }
                }
            };

            return View(model);
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ReservationCreateViewModel model)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c =>
                    c.NumeroIdentite == model.NumeroIdentite);

            if (client == null)
                ModelState.AddModelError("",
                    "Client introuvable.");

            var chambre = await _context.Chambres
                .FirstOrDefaultAsync(c =>
                    c.IdChambre == model.ChambreId);

            if (chambre == null)
                ModelState.AddModelError("",
                    "Chambre introuvable.");

            bool chambreOccupee =
                await _context.Reservations.AnyAsync(r =>
                    r.ChambreId == model.ChambreId &&
                    model.DateDebut < r.DateFin &&
                    model.DateFin > r.DateDebut);

            if (chambreOccupee)
                ModelState.AddModelError("",
                    "Cette chambre est déjà réservée.");

            if (ModelState.IsValid
                && client != null
                && chambre != null)
            {
                var nombreJours =
                    (model.DateFin - model.DateDebut).Days;

                decimal coefficient =
                    GetCoefficientSaison(model.DateDebut);

                var reservation = new Reservation
                {
                    ClientId = client.Id,
                    ChambreId = model.ChambreId,
                    DateDebut = model.DateDebut,
                    DateFin = model.DateFin,
                    NombrePersonnes = model.NombrePersonnes,
                    NombreJours = nombreJours,

                    PrixTotal =
                        nombreJours
                        * chambre.PrixParNuit
                        * coefficient
                };

                _context.Reservations.Add(reservation);

                chambre.Statut = "occupée";

                await _context.SaveChangesAsync();

                // Accompagnants
                foreach (var a in model.Accompagnants)
                {
                    _context.Accompagnants.Add(
                        new Accompagnant
                        {
                            Nom = a.Nom,
                            Prenom = a.Prenom,
                            DateNaissance = a.DateNaissance,
                            ReservationId = reservation.IdReservation
                        });
                }

                // Services
                foreach (var serviceId in model.ServicesIds)
                {
                    _context.ReservationServices.Add(
                        new ReservationService
                        {
                            ReservationId = reservation.IdReservation,
                            ServiceId = serviceId,
                            Quantite = nombreJours
                        });
                }

                var montantServices =
                    await _context.ServicesSupplementaires
                    .Where(s => model.ServicesIds.Contains(s.IdService))
                    .SumAsync(s => s.PrixService * nombreJours);

                int nombreReservationsClient =
                    await _context.Reservations
                    .CountAsync(r => r.ClientId == client.Id);

                decimal sousTotal =
                    reservation.PrixTotal + montantServices;

                decimal promotion =
                    nombreReservationsClient >= 3
                    ? sousTotal * 0.20m
                    : 0;

                var facture = new Facture
                {
                    NumeroFacture =
                        "FAC-" + DateTime.Now.ToString("yyyyMMddHH"),

                    DateFacture = DateTime.Today,

                    DateEcheance =
                        DateTime.Today.AddDays(7),

                    Statut = "Non payé",

                    ReservationId = reservation.IdReservation,

                    MontantSejour = reservation.PrixTotal,
                    MontantServices = montantServices,
                    Promotion = promotion,
                    PenaliteRetard = 0,

                    MontantTotal =
                        sousTotal - promotion
                };

                _context.Factures.Add(facture);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.TypesChambres = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "simple",
                    Text = "Simple"
                },

                new SelectListItem
                {
                    Value = "double",
                    Text = "Double"
                },

                new SelectListItem
                {
                    Value = "suite",
                    Text = "Suite"
                },

                new SelectListItem
                {
                    Value = "familiale",
                    Text = "Familiale"
                }
            };

            return View(model);
        }

        // GET CLIENT BY CIN
        [HttpGet]
        public async Task<IActionResult> GetClientByCin(string cin)
        {
            cin = cin.Trim();

            var client = await _context.Clients
                .Where(c => c.NumeroIdentite.Trim() == cin)
                .Select(c => new
                {
                    id = c.Id,
                    nom = c.Nom,
                    prenom = c.Prenom
                })
                .FirstOrDefaultAsync();

            if (client == null)
                return NotFound();

            return Json(client);
        }

        // GET CHAMBRES DISPONIBLES
        [HttpGet]
        public async Task<IActionResult> GetChambresDisponibles(
            string type,
            DateTime dateDebut,
            DateTime dateFin)
        {
            var chambresReservees =
                await _context.Reservations
                .Where(r =>
                    dateDebut < r.DateFin &&
                    dateFin > r.DateDebut)
                .Select(r => r.ChambreId)
                .ToListAsync();

            var chambres = await _context.Chambres
                .Where(c =>
                    c.Type.ToLower() == type.ToLower()
                    && !chambresReservees.Contains(c.IdChambre))
                .Select(c => new
                {
                    c.IdChambre,
                    c.Numero
                })
                .ToListAsync();

            return Json(chambres);
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Chambre)
                .Include(r => r.Accompagnants)
                .Include(r => r.ReservationServices)
                .FirstOrDefaultAsync(r =>
                    r.IdReservation == id);

            if (reservation == null)
                return NotFound();

            ViewData["ChambreId"] = new SelectList(
                _context.Chambres,
                "IdChambre",
                "Numero",
                reservation.ChambreId);

            ViewData["ClientId"] = new SelectList(
                _context.Clients.Select(c => new
                {
                    c.Id,
                    FullName =
                        c.Nom + " "
                        + c.Prenom + " - "
                        + c.NumeroIdentite
                }),
                "Id",
                "FullName",
                reservation.ClientId);

            ViewBag.Services =
                await _context.ServicesSupplementaires
                .ToListAsync();

            return View(reservation);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("IdReservation,DateDebut,DateFin,NombreJours,PrixTotal,ClientId,ChambreId")]
            Reservation reservation)
        {
            if (id != reservation.IdReservation)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["ChambreId"] = new SelectList(
                    _context.Chambres,
                    "IdChambre",
                    "Numero",
                    reservation.ChambreId);

                ViewData["ClientId"] = new SelectList(
                    _context.Clients.Select(c => new
                    {
                        c.Id,
                        FullName =
                            c.Nom + " "
                            + c.Prenom + " - "
                            + c.NumeroIdentite
                    }),
                    "Id",
                    "FullName",
                    reservation.ClientId);

                return View(reservation);
            }

            var oldReservation =
                await _context.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.IdReservation == id);

            if (oldReservation == null)
                return NotFound();

            if (oldReservation.ChambreId != reservation.ChambreId)
            {
                var oldRoom =
                    await _context.Chambres
                    .FindAsync(oldReservation.ChambreId);

                if (oldRoom != null)
                    oldRoom.Statut = "disponible";
            }

            var newRoom =
                await _context.Chambres
                .FindAsync(reservation.ChambreId);

            if (newRoom != null)
                newRoom.Statut = "occupée";

            _context.Update(reservation);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Chambre)
                .FirstOrDefaultAsync(r =>
                    r.IdReservation == id);

            if (reservation == null)
                return RedirectToAction(nameof(Index));

            // Facture liée
            var facture = await _context.Factures
                .FirstOrDefaultAsync(f =>
                    f.ReservationId == id);

            // Accompagnants liés
            var accompagnants =
                await _context.Accompagnants
                .Where(a => a.ReservationId == id)
                .ToListAsync();

            // Services liés
            var reservationServices =
                await _context.ReservationServices
                .Where(rs => rs.ReservationId == id)
                .ToListAsync();

            if (facture != null)
            {
                _context.Factures.Remove(facture);
            }

            _context.Accompagnants
                .RemoveRange(accompagnants);

            _context.ReservationServices
                .RemoveRange(reservationServices);

            // Chambre disponible
            if (reservation.Chambre != null)
            {
                reservation.Chambre.Statut = "disponible";
            }

            // Reservation
            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // SAISON
        private decimal GetCoefficientSaison(DateTime date)
        {
            int mois = date.Month;

            // Très haute saison
            if (mois == 6 || mois == 7 || mois == 8)
                return 1.50m;

            // Haute saison
            if (mois == 5 || mois == 12)
                return 1.30m;

            // Basse saison
            return 1.00m;
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations
                .Any(e => e.IdReservation == id);
        }
    }
}