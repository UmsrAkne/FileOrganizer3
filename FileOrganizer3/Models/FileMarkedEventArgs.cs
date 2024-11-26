using System;
using System.Collections.Generic;

namespace FileOrganizer3.Models
{
    public class FileMarkedEventArgs : EventArgs
    {
        public FileMarkedEventArgs(List<FileInfoWrapper> items)
        {
            Items = items;
        }

        public List<FileInfoWrapper> Items { get; }
    }
}