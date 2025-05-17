using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTools
{
    public class CopyPlan
    {
        public List<(FileInfo Source, FileInfo Destination)> Files { get; } = new();

        public List<(DirectoryInfo Source, DirectoryInfo Destination)> Directories { get; } = new();

        public List<FileSystemInfo> AlreadyExists { get; } = new();

        public long TotalSizeBytes { get; set; } = 0;

        public int TotalFileCount => Files.Count;

        public List<string> Warnings { get; } = new();
    }
}
