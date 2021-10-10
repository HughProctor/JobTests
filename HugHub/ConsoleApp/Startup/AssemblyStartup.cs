using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Startup
{
    public class AssemblyStartup
    {
        public static void Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyLoad += new AssemblyLoadEventHandler(MyAssemblyLoadEventHandler);
        }

        static void MyAssemblyLoadEventHandler(object sender, AssemblyLoadEventArgs args)
        {
            Console.WriteLine("ASSEMBLY LOADED: " + args.LoadedAssembly.FullName);
            //If this is the assembly that you want to call an initialize method..
        }
    }
}
