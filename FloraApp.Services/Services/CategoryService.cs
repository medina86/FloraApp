using FloraApp.Model.Exceptions;
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
    public class CategoryService : BaseCrudService <CategoryResponse, CategorySearchObject, Category, CategoryUpsertRequest, CategoryUpsertRequest>, ICategoryService
    {
        public CategoryService(FloraAppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Category> AddFilter(IQueryable<Category> query, CategorySearchObject search)
        {
            if (!string.IsNullOrWhiteSpace(search?.Name))
            {
                query = query.Where(c => c.Name.Contains(search.Name));
            }

            if (!string.IsNullOrWhiteSpace(search?.FTS))
            {
                query = query.Where(c => 
                    c.Name.Contains(search.FTS) || 
                    (c.Description != null && c.Description.Contains(search.FTS))
                );
            }

            return query;
        }

        protected override CategoryResponse MapToResponse(Category entity)
        {
            return _mapper.Map<CategoryResponse>(entity);
        }

        protected override Category MapInsertToEntity(Category entity, CategoryUpsertRequest request)
        {
            entity.Name = request.Name;
            entity.Description = request.Description;
            return entity;
        }

        protected override void MapUpdateToEntity(Category entity, CategoryUpsertRequest request)
        {
            entity.Name = request.Name;
            entity.Description = request.Description;
        }

        public async Task<PagedResult<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return new PagedResult<CategoryResponse>
            {
                Items = categories.Select(MapToResponse).ToList(),
                TotalCount = categories.Count
            };
        }
        protected override Task BeforeInsert(Category entity, CategoryUpsertRequest request)
        {
            throw new UserExceptions("Operacija nije dozvoljena");
        }
    }
} 