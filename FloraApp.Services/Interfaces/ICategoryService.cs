using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloraApp.Services.Interfaces
{
    public interface ICategoryService : ICrudService<CategoryResponse, CategorySearchObject, CategoryUpsertRequest, CategoryUpsertRequest>
    {
        Task<PagedResult<CategoryResponse>> GetAllCategoriesAsync();
    }
} 