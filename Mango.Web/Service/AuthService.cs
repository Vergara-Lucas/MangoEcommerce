using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AssignRoleAssync(RegistrationRequestDTO RegistrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = RegistrationRequestDTO,
                Url = StaticDetail.AuthAPIBase + "/api/auth/assignRole"
            });
        }

        public async Task<ResponseDTO?> LoginAssync(LoginRequestDTO loginRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = loginRequestDTO,
                Url = StaticDetail.AuthAPIBase + "/api/auth/login"
            });

        }

        public async Task<ResponseDTO?> RegisterAssync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = registrationRequestDTO,
                Url = StaticDetail.AuthAPIBase + "/api/auth/register"
            }, withBaerer: false);
        }
    }
}
