# CombatManager.APIs

Client libraries and Pathfinder Combat Manager application — for integrating with and running [Combat Manager](https://github.com/KyleADOlson/CombatManager) style combat management.

## Structure

| Folder | Description |
|--------|-------------|
| **pf-combat-manager/** | Pathfinder Combat Manager (from PFAssistant) — full application with API, Base, and WPF7 UI |
| **samples/** | Sample console app for API integration |

### pf-combat-manager Projects

| Project | Description |
|---------|-------------|
| **CombatManager.Api.Core** | Shared interfaces, DTOs, request types (netstandard2.0) |
| **CombatManager.Api** | HTTP/WebSocket client for Combat Manager API |
| **CombatManager.Api.Test** | Unit tests |
| **CombatManager.Base** | Core combat logic, database, HTML server |
| **CombatManager.WPF7** | Windows WPF application (net8.0-windows) |

## Requirements

- .NET 8.0 SDK or .NET 10.0 SDK
- **Api.Core Dependency Model:** `CombatManager.Api.Core` is a local project (not a NuGet package). See [ADR-0001](https://github.com/PolyhydraGames/rpg-combat-manager/blob/main/docs/decisions/0001-api-core-dependency-model.md) for details.
- NuGet: PolyhydraGames packages (see NuGet.Config) — requires Azure DevOps authentication (see [docs/nuget-feed-auth.md](docs/nuget-feed-auth.md)) — only needed for `CombatManager.Base` and `CombatManager.WPF7`

## Building

```bash
dotnet restore CombatManager.APIs.sln --configfile NuGet.Config
dotnet build CombatManager.APIs.sln
```

If restore fails with `401 (Unauthorized)` against the Polyhydra feed, complete the auth steps in [docs/nuget-feed-auth.md](docs/nuget-feed-auth.md) and rerun the restore command.

To build only the API (without WPF):
```bash
dotnet build pf-combat-manager/CombatManager.Api/
```

## Quick Start (API Client)

```csharp
using CombatManager.Api;

var service = new CombatManagerService("http://localhost:8080/")
{
    Passcode = "your-passcode"
};

var state = await service.GetCombatState();
Console.WriteLine($"Round {state.Round}, {state.CombatList?.Count} combatants");
```

## License

See [LICENSE](LICENSE).


## 📖 Documentation
Detailed documentation can be found in the following sections:
- [Feature Index](./docs/features/README.md)
- [Core Capabilities](./docs/features/core-capabilities.md)
