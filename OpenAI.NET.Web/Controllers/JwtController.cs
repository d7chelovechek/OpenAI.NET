using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenAI.NET.Models;
using OpenAI.NET.Models.Jwt.Add;
using OpenAI.NET.Models.Jwt.Auth;
using OpenAI.NET.Models.Jwt.Remove;
using OpenAI.NET.Web.EntityFrameworkCore.Models;
using OpenAI.NET.Web.EntityFrameworkCore.Repositories;
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

        [HttpPost, Route("/Jwt/Add"), Authorize(Roles = Permission.CanManageUsers)]
        public async Task<IActionResult> AddAsync(AddRequestParameters request)
        {
            return await BuildAddResponseAsync(request);
        }

        [HttpPost, Route("/Jwt/Remove"), Authorize(Roles = Permission.CanManageUsers)]
        public async Task<IActionResult> RemoveAsync(RemoveRequestParameters request)
        {
            return await BuildRemoveResponseAsync(request);
        }

        [NonAction]
        private async Task<IActionResult> BuildAuthResponseAsync(
            AuthRequestParameters request)
        {
            ResponseBuilder builder = new(typeof(AuthRequestParameters), request);
            Response response = builder.Build();

            if (response.Exceptions is not null)
            {
                return BadRequest(
                    JsonConvert.SerializeObject(response,
                    Formatting.Indented));
            }

            try
            {
                if (await GetClaimsIdentityAsync(request) is
                    (ClaimsIdentity, EntityFrameworkCore.Models.User) identity)
                {
                    DateTime date = DateTime.UtcNow;

                    JwtSecurityToken jwt = new(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        identity.Item1.Claims,
                        date,
                        date.Add(identity.Item2.TokenLifeTime),
                        new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                            SecurityAlgorithms.HmacSha256));

                    response.Body = new AuthResponseBody()
                    {
                        Name = identity.Item1.Name,
                        Permissions = identity.Item2.Permissions,
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                        ExpirationDate = date.Add(identity.Item2.TokenLifeTime).ToString("G")
                    };

                    return base.Content(JsonConvert.SerializeObject(
                        response,
                        Formatting.Indented));
                }
                else
                {
                    throw new Exception("There's no user with such data");
                }
            }
            catch (Exception ex)
            {
                builder.AddException(
                    "Exception in user authorization",
                    ex.Message);
            }

            if (response.Exceptions is not null)
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
        private async Task<IActionResult> BuildAddResponseAsync(
            AddRequestParameters request)
        {
            ResponseBuilder builder = new(typeof(AddRequestParameters), request);
            Response response = builder.Build();

            if (response.Exceptions is not null)
            {
                return BadRequest(
                    JsonConvert.SerializeObject(response,
                    Formatting.Indented));
            }

            try
            {
                if (await GetUserByNameAsync(request.Name) is null)
                {
                    if(!TimeSpan.TryParse(request.TokenLifeTime, out TimeSpan tokenLifeTime))
                    {
                        tokenLifeTime = TimeSpan.Zero;
                    }

                    User user = new()
                    {
                        Id = Guid.NewGuid(),
                        Name = request.Name,
                        PasswordHash = Sha256.GetHash(request.Password),
                        Permissions = request.Permissions,
                        TokenLifeTime = tokenLifeTime,
                    };
                    await _userRepository.AddAsync(user);

                    response.Body = new AddResponseBody()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Permissions = user.Permissions,
                        TokenLifeTime = user.TokenLifeTime.ToString("G")
                    };

                    return base.Content(JsonConvert.SerializeObject(
                        response,
                        Formatting.Indented));
                }
                else
                {
                    throw new Exception("User with this Name already exists");
                }
            }
            catch (Exception ex)
            {
                builder.AddException(
                    "Exception in adding new user",
                    ex.Message);
            }

            if (response.Exceptions is not null)
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
        private async Task<IActionResult> BuildRemoveResponseAsync(
            RemoveRequestParameters request)
        {
            ResponseBuilder builder = new(typeof(RemoveRequestParameters), request);
            Response response = builder.Build();

            if (response.Exceptions is not null)
            {
                return BadRequest(
                    JsonConvert.SerializeObject(response,
                    Formatting.Indented));
            }

            try
            {
                if (await GetUserByNameAsync(request.Name) is User user)
                {
                    if (user.Permissions.Contains(Permission.Untouchable))
                    {
                        if (User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role) &&
                            x.Value.Equals(Permission.Untouchable)) is null)
                        { 
                            throw new Exception("It isn't possible to delete Untouchable user");
                        }
                    }

                    await _userRepository.RemoveAsync(user);
                    
                    response.Body = new RemoveResponseBody()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Status = "Removing was successful"
                    };

                    return base.Content(JsonConvert.SerializeObject(
                        response,
                        Formatting.Indented));
                }
                else
                {
                    throw new Exception("There's no user with this name");
                }
            }
            catch (Exception ex)
            {
                builder.AddException(
                    "Exception in removing user",
                    ex.Message);
            }

            if (response.Exceptions is not null)
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
        private async Task<(ClaimsIdentity, User)> GetClaimsIdentityAsync(
            AuthRequestParameters request)
        {
            List<User> users = await _userRepository.GetAllAsync();

            if (users.FirstOrDefault(x => x.Name.Equals(request.Name) &&
                x.PasswordHash.Equals(Sha256.GetHash(request.Password))) is User user)
            {
                List<Claim> claims = new()
                {
                    new Claim(
                        JwtRegisteredClaimNames.Iss,
                        _configuration["Jwt:Issuer"]),
                    new Claim(
                        JwtRegisteredClaimNames.Aud,
                        _configuration["Jwt:Audience"]),
                    new Claim(
                        JwtRegisteredClaimNames.Sub,
                        _configuration["Jwt:Subject"]),
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        user.Id.ToString()),
                    new Claim(
                        ClaimTypes.Name,
                        user.Name)
                };
                foreach (string permission in user.Permissions)
                {
                    claims.Add(new Claim(ClaimTypes.Role, permission));
                }

                return (new ClaimsIdentity(claims, "AccessToken"), user);
            }

            return (null, null);
        }

        [NonAction]
        private async Task<User> GetUserByNameAsync(
            string userName)
        {
            return await _userRepository.GetUserByNameAsync(userName);
        }
    }
}