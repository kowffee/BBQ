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

        internal static async Task DeleteFileType(List<(string DirectoryPath, string FileFormats)> directories, bool deleteInSubfolders = true)
        {
            try
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(directories, directory =>
                    {
                        if (!Directory.Exists(directory.DirectoryPath))
                        {
                            Print($"Directory does not exist: {directory.DirectoryPath}", Red);
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(directory.FileFormats))
                        {
                            Print($"Invalid file formats for directory {directory.DirectoryPath}.", Red);
                            return;
                        }

                        try
                        {
                            string[] formats = directory.FileFormats.Split('|');

                            SearchOption searchOption = deleteInSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                            Parallel.ForEach(formats, format =>
                            {
                                var filesToDelete = Directory.EnumerateFiles(directory.DirectoryPath, format, searchOption);

                                Parallel.ForEach(filesToDelete, file =>
                                {
                                    try
                                    {
                                        File.Delete(file);
                                        Print($"Deleted: {file}", Yellow);
                                    }
                                    catch (Exception ex)
                                    {
                                        Print($"Error deleting file {file}: {ex.Message}", Red);
                                    }
                                });
                            });
                        }
                        catch (Exception ex)
                        {
                            Print($"An error occurred while processing files in directory {directory.DirectoryPath}: {ex.Message}", Red);
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                Print($"An unexpected error occurred: {ex.Message}", Red);
            }
        }


        internal static async Task DeleteSubfolders(string directoryPath)
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

                    try
                    {
                        var subfolders = Directory.EnumerateDirectories(directoryPath);

                        Parallel.ForEach(subfolders, subfolder =>
                        {
                            try
                            {
                                Directory.Delete(subfolder, true);
                                Print($"Deleted sub-folder: {subfolder}", DarkYellow);
                            }
                            catch (Exception ex)
                            {
                                Print($"Error deleting sub-folder {subfolder}: {ex.Message}", Red);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Print($"An error occurred while processing sub-folders in directory {directoryPath}: {ex.Message}", Red);
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
