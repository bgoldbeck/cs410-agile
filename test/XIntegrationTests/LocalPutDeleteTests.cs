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
        private static string test_Dir = "/Test1";
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
            File.Create(path + "/" + originalName);
        }

        internal DFtpFile CreateAndPutFileOnLocal(FtpClient ftpClient, String filePath, String newFileName)
        {
            DFtpFile localSelection = new DFtpFile(filePath + "/" + newFileName, FtpFileSystemObjectType.File, newFileName);
            Directory.CreateDirectory(filePath);
            FileStream newStream = File.Create(filePath + "/" + newFileName);
            newStream.Close();

            return localSelection;
        }

        internal bool SearchForLocalFile(String path, String filename)
        {
            if (File.Exists(path + "/" + filename)) {
                return true;
            }
            else { return false; }
        }

        internal void DeleteLocalFile(FtpClient ftpClient, String path, DFtpFile localSelection)
        {
            DFtpAction action = new DeleteFileLocalAction(ftpClient, test_Dir, localSelection);
            DFtpResult result = action.Run();
            return;
        }

        [Fact]
        public void CreatePutDeleteTest()
        {
            EstablishConnection();
            if (client.DirectoryExists(test_Dir))
            {
                client.DeleteDirectory(test_Dir);
            }
            // 1. Create and put file on server.
            DFtpFile newFile = CreateAndPutFileOnLocal(client, test_Dir, "NewFile");

            // 2. Search for file, make sure that it exists.
            Assert.True(SearchForLocalFile(test_Dir, "NewFile"));

            // 3.  Delete the file
            DeleteLocalFile(client, test_Dir, newFile);

            // 4. We should NOT see the file on the server anymore
            Assert.False(SearchForLocalFile(test_Dir, "NewFile"));
            if (client.DirectoryExists(test_Dir))
            {
                client.DeleteDirectory(test_Dir);
            }
            return;
        }
    }
}
