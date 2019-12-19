﻿using System;

namespace Icon.Web.Mvc.Models
{
    public class FailedGlobalEventViewModel
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string RegionAbbreviation { get; set; }
        public string EventMessage { get; set; }
        public DateTime ProcessFailedDate { get; set; }
    }
}