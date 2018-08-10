using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FluentFTP;

namespace Actions
{
    /// <summary>
    /// An action for searching for a file on the local server.
    /// </summary>
    public class SearchFileLocalAction : DFtpAction
    {
        protected String pattern;
        protected String startPath;
        protected bool includeSubdirectories;

        public SearchFileLocalAction( String pattern, String startPath, bool includeSubdirectories = true)
            : base(null, null, null, null, null)
        {
            this.pattern = pattern;
            this.startPath = startPath;
            this.includeSubdirectories = includeSubdirectories;
        }

        /// <summary>
        /// Run this action.
        /// </summary>
        /// <returns>The DFtpResult casted as DFtpListResult that contains the files that were found.</returns>
        public override DFtpResult Run()
        {
            List<DFtpFile> found = new List<DFtpFile>();
            GetFilesForDirectory(Directory.GetFiles(this.startPath), ref found, true);
            GetFilesForDirectory(Directory.GetFiles(this.startPath), ref found, false);

            List<DFtpFile> filtered = new List<DFtpFile>();
            foreach (DFtpFile item in found)
            {
                if (item.GetName().Contains(this.pattern))
                {
                    filtered.Add(item);
                }
            }

            String info = new String("Searched for pattern " + Path.DirectorySeparatorChar + pattern + Path.DirectorySeparatorChar + "on local server in"
                + Path.DirectorySeparatorChar + startPath + Path.DirectorySeparatorChar);

            return filtered.Count > 0 ?
                new DFtpListResult(DFtpResultType.Ok, info + " [Found: " + filtered.Count + " files]", filtered) :
                new DFtpResult(DFtpResultType.Error, info + " [No files found]");
        }

        /// <summary>
        /// Search the local server for a particular file.
        /// </summary>
        /// <param name="result"> stores files</param>
        /// <param name="list">Total list</param>
        /// <param name="isFile">checks for file</param>

        private void GetFilesForDirectory(String[] result, ref List<DFtpFile> list, bool isFile)
        {
            if (result == null || result.Length == 0)
            {
                return;
            }

            if (isFile == true)
            {
                foreach (String item in result)
                {
                    list.Add(new DFtpFile((item), FtpFileSystemObjectType.File));
                }
            }
            else
            {
                foreach (String item in result)
                {
                   if (Directory.Exists(item))
                   {
                       GetFilesForDirectory(Directory.GetFiles(item), ref list, true);
                       GetFilesForDirectory(Directory.GetDirectories(item), ref list, false);
                   }
                }
            }
        }
    }
}
