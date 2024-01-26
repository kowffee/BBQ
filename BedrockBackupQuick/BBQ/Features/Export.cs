using static System.ConsoleColor;
using static BBQ.Utils.ConsoleUtil;
using System.IO.Compression;

namespace BBQ.Features
{
    internal class Export
    {
        private static string LSPath = Path.Combine(Program.MinecraftPackageDirectory, "LocalState");

        internal static void ExportBackup()
        {
            Print("Beginning Export, this may take a few minutes.", Yellow);
            DateTime dtNow = DateTime.Now;
            string ZipFormat = $"{dtNow.Year}-{dtNow.Month}-{dtNow.Day}_{dtNow.Hour}{dtNow.Minute}{dtNow.Second}-BACKUP.zip";
            ZipFile.CreateFromDirectory(LSPath, Path.Combine(Program.AppDir, "BbqData", ZipFormat));
            Print("Backup created at " + Path.Combine(Program.AppDir, "BbqData", ZipFormat), Green);
            UserInput("Done, you may now exit", Green);
        }
    }
}
