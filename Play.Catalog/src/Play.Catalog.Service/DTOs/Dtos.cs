using System;

namespace Play.Catalog.Service.DTOs;

public record ItemDto(Guid id, string Name, string Description, decimal Price, DateTimeOffset CreateDate);

public record CreateItemDto(string Name, string Description, decimal Price);

public record UpdateItemDto(string Name, string Description, decimal Price);
