namespace Mango.Web.Models
{
    public class ResponseDTO //respuesta
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message {  get; set; } = string.Empty;
    }
}
