using Icon.Testing.Builders;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Icon.Web.DataAccess.Queries;

namespace Icon.Web.Tests.Integration.Commands
{
	[TestClass]
	public class AddLocaleCommandHandlerTests
	{
		private AddLocaleCommandHandler addLocaleCommandHandler;
		private AddLocaleCommand addLocaleCommand;
		private IconContext context;
		private DbContextTransaction transaction;
		private string localeName;
		private Agency testAgency;
		private string testAgencyId;
		private GetCurrencyForCountryQuery getCurrencyQuery;

		[TestInitialize]
		public void Initialize()
		{
			context = new IconContext();
			getCurrencyQuery = new GetCurrencyForCountryQuery(context);
			addLocaleCommandHandler = new AddLocaleCommandHandler(context, getCurrencyQuery);

			localeName = "Integration Test Store";

			addLocaleCommand = new AddLocaleCommand
			{
				LocaleName = localeName,
				LocaleParentId = 1,
				OpenDate = DateTime.Now,
				OwnerOrgPartyId = 1,
				LocaleTypeId = LocaleTypes.Store,
				CountryId = 1
			};

			transaction = context.Database.BeginTransaction();
		}

		[TestCleanup]
		public void Cleanup()
		{
			transaction.Rollback();
			transaction.Dispose();
			context.Dispose();
		}

		private void StageTestAgency()
		{
			testAgencyId = "ZZ";
			testAgency = new TestAgencyBuilder().WithAgencyId(testAgencyId);

			context.Agency.Add(testAgency);
			context.SaveChanges();
		}

		[TestMethod]
		public void AddLocale_SuccessfulExecution_NewLocaleShouldBeAdded()
		{
			// Given.
			StageTestAgency();

			addLocaleCommand.BusinessUnit = "99999";
			addLocaleCommand.PhoneNumber = "512-555-5555";
			addLocaleCommand.ContactPerson = "Trey D'Amico";
			addLocaleCommand.StoreAbbreviation = "ITS";
			addLocaleCommand.EwicAgencyId = testAgencyId;
			addLocaleCommand.IrmaStoreId = "TestIrmaStoreId";
			addLocaleCommand.StorePosType = "TestStorePostType";
			addLocaleCommand.Fax = "TestFax";
			addLocaleCommand.UserName = "Test User";
			addLocaleCommand.SodiumWarningRequired = true;
			addLocaleCommand.PrimeMerchantIDEncrypted = "ThisIsToTestPrimeMerchantIDEncryptedLength";

			// When.
			addLocaleCommandHandler.Execute(addLocaleCommand);

			// Then.
			var newLocale = context.Locale.Single(l => l.localeName == addLocaleCommand.LocaleName);

			Assert.AreEqual(addLocaleCommand.LocaleName, newLocale.localeName);
			Assert.AreEqual(addLocaleCommand.BusinessUnit, newLocale.LocaleTrait.Single(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue);
			Assert.AreEqual(addLocaleCommand.PhoneNumber, newLocale.LocaleTrait.Single(lt => lt.traitID == Traits.PhoneNumber).traitValue);
			Assert.AreEqual(addLocaleCommand.ContactPerson, newLocale.LocaleTrait.Single(lt => lt.traitID == Traits.ContactPerson).traitValue);
			Assert.AreEqual(addLocaleCommand.StoreAbbreviation, newLocale.LocaleTrait.Single(lt => lt.traitID == Traits.StoreAbbreviation).traitValue);
			Assert.AreEqual(addLocaleCommand.IrmaStoreId, newLocale.LocaleTrait.Single(lt => lt.traitID == Traits.IrmaStoreId).traitValue);
			Assert.AreEqual(addLocaleCommand.StorePosType, newLocale.LocaleTrait.Single(lt => lt.traitID == Traits.StorePosType).traitValue);
			Assert.AreEqual(addLocaleCommand.Fax, newLocale.LocaleTrait.Single(lt => lt.traitID == Traits.Fax).traitValue);
			Assert.AreEqual(addLocaleCommand.UserName, newLocale.LocaleTrait.Single(lt => lt.traitID == Traits.ModifiedUser).traitValue);
			Assert.AreEqual(addLocaleCommand.EwicAgencyId, context.Locale.Single(l => l.localeID == newLocale.localeID).Agency.Single().AgencyId);
			Assert.AreEqual(addLocaleCommand.SodiumWarningRequired, newLocale.LocaleTrait.Single(lt => lt.traitID == Traits.SodiumWarningRequired).traitValue == "1");
			Assert.AreEqual(addLocaleCommand.PrimeMerchantIDEncrypted, newLocale.LocaleTrait.Single(lt => lt.traitID == Traits.PrimenowMerchantIdEncrypted).traitValue);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddLocale_DuplicateLocaleName_ExceptionShouldBeThrown()
		{
			// Given.

			// When.
			addLocaleCommandHandler.Execute(addLocaleCommand);

			// Alter capitalization to ensure that the duplicate matching isn't case-sensitive.
			addLocaleCommand.LocaleName = "IntEgrAtiOn TeST sTORe";

			addLocaleCommandHandler.Execute(addLocaleCommand);

			// Then.
			// Expected exception.
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddLocale_DuplicateBusinessUnit_ExceptionShouldBeThrown()
		{
			// Given.

			// When.
			addLocaleCommand.BusinessUnit = "99999";
			addLocaleCommandHandler.Execute(addLocaleCommand);

			// Change the locale name, but leave the business unit the same.
			addLocaleCommand.LocaleName = "New Integration Test Store";

			addLocaleCommandHandler.Execute(addLocaleCommand);

			// Then.
			// Expected exception.
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddLocale_DuplicateStoreAbbreviationCombo_ExceptionShouldBeThrown()
		{
			// Given.
			addLocaleCommand = new AddLocaleCommand();
			addLocaleCommand.StoreAbbreviation = this.context.LocaleTrait.First(lt => lt.traitID == Traits.StoreAbbreviation).traitValue;
			addLocaleCommand.BusinessUnit = "99999";
			addLocaleCommand.PhoneNumber = "512-555-5555";
			addLocaleCommand.ContactPerson = "Trey D'Amico";

			// When.
			addLocaleCommandHandler.Execute(addLocaleCommand);

			// Then.
			// Expected exception.
		}

		[TestMethod]
		[ExpectedException(typeof(DbEntityValidationException))]
		public void AddLocale_DbValidationError_ShouldThrowException()
		{
			// Given.
			string tooLongStoreName = "FAIL";
			for (int i = 0; i < 7; i++)
			{
				tooLongStoreName += tooLongStoreName;
			}

			addLocaleCommand.LocaleName = tooLongStoreName;
			addLocaleCommand.BusinessUnit = "99999";

			// When.
			addLocaleCommandHandler.Execute(addLocaleCommand);

			// Then.
			// Expected exception.
		}

		[TestMethod]
		public void AddLocale_WhenCloseDateIsNull_NewLocaleShouldBeAdded_WithNullCloseDate()
		{
			// Given.
			StageTestAgency();

			addLocaleCommand.BusinessUnit = "99999";
			addLocaleCommand.PhoneNumber = "512-555-5555";
			addLocaleCommand.ContactPerson = "Trey D'Amico";
			addLocaleCommand.StoreAbbreviation = "ITS";
			addLocaleCommand.EwicAgencyId = testAgencyId;
			addLocaleCommand.IrmaStoreId = "TestIrmaStoreId";
			addLocaleCommand.StorePosType = "TestStorePostType";
			addLocaleCommand.Fax = "TestFax";
			addLocaleCommand.UserName = "Test User";
			addLocaleCommand.CloseDate = null;

			// When.
			addLocaleCommandHandler.Execute(addLocaleCommand);

			// Then.
			var newLocale = context.Locale.Single(l => l.localeName == addLocaleCommand.LocaleName);

			Assert.AreEqual(addLocaleCommand.CloseDate, newLocale.localeCloseDate);
		}		
	}
}