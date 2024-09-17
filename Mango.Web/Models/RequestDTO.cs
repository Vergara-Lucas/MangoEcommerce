using static Mango.Web.Utility.StaticDetail;

namespace Mango.Web.Models
{
    public class RequestDTO //peticion
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; } 
        public string AccesToken { get; set; }
    }
}
