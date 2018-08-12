using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XIntegrationTests
{
    public class LocalCreateDirectoryTests
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

        internal void CreateDirectoryLocal(FtpClient ftpClient, String directoryPath)
        {
            DFtpAction action = new CreateDirectoryLocalAction(client, directoryPath);
            DFtpResult result = action.Run();
            return;
        }

        internal bool SearchForLocalDirectory(String path)
        {
            if (Directory.Exists(path)) {
                return true;
            }
            else { return false; }
        }

        internal void DeleteDirectoryLocal(String directoryPath)
        {
            Directory.Delete(directoryPath);
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
            String directoryPath = test_Dir + Path.DirectorySeparatorChar + "NewDirectory";

            // 1. Create and put directory on local machine.
            CreateDirectoryLocal(client, directoryPath);

            // 2. Search for directory, make sure that it exists.
            Assert.True(SearchForLocalDirectory(directoryPath));

            // 3.  Delete the directory
            DeleteDirectoryLocal(directoryPath);

            // 4. We should NOT see the directory on the local machine anymore
            Assert.False(SearchForLocalDirectory(directoryPath));
            if (client.DirectoryExists(test_Dir))
            {
                client.DeleteDirectory(test_Dir);
            }
            return;
        }
    }
}
