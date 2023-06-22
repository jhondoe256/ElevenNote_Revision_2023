using ElevenNote.Models.Note;

namespace ElevenNote.Models.Category
{
    public class CategoryDetail
    {
       
        public int Id { get; set; }

        public string CategoryTitle { get; set; } = string.Empty;

        public List<NoteListItem> Notes { get; set; }
    }
}