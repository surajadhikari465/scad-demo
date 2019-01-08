using System.Collections.Generic;
using System.Configuration;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.ConnectionBuilders;
using Mammoth.Logging;
using SimpleInjector;
using MammothGpmService.Service;
using Mammoth.Common.DataAccess.Decorators;
using MammothGpmService.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using MammothGpmService.Controller;
using MammothGpmService.AmazonUploader;
using MammothGpmService.DataAccess;

namespace MammothGpmPrice.Service
{
	public class SimpleInjectorInitializer
	{
		public static Container InitializeContainer()
		{
			var container = new Container();

			container.RegisterSingleton<ILogger>(() => new NLoggerInstance(typeof(NLoggerInstance),
				ConfigurationManager.AppSettings["InstanceID"].ToString()));
			container.Register<IGetPriceDataHandler, GetPriceDataHandler>(Lifestyle.Singleton);
			container.Register<IAmazonFileUploader, AmazonFileUploader>(Lifestyle.Singleton);
			container.RegisterSingleton<PriceDataContoller>();
			container.Register<IMammothGpmPriceService, MammothGpmPriceService>(Lifestyle.Singleton);

			return container;
		}
	}
}
