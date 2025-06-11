using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloraApp.Services.Interfaces
{
    public interface IService<T, TSearch>
        where T : class 
        where TSearch : BaseSearchObject
    {
        Task<PagedResult<T>> GetAsync(TSearch search);
        Task<T> GetByIdAsync(int id);
    }
} 
