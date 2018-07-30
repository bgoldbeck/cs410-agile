﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Actions;

using FluentFTP;
using IO;
using System.Linq;

namespace UI
{
    public class ChangeDirectoryDownUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.Enter;

        public bool RequiresLogin => true;

        public bool RequiresFile => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => false;

        public string MenuText => "[Enter] = Change directory";

        public DFtpResult Go()
        {
            // Get listing for current directory
            if (Client.state == ClientState.VIEWING_REMOTE)
            {
                DFtpAction getListingAction = new GetListingRemoteAction(Client.ftpClient, Client.remoteDirectory);
                DFtpResult tempResult = getListingAction.Run();
                DFtpListResult listResult = null;
                if (tempResult is DFtpListResult)
                {
                    listResult = (DFtpListResult)tempResult;
                    List<DFtpFile> list = listResult.Files;
                    // Filter out files
                    list = list.Where(x => x.Type() == FtpFileSystemObjectType.Directory).ToList();

                    // Choose from directories
                    DFtpFile selection = IOHelper.Select<DFtpFile>("Choose a directory to enter.", list);

                    // If something has been selected, update the remote selection
                    if (selection != null)
                    {
                        Client.remoteSelection = null;
                        Client.remoteDirectory = selection.GetFullPath();
                        return new DFtpResult(DFtpResultType.Ok, "Changed to directory '" + Client.remoteDirectory + "'.");
                    }
                }
                return tempResult;
            }
            else if(Client.state == ClientState.VIEWING_LOCAL)
            {
                // Get listing for current directory
                DFtpAction action = new GetListingLocalAction(Client.localDirectory);
                DFtpResult result = action.Run();
                DFtpListResult listResult = null;
                if (result is DFtpListResult)
                {
                    listResult = (DFtpListResult)result;
                    List<DFtpFile> list = listResult.Files;
                    // Filter out files
                    list = list.Where(x => x.Type() == FtpFileSystemObjectType.Directory).ToList();

                    // Choose from directories
                    DFtpFile selection = IOHelper.Select<DFtpFile>("Choose a directory to enter.", list);

                    // If something has been selected, update the local selection
                    if (selection != null)
                    {
                        Client.localSelection = null;
                        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            Client.localDirectory = Client.localDirectory + @"\" + selection;
                        }
                        else
                        {
                            Client.localDirectory = Client.localDirectory + "/" + selection;
                        }
                        return new DFtpResult(DFtpResultType.Ok, "Changed to directory '" + Client.localDirectory + "'.");
                    }
                }
                return result;
            }
            else
            {
                return new DFtpResult(DFtpResultType.Error, "Error on changing directory.");
            }
        }
    }
}