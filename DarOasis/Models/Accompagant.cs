namespace DarOasis.Models
{
    public class Accompagnant
    {
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;

        public string Prenom { get; set; } = string.Empty;

        public DateTime DateNaissance { get; set; }

        public int ReservationId { get; set; }

        public Reservation? Reservation { get; set; }
    }
}