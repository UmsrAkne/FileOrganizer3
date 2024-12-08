using System;
using System.Collections.Generic;

namespace FileOrganizer3.Models
{
    /// <summary>
    /// ファイルのマーク・無視状態が変更された際に発生するイベントの追加情報を格納する EventArgs です。
    /// </summary>
    public class FileStatusChangedEventArgs : EventArgs
    {
        public FileStatusChangedEventArgs(List<FileInfoWrapper> items)
        {
            Items = items;
        }

        public List<FileInfoWrapper> Items { get; }
    }
}