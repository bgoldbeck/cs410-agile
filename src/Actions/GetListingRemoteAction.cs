using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;

namespace Actions
{
    public class GetListingRemoteAction : DFtpAction
    {
        protected bool view_hidden;
        public GetListingRemoteAction(FtpClient ftpClient, String targetDirectory, bool view_hidden) : 
            base(ftpClient, null, null, targetDirectory, null)
        {
            this.view_hidden = view_hidden;
        }

        public override DFtpResult Run()
        {
            try
            {
                //if we are viewing hidden
                if (view_hidden == true)
                {
                    //Create an empty list to store our result
                    List<DFtpFile> dFtpListing = new List<DFtpFile>();
                    //Get the listing from the server with the all files flag
                    FtpListItem[] fluentListing = ftpClient.GetListing(remoteDirectory, FtpListOption.AllFiles);
                    //Call our populate list function on the given listing to fill out dFtpListing
                    PopulateList(fluentListing, ref dFtpListing);
                    return new DFtpListResult(DFtpResultType.Ok, "Got listing for " + remoteDirectory, dFtpListing);
                }
                //Not viewing hidden
                else
                {
                    List<DFtpFile> dFtpListing = new List<DFtpFile>();
                    FtpListItem[] fluentListing = ftpClient.GetListing(remoteDirectory);
                    PopulateList(fluentListing, ref dFtpListing);
                    return new DFtpListResult(DFtpResultType.Ok, "Got listing for " + remoteDirectory, dFtpListing);
                }
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, ex.Message); // FluentFTP didn't like something.
            }
        }

        private void PopulateList(FtpListItem[] result, ref List<DFtpFile> list)
        {
            //loops through the ftplistitem array given by fluentftp
            foreach (FtpListItem item in result)
            {
                //Add each file
                list.Add(new DFtpFile(item));
            }
            //Sort the final list
            list.Sort();
        }
    }
}
