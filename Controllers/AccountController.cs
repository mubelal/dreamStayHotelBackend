using dreamStayHotel.DTO;
using dreamStayHotel.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dreamStayHotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO) 
        {
            var newUser = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PasswordHash = registerDTO.Password,
                UserName = registerDTO.UserName,
                FullName = registerDTO.FullName,
                Address = registerDTO.Address,
                PhoneNumber = registerDTO.PhoneNumber,
                DateOfBirth = registerDTO.DateOfBirth,
            };

            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null) return BadRequest("A felhasználó már létezik");

            var createUser = await userManager.CreateAsync(newUser!, registerDTO.Password);
            if (!createUser.Succeeded) return BadRequest("Nem sikerült létrehozni a felhasználót");

            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if(checkAdmin is  null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return Ok("A felhasználó sikeresen létrehozva");
            }

            var checkUser = await roleManager.FindByNameAsync("User");
            if (checkUser is null) await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

            await userManager.AddToRoleAsync(newUser, "User");
            return Ok("A felhasználó sikeresen létrehozva");
        }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        if (loginDTO is null) return BadRequest("Üres mező!");

        var getUser = await userManager.FindByNameAsync(loginDTO.UserName);
        if (getUser is null) return NotFound("A felhasználó nem található");
        
        bool checkPassword = await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
        if (!checkPassword) return BadRequest("Helytelen jelszó");

        var getUserRole = await userManager.GetRolesAsync(getUser);
        string token = GenerateToken(getUser.Id, getUser.UserName!, getUser.Email!, getUserRole.First());
        
        return Ok(token);
    }


    private string GenerateToken(string id, string userName, string email, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var credantials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var userClaims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, id),
        new Claim(ClaimTypes.Name, userName),
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.Role, role),
    };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credantials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    }
}
