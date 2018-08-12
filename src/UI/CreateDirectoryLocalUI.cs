using Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using IO;

namespace UI
{
    public class CreateDirectoryLocalUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.C;

        public bool RequiresLogin => true;

        public bool RequiresSelection => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => true;

        public string MenuText => "(C)reate directory";

        public DFtpResult Go()
        {
            String name = IOHelper.AskString("Enter new directory name.");

            // Create the action, Initialize it with the info we've collected
            DFtpAction action = new CreateDirectoryLocalAction(Client.ftpClient, Client.localDirectory + Path.DirectorySeparatorChar + name);

            // Carry out the action and get the result
            DFtpResult result = action.Run();

            // Give some feedback if successful
            if (result.Type == DFtpResultType.Ok)
            {
                IOHelper.Message("The directory '" + name + "' was created successfully.");
            }
            // Return the result after running.

            return result;
        }
    }
}
