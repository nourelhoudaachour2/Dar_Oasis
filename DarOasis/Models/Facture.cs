namespace DarOasis.Models
{
    public class Facture
    {
        public int IdFacture { get; set; }

        public string NumeroFacture { get; set; } = string.Empty;

        public DateTime DateFacture { get; set; }

        public DateTime DateEcheance { get; set; }

        public string Statut { get; set; } = "Non payé";

        public decimal MontantSejour { get; set; }

        public decimal MontantServices { get; set; }

        public decimal Promotion { get; set; }

        public decimal PenaliteRetard { get; set; }

        public decimal MontantTotal { get; set; }

        public int ReservationId { get; set; }

        public Reservation? Reservation { get; set; }
    }
}