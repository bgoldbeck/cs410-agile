using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;

namespace UI
{
    public class DeleteFileLocalUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.D;

        public bool RequiresLogin => true;

        public bool RequiresSelection => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => true;

        public string MenuText => "(D)elete Local File";


        public DFtpResult Go()
        {
            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new DeleteFileLocalAction(Client.ftpClient, Client.localDirectory, Client.localSelection);

            // Carry out the action and get the result
            DFtpResult result = action.Run();

            // Nullify the selection if successful.
            if (result.Type == DFtpResultType.Ok)
            {
                IOHelper.Message("The file '" + Client.localSelection.GetName() + "' was deleted successfully.");
                Client.remoteSelection = null;
            }
            
            return result;
        }
    }
}
