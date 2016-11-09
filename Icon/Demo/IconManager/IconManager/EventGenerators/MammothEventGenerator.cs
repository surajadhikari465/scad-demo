using Icon.Framework;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconManager.EventGenerators
{
    public class MammothEventGenerator : IMammothEventGenerator
    {
        public void GenerateMammothEvents(string businessUnit)
        {
            string region = null;
            using (IconContext iconContext = new IconContext())
            {
                var locale = GetLocale(businessUnit, iconContext);
                region = GetRegion(locale);
            }

            using (IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(region)))
            {
                GenerateMammothEvents(irmaContext, businessUnit);
            }
        }

        private Locale GetLocale(string businessUnit, IconContext context)
        {
            var localeTrait = context.LocaleTrait
                .SingleOrDefault(lt => lt.traitID == Traits.PsBusinessUnitId && lt.traitValue == businessUnit);
            if (localeTrait == null)
            {
                throw new ArgumentException(string.Format("No store exists with a Business Unit of {0}.", businessUnit));
            }

            return localeTrait.Locale;
        }

        private string GetRegion(Locale locale)
        {
            var region = locale.Locale2.LocaleTrait.FirstOrDefault(lt => lt.traitID == Traits.RegionAbbreviation);
            if(region != null)
            {
                return region.traitValue;
            }
            else
            {
                return GetRegion(locale.Locale2);
            }
        }

        private void GenerateMammothEvents(IrmaContext irmaContext, string businessUnit)
        {
            irmaContext.Database.ExecuteSqlCommand(@"
                DECLARE @Store_No int = (select top 1 Store_No from Store s where s.BusinessUnit_ID = @BU),
	                    @itemLocaleEventTypeId INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = 'ItemLocaleAddOrUpdate'),
                        @priceEventTypeId INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = 'Price')

                DECLARE	@Identifiers table (Identifier nvarchar(13), Item_Key int);

                INSERT INTO @Identifiers
                SELECT ii.Identifier, ii.Item_Key
                FROM ItemIdentifier ii
                JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
                JOIN Item i on ii.Item_Key = i.Item_Key
                JOIN StoreItem si on i.Item_Key = si.Item_Key
	                and si.Store_No = @Store_No
                WHERE ii.Deleted_Identifier = 0
	                and ii.Remove_Identifier = 0
	                and i.Deleted_Item = 0
	                and i.Remove_Item = 0

                INSERT INTO mammoth.ItemLocaleChangeQueue(Item_Key, Store_No, Identifier, EventTypeID)
                SELECT i.Item_Key,
	                @Store_No,
	                i.Identifier,
	                @itemLocaleEventTypeId
                FROM @Identifiers i

                INSERT INTO mammoth.PriceChangeQueue(Item_Key, Store_No, Identifier, EventTypeID)
                SELECT i.Item_Key,
	                @Store_No,
	                i.Identifier,
	                @priceEventTypeId
                FROM @Identifiers i",
                new SqlParameter("BU", int.Parse(businessUnit)) { SqlDbType = System.Data.SqlDbType.Int });
        }
    }
}
