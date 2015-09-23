using System;
using System.Collections.Generic;
using System.Linq;
using VisualStudioAdapter;

namespace iutest_adapter.Utility.VisualStudio
{
    class ProjectInfo
    {
        public VisualStudioAdapter.Defines DefinesHandler { get; set; }
        public string Output { get; set; }

        public ProjectInfo(string output)
        {
            Output = output;
            CppSourceFiles = new List<string>();
        }

        public IList<string> CppSourceFiles { get; set; }

    }
}
