using Irma.Framework;
using Irma.Testing.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Testing.Common
{
    public class TestIrmaDataHelper
    {
        /// <summary
        /// Instantiates an ItemBrand object and saves it to the database
        /// </summary>
        /// <param name="irmaContext">IrmaContext reference</param>
        /// <param name="brandName">name of the brand to be saved (default value: "test Brand X")</param>
        /// <param name="save">flag indicating whether to call SaveChanges() on the context
        ///   after adding the object</param>
        /// <returns>saved ItemBrand object</returns>
        public ItemBrand CreateAndSaveItemBrandForTest(IrmaContext irmaContext,
            string brandName = TestingConstants.BrandNameX, bool save = true)
        {
            var testBrand = new ItemBrand
            {
                Brand_Name = brandName
            };
            irmaContext.ItemBrand.Add(testBrand);
            if (save)
            {
                irmaContext.SaveChanges();
            }
            return testBrand;
        }

        /// <summary>
        /// Instantiates a SubTeam object and saves it to the database 
        /// </summary>
        /// <param name="irmaContext"></param>
        /// <param name="subTeamName">Name to use for the subteam being added</param>
        /// <param name="save">flag indicating whether to call SaveChanges() on the context
        ///   after adding the object</param>
        /// <returns></returns>
        public SubTeam CreateAndSaveSubteamForTest(IrmaContext irmaContext,
            string subTeamName = "test SubTeam", bool save = true)
        {
            var testSubTeam = new SubTeam
            {
                SubTeam_Name = subTeamName
            };
            irmaContext.SubTeam.Add(testSubTeam);
            if (save)
            {
                irmaContext.SaveChanges();
            }
            return testSubTeam;
        }

        /// <summary>
        /// Instantiates an Item object (including a SubTeam sub-object) and saves it to the database
        /// </summary>
        /// <param name="irmaContext">IrmaContext reference</param>
        /// <param name="irmaBrandId">brand id (in IRMA) to be associated with the item</param>
        /// <param name="subTeamName">name to use for the stub SubTeam created for the item 
        ///     (default value: "test SubTeam")</param>
        /// <returns>saved Item object<</returns>
        public Item CreateAndSaveItemAndSubteamForTest(IrmaContext irmaContext, int irmaBrandId,
            string subTeamName = "test SubTeam")
        {
            var testSubTeam = CreateAndSaveSubteamForTest(irmaContext, subTeamName, false);

            return CreateAndSaveItemForTest(irmaContext, irmaBrandId, testSubTeam);
        }

        /// <summary
        /// Instantiates an Item object and saves it to the database
        /// </summary>
        /// <param name="irmaContext">IrmaContext reference</param>
        /// <param name="irmaBrandId">brand id (in IRMA) to be associated with the item</param>
        /// <param name="subteam">SubTeam entity associated with the item</param>
        /// <param name="itemDescription">ItemDescription field for the item</param>
        /// <param name="save">flag indicating whether to call SaveChanges() on the context
        ///   after adding the object</param>
        /// <returns></returns>
        public Item CreateAndSaveItemForTest(IrmaContext irmaContext, int irmaBrandId,
          SubTeam subteam, string itemDescription = "test item", bool save = true)
        { 
            Item testItem = new TestIrmaDbItemBuilder()
                .WithBrand_ID(irmaBrandId)
                .WithSubTeam(subteam)
                .WithItem_Description(itemDescription);

            irmaContext.Item.Add(testItem);
            if (save)
            {
                irmaContext.SaveChanges();
            }
            return testItem;
        }

        /// <summary>
        /// Instantiates a ValidatedBrand object and saves it to the database
        /// </summary>
        /// <param name="irmaContext">IrmaContext reference</param>
        /// <param name="irmaBrandId">IRMA brand id</param>
        /// <param name="iconBrandId">iCON brand ID: default value of -1 [defaultIconBrandId] indicates
        /// an ID value that should never be assigned to a real brand in any database</param>
        /// <param name="save">flag indicating whether to call SaveChanges() on the context
        ///   after adding the object</param>
        /// <returns>saved ValidatedBrand object</returns>
        public ValidatedBrand CreateAndSaveValidatedBrandForTest(IrmaContext irmaContext,
            int? irmaBrandId, int iconBrandId = TestingConstants.IconBrandId_Negative, bool save = true)
        {
            var testValidatedBrand = new ValidatedBrand
            {
                IrmaBrandId = irmaBrandId.GetValueOrDefault(0),
                IconBrandId = iconBrandId
            };
            irmaContext.ValidatedBrand.Add(testValidatedBrand);
            if (save)
            {
                irmaContext.SaveChanges();
            }
            return testValidatedBrand;
        }

        public ValidatedBrand CreateAndSaveValidatedBrandForTest(IrmaContext irmaContext, ItemBrand itemBrand, int iconBrandId, bool save = true)
        {
            return CreateAndSaveValidatedBrandForTest(irmaContext, itemBrand.Brand_ID, iconBrandId, save);
        }
    }
}
