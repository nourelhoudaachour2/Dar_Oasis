namespace DarOasis.Models
{
    public class Chambre
    {
        public int IdChambre { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; 
        public decimal PrixParNuit { get; set; }
        public bool Disponible { get; set; } = true;
     

        public decimal Note { get; set; } = 0;
        public string Statut { get; set; } = "disponible";

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
    }
}
