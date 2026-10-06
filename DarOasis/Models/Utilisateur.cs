namespace DarOasis.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MotDePasse { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;


        // MArbout bil employé bech njm n3aytlou
        public int? EmployeId { get; set; }

        public Employe? Employe { get; set; }
    }
}