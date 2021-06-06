using SolarCoffee.Data.Models;
using System.Collections.Generic;

namespace SolarCoffee.Services.Order
{
    public interface IOrderService
    {
        List<SalesOrder> GetOrders();
        ServiceResponse<bool> GenerateOpenOrder(SalesOrder order);
        ServiceResponse<bool> MarkFulfilled(int id);

    }
}