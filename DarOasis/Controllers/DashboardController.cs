using DarOasis.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DarOasis.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") == null)
                return RedirectToAction("Login", "Auth");

            var role = HttpContext.Session.GetString("UserRole");

            if (role == "receptionniste")
            {
                return DashboardReceptionniste();
            }

            return DashboardGerant();
        }

        private IActionResult DashboardGerant()
        {
            ViewBag.TotalChambres = _context.Chambres.Count();
            ViewBag.ChambresDisponibles = _context.Chambres.Count(c => c.Statut == "disponible");
            ViewBag.ChambresOccupees = _context.Chambres.Count(c => c.Statut == "occupée");

            ViewBag.TotalClients = _context.Clients.Count();
            ViewBag.TotalEmployes = _context.Employes.Count();

            int total = _context.Chambres.Count();
            int occupees = _context.Chambres.Count(c => c.Statut == "occupée");

            ViewBag.TauxOccupation =
                total > 0 ? (int)Math.Round((double)occupees / total * 100) : 0;

            int annee = DateTime.Now.Year;

            var caParMois = _context.Factures
                .Where(f => f.DateFacture.Year == annee)
                .GroupBy(f => f.DateFacture.Month)
                .Select(g => new
                {
                    Mois = g.Key,
                    Total = g.Sum(f => f.MontantTotal)
                })
                .ToList();

            var caArray = new decimal[12];

            foreach (var item in caParMois)
            {
                caArray[item.Mois - 1] = item.Total;
            }

            ViewBag.CAParMois = caArray;
            ViewBag.CAMoisActuel = caArray[DateTime.Now.Month - 1];

            ViewBag.Top5Clients = _context.Reservations
                .Include(r => r.Client)
                .GroupBy(r => r.Client)
                .Select(g => new
                {
                    Client = g.Key,
                    NbReservations = g.Count(),
                    TotalPaye = g.Sum(r => r.PrixTotal)
                })
                .OrderByDescending(x => x.NbReservations)
                .Take(5)
                .ToList();

            ViewBag.TopMois = _context.Reservations
                .GroupBy(r => r.DateDebut.Month)
                .Select(g => new
                {
                    Mois = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .ToList();

            return View("Index");
        }

        private IActionResult DashboardReceptionniste()
        {
            DateTime today = DateTime.Today;

            ViewBag.TotalChambres = _context.Chambres.Count();
            ViewBag.ChambresDisponibles = _context.Chambres.Count(c => c.Statut == "disponible");
            ViewBag.ChambresOccupees = _context.Chambres.Count(c => c.Statut == "occupée");

            int total = _context.Chambres.Count();
            int occupees = _context.Chambres.Count(c => c.Statut == "occupée");

            ViewBag.TauxOccupation =
                total > 0 ? (int)Math.Round((double)occupees / total * 100) : 0;

            ViewBag.TotalClients = _context.Clients.Count();

            ViewBag.ReservationsAujourdhui = _context.Reservations
                .Count(r => r.DateDebut.Date == today);

            ViewBag.DepartsAujourdhui = _context.Reservations
                .Count(r => r.DateFin.Date == today);

            ViewBag.FacturesNonPayees = _context.Factures
                .Count(f => f.Statut == "Non payé" || f.Statut == "non payé");

            ViewBag.ListeReservationsJour = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Chambre)
                .Where(r => r.DateDebut.Date == today)
                .OrderBy(r => r.DateDebut)
                .Take(5)
                .ToList();
            ViewBag.ClientsRecents = _context.Clients
                .OrderByDescending(c => c.Id)
                .Take(5)
                .ToList();

            ViewBag.FacturesEnAttente = _context.Factures
                .Where(f => f.Statut == "Non payé" || f.Statut == "non payé")
                .OrderByDescending(f => f.DateFacture)
                .Take(5)
                .ToList();

            return View("Receptionniste");
        }
    }
}