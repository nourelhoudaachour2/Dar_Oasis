using System.Text.RegularExpressions;

namespace DarOasis.Models
{
    public class Reservation
    {
        public int IdReservation { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public int NombreJours { get; set; }
        public decimal PrixTotal { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int ChambreId { get; set; }
        public Chambre? Chambre { get; set; }

       
        public Facture? Facture { get; set; }
        public ICollection<ReservationService> ReservationServices { get; set; } = new List<ReservationService>();

    }
}
