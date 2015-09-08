using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using iutest_adapter.Utility;
using iutest_adapter.Utility.VisualStudio;

namespace iutest_adapter
{
    public class iutestExeTestDiscoverer : IIutestTestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext
            , IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            var cppFiles = FindCppFiles();
            foreach( var f in cppFiles )
            {
                Logger.Info(f);
            }
        }

        private IEnumerable<string> FindCppFiles()
        {
            var dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(EnvDTE.DTE));

            var solution = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(IVsSolution));
            var loadedProjects = SolutionHelper.EnumerateLoadedProjects((IVsSolution)solution, __VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION).OfType<IVsProject>();

            return loadedProjects.SelectMany(FindCppFiles).ToList();
        }

        private IEnumerable<string> FindCppFiles(IVsProject project)
        {
            return from item in SolutionHelper.GetProjectItems(project)
                   where IsTestFile(item)
                   select item;
        }
        private static bool IsCppFile(string path)
        {
            return ".cpp".Equals(Path.GetExtension(path), StringComparison.OrdinalIgnoreCase)
                || ".cxx".Equals(Path.GetExtension(path), StringComparison.OrdinalIgnoreCase);
        }

        private bool IsTestFile(string path)
        {
            try
            {
                return IsCppFile(path);
            }
            catch (IOException e)
            {
                Logger.Error("IO error when detecting a test file during Test Container Discovery" + e.Message);
            }

            return false;
        }
    }
}
