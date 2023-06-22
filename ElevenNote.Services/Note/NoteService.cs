using System.Security.Claims;
using AutoMapper;
using ElevenNote.Data;
using ElevenNote.Data.AppContext;
using ElevenNote.Models.Note;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ElevenNote.Services.Note
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly int _userId;
        public NoteService(IHttpContextAccessor httpcontextAccessor, ApplicationDbContext context, IMapper mapper)
        {
            var userClaims = httpcontextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            var value = userClaims?.FindFirst("id")?.Value;

            var validId = int.TryParse(value, out _userId);

            if (!validId)
                throw new Exception("Attempted to build NoteService without User Id claim.");
            _context = context;
            _mapper = mapper;
        }

        public async Task<NoteListItem> CreateNote(NoteCreate request)
        {
            // NoteEntity noteEntity = new()
            // {
            //     Title = request.Title,
            //     Content = request.Content,
            //     CreatedUtc = DateTimeOffset.UtcNow,
            //     OwnerId = _userId
            // };

            var entity = _mapper.Map<NoteEntity>(request, opt => opt.AfterMap((src, dest) => dest.OwnerId = _userId));

            _context.Notes.Add(entity);
            var numberOfChanges = await _context.SaveChangesAsync();

            if (numberOfChanges == 1)
            {
                return _mapper.Map<NoteListItem>(entity);
                // return new NoteListItem
                // {
                //     Id = noteEntity.Id,
                //     Title = noteEntity.Title,
                //     CreatedUtc = noteEntity.CreatedUtc
                // };
            }

            return null!;
        }

        public async Task<bool> DeleteNote(int noteId)
        {
            var note = await _context.Notes.Where(n => n.OwnerId == _userId).FirstOrDefaultAsync(n => n.Id == noteId);

            if (note is null) return false;

            _context.Notes.Remove(note);

            var numberOfChanges = await _context.SaveChangesAsync();
            return numberOfChanges == 1;
        }

        public async Task<NoteDetail> GetNote(int noteId)
        {
            var note = await _context.Notes.Include(n => n.Category).Where(n => n.OwnerId == _userId).FirstOrDefaultAsync(n => n.Id == noteId);

            if (note is null) return null!;
            var conversion = _mapper.Map<NoteDetail>(note);
            
            return conversion;

            // return new NoteDetail
            // {
            //     Id = note.Id,
            //     Title = note.Title,
            //     Content = note.Content,
            //     CreatedUtc = note.CreatedUtc,
            //     ModifiedUtc = note.ModifiedUtc
            // };
        }

        public async Task<IEnumerable<NoteListItem>> GetNotes()
        {
            // var notes = await _context.Notes.Where(n => n.OwnerId == _userId).Select(n => new NoteListItem
            // {
            //     Id = n.Id,
            //     Title = n.Title,
            //     CreatedUtc = n.CreatedUtc
            // }).ToListAsync();

            var notes = await _context.Notes.Where(n => n.OwnerId == _userId).Select(n => _mapper.Map<NoteListItem>(n)).ToListAsync();
            return notes;
        }

        public async Task<bool> UpdateNote(NoteEdit model)
        {
            var isUserOwned = await _context.Notes.AnyAsync(n => n.OwnerId == _userId && n.Id == model.Id);
            if (!isUserOwned) return false;

            var newEntity = _mapper.Map<NoteEdit, NoteEntity>(model, opt => opt.AfterMap((src, dest) => dest.OwnerId = _userId));

            //update the entry state, what is another way to tell the DbContext something has changed
            _context.Entry(newEntity).State = EntityState.Modified;

            //because we currently don't have access to our created value, we'll just mark it as not modified
            _context.Entry(newEntity).Property(e => e.CreatedUtc).IsModified = false;

            var numberOfChanges = await _context.SaveChangesAsync();
            return numberOfChanges == 1;
        }

        // public async Task<bool> UpdateNote(NoteEdit model)
        // {
        //     var noteInDb = await _context.Notes.FirstOrDefaultAsync(n => n.OwnerId == _userId && n.Id == model.Id);
        //     if (noteInDb is null) return false;

        //     noteInDb.Title = model.Title;
        //     noteInDb.Content = model.Content;
        //     noteInDb.ModifiedUtc = model.ModifiedUtc;

        //     var numberOfChanges = await _context.SaveChangesAsync();
        //     return numberOfChanges == 1;
        // }
    }
}