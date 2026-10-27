# Instrucciones

# Mejoras de rendimiento
- Utilizar siempre metodos asincronos para mejorar el rendimiento y la escalabilidad de la aplicacion.
- No utilizar nulos en base de datos, en su lugar utilizar valores por defecto o tipos de datos que permitan representar la ausencia de valor.
- Utilizar Iqueryable en lugar de IEnumerable para mejorar el rendimiento de las consultas a la base de datos.
- Utilizar Astracking para mejorar el rendimiento de las consultas a la base de datos cuando no se necesita realizar seguimiento de los cambios en los objetos.

# Buenas practicas para escribir instrucciones claras y efectivas:

- Cuando crees una funcion asegurate de documentarla correctamente, incluyendo su propósito, parámetros y valor de retorno.
- Utiliza variables con nombres descriptivos y significativos para mejorar la legibilidad del código.
- Realiza siempre una auditorio del codigo para identificar posibles errores, vulnerabilidades y oportunidades de mejora.


# Buenas practicas de respuestas

- No aplicar los cambios directamente en el código, sino que se deben sugerir cambios y mejoras de manera clara y concisa por medio de funciones o planes.
- Utilizar fuentes confiables y actualizadas para respaldar las respuestas.

# Architectura Clean code

- Infrastructure:
  - Contiene la implementación de la infraestructura, como bases de datos, servicios externos y otros componentes que interactúan con el mundo exterior.
  - Debe ser independiente de la lógica de negocio y de la presentación.
  - Debe utilizar token de cancelación para mejorar el rendimiento y la escalabilidad de la aplicación.
- Aplication:
  - Contiene la lógica de negocio y las reglas de la aplicación.
  - Debe ser independiente de la infraestructura y de la presentación.
- Domain:
  - Contiene las entidades y objetos de valor que representan el modelo de negocio.
  - Debe ser independiente de la infraestructura, la aplicación y la presentación.
- Presentation:
  - Contiene la interfaz de usuario y la lógica de presentación.
  - Debe ser independiente de la infraestructura, la aplicación y el dominio.

# Comandos utiles
  - Scaffold desde SQLite (genera entidades y contexto):
    - `dotnet ef dbcontext scaffold "Data Source=C:\Users\richa\Desktop\Proyectos de Clase\WebAvanzada3Cuatrimestre\WebAvanzada3Cuatrimestre.Infrastructure\lavacar.db" Microsoft.EntityFrameworkCore.Sqlite --output-dir C:\Users\richa\Desktop\Proyectos de Clase\WebAvanzada3Cuatrimestre\WebAvanzada3Cuatrimestre.Domain\Entidades --context ApplicationDbContext --context-dir Data --force`
    - `Set-Location 'C:\Users\richa\Desktop\Proyectos de Clase\WebAvanzada3Cuatrimestre\WebAvanzada3Cuatrimestre.Infrastructure'; dotnet ef dbcontext scaffold "Data Source=C:\Users\richa\Desktop\Proyectos de Clase\WebAvanzada3Cuatrimestre\WebAvanzada3Cuatrimestre.Infrastructure\lavacar.db" Microsoft.EntityFrameworkCore.Sqlite --output-dir "C:\Users\richa\Desktop\Proyectos de Clase\WebAvanzada3Cuatrimestre\WebAvanzada3Cuatrimestre.Domain\Entidades" --context ApplicationDbContext --context-dir Data --force`