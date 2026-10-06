using System.ComponentModel.DataAnnotations;

namespace DarOasis.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Email invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; } = string.Empty;
    }
}
