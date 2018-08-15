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

        public bool RequiresSelection => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "Change file (P)ermissions";

        public DFtpResult Go()
        {
            try
            {
                bool valid = true;
                int permissions = IOHelper.AskInt("Enter new permissions in chmod format (e.g. 730): ");
                String permString = permissions.ToString();
                if(permString.Length != 3)
                {
                    IOHelper.Message("Permission must be 3 digits long.");
                    valid = false;
                }
                foreach (char c in permString)
                {
                    switch(c)
                    {
                        case '0':
                        case '1':
                        case '2':
                        case '4':
                        case '3':
                        case '5':
                        case '6':
                        case '7':
                            break;
                        default:
                            IOHelper.Message("Permission must be in chmod format.");
                            valid = false;
                            break;
                    }
                }

                if (valid)
                {
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
                else
                {
                    return new DFtpResult(DFtpResultType.Error, "Permission input was invalid");
                }
            }

            catch (Exception ex)
            {
                IOHelper.Message("Exception inputting permissions: " + ex.Message);
                return new DFtpResult(DFtpResultType.Error, ex.Message);
            }
        }
    }
}
