﻿using ETicaret.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ETicaret.Application.DTOs.Token CreateAccessToken(int minute)
        {
            Application.DTOs.Token token = new();
            // SEcurityKey simetriği oluşturuluyor
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            // Şifrelenmiş kimlik oluşturuluyor
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            // oluşturulacak token ayarları yapılıyor
            token.Expiration = DateTime.UtcNow.AddMinutes(minute);
            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires:token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials:signingCredentials
                );

            //Token oluşturulunca sınıfından bir örnek alalım.
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
