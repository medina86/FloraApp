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
    public class BlogCommentService : BaseCrudService<BlogCommentResponse, BlogCommentSearchObject, BlogComment, BlogCommentUpsertRequest, BlogCommentUpsertRequest>, IBlogCommentService
    {
        public BlogCommentService(FloraAppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<BlogCommentResponse> GetByIdAsync(int id)
        {
            var entity = await _context.BlogComments
                .Include(bc => bc.User)
                .FirstOrDefaultAsync(bc => bc.Id == id);

            if (entity == null)
                return null!;

            var response = _mapper.Map<BlogCommentResponse>(entity);
            response.UserName = entity.User?.Username ?? "Unknown User";

            return response;
        }

        public override async Task<PagedResult<BlogCommentResponse>> GetAsync(BlogCommentSearchObject search)
        {
            var query = _context.BlogComments
                .Include(bc => bc.User)
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
                var response = _mapper.Map<BlogCommentResponse>(entity);
                response.UserName = entity.User?.Username ?? "Unknown User";
                return response;
            }).ToList();

            return new PagedResult<BlogCommentResponse>
            {
                Items = responses,
                TotalCount = totalCount,
                Page = search?.Page ?? 1,
                PageSize = search?.PageSize ?? 10
            };
        }

        public async Task<PagedResult<BlogCommentResponse>> GetCommentsByBlogPostIdAsync(int blogPostId, BlogCommentSearchObject search)
        {
            search.BlogPostId = blogPostId;
            return await GetAsync(search);
        }

        public async Task<PagedResult<BlogCommentResponse>> GetAllBlogCommentsAsync()
        {
            var blogComments = await _context.BlogComments.Include(bc => bc.User).ToListAsync();
            return new PagedResult<BlogCommentResponse>
            {
                Items = blogComments.Select(entity =>
                {
                    var response = _mapper.Map<BlogCommentResponse>(entity);
                    response.UserName = entity.User?.Username ?? "Unknown User";
                    return response;
                }).ToList(),
                TotalCount = blogComments.Count
            };
        }

        protected override IQueryable<BlogComment> AddFilter(IQueryable<BlogComment> query, BlogCommentSearchObject search)
        {
            if (search?.BlogPostId.HasValue == true)
            {
                query = query.Where(bc => bc.BlogPostId == search.BlogPostId.Value);
            }

            if (search?.UserId.HasValue == true)
            {
                query = query.Where(bc => bc.UserId == search.UserId.Value);
            }

            if (!string.IsNullOrEmpty(search?.Content))
            {
                query = query.Where(bc => bc.Content.Contains(search.Content));
            }

            if (search?.CreatedFrom.HasValue == true)
            {
                query = query.Where(bc => bc.CreatedAt >= search.CreatedFrom.Value);
            }

            if (search?.CreatedTo.HasValue == true)
            {
                query = query.Where(bc => bc.CreatedAt <= search.CreatedTo.Value);
            }

            return query.OrderByDescending(bc => bc.CreatedAt);
        }

        protected override BlogComment MapInsertToEntity(BlogComment entity, BlogCommentUpsertRequest request)
        {
            entity.BlogPostId = request.BlogPostId;
            entity.UserId = request.UserId;
            entity.Content = request.Content;
            entity.CreatedAt = DateTime.UtcNow;
            return entity;
        }

        protected override void MapUpdateToEntity(BlogComment entity, BlogCommentUpsertRequest request)
        {
            entity.BlogPostId = request.BlogPostId;
            entity.UserId = request.UserId;
            entity.Content = request.Content;
        }

        protected override BlogCommentResponse MapToResponse(BlogComment entity)
        {
            var response = _mapper.Map<BlogCommentResponse>(entity);
            response.UserName = entity.User?.Username ?? "Unknown User";
            return response;
        }

        protected override Task BeforeInsert(BlogComment entity, BlogCommentUpsertRequest request)
        {
            throw new UserExceptions("Operacija nije dozvoljena");
        }
    }
}