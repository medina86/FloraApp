using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;

namespace FloraApp.Services.Interfaces
{
    public interface IBlogCommentService : ICrudService<BlogCommentResponse, BlogCommentSearchObject, BlogCommentUpsertRequest, BlogCommentUpsertRequest>
    {
        Task<PagedResult<BlogCommentResponse>> GetCommentsByBlogPostIdAsync(int blogPostId, BlogCommentSearchObject search);
        Task<PagedResult<BlogCommentResponse>> GetAllBlogCommentsAsync();
    }
} 