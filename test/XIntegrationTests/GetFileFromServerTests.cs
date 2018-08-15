using Actions;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace XIntegrationTests
{
    public class GetFileFromServerTests
    {
        [Fact]
        //testing for files in the current directory
        internal bool GetFileFromServer()
        {
            String sourceDir = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Source";
            String desinationtDir = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Destination";

            if (Directory.Exists(sourceDir))
            {
                Directory.Delete(sourceDir, true);
            }

            if (Directory.Exists(desinationtDir))
            {
                Directory.Delete(desinationtDir, true);
            }

            Directory.CreateDirectory(sourceDir);
            Directory.CreateDirectory(desinationtDir);

            String fileName = "MyFile.txt";
            String filePath = sourceDir + Path.DirectorySeparatorChar + fileName;
            FileStream fs = File.Create(filePath);
            fs.Close();

            DFtpFile file = new DFtpFile(filePath, FluentFTP.FtpFileSystemObjectType.File);
            FtpClient client = new FtpClient(sourceDir);

            GetFileFromRemoteServerAction action = new GetFileFromRemoteServerAction(client, desinationtDir, sourceDir, file);
            DFtpResult result = action.Run();

            String destinationFilePath = desinationtDir + Path.DirectorySeparatorChar + fileName;
            return File.Exists(destinationFilePath);
        }
    }
}
