using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new ();
            return View(loginRequestDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            ResponseDTO? responseDTO = await _authService.LoginAssync(loginRequestDTO);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(responseDTO.Result));
                await SingInUser(loginResponseDTO);
                _tokenProvider.SetToken(loginResponseDTO.Token);
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["error"] = responseDTO.Message;
                return View(loginRequestDTO);
            }
        }
        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>(){
                new SelectListItem{Text= StaticDetail.RoleAdmin,Value=StaticDetail.RoleAdmin },
                new SelectListItem{Text= StaticDetail.RoleCustomer,Value=StaticDetail.RoleCustomer },
            };
            ViewBag.RoleList = roleList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO obj) { 
            
            ResponseDTO response = await _authService.RegisterAssync(obj);
            ResponseDTO assignRole;
            if (response.IsSuccess && response != null)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = StaticDetail.RoleCustomer;
                }
                assignRole = await _authService.AssignRoleAssync(obj);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else {
                TempData["error"] = response.Message;
            }
            var roleList = new List<SelectListItem>(){
                new SelectListItem{Text= StaticDetail.RoleAdmin,Value=StaticDetail.RoleAdmin },
                new SelectListItem{Text= StaticDetail.RoleCustomer,Value=StaticDetail.RoleCustomer },
            };
            ViewBag.RoleList = roleList;
            return View(obj);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index","Home");
        }
        private async Task SingInUser(LoginResponseDTO model) {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            //reclamar identidad
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));             
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            //agregamos el rol para poder hacer metodos solo para determinados roles
            //[Authorize(StaticDetail.RoleAdmin)] solo puede acceder ese rol
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
