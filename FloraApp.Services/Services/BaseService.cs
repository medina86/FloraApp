using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Database;
using FloraApp.Services.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FloraApp.Services.Services
{
    public abstract class BaseService<T, TSearch, TEntity> : IService<T, TSearch> 
        where T : class 
        where TSearch : BaseSearchObject
        where TEntity : class
    {
        protected readonly FloraAppDbContext _context;
        protected readonly IMapper _mapper;
        
        public BaseService(FloraAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual async Task<PagedResult<T>> GetAsync(TSearch search)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            
            query = AddFilter(query, search);
            
            int? totalCount = null;
            if (search?.IncludeTotalCount == true){
                totalCount = await query.CountAsync();
            }

            if (search?.RetrieveAll != true)
            {
                // Apply Skip if Page is specified
                if (search?.Page.HasValue == true && search?.PageSize.HasValue == true)
                {
                    query = query.Skip(search.Page.Value * search.PageSize.Value);
                }
                
                // Apply Take if PageSize is specified (regardless of Page)
                if (search?.PageSize.HasValue == true)
                {
                    query = query.Take(search.PageSize.Value);
                }
            }
            var list = await query.ToListAsync();
            return new PagedResult<T>
            {   
                Items = list.Select(MapToResponse).ToList(),
                TotalCount = totalCount,
                Page = search?.Page,
                PageSize = search?.PageSize
            };
        }

        protected virtual IQueryable<TEntity> AddFilter(IQueryable<TEntity> query, TSearch search)
        {
            return query;
        }

        protected abstract T MapToResponse(TEntity entity);

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return null;
            }
            return MapToResponse(entity);
        }
       
    }
}