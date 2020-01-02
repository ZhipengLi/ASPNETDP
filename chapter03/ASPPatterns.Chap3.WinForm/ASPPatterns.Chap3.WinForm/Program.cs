using ASPPatterns.Chap3.WinForm.Model;
using ASPPatterns.Chap3.WinForm.Repository;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASPPatterns.Chap3.WinForm.View
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            BootStrapper.ConfigureStructureMap();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PrdForm());
        }
    }

    public class BootStrapper
    {
        public static void ConfigureStructureMap()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ModelRegistry>();
            });
        }
        public class ModelRegistry : Registry
        {
            public ModelRegistry()
            {
                For<IProductRepository>().Use<ProductRepository>();
            }
        }
    }
}
