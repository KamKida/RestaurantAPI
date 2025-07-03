using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {

        private readonly ILogger _logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {

            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
               await next.Invoke(context);
            }
            catch (BadRequestException badRequestExeprion)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestExeprion.Message);
            }
            catch(NotFoundException nTex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(nTex.Message);
            }
            catch(ForbidExeption fEx)
            {
                context.Response.StatusCode = 403;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong.");
            }
        }
    }
}
