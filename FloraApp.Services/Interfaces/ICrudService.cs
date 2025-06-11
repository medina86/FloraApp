using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FloraApp.Services.Interfaces
{
    public interface ICrudService<T, TSearch, TInsert, TUpdate> : IService<T, TSearch> 
        where T : class 
        where TSearch : BaseSearchObject
        where TInsert : class 
        where TUpdate : class
    {
        Task<T> CreateAsync(TInsert request);
        Task<T?> UpdateAsync(int id, TUpdate request);
        Task<bool> DeleteAsync(int id);
    }
} 