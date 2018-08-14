using System;
using System.Collections.Generic;
using System.Text;

namespace UI
{
    public class ViewHiddenUI : IDFtpUI
    {
        public ConsoleKey Key => ConsoleKey.H;

        public bool RequiresLogin => true;

        public bool RequiresSelection => false;

        public bool HideForDirectory => false;

        public bool HideForFile => false;

        public bool HideForLocal => false;

        public bool HideForRemote => false;

        public string MenuText => "View (H)idden";

        public DFtpResult Go()
        {
            Client.view_hidden = Client.view_hidden == false ? true : false;
            return new DFtpResult(DFtpResultType.Ok, "View hidden changed to " + Client.view_hidden);
        }
    }
}
