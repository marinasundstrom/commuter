using System;
using System.IO;
using System.Reflection;

namespace Commuter.Helpers
{
    public static class Utils
    {
        private static bool? isRunningInSimulator;

        public static void ExtractSaveResource(string filename, string location)
        {
            var a = Assembly.GetExecutingAssembly();
            using (var resFilestream = a.GetManifestResourceStream(filename))
            {
                if (resFilestream != null)
                {
                    var full = Path.Combine(location, filename);

                    using (var stream = File.Create(full))
                    {
                        resFilestream.CopyTo(stream);
                    }

                }
            }
        }

        public static bool IsRunningInSimulator
        {
            get
            {
                if (isRunningInSimulator == null)
                {
                    isRunningInSimulator = false;

                    try
                    {
                        var runtimeType = Assembly
                            .Load("Xamarin.iOS")
                            .GetType("ObjCRuntime.Runtime");

                        if (runtimeType != null)
                        {
                            var archProperty = runtimeType.GetField("Arch");
                            var architecture = archProperty.GetValue(null).ToString();
                            isRunningInSimulator = architecture == "SIMULATOR";
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }

                return isRunningInSimulator.GetValueOrDefault();
            }
        }
    }
}
