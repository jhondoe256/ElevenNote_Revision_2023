using ElevenNote.Models.Category;

namespace ElevenNote.Services.Category
{
    public interface ICategoryService
    {
        Task<CategoryListItem> PostCategory(CategoryCreate model);
        Task<CategoryListItem> UpdateCategory(CategoryEdit model);
        Task<CategoryDetail> GetCategory(int categoryId);
        Task<List<CategoryListItem>> GetCategories();
        Task<bool> DeleteCategory(int categoryId);
    }
}