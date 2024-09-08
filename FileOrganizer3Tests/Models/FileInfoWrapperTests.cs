using System.IO.Abstractions;
using Moq;
using FileInfoWrapper = FileOrganizer3.Models.FileInfoWrapper;

namespace FileOrganizer3Tests.Models
{
    [TestFixture]
    public class FileInfoWrapperTests
    {
        [Test]
        public void Rename_テスト()
        {
            var fileSystemMock = new Mock<IFileSystem>();
            var fileInfoMock = new Mock<IFileInfo>();

            fileInfoMock.Setup(f => f.Directory!.FullName).Returns("C:\\Test");
            var fileInfoWrapper = new FileInfoWrapper(null, fileInfoMock.Object, fileSystemMock.Object)
            {
                TemporaryName = "newName.txt",
            };

            fileInfoWrapper.Rename();
            fileInfoMock.Verify(f => f.MoveTo(@"C:\Test\newName.txt"), Times.Once);
        }
    }
}