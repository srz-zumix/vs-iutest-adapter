using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace iutest_adapter.Utility
{
    public class Logger
    {
        static IMessageLogger _logger = null;

        public static void Initialize(IMessageLogger logger)
        {
            _logger = logger;            
        }
        public static void Uninitialize()
        {
            _logger = null;
        }
        public static void SendMessage(TestMessageLevel level, string msg)
        {
            if( _logger != null )
            {
                _logger.SendMessage(level, msg);
            }
        }
        public static void SendMessage(TestMessageLevel level, string format, params object[] args)
        {
            SendMessage(level, string.Format(CultureInfo.InvariantCulture, format, args));
        }
        public static void Error(string format, params object[] args)
        {
            SendMessage(TestMessageLevel.Error, format, args);
        }
        public static void Warn(string format, params object[] args)
        {
            SendMessage(TestMessageLevel.Warning, format, args);
        }
        public static void Info(string format, params object[] args)
        {
            SendMessage(TestMessageLevel.Informational, format, args);
        }
    }
}
