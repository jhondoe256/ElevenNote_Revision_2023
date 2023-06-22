using ElevenNote.Models.Category;
using ElevenNote.Services.Category;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Categorycontroller : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public Categorycontroller(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _categoryService.GetCategory(id);

            if (category is null) return BadRequest("Invalid id");

            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CategoryCreate model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _categoryService.PostCategory(model);
            if (response is null) return BadRequest("Unable to post Category.");

            return Ok(response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, CategoryEdit model)
        {
            if (id != model.Id)
                return BadRequest("Invalid Id.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _categoryService.UpdateCategory(model);
            if (response is null) return BadRequest("Unable to update Category.");

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Id.");

            return (await _categoryService.DeleteCategory(id)) ?
                    Ok("Deletion Successful!")
                    :
                    BadRequest("Unable to update Category.");
        }
    }
}