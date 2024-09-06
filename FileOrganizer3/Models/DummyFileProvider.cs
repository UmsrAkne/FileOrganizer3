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
                new (new FileInfo("test_test_test222.mp3")) { Index = 2, },
                new (new FileInfo("test_test_test333.mp3")) { Index = 3, },
                new (new FileInfo("test_test_test444.mp3")) { Index = 99, },
            };
        }
    }
}