﻿using static System.ConsoleColor;
using static BBQ.Utils.ConsoleUtil;

namespace BBQ.Features
{
    internal class Clean
    {
        internal static void Cleanup()
        {
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



            UserInput("Done, you may now exit.", Green);
        }
    }
}
