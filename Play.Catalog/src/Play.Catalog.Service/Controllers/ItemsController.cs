using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;
using Play.Catalog.Service.Data;
using Play.Common;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    
    private readonly IRepository<Item> _itemsRepository;

    public ItemsController(IRepository<Item> repository)
    {
        _itemsRepository = repository;
    }


    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItems()
    {
        var items = (await _itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());
        return items;
    }

    [HttpGet("id")]
    public async Task<ActionResult<ItemDto>> GetItemById(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);

        if (item == null) return NotFound();

        return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostItem(CreateItemDto createItem)
    {
        var item = new Item
        {
            Name = createItem.Name,
            Description = createItem.Description,
            CreateDate = DateTimeOffset.UtcNow,
            Price = createItem.Price,
        };

        await _itemsRepository.CreateAsync(item);

        return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
    }

    [HttpPut("id")]
    public async Task<IActionResult> PutItem(Guid id, UpdateItemDto updateItemDto)
    {
        var existingItem = await _itemsRepository.GetAsync(id);

        if (existingItem == null) return NotFound();

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;

        await _itemsRepository.UpdateAsync(existingItem);

        return NoContent();

    }

    [HttpDelete("id")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);

        if (item == null) return NotFound();

        await _itemsRepository.RemoveAsync(item.Id);

        return NoContent();
    }
}
