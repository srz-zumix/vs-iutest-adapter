using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace iutest_adapter
{
    public class iutestTestExecutor : ITestExecutor
    {
        public void Cancel()
        {

        }
        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {

        }
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {

        }

        public const string ExecutorUriString = "executor://iutestTestExecutor/v1";
        public static readonly Uri ExecutorUri = new Uri(iutestTestExecutor.ExecutorUriString);
    }
}
