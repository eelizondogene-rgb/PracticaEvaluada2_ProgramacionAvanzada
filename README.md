# Productos médicos

Sistema web para administrar el catálogo de productos médicos y sus categorías. Permite consultar, registrar, actualizar y eliminar categorías y productos desde una interfaz ASP.NET Core MVC. Los datos se guardan en SQLite.

Práctica Programada 2 — Programación Avanzada (SC-701).

## Integrantes

- Gustavo Montoya Bolaños
- Valeria Brenes Quesada
- Andreina Bolaños Valverde
- Genesis Gonzalez Elizondo

## Repositorio

https://github.com/eelizondogene-rgb/PracticaEvaluada2_ProgramacionAvanzada

## Arquitectura

La solución `WebAvanzada3Cuatrimestre.slnx` separa el sistema en cuatro proyectos. Las dependencias van hacia el dominio: Domain no referencia a nadie, Application solo referencia a Domain, Infrastructure referencia a Application y Domain, y el proyecto web es la raíz de composición.

| Proyecto | Responsabilidad |
| --- | --- |
| `WebAvanzada3Cuatrimestre.Domain` | Entidades `Categoria` y `Producto`, y la regla `ValidarReglaNegocioNombreRequerido`. |
| `WebAvanzada3Cuatrimestre.Application` | Casos de uso, DTOs, `Respuesta<T>`, interfaces de repositorio y el perfil de AutoMapper `MapeoClases`. |
| `WebAvanzada3Cuatrimestre.Infrastructure` | `ApplicationDbContext`, migraciones SQLite, `Repository<T>` y los repositorios concretos. |
| `WebAvanzada3Cuatrimestre` | Controladores, vistas, archivos estáticos, configuración y registro de dependencias en `Program.cs`. |

El flujo de una operación es: la vista envía la petición al controlador, el controlador llama al servicio mediante su interfaz, el servicio aplica las reglas y usa el repositorio, y el repositorio persiste con Entity Framework Core.

## Paquetes NuGet

Versiones declaradas en los archivos `.csproj`. Domain no tiene paquetes.

| Paquete | Versión | Proyecto |
| --- | --- | --- |
| AutoMapper | 16.2.0 | Application |
| Microsoft.EntityFrameworkCore | 10.0.12 | Infrastructure |
| Microsoft.EntityFrameworkCore.Sqlite | 10.0.12 | Infrastructure |
| Microsoft.EntityFrameworkCore.Design | 10.0.12 | Infrastructure y Web |
| Microsoft.EntityFrameworkCore.Tools | 10.0.12 | Infrastructure |

Los tres proyectos de código y el proyecto web usan `net10.0`.

La interfaz también carga Bootstrap Icons 1.11.3, DataTables 3.1.3 y SweetAlert2 desde CDN, además de Bootstrap y jQuery incluidos en `wwwroot/lib`. Esas bibliotecas no son paquetes NuGet de la solución.

## SOLID y patrones

**Responsabilidad única.** Cada controlador solo coordina HTTP. `CategoriaController` depende de `ICategoriaServicio` y `ProductoController` de `IProductoServicio` e `ICategoriaServicio`. Las reglas de nombre, longitud, duplicados y categoría asociada están en los servicios, no en los controladores ni en los repositorios.

**Abierto/cerrado.** `Repository<T>` concentra el CRUD común y expone `Consulta()` para ampliar la consulta sin copiar el repositorio. `ProductoRepository` lo sobreescribe para incluir la categoría en la misma consulta.

**Sustitución de Liskov.** `CategoriaRepository` y `ProductoRepository` heredan `Repository<T>`. `CategoriaRepository` no sobrescribe `GetByIdAsync` ni `UpdateAsync`; sus métodos propios tienen otro resultado: `GetCategoriaByIdAsync` devuelve una categoría vacía cuando el identificador no es positivo, y `UpdateCategoriaAsync` devuelve verdadero sin guardar cuando el nombre no cambió. `ProductoRepository` sí sobrescribe `Consulta()`, así que las lecturas heredadas incluyen la categoría. El código muestra esa herencia y esa reutilización. No hay una prueba ni un uso que sustituya un repositorio por el otro, así que este documento no afirma que el principio quede demostrado.

**Segregación de interfaces.** `ICategoriaRepository` e `IProductoRepository` viven en Application y declaran solo las operaciones de su módulo. No heredan `IRepository<T>`.

**Inversión de dependencias.** Application define las interfaces que Infrastructure implementa. `Program.cs` registra las implementaciones concretas:

```csharp
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();
```

Patrones usados en el código:

- Repositorio genérico y repositorios específicos.
- Inyección de dependencias en el contenedor de ASP.NET Core.
- DTO (`CategoriaDto`, `ProductoDto`) para no exponer las entidades a las vistas.
- AutoMapper en `MapeoClases`, incluido el nombre de la categoría dentro del producto.
- Objeto de resultado `Respuesta<T>` para las reglas de negocio, con `EsCorrecto`, `Mensaje`, `Codigo` y `Dato`.
- Middleware para las excepciones no controladas.
- MVC, con pantallas en Razor y operaciones de tabla por JSON.

