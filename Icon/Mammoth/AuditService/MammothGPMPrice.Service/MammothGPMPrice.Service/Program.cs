using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using MammothGpmService.Service;
using System.Configuration;

namespace MammothGpmPrice.Service
{
	class Program
	{
		static void Main(string[] args)
		{
			string apiDescription = ConfigurationManager.AppSettings["ApiDescription"].ToString();
			string apiDisplayName = ConfigurationManager.AppSettings["ApiDisplayName"].ToString();
			string apiServiceName = ConfigurationManager.AppSettings["ApiServiceName"].ToString();
			
			HostFactory.Run(configure =>
			{
				var container = SimpleInjectorInitializer.InitializeContainer();
				container.Verify();

				configure.Service<IMammothGpmPriceService>(service =>
				{
					service.ConstructUsing(s => container.GetInstance<MammothGpmPriceService>());
					service.WhenStarted(s => s.Start());
					service.WhenStopped(s => s.Stop());
				}); 
				configure.RunAsLocalSystem();
				configure.SetServiceName(apiServiceName);
				configure.SetDisplayName(apiDisplayName);
				configure.SetDescription(apiDescription);
			});
			
		}
	}
}
