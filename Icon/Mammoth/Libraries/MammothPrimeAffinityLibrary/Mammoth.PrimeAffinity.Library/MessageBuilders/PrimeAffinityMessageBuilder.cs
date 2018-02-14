using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess;
using System.Linq;

namespace Mammoth.PrimeAffinity.Library.MessageBuilders
{
    public class PrimeAffinityMessageBuilder : IMessageBuilder<PrimeAffinityMessageBuilderParameters>
    {
        private const string BusinessUnitTraitCode = "BU";
        private string BusinessUnitTraitDescription = "PS Business Unit ID";

        private PrimeAffinityMessageBuilderSettings settings;
        private ISerializer<items> serializer;

        public PrimeAffinityMessageBuilder(
            ISerializer<items> serializer,
            PrimeAffinityMessageBuilderSettings settings)
        {
            this.serializer = serializer;
            this.settings = settings;
        }

        public string BuildMessage(PrimeAffinityMessageBuilderParameters request)
        {
            return serializer.Serialize(
                new items
                {
                    item = request.PrimeAffinityMessageModels.Select(p => new ItemType
                    {
                        id = p.ItemID,
                        @base = new BaseItemType
                        {
                            type = new ItemTypeType
                            {
                                code = p.ItemTypeCode,
                                description = ItemTypes.Descriptions.ByCode[p.ItemTypeCode]
                            }
                        },
                        locale = new LocaleType[]
                        {
                            new LocaleType
                            {
                                id = p.BusinessUnitID.ToString(),
                                name = p.StoreName,
                                type = new LocaleTypeType
                                {
                                    code = LocaleCodeType.STR,
                                    description = LocaleDescType.Store
                                },
                                Item = new StoreItemAttributesType
                                {
                                    scanCode = new ScanCodeType[]
                                    {
                                        new ScanCodeType
                                        {
                                            code = p.ScanCode
                                        }
                                    },
                                    selectionGroups = new SelectionGroupsType
                                    {
                                        group = new GroupTypeType[]
                                        {
                                            new GroupTypeType
                                            {
                                                Action = p.MessageAction,
                                                ActionSpecified = true,
                                                id = settings.PrimeAffinityPsgName,
                                                name = settings.PrimeAffinityPsgName,
                                                type = settings.PrimeAffinityPsgType
                                            }
                                        }
                                    }
                                },
                                traits = new TraitType[]
                                {
                                    new TraitType
                                    {
                                        code = BusinessUnitTraitCode,
                                        type = new TraitTypeType
                                        {
                                            description = BusinessUnitTraitDescription,
                                            value = new TraitValueType[]
                                            {
                                                new TraitValueType
                                                {
                                                    value = p.BusinessUnitID.ToString()
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }).ToArray()
                });
        }
    }
}
