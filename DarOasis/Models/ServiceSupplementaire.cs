namespace DarOasis.Models
{
    public class ServiceSupplementaire
    {
        public int IdService { get; set; }
        public string NomService { get; set; } = string.Empty;
        public decimal PrixService { get; set; }
        public ICollection<ReservationService> ReservationServices { get; set; } = new List<ReservationService>();
    }
}
