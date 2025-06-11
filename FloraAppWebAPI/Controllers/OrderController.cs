using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FloraAppWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : BaseCrudController<
        OrderResponse,
        OrderSearchObject,
        OrderInsertRequest,
        OrderUpdateRequest>
    {
        public OrderController(ICrudService<
            OrderResponse,
            OrderSearchObject,
            OrderInsertRequest,
            OrderUpdateRequest> service) : base(service)
        {
        }
    }
} 