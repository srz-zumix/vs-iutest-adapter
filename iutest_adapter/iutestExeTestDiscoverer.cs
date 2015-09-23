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
using VisualStudioAdapter;
using BoostTestAdapter.SourceFilter;

using VSTestCase = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase;

namespace iutest_adapter
{
    public class iutestExeTestDiscoverer : IIutestTestDiscoverer
    {
        private readonly ISourceFilter[] _sourceFilters;


        public iutestExeTestDiscoverer()
            : this(new DefaultVisualStudioInstanceProvider())
        {
            _sourceFilters = SourceFilterFactory.Get();
        }

        public iutestExeTestDiscoverer(IVisualStudioInstanceProvider provider)
        {
            this.VSProvider = provider;
        }

        public IVisualStudioInstanceProvider VSProvider { get; private set; }

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext
            , IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            IList<ProjectInfo> projects = FindProjectInfo(sources);
        }

        private IList<ProjectInfo> FindProjectInfo(IEnumerable<string> sources)
        {
            IList<ProjectInfo> infos = new List<ProjectInfo>();
            var projects = VSProvider.Instance.Solution.Projects;
            List<string> sourcesCopy = sources.ToList();

            foreach (var project in projects)
            {
                IProjectConfiguration configuration = project.ActiveConfiguration;

                int index = sourcesCopy.FindIndex(source => string.Equals(source, configuration.PrimaryOutput, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    ProjectInfo info = new ProjectInfo(sourcesCopy[index]);
                    sourcesCopy.RemoveAt(index);

                    info.DefinesHandler = configuration.CppCompilerOptions.PreprocessorDefinitions;
                    foreach (string s in project.SourceFiles)
                    {
                        info.CppSourceFiles.Add(s);
                    }

                    infos.Add(info);
                }
            }
            return infos;
        }

        private void GetIutest(IList<ProjectInfo> projects, ITestCaseDiscoverySink discoverySink)
        {
            if (projects == null) return;

            foreach( ProjectInfo project in projects )
            {
                foreach( string sourceFile in project.CppSourceFiles )
                {
                    try
                    {
                        using (var reader = new StreamReader(sourceFile))
                        {
                            try
                            {
                                CppSourceFile cppSourceFile = new CppSourceFile()
                                {
                                    FileName = sourceFile,
                                    SourceCode = reader.ReadToEnd()
                                };


                                ApplySourceFilter(cppSourceFile, new Defines(project.DefinesHandler));


                            }
                            catch (Exception e)
                            {
                                Logger.Error("Exception: \"{0}\", {1}", sourceFile, e.Message);
                                Logger.Error(e.StackTrace);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Exception: \"{0}\", {1}", sourceFile, e.Message);
                        Logger.Error(e.StackTrace);
                    }
                }
            }
        }

        private void ApplySourceFilter(CppSourceFile cppSourceFile, Defines definesHandler)
        {
            foreach( var filter in _sourceFilters )
            {
                filter.Filter(cppSourceFile, definesHandler);
            }
        }
    }
}
