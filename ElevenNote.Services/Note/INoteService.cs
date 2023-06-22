using ElevenNote.Models.Note;

namespace ElevenNote.Services.Note
{
    public interface INoteService
    {
        Task<IEnumerable<NoteListItem>> GetNotes();
        Task<NoteDetail> GetNote(int noteId);
        Task<NoteListItem> CreateNote(NoteCreate request);
        Task<bool> UpdateNote(NoteEdit model);
        Task<bool> DeleteNote(int noteId);
    }
}