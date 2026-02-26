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

- .NET 8.0 SDK
- NuGet: PolyhydraGames packages (see NuGet.Config) — may require Azure DevOps authentication)

## Building

```bash
dotnet restore
dotnet build
```

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
