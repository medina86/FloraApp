using FloraApp.Model.Exceptions;
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

namespace FloraApp.Services.Services
{
    public class BlogPostService : BaseCrudService<BlogPostResponse, BlogPostSearchObject, BlogPost, BlogPostUpsertRequest, BlogPostUpsertRequest>, IBlogPostService
    {
        public BlogPostService(FloraAppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<BlogPostResponse> GetByIdAsync(int id)
        {
            var entity = await _context.BlogPosts
                .Include(bp => bp.BlogComments)
                .FirstOrDefaultAsync(bp => bp.Id == id);

            if (entity == null)
                return null!;

            var response = _mapper.Map<BlogPostResponse>(entity);
            response.CommentCount = entity.BlogComments?.Count ?? 0;

            return response;
        }

        public override async Task<PagedResult<BlogPostResponse>> GetAsync(BlogPostSearchObject search)
        {
            var query = _context.BlogPosts
                .Include(bp => bp.BlogComments)
                .AsQueryable();

            query = AddFilter(query, search);

            int? totalCount = null;
            if (search?.IncludeTotalCount == true)
            {
                totalCount = await query.CountAsync();
            }

            if (search?.Page.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.Page.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }

            var entities = await query.ToListAsync();
            var responses = entities.Select(entity =>
            {
                var response = _mapper.Map<BlogPostResponse>(entity);
                response.CommentCount = entity.BlogComments?.Count ?? 0;
                return response;
            }).ToList();

            return new PagedResult<BlogPostResponse>
            {
                Items = responses,
                TotalCount = totalCount,
                Page = search?.Page ?? 1,
                PageSize = search?.PageSize ?? 10
            };
        }

        public async Task<PagedResult<BlogPostResponse>> GetAllBlogPostsAsync()
        {
            var blogPosts = await _context.BlogPosts.ToListAsync();
            return new PagedResult<BlogPostResponse>
            {
                Items = blogPosts.Select(MapToResponse).ToList(),
                TotalCount = blogPosts.Count
            };
        }

        protected override IQueryable<BlogPost> AddFilter(IQueryable<BlogPost> query, BlogPostSearchObject search)
        {
            if (!string.IsNullOrEmpty(search?.Title))
            {
                query = query.Where(bp => bp.Title.Contains(search.Title));
            }

            if (!string.IsNullOrEmpty(search?.Content))
            {
                query = query.Where(bp => bp.Content.Contains(search.Content));
            }

            if (search?.CreatedFrom.HasValue == true)
            {
                query = query.Where(bp => bp.CreatedAt >= search.CreatedFrom.Value);
            }

            if (search?.CreatedTo.HasValue == true)
            {
                query = query.Where(bp => bp.CreatedAt <= search.CreatedTo.Value);
            }

            return query.OrderByDescending(bp => bp.CreatedAt);
        }

        protected override BlogPost MapInsertToEntity(BlogPost entity, BlogPostUpsertRequest request)
        {
            entity.Title = request.Title;
            entity.Content = request.Content;
            entity.ImageUrl = request.ImageUrl;
            entity.CreatedAt = DateTime.UtcNow;
            return entity;
        }

        protected override void MapUpdateToEntity(BlogPost entity, BlogPostUpsertRequest request)
        {
            entity.Title = request.Title;
            entity.Content = request.Content;
            entity.ImageUrl = request.ImageUrl;
        }

        protected override BlogPostResponse MapToResponse(BlogPost entity)
        {
            var response = _mapper.Map<BlogPostResponse>(entity);
            response.CommentCount = entity.BlogComments?.Count ?? 0;
            return response;
        }

        protected override Task BeforeInsert(BlogPost entity, BlogPostUpsertRequest request)
        {
            throw new UserExceptions("Operacija nije dozvoljena");
        }
    }
}