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


            for (int i = 1; i <= 3; i += 1)
            {
                FileStream file = File.Create(path + Path.DirectorySeparatorChar + "File" + i + ".txt");
                file.Close();
            }

            DFtpAction action = new SearchFileLocalAction("File", path, true);

            DFtpResult result = action.Run();

            bool isListType = typeof(DFtpListResult).IsInstanceOfType(result);

            Directory.Delete(path, true);

            if (isListType)
            {
                DFtpListResult listResult = (DFtpListResult)result;
                return listResult.Files.Count == 3;
            }

            return false;
        }

        [Fact]
        //testing for files in subdirectory with recursive check false
        internal bool SearchFileLocalSubDirFailure()
        {
            String path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "TestDir";
            String subdirectoryPAth = path + Path.DirectorySeparatorChar + "SubDir";
            //  Directory.CreateDirectory(path);
            Directory.CreateDirectory(subdirectoryPAth);

            for (int i = 1; i <= 3; i += 1)
            {
                FileStream file = File.Create(subdirectoryPAth + Path.DirectorySeparatorChar + "File" + i + ".txt");
                file.Close();
            }

            DFtpAction action = new SearchFileLocalAction("File", path, false);

            DFtpResult result = action.Run();

            bool isListType = typeof(DFtpListResult).IsInstanceOfType(result);

            Directory.Delete(path, true);

            return !isListType;
        }
        [Fact]
        //testing for subdirectory files from parent directory and it should return zero
        internal bool SearchFileLocalSubDirSuccess()
        {
            String path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Test";
            String subdirectoryPAth = path + Path.DirectorySeparatorChar + "subdirec";
            //  Directory.CreateDirectory(path);
            Directory.CreateDirectory(subdirectoryPAth);

            for (int i = 1; i <= 3; i += 1)
            {
                FileStream file = File.Create(subdirectoryPAth + Path.DirectorySeparatorChar + "File" + i + ".txt");
                file.Close();
            }

            DFtpAction action = new SearchFileLocalAction("File", path, true);

            DFtpResult result = action.Run();

            bool isListType = typeof(DFtpListResult).IsInstanceOfType(result);

            Directory.Delete(path, true);

            if (isListType)
            {
                DFtpListResult listResult = (DFtpListResult)result;
                return listResult.Files.Count == 3;
            }

            return false;
        }
    }
}