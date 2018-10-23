using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace FileManager.Tests
{
    [TestClass]
    public class FilesystemItemTests
    {
        private DirectoryInfo _sampleDirectory;
        private FileSystemInfo _sampleFile;

        [TestInitialize]
        public void Init()
        {
            var tempDir = Path.GetTempPath();
            this._sampleDirectory = Directory.CreateDirectory(Path.Combine(tempDir, "sampleDirectory"));
            var stream = File.Create(Path.Combine(_sampleDirectory.FullName, "file.txt"));
            stream.Dispose();
            this._sampleFile = _sampleDirectory.GetFiles("file.txt")[0];
        }

        [TestMethod]
        public void TestFileDetection()
        {
            FilesystemItem fsi = new FilesystemItem(_sampleDirectory);
            Assert.IsTrue(fsi.IsDirectory);

            fsi = new FilesystemItem(_sampleFile);
            Assert.IsFalse(fsi.IsDirectory);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _sampleDirectory.Delete(true);
        }
    }
}
