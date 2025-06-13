using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;

namespace FloraApp.Services.Interfaces
{
    public interface IBlogPostService : ICrudService<BlogPostResponse, BlogPostSearchObject, BlogPostUpsertRequest, BlogPostUpsertRequest>
    {
        Task<PagedResult<BlogPostResponse>> GetAllBlogPostsAsync();
    }
} 