using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Plugin
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa wtyczki jest wymagana")]
        [StringLength(100)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wersja jest wymagana")]
        [Display(Name = "Wersja")]
        public string Version { get; set; } = "VST3";

        [Display(Name = "Cena")]
        [Range(0, double.MaxValue, ErrorMessage = "Cena nie może być ujemna")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Wymagania systemowe")]
        [StringLength(1000)]
        public string? SystemRequirements { get; set; }

        [Display(Name = "Opis")]
        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Producent")]
        public int ManufacturerId { get; set; }

        [ForeignKey("ManufacturerId")]
        public Manufacturer? Manufacturer { get; set; }

        [Required]
        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public ICollection<License> Licenses { get; set; } = new List<License>();
    }
}
