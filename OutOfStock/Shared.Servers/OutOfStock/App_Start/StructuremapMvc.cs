using System.Web.Mvc;
using OutOfStock.DependencyResolution;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(OutOfStock.App_Start.StructuremapMvc), "Start")]

namespace OutOfStock.App_Start {
    public static class StructuremapMvc {
        public static void Start() {
            var container = IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}