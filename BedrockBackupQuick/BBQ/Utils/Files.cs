using static System.ConsoleColor;
using static BBQ.Utils.ConsoleUtil;

namespace BBQ.Utils
{
    internal class Files
    {
        internal static bool CheckIfMcDirectoryExists()
        {
            if (Directory.Exists(Program.MinecraftPackageDirectory))
                return true;

            return false;
        }

        internal static async Task DeleteFileType(string directoryPath, string fileFormats)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Print($"Directory does not exist: {directoryPath}", Red);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(fileFormats))
                    {
                        Print("Invalid file format.", Red);
                        return;
                    }

                    try
                    {
                        // Split file formats and create an array
                        string[] formats = fileFormats.Split('|');

                        // Iterate through the directory and subdirectories
                        foreach (string format in formats)
                        {
                            // Use Directory.EnumerateFiles to get all files with the specified format
                            var filesToDelete = Directory.EnumerateFiles(directoryPath, format, SearchOption.AllDirectories);

                            // Delete each file
                            foreach (var file in filesToDelete)
                            {
                                try
                                {
                                    File.Delete(file);
                                }
                                catch (Exception ex)
                                {
                                    Print($"Error deleting file {file}: {ex.Message}", Red);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Print($"An error occurred while processing files: {ex.Message}", Red);
                    }
                });
            }
            catch (Exception ex)
            {
                Print($"An unexpected error occurred: {ex.Message}", Red);
            }
        }
    }
}
