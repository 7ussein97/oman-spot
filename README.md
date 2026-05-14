# PhotoSpotOman

PhotoSpotOman is an ASP.NET Core MVC web application for sharing and managing photography spots in Oman.

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (SQL Server)
- Cookie Authentication and Authorization Policies
- Razor Views

## Project Structure

- `PhotoSpotOman.sln` - Solution file
- `PhotoSpotOman/` - Main web application
- `PhotoSpotOman/Controllers/` - MVC controllers (Spot, Category, Comment, Like, Login, etc.)
- `PhotoSpotOman/Models/` - Domain models
- `PhotoSpotOman/Data/` - EF Core DbContext
- `PhotoSpotOman/Migrations/` - EF Core migrations
- `PhotoSpotOman/Views/` - Razor views
- `PhotoSpotOman/wwwroot/` - Static assets
- `PhotoSpotOman/docs/` - Diagrams and project documentation

## Prerequisites

- .NET SDK 8.0+
- SQL Server / LocalDB

## Configuration

Application settings are stored in:

- `PhotoSpotOman/appsettings.json`
- `PhotoSpotOman/appsettings.Development.json`

Update the following before running in your environment:

- `ConnectionStrings:DefaultConnection`
- `JWTSettings` values (secret, issuer, audience, expiration)

For production, store secrets using environment variables or Secret Manager instead of committing them to source control.

## Run Locally

From the repository root:

```bash
dotnet restore .\PhotoSpotOman.sln
dotnet build .\PhotoSpotOman.sln
dotnet run --project .\PhotoSpotOman\PhotoSpotOman.csproj
```

Default development URL in launch settings:

- `http://localhost:5191`

## Database Migrations

Apply existing migrations:

```bash
dotnet ef database update --project .\PhotoSpotOman\PhotoSpotOman.csproj
```

Create a new migration:

```bash
dotnet ef migrations add <MigrationName> --project .\PhotoSpotOman\PhotoSpotOman.csproj
```

If `dotnet ef` is not installed:

```bash
dotnet tool install --global dotnet-ef
```

## Authentication & Authorization

- Cookie-based authentication is configured in `Program.cs`.
- Authorization policies include:
  - `AdminOnly`
  - `ContributorOnly`
  - `AnyRole`
- Access denied path: `/Page/Forbidden`

## Notes

- Uploaded spot images are stored under `PhotoSpotOman/wwwroot/uploads/spots/`.
- The project includes documentation diagrams in `PhotoSpotOman/docs/`.

## License

No license file is currently defined.
