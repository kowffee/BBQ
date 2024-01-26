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
    }
}
