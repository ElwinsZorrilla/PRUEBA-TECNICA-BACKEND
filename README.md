# Library Proxy - Backend

API hecha en .NET 8 que actúa como proxy entre el frontend y la API externa [FakeRestAPI](https://fakerestapi.azurewebsites.net/index.html). Maneja libros y autores con todas las operaciones CRUD.

## Por qué un proxy

La idea es no consumir la API externa directamente desde el frontend, sino tener una capa intermedia que se encarga de validar datos, manejar errores, aplicar reintentos cuando algo falla y registrar logs detallados. Así el frontend solo se preocupa por mostrar información.

## Tecnologías

- .NET 8
- MediatR (CQRS)
- AutoMapper
- Polly (resiliencia HTTP)
- Serilog (logging)
- FluentValidation
- Swagger / OpenAPI
- xUnit, Moq y FluentAssertions para tests

## Arquitectura

El proyecto sigue Clean Architecture, separado en cuatro capas:

```
src/
├── LibraryProxy.API              Controllers, middleware, configuración
├── LibraryProxy.Application      CQRS (Commands, Queries, Handlers, Validators)
├── LibraryProxy.Domain           Entidades y contratos
└── LibraryProxy.Infrastructure   Acceso a la API externa con HttpClient
```

La regla es que cada capa solo conoce a las que están "debajo". El Domain no conoce a nadie, la Application conoce al Domain, la Infrastructure implementa los contratos del Domain, y la API orquesta todo. Esto hace que cambiar una pieza no rompa el resto.

## Cómo levantar el proyecto

Necesitas el [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.

Clona el repo:

```bash
git clone https://github.com/ElwinsZorrilla/PRUEBA-TECNICA-BACKEND.git
cd PRUEBA-TECNICA-BACKEND
```

Restaura paquetes:

```bash
dotnet restore
```

Compila:

```bash
dotnet build
```

Ejecuta la API:

```bash
dotnet run --project src/LibraryProxy.API
```

Una vez corriendo, tienes disponibles:

- API base: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`
- Health check: `http://localhost:5000/health`

## Endpoints

### Books

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/v1/books` | Listar todos |
| GET | `/api/v1/books/paged` | Listar paginado con cursor |
| GET | `/api/v1/books/{id}` | Obtener uno |
| POST | `/api/v1/books` | Crear |
| PUT | `/api/v1/books/{id}` | Actualizar |
| DELETE | `/api/v1/books/{id}` | Eliminar |

### Authors

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/v1/authors` | Listar todos |
| GET | `/api/v1/authors/paged` | Listar paginado con cursor |
| GET | `/api/v1/authors/{id}` | Obtener uno |
| POST | `/api/v1/authors` | Crear |
| PUT | `/api/v1/authors/{id}` | Actualizar |
| DELETE | `/api/v1/authors/{id}` | Eliminar |

## Cómo funciona internamente

### CQRS con MediatR

Cada operación se modela como un Command (escritura) o Query (lectura). El controller solo recibe la petición HTTP, la convierte en un Command/Query y se la pasa a MediatR. MediatR busca el handler correspondiente y lo ejecuta.

Esto mantiene los controllers delgados y la lógica de negocio bien separada.

### Pipeline de Behaviors

Antes de llegar al handler, cada request pasa por una cadena de behaviors:

```
Request → Logging → Performance → Validation → Handler
```

- **Logging** registra cuándo entra y cuándo sale, con su duración.
- **Performance** emite un warning si el request tarda más de 500ms.
- **Validation** ejecuta los validators de FluentValidation. Si algo falla, lanza una `ValidationException` antes de tocar el handler.

### Polly para resiliencia HTTP

Las llamadas a FakeRestAPI pueden fallar (timeout, 5xx, red caída). Polly se encarga de:

- **Retry**: hasta 3 reintentos con espera exponencial (1s, 2s, 4s).
- **Circuit Breaker**: si fallan 5 peticiones seguidas, corta el circuito por 30 segundos para no saturar el servicio externo.

### AutoMapper

La API externa devuelve un formato JSON específico. Internamente trabajamos con nuestras propias entidades. AutoMapper convierte automáticamente:

- `BookApiModel` ↔ `Book`
- `AuthorApiModel` ↔ `Author`

Los profiles de mapeo viven en `Infrastructure/ExternalApi/Mappings/`.

## Tests

```bash
dotnet test
```

Hay 27 tests unitarios cubriendo handlers (queries y commands), validators y el pipeline de validación.

## Logs

Los logs se escriben tanto en consola como en archivos diarios:

```
src/LibraryProxy.API/logs/log-YYYYMMDD.txt
```

Cada entrada incluye timestamp, nivel y mensaje con datos estructurados.

## Problemas comunes

**El puerto 5000 está ocupado.** Modifica `src/LibraryProxy.API/Properties/launchSettings.json` y cambia el puerto en el perfil `http`.

**No conecta a FakeRestAPI.** Revisa la URL en `appsettings.json`. Debe apuntar a `https://fakerestapi.azurewebsites.net/api/v1/`.

**Los tests fallan porque hay archivos bloqueados.** Mata cualquier proceso de la API que esté corriendo:

```bash
taskkill /F /IM dotnet.exe
dotnet test
```

## Repositorios

- Backend: https://github.com/ElwinsZorrilla/PRUEBA-TECNICA-BACKEND.git
- Frontend: https://github.com/ElwinsZorrilla/PRUEBA-TECNICA-FRONT.git
