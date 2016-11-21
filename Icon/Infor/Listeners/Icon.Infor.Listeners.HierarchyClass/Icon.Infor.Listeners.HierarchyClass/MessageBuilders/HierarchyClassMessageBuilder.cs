using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using Icon.Infor.Listeners.HierarchyClass.Requests;
using System;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.HierarchyClass.MessageBuilders
{
    public class HierarchyClassMessageBuilder : IMessageBuilder<HierarchyClassEsbServiceRequest>
    {
        private ISerializer<Contracts.HierarchyType> serializer;

        public HierarchyClassMessageBuilder(ISerializer<Contracts.HierarchyType> serializer)
        {
            this.serializer = serializer;
        }

        public string BuildMessage(HierarchyClassEsbServiceRequest request)
        {
            if (request.HierarchyClasses != null && request.HierarchyClasses.Any())
            {
                Contracts.HierarchyType contract = new Contracts.HierarchyType
                {
                    name = request.HierarchyName,
                    Action = request.Action,
                    ActionSpecified = true,
                    prototype = new Contracts.HierarchyPrototypeType
                    {
                        hierarchyLevelName = request.HierarchyLevelName,
                        itemsAttached = HierarchyConstants.ItemsAttachedTrue
                    },
                    @class = request.HierarchyClasses.Select(hc => new Contracts.HierarchyClassType
                    {
                        Action = hc.Action,
                        ActionSpecified = true,
                        id = hc.HierarchyClassId.ToString(),
                        name = hc.HierarchyClassName,
                        parentId = new Contracts.hierarchyParentClassType
                        {
                            Value = hc.ParentHierarchyClassId
                        },
                        traits = hc.HierarchyClassTraits.Select(hct => new Contracts.TraitType
                        {
                            code = hct.Key,
                            type = new Contracts.TraitTypeType
                            {
                                value = new[]
                                {
                                    new Contracts.TraitValueType { value = hct.Value }
                                }
                            }
                        }).ToArray()
                    }).ToArray()
                };

                return serializer.Serialize(contract, new Utf8StringWriter());
            }
            else
            {
                throw new ArgumentException("Unable to build hierarchy class message. No models were passed to message builder.");
            }
        }
    }
}
