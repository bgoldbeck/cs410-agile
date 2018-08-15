using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;
using IO;
using System.Linq;


namespace XIntegrationTests
{
    public class PutAndGetMultipleTests
    {
        private static FtpClient client = null;
        private const String RemoteTestDirectory = "/remote_multiple_test_directory";
        private const String LocalTestDirectory = @"C:\TestD";

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
            DFtpFile tempFile = new DFtpFile(Path.GetTempFileName(), FtpFileSystemObjectType.File);
            return tempFile;
        }

        internal bool SearchForLocalFile(String fullPath)
        {
            if (File.Exists(fullPath))
            {
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

        internal void RemoveFileOnServer(FtpClient ftpClient, DFtpFile file)
        {
            DFtpFile remoteSelection = file;

            DFtpAction action = new DeleteFileRemoteAction(ftpClient, RemoteTestDirectory, remoteSelection);

            DFtpResult result = action.Run();
            return;
        }

        internal bool SearchForFileOnServer(FtpClient ftpClient, String pattern)
        {
            DFtpAction action = new SearchFileRemoteAction(ftpClient, pattern, RemoteTestDirectory);

            DFtpResult result = action.Run();

            return result.Type == DFtpResultType.Ok ? true : false;
        }

        internal DFtpFile CreateAndPutFileInDirectoryOnServer(FtpClient ftpClient, String remoteDirectory = RemoteTestDirectory)
        {
            String filepath = Path.GetTempFileName();
            String localDirectory = Path.GetDirectoryName(filepath);
            DFtpFile localSelection = new DFtpFile(filepath, FtpFileSystemObjectType.File);

            DFtpAction action = new PutFileAction(client, localDirectory, localSelection, remoteDirectory);

            DFtpResult result = action.Run();

            return localSelection;
        }

        //internal void GetMultipleFromRemoteServer(FtpClient ftpClient, String localDirectory, List<DFtpFile> files)
        //{

        //  DFtpAction action = new GetMultipleAction(ftpClient, localDirectory, files);

        //DFtpResult result = action.Run();
        //return;
        //}




        [Fact]
        public void PutMultipleTest()
        {
            //Create and put 3 files on local machine.
            List<DFtpFile> files = new List<DFtpFile>();
            for (int i = 0; i < 3; ++i)
            {
                files.Add(CreateFileOnLocal());
            }

            // Get multiple files from the directory
            DFtpAction listingAction = new GetListingLocalAction(LocalTestDirectory, true);
            DFtpResult tempList = listingAction.Run();

            DFtpListResult listResult = null;

            if (tempList is DFtpListResult)
            {
                listResult = (DFtpListResult)tempList;
                List<DFtpFile> list = listResult.Files.Where(x => x.Type() == FtpFileSystemObjectType.File).ToList();
                DFtpAction action = new PutMultipleAction(client, list, RemoteTestDirectory, true);
                DFtpResult result = action.Run();
                if (result is DFtpListResult)
                {
                    listResult = (DFtpListResult)result;
                    Assert.True(listResult.Files.Count == 3);
                }
                else
                {
                    return;
                }
            }

            else
            {
                return;
            }

            foreach (DFtpFile file in files)
            {
                // Delete each file
                RemoveFileOnServer(client, file);

                // Make sure it's gone
                Assert.False(SearchForFileOnServer(client, file.GetName()));
            }
            if (client.DirectoryExists(RemoteTestDirectory))
            {
                client.DeleteDirectory(RemoteTestDirectory);
            }

            return;
        }


        [Fact]
        public void GetMultipleTest()
        {
            EstablishConnection();
            if (client.DirectoryExists(RemoteTestDirectory))
            {
                client.DeleteDirectory(RemoteTestDirectory);
            }
            List<DFtpFile> files = new List<DFtpFile>();
            // Create and put 3 files on server.
            for (int i = 0; i < 3; ++i)
            {
                files.Add(CreateAndPutFileInDirectoryOnServer(client));
            }

            // Get multiple files from the directory
            DFtpAction listingAction = new GetListingRemoteAction(client, RemoteTestDirectory, true);
            DFtpResult tempList = listingAction.Run();

            DFtpListResult listResult = null;

            if (tempList is DFtpListResult)
            {
                listResult = (DFtpListResult)tempList;
                List<DFtpFile> list = listResult.Files.Where(x => x.Type() == FtpFileSystemObjectType.File).ToList();
                DFtpAction action = new GetMultipleAction(client, LocalTestDirectory, list);
                DFtpResult result = action.Run();
                if (result is DFtpListResult)
                {
                    listResult = (DFtpListResult)result;
                    Assert.True(listResult.Files.Count == 3);
                }
                else
                {
                    return;
                }
            }

            else
            {
                return;
            }

            foreach (DFtpFile file in files)
            {
                // Delete each file
                RemoveFileOnServer(client, file);

                // Make sure it's gone
                Assert.False(SearchForFileOnServer(client, file.GetName()));
            }
            if (client.DirectoryExists(RemoteTestDirectory))
            {
                client.DeleteDirectory(RemoteTestDirectory);
            }

            return;
        }
    }
}
       