using Microsoft.AspNetCore.Mvc;
using FloraApp.Model.Responses;
using FloraApp.Model.Requests;
using FloraApp.Services.Interfaces;
using FloraApp.Model.SearchObjects;
using System.Collections.Generic;

namespace FloraAppWebAPI.Controllers
{
    [ApiController]
    
    [Route("api/[controller]")]
    public class ProductController : BaseCrudController<ProductResponse, ProductSearchObject, ProductUpsertRequest, ProductUpsertRequest>
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService service) : base(service)
        {
            _productService = service;
        }
    }
}
