# Base de Práctica Programada 2 — Productos médicos

Se partió del repositorio `WebAvanzada3Cuatrimestre-master` del curso SC-701. Es una **base compartida**, no la práctica terminada.

## Incluye
- La solución MVC con proyectos Domain, Infrastructure y Application.
- `Categoria` y `Producto` (campos mínimos: `Id`, `Nombre` y `CategoriaId`), y relación uno a muchos.
- SQLite (`Data Source=productosmedicos.db`) y EF Core, igual que el ejemplo del profesor.
- `IRepository<T>` y `Repository<T>` del repositorio de clase, sin reescribir su implementación.
- Registro del repositorio genérico por inyección de dependencias.
- Middleware global de excepciones original del profesor.

## Trabajo pendiente, por integrante
1. **Persona 1 — Base común:** revisar/explicar DbContext, conexión SQLite, repositorio genérico y crear BD. Esta plantilla deja esa base preparada.
2. **Persona 2 — Categorías:** DTO, mapeo, servicio, controlador, vistas y validaciones del CRUD.
3. **Persona 3 — Productos:** DTO, mapeo, servicio, controlador, vistas y combo box de categorías.
4. **Persona 4 — Excepciones/validaciones:** probar y ajustar el middleware, validar asociación de productos y eliminación de categorías con productos, y casos de error.

**Coordinación:** Personas 2 y 3 deben registrar sus servicios en `Program.cs`, incorporar perfiles de AutoMapper y acordar quién cambia los archivos compartidos. La persona 3 consulta las categorías creadas por persona 2 para el combo box. No hay CRUD completos en esta plantilla.

## Ejecutar
Abrir `WebAvanzada3Cuatrimestre.slnx` con Visual Studio; en la carpeta del proyecto web ejecutar `dotnet restore` y `dotnet run`. Si todavía no existe la base, **se debe crear** antes de utilizar CRUD (por ejemplo, mediante migraciones de EF Core con el proyecto web de inicio y Infrastructure como proyecto de datos). No se configuró `EnsureCreated()` automático ni hay archivo .db incluido.

## Entrega final
El PDF pide un solo ZIP y un `readme.txt`/`.md` con integrantes, vínculo a GitHub si corresponde, arquitectura, paquetes NuGet y SOLID/patrones utilizados.
