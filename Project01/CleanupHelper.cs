using System.Diagnostics;

namespace Project01;
public static class CleanUpHelper
{
    public static void OnShutdown(IServiceProvider services)
    {
        var environment = services.GetRequiredService<IHostEnvironment>();

        if (!environment.IsEnvironment("Test"))
        {
            // Define the path to your database file
            var dbPath = "./Database/Project01.db";

            // Delete the database file if it exists
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
            else
            {
                Console.WriteLine($"No database file found at: {dbPath}");
            }

            // Apply migrations to recreate the database
            ExecuteShellCommand("dotnet", "ef database update");
        }
    }

    public static void ExecuteShellCommand(string command, string args)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using (var process = new Process { StartInfo = processInfo })
        {
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine(output);
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("Error: " + error);
            }
        }
    }
}