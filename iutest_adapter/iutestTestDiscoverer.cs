using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using iutest_adapter.Utility;

namespace iutest_adapter
{
    //[FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri(iutestTestExecutor.ExecutorUriString)]
    public class iutestTestDiscoverer : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext
            , IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
#if DEBUG && LAUNCH_DEBUGGER
            System.Diagnostics.Debugger.Launch();
#endif

            try
            {
                Logger.Initialize(logger);

                var sourceGroups = sources.GroupBy(Path.GetExtension);
                foreach (IGrouping<string, string> sourceGroup in sourceGroups)
                {
                    IIutestTestDiscoverer discoverer = iutestTestDiscovererFactory.GetTestDiscoverer(sourceGroup.Key);
                    if( discoverer != null )
                    {
                        discoverer.DiscoverTests(sourceGroup, discoveryContext, logger, discoverySink);
                    }
                }
            }
            catch( Exception e )
            {
                Logger.Error("iutest_adapter: Exception caught while DiscoverTests: {0}, {1} ", e.Message, e.HResult );
                Logger.Error(e.StackTrace);
            }
            finally
            {
                Logger.Uninitialize();
            }
        }

    }
}
