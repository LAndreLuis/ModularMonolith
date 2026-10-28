using Modules.Identity.Abstractions;

namespace Modules.Identity.Features.Identity.CreateLogin
{
    public sealed record CreateLoginCommand(string Email, string Password)
        : ICommand<LoginResult>;
}
