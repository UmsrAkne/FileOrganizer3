using System.Collections.Generic;
using System.IO;

namespace FileOrganizer3.Models
{
    public class DummyFileProvider
    {
        public static IEnumerable<FileInfoWrapper> GetDummyFiles()
        {
            return new List<FileInfoWrapper>()
            {
                new (new FileInfo("test_test_test111.mp3")) { Index = 1, },
                new (new FileInfo("test_test_test222.mp3")) { Index = 2, IsIgnored = true, },
                new (new FileInfo("test_test_test333.mp3")) { Index = 3, IsMarked = true, TemporaryName = "TempName.mp3", },
                new (new FileInfo("test_test_test444.mp3")) { Index = 4, IsMarked = true, IsIgnored = true, },
                new (new FileInfo("test_test_test555.mp3")) { Index = 5, Playing = true, },
                new (new FileInfo("test_test_test666.mp3")) { Index = 6, },
                new (new FileInfo("test_test_test444.mp3")) { Index = 99, },
                new (new FileInfo("testSounds\\se1.mp3")) { Index = 100, },
                new (new FileInfo("testSounds\\se2.ogg")) { Index = 101, },
                new (new FileInfo("testSounds\\se3.wav")) { Index = 102, },
            };
        }
    }
}