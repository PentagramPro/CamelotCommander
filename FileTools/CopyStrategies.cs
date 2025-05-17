using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTools
{
    public static class CopyStrategies
    {
        public enum ConflictStrategy
        {
            Overwrite,
            Skip,
            Abort
        }

        public enum FileLockStrategy
        {
            Skip,
            Abort
        }
    }
}
