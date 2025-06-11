using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;

namespace FloraApp.Services.Interfaces
{
    public interface IOrderService : ICrudService<
        OrderResponse,
        OrderSearchObject,
        OrderInsertRequest,
        OrderUpdateRequest>
    {
    }
} 