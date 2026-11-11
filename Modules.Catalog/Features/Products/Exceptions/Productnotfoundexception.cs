namespace Modules.Catalog.Features.Products;

public sealed class ProductNotFoundException(Guid id)
    : Exception($"Product {id} not found.");