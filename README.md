# SmithForever

A Mount & Blade II: Bannerlord mod that eliminates the stamina (energy) cost for **Refining**, **Smelting**, and **Smithing** — so these actions can be performed as long as materials are available.

## How It Works

Uses [Harmony](https://github.com/pardeike/Harmony) runtime patching to intercept the energy cost methods on `DefaultSmithingModel`:

- `GetEnergyCostForRefining` → returns 0
- `GetEnergyCostForSmithing` → returns 0
- `GetEnergyCostForSmelting` → returns 0

No game files are modified. Patches are applied on load and cleanly reverted on unload.

**Overridden class:** `DefaultSmithingModel` — any other mod that patches the same energy cost methods may conflict.

## Requirements

- Mount & Blade II: Bannerlord (tested with current stable)
- .NET Framework 4.7.2

## Build & Deploy

```powershell
# Build
dotnet build src\SmithForever\SmithForever.csproj -c Release

# Deploy to game
./deploy.ps1
```

Adjust `GameFolder` in the `.csproj` or pass it as a parameter to `deploy.ps1` if Bannerlord is installed at a non-standard path.

## Installation (Manual)

1. Copy the `SmithForever` folder to `<Bannerlord>\Modules\`
2. The folder should contain:
   - `SubModule.xml`
   - `bin\Win64_Shipping_Client\SmithForever.dll`
   - `bin\Win64_Shipping_Client\0Harmony.dll`
3. Enable "Smith Forever" in the Bannerlord launcher

## Credits

Inspired by [bannerlord_smith_forever](https://github.com/calsev/bannerlord_smith_forever) by calsev.
