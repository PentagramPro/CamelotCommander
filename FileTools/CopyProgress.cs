using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTools
{
    public class CopyProgress
    {

        public int FilesCopied { get; set; }
        public int TotalFiles { get; set; }
        public long BytesCopied { get; set; }
        public long TotalBytes { get; set; }
        public string? CurrentFile { get; set; }
    }
    
}
