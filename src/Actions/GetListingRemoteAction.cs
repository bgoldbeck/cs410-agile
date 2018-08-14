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
                if (view_hidden == true)
                {
                    List<DFtpFile> dFtpListing = new List<DFtpFile>();
                    FtpListItem[] fluentListing = ftpClient.GetListing(remoteDirectory, FtpListOption.AllFiles);
                    PopulateList(fluentListing, ref dFtpListing);
                    return new DFtpListResult(DFtpResultType.Ok, "Got listing for " + remoteDirectory, dFtpListing);
                }
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
            foreach (FtpListItem item in result)
            {
                list.Add(new DFtpFile(item));
            }
            list.Sort();
        }
    }
}
