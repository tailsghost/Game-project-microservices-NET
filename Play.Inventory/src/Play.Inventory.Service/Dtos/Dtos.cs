using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Service.Dtos;

public class Dtos
{
    public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int Quantity);

    public record InventoryItemDto(Guid CatalogItemId, string Name, string Description, int Quantity, DateTimeOffset AcquiredDate);

    public record CatalogItemDto(Guid id, [Required] string Name, string Description);
}
