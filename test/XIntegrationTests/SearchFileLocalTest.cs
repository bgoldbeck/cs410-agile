using System;
using Xunit;

using Actions;
using System.Collections.Generic;
using FluentFTP;
using System.Net;
using System.IO;
namespace XIntegrationTests
{

    public class SearchFileLocalTest
    {
        [Fact]
        //testing for files in the current directory
        internal bool SearchFileLocal()
        {
            String path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Test";
            Directory.CreateDirectory(path);
            //  Directory.CreateDirectory(path);


            File.Create(path + Path.PathSeparator + "File1.txt");
            File.Create(path + Path.PathSeparator + "File2.txt");
            File.Create(path + Path.PathSeparator + "File3.txt");

            DFtpAction action = new SearchFileLocalAction(path, "File", true);

            DFtpResult result = action.Run();

            bool isListType = typeof(DFtpListResult).IsInstanceOfType(result);

            Directory.Delete(path);

            if (isListType)
            {
                DFtpListResult listResult = (DFtpListResult)result;
                return listResult.Files.Count == 3;
            }

            return false;
        }
        [Fact]
        //testing for subdirectory files from parent directory and it should return zero
        internal bool SearchFileLocalSubDirFailure()
        {
            String path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Test";
            String subdirectoryPAth = path + Path.DirectorySeparatorChar + "subdirec";
            //  Directory.CreateDirectory(path);
            Directory.CreateDirectory(subdirectoryPAth);


            File.Create(subdirectoryPAth + Path.DirectorySeparatorChar + "File1.txt");
            File.Create(subdirectoryPAth + Path.DirectorySeparatorChar + "File2.txt");
            File.Create(subdirectoryPAth + Path.DirectorySeparatorChar + "File3.txt");
            DFtpAction action = new SearchFileLocalAction(path, "File", false);

            DFtpResult result = action.Run();

            bool isListType = typeof(DFtpListResult).IsInstanceOfType(result);

            Directory.Delete(path);

            if (isListType)
            {
                DFtpListResult listResult = (DFtpListResult)result;
                return listResult.Files.Count == 0;
            }

            return false;
        }
    }
}