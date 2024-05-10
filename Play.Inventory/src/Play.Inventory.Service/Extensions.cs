using Play.Inventory.Service.Entities;
using static Play.Inventory.Service.Dtos.Dtos;

namespace Play.Inventory.Service;

public static class Extensions
{
    public static InventoryItemDto AsDto(this InventoryItem item, string name, string description) =>
            new InventoryItemDto(item.CatalogItemId, name, description, item.Quantity, item.AcquiredDate);
    
}
