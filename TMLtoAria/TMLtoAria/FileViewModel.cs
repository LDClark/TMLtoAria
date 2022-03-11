using System;

namespace TMLtoAria
{
    public class FileViewModel
    {
        public string FileName { get; set; }
        public string FileNameWithCreationTime { get; set; }
        public string FullPath { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsSelected { get; set; }
    }
}
