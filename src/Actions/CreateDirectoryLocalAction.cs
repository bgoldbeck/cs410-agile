using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using FluentFTP;

namespace Actions
{
    /// <summary>
    /// Create a directory on the local machine, also creating other parent directories on the way.
    /// </summary>
    public class CreateDirectoryLocalAction : DFtpAction
    {
        private String newDirectoryPath = null;

        /// <summary>
        /// Constructor for action.
        /// </summary>
        /// <param name="ftpClient">Currently connected client.</param>
        /// <param name="newDirectoryPath">Path of the directory to create.</param>
        public CreateDirectoryLocalAction(FtpClient ftpClient, String newDirectoryPath) : 
            base(ftpClient, null, null, null, null)
        {
            this.newDirectoryPath = newDirectoryPath;
        }

        public override DFtpResult Run()
        {
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(newDirectoryPath);
                return new DFtpResult(DFtpResultType.Ok, "Created directory " + di.FullName);
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, ex.Message);
            }
        }
    }
}
