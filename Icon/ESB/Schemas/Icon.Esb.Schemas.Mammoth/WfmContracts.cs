﻿using System.Numerics;
using System.Xml.Serialization;

namespace Icon.Esb.Schemas.Mammoth
{
    // 
    // This source code was auto-generated by xsd, Version=4.8.3928.0.
    //

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Mammoth/ErrorMessage.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Mammoth/ErrorMessage.xsd", IsNullable = false)]
    public partial class ErrorMessage
    {

        private string applicationField;

        private string messageIDField;

        private NameValuePair[] messagePropertiesField;

        private string messageField;

        private string errorCodeField;

        private string errorDetailsField;

        private string errorSeverityField;

        private NameValuePair[] extendedErrorPropertiesField;

        /// <remarks/>
        public string Application
        {
            get
            {
                return this.applicationField;
            }
            set
            {
                this.applicationField = value;
            }
        }

        /// <remarks/>
        public string MessageID
        {
            get
            {
                return this.messageIDField;
            }
            set
            {
                this.messageIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("NameValuePair", IsNullable = false)]
        public NameValuePair[] MessageProperties
        {
            get
            {
                return this.messagePropertiesField;
            }
            set
            {
                this.messagePropertiesField = value;
            }
        }

        /// <remarks/>
        public string Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
            }
        }

        /// <remarks/>
        public string ErrorDetails
        {
            get
            {
                return this.errorDetailsField;
            }
            set
            {
                this.errorDetailsField = value;
            }
        }

        /// <remarks/>
        public string ErrorSeverity
        {
            get
            {
                return this.errorSeverityField;
            }
            set
            {
                this.errorSeverityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("NameValuePair", IsNullable = false)]
        public NameValuePair[] ExtendedErrorProperties
        {
            get
            {
                return this.extendedErrorPropertiesField;
            }
            set
            {
                this.extendedErrorPropertiesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Mammoth/ErrorMessage.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Mammoth/ErrorMessage.xsd", IsNullable = false)]
    public partial class NameValuePair
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Mammoth/ErrorMessage.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Mammoth/ErrorMessage.xsd", IsNullable = false)]
    public partial class ExtendedErrorProperties
    {

        private NameValuePair[] nameValuePairField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NameValuePair")]
        public NameValuePair[] NameValuePair
        {
            get
            {
                return this.nameValuePairField;
            }
            set
            {
                this.nameValuePairField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Mammoth/ErrorMessage.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Mammoth/ErrorMessage.xsd", IsNullable = false)]
    public partial class MessageProperties
    {

        private NameValuePair[] nameValuePairField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NameValuePair")]
        public NameValuePair[] NameValuePair
        {
            get
            {
                return this.nameValuePairField;
            }
            set
            {
                this.nameValuePairField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Mammoth/MammothPrices.xsd")]
    [System.Xml.Serialization.XmlRootAttribute("MammothPrice", Namespace = "http://schemas.wfm.com/Mammoth/MammothPrices.xsd", IsNullable = false)]
    public partial class MammothPriceType
    {

        private string regionField;

        private BigInteger priceIDField;

        private bool priceIDFieldSpecified;

        private int businessUnitField;

        private int itemIdField;

        private string gpmIdField;

        private int multipleField;

        private decimal priceField;

        private decimal? percentOffField;

        private bool percentOffFieldSpecified;

        private System.DateTime startDateField;

        private System.DateTime? endDateField;

        private bool endDateFieldSpecified;

        private string priceTypeField;

        private string priceTypeAttributeField;

        private string sellableUomField;

        private string currencyCodeField;

        private System.DateTime? tagExpirationDateField;

        private bool tagExpirationDateFieldSpecified;

        private string actionField;

        private string itemTypeCodeField;

        private string storeNameField;

        private string scanCodeField;

        private int? subTeamNumberField;

        private bool subTeamNumberFieldSpecified;

        /// <remarks/>
        public string Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        public BigInteger PriceID
        {
            get
            {
                return this.priceIDField;
            }
            set
            {
                this.priceIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PriceIDSpecified
        {
            get
            {
                return this.priceIDFieldSpecified;
            }
            set
            {
                this.priceIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int BusinessUnit
        {
            get
            {
                return this.businessUnitField;
            }
            set
            {
                this.businessUnitField = value;
            }
        }

        /// <remarks/>
        public int ItemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }

        /// <remarks/>
        public string GpmId
        {
            get
            {
                return this.gpmIdField;
            }
            set
            {
                this.gpmIdField = value;
            }
        }

        /// <remarks/>
        public int Multiple
        {
            get
            {
                return this.multipleField;
            }
            set
            {
                this.multipleField = value;
            }
        }

        /// <remarks/>
        public decimal Price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        public decimal? PercentOff
        {
            get
            {
                return this.percentOffField;
            }
            set
            {
                this.percentOffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PercentOffSpecified
        {
            get
            {
                return this.percentOffFieldSpecified;
            }
            set
            {
                this.percentOffFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime StartDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        public System.DateTime? EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EndDateSpecified
        {
            get
            {
                return this.endDateFieldSpecified;
            }
            set
            {
                this.endDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string PriceType
        {
            get
            {
                return this.priceTypeField;
            }
            set
            {
                this.priceTypeField = value;
            }
        }

        /// <remarks/>
        public string PriceTypeAttribute
        {
            get
            {
                return this.priceTypeAttributeField;
            }
            set
            {
                this.priceTypeAttributeField = value;
            }
        }

        /// <remarks/>
        public string SellableUom
        {
            get
            {
                return this.sellableUomField;
            }
            set
            {
                this.sellableUomField = value;
            }
        }

        /// <remarks/>
        public string CurrencyCode
        {
            get
            {
                return this.currencyCodeField;
            }
            set
            {
                this.currencyCodeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime? TagExpirationDate
        {
            get
            {
                return this.tagExpirationDateField;
            }
            set
            {
                this.tagExpirationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TagExpirationDateSpecified
        {
            get
            {
                return this.tagExpirationDateFieldSpecified;
            }
            set
            {
                this.tagExpirationDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        public string ItemTypeCode
        {
            get
            {
                return this.itemTypeCodeField;
            }
            set
            {
                this.itemTypeCodeField = value;
            }
        }

        /// <remarks/>
        public string StoreName
        {
            get
            {
                return this.storeNameField;
            }
            set
            {
                this.storeNameField = value;
            }
        }

        /// <remarks/>
        public string ScanCode
        {
            get
            {
                return this.scanCodeField;
            }
            set
            {
                this.scanCodeField = value;
            }
        }

        /// <remarks/>
        public int? SubTeamNumber
        {
            get
            {
                return this.subTeamNumberField;
            }
            set
            {
                this.subTeamNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SubTeamNumberSpecified
        {
            get
            {
                return this.subTeamNumberFieldSpecified;
            }
            set
            {
                this.subTeamNumberFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Mammoth/MammothPrices.xsd")]
    [System.Xml.Serialization.XmlRootAttribute("MammothPrices", Namespace = "http://schemas.wfm.com/Mammoth/MammothPrices.xsd", IsNullable = false)]
    public partial class MammothPricesType
    {

        private MammothPriceType[] mammothPriceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MammothPrice")]
        public MammothPriceType[] MammothPrice
        {
            get
            {
                return this.mammothPriceField;
            }
            set
            {
                this.mammothPriceField = value;
            }
        }
    }
}
