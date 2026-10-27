# Buenas prácticas aplicadas en el código

Este documento resume únicamente las buenas prácticas que actualmente están aplicadas en la solución `WebAvanzada3Cuatrimestre`.

## 1. Separación por capas

La solución está organizada en proyectos con responsabilidades diferenciadas:

- `WebAvanzada3Cuatrimestre.Domain`: entidades y reglas de negocio.
- `WebAvanzada3Cuatrimestre.Application`: servicios, DTOs y respuestas de aplicación.
- `WebAvanzada3Cuatrimestre.Infrastructure`: persistencia, `DbContext` y repositorios.
- `WebAvanzada3Cuatrimestre`: composición de la aplicación y configuración de ASP.NET Core.

Esta separación facilita el mantenimiento y evita concentrar toda la lógica en la interfaz web.

## 2. Interfaces para abstraer servicios y repositorios

Se utilizan interfaces para definir contratos:

- `ICarroServicio`
- `ICarroRepository`
- `IDuennoRepository`

Esto permite que las implementaciones puedan sustituirse y facilita la creación de pruebas unitarias mediante mocks o dobles de prueba.

## 3. Inyección de dependencias

Las dependencias se reciben mediante constructores, por ejemplo, el contexto en los repositorios y el repositorio y mapper en `CarroServicio`.

También se registran servicios y repositorios con alcance `Scoped` en `Program.cs`:

```csharp
builder.Services.AddScoped<ICarroRepository, CarroRepository>();
builder.Services.AddScoped<IDuennoRepository, DuennoRepository>();
builder.Services.AddScoped<ICarroServicio, CarroServicio>();
```

Esto evita crear manualmente las dependencias y permite que ASP.NET Core controle su ciclo de vida.

## 4. Uso de DTOs

La capa de aplicación utiliza `CarroDto` en sus contratos públicos en lugar de exponer directamente la entidad de dominio.

Esto permite separar:

- El modelo utilizado por la base de datos.
- El modelo utilizado por los servicios y la interfaz de usuario.

## 5. Uso de AutoMapper

`CarroServicio` utiliza AutoMapper para convertir entre entidades y DTOs:

- `CarroDto` a `Carro`.
- `Carro` a `CarroDto`.

Esto reduce código repetitivo de asignación de propiedades.

## 6. Operaciones asíncronas

Las operaciones de aplicación y persistencia utilizan métodos asíncronos, entre ellos:

- `ToListAsync`.
- `FindAsync`.
- `FirstOrDefaultAsync`.
- `AddAsync`.
- `SaveChangesAsync`.

Esto evita bloquear el hilo durante las operaciones de acceso a datos.

## 7. Uso de `CancellationToken`

Los servicios y repositorios reciben `CancellationToken` y lo propagan a las operaciones de Entity Framework Core.

Esto permite cancelar consultas o escrituras cuando la solicitud ya no es necesaria.

Ejemplo:

```csharp
await _carroRepository.UpdateCarroAsync(carroEntity, cancellationToken);
```

## 8. Consultas de solo lectura con `AsNoTracking`

Las consultas que no necesitan modificar las entidades utilizan `AsNoTracking()`:

```csharp
return await _context.Carros
	.AsNoTracking()
	.Include(c => c.FkduennoNavigation)
	.ToListAsync(cancellationToken);
```

Esto reduce el trabajo de seguimiento de cambios de Entity Framework Core y puede mejorar el rendimiento en consultas de lectura.

## 9. Validación de reglas de negocio en Domain

La regla de que solamente se permiten carros Ferrari está ubicada en la entidad `Carro`, en lugar de estar implementada directamente en el repositorio.

```csharp
public bool ValidarReglaNegocioSoloFerrari()
{
	return string.Equals(
		Marca,
		MarcaPermita,
		StringComparison.OrdinalIgnoreCase);
}
```

La comparación no distingue entre mayúsculas y minúsculas.

## 10. Uso de constantes para valores de negocio

La marca permitida se define mediante una constante:

```csharp
public const string MarcaPermita = "Ferrari";
```

Esto evita repetir el valor literal en diferentes lugares del código.

## 11. Validación antes de guardar datos

`CarroServicio` valida la regla de negocio antes de crear o actualizar un carro.

Si la validación falla, no se ejecuta la operación de persistencia y se devuelve una respuesta indicando el error.

## 12. Respuestas estandarizadas

Las operaciones de creación, actualización y eliminación utilizan `Respuesta<T>` para devolver una estructura común con:

- `EsCorrecto`.
- `Mensaje`.
- `Codigo`.
- `Dato`.

Esto permite que los consumidores de la capa de aplicación manejen las respuestas de forma uniforme.

## 13. Nombres de propiedades en PascalCase

Las propiedades públicas de `Respuesta<T>` siguen la convención de C#:

```csharp
public bool EsCorrecto { get; set; }
public string Mensaje { get; set; }
public int Codigo { get; set; }
public T Dato { get; set; }
```

## 14. Comprobación del resultado de `SaveChangesAsync`

Los repositorios retornan un `bool` según el número de registros afectados:

```csharp
return await _context.SaveChangesAsync(cancellationToken) > 0;
```

Esto permite a la capa de aplicación saber si la operación se ejecutó correctamente.

Esta práctica se aplica en las operaciones de `CarroRepository` y `DuennoRepository`.

## 15. Verificación de existencia antes de actualizar o eliminar

Antes de actualizar o eliminar, los repositorios verifican que la entidad exista.

Si no existe, retornan `false` y evitan ejecutar una operación inválida.

## 16. Actualización controlada de entidades

En las operaciones de actualización se consulta primero la entidad existente y después se asignan las propiedades permitidas:

```csharp
var duennoExistente = await _context.Duennos
	.FirstOrDefaultAsync(d => d.Id == duenno.Id, cancellationToken);
```

Después se actualizan los valores necesarios y se guarda el cambio.

## 17. Configuración de Entity Framework Core mediante inyección

`ApplicationDbContext` recibe `DbContextOptions` mediante el constructor y se registra en `Program.cs`:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlite(
		builder.Configuration.GetConnectionString("DefaultConnection")));
```

Esto permite configurar el proveedor de base de datos desde la composición de la aplicación.

## 18. Relaciones e índices configurados en Entity Framework Core

El contexto configura la relación entre `Carro` y `Duenno`, además de índices relacionados con la placa y el propietario.

También se define la restricción de unicidad para la placa del carro.

## 19. Nullable Reference Types habilitado

Los proyectos utilizan:

```xml
<Nullable>enable</Nullable>
```

Esto ayuda a detectar posibles referencias nulas durante la compilación.

## 20. Compilación verificada

Después de aplicar los cambios recientes, la solución fue compilada correctamente:

```text
Build successful
```

Esto confirma que las modificaciones realizadas mantienen la solución compilable.
