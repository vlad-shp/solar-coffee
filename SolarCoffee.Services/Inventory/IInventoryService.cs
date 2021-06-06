using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Inventory
{
    public interface IInventoryService
    {
        List<ProductInventory> GetCurrentInventory();
        Task<List<ProductInventory>> GetCurrentInventoryAsync();
        ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);
        ProductInventory GetProductById(int productId);
        List<ProductInventorySnapshot> GetSnapshotHistory();
    }
}