﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Class IRMA
        
        Private Shared resourceMan As Global.System.Resources.ResourceManager
        
        Private Shared resourceCulture As Global.System.Globalization.CultureInfo
        
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("IRMA", GetType(IRMA).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to All Stores.
        '''</summary>
        Friend Shared ReadOnly Property AllStores() As String
            Get
                Return ResourceManager.GetString("AllStores", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to IRMA Application is Already Open.{0}Cannot have more than {1} instances of the application..
        '''</summary>
        Friend Shared ReadOnly Property AppAlreadyRunning() As String
            Get
                Return ResourceManager.GetString("AppAlreadyRunning", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Apply changes anyway?.
        '''</summary>
        Friend Shared ReadOnly Property ApplyChanges() As String
            Get
                Return ResourceManager.GetString("ApplyChanges", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Print batch requests were sent to SLAW.
        '''</summary>
        Friend Shared ReadOnly Property BypassNoTagLogicInformationMessage() As String
            Get
                Return ResourceManager.GetString("BypassNoTagLogicInformationMessage", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Checking this box will result in No-Tag Transaction history being ignored when sending the Print Batch Request to SLAW (NOTE all other No-Tag exclusions will be enforced). Are you sure you want to ignore No-Tag Transaction History for the selected PrintBatches?.
        '''</summary>
        Friend Shared ReadOnly Property BypassNoTagLogicWarning() As String
            Get
                Return ResourceManager.GetString("BypassNoTagLogicWarning", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to This row cannot be changed.
        '''</summary>
        Friend Shared ReadOnly Property CannotChangeRow() As String
            Get
                Return ResourceManager.GetString("CannotChangeRow", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Some rows were not editable and could not be deleted.
        '''</summary>
        Friend Shared ReadOnly Property CannotDeleteRow() As String
            Get
                Return ResourceManager.GetString("CannotDeleteRow", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Changes not applied due to the following error(s).
        '''</summary>
        Friend Shared ReadOnly Property ChangesNotApplied() As String
            Get
                Return ResourceManager.GetString("ChangesNotApplied", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Company Name.
        '''</summary>
        Friend Shared ReadOnly Property CompanyName() As String
            Get
                Return ResourceManager.GetString("CompanyName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to d/M/yyyy.
        '''</summary>
        Friend Shared ReadOnly Property DateFormatCustom() As String
            Get
                Return ResourceManager.GetString("DateFormatCustom", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Date is invalid.
        '''</summary>
        Friend Shared ReadOnly Property DateInvalid() As String
            Get
                Return ResourceManager.GetString("DateInvalid", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to M/dd/yyyy.
        '''</summary>
        Friend Shared ReadOnly Property DateStringFormat() As String
            Get
                Return ResourceManager.GetString("DateStringFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Def.
        '''</summary>
        Friend Shared ReadOnly Property Def() As String
            Get
                Return ResourceManager.GetString("Def", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Del.
        '''</summary>
        Friend Shared ReadOnly Property Del() As String
            Get
                Return ResourceManager.GetString("Del", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Delete all highlighted rows?.
        '''</summary>
        Friend Shared ReadOnly Property DeleteHighlightedRows() As String
            Get
                Return ResourceManager.GetString("DeleteHighlightedRows", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Description.
        '''</summary>
        Friend Shared ReadOnly Property Description() As String
            Get
                Return ResourceManager.GetString("Description", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} is a duplicate.  You must enter a unique value..
        '''</summary>
        Friend Shared ReadOnly Property Duplicate() As String
            Get
                Return ResourceManager.GetString("Duplicate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to End Date.
        '''</summary>
        Friend Shared ReadOnly Property EndDate() As String
            Get
                Return ResourceManager.GetString("EndDate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to End Date must be greater than Start Date.
        '''</summary>
        Friend Shared ReadOnly Property EndDateGreater() As String
            Get
                Return ResourceManager.GetString("EndDateGreater", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to End Date must be greater than or equal to Start Date.
        '''</summary>
        Friend Shared ReadOnly Property EndDateGreaterEqual() As String
            Get
                Return ResourceManager.GetString("EndDateGreaterEqual", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to End Date is invalid.
        '''</summary>
        Friend Shared ReadOnly Property EndDateInvalid() As String
            Get
                Return ResourceManager.GetString("EndDateInvalid", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to You must enter a Sub Team..
        '''</summary>
        Friend Shared ReadOnly Property EnterSubTeam() As String
            Get
                Return ResourceManager.GetString("EnterSubTeam", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Network logon failed.  Please contact the helpdesk for assistance..
        '''</summary>
        Friend Shared ReadOnly Property FailedLogon() As String
            Get
                Return ResourceManager.GetString("FailedLogon", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Identifier.
        '''</summary>
        Friend Shared ReadOnly Property Identifier() As String
            Get
                Return ResourceManager.GetString("Identifier", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} cannot contain character(s) &apos;{1}&apos;..
        '''</summary>
        Friend Shared ReadOnly Property InvalidCharacters() As String
            Get
                Return ResourceManager.GetString("InvalidCharacters", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Invalid date range.
        '''</summary>
        Friend Shared ReadOnly Property InvalidDateRange() As String
            Get
                Return ResourceManager.GetString("InvalidDateRange", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Your IRMA account has been disabled.{0}Please contact the helpdesk for assistance..
        '''</summary>
        Friend Shared ReadOnly Property IRMAAccountDisabled() As String
            Get
                Return ResourceManager.GetString("IRMAAccountDisabled", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to IRMA account not found.{0}Please contact the helpdesk for assistance..
        '''</summary>
        Friend Shared ReadOnly Property IRMAAccountNotFound() As String
            Get
                Return ResourceManager.GetString("IRMAAccountNotFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Item.
        '''</summary>
        Friend Shared ReadOnly Property Item() As String
            Get
                Return ResourceManager.GetString("Item", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Item Description.
        '''</summary>
        Friend Shared ReadOnly Property ItemDescription() As String
            Get
                Return ResourceManager.GetString("ItemDescription", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to ITEM_KEY.
        '''</summary>
        Friend Shared ReadOnly Property ItemKey() As String
            Get
                Return ResourceManager.GetString("ItemKey", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Last Cost.
        '''</summary>
        Friend Shared ReadOnly Property LastCost() As String
            Get
                Return ResourceManager.GetString("LastCost", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Location.
        '''</summary>
        Friend Shared ReadOnly Property Location() As String
            Get
                Return ResourceManager.GetString("Location", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to More data is available.{0}For more data, please limit search criteria..
        '''</summary>
        Friend Shared ReadOnly Property MoreDataAvailable() As String
            Get
                Return ResourceManager.GetString("MoreDataAvailable", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Tare values must be less than or equal to 1.000 or equal 9.999.
        '''</summary>
        Friend Shared ReadOnly Property msg_error_InvalidTareValue() As String
            Get
                Return ResourceManager.GetString("msg_error_InvalidTareValue", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} must contain a numeric value. Alpha characters are not allowed. i.e. 0003726.
        '''</summary>
        Friend Shared ReadOnly Property msg_NumericValue() As String
            Get
                Return ResourceManager.GetString("msg_NumericValue", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to MSRP.
        '''</summary>
        Friend Shared ReadOnly Property MSRP() As String
            Get
                Return ResourceManager.GetString("MSRP", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to You must select an item from the grid..
        '''</summary>
        Friend Shared ReadOnly Property MustSelect() As String
            Get
                Return ResourceManager.GetString("MustSelect", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Nat.
        '''</summary>
        Friend Shared ReadOnly Property Nat() As String
            Get
                Return ResourceManager.GetString("Nat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to New Cost.
        '''</summary>
        Friend Shared ReadOnly Property NewCost() As String
            Get
                Return ResourceManager.GetString("NewCost", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to New.
        '''</summary>
        Friend Shared ReadOnly Property NewTitle() As String
            Get
                Return ResourceManager.GetString("NewTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 99.
        '''</summary>
        Friend Shared ReadOnly Property NinetyNine() As String
            Get
                Return ResourceManager.GetString("NinetyNine", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to None Found..
        '''</summary>
        Friend Shared ReadOnly Property NoneFound() As String
            Get
                Return ResourceManager.GetString("NoneFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No data matches the specified criteria.
        '''</summary>
        Friend Shared ReadOnly Property NoSearchData() As String
            Get
                Return ResourceManager.GetString("NoSearchData", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} cannot be zero..
        '''</summary>
        Friend Shared ReadOnly Property NotZero() As String
            Get
                Return ResourceManager.GetString("NotZero", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to #####0.##.
        '''</summary>
        Friend Shared ReadOnly Property NumberFormatBigDecimal() As String
            Get
                Return ResourceManager.GetString("NumberFormatBigDecimal", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to #####0.
        '''</summary>
        Friend Shared ReadOnly Property NumberFormatBigInteger() As String
            Get
                Return ResourceManager.GetString("NumberFormatBigInteger", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to #####0.###.
        '''</summary>
        Friend Shared ReadOnly Property NumberFormatDecimal3() As String
            Get
                Return ResourceManager.GetString("NumberFormatDecimal3", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to ##0.
        '''</summary>
        Friend Shared ReadOnly Property NumberFormatSmallInteger() As String
            Get
                Return ResourceManager.GetString("NumberFormatSmallInteger", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} Selected.
        '''</summary>
        Friend Shared ReadOnly Property NumberSelected() As String
            Get
                Return ResourceManager.GetString("NumberSelected", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} Total.
        '''</summary>
        Friend Shared ReadOnly Property NumberTotal() As String
            Get
                Return ResourceManager.GetString("NumberTotal", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to This must contain a numeric value. Alpha characters are not allowed. i.e. 0003726.
        '''</summary>
        Friend Shared ReadOnly Property NumericValue() As String
            Get
                Return ResourceManager.GetString("NumericValue", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 1.
        '''</summary>
        Friend Shared ReadOnly Property One() As String
            Get
                Return ResourceManager.GetString("One", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Package selected batches?.
        '''</summary>
        Friend Shared ReadOnly Property PackageBatches() As String
            Get
                Return ResourceManager.GetString("PackageBatches", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Quantity.
        '''</summary>
        Friend Shared ReadOnly Property Quantity() As String
            Get
                Return ResourceManager.GetString("Quantity", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Quantity cannot be zero.{0}Enter Quantity?.
        '''</summary>
        Friend Shared ReadOnly Property QuantityNotZero() As String
            Get
                Return ResourceManager.GetString("QuantityNotZero", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Read Only ({0}).
        '''</summary>
        Friend Shared ReadOnly Property ReadOnlyUserName() As String
            Get
                Return ResourceManager.GetString("ReadOnlyUserName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} is required..
        '''</summary>
        Friend Shared ReadOnly Property Required() As String
            Get
                Return ResourceManager.GetString("Required", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Save changes?.
        '''</summary>
        Friend Shared ReadOnly Property SaveChanges() As String
            Get
                Return ResourceManager.GetString("SaveChanges", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0}Save changes?.
        '''</summary>
        Friend Shared ReadOnly Property SaveChanges1() As String
            Get
                Return ResourceManager.GetString("SaveChanges1", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Please select a single row..
        '''</summary>
        Friend Shared ReadOnly Property SelectSingleRow() As String
            Get
                Return ResourceManager.GetString("SelectSingleRow", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No store has been selected..
        '''</summary>
        Friend Shared ReadOnly Property SelectStore() As String
            Get
                Return ResourceManager.GetString("SelectStore", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No SubTeam has been selected..
        '''</summary>
        Friend Shared ReadOnly Property SelectSubTeam() As String
            Get
                Return ResourceManager.GetString("SelectSubTeam", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Really send changes?.
        '''</summary>
        Friend Shared ReadOnly Property SendChanges() As String
            Get
                Return ResourceManager.GetString("SendChanges", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Start and End Dates are required..
        '''</summary>
        Friend Shared ReadOnly Property StartAndEndDatesRequired() As String
            Get
                Return ResourceManager.GetString("StartAndEndDatesRequired", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Start Date.
        '''</summary>
        Friend Shared ReadOnly Property StartDate() As String
            Get
                Return ResourceManager.GetString("StartDate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Start Date is invalid.
        '''</summary>
        Friend Shared ReadOnly Property StartDateInvalid() As String
            Get
                Return ResourceManager.GetString("StartDateInvalid", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Start Date cannot be in the past.
        '''</summary>
        Friend Shared ReadOnly Property StartDateNotPast() As String
            Get
                Return ResourceManager.GetString("StartDateNotPast", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Store Name.
        '''</summary>
        Friend Shared ReadOnly Property StoreName() As String
            Get
                Return ResourceManager.GetString("StoreName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Store_No.
        '''</summary>
        Friend Shared ReadOnly Property StoreNumber() As String
            Get
                Return ResourceManager.GetString("StoreNumber", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to You are logging into a development or test system..
        '''</summary>
        Friend Shared ReadOnly Property TestWarning() As String
            Get
                Return ResourceManager.GetString("TestWarning", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 2.
        '''</summary>
        Friend Shared ReadOnly Property Two() As String
            Get
                Return ResourceManager.GetString("Two", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Type.
        '''</summary>
        Friend Shared ReadOnly Property Type() As String
            Get
                Return ResourceManager.GetString("Type", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Really unlock this record?.
        '''</summary>
        Friend Shared ReadOnly Property UnlockRecord() As String
            Get
                Return ResourceManager.GetString("UnlockRecord", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Update completed - {0} records affected.
        '''</summary>
        Friend Shared ReadOnly Property UpdateCompleted() As String
            Get
                Return ResourceManager.GetString("UpdateCompleted", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to You must enter a valid date..
        '''</summary>
        Friend Shared ReadOnly Property ValidDate() As String
            Get
                Return ResourceManager.GetString("ValidDate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Vendor_ID.
        '''</summary>
        Friend Shared ReadOnly Property VendorID() As String
            Get
                Return ResourceManager.GetString("VendorID", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Vendor ID.
        '''</summary>
        Friend Shared ReadOnly Property VendorIDTitle() As String
            Get
                Return ResourceManager.GetString("VendorIDTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Vendor Name.
        '''</summary>
        Friend Shared ReadOnly Property VendorName() As String
            Get
                Return ResourceManager.GetString("VendorName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to WFM.
        '''</summary>
        Friend Shared ReadOnly Property WFM() As String
            Get
                Return ResourceManager.GetString("WFM", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to yyyy-MM-dd.
        '''</summary>
        Friend Shared ReadOnly Property YearDateFormat() As String
            Get
                Return ResourceManager.GetString("YearDateFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 0.
        '''</summary>
        Friend Shared ReadOnly Property Zero() As String
            Get
                Return ResourceManager.GetString("Zero", resourceCulture)
            End Get
        End Property
    End Class
End Namespace
