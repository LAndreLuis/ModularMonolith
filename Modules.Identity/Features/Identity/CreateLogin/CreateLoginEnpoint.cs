using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Identity.Abstractions;

namespace Modules.Identity.Features.Identity.CreateLogin
{
    public static class CreateLoginEndpoint
    {
        internal static void MapCreateLoginEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login",
                async (CreateLoginCommand command,
                       ICommandHandler<CreateLoginCommand, LoginResult> handler,
                       CancellationToken ct) =>
                {
                    var result = await handler.HandleAsync(command, ct);

                    if (!result.Succeeded)
                        return Results.Unauthorized();

                    return Results.Ok(new { token = result.Token });
                })
            .AllowAnonymous();   // ⬅️ login NÃO exige token
        }
    }
}