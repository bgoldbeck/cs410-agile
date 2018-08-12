using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using FluentFTP;

namespace Actions
{
    /// <summary>
    /// An action that removes a file from the local machine.
    /// </summary>
    public class DeleteFileLocalAction : DFtpAction
    {
        /// <summary>
        /// Constructor to build an action that removes a file from the local machine.
        /// </summary>
        /// <param name="ftpClient"> The client connection to the server.</param>
        /// <param name="localDirectory">The local directory path where the file to delete resides.</param>
        /// <param name="localSelection">The file to remove</param>
        public DeleteFileLocalAction(FtpClient ftpClient, String localDirectory, DFtpFile localSelection)
            : base(ftpClient, localDirectory, localSelection, null, null)
        {
        }

        /// <summary>
        /// Attempt to remote the file from the local machine.
        /// </summary>
        /// <returns>DftpResultType.Ok, if the file was removed.</returns>
        public override DFtpResult Run()
        {
            String target = localDirectory + Path.DirectorySeparatorChar + localSelection.GetName();
            
            try
            { 
                // System.IO -- Delete me file. pls.
                File.Delete(target);

                return File.Exists(target) == false ?
                    new DFtpResult(DFtpResultType.Ok, "File with path \"" + target + "\" removed from local machine.") :  
                    new DFtpResult(DFtpResultType.Error, "file with path \"" + target + "\" could not be removed from local machine.");
            }
            catch (Exception ex)
            {
                return new DFtpResult(DFtpResultType.Error, "file with path \"" + target + "\" " +
                    "could not be removed from local machine." + Environment.NewLine + ex.Message + Environment.NewLine);
            }
        }
    }
}
