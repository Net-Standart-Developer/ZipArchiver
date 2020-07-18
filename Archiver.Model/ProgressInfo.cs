using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public class ProgressInfo
    {
        public int Value { get; set; }
        public int MaxValue { get; set; }

        public bool IsArchivation { get; private set; }
        public bool IsHaveCancel { get; set; }

        public string CurrentFile { get; set; }

        public ProgressInfo(int value, bool isArchivation) : this(value, 100, isArchivation) { }
        
        public ProgressInfo(int value, int maxValue, bool isArchivation)
        {
            if (value >= 0)
                Value = value;
            else
                Value = -1;

            if (maxValue >= value)
                MaxValue = maxValue;
            else
                MaxValue = -1;

            IsArchivation = isArchivation;
        }
    }
}
