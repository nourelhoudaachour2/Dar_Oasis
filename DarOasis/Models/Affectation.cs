namespace DarOasis.Models
{
    public class Affectation
    {
        public int IdAffectation { get; set; }
        public DateTime DateAffectation { get; set; }

        public int EmployeId { get; set; }
        public Employe? Employe { get; set; }

        public int ChambreId { get; set; }
        public Chambre? Chambre { get; set; }
    }
}
