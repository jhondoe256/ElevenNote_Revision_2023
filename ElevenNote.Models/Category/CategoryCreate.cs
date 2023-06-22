using System.ComponentModel.DataAnnotations;

namespace ElevenNote.Models.Category
{
    public class CategoryCreate
    {
        [Required]
        [MinLength(2, ErrorMessage = "{1} Can only hold  at least {0} characters.")]
        [MaxLength(100, ErrorMessage = "{1} Can only hold  at least {0} Max characters.")]
        public string CategoryTitle { get; set; } = string.Empty;  
    }
}