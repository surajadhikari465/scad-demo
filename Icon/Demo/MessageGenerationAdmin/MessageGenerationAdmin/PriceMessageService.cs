using Icon.Esb;
using Icon.Esb.Producer;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts = Icon.Service.Library.ContractTypes;
using System.Data.Entity;

namespace MessageGenerationAdmin
{
    public interface IPriceMessageService
    {
        void DeleteTpr(string scanCode, int localeId, decimal price, DateTime startDate);
    }

    public class PriceMessageService : IPriceMessageService
    {
        private IconContext context;
        private IEsbProducer producer;
        private ISerializer<Contracts.items> serializer;

        public PriceMessageService(IconContext context, IEsbProducer producer, ISerializer<Contracts.items> serializer)
        {
            this.producer = producer;
            this.context = context;
            this.serializer = serializer;
        }

        public void DeleteTpr(string scanCode, int localeId, decimal price, DateTime startDate)
        {
            Item item = GetItem(scanCode);
            Locale locale = GetLocale(localeId);
            var businessUnit = locale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId);
            var scanCodeEntity = item.ScanCode.First();

            var deleteTpr = CreateDeleteTpr(scanCode, price, startDate, item, locale, businessUnit, scanCodeEntity);
            var serializedTpr = serializer.Serialize(deleteTpr, new Utf8StringWriter());
            producer.Send(serializedTpr);
        }

        private Contracts.items CreateDeleteTpr(string scanCode, decimal price, DateTime startDate, Item item, Locale locale, LocaleTrait businessUnit, ScanCode scanCodeEntity)
        {
            return new Contracts.items
            {
                item = new Contracts.ItemType[]
                {
                    new Contracts.ItemType
                    {
                        id = item.itemID,
                        @base = new Contracts.BaseItemType
                        {
                            type = new Contracts.ItemTypeType
                            {
                                code = item.ItemType.itemTypeCode,
                                description = item.ItemType.itemTypeDesc
                            }
                        },
                        locale = new Contracts.LocaleType[]
                            {
                                new Contracts.LocaleType
                                {
                                    Action = Contracts.ActionEnum.Delete,
                                    ActionSpecified = true,
                                    id = businessUnit.traitValue,
                                    name = locale.localeName,
                                    type = new Contracts.LocaleTypeType
                                    {
                                        code = Contracts.LocaleCodeType.STR,
                                        description = Contracts.LocaleDescType.Store
                                    },
                                    Item = new Contracts.StoreItemAttributesType
                                    {
                                        scanCode = new Contracts.ScanCodeType[]
                                        {
                                            new Contracts.ScanCodeType
                                            {
                                                id = scanCodeEntity.scanCodeID,
                                                code = scanCode,
                                                typeId = scanCodeEntity.ScanCodeType.scanCodeTypeID,
                                                typeIdSpecified = true,
                                                typeDescription = scanCodeEntity.ScanCodeType.scanCodeTypeDesc
                                            }                                    
                                        },
                                        prices = new Contracts.PriceType[]
                                        {
                                            new Contracts.PriceType
                                            {
                                                type = new Contracts.PriceTypeType
                                                {
                                                    description = ItemPriceDescriptions.TemporaryPriceReduction,
                                                    id = ItemPriceTypes.Tpr.ToString()
                                                },
                                                uom = new Contracts.UomType
                                                {
                                                    codeSpecified = true,
                                                    nameSpecified = true,
                                                    code = Contracts.WfmUomCodeEnumType.EA,//GetUomCode(message.UomCode),
                                                    name = Contracts.WfmUomDescEnumType.EACH//GetUomName(message.UomName, message.ScanCode)
                                                },
                                                currencyTypeCode = Contracts.CurrencyTypeCodeEnum.USD,// GetCurrencyTypeCode(message.CurrencyCode),
                                                priceAmount = new Contracts.PriceAmount
                                                {
                                                    amount = price,
                                                    amountSpecified = true
                                                },
                                                priceMultiple = 1,
                                                priceStartDate = startDate,
                                                priceStartDateSpecified = true,
                                                priceEndDate = new DateTime(2050, 1, 1),
                                                priceEndDateSpecified = true
                                            }
                                        }
                                    }
                                }
                            }
                    }
                }
            };

        }

        private Locale GetLocale(int localeId)
        {
            return context.Locale
                .Include(l => l.LocaleTrait)
                .First(l => l.localeID == localeId);
        }

        private Item GetItem(string scanCode)
        {
            return context.Item
                .Include(i => i.ItemType)
                .Include(i => i.ScanCode.Select(sc => sc.ScanCodeType))
                .First(i => i.ScanCode.FirstOrDefault().scanCode == scanCode);
        }
    }
}
