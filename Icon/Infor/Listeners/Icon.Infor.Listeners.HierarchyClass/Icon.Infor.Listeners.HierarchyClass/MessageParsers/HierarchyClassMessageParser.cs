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

namespace Icon.Infor.Listeners.HierarchyClass
{
    public class HierarchyClassMessageParser : MessageParserBase<HierarchyType, IEnumerable<HierarchyClassModel>>
    {
        private ILogger<HierarchyClassMessageParser> logger;

        public HierarchyClassMessageParser(ILogger<HierarchyClassMessageParser> logger)
        {
            this.logger = logger;
        }

        public override IEnumerable<HierarchyClassModel> ParseMessage(IEsbMessage message)
        {
            try
            {

                var hierarchy = base.DeserializeMessage(message);

                var inforMessageId = message.GetProperty("IconMessageID");
                var messageParseTime = DateTime.Now;

                var hierarchyName = hierarchy.name;
                var hierarchyLevelName = hierarchy.prototype.hierarchyLevelName;

                List<HierarchyClassModel> hierarchyClasses = new List<HierarchyClassModel>();

                foreach (var hierarchyClass in hierarchy.@class)
                {
                    try
                    {
                        hierarchyClasses.Add(new HierarchyClassModel
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
                        logger.Error(JsonConvert.SerializeObject(
                            new
                            {
                                ErrorCode = ApplicationErrors.Codes.UnableToParseHierarchyClass,
                                ErrorDetails = ApplicationErrors.Descriptions.UnableToParseHierarchyClass,
                                InforMessageId = message.GetProperty("IconMessageID"),
                                HierarchyClass = hierarchyClass,
                                Exception = ex
                            }));
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
