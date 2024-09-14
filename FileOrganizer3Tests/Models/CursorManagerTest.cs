using System.Collections.ObjectModel;
using FileOrganizer3.Models;

namespace FileOrganizer3Tests.Models
{
    [TestFixture]
    public class CursorManagerTest
    {

        private CursorManager cursorManager;

        [SetUp]
        public void Setup()
        {
            cursorManager = new CursorManager()
            {
                Items = new ObservableCollection<FileInfoWrapper>()
                {
                    new (new FileInfo("a")) { IsMarked = false, },
                    new (new FileInfo("a")) { IsMarked = false, },
                    new (new FileInfo("a")) { IsMarked = false, },
                    new (new FileInfo("a")) { IsMarked = false, },
                    new (new FileInfo("a")) { IsMarked = false, },
                },

                SelectedIndex = 0,
            };
        }

        [Test]
        public void MoveCursorToNextMarkTest_要素1()
        {
            cursorManager.Items[1].IsMarked = true;

            cursorManager.MoveCursorToNextMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(1));

            cursorManager.MoveCursorToNextMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(1));
        }

        [Test]
        public void MoveCursorToNextMarkTest_要素2()
        {
            cursorManager.Items[1].IsMarked = true;
            cursorManager.Items[3].IsMarked = true;

            cursorManager.MoveCursorToNextMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(1));

            cursorManager.MoveCursorToNextMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(3));

            cursorManager.MoveCursorToNextMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(1));

            cursorManager.MoveCursorToNextMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(3));
        }

        [Test]
        public void MoveCursorToPrevMark_要素1()
        {
            cursorManager.Items[1].IsMarked = true;

            cursorManager.MoveCursorToPrevMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(1));

            cursorManager.MoveCursorToPrevMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(1));
        }

        [Test]
        public void MoveCursorToPrevMark_要素2()
        {
            cursorManager.Items[1].IsMarked = true;
            cursorManager.Items[3].IsMarked = true;

            cursorManager.MoveCursorToPrevMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(3));

            cursorManager.MoveCursorToPrevMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(1));

            cursorManager.MoveCursorToPrevMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(3));

            cursorManager.MoveCursorToPrevMark();
            Assert.That(cursorManager.SelectedIndex, Is.EqualTo(1));
        }
    }
}