using System;
using System.IO;

namespace iutest_adapter
{
    public class iutestTestDiscovererFactory
    {
        public static IIutestTestDiscoverer GetTestDiscoverer(string identifier)
        {
            IIutestTestDiscoverer discoverer = GetInternalTestDiscoverer(identifier);
            return discoverer;
        }

        private static IIutestTestDiscoverer GetInternalTestDiscoverer(string source)
        {
            switch (Path.GetExtension(source))
            {
                case ".exe": return new iutestExeTestDiscoverer();
            }

            return null;
        }
    }
}
