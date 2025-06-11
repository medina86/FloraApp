using MapsterMapper;
using FloraApp.Services.Database;
using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;

namespace FloraApp.Services.Services
{
    public class OrderService : BaseCrudService<
        OrderResponse,
        OrderSearchObject,
        Order,
        OrderInsertRequest,
        OrderUpdateRequest>,
        IOrderService
    {
        public OrderService(FloraAppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override OrderResponse MapToResponse(Order entity)
        {
            return _mapper.Map<OrderResponse>(entity);
        }
    }
} 