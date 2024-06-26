﻿using Core.Exceptions;
using Models.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Api.Pipeline
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;
        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
                        _logger.LogError(JsonConvert.SerializeObject(exception));
                        break;
                    case BadRequestException badRequestException:
                        apiResponse = BaseApiResponse.FailCondition(badRequestException.Errors, exception.Message, HttpStatusCode.BadRequest);
                        _logger.LogError(JsonConvert.SerializeObject(exception));   
                        break;

                        default:
                        apiResponse = BaseApiResponse.FailCondition("System can't handle this operation, Try a few minutes later!", HttpStatusCode.BadRequest);
                        _logger.LogError(JsonConvert.SerializeObject(exception));
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
