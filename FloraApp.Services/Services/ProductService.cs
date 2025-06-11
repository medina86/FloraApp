using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Database;
using FloraApp.Services.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FloraApp.Services.Services
{
    public class ProductService : BaseCrudService<ProductResponse, ProductSearchObject, Product, ProductUpsertRequest, ProductUpsertRequest>, IProductService
    {
        public ProductService(FloraAppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Product> AddFilter(IQueryable<Product> query, ProductSearchObject search)
        {
            if (search?.CategoryId != null)
            {
                query = query.Where(p => p.CategoryId == search.CategoryId);
            }

            if (search?.MinPrice != null)
            {
                query = query.Where(p => p.Price >= search.MinPrice);
            }

            if (search?.MaxPrice != null)
            {
                query = query.Where(p => p.Price <= search.MaxPrice);
            }

            if (!string.IsNullOrWhiteSpace(search?.FTS))
            {
                query = query.Where(p =>
                    p.Name.Contains(search.FTS) ||
                    p.Description.Contains(search.FTS)
                );
            }

            return query;
        }

        public override async Task<PagedResult<ProductResponse>> GetAsync(ProductSearchObject search)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();
            
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
            return new PagedResult<ProductResponse>
            {   
                Items = list.Select(MapToResponse).ToList(),
                TotalCount = totalCount,
                Page = search?.Page,
                PageSize = search?.PageSize
            };
        }

        public override async Task<ProductResponse> GetByIdAsync(int id)
        {
            var entity = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            if (entity == null)
            {
                return null;
            }
            
            return MapToResponse(entity);
        }

        protected override ProductResponse MapToResponse(Product entity)
        {
            return _mapper.Map<ProductResponse>(entity);
        }

        protected override Product MapInsertToEntity(Product entity, ProductUpsertRequest request)
        {
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Price = request.Price;
            entity.CategoryId = request.CategoryId;
            return entity;
        }

        protected override void MapUpdateToEntity(Product entity, ProductUpsertRequest request)
        {
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Price = request.Price;
            entity.CategoryId = request.CategoryId;
        }
    }
} 