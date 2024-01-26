using System.Diagnostics;
using static System.ConsoleColor;
using static BBQ.Utils.ConsoleUtil;

namespace BBQ.Features
{
    internal class Export
    {
        internal static void ExportBackup()
        {
            DateTime dtNow = DateTime.Now;
            string ZipFormat = $"{dtNow.Year}-{dtNow.Month}-{dtNow.Day}_{dtNow.Hour}{dtNow.Minute}{dtNow.Second}-LocalStateBackup.zip";

            Print("Beginning Export, this may take a few minutes.", Yellow);
            try
            {
                Stopwatch zipTime = Stopwatch.StartNew();
                System.IO.Compression.ZipFile.CreateFromDirectory(Program.LSPath, Path.Combine(Program.AppDir, "BbqData", ZipFormat));
                zipTime.Stop();
                Print($"({zipTime.Elapsed.Seconds}s) Backup created at {Path.Combine(Program.AppDir, "BbqData", ZipFormat)}.", Yellow);
            } catch (Exception e)
            {
                Print($"Backup failed, error printed below:\n{e.Message}", Red);
                UserInput("Press enter to close.", Yellow);
                Environment.Exit(0);
            }

            UserInput("Done, you may now exit.", Green);
        }
    }
}
