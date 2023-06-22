using System.ComponentModel.DataAnnotations;

namespace ElevenNote.Data
{
    public class CategoryEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "{1} Can only hold  at least {0} characters.")]
        [MaxLength(100, ErrorMessage = "{1} Can only hold  at least {0} Max characters.")]
        public string CategoryTitle { get; set; } = string.Empty;

        public List<NoteEntity> Notes { get; set; }
    }
}