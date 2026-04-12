namespace DarOasis.Models
{
    public class ReservationService
    {
        public int Id { get; set; }
        public int Quantite { get; set; } = 1;

        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public int ServiceId { get; set; }
        public ServiceSupplementaire? ServiceSupplementaire { get; set; }
    }
}
