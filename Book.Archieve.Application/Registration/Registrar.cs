using Book.Archieve.Application.Validator;
using Book.Archieve.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book.Archieve.Application.Repo.UserGroup;
using Book.Archieve.Application.UnitOfWork;
using Book.Archieve.Application.Definition;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Book.Archieve.Application.Repo.BookGroup;

namespace Book.Archieve.Application.Registration
{
    public static class Registrar
    {
        public static IServiceCollection RegisterBookArchieve(this IServiceCollection services, string connectionString)
        {

            if (string.IsNullOrWhiteSpace(JWTDefinition.Issuer) || string.IsNullOrWhiteSpace(JWTDefinition.Key))
                throw new Exception("JWT için Issuer ve Key bilgileri gerekiyor");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JWTDefinition.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTDefinition.Key))
                    };
                });

            services.AddDbContext<BookContext>(
                 options => options.UseSqlServer(connectionString, optionsBuilder =>
                 {
                     optionsBuilder.MigrationsAssembly("Book.Archieve.Infrastructure");
                 }))
                .AddValidatorsFromAssemblyContaining<UserValidator>()
                .AddRepositories()
                .AddUnitOfWorks()
                ;

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return
                    services
                    // usergroup
                    .AddScoped<IUserRepository, UserRepository>()
                    .AddScoped<IAuthRepository, AuthRepository>()
                    // bookgroup
                    .AddScoped<IBookRepository, BookRepository>()
                    .AddScoped<INoteRepository, NoteRepository>()
                    ;
        }

        private static IServiceCollection AddUnitOfWorks(this IServiceCollection services)
        {
            return services
                    .AddScoped<IUserUnitOfWork, UserUnitOfWork>()
                    .AddScoped<IBookUnitOfWork, BookUnitOfWork>()
                    ;
        }
    }
}
