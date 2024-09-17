namespace Mango.Web.Service.IService
{
    public interface ITokenProvider
    {
        public void ClearToken();
        public string? GetToken();
        public void SetToken(string token);
    }
}
