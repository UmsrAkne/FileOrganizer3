using System.Collections.ObjectModel;
using FileOrganizer3.Models;

namespace FileOrganizer3Tests.Models
{
    [TestFixture]
    public class FileContainerTests
    {
        private FileContainer fileContainer;

        [SetUp]
        public void SetUp()
        {
            fileContainer = new FileContainer
            {
                FileInfoWrappers = new ObservableCollection<FileInfoWrapper>()
                {
                    new FileInfoWrapper(new FileInfo("test01.mp3"), null) {Index = 0, },
                    new FileInfoWrapper(new FileInfo("test02.mp3"), null) {Index = 1, },
                    new FileInfoWrapper(new FileInfo("test03.mp3"), null) {Index = 2, },
                    new FileInfoWrapper(new FileInfo("test04.mp3"), null) {Index = 3, },
                    new FileInfoWrapper(new FileInfo("test05.mp3"), null) {Index = 4, },
                },
            };
        }

        [Test]
        public void ReIndex_AssignsSequentialIndicesToFiles_通常動作のテスト()
        {
            fileContainer.ReIndex(fileContainer.FileInfoWrappers);

            var results = new List<int>();
            foreach (var w in fileContainer.FileInfoWrappers)
            {
                results.Add(w.Index);
            }

            CollectionAssert.AreEqual(new [] { 1,2,3,4,5, }, results);
        }

        [Test]
        public void ReIndexTest_無視しているアイテムを含む()
        {
            fileContainer.FileInfoWrappers[0].IsIgnored = true;
            fileContainer.FileInfoWrappers[1].IsIgnored = true;

            fileContainer.ReIndex(fileContainer.FileInfoWrappers);

            var results = new List<int>();
            foreach (var w in fileContainer.FileInfoWrappers)
            {
                results.Add(w.Index);
            }

            CollectionAssert.AreEqual(new[] { 0, 0, 1, 2, 3, }, results);
        }

        [Test]
        public void ReIndexTest_全てのアイテムが無視状態()
        {
            foreach (var f in fileContainer.FileInfoWrappers)
            {
                f.IsIgnored = true;
            }

            fileContainer.ReIndex(fileContainer.FileInfoWrappers);

            var results = new List<int>();
            foreach (var w in fileContainer.FileInfoWrappers)
            {
                results.Add(w.Index);
            }

            CollectionAssert.AreEqual(new[] { 0, 0, 0, 0, 0, }, results);
        }
    }
}