using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Database;
using FloraApp.Services.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FloraApp.Services.Services
{
    public class CustomBouquetService : BaseCrudService<CustomBouquetResponse, CustomBouquetSearchObject, CustomBouquet, CustomBouquetUpsertRequest, CustomBouquetUpsertRequest>, ICustomBouquetService
    {
        private readonly ICustomBouquetItemService _itemService;

        public CustomBouquetService(FloraAppDbContext context, IMapper mapper, ICustomBouquetItemService itemService) 
            : base(context, mapper)
        {
            _itemService = itemService;
        }

        protected override IQueryable<CustomBouquet> AddFilter(IQueryable<CustomBouquet> query, CustomBouquetSearchObject search)
        {
            if (search?.UserId != null)
            {
                query = query.Where(cb => cb.UserId == search.UserId);
            }

            if (search?.MinPrice != null)
            {
                query = query.Where(cb => cb.Price >= search.MinPrice);
            }

            if (search?.MaxPrice != null)
            {
                query = query.Where(cb => cb.Price <= search.MaxPrice);
            }

            if (!string.IsNullOrWhiteSpace(search?.Status))
            {
                query = query.Where(cb => cb.Status == search.Status);
            }

            if (search?.CreatedFrom != null)
            {
                query = query.Where(cb => cb.CreatedAt >= search.CreatedFrom);
            }

            if (search?.CreatedTo != null)
            {
                query = query.Where(cb => cb.CreatedAt <= search.CreatedTo);
            }

            if (!string.IsNullOrWhiteSpace(search?.FTS))
            {
                query = query.Where(cb =>
                    cb.Note != null && cb.Note.Contains(search.FTS) ||
                    cb.Status.Contains(search.FTS)
                );
            }

            return query;
        }

        public override async Task<PagedResult<CustomBouquetResponse>> GetAsync(CustomBouquetSearchObject search)
        {
            var query = _context.CustomBouquets
                .Include(cb => cb.User)
                .AsQueryable();
            
            query = AddFilter(query, search);
            
            int? totalCount = null;
            if (search?.IncludeTotalCount == true)
            {
                totalCount = await query.CountAsync();
            }

            if (search?.RetrieveAll != true)
            {
                if (search?.Page.HasValue == true && search?.PageSize.HasValue == true)
                {
                    query = query.Skip(search.Page.Value * search.PageSize.Value);
                }
                
                if (search?.PageSize.HasValue == true)
                {
                    query = query.Take(search.PageSize.Value);
                }
            }
            
            var list = await query.ToListAsync();
            var responses = new List<CustomBouquetResponse>();

            foreach (var bouquet in list)
            {
                var response = MapToResponse(bouquet);
                response.Items = await _itemService.GetByBouquetIdAsync(bouquet.Id);
                responses.Add(response);
            }

            return new PagedResult<CustomBouquetResponse>
            {   
                Items = responses,
                TotalCount = totalCount,
                Page = search?.Page,
                PageSize = search?.PageSize
            };
        }

        public async Task<PagedResult<CustomBouquetResponse>> GetByUserIdAsync(int userId)
        {
            var bouquets = await _context.CustomBouquets
                .Include(cb => cb.User)
                .Where(cb => cb.UserId == userId)
                .ToListAsync();

            var responses = new List<CustomBouquetResponse>();
            foreach (var bouquet in bouquets)
            {
                var response = MapToResponse(bouquet);
                response.Items = await _itemService.GetByBouquetIdAsync(bouquet.Id);
                responses.Add(response);
            }

            return new PagedResult<CustomBouquetResponse>
            {
                Items = responses,
                TotalCount = bouquets.Count
            };
        }

        protected override CustomBouquetResponse MapToResponse(CustomBouquet entity)
        {
            var response = _mapper.Map<CustomBouquetResponse>(entity);
            response.UserName = entity.User?.Username ?? string.Empty;
            return response;
        }

        protected override CustomBouquet MapInsertToEntity(CustomBouquet entity, CustomBouquetUpsertRequest request)
        {
            entity.UserId = request.UserId;
            entity.Price = 0; // Initial price is 0, will be updated when items are added
            entity.Note = request.Note;
            entity.Status = request.Status;
            entity.CreatedAt = DateTime.UtcNow;
            return entity;
        }

        protected override void MapUpdateToEntity(CustomBouquet entity, CustomBouquetUpsertRequest request)
        {
            entity.Note = request.Note;
            entity.Status = request.Status;
            // Price is managed by CustomBouquetItemService
        }
    }
} 