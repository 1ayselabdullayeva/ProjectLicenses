using System.Net;

namespace Models.Common
{
    public class BaseApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public Dictionary<string, IEnumerable<string>> Errors { get; set; }

        public static BaseApiResponse SuccessCondition(string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new BaseApiResponse
            {
                StatusCode = statusCode,
                Success = true,
                Message = message
            };
        }
        public static BaseApiResponse<T> SuccessCondition<T>(T data, string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
            where T : class
        {
            return new BaseApiResponse<T>
            {
                StatusCode = statusCode,
                Success = true,
                Message = message,
                Data = data
            };
        }
        public static BaseApiResponse FailCondition(string message = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new BaseApiResponse
            {
                StatusCode = statusCode,
                Success = false,
                Message = message
            };
        }
        public static BaseApiResponse FailCondition(Dictionary<string, IEnumerable<string>> errors, string message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new BaseApiResponse
            {
                StatusCode = statusCode,
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
    public class BaseApiResponse<T> : BaseApiResponse where T : class
    {
        public T Data { get; set; }
    }
}
