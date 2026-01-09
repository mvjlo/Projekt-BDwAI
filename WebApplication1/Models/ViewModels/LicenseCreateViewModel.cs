using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class LicenseCreateViewModel
    {
        [Required]
        [Display(Name = "Wtyczka")]
        public int PluginId { get; set; }

        [Required(ErrorMessage = "Klucz seryjny jest wymagany")]
        [Display(Name = "Klucz seryjny")]
        [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$", 
            ErrorMessage = "Klucz seryjny musi mieć format XXXX-XXXX-XXXX-XXXX")]
        public string SerialKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data zakupu jest wymagana")]
        [Display(Name = "Data zakupu")]
        [DataType(DataType.Date)]
        [PurchaseDateValidation]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Display(Name = "Data wygaśnięcia")]
        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }
    }

    public class PurchaseDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date > DateTime.Now)
                {
                    return new ValidationResult("Data zakupu nie może być z przyszłości");
                }
            }
            return ValidationResult.Success;
        }
    }
}
