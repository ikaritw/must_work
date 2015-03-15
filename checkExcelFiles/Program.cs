using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace checkExcelFiles {
    static class Program {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main() {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new mainForm());
        }

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
            //throw new NotImplementedException();
            string prefix = "checkExcelFiles.lib";//看看dll被放在哪裡
            AssemblyName assembly = new AssemblyName(args.Name);
            string resourceName = string.Format("{0}.{1}.dll", prefix, assembly.Name);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)) {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
    }
}
