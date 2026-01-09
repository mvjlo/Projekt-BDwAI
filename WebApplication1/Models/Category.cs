using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa kategorii jest wymagana")]
        [StringLength(50)]
        [Display(Name = "Nazwa kategorii")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        [StringLength(500)]
        public string? Description { get; set; }

        public ICollection<Plugin> Plugins { get; set; } = new List<Plugin>();
    }
}
