using System.Text.Json;

namespace SimpleOtisWebAPI.Services
{
    public class CustomNotAcceptableMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomNotAcceptableMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            if(context.Response.StatusCode == StatusCodes.Status406NotAcceptable)
            {
                var acceptHeader = context.Request.Headers["Accept"].ToString();

                context.Response.ContentType = "application/json";

                var response = new
                {
                    Code = StatusCodes.Status406NotAcceptable,
                    Message = $"The Requested Format {acceptHeader} is not supported"
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
