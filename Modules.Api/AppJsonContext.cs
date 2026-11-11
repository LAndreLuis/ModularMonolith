using System.Text.Json;
using System.Text.Json.Serialization;
using Modules.Catalog.Features.Products.GetAllProduct;
using Modules.Catalog.Features.Products.GetByIdProduct;
using Modules.Catalog.Features.Products.UpdateProduct;

namespace Host.Api;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(IEnumerable<GetAllProductDto>))]
[JsonSerializable(typeof(List<GetAllProductDto>))]
[JsonSerializable(typeof(UpdateProductDto))]
[JsonSerializable(typeof(GetByIdProductDto))]
[JsonSerializable(typeof(IEnumerable<GetByIdProductDto>))]
[JsonSerializable(typeof(List<GetByIdProductDto>))]
internal partial class AppJsonContext : JsonSerializerContext;