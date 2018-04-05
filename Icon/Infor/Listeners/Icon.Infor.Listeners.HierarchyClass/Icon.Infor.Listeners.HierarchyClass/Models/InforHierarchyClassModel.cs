﻿using Icon.Esb.Schemas.Wfm.Contracts;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Icon.Infor.Listeners.HierarchyClass.Models
{
    public class InforHierarchyClassModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ActionEnum Action { get; set; }
        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public string HierarchyName { get; set; }
        public int ParentHierarchyClassId { get; set; }
        public string HierarchyLevelName { get; set; }
        public Dictionary<string, string> HierarchyClassTraits { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
        public string InforMessageId { get; set; }
        public DateTime MessageParseTime { get; set; }
        public decimal? SequenceId { get; set; }
    }
}
