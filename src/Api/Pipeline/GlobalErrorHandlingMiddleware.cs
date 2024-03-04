using Core.Exceptions;
using Models.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Api.Pipeline
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                #region scope for db
                //using (var scope = context.RequestServices.CreateScope())
                //{
                //	var _dbContext = scope.ServiceProvider.GetRequiredService<>();
                //}
                #endregion
                var apiResponse = new BaseApiResponse();
                switch (exception)
                {
                    case ResourceNotFoundException:
                        apiResponse = BaseApiResponse.FailCondition(exception.Message, HttpStatusCode.NotFound);
                        break;
                    case BadRequestException badRequestException:
                        apiResponse = BaseApiResponse.FailCondition(badRequestException.Errors, exception.Message, HttpStatusCode.BadRequest);
                        break;
                        //case UnHandledException:
                        //default:
                        apiResponse = BaseApiResponse.FailCondition("System can't handle this operation, Try a few minutes later!", HttpStatusCode.BadRequest);

                        break;
                }
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)apiResponse.StatusCode;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy(),
                    },
                    NullValueHandling = NullValueHandling.Ignore
                }));
            }
        }
    }
    public static class GlobalErrorHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseGlobalErrorHandlingMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }
    }
}
