using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders.Factories
{
    public class ItemWorksheetBuilderFactory : IWorksheetBuilderFactory<ItemExcelModel>
    {
        private Container container;

        public ItemWorksheetBuilderFactory(Container container)
        {
            this.container = container;
        }

        public IEnumerable<IWorksheetBuilder> CreateWorksheetBuilders()
        {
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQuery = container.GetInstance<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getMerchTaxMappingsQueryHandler = container.GetInstance<IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>>();
            IQueryHandler<GetCertificationAgenciesByTraitParameters, List<Framework.HierarchyClass>> getCertificationAgenciesQueryHandler = container.GetInstance<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<Framework.HierarchyClass>>>();

            return new List<IWorksheetBuilder>
            {
                new BrandWorksheetBuilder(getHierarchyLineageQuery),
                new MerchandiseWorksheetBuilder(getHierarchyLineageQuery),
                new TaxWorksheetBuilder(getHierarchyLineageQuery),
                new BrowsingWorksheetBuilder(getHierarchyLineageQuery),
                new MerchTaxMappingWorksheetBuilder(getMerchTaxMappingsQueryHandler),
                new NationalWorksheetBuilder(getHierarchyLineageQuery),
                new GlutenFreeWorksheetBuilder(getCertificationAgenciesQueryHandler),
                new KosherWorksheetBuilder(getCertificationAgenciesQueryHandler),
                new NonGmoWorksheetBuilder(getCertificationAgenciesQueryHandler),
                new OrganicWorksheetBuilder(getCertificationAgenciesQueryHandler),
                new VeganWorksheetBuilder(getCertificationAgenciesQueryHandler)
            };
        }
    }
}