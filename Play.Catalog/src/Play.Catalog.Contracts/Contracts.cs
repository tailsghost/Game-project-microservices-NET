namespace Play.Catalog.Contracts;

public record CatalogItemCreated(Guid itemId, string name, string Description);
public record CatalogItemUpdated(Guid itemId, string name, string Description);
public record CatalogItemDeleted(Guid itemId);

