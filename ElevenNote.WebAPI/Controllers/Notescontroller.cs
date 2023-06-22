using ElevenNote.Models.Note;
using ElevenNote.Services.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class Notescontroller : ControllerBase
    {
        private readonly INoteService _noteService;

        public Notescontroller(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var notes = await _noteService.GetNotes();
            return Ok(notes);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var note = await _noteService.GetNote(id);

            if (note is null) return NotFound();

            return Ok(note);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(NoteCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _noteService.CreateNote(model);

            if (response is null)
            {
                return BadRequest("Could not create Note.");
            }

            return Ok(response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, NoteEdit model)
        {
            if (id != model.Id)
                return BadRequest("Invalid Id.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await _noteService.UpdateNote(model) ?
            Ok("Update successful.") : BadRequest("Note could not be updated.");
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Id.");

            return await _noteService.DeleteNote(id) ?
            Ok("Delete successful.") : BadRequest("Note could not be Deleted.");
        }
    }
}