using AutoMapper;
using ElevenNote.Data;
using ElevenNote.Data.AppContext;
using ElevenNote.Models.Category;
using Microsoft.EntityFrameworkCore;

namespace ElevenNote.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
             var category = await _context.Categories.FindAsync(categoryId);
            if(category is null) return false;

            _context.Categories.Remove(category);

            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<List<CategoryListItem>> GetCategories()
        {
            var categories = await _context.Categories.Select(c=>_mapper.Map<CategoryListItem>(c)).ToListAsync();
            return categories;
        }

        public async Task<CategoryDetail> GetCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if(category is null) return null!;

            return _mapper.Map<CategoryDetail>(category);
        }

        public async Task<CategoryListItem> PostCategory(CategoryCreate model)
        {
            var entity = _mapper.Map<CategoryEntity>(model);
            await _context.Categories.AddAsync(entity);

            var numberOfChanges = await _context.SaveChangesAsync();
            if (numberOfChanges == 1)
            {
                return _mapper.Map<CategoryListItem>(entity);
            }
            return null!;
        }

        public async Task<CategoryListItem> UpdateCategory(CategoryEdit model)
        {
            var categoryInDb = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);

            var conversion = _mapper.Map<CategoryEdit, CategoryEntity>(model);

            _context.Categories.Update(conversion);

            if (await _context.SaveChangesAsync() == 1)
                return _mapper.Map<CategoryListItem>(conversion);
            else
                return null!;
        }
    }
}