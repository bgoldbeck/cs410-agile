using Actions;
using System;
using System.Collections.Generic;
using System.Text;
using IO;

namespace UI
{
    public class ChmodFileRemoteUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.P;

        public bool RequiresLogin => true;

        public bool RequiresSelection => false;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "Change file (P)ermissions";

        public DFtpResult Go()
        {
            int permissions;
            try
            {
                permissions = IOHelper.AskInt("Enter new permissions in chmod format (e.g. 730): ");

                // Create the action, Initialize it with the info we've collected
                DFtpAction action = new ChmodFileRemoteAction(Client.ftpClient, Client.remoteDirectory, Client.remoteSelection, permissions);

                // Carry out the action and get the result
                DFtpResult result = action.Run();

                // Give some feedback if successful
                if (result.Type == DFtpResultType.Ok)
                {
                    IOHelper.Message("Permissions for this file were changed to " + permissions + ".");
                }
                // Return the result after running.

                return result;
            }

            catch (Exception ex)
            {
                IOHelper.Message("Exception inputting permissions: " + ex.Message);
                return new DFtpResult(DFtpResultType.Error, ex.Message);
            }
        }
    }
}
