# hacka-properties-service

A small microservice to manage agricultural properties and their fields. Built with ASP.NET Core targeting .NET 8 (C# 12). The service exposes authenticated CRUD endpoints for producers to create, read, update and delete properties and nested fields. Authentication is expected via JWT where the producer's id is present in the `sub` claim.

## Features

- Authenticated endpoints (reads the producer id from the `sub` claim in the JWT).
- CRUD for `Property` entities.
- Nested CRUD for `Field` entities under a `Property`.
- In-memory store (default) for quick development and prototyping.
- Clear DTOs for request/response shapes.

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or VS Code
- Git

## Quick start (local)

1. Clone the repository:
git clone https://github.com/caiobrdaricfiap/hacka-properties-service.git cd hacka-properties-service


2. Restore and run:
dotnet restore dotnet run --project <path-to-project-csproj>


3. The API defaults to `https://localhost:5001` (confirm `launchSettings.json` or `appsettings.json`).

## Configuration

- `appsettings.json` — general configuration (port, logging, etc.).
- `Properties/launchSettings.json` — local launch profiles used by Visual Studio.

Authentication

- The controller expects an authenticated user and reads the producer id from the JWT `sub` claim. If you do not have authentication setup, you can stub the principal in development or extend `Program.cs` to allow anonymous calls during local development.

## API Reference

Base route: `GET/POST/PUT/DELETE /api/properties`

Properties

- POST `/api/properties`
  - Creates a property.
  - Body: `CreatePropertyRequest` (name, location)
  - Response: `201 Created` with `PropertyResponse`.

- GET `/api/properties`
  - Returns all properties for the authenticated producer.
  - Response: `200 OK` with `PropertyResponse[]`.

- GET `/api/properties/{id}`
  - Returns a property by id (if owned by producer).
  - Response: `200 OK` or `404 Not Found`.

- PUT `/api/properties/{id}`
  - Updates a property.
  - Body: `UpdatePropertyRequest` (name, location)
  - Response: `204 No Content` or `404 Not Found`.

- DELETE `/api/properties/{id}`
  - Deletes a property.
  - Response: `204 No Content` or `404 Not Found`.

Fields (nested under a property)

- POST `/api/properties/{propertyId}/fields`
  - Adds a field to a property.
  - Body: `CreateFieldRequest` (name, areaInHectares, crop)
  - Response: `201 Created` with `FieldResponse`.

- GET `/api/properties/{propertyId}/fields`
  - Lists fields for a property.
  - Response: `200 OK` with `FieldResponse[]`.

- GET `/api/properties/{propertyId}/fields/{fieldId}`
  - Gets a single field.
  - Response: `200 OK` or `404 Not Found`.

- PUT `/api/properties/{propertyId}/fields/{fieldId}`
  - Updates a field.
  - Body: `UpdateFieldRequest` (name, areaInHectares, crop)
  - Response: `204 No Content` or `404 Not Found`.

- DELETE `/api/properties/{propertyId}/fields/{fieldId}`
  - Removes a field from a property.
  - Response: `204 No Content`.

Authentication header example (Bearer token):
curl -H "Authorization: Bearer <token>" https://localhost:5001/api/properties


Note: Token must contain the `sub` claim set to the producer's GUID.

## DTOs / Domain Model

- DTOs located under `Application/DTOs`: `CreatePropertyRequest`, `UpdatePropertyRequest`, `PropertyResponse`, `CreateFieldRequest`, `UpdateFieldRequest`, `FieldResponse`.
- Domain entities located under `Domain/Entities`: `Property`, `Field` with domain methods for update/add/remove.

## Data persistence

- Current implementation uses an in-memory static list: `private static readonly List<Property> _properties` in `PropertiesController` for quick prototyping.
- Next step: wire up EF Core (`Infra/Data/AppDbContext.cs`) and migrate to a persistent database.

## Error handling

- Basic error responses are returned from controllers (404, 401, 204, etc.).
- Middleware skeleton exists at `Infra/Middleware/ExceptionMiddleware.cs` for centralized exception handling.

## Testing

- No automated tests are included yet. Recommended: add unit tests for domain logic and integration tests for controllers.

## Development notes / Next steps

- Implement persistence with EF Core and migrations.
- Add model validation (DataAnnotations or FluentValidation).
- Add integration tests and CI pipeline.
- Harden authentication and authorization policies.
- Add OpenAPI/Swagger documentation.

## Contributing

Contributions are welcome. Please follow the repository contribution guidelines. Create feature branches, run tests locally and open a pull request describing changes.

## License

Specify your license here (e.g., MIT).