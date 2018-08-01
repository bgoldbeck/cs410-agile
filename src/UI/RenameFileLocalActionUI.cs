﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;
using IO;

namespace UI
{
    public class RenameFileLocalActionUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.N;

        public bool RequiresLogin => true;

        public bool RequiresFile => true;

        public bool HideForDirectory => true;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => true;

        public string MenuText => "Re(N)ame File";


        public DFtpResult Go()
        {
            String newName = IOHelper.AskString("Enter replacement name:");
            // Create the action
            // Initialize it with the info we've collected
            DFtpAction action = new RenameFileLocalAction(Client.ftpClient, Client.localDirectory, Client.localSelection, newName);

            // Carry out the action and get the result
            DFtpResult result = action.Run();
            
            return result;
        }
    }
}