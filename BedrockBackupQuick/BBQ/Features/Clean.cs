using System.Diagnostics;
using static System.ConsoleColor;
using static BBQ.Utils.ConsoleUtil;
using static BBQ.Utils.Files;

namespace BBQ.Features
{
    internal class Clean
    {
        internal static async Task Cleanup()
        {
            string localCachePath = Path.Combine(Program.MinecraftPackageDirectory, "LocalCache", "minecraftpe");
            bool choosing = true;

            while (choosing)
            {
                string? input = UserInput("Although the process of cleaning and deleting outdated files from the LocalState folder is generally safe, it's possible that you may not be comfortable with it. This action involves removing old, unnecessary files, which could potentially cause issues if any important data is inadvertently deleted. However, the risk is low. Do you still want to proceed? (y/n)");
                switch (input.ToLower())
                {
                    case "y":
                        choosing = false;
                        break;
                    case "n":
                        UserInput("Cancelled. Press enter to close.", Yellow);
                        Environment.Exit(0);
                        return;
                    default:
                        Print("Invalid choice. Please enter 'y' or 'n'.", Red);
                        break;
                }
            }

            Print("Beginning Cleanup, this may take a few minutes.", Yellow);
            Stopwatch cleanTime = Stopwatch.StartNew();

            #region Meat of the Clean
            var deleteList = new List<(string DirectoryPath, string FileFormats)>
            {
                (localCachePath, "*.*"),
                (Path.Combine(Program.MinecraftPackageDirectory, "RoamingState"), "logs.txt|Log.txt"),
                (Program.LSPath, "*.ent|telemetry_info.json|invalid_known_packs.json"),
                (Path.Combine(Program.MinecraftPackageDirectory, "AC", "CrashDumps"), "*.dmp"), // These are normally 35mb+
                (Path.Combine(Program.LSPath, "treatments", "treatment_packs2"), "*.json")
            };
            await DeleteFileType(deleteList);// Delete file types in the directory and its sub-folders
            var deleteListSpecific = new List<(string DirectoryPath, string FileFormats)>
            {
                (Path.Combine(Program.LSPath, "games", "com.mojang"), "*.dat")
            };
            await DeleteFileType(deleteListSpecific, false);// Delete file types ONLY in the directory
            await DeleteSubfolders(Path.Combine(localCachePath, "MessagingService"));
            await DeleteSubfolders(Path.Combine(Program.LSPath, "premium_cache", "persona"));
            await DeleteSubfolders(Path.Combine(Program.LSPath, "treatments", "treatment_packs2")); // Switching versions sometimes causes UI bugs bc of treatments, deleting these fixes the UI issues
            cleanTime.Stop();
            #endregion

            // If it finishes in under a second, display in miliseconds
            Print(cleanTime.Elapsed.TotalSeconds < 1 ? $"Finished in {cleanTime.Elapsed.Milliseconds}ms" : $"Finished in {cleanTime.Elapsed.Seconds}s", Yellow);
            UserInput("Done, you may now exit.", Green);
        }
    }
}
