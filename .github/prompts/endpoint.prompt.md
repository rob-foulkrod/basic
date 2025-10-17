---
mode: agent
---

# ASP.NET Controllerless API Endpoint Best Practices

## Core Principles

1. **Single Responsibility** - Each handler maps to one operation
2. **Dependency Injection** - Inject services into handler parameters
3. **Input Validation** - Validate before processing; return 400 Bad Request on failure
4. **Proper HTTP Status Codes** - 200 OK, 201 Created, 204 No Content, 400 Bad Request, 404 Not Found, 500 Error
5. **Consistent Response Format** - Use `Results.*` helpers for standardized responses
6. **Documentation** - Include `.WithOpenApi()`, `.WithSummary()`, and `.Produces()` metadata
7. **Error Handling** - Catch validation exceptions and return meaningful error messages
8. **Testing** - Write unit tests for handlers using dependency injection mocks

## Minimal Example

```csharp
app.MapGet("/api/equipment/{id}", GetEquipmentById)
    .WithName("GetEquipment")
    .WithOpenApi()
    .Produces<Equipment>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithSummary("Retrieves equipment by ID");

async Task<IResult> GetEquipmentById(int id, IEquipmentService service)
{
    var equipment = service.GetEquipmentById(id);
    return equipment != null 
        ? Results.Ok(equipment) 
        : Results.NotFound();
}
```

## Structure
- **Route Definition** - Use `MapGet/Post/Put/Delete()` with metadata
- **Handler Method** - Separate async/sync method with clear responsibility
- **Service Injection** - Receive dependencies as parameters
- **Result Mapping** - Convert domain models to DTOs and HTTP responses
