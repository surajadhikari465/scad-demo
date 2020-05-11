using System.Collections.Generic;
using System.Data;
using System.Linq;
using BrandUploadProcessor.Common.Models;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Common.Validators;

namespace BrandUploadProcessor.DataAccess.Queries
{
    public class GetBrandAttributesQueryHandler : IQueryHandler<EmptyQueryParameters<IEnumerable<BrandAttributeModel>>, IEnumerable<BrandAttributeModel>>
    {
        private readonly IDbConnection connection;
        public GetBrandAttributesQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<BrandAttributeModel> Search(EmptyQueryParameters<IEnumerable<BrandAttributeModel>> parameters)
        {
            var validationRules = IconPropertyValidationRules.GetIconPropertyValidationRules("BrandName");
            
            var sql = $@"
                select traitId, TraitCode, TraitPattern, TraitDesc, cast(0 as bit) IsRequired, cast(0 as bit) IsReadOnly  from trait where traitcode in ('BA','GRD', 'PCO', 'ZIP', 'LCL') 
                union all
                select null TraitId, 'BN' Traitcode, '{validationRules.ExpressionToValidate}' TraitPattern, 'Brand Name' TraitDesc, cast(0 as bit) IsRequired, cast(0 as bit) IsReadOnly
            ";

            var attributes = connection.Query<BrandAttributeModel>(sql).ToList();

            var brandName = attributes.First(a => a.TraitDesc == "Brand Name");
            var brandAbbrev = attributes.First(a => a.TraitDesc == "Brand Abbreviation");

            brandName.IsRequired = true;
            brandAbbrev.IsRequired = true;

            return attributes;
        }
    }


 

}