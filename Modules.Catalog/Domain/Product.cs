namespace Modules.Catalog.Domain
{
    public sealed class Product
    {
        public Guid Id { get; set; }
        public string TenantId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
