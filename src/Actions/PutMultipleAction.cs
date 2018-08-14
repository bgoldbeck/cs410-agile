using System;
using System.Collections.Generic;
using System.Text;

using FluentFTP;
using IO;

namespace Actions
{
    /// <summary>
    /// An action that Downloads a file from the ftp server.
    /// </summary>
    public class PutMultipleAction : DFtpAction
    {
        private List<DFtpFile> multiFiles;
        private bool overwrite = true;

        /// <summary>
        /// Constructor to build an action that downloads a file from the ftp server.
        /// </summary>
        /// <param name="ftpClient"> The client connection to the server.</param>
        /// <param name="localDirectory">The local directory path</param>
        /// <param name="multiFiles">The remote files to download</param>
        public PutMultipleAction(FtpClient ftpClient, List<DFtpFile> multiFiles, String remoteDirectory, bool overwrite = true)
            : base(ftpClient, null, null, remoteDirectory, null)
        {
            this.multiFiles = multiFiles;
            this.overwrite = overwrite;
        }
        /// <summary>
        /// Attempt to upload multiple files to the FtpClient.
        /// </summary>
        /// <returns>DftpResultType.Ok, if the files are uploaded.</returns>
        public override DFtpResult Run()
        {
            //if remote source exists.
            if (multiFiles == null)
            {
                return new DFtpResult(DFtpResultType.Error, "please select some files from local directory.");
            }

            int numberOfFiles = multiFiles.Count;

            FtpExists existsMode = overwrite ? FtpExists.Overwrite : FtpExists.Skip;
            bool createDirectoryStructure = true;
            FtpVerify verifyMode = FtpVerify.Retry;
            ftpClient.RetryAttempts = 3;

            try
            { 
                for (int i = 0; i < numberOfFiles; ++i)
                {
                    String remoteTarget = remoteDirectory + (this.isWindows() ? "\\" : "/") + multiFiles[i].GetName();
                    ftpClient.UploadFile(multiFiles[i].GetFullPath(), remoteTarget, existsMode, createDirectoryStructure, verifyMode);
                }
                return new DFtpResult(DFtpResultType.Ok, "Files are uploaded to remote directory.");
            }

            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, ex.Message);
            }
        }
    }
}

