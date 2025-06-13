using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloraAppWebAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class BlogPostController : BaseCrudController<BlogPostResponse, BlogPostSearchObject, BlogPostUpsertRequest, BlogPostUpsertRequest>
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService service) : base(service)
        {
            _blogPostService = service;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<BlogPostResponse>>> GetAll()
        {
            try
            {
                var result = await _blogPostService.GetAllBlogPostsAsync();
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}