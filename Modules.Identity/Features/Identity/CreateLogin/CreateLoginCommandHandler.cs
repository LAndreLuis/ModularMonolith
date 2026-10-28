using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modules.Identity.Abstractions;
using Modules.Identity.Data;
using Modules.Identity.Services;

namespace Modules.Identity.Features.Identity.CreateLogin
{
    public sealed class CreateLoginCommandHandler
        : ICommandHandler<CreateLoginCommand, LoginResult>
    {
        private readonly IdentityDbContext _context;
        private readonly IConfiguration _config;

        public CreateLoginCommandHandler(IdentityDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginResult> HandleAsync(
            CreateLoginCommand command,
            CancellationToken cancellationToken = default)
        {
            // Busca o usuário sem restrição de tenant na consulta inicial do login
            var user = await _context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

            if (user is null || !PasswordHasherService.VerifyPassword(user.PasswordHash, command.Password))
                return LoginResult.Failure("Credenciais inválidas.");

            // Gera o JWT contendo a claim tenant_id lida pelo Finbuckle
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new("tenant_id", user.TenantId.ToString()) // Finbuckle usará essa claim
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return LoginResult.Success(jwt);
        }
    }


public sealed record LoginResult(bool Succeeded, string? Token, string? Error)
    {
        public static LoginResult Success(string token) => new(true, token, null);
        public static LoginResult Failure(string error) => new(false, null, error);
    }
}