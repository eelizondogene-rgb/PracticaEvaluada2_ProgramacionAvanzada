using System.Net;

namespace WebAvanzada3Cuatrimestre.Middleware
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description); //Codigo 500, 404, 400, 401, 403, 200, 201, 202, 204, y el que paso
}
