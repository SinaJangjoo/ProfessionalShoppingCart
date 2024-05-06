using System.Net;

namespace TestD.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Error = new List<string>();  //To handle an empty List of ErrorMessages
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Error { get; set; }
        public object Result { get; set; }
    }
}
