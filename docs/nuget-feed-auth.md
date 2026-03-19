# Azure Artifacts NuGet Feed Authentication

This project uses Azure DevOps Artifacts for some packages. This document explains how to authenticate.

## Feed URL

```
https://pkgs.dev.azure.com/polyhydragames/_packaging/PolyhydraSoftware/nuget/v3/index.json
```

## Expected Behavior

Without authentication, `dotnet restore` may fail with:

```
error: Unable to load the service index for source https://pkgs.dev.azure.com/polyhydragames/_packaging/PolyhydraSoftware/nuget/v3/index.json.
error: Response status code does not indicate success: 401 (Unauthorized).
```

This is expected — the feed requires authentication.

## Authentication

### Option 1: Azure Artifacts Credential Provider (Recommended)

The simplest way is to use the Azure Artifacts Credential Provider, which automatically authenticates with your Azure DevOps credentials.

#### Windows

```powershell
# Install via .NET tool
dotnet tool install --global AzureArtifacts.CredentialProvider

# Or use the extension (VS2022+)
```

#### Linux/macOS

```bash
# Install via .NET tool
dotnet tool install --global AzureArtifacts.CredentialProvider
```

After installation, `dotnet restore` should work automatically using your Azure DevOps credentials.

### Option 2: Personal Access Token (PAT)

If the credential provider doesn't work, you can use a Personal Access Token.

#### Create a PAT

1. Go to https://dev.azure.com/polyhydragames/_usersSettings/tokens
2. Create a new token with **Packaging (read)** scope
3. Copy the token

#### Configure the Token

**Windows (PowerShell):**

```powershell
# Add to %AppData%\NuGet\NuGet.Config (user-level)
dotnet nuget add source https://pkgs.dev.azure.com/polyhydragames/_packaging/PolyhydraSoftware/nuget/v3/index.json --name Polyhydra --store-password-in-clear-text --username your-email@domain.com --password YOUR_PAT_HERE
```

**Linux/macOS:**

```bash
# Add to ~/.nuget/NuGet/NuGet.Config (user-level)
dotnet nuget add source https://pkgs.dev.azure.com/polyhydragames/_packaging/PolyhydraSoftware/nuget/v3/index.json --name Polyhydra --store-password-in-clear-text --username your-email@domain.com --password YOUR_PAT_HERE
```

Or add to project-specific NuGet.Config:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <clear />
        <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3"/>
        <add key="Poly"
             value="https://pkgs.dev.azure.com/polyhydragames/_packaging/PolyhydraSoftware/nuget/v3/index.json"
             username="your-email@domain.com"
             password="YOUR_PAT_HERE" />
    </packageSources>
</configuration>
```

**Warning:** Never commit PATs to version control. Add `NuGet.Config` to `.gitignore` if using local credentials.

## Verify

Before auth (expected failure mode):

```bash
dotnet restore CombatManager.APIs.sln --configfile NuGet.Config
# Expected: NU1301 + 401 (Unauthorized) for the Polyhydra feed
```

After configuring authentication (expected success mode):

```bash
dotnet restore CombatManager.APIs.sln --configfile NuGet.Config
```

You should see restore complete with no NU1301/401 errors from:

`https://pkgs.dev.azure.com/polyhydragames/_packaging/PolyhydraSoftware/nuget/v3/index.json`

## Troubleshooting

- **401 after using PAT**: Ensure the PAT has "Packaging" > "Read" scope
- **Credential provider not found**: Add `~/.nuget/bin` to your PATH after installing the tool
- **Cached credentials**: Clear NuGet caches with `dotnet nuget locals http-cache --clear`

## Alternative: Build Without Auth

If you only need the API (not WPF):

```bash
dotnet build pf-combat-manager/CombatManager.Api/
```

This may work without the Azure feed if those dependencies are on nuget.org.
