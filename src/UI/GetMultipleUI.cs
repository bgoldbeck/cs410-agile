﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;
using System.Linq;
using FluentFTP;

namespace UI
{
    public class GetMultipleUI : IDFtpUI
    {

        public ConsoleKey Key => ConsoleKey.M;

        public bool RequiresLogin => true;

        public bool RequiresSelection => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => true;

        public bool HideForRemote => false;

        public string MenuText => "Get (M)ultiple Files From Remote Server";

        public DFtpResult Go()
        {
            
            // Get listing for remote directory
            DFtpAction getListingAction = new GetListingRemoteAction(Client.ftpClient, Client.remoteDirectory,Client.view_hidden);
            DFtpResult tempResult = getListingAction.Run();
            if(tempResult.Type == DFtpResultType.Ok)
            {  
                DFtpListResult listResult = null;
                if (tempResult is DFtpListResult)
                {
                    listResult = (DFtpListResult)tempResult;
                    List<DFtpFile> list = listResult.Files.Where(x => x.Type() == FtpFileSystemObjectType.File).ToList();
                    
                    List<DFtpFile> selected = new List<DFtpFile>();
                    selected = IOHelper.SelectMultiple("Select multiple files to download!(Arrow keys navigate, spacebar selects/deselects, enter confirms the current selection.)", list, false);

                    if (selected.Count == 0)
                    {
                        IOHelper.Message("No files selected.");
                        return new DFtpResult(DFtpResultType.Ok);
                    }

                    DFtpAction action = new GetMultipleAction(Client.ftpClient, Client.localDirectory, selected);

                    // Carry out the action and get the result
                    DFtpResult result = action.Run();

                    // Give some feedback if successful
                    if (result.Type == DFtpResultType.Ok)
                    {
                        IOHelper.Message("files downloaded successfully.");
                    }
                    return result;
                }
                else
                {
                    return new DFtpResult(DFtpResultType.Error, "Error on the operation.");
                }
            }
                
                
            else
            {
                return new DFtpResult(DFtpResultType.Error, "Error on the operation.");
            }
        }
    }
}
