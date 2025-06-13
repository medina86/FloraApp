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
    public class BlogCommentController : BaseCrudController<BlogCommentResponse, BlogCommentSearchObject, BlogCommentUpsertRequest, BlogCommentUpsertRequest>
    {
        private readonly IBlogCommentService _blogCommentService;

        public BlogCommentController(IBlogCommentService service) : base(service)
        {
            _blogCommentService = service;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<BlogCommentResponse>>> GetAll()
        {
            try
            {
                var result = await _blogCommentService.GetAllBlogCommentsAsync();
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}