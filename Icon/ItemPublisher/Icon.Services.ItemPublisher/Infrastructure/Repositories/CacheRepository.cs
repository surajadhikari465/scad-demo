using Dapper;
using Icon.Services.ItemPublisher.Infrastructure.Repositories.Entities;
using Icon.Services.ItemPublisher.Repositories.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Repositories
{
    public class CacheRepository : AbstractRepository, ICacheRepository
    {
        public CacheRepository(IProviderFactory dbProviderFactory)
           : base(dbProviderFactory)
        {
        }

        /// <summary>
        /// Returns all attributes in the dbo.Attributes table
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, Attributes>> GetAttributes()
        {
            IEnumerable<Attributes> items = await this.DbProviderFactory.Provider.Connection.QueryAsync<Attributes>(
                $@"SELECT
                    a.AttributeId,
                    a.AttributeName,
                    a.DisplayName as AttributeDisplayName,
                    a.Description,
                    a.IsPickList,
                    a.XmlTraitDescription,
                    a.TraitCode,
                    t.TraitId,
                    a.IsSpecialTransform,
                    dt.DataType AS DataTypeName
                FROM dbo.Attributes a
                JOIN dbo.DataType dt ON a.DataTypeId = dt.DataTypeId
                join dbo.Trait t on t.traitCode = a.traitCode",
                null,
                this.DbProviderFactory.Provider.Transaction);

            Dictionary<string, Attributes> response = new Dictionary<string, Attributes>();

            foreach (Attributes attribute in items)
            {
                response[attribute.AttributeName] = attribute;
            }

            return response;
        }

        /// <summary>
        /// Returns all hierarchies in the dbo.Hierarchy table
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, HierarchyCacheItem>> GetHierarchies()
        {
            IEnumerable<HierarchyCacheItem> items = await this.DbProviderFactory.Provider.Connection.QueryAsync<HierarchyCacheItem>($@"SELECT
            HierarchyId,
            HierarchyName
            FROM dbo.Hierarchy",
            null,
            this.DbProviderFactory.Provider.Transaction);

            Dictionary<string, HierarchyCacheItem> response = new Dictionary<string, HierarchyCacheItem>();

            foreach (HierarchyCacheItem hierarchy in items)
            {
                response[hierarchy.HierarchyName] = hierarchy;
            }

            return response;
        }

        /// <summary>
        /// Returns all Product Selection Groups in app.ProductSelectionGroup that are for Products
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<int, ProductSelectionGroup>> GetProductSelectionGroups()
        {
            var productSelectionGroups = await this.DbProviderFactory.Provider.Connection
                .QueryAsync<ProductSelectionGroup>($@"SELECT
	                 psg.AttributeId
                    ,psg.AttributeValue
                    ,a.AttributeName
                    ,psg.ProductSelectionGroupId
	                ,psg.ProductSelectionGroupName
	                ,psg.ProductSelectionGroupTypeId
	                ,psg.TraitId
	                ,psg.TraitValue
	                ,psg.MerchandiseHierarchyClassId
	                ,pgst.ProductSelectionGroupTypeName
	                FROM app.ProductSelectionGroup psg
                    LEFT JOIN dbo.Attributes a on a.AttributeId = psg.AttributeId
                    LEFT JOIN app.ProductSelectionGroupType pgst on pgst.ProductSelectionGroupTypeId = psg.ProductSelectionGroupTypeId",
                    null,
                    this.DbProviderFactory.Provider.Transaction);

            Dictionary<int, ProductSelectionGroup> psgs = productSelectionGroups.ToDictionary(psg => psg.ProductSelectionGroupId, value => value);

            return psgs;
        }

        /// <summary>
        /// Returns a single attribute if it exists with the provided traitCode
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Attributes> GetSingleAttribute(string attributeName)
        {
            return await this.DbProviderFactory.Provider.Connection.QueryFirstOrDefaultAsync<Attributes>(
                $@"SELECT
                    a.AttributeId,
                    a.AttributeName,
                    a.DisplayName as AttributeDisplayName,
                    a.Description,
                    a.TraitCode,
                    a.IsPickList,
                    a.XmlTraitDescription,
                    a.IsSpecialTransform,
                    t.TraitId,
                    dt.DataType AS DataTypeName
                FROM dbo.Attributes a
                JOIN dbo.DataType dt ON a.DataTypeId = dt.DataTypeId
                LEFT JOIN dbo.Trait t on t.traitCode = a.traitCode
                WHERE a.attributeName=@attributeName",
                new { attributeName = attributeName },
                this.DbProviderFactory.Provider.Transaction);
        }

        /// <summary>
        /// Returns a single hierarchy if it exists with the supplied name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<HierarchyCacheItem> GetSingleHierarchy(string name)
        {
            return await this.DbProviderFactory.Provider.Connection.QueryFirstOrDefaultAsync<HierarchyCacheItem>($@"SELECT
                HierarchyId,
                HierarchyName
                FROM dbo.Hierarchy
                WHERE HierarchyName=@HierarchyName",
                new { HierarchyName = name },
                this.DbProviderFactory.Provider.Transaction);
        }

        public async Task<Dictionary<string, string>> GetUoms()
        {
            string sql = "SELECT uomID, uomCode, uomName FROM dbo.UOM";
            var uoms = await this.DbProviderFactory.Provider.Connection
                .QueryAsync<Uom>(sql, transaction: this.DbProviderFactory.Provider.Transaction);
            Dictionary<string, string> uomLookup = uoms.ToDictionary(u => u.UomCode, k => k.UomName);
            return uomLookup;
        }
    }
}