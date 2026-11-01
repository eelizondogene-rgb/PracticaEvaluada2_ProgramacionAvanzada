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
                if (DebeResponderDesdeElMiddleware(context, ex))
                {
                    await HandleExceptionAsync(context, ex);
                    return;
                }

                throw;
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var metodo = context.Request.Method;
            var ruta = context.Request.Path.Value ?? "/";
            var traceIdentifier = context.TraceIdentifier;

            if (exception is OperationCanceledException && context.RequestAborted.IsCancellationRequested)
            {
                _logger.LogInformation(
                    exception,
                    "Solicitud cancelada por el cliente. Método: {Method}, Ruta: {Path}, TraceIdentifier: {TraceIdentifier}",
                    metodo,
                    ruta,
                    traceIdentifier);
                return;
            }

            _logger.LogError(
                exception,
                "Excepción no controlada. Método: {Method}, Ruta: {Path}, TraceIdentifier: {TraceIdentifier}",
                metodo,
                ruta,
                traceIdentifier);

            if (context.Response.HasStarted)
            {
                _logger.LogWarning(
                    "La respuesta ya había iniciado. No se modificó el estado HTTP ni se escribió el cuerpo. Método: {Method}, Ruta: {Path}, TraceIdentifier: {TraceIdentifier}",
                    metodo,
                    ruta,
                    traceIdentifier);
                return;
            }

            ExceptionResponse response = exception switch
            {
                KeyNotFoundException => new ExceptionResponse(HttpStatusCode.NotFound, "No se ha encontrado el recurso."),
                UnauthorizedAccessException => new ExceptionResponse(HttpStatusCode.Forbidden, "No tiene permisos para acceder a este recurso."),
                NotImplementedException => new ExceptionResponse(HttpStatusCode.NotImplemented, "La funcionalidad no está implementada."),
                BadHttpRequestException solicitudInvalida => new ExceptionResponse((HttpStatusCode)solicitudInvalida.StatusCode, "La solicitud HTTP no es válida."),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }

        private static bool DebeResponderDesdeElMiddleware(HttpContext context, Exception exception)
        {
            if (exception is OperationCanceledException && context.RequestAborted.IsCancellationRequested)
            {
                return true;
            }

            if (context.Response.HasStarted)
            {
                return true;
            }

            return ClienteEsperaJson(context);
        }

        private static bool ClienteEsperaJson(HttpContext context)
        {
            if (EsSolicitudAjax(context))
            {
                return true;
            }

            foreach (var encabezado in context.Request.Headers.Accept)
            {
                if (string.IsNullOrEmpty(encabezado))
                {
                    continue;
                }

                if (EncabezadoAceptaJson(encabezado))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool EsSolicitudAjax(HttpContext context)
        {
            foreach (var valor in context.Request.Headers["X-Requested-With"])
            {
                if (string.Equals(valor, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool EncabezadoAceptaJson(string encabezado)
        {
            foreach (var tipo in encabezado.Split(','))
            {
                var mediaType = tipo.Split(';', 2)[0].Trim();
                if (mediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase)
                    || mediaType.EndsWith("+json", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