## Requisitos

- SDK de .NET 10.
- `dotnet-ef` de la línea 10, compatible con Entity Framework Core 10.0.12 de los `.csproj`, solo para aplicar migraciones desde la consola.
- Visual Studio con la carga de trabajo de ASP.NET Core, o la CLI de .NET.

El archivo `productosmedicos.db` no forma parte del repositorio: `*.db` está en `.gitignore`. Hay que crearlo con la migración incluida, `20261027160751_InicialCategoriasProductos`.

## Ejecución

La cadena `DefaultConnection` está solo en `WebAvanzada3Cuatrimestre/appsettings.json` y vale `Data Source=productosmedicos.db`. `appsettings.Development.json` no la cambia. No existe una fábrica de diseño ni un `workingDirectory` en `launchSettings.json`. SQLite resuelve esa ruta relativa desde el directorio de trabajo del proceso.

Por eso `dotnet ef` y `dotnet run` se ejecutan dentro de `WebAvanzada3Cuatrimestre`. Desde ahí, la base queda en:

`WebAvanzada3Cuatrimestre/productosmedicos.db`

Ejecutar `dotnet run --project WebAvanzada3Cuatrimestre` desde la raíz del repositorio dejaría el archivo en ese otro directorio, distinto del que usa el proyecto web.

Si `dotnet ef` no está disponible, o `dotnet ef --version` no muestra una versión 10, instalar la herramienta una sola vez:

```bash
dotnet tool install --global dotnet-ef --version 10.0.12
```

Desde la raíz del repositorio:

```bash
cd WebAvanzada3Cuatrimestre
dotnet restore ../WebAvanzada3Cuatrimestre.slnx
dotnet ef database update --project ../WebAvanzada3Cuatrimestre.Infrastructure/WebAvanzada3Cuatrimestre.Infrastructure.csproj --startup-project .
dotnet run --launch-profile https
```

El perfil `https` abre `https://localhost:7092` y también escucha `http://localhost:5003`. El perfil `http` usa solo `http://localhost:5003`.

En Visual Studio se abre `WebAvanzada3Cuatrimestre.slnx` y se ejecuta el proyecto web. Como el perfil no define otro directorio de trabajo, el depurador usa la carpeta de ese proyecto y abre el mismo `productosmedicos.db`.

## Funcionalidades

### Categorías

- Listado, alta, edición y eliminación desde la pantalla Categorías.
- La tabla se carga con `GET /Categoria/ObtenerCategorias`.
- El alta y la edición envían `CategoriaDto`. Si el modelo no es válido, el controlador responde `BadRequest`; si la regla de negocio falla, responde `Respuesta<T>`.
- El nombre es obligatorio. Después de recortar espacios, no puede superar 100 caracteres.
- No se permiten dos categorías con el mismo nombre, sin distinguir mayúsculas y minúsculas.
- No se elimina una categoría que tenga productos. La relación en `ApplicationDbContext` usa `DeleteBehavior.Restrict` y el servicio responde: «No se puede eliminar la categoría porque tiene productos asociados.»
- La eliminación se solicita con `DELETE /Categoria/EliminarCategoria`.

### Productos

- Listado, alta, edición y eliminación desde la pantalla Productos.
- El listado incluye el nombre de la categoría (`CategoriaNombre`).
- El formulario ofrece las categorías existentes en un combo. El índice las carga con `ICategoriaServicio`.
- El nombre es obligatorio y, ya recortado, no supera 150 caracteres.
- La categoría elegida debe existir. Si no, el servicio responde: «Debe seleccionar una categoría existente.»
- Crear, actualizar y eliminar exigen el token antifalsificación.
- La eliminación se solicita con `POST /Producto/EliminarProducto`.

## Manejo global de excepciones

`MiddlewareGlobalExceptionHandler` rodea el resto del pipeline. Registra la excepción con el método, la ruta y el identificador de seguimiento, sin devolver el mensaje interno al cliente.

Para una petición AJAX (`X-Requested-With: XMLHttpRequest`) o que acepta `application/json` o un tipo `+json`, escribe JSON con `ExceptionResponse`:

| Excepción | Estado | Mensaje |
| --- | --- | --- |
| `KeyNotFoundException` | 404 | No se ha encontrado el recurso. |
| `UnauthorizedAccessException` | 403 | No tiene permisos para acceder a este recurso. |
| `NotImplementedException` | 501 | La funcionalidad no está implementada. |
| `BadHttpRequestException` | El estado de esa excepción | La solicitud HTTP no es válida. |
| Cualquier otra | 500 | Ocurrió un error inesperado. |

Si el cliente canceló la solicitud, el middleware no escribe un cuerpo de error. Si la respuesta ya comenzó, tampoco cambia el estado ni el cuerpo. En los demás casos de una página HTML, vuelve a lanzar la excepción: en Development la muestra la página de diagnóstico del framework y, fuera de Development, `UseExceptionHandler` la envía a `/Home/Error`.

Las validaciones de negocio no pasan por este middleware. Los servicios las devuelven en `Respuesta<T>`.
