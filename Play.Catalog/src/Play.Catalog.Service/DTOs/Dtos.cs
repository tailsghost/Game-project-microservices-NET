using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.DTOs;

public record ItemDto(Guid id, [Required] string Name, string Description, [Range(0, 1000)] decimal Price, DateTimeOffset CreateDate);

public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
