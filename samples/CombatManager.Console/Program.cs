using CombatManager.Api;

// Example: Connect to Combat Manager WebSocket/HTTP server
// Replace with your Combat Manager server address (e.g., http://localhost:8080/)
var address = args.Length > 0 ? args[0] : "http://localhost:8080/";
var passcode = args.Length > 1 ? args[1] : "";

var service = new CombatManagerService(address)
{
    Passcode = passcode
};

Console.WriteLine($"Connecting to Combat Manager at {address}");
Console.WriteLine("Fetching combat state...");

try
{
    var state = await service.GetCombatState();
    Console.WriteLine($"Round: {state.Round}, Combatants: {state.CombatList?.Count ?? 0}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine("Ensure Combat Manager (CombatViewService) is running and the address is correct.");
}
