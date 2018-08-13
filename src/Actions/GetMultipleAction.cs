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
    public class GetMultipleAction : DFtpAction
    {
        private List<DFtpFile> multiFiles;

        /// <summary>
        /// Constructor to build an action that downloads a file from the ftp server.
        /// </summary>
        /// <param name="ftpClient"> The client connection to the server.</param>
        /// <param name="localDirectory">The local directory path</param>
        /// <param name="multiFiles">The remote files to download</param>
        public GetMultipleAction(FtpClient ftpClient, string localDirectory, List<DFtpFile> multiFiles)
            : base(ftpClient, localDirectory, null, null, null)
        {
            this.multiFiles = multiFiles;
        }
        /// <summary>
        /// Attempt to download multiple files from the FtpClient.
        /// </summary>
        /// <returns>DftpResultType.Ok, if the files are downloaded.</returns>
        public override DFtpResult Run()
        {
            //if remote source exists.
            if (multiFiles == null)
            {
                return new DFtpResult(DFtpResultType.Error, "please select some files before fetching from remote server");
            }

            int numberOfFiles = multiFiles.Count;

            try
            { 
                for (int i = 0; i < numberOfFiles; ++i)
                {
                    String localTarget = localDirectory + (this.isWindows() ? "\\" : "/") + multiFiles[i].GetName();
                    ftpClient.DownloadFile(localTarget, multiFiles[i].GetFullPath());
                    
                }
                return new DFtpResult(DFtpResultType.Ok, "Files are downloaded to local directory.");
            }
            
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, ex.Message);
            }
        }
    }
}

            