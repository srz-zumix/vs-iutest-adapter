using System;
using System.ComponentModel.Composition;
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
        public iutestExeTestDiscoverer()
            : this(new DefaultVisualStudioInstanceProvider())
        {
        }

        public iutestExeTestDiscoverer(IVisualStudioInstanceProvider provider)
        {
            this.VSProvider = provider;
        }

        public IVisualStudioInstanceProvider VSProvider { get; private set; }

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
            var projects = VSProvider.Instance.Solution.Projects;

            return projects.SelectMany(FindCppFiles).ToList();
        }

        private IEnumerable<string> FindCppFiles(VisualStudioAdapter.IProject project)
        {
            return from item in project.SourceFiles
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
