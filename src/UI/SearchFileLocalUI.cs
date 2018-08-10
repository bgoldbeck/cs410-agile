using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Actions;

using FluentFTP;
using IO;

namespace UI
{
    public class SearchFileLocalUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.E;

        public bool RequiresLogin => true;

        public bool RequiresSelection => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => true;

        public string MenuText => "S(e)arch in local file/directory";
    
        public DFtpResult Go()
        {
            String pattern = IOHelper.AskString("What pattern to search for?");
            bool searchRecursive = IOHelper.AskBool("Include subdirectories?", "yes", "no");

            //create an action and initialize with the info we have collected
            DFtpAction action = new SearchFileLocalAction(pattern, Client.localDirectory, searchRecursive);

            DFtpResult result = action.Run();
            //check if the file is present in the list
            if (typeof(DFtpListResult).IsInstanceOfType(result))
            {
                DFtpListResult listResult = (DFtpListResult)result;
                IOHelper.Select<DFtpFile>("Search Result:", listResult.Files, true); 
            } else
            {
                bool searchAgain = IOHelper.AskBool("Unable to find any file with pattern: " + pattern + ". Do you want to search again?", "yes", "no");
                if (searchAgain)
                {
                    Go();
                }
            }

            return result;
        }
    }
}
