using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDTO?> RegisterAssync(RegistrationRequestDTO registrationRequestDTO);
        Task<ResponseDTO?> LoginAssync(LoginRequestDTO loginRequestDTO);
        
        Task<ResponseDTO?> AssignRoleAssync(RegistrationRequestDTO RegistrationRequestDTO);
    }
}
