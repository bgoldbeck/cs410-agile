﻿using System;
using System.Collections.Generic;
using System.Text;
using Actions;
using FluentFTP;
using IO;

namespace UI
{
    /// <summary>
    /// This class 
    /// </summary>
    public class Browser
    {
        public List<IDFtpUI> Actions { get; private set; } = new List<IDFtpUI>
        {
            new ContextSwitchUI(),
            new CreateDirectoryRemoteUI(),
            new PutFileUI(),
            new SearchFileRemoteUI(),
            new DeleteFileRemoteUI(),
            new SelectRemoteUI(),
        };

        public Browser()
        {
        }
    
        public void DrawResultList(DFtpListResult list)
        {
            Console.WriteLine();
            foreach (DFtpFile file in list.Files)
            {
                Console.WriteLine(file.GetName() + " " + file.GetSize() + " " + (file.Type() == FluentFTP.FtpFileSystemObjectType.Directory? "dir" : "file"));
            }
            Console.WriteLine();

            ConsoleUI.WaitForAnyKey();
            return;
        }

        public void DrawClientInfo()
        {
            String localSelectionText = "";
            if (Client.localSelection != null)
                localSelectionText = Client.localSelection.ToString();
            String remoteSelectionText = "";
            if (Client.remoteSelection != null)
                remoteSelectionText = Client.remoteSelection.ToString();
            ConsoleUI.WriteLine("Client.localDirectory:  " + Client.localDirectory, Color.White);
            ConsoleUI.WriteLine("Client.localDirectory:  " + localSelectionText, Color.White);
            ConsoleUI.WriteLine("Client.remoteDirectory: " + Client.remoteDirectory, Color.White);
            ConsoleUI.WriteLine("Client.remoteSelection: " + remoteSelectionText, Color.White);
        }

        public void DrawActionsMenu()
        {
            ConsoleUI.WriteLine("Actions: ", Color.Gold);
            foreach (IDFtpUI action in Actions)
            {
                if (action.RequiresLogin && Client.ftpClient == null)
                {
                    // DO LOGIN.???
                    continue;
                }
                if (action.RequiresFile && ( Client.localSelection == null || Client.localSelection.Type() != FtpFileSystemObjectType.File) )
                {
                    continue;
                }
                if (action.HideForDirectory && Client.localSelection != null && Client.localSelection.Type() == FtpFileSystemObjectType.Directory)
                {
                    continue;
                }
                if (action.HideForFile && Client.localSelection != null && Client.localSelection.Type() == FtpFileSystemObjectType.File)
                {
                    continue;
                }
                if (action.HideForLocal && Client.state == ClientState.VIEWING_LOCAL)
                {
                    continue;
                }
                if (action.HideForRemote && Client.state == ClientState.VIEWING_REMOTE)
                {
                    continue;
                }
                ConsoleUI.WriteLine(action.MenuText, Color.Olive, 5);
            }
            return;
        }

        public void DrawListing()
        {
            DFtpResult result = null;
            DFtpAction action = null;
            if (Client.state == ClientState.VIEWING_LOCAL)
            {
                ConsoleUI.WriteLine("Listing for: " + Client.localDirectory, Color.Gold);
                return;
                //action = new GetListingLocalAction(Client.ftpClient, Client.localDirectory);
                //result = action.Run();
            }
            else if (Client.state == ClientState.VIEWING_REMOTE)
            {
                ConsoleUI.WriteLine("Listing for: " + Client.remoteDirectory, Color.Gold);
                action = new GetListingRemoteAction(Client.ftpClient, Client.remoteDirectory);
            }
            result = action.Run();

            History.Log(result.ToString());

            if (result is DFtpListResult)
            {
                DFtpListResult listResult = (DFtpListResult)result;
                foreach (DFtpFile file in listResult.Files)
                {
                    ConsoleUI.WriteLine(file.ToString(), Color.Green);
                }
            }
        }
    }
}
