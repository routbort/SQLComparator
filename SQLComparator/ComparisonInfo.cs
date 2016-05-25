using System;
using System.Collections.Generic;
using System.Text;

namespace SQLComparator
{
    [Serializable]
   public class ComparisonInfo
    {
        public string Query1;
        public string Query2;
        public string ConnectionString1;
        public string ConnectionString2;
        public bool UseAdvancedCompare;
        public int? MaxAllowedMismatches;

    }
}
