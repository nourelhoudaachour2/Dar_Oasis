using DarOasis.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DarOasis.ViewModels
{
    public class AccompagnantInput
    {
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public DateTime DateNaissance { get; set; }
    }

    public class ReservationCreateViewModel
    {
        public string NumeroIdentite { get; set; } = string.Empty;

        public string NomClient { get; set; } = string.Empty;
        public string PrenomClient { get; set; } = string.Empty;

        public int ClientId { get; set; }

        public int NombrePersonnes { get; set; }

        public List<AccompagnantInput> Accompagnants { get; set; } = new();

        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }

        public string TypeChambre { get; set; } = string.Empty;

        public int ChambreId { get; set; }

        public List<SelectListItem> TypesChambres { get; set; } = new();
        public List<SelectListItem> ChambresDisponibles { get; set; } = new();

        public List<int> ServicesIds { get; set; } = new();

        public List<ServiceSupplementaire> ServicesDisponibles { get; set; } = new();

    }
}