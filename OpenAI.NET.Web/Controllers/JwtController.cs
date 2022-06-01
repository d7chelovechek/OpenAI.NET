using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenAI.NET.Web.EntityFrameworkCore.Models;
using OpenAI.NET.Web.EntityFrameworkCore.Repositories;
using OpenAI.NET.Web.Models;
using OpenAI.NET.Web.Models.Jwt.Auth;
using OpenAI.NET.Web.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.Controllers
{
    public class JwtController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly UserRepository _userRepository;

        public JwtController(IConfiguration configuration, UserRepository userRepository)
        {
            _configuration = configuration;

            _userRepository = userRepository;
        }

        [HttpPost, Route("/Jwt/Auth"), AllowAnonymous]
        public async Task<IActionResult> AuthAsync(AuthRequestParameters request)
        {
            return await BuildAuthResponseAsync(request);
        }

        [NonAction]
        private async Task<IActionResult> BuildAuthResponseAsync(
            AuthRequestParameters request)
        {
            ResponseBuilder builder = new(typeof(AuthRequestParameters), request);
            Response response = builder.Build();

            if (response.Errors is not null)
            {
                return BadRequest(
                    JsonConvert.SerializeObject(response,
                    Formatting.Indented));
            }

            if (await GetIdentity(request) is (ClaimsIdentity, TimeSpan) identity)
            {
                DateTime date = DateTime.UtcNow;

                JwtSecurityToken jwt = new(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    identity.Item1.Claims,
                    date,
                    date.Add(identity.Item2),
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256));

                response.Body = new AuthResponseBody()
                {
                    Name = identity.Item1.Name,
                    Permission = identity.Item1.Claims.First(x => x.Type.Equals(identity.Item1.RoleClaimType)).Value,
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                    ExpirationDate = date.Add(identity.Item2).ToString("G")
                };

                return Content(JsonConvert.SerializeObject(
                    response,
                    Formatting.Indented));
            }
            else
            {
                builder.AddError(
                    "Authorization error",
                    "There's no user with such data");
            }

            if (response.Errors is not null)
            {
                return BadRequest(JsonConvert.SerializeObject(
                    response,
                    Formatting.Indented));
            }

            return Content(JsonConvert.SerializeObject(
                response,
                Formatting.Indented));
        }

        [NonAction]
        private async Task<(ClaimsIdentity, TimeSpan)> GetIdentity(AuthRequestParameters request)
        {
            List<User> users = await _userRepository.GetAllAsync();
            ClaimsIdentity claimsIdentity = null;
            TimeSpan tokenLifeTime = TimeSpan.Zero;

            if (users.FirstOrDefault(x => x.Name.Equals(request.Name) &&
                x.PasswordHash.Equals(Sha256.GetHash(request.Password))) is User user)
            {
                List<Claim> claims = new()
                {
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
                    new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name)
                };
                foreach (string permission in user.Permissions)
                {
                    claims.Add(new Claim(ClaimTypes.Role, permission));
                }

                claimsIdentity = new ClaimsIdentity(claims, "AccessToken");
                tokenLifeTime = user.TokenLifeTime;
            }

            return (claimsIdentity, tokenLifeTime);
        }
    }
}