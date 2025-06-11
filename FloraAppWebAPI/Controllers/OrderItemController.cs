using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FloraAppWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : BaseCrudController<
        OrderItemResponse,
        OrderItemSearchObject,
        OrderItemInsertRequest,
        OrderItemUpdateRequest>
    {
        public OrderItemController(ICrudService<
            OrderItemResponse,
            OrderItemSearchObject,
            OrderItemInsertRequest,
            OrderItemUpdateRequest> service) : base(service)
        {
        }
    }
} 