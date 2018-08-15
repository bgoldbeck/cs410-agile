using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XIntegrationTests
{
    public class LocalPutDeleteTests
    {
        private static FtpClient client = null;
        private static string test_Dir = "local_put_delete_tests";
        internal FtpClient EstablishConnection()
        {
            if (client == null)
            {
                client = new FtpClient("hypersweet.com")
                {
                    Port = 21,
                    Credentials = new NetworkCredential("cs410", "cs410")
                };
            }
            return client;
        }

        internal void CreateAndPutFileOnLocal(String path, String originalName)
        {
            Directory.CreateDirectory(path);
            File.Create(path + Path.DirectorySeparatorChar + originalName);
        }

        internal DFtpFile CreateFileOnLocal()
        {
            DFtpFile tempFile = new DFtpFile(Path.GetTempFileName(),FtpFileSystemObjectType.File);
            return tempFile;
        }

        internal bool SearchForLocalFile(String fullPath)
        {
            if (File.Exists(fullPath)) {
                return true;
            }
            else { return false; }
        }

        internal void DeleteLocalFile(DFtpFile localSelection)
        {
            DFtpAction action = new DeleteFileLocalAction(localSelection);
            DFtpResult result = action.Run();
            return;
        }

        [Fact]
        public void CreatePutDeleteTest()
        {
            // 1. Create and put file on local machine.
            DFtpFile tempFile = CreateFileOnLocal();

            // 2. Search for file, make sure that it exists.
            Assert.True(SearchForLocalFile(tempFile.GetFullPath()));

            // 3.  Delete the file
            DeleteLocalFile(tempFile);

            // 4. We should NOT see the file on the local machine anymore
            Assert.False(SearchForLocalFile(tempFile.GetFullPath()));
            
            return;
        }
    }
}
