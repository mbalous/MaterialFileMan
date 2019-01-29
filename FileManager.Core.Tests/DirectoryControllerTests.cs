using System;
using System.IO;
using System.Linq;
using Xunit;

namespace FileManager.Core.Tests
{
    public class DirectoryControllerTests : IDisposable
    {
        private readonly Random rand = new Random();
        private DirectoryInfo _testDirectory;
        
        public DirectoryControllerTests()
        {
            string tempDir = Path.GetTempPath();
            this._testDirectory = Directory.CreateDirectory(Path.Combine(tempDir, $"sampleDirectory{rand.Next(100, 999)}"));
        }

        [Fact]
        public void TestChildItems()
        {
            string fileName = GetRandomString(10);
            string fullname = CreateFileInDirectoryInternal(fileName, this._testDirectory);

            DirectoryController dc = new DirectoryController(_testDirectory.FullName, false);
            FileSystemInfo[] childItems = dc.ChildItems;
            Assert.Equal(fullname, childItems[0].FullName);
        }

        [Fact]
        public void TestDelete()
        {
            string fileName = GetRandomString(10);
            string fullname = CreateFileInDirectoryInternal(fileName, this._testDirectory);

            DirectoryController dc = new DirectoryController(_testDirectory.FullName, false);
            dc.DeleteChildItem(fileName);
            FileSystemInfo[] childItems = dc.ChildItems;
            Assert.DoesNotContain(childItems, x => x.FullName == fullname);

        }

        public void Dispose()
        {
            if (this._testDirectory.Exists)
            {
                this._testDirectory.Delete(true);
            }
        }


        private string GetRandomString(int lenth)
        {
            // courtesy of stackoverflow, lel

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, lenth)
              .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        private string CreateFileInDirectoryInternal(string fileName, DirectoryInfo directory)
        {
            string fullFileName = Path.Combine(directory.FullName, fileName);
            using (FileStream stream = File.Create(fullFileName))
            {
                
            }

            return fullFileName;
        }
    }
}
