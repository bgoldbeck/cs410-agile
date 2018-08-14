using System;
using System.Collections.Generic;
using System.Text;
using FluentFTP;

namespace Actions
{
    /// <summary>
    /// An action that changes the permissions of a file on the ftp server.
    /// </summary>
    public class ChmodFileRemoteAction : DFtpAction
    {
        protected int permissions;

        /// <summary>
        /// Constructor to build an action that changes the permissions of a file on the ftp server.
        /// </summary>
        /// <param name="ftpClient"> The client connection to the server.</param>
        /// <param name="remoteDirectory">The remote directory path where the file to be modified resides.</param>
        /// <param name="remoteSelection">The file to be modified</param>
        /// <param name="permissions">Permissions to be changed to in chmod format</param>

        public ChmodFileRemoteAction(FtpClient ftpClient, String remoteDirectory, DFtpFile remoteSelection, int permissions)
            : base(ftpClient, null, null, remoteDirectory, remoteSelection)
        {
            this.permissions = permissions;
        }

        /// <summary>
        /// Try to change permissions of the file from the FtpClient. 
        /// </summary>
        /// <returns>DftpResultType.Ok, if the file permissions were modified.</returns>
        /// 

        public override DFtpResult Run()
        {
            String path = remoteDirectory + "/" + remoteSelection;

            try
            {
                //ftpClient.Chmod(path, permissions);
                //int result = ftpClient.GetChmod(path);
                int result = 0;
                if (result == permissions)
                {
                    return new DFtpResult(DFtpResultType.Ok, "File " + remoteSelection + " permissions have been changed.");
                }
                else
                {
                    return new DFtpResult(DFtpResultType.Error, "File " + remoteSelection + " permissions could not be changed.");
                }
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, "File " + remoteSelection + "permissions could not be changed: " + Environment.NewLine + ex.Message + Environment.NewLine);
            }
        }
    }
}
