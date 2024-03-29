﻿using System.Diagnostics;
using System.IO.Compression;
using static System.ConsoleColor;
using static BBQ.Utils.ConsoleUtil;

namespace BBQ.Features
{
    internal class Import
    {
        internal static void ImportBackup()
        {
            string importFile = Path.Combine(Program.AppDir, "BbqData", "Import.zip");
            bool choosing = true;

            if (!File.Exists(importFile))
            {
                Print($"No import file was found, please add a LocalState zip file into {Path.Combine(Program.AppDir, "BbqData")} named 'Import.zip'.", Red);
                UserInput("Press enter to close.", Yellow);
                Environment.Exit(0);
            }

            while (choosing)
            {
                string? input = UserInput("You risk ruining your LocalState folder, make sure the file you are importing is a valid, compressed LocalState folder that you want to use and has your files. Are you sure you want to continue? (y/n)");
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

            Print("Beginning Import, this may take a few minutes.", Yellow);
            // it has to be like this otherwise random errors (like no permission) will cancel the extraction. I'm cri
            try
            {
                Stopwatch importTimer = Stopwatch.StartNew();

                using (ZipArchive archive = ZipFile.OpenRead(importFile))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        try
                        {
                            string entryPath = Path.Combine(Program.LSPath, entry.FullName);

                            // Check if the entry represents a directory
                            if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\"))
                            {
                                Directory.CreateDirectory(entryPath);
                            }
                            else
                            {
                                entry.ExtractToFile(entryPath, true);
                            }
                        }
                        catch (UnauthorizedAccessException unauthorizedEx)
                        {
                            Print($"Access to the path is denied for {entry.FullName}: {unauthorizedEx.Message}", Red);
                        }
                        catch (Exception ex)
                        {
                            Print($"Failed to extract {entry.FullName}: {ex.Message}", Red);
                        }
                    }
                }

                importTimer.Stop();
                Print($"Done in {importTimer.Elapsed.Seconds} seconds.", Yellow);
            }
            catch (Exception ex)
            {
                Print($"Import failed, error printed below:\n{ex.Message}", Red);
                UserInput("Press enter to close.", Yellow);
                Environment.Exit(0);
            }

            UserInput("Done, you may now exit.", Green);
        }
    }
}
