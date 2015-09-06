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
    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri(iutestTestExecutor.ExecutorUriString)]
    class iutestTestDiscoverer : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext
            , IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            foreach( string s in sources )
            {
                Console.WriteLine(s);
            }
        }

    }
}
