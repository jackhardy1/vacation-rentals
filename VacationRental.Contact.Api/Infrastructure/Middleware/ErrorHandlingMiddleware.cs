using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using VacationRental.Contact.Api.Models.Error;

namespace VacationRental.Contact.Api.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (Exception ex)
            {
                var response = this.BuildErrors(ex);

                var result = JsonConvert.SerializeObject(response);

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = response.errors.FirstOrDefault()?.status ?? 500;

                await context.Response.WriteAsync(result);
            }
        }

        private ServerResponseFailureModel BuildErrors(Exception exception)
        {
            var response = new ServerResponseFailureModel();

            if (exception is NotFoundException)
            {
                var errorModel = new ServerErrorModel();

                errorModel.code = HttpStatuses.NotFound;

                errorModel.status = (int)HttpStatusCode.NotFound;
                
                response.errors.Add(errorModel);
            }

            if (exception is BadRequestException)
            {
                var errorModel = new ServerErrorModel();

                errorModel.code = HttpStatuses.BadRequest;

                errorModel.status = (int)HttpStatusCode.BadRequest;

                response.errors.Add(errorModel);
            }

            return response;
        }
    }

    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}

