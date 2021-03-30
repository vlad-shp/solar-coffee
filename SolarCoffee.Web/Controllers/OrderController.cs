using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Customer;
using SolarCoffee.Services.Order;
using SolarCoffee.Services.Product;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class OrderController:ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(ILogger<ProductController> logger, IOrderService orderService, ICustomerService customerService)
        {
            _logger = logger;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost("/api/invoice")]
        public ActionResult GenerateNewOrder([FromBody] InvoiceModel invoice)
        {
            _logger.LogInformation("Generating invoice");

            var order = OrderMapper.SerializeInvoiceToOrder(invoice);
            _orderService.GenerateOpenOrder(order);
            order.Customer = _customerService.GetById(invoice.CustomerId);

            return Ok();
        }

        [HttpGet("/api/order")]
        public ActionResult GetOrders()
        {
            var orders = _orderService.GetOrders();
            var orderModels = OrderMapper.SerializingOrdersToViewModels(orders);
            return Ok(orderModels);
        }
        
        [HttpPatch("/api/order/complete/{id}")]
        public ActionResult MarkOrderComplete(int id)
        {
            _logger.LogInformation($"Making order {id} complete...");
            _orderService.MarkFulfilled(id);
            return Ok();
        }
    }
}