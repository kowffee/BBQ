using System.Diagnostics;
using static System.ConsoleColor;
using static BBQ.Utils.ConsoleUtil;

namespace BBQ.Features
{
    internal class Export
    {
        private static string LSPath = Path.Combine(Program.MinecraftPackageDirectory, "LocalState");

        internal static void ExportBackup()
        {
            DateTime dtNow = DateTime.Now;
            string ZipFormat = $"{dtNow.Year}-{dtNow.Month}-{dtNow.Day}_{dtNow.Hour}{dtNow.Minute}{dtNow.Second}-LocalStateBackup.zip";
            Print("Beginning Export, this may take a few minutes.", Yellow);

            try
            {
                Stopwatch zipTime = Stopwatch.StartNew();
                System.IO.Compression.ZipFile.CreateFromDirectory(LSPath, Path.Combine(Program.AppDir, "BbqData", ZipFormat));
                zipTime.Stop();
                Print($"({zipTime.Elapsed.TotalSeconds}s) Backup created at {Path.Combine(Program.AppDir, "BbqData", ZipFormat)}", Green);
            } catch (Exception e)
            {
                Print($"Backup failed, error printed below:\n{e.Message}", Red);
                UserInput("Press enter to close");
                Environment.Exit(0);
            }

            UserInput("Done, you may now exit", Green);
        }
    }
}
