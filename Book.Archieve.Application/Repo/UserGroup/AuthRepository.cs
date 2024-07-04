using Book.Archieve.Application.Definition;
using Book.Archieve.Domain.DTO.Request;
using Book.Archieve.Domain.Entity;
using Book.Archieve.Infrastructure.Context;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.Repo.UserGroup
{
    public class AuthRepository : RepositoryBase, IAuthRepository
    {

        public AuthRepository(BookContext bookContext) : base(bookContext)
        {

        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTDefinition.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.GivenName, user.FirstName),
        new Claim(ClaimTypes.Surname, user.LastName),
        new Claim(ClaimTypes.Name, user.FullName),
    };

            var token = new JwtSecurityToken(JWTDefinition.Issuer,
                JWTDefinition.Issuer,
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       
    }

    public interface IAuthRepository
    {
        string GenerateToken(User user);
    }
}
