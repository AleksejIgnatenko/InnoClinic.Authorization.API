using System.Text.Json;
using FluentValidation;
using InnoClinic.Authorization.Core.Exceptions;
using Serilog;

namespace InnoClinic.Authorization.API.Middlewares
{
    /// <summary>
    /// Middleware for handling exceptions that occur during the request processing pipeline.
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate in the request processing pipeline.</param>
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware to handle exceptions during the processing of HTTP requests.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                var statusCode = StatusCodes.Status400BadRequest;

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { error = ex.Errors });
                await context.Response.WriteAsync(result);
            }
            catch (ExceptionWithStatusCode ex)
            {
                var statusCode = (int)ex.HttpStatusCode;

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }
}