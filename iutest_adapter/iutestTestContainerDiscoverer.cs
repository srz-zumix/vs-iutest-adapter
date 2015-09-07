using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace iutest_adapter
{
    [Export (typeof(ITestContainerDiscoverer))]
    class iutestTestContainerDiscoverer : ITestContainerDiscoverer
    {
        public const string ExecutorUriString = "executor:/iutestTestExecutor/v1";

        public event EventHandler TestContainersUpdated;

        private bool initialContainerSearch;
        private readonly List<ITestContainer> cachedContainers;

        public Uri ExecutorUri { get { return new System.Uri(ExecutorUriString); } }
        public IEnumerable<ITestContainer> TestContainers { get { return GetTestContainers(); } }

        private IEnumerable<ITestContainer> GetTestContainers()
        {
            if (initialContainerSearch)
            {
                cachedContainers.Clear();
                initialContainerSearch = false;
            }

            return cachedContainers;
        }
    }
}
