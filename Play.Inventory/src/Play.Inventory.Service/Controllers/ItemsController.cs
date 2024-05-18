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
    private readonly IRepository<InventoryItem> _inventoryRepository;
    private readonly IRepository<CatalogItem> _catalogRepository;


    public ItemsController(IRepository<InventoryItem> inventoryRepository, IRepository<CatalogItem> catalogRepository)
    {
        _inventoryRepository = inventoryRepository;
        _catalogRepository = catalogRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetItem(Guid userId)
    {
        if (userId == Guid.Empty) return BadRequest();

        
        var inventoryItemEntities = await _inventoryRepository.GetAllAsync(item => item.UserId.Equals(userId));
        var itemIds = inventoryItemEntities.Select(item => item.CatalogItemId);
        var catalogItemEntities = await _catalogRepository.GetAllAsync(item => itemIds.Contains(item.Id));

        var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem =>
        {
            var catalogItem = catalogItemEntities.Single(catalogItem => catalogItem.Id.Equals(inventoryItem.CatalogItemId));
            return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
        });

        return Ok(inventoryItemDtos);
    }

    [HttpPost]
    public async Task<ActionResult> PostItem(GrantItemsDto itemDto)
    {
        var inventoryItem = await _inventoryRepository.GetAsync(
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

            await _inventoryRepository.CreateAsync(inventoryItem);
        } else
        {
            inventoryItem.Quantity += itemDto.Quantity;
            await _inventoryRepository.UpdateAsync(inventoryItem);
        }

        return Ok();
    }
}
