using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa producenta jest wymagana")]
        [StringLength(100)]
        [Display(Name = "Nazwa producenta")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Strona WWW")]
        [Url(ErrorMessage = "Niepoprawny format URL")]
        public string? Website { get; set; }

        [Display(Name = "Email wsparcia")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email")]
        public string? SupportEmail { get; set; }

        public ICollection<Plugin> Plugins { get; set; } = new List<Plugin>();
    }
}
