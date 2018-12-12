using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class TestResults
    {
        public int Results { get; private set; }
        public string Type { get; private set; }

        public TestResults(int results, string type)
        {
            Results = results;
            Type = type;
        }
    }
}
