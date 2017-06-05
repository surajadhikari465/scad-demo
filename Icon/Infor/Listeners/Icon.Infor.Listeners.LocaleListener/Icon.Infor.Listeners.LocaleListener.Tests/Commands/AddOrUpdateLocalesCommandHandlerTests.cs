using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.LocaleListener.Commands;
using Moq;
using Icon.Logging;
using Icon.Framework;
using Mammoth.Common.DataAccess.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using Icon.Infor.Listeners.LocaleListener.Models;
using Icon.Esb.Schemas.Wfm.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using System.Reflection;
using System.Transactions;

namespace Icon.Infor.Listeners.LocaleListener.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateLocalesCommandHandlerTests
    {
        private AddOrUpdateLocalesCommandHandler addOrUpdateLocalesCommandHandler;
        private SqlDbProvider dbProvider;
        private TransactionScope transaction;
        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted });
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            dbProvider.Connection.Open();
            addOrUpdateLocalesCommandHandler = new AddOrUpdateLocalesCommandHandler(dbProvider);   
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateLocales_InsertLocale_ShouldAddLocaleToDatabase()
        {
            Models.LocaleAddress localeAddress = new Models.LocaleAddress(4457, "Altamonte Springs Test", "", "", "Tampa", "30216",
                                                                          "USA", "FL", "Eastern Standard Time", "26.365658", "-80.114501", 7865);
            List< LocaleTraitModel > LocaleTraits = new List<LocaleTraitModel>()
                                                        {   new LocaleTraitModel(Traits.PhoneNumber,"407-767-2100",null,7865),
                                                            new LocaleTraitModel(Traits.Fax,"407-767-2111", null,7865),
                                                            new LocaleTraitModel(Traits.ContactPerson,"Admin",  null,7865),
                                                            new LocaleTraitModel(Traits.IrmaStoreId,"555",  null,7865),
                                                            new LocaleTraitModel(Traits.PsBusinessUnitId,"7865",  null,7865),
                                                            new LocaleTraitModel(Traits.StorePosType,"", null,7865),
                                                            new LocaleTraitModel(Traits.StoreAbbreviation,"TST",  null,7865),
                                                        };

            LocaleModel storeModel = new LocaleModel
                (
                    0,
                    2002,
                    7865,
                    "TestStore",
                    "ST",
                    DateTime.Now,
                    DateTime.Now,
                    null,
                    ActionEnum.AddOrUpdate,
                    localeAddress,
                    LocaleTraits
                );

            LocaleModel metroModel = createLocaleModel(2002, "TestMetro", 2001, "MT", ActionEnum.AddOrUpdate, new List<LocaleModel> { storeModel });
            LocaleModel regionModel = createLocaleModel(2001, "TestRegion", 2000, "RG", ActionEnum.AddOrUpdate, new List<LocaleModel> { metroModel });
            LocaleModel chainModel = createLocaleModel(2000, "Testchain", null, "Ch", ActionEnum.AddOrUpdate, new List<LocaleModel>() { regionModel });
            LocaleModel organizationModel = createLocaleModel(0, "TestCompany", null, "CMP", ActionEnum.AddOrUpdate, new List<LocaleModel>() { chainModel });

            AddOrUpdateLocalesCommand addOrUpdateLocalesCommand = new AddOrUpdateLocalesCommand();
            addOrUpdateLocalesCommand.Locale = organizationModel;
            addOrUpdateLocalesCommandHandler.Execute(addOrUpdateLocalesCommand);

            var storeDb = this.dbProvider.Connection.Query<dynamic>(@"
            DECLARE
		            @LocaleMessageId int = (select MessageTypeId from app.MessageType where MessageTypeName = 'Locale'),
		            @ReadyStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready'),
		            @BusinessUnitTraitId int = (select traitID from Trait where traitCode = 'BU'),
		            @StoreAbbreviationTraitId int = (select traitID from Trait where traitCode = 'SAB'),
		            @PhoneNumberTraitId int = (select traitID from Trait where traitCode = 'PHN'),
                    @FaxTraitId int = (select traitID from Trait where traitCode = 'FAX'),
                    @ContactPersonTraitId int = (select traitID from Trait where traitCode = 'CPN'),
                    @IrmaStoreNumberTraitId int = (select traitID from Trait where traitCode = 'ISI'),
                    @PosTypeTraitId int = (select traitID from Trait where traitCode = 'SPT')

                SELECT 
                    l.LocaleId,
                    lt.traitValue as BusinessUnitId,
                    l.LocaleName as LocaleName,
                    p.LocaleId as ParentLocaleId,
                    ltp.localeTypeCode as TypeCode,
                    abbr.traitValue as Abbreviation,
                    l.localeOpenDate as OpenDate,
                    l.localeCloseDate as CloseDate,
                    la.addressID as AddressId,
                    pa.addressLine1 as AddressLine1,
                    pa.addressLine2 as AddressLine2,
                    pa.addressLine3 as AddressLine3,
                    ci.cityName as CityName,
                    pc.postalCode as PostalCode,
                    c.countryCode as CountryCode,
                    t.territoryCode as TerritoryCode,
                    tz.posTimeZoneName as TimeZoneName,
                    pa.latitude as Latitude,
                    pa.longitude as Longitude,
                    ph.traitValue as PhoneNumber,
                    fax.traitValue as Fax,
                    ctp.traitValue as ContactPerson,
                    agl.AgencyId as EwicAgency,
                    isn.TraitValue as IrmaStoreNumber,
                    pst.TraitValue as PosType
                FROM 
                    Locale l
                    join LocaleType ltp on l.localeTypeID = ltp.localeTypeID
		            left join Locale p on l.parentLocaleID = p.localeID
		            left join Locale r on p.parentLocaleID = r.localeID
                    left join ewic.AgencyLocale agl on agl.LocaleId = l.LocaleId
		            left join LocaleTrait lt on l.localeID = lt.localeID and lt.traitID = @BusinessUnitTraitId
		            left join LocaleTrait abbr on l.localeID = abbr.localeID and abbr.traitID = @StoreAbbreviationTraitId
		            left join LocaleTrait ph on l.localeID = ph.localeID and ph.traitID = @PhoneNumberTraitID
                    left join LocaleTrait fax on l.localeID = fax.localeID and fax.traitID = @FaxTraitId
                    left join LocaleTrait ctp on l.localeID = ctp.localeID and ctp.traitID = @ContactPersonTraitId
                    left join LocaleTrait isn on l.localeID = isn.localeID and isn.traitID = @IrmaStoreNumberTraitId
                    left join LocaleTrait pst on l.localeID = pst.localeID and pst.traitID = @PosTypeTraitId
		            left join LocaleAddress la on l.localeID = la.localeID
		            left join Address a on la.addressID = a.addressID
		            left join AddressUsage au on la.addressUsageID = au.addressUsageID
		            left join PhysicalAddress pa on a.addressID = pa.addressID
		            left join Country c on pa.countryID = c.countryID
		            left join Territory t on pa.territoryID = t.territoryID
		            left join City ci on pa.cityID = ci.cityID
		            left join PostalCode pc on pa.postalCodeID = pc.postalCodeID
		            left join Timezone tz on pa.timezoneID = tz.timezoneID
                WHERE lt.traitValue = @BusinessUnitId",
                new
                {
                    storeModel.BusinessUnitId
                },
                dbProvider.Transaction).First();

            Assert.AreEqual(storeModel.BusinessUnitId.ToString(), storeDb.BusinessUnitId);
            Assert.AreEqual(storeModel.Name, storeDb.LocaleName);
            Assert.AreEqual(storeModel.ParentLocaleId, storeDb.ParentLocaleId);
            Assert.AreEqual(storeModel.TypeCode, storeDb.TypeCode);
            Assert.AreEqual(storeModel.TypeCode, storeDb.TypeCode);
            Assert.AreEqual(storeModel.LocaleTraits.Where(lt => lt.TraitId == Traits.StoreAbbreviation).FirstOrDefault().TraitValue, storeDb.Abbreviation);
            Assert.AreEqual(storeModel.OpenDate.ToShortDateString(), storeDb.OpenDate.ToShortDateString());
            Assert.AreEqual(storeModel.CloseDate.ToShortDateString(), storeDb.CloseDate.ToShortDateString());
            Assert.AreEqual(storeModel.Address.AddressLine1, storeDb.AddressLine1);
            Assert.AreEqual(storeModel.Address.AddressLine2, storeDb.AddressLine2);
            Assert.AreEqual(storeModel.Address.AddressLine3, storeDb.AddressLine3);
            Assert.AreEqual(storeModel.Address.CityName, storeDb.CityName);
            Assert.AreEqual(storeModel.Address.PostalCode, storeDb.PostalCode);
            Assert.AreEqual(storeModel.Address.Country, storeDb.CountryCode);
            Assert.AreEqual(storeModel.Address.TerritoryCode, storeDb.TerritoryCode);
            Assert.AreEqual(storeModel.Address.Latitude, storeDb.Latitude.ToString());
            Assert.AreEqual(storeModel.Address.Longitude, storeDb.Longitude.ToString());
            Assert.AreEqual(storeModel.Address.TimeZoneName, storeDb.TimeZoneName);
            Assert.AreEqual(storeModel.LocaleTraits.Where(lt => lt.TraitId == Traits.PhoneNumber).FirstOrDefault().TraitValue, storeDb.PhoneNumber);
            Assert.AreEqual(storeModel.LocaleTraits.Where(lt => lt.TraitId == Traits.Fax).FirstOrDefault().TraitValue, storeDb.Fax);
            Assert.AreEqual(storeModel.LocaleTraits.Where(lt => lt.TraitId == Traits.ContactPerson).FirstOrDefault().TraitValue, storeDb.ContactPerson);
            Assert.AreEqual(storeModel.EwicAgency, storeDb.EwicAgency);
            Assert.AreEqual(storeModel.LocaleTraits.Where(lt => lt.TraitId == Traits.IrmaStoreId).FirstOrDefault().TraitValue, storeDb.IrmaStoreNumber);
            Assert.AreEqual(storeModel.LocaleTraits.Where(lt=>lt.TraitId == Traits.StorePosType).FirstOrDefault().TraitValue, storeDb.PosType);

        }

        private LocaleModel createStoreLocaleModel(int localeId, int? parentLocaleId, int businessUnitId, string name, string typeCode, DateTime openDate,
                                               DateTime closeDate,  string ewicAgency, string posType, ActionEnum action,
                                               Models.LocaleAddress address,IEnumerable<LocaleTraitModel> localeTraitModelCollection)
        {
            LocaleModel localeModel = new LocaleModel(localeId, parentLocaleId, businessUnitId, name, typeCode, openDate, closeDate,
                                                      ewicAgency, action,address, localeTraitModelCollection);
            return localeModel;
        }

        private LocaleModel createLocaleModel(int localeId, string name, int? parentLocaleId, string typeCode, ActionEnum action, List<LocaleModel> childLocale)
        {
            LocaleModel localeModel = new LocaleModel(localeId, name, parentLocaleId, typeCode, action);
            localeModel.Locales = childLocale;
            return localeModel;
        }
    }
}
