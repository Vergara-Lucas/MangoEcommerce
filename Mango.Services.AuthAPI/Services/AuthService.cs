using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Mango.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        //para manejar usuarios
        private readonly UserManager<ApplicationUser> _userManager;
        //para manejar Roles usamos el tipo generico
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AppDbContext db, IJwtTokenGenerator jwtTokenGenerator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.applicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user!=null) {
                //hacemos el metodo asinc en sinc .GetAwaiter().GetResult()
                //si no existe el rol lo creo y lo asigno, si existe el rol lo asigno
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult()) {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user,roleName);
                return true;
            }

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.applicationUsers.FirstOrDefault(u=> u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user,loginRequestDTO.Password);
            if (user == null || isValid == false) {
                return new LoginResponseDTO() { User = null, Token=""};
            }
            //encontro user generamos jwt token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDTO userDTO = new() { 
                Email = user.Email,
                ID = user.Id,
                Name = user.name,
                PhoneNumber = user.PhoneNumber
            };
            LoginResponseDTO loginResponseDTO = new() { 
                User = userDTO,
                Token = token
            };
            return loginResponseDTO;
        }
    

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                name = registrationRequestDTO.Name,
                PhoneNumber = registrationRequestDTO.PhoneNumber
            };

            try {
                //creo el user con el manejador de usuarios
                var result = await _userManager.CreateAsync(user,registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    
                    return "";
                }
                else {
                    return result.Errors.FirstOrDefault().Description;                
                }

            } catch (Exception ex) { }
            return "Error Encountered";
        }
    }
}
