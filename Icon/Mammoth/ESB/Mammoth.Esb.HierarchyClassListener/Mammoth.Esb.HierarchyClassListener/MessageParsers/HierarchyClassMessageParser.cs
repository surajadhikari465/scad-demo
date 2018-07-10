using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Models;
using System.Collections.Generic;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.Esb.HierarchyClassListener.MessageParsers
{
    public class HierarchyClassMessageParser : MessageParserBase<Contracts.HierarchyType, List<HierarchyClassModel>>
    {
        public override List<HierarchyClassModel> ParseMessage(IEsbMessage message)
        {
            var contract = DeserializeMessage(message);

            var models = new List<HierarchyClassModel>();

            foreach (var hierarchyClass in contract.@class)
            {
                models.Add(new HierarchyClassModel
                {
                    HierarchyClassId = int.Parse(hierarchyClass.id),
                    HierarchyClassParentId = hierarchyClass.parentId.Value,
                    HierarchyClassName = hierarchyClass.name,
                    HierarchyId = Hierarchies.ByName[contract.name],
                    HierarchyLevelName = contract.prototype.hierarchyLevelName,
                    Action = hierarchyClass.Action
                });
            }

            return models;
        }
    }
}
