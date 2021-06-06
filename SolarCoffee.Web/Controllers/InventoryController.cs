using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryService> _logger;

        public InventoryController(
            ILogger<InventoryService> logger,
            IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpGet("/api/inventory")]
        public async Task<IActionResult> GetCurrentInventory()
        {
            _logger.LogInformation("Getting all inventory ...");
            var inventory = (await _inventoryService.GetCurrentInventoryAsync())
                .Select(pi => new ProductInventoryModel
                {
                    Id = pi.Id,
                    Product = ProductMapper.SerializeProductModel(pi.Product),
                    IdealQuantity = pi.IdealQuantity,
                    QuantityOnHand = pi.QuantityOnHand
                })
                .OrderBy(inv => inv.Product.Name)
                .ToList();

            return Ok(inventory);
        }

        //public ActionResult GetCurrentInventory()
        //{
        //    _logger.LogInformation("Getting all inventory ...");
        //    var inventory = _inventoryService.GetCurrentInventory()
        //        .Select(pi => new ProductInventoryModel
        //        {
        //            Id = pi.Id,
        //            Product = ProductMapper.SerializeProductModel(pi.Product),
        //            IdealQuantity = pi.IdealQuantity,
        //            QuantityOnHand = pi.QuantityOnHand
        //        })
        //        .OrderBy(inv => inv.Product.Name)
        //        .ToList();

        //    return Ok(inventory);
        //}

        [HttpPatch("/api/inventory")]
        public ActionResult UpdateInventory([FromBody] ShipmentModel shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Updating inventory for {shipment.ProductId} -" +
                                   $"Adjustment {shipment.Adjustment}");

            var id = shipment.ProductId;
            var adjustment = shipment.Adjustment;

            var inventory = _inventoryService.UpdateUnitsAvailable(id, adjustment);
            return Ok(inventory);
        }
    }
}