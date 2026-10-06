namespace DarOasis.Models
{
    public class Employe
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Poste { get; set; } = string.Empty;
        public DateTime DateNaissance { get; set; }

        public DateTime DateEmbauche { get; set; }

        public string Adresse { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public decimal Salaire { get; set; }
        public string? ImagePath { get; set; }


        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
    }
}
