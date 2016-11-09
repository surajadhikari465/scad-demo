﻿using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateHierarchyClassTraitCommand
    {
        public HierarchyClass UpdatedHierarchyClass { get; set; }
        public string TaxAbbreviation { get; set; }
        public string GlAccount { get; set; }
        public int SubTeamHierarchyClassId { get; set; }
        public string NonMerchandiseTrait { get; set; }
        public string ProhibitDiscount { get; set; }
        public string PosDeptNumber { get; set; }
        public string TeamNumber { get; set; }
        public string TeamName { get; set; }
        public bool NonAlignedSubteam { get; set; }
        public string UserName { get; set; }
        public string TaxRomance { get; set; }
        public string NationalClassCode { get; set; }
        public bool SubteamChanged { get; set; } //output value
        public string SubBrickCode { get; set; }
    }
}
