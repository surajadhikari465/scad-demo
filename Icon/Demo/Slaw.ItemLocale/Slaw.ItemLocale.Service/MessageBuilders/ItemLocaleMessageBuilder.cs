using Mammoth.Common.DataAccess;
using Slaw.ItemLocale.Serializers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Slaw.ItemLocale.Service.MessageBuilders
{
    public class ItemLocaleMessageBuilder
    {
        private ISerializer<Contracts.items> serializer;

        public ItemLocaleMessageBuilder(ISerializer<Contracts.items> serializer)
        {
            this.serializer = serializer;
        }

        public string BuildMessage(IEnumerable<ItemLocaleModel> models)
        {
            return serializer.Serialize(
                new Contracts.items
                {
                    item = models.Select(m => BuildItemType(m)).ToArray()
                }, 
                new Utf8StringWriter());
        }

        private Contracts.ItemType BuildItemType(ItemLocaleModel model)
        {
            return new Contracts.ItemType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = model.IconItemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = model.ItemTypeCode,
                        description = model.ItemTypeDesc
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = model.BusinessUnitId.ToString(),
                        name = model.LocaleName,
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
                                    code = model.ScanCode,
                                }
                            },
                            traits = new Contracts.TraitType[]
                            {
                                CreateTrait(model.CaseDiscount, Attributes.Codes.CaseDiscountEligible),
                                CreateTrait(model.TmDiscount, Attributes.Codes.TmDiscountEligible),
                                CreateTrait(model.AgeRestriction, Attributes.Codes.AgeRestrict),
                                CreateTrait(model.RestrictedHours, Attributes.Codes.RestrictedHours),
                                CreateTrait(model.Authorized, Attributes.Codes.AuthorizedForSale),
                                CreateTrait(model.Discontinued, Attributes.Codes.Discontinued),
                                CreateTrait(model.LabelTypeDescription, Attributes.Codes.LabelTypeDesc),
                                CreateTrait(model.LocalItem, Attributes.Codes.LocalItem),
                                CreateTrait(model.ProductCode, Attributes.Codes.ProductCode),
                                CreateTrait(model.RetailUnit, Attributes.Codes.RetailUnit),
                                CreateTrait(model.SignDescription, Attributes.Codes.SignCaption),
                                CreateTrait(model.Locality, Attributes.Codes.Locality),
                                CreateTrait(model.SignRomanceLong, Attributes.Codes.SignRomanceLong),
                                CreateTrait(model.SignRomanceShort, Attributes.Codes.SignRomanceShort),
                                CreateTrait(model.ColorAdded, Attributes.Codes.ColorAdded),
                                CreateTrait(model.CountryOfProcessing, Attributes.Codes.CountryOfProcessing),
                                CreateTrait(model.Origin, Attributes.Codes.Origin),
                                CreateTrait(model.ElectronicShelfTag, Attributes.Codes.ElectronicShelfTag),
                                CreateTrait(model.Exclusive, Attributes.Codes.Exclusive),
                                CreateTrait(model.NumberOfDigitsSentToScale, Attributes.Codes.NumberOfDigitsSentToScale),
                                CreateTrait(model.ChicagoBaby, Attributes.Codes.ChicagoBaby),
                                CreateTrait(model.TagUom, Attributes.Codes.TagUom),
                                CreateTrait(model.ScaleExtraText, Attributes.Codes.ScaleExtraText),
                                CreateTrait(model.LinkedItem, Attributes.Codes.LinkedScanCode),
                                CreateTrait(model.Msrp, Attributes.Codes.Msrp)
                            }
                        }
                    }
                }
            };
        }

        private Contracts.TraitType CreateTrait(decimal? traitValue, string traitCode)
        {
            var stringTraitValue = traitValue?.ToString("0.00");
            return CreateTrait(stringTraitValue, traitCode);
        }

        private Contracts.TraitType CreateTrait(DateTime? traitValue, string traitCode)
        {
            string stringTraitValue = traitValue?.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            return CreateTrait(stringTraitValue, traitCode);
        }

        private Contracts.TraitType CreateTrait(bool? traitValue, string traitCode)
        {
            string stringTraitValue = null;
            if (traitValue.HasValue)
            {
                stringTraitValue = traitValue.Value ? "1" : "0";
                return CreateTrait(stringTraitValue, traitCode);
            }
            else
            {
                return CreateTrait(stringTraitValue, traitCode);
            }
        }

        private Contracts.TraitType CreateTrait(int? traitValue, string traitCode)
        {
            var stringTraitValue = traitValue?.ToString();
            return CreateTrait(stringTraitValue, traitCode);
        }

        private Contracts.TraitType CreateTrait(bool traitValue, string traitCode)
        {
            var stringTraitValue = traitValue ? "1" : "0";
            return CreateTrait(stringTraitValue, traitCode);
        }

        private Contracts.TraitType CreateTrait(string traitValue, string traitCode)
        {
            return new Contracts.TraitType
            {
                ActionSpecified = true,
                Action = traitValue == null ? Contracts.ActionEnum.Delete : Contracts.ActionEnum.AddOrUpdate,
                code = traitCode,
                type = new Contracts.TraitTypeType
                {
                    description = Attributes.Descriptions.ByCode[traitCode],
                    value = new Contracts.TraitValueType[]
                    {
                        new Contracts.TraitValueType
                        {
                            value = traitValue == null ? string.Empty : traitValue
                        }
                    }
                }
            };
        }
    }
}
