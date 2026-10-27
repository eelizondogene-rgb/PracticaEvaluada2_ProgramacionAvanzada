using System.Net;

namespace WebAvanzada3Cuatrimestre.Middleware
{
    public class MiddlewareGlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareGlobalExceptionHandler> _logger;

        public MiddlewareGlobalExceptionHandler(RequestDelegate next, ILogger<MiddlewareGlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }


        //Como se configura el middleware

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // cualquier error que pase aca, va ir a al catch
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ExceptionResponse response = exception switch
            {
                //Aqui se pueden agregar mas tipos de excepciones
                KeyNotFoundException => new ExceptionResponse(HttpStatusCode.NotFound, exception.Message + "No se ha encontrado del recurso"),
                UnauthorizedAccessException => new ExceptionResponse(HttpStatusCode.Unauthorized, exception.Message + "No tiene permisos para acceder a este recurso"),
                NotImplementedException => new ExceptionResponse(HttpStatusCode.NotImplemented, exception.Message + "la funcionalidad no está implementada"),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.")

            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);


        }
    }
}
