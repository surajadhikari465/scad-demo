﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.6.1590.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1590.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Mammoth/PriceMessageArchive.xsd")]
[System.Xml.Serialization.XmlRootAttribute("PriceMessageArchive", Namespace = "http://schemas.wfm.com/Mammoth/PriceMessageArchive.xsd", IsNullable = false)]
public partial class PriceMessageArchiveType
{

    private int itemIDField;

    private int businessUnitIDField;

    private string messageIDField;

    private string messageHeadersField;

    private string messageBodyField;

    private string errorCodeField;

    private string errorDetailsField;

    /// <remarks/>
    public int ItemID
    {
        get
        {
            return this.itemIDField;
        }
        set
        {
            this.itemIDField = value;
        }
    }

    /// <remarks/>
    public int BusinessUnitID
    {
        get
        {
            return this.businessUnitIDField;
        }
        set
        {
            this.businessUnitIDField = value;
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
    public string MessageHeaders
    {
        get
        {
            return this.messageHeadersField;
        }
        set
        {
            this.messageHeadersField = value;
        }
    }

    /// <remarks/>
    public string MessageBody
    {
        get
        {
            return this.messageBodyField;
        }
        set
        {
            this.messageBodyField = value;
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
}
