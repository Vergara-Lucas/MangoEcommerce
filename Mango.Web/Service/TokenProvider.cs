using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetail.TokenCookie);
        }

        public string? GetToken()
        {
            string? token = null;
            bool? HasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticDetail.TokenCookie, out token);
            return HasToken is true? token : null;
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.TokenCookie,token);
            
        }
    }
}
