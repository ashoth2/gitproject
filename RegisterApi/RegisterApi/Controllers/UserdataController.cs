using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RegisterApi.Models;
using RegisterApi.Role;
using RegisterApi.Services;
using RegisterApiDAL.EFModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace RegisterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserdataController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly UserManager<IdentityUser> usermanager;

        public UserdataController(IConfiguration configuration,IUserService userService,UserManager<IdentityUser>usermanager,ILogger<UserdataController>logg)
        {
            this.configuration = configuration;
            this.userService = userService;
            this.usermanager = usermanager;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult>Register(RegisterModel registerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                UserModel usermodel = new UserModel();
                 usermodel.UserName = registerModel.FirstName + "" + registerModel.LastName;
                usermodel.Email = registerModel.Email;

                var result = await usermanager.CreateAsync(usermodel,registerModel.Password);
                if(result.Succeeded)
                {
                    var register = await userService.Register(registerModel, Enumeration.User.ToString());
                    if (register.StatusCode == StatusCodes.Status200OK)
                        return Ok(register);
                    else
                        return BadRequest(register);
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpPost]
        [Route("[action]")]
        public async Task <IActionResult> Login(LoginModel login)
        {
            if(ModelState.IsValid)
            {
                var checkuser = await usermanager.FindByNameAsync(login.UserName);

                if (checkuser == null)
                {
                    return BadRequest(new { status = "Error", Message = "UserNotExist" });
                }
                else if (await usermanager.CheckPasswordAsync(checkuser, login.Password))
                {
                    var userroles = await usermanager.GetRolesAsync(checkuser);
                    var authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, checkuser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }; 

                    foreach (var role in userroles)
                    {
                        authClaim.Add(new Claim(ClaimTypes.Role, role));
                    }
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
                    var token = new JwtSecurityToken(
                       issuer: configuration["AppSettings:ValidIssuer"],
                       audience: configuration["AppSettings:ValidAudience"],
                       expires: DateTime.Now.AddMinutes(10),
                       claims: authClaim,
                       signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                       );
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                else
                {
                    return BadRequest(new { status = "Error", Message = "InvalidUser" });
                }
            }
            else
                return BadRequest(ModelState);

            }

       

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Registration>> GetUser()
        {

            try
            {
                return Ok(await userService.GetUser());
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel { StatusCode = StatusCodes.Status500InternalServerError, Message = e.Message });
            }

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Registration>> GetByIdUser(int UserId)
        {
            var data = await userService.GetUser(UserId);
            return Ok(data);
        }


    }
 }
        
    

