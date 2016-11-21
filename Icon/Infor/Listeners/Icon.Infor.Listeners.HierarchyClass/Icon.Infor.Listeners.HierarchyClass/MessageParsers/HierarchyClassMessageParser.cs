using Icon.Esb.MessageParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using Icon.Logging;
using Newtonsoft.Json;
using Icon.Infor.Listeners.HierarchyClass.Extensions;

namespace Icon.Infor.Listeners.HierarchyClass
{
    public class HierarchyClassMessageParser : MessageParserBase<HierarchyType, IEnumerable<InforHierarchyClassModel>>
    {
        private ILogger<HierarchyClassMessageParser> logger;

        public HierarchyClassMessageParser(ILogger<HierarchyClassMessageParser> logger)
        {
            this.logger = logger;
        }

        public override IEnumerable<InforHierarchyClassModel> ParseMessage(IEsbMessage message)
        {
            try
            {
                var hierarchy = base.DeserializeMessage(message);

                var inforMessageId = message.GetProperty("IconMessageID");
                var messageParseTime = DateTime.Now;

                var hierarchyName = hierarchy.name;
                var hierarchyLevelName = hierarchy.prototype.hierarchyLevelName;

                List<InforHierarchyClassModel> hierarchyClasses = new List<InforHierarchyClassModel>();

                foreach (var hierarchyClass in hierarchy.@class)
                {
                    try
                    {
                        hierarchyClasses.Add(new InforHierarchyClassModel
                        {
                            Action = hierarchyClass.Action,
                            HierarchyClassId = int.Parse(hierarchyClass.id),
                            HierarchyClassName = hierarchyClass.name,
                            HierarchyName = hierarchyName,
                            HierarchyLevelName = hierarchyLevelName,
                            ParentHierarchyClassId = hierarchyClass.parentId.Value,
                            HierarchyClassTraits = hierarchyClass.traits == null ?
                                new Dictionary<string, string>() :
                                hierarchyClass.traits.ToDictionary(
                                    hct => hct.code,
                                    hct => hct.type.value.First().value),
                            InforMessageId = inforMessageId,
                            MessageParseTime = messageParseTime
                        });
                    }
                    catch (Exception ex)
                    {
                        logger.Error(
                            new
                            {
                                ErrorCode = ApplicationErrors.Codes.UnableToParseHierarchyClass,
                                ErrorDetails = ApplicationErrors.Descriptions.UnableToParseHierarchyClass,
                                InforMessageId = message.GetProperty("IconMessageID"),
                                HierarchyClass = hierarchyClass,
                                Exception = ex
                            }.ToJson());
                    }
                }

                return hierarchyClasses;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Failed to parse Infor HierarchyClass message.", ex);
            }
        }
    }
}
