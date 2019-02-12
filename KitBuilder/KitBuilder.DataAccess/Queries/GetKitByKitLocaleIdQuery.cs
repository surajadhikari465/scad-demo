using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitBuilder.DataAccess.Queries
{
	public class GetKitByKitLocaleIdQuery : IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale>
	{
		private IRepository<KitLocale> kitLocaleRepository;

		public GetKitByKitLocaleIdQuery(IRepository<KitLocale> kitLocaleRepository)
		{
			this.kitLocaleRepository = kitLocaleRepository;
		}

		public KitLocale Search(GetKitByKitLocaleIdParameters parameters)
		{
			var kitLocale = (kitLocaleRepository.UnitOfWork.Context.KitLocale.Where(kl => kl.KitLocaleId == parameters.KitLocaleId)
					 .Include(k => k.Kit).ThenInclude(i => i.Item)
					 .Include(kll => kll.KitLinkGroupLocale).ThenInclude(k => k.KitLinkGroupItemLocale)
					 .ThenInclude(i => i.KitLinkGroupItem).ThenInclude(i => i.LinkGroupItem)
					 .ThenInclude(i => i.Item)).FirstOrDefault();

			return kitLocale;
		}
	}
}
