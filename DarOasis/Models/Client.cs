namespace DarOasis.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Nationalite { get; set; } = string.Empty;
        public string TypePieceIdentite { get; set; } = string.Empty;
        public int NumeroIdentite { get; set; }

       
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
