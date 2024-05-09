using Play.Catalog.Service.Data;
using Play.Catalog.Service.DTOs;

namespace Play.Catalog.Service;

public static class Extensions
{
    public static ItemDto AsDto(this Item item) =>
         new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreateDate);
    
}
