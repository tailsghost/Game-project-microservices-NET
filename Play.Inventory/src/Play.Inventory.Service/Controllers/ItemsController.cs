using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using static Play.Inventory.Service.Dtos.Dtos;

namespace Play.Inventory.Service.Controllers;

[Route("items")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly IRepository<InventoryItem> _repository;
    private readonly CatalogClient _catalogClient;

    public ItemsController(IRepository<InventoryItem> repository, CatalogClient client)
    {
        _repository = repository;
        _catalogClient = client;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetItem(Guid userId)
    {
        if (userId == Guid.Empty) return BadRequest();

        var catalogItems = await _catalogClient.GetCatalogItemsAsync();
        var inventoryItemEntities = await _repository.GetAllAsync(item => item.UserId.Equals(userId));

        var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem =>
        {
            var catalogItem = catalogItems.Single(catalogItem => catalogItem.id.Equals(inventoryItem.CatalogItemId));
            return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
        });

        return Ok(inventoryItemDtos);
    }

    [HttpPost]
    public async Task<ActionResult> PostItem(GrantItemsDto itemDto)
    {
        var inventoryItem = await _repository.GetAsync(
            item => item.UserId.Equals(itemDto.UserId) 
            && item.CatalogItemId.Equals(itemDto.CatalogItemId));

        if (inventoryItem == null)
        {
            inventoryItem = new InventoryItem
            {
                CatalogItemId = itemDto.CatalogItemId,
                UserId = itemDto.UserId,
                Quantity = itemDto.Quantity,
                AcquiredDate = DateTimeOffset.UtcNow,
            };

            await _repository.CreateAsync(inventoryItem);
        } else
        {
            inventoryItem.Quantity += itemDto.Quantity;
            await _repository.UpdateAsync(inventoryItem);
        }

        return Ok();
    }
}
