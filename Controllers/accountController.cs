using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using userIdentity.Models;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //api/ValuesController/register
    public class account : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        public account(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , IConfiguration config, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel usermodel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appuser = new ApplicationUser();
                appuser.Email = usermodel.Email;
                appuser.UserName = usermodel.FullName;
                appuser.PasswordHash = usermodel.Password;

                IdentityResult result = await _userManager.CreateAsync(appuser, usermodel.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(appuser);
                    // URL encode the token to make it safe for URLs
                    var encodedToken = System.Web.HttpUtility.UrlEncode(token);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new
                    {
                        userId = appuser.Id,
                        token = encodedToken
                    }, Request.Scheme);

                    await _emailService.SendEmailAsync(appuser.Email, "Confirm your email",
                        $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>");

                    return Ok(new {
                        message = "Account added successfully, Please confirm your Email",
                       });
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return BadRequest("Invalid email confirmation parameters");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("User not found");

            // URL decode the token
            var decodedToken = System.Web.HttpUtility.UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            
            if (result.Succeeded)
                return Ok("Email confirmed successfully!");
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest($"Email confirmation failed: {errors}");
            }
        }

        [HttpPost("Login")]
        public async Task <IActionResult> Login(Login user) 
        {

            if (ModelState.IsValid)
            {
                ApplicationUser found_user =   await  _userManager.FindByNameAsync(user.username);
                if (found_user != null) {

                    if (!await _userManager.IsEmailConfirmedAsync(found_user))
                    {
                        ModelState.AddModelError("", "Please confirm your email before logging in.");
                        return Unauthorized("Email Not Confirmed");
                    }

                    bool found = await _userManager.CheckPasswordAsync(found_user, user.password);
                    if (found == true)
                    {
                        // Claims Token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, found_user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, found_user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await _userManager.GetRolesAsync(found_user);

                        foreach (var role in roles) {
                            claims.Add(new Claim( ClaimTypes.Role , role));
                        }
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

                        SigningCredentials signing = new SigningCredentials( securityKey,algorithm:SecurityAlgorithms.HmacSha256);
                        // Create token
                        JwtSecurityToken MYtoken = new JwtSecurityToken(
                            issuer: _config["JWT:ValidIssuer"], // url web api
                            audience: _config["JWT:ValidAudience"], // url consumer angular
                            claims: claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials:signing

                         );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(MYtoken),
                            expireation = MYtoken.ValidTo

                        });
                    }
                    return Unauthorized();

                }       
                
                return Unauthorized();

            }
            else { 
               return Unauthorized();
            }
        }

        [HttpPost("LogOut")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok("user signed out successifully, please delete the token");
        }
    }
}
