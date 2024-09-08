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
                new (new FileInfo("test_test_test111.mp3"), null) { Index = 1, },
                new (new FileInfo("test_test_test222.mp3"), null) { Index = 2, IsIgnored = true, },
                new (new FileInfo("test_test_test333.mp3"), null) { Index = 3, IsMarked = true, },
                new (new FileInfo("test_test_test444.mp3"), null) { Index = 4, IsMarked = true, IsIgnored = true, },
                new (new FileInfo("test_test_test555.mp3"), null) { Index = 5, Playing = true, },
                new (new FileInfo("test_test_test666.mp3"), null) { Index = 6, },
                new (new FileInfo("test_test_test444.mp3"), null) { Index = 99, },
                new (new FileInfo("testSounds\\se1.mp3"), null) { Index = 100, },
                new (new FileInfo("testSounds\\se2.ogg"), null) { Index = 101, },
                new (new FileInfo("testSounds\\se3.wav"), null) { Index = 102, },
            };
        }
    }
}