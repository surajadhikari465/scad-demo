using System;
using System.Collections.Generic;
using System.Text;

namespace Icon.Esb.Schemas.Wfm.Contracts
{

    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRoot("traits", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
    public partial class TraitsType
    {
        private TraitType[] traitsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }
    }
}
