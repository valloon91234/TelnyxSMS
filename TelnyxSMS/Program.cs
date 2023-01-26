// See https://aka.ms/new-console-template for more information

// See https://github.com/tonerdo/dotnet-env
using Telnyx;

DotNetEnv.Env.Load("config.env");
var TELNYX_API_KEY = DotNetEnv.Env.GetString("TELNYX_API_KEY");
var telnyxNumber = DotNetEnv.Env.GetString("TELNYX_NUMBER");
var lines = File.ReadAllLines("list.txt");
TelnyxConfiguration.SetApiKey(TELNYX_API_KEY);
MessagingSenderIdService service = new();
Logger logger = new($"{DateTime.UtcNow:yyyy-MM-dd  HH.mm.ss}");
int lineCount = lines.Length;
for (int i = 0; i < lineCount; i++)
{
    string? destinationNumber = null;
    try
    {
        var lineParams = lines[i].Split("\t", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        destinationNumber = lineParams[0].Replace(" ", "");
        var contentText = lineParams[1];
        NewMessagingSenderId options = new()
        {
            From = telnyxNumber,
            To = destinationNumber,
            Text = contentText
        };
        var messageResponse = service.CreateAsync(options).Result;
        logger.WriteLine($"[{i + 1} / {lineCount}] \t {destinationNumber} \t OK: {messageResponse.Id}", ConsoleColor.Green);
    }
    catch (Exception ex)
    {
        logger.WriteLine($"[{i + 1} / {lineCount}] \t {destinationNumber} \t Error: {ex.Message}", ConsoleColor.Red);
    }
}

Console.WriteLine("Press any key to exit...");
Console.Read();
