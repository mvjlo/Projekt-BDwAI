using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class License
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Wtyczka")]
        public int PluginId { get; set; }

        [ForeignKey("PluginId")]
        public Plugin? Plugin { get; set; }

        [Required]
        [Display(Name = "Użytkownik")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        [Required(ErrorMessage = "Klucz seryjny jest wymagany")]
        [Display(Name = "Klucz seryjny")]
        [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$", 
            ErrorMessage = "Klucz seryjny musi mieć format XXXX-XXXX-XXXX-XXXX")]
        [StringLength(19)]
        public string SerialKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data zakupu jest wymagana")]
        [Display(Name = "Data zakupu")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Display(Name = "Data wygaśnięcia")]
        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }
    }
}
