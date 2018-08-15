using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;

namespace XIntegrationTests
{
    public class ChmodFileRemoteTests
    {
        private static FtpClient client = null;
        private static String test_dir = "temp_chmod";
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

        internal DFtpFile CreateAndPutFileOnServer(FtpClient ftpClient, String path, String newFileName)
        {
            String filepath = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(filepath);
            DFtpAction action = new CreateDirectoryRemoteAction(ftpClient, filepath);
            DFtpResult test = action.Run();
            DFtpFile localSelection = new DFtpFile(filepath, FtpFileSystemObjectType.File);
            DFtpAction fileaction = new PutFileAction(client, localDirectory, localSelection, path);

            DFtpResult result = fileaction.Run();

            return localSelection;
        }

        internal bool ChmodFileOnServer(FtpClient ftpClient, String remoteDirectory, DFtpFile remoteSelection, int permissions)
        {
            DFtpAction action = new ChmodFileRemoteAction(ftpClient, remoteDirectory, remoteSelection, permissions);
            DFtpResult result = action.Run();
            if(result.Type == DFtpResultType.Ok)
            {
                return true;
            }
            else { return false; }
        }

        internal void RemoveFileOnServer(FtpClient ftpClient, DFtpFile file)
        {
            DFtpAction action = new DeleteFileRemoteAction(ftpClient, file.GetFullPath(), file);

            DFtpResult result = action.Run();
            return;
        }

        internal bool CheckChmodFileOnServer(FtpClient ftpClient, String remoteDirectory, DFtpFile remoteSelection, int permissions)
        {
            //GetChmod is bugged 
            FtpListItem[] list = ftpClient.GetListing(remoteDirectory + "/" + remoteSelection.GetName());
            int filePerms = list[0].Chmod;
            if(filePerms == permissions)
            {
                return true;
            }
            else { return false; }
        }

        internal bool SearchForFileOnServer(FtpClient ftpClient, String path, String pattern)
        {
            DFtpAction action = new SearchFileRemoteAction(ftpClient, pattern, path);

            DFtpResult result = action.Run();

            return result.Type == DFtpResultType.Ok ? true : false;
        }

        [Fact]
        public void CreateChmodRemoveTest()
        {
            EstablishConnection();
            if (client.DirectoryExists(test_dir))
            {
                client.DeleteDirectory(test_dir, FtpListOption.AllFiles);
            }

            // 1. Create and put file on server.
            DFtpFile newFile = CreateAndPutFileOnServer(client, test_dir, "NewFile");

            // 2. Search for file, make sure that it exists.
            Assert.True(SearchForFileOnServer(client, test_dir, newFile.GetName()));

            // 3. Change the file permissions
            Assert.True(ChmodFileOnServer(client, test_dir, newFile, 777));

            // 4. Check the file permissions 
            Assert.True(CheckChmodFileOnServer(client, test_dir, newFile, 777));

            // 5. Delete it
            client.DeleteFile(test_dir + "/" + newFile.GetName());

            // 6. We should NOT see the file on the server anymore
            Assert.False(SearchForFileOnServer(client, test_dir, newFile.GetName()));

            if (client.DirectoryExists(test_dir))
            {
                client.DeleteDirectory(test_dir, FtpListOption.AllFiles);
            }
            return;
        }
    }
}
