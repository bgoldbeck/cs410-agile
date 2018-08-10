using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;

namespace UI
{
    public class GetMultipleUI : IDFtpUI
    {

        public ConsoleKey Key => ConsoleKey.M;

        public bool RequiresLogin => true;

        public bool RequiresSelection => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "Get (M)ultiple Files From Remote Server";

        public DFtpResult Go()
        {
            // Get listing for remote directory
            DFtpAction getListingAction = new GetListingRemoteAction(Client.ftpClient, Client.remoteDirectory);
            DFtpResult tempResult = getListingAction.Run();
            DFtpListResult listResult = null;
            if (tempResult is DFtpListResult)
            {
                listResult = (DFtpListResult)tempResult;
                List<DFtpFile> list = listResult.Files;

                List<DFtpFile> selected = new List<DFtpFile>();
                selected = IOHelper.SelectMultiple("Select multiple files to download.", list, false);

                DFtpAction action = new GetMultipleAction(Client.ftpClient, Client.localDirectory, selected);

                // Carry out the action and get the result
                DFtpResult result = action.Run();

                // Give some feedback if successful
                if (result != null)
                {
                    IOHelper.Message("files downloaded successfully.");
                    return new DFtpResult(DFtpResultType.Ok);
                }
            }
            return tempResult;
        }
    }
}
