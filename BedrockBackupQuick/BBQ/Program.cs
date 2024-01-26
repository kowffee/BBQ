using BBQ.Utils;
using static System.ConsoleColor;
using static BBQ.Utils.ConsoleUtil;

namespace BBQ
{
    internal class Program
    {
        internal static string MinecraftPackageDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages", "Microsoft.MinecraftUWP_8wekyb3d8bbwe");
        internal static string AppDir = AppDomain.CurrentDomain.BaseDirectory;

        static void Main(string[] args)
        {
            Print("Welcome to BedrockBackupQuick (BBQ)! This tool simplifies the process of importing and exporting your LocalState folder for Minecraft: Bedrock Edition.", Cyan);

            if (!Files.CheckIfMcDirectoryExists())
            {
                Print($"Could not find the Minecraft package folder at {MinecraftPackageDirectory}", Red);
                UserInput("Press Enter to close", Red);
                Environment.Exit(0);
            }
            Directory.CreateDirectory(Path.Combine(AppDir, "BbqData"));

            while (true)
            {
                string? choice = UserInput("Would you like to import or export?");
                switch (choice)
                {
                    case "import":
                        Features.Import.ImportBackup();
                        return;
                    case "export":
                        Features.Export.ExportBackup();
                        return;
                    default:
                        Print("Invalid choice. Please enter 'import' or 'export'.", Red);
                        break;
                }
            }
        }
    }
}
