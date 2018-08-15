using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

using FluentFTP;

namespace Actions
{
    public class GetListingLocalAction : DFtpAction
    {
        protected bool view_hidden;
        public GetListingLocalAction(String targetDirectory, bool view_hidden) :
            base(null , targetDirectory, null, null , null)
        {
            this.view_hidden = view_hidden;
        }

        public override DFtpResult Run()
        {
            try
            {
                if (Directory.Exists(localDirectory))
                {
                    //Create an empty list of DftpFiles to store our file list
                    List<DFtpFile> dFtpLocalListing = new List<DFtpFile>();
                    //Grab all directories in the provided directory and store them in the list
                    PopulateLocalList(Directory.GetDirectories(localDirectory), ref dFtpLocalListing, false, view_hidden);
                    //Grab all files in the provided directory and store them in the list
                    PopulateLocalList(Directory.GetFiles(localDirectory), ref dFtpLocalListing, true, view_hidden);
                    //return the completed list
                    return new DFtpListResult(DFtpResultType.Ok, "Got listing for " + localDirectory, dFtpLocalListing);
                }
                else
                {
                    //if directory does not exist return a failure
                    return new DFtpResult(DFtpResultType.Error, "Directory does not exist");
                }
            }
            catch(UnauthorizedAccessException ex)
            {
                return new DFtpResult(DFtpResultType.Error, "You do not have permission to access this directory. "+ ex.Message);
            }
        }

        private void PopulateLocalList(String[] result, ref List<DFtpFile> list, bool isfile, bool hidden)
        {
            if(isfile == true)
            {
                foreach (String item in result)
                {
                    bool isHidden = (File.GetAttributes(item) & FileAttributes.Hidden) == FileAttributes.Hidden;
                    if (hidden == false && isHidden == false)
                    {
                        list.Add(new DFtpFile((item), FtpFileSystemObjectType.File));
                    }
                    else if(hidden == true)
                    {
                        list.Add(new DFtpFile((item), FtpFileSystemObjectType.File));
                    }
                }
            }
            else
            {
                foreach (String item in result)
                {
                    if (hidden == false && ((File.GetAttributes(item) & FileAttributes.Hidden) == FileAttributes.Hidden))
                    {
                        list.Add(new DFtpFile((item), FtpFileSystemObjectType.Directory));
                    }
                    else
                    {
                        list.Add(new DFtpFile((item), FtpFileSystemObjectType.Directory));
                    }
                }
            }
        }
    }
}
