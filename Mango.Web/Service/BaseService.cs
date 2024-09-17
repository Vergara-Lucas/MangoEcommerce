using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using static Mango.Web.Utility.StaticDetail;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
			_tokenProvider = tokenProvider;

		}
        public async Task<ResponseDTO?> SendAsync(RequestDTO requestDTO, bool withBaerer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
                //mensaje http de peticion
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //token
                //le enviamos en la cabecera la autorizacion y el token
                if (withBaerer) {
                    var token = _tokenProvider.GetToken();
					message.Headers.Add("Authorization",$"Bearer {token}");
                }
                message.RequestUri = new Uri(requestDTO.Url);
                //data es un object si trae valor es un put o un post
                if (requestDTO.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDTO.Data), Encoding.UTF8, "application/json");
                }
                //mensaje http de respuesta
                HttpResponseMessage? apiResponse = null;
                //asigno el metodo segun cual sea
                switch (requestDTO.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                //le mando de forma asincronica a mi httpclient la peticion y se la seteo a mi respuesta
                apiResponse = await client.SendAsync(message);
                //Seteo el responseDTO segun el statuscode
                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Acceso denegado" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Sin autorización" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                        return apiResponseDTO;

                }
            }
            catch(Exception ex) { 
                var dto = new ResponseDTO { 
                    Message = ex.Message,
                    IsSuccess = false
                };
                return dto;
            }
        }
    }
}
