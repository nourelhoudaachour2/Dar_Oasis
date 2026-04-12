namespace DarOasis.Models
{
    public class Facture
    {
        public int IdFacture { get; set; }
        public DateTime DateFacture { get; set; }
        public decimal MontantSejour { get; set; }
        public decimal MontantServices { get; set; }
        public decimal MontantTotal { get; set; }

        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
