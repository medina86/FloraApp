using MapsterMapper;
using FloraApp.Services.Database;
using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;

namespace FloraApp.Services.Services
{
    public class OrderItemService : BaseCrudService<
        OrderItemResponse,
        OrderItemSearchObject,
        OrderItem,
        OrderItemInsertRequest,
        OrderItemUpdateRequest>,
        IOrderItemService
    {
        public OrderItemService(FloraAppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override OrderItemResponse MapToResponse(OrderItem entity)
        {
            return _mapper.Map<OrderItemResponse>(entity);
        }
    }
} 