Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Configuration
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports System.Data
Imports System.Data.SqlClient



<WebService(Name:="ESRS Web Service", Namespace:="http://iad-sw/IRMAWebServices/ESRS/", Description:="IRMA WebServices: ESRS Integration")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ESRS
    Inherits System.Web.Services.WebService
    ' function to get a list of items
    <WebMethod()> _
    Public Function GetESRSItemList(ByVal IdentifierList As String, ByVal IdentifierListSeparator As String, ByVal Store_No As String) As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As New DBParam
        Dim ds As DataSet

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "IdentifierList"
        currentParam.Value = IdentifierList
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        If IdentifierListSeparator.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "IdentifierListSeparator"
            currentParam.Value = IdentifierListSeparator
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        If Store_No.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ds = factory.GetStoredProcedureDataSet("GetESRSItemList", paramList)
        Return ds
    End Function
    <WebMethod()> _
    Public Function GetESRSPriceCheck(ByVal IdentifierList As String, ByVal IdentifierListSeparator As String, ByVal Store_No As String) As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As New DBParam
        Dim ds As DataSet

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "IdentifierList"
        currentParam.Value = IdentifierList
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        If IdentifierListSeparator.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "IdentifierListSeparator"
            currentParam.Value = IdentifierListSeparator
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        If Store_No.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ds = factory.GetStoredProcedureDataSet("GetESRSPriceCheck", paramList)
        Return ds
    End Function

    ' Function to lookup item details based on various criteria
    <WebMethod()> _
Public Function GetESRSItemSearch(ByVal Item_Description As String, ByVal Identifier As String, ByVal Brand_Name As String, ByVal SubTeam_No As String, ByVal Category_ID As String, ByVal Store_No As String, ByVal PriceChgTypeID As String) As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As New DBParam
        Dim ds As DataSet

        ' setup parameters for stored proc
        ' Item description
        If Item_Description.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "Item_Description"
            currentParam.Value = Item_Description
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ' UPC
        If Identifier.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Value = Identifier
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ' Brand Name
        If Brand_Name.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "Brand_Name"
            currentParam.Value = Brand_Name
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ' Sub-Team Number
        If SubTeam_No.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeam_No
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ' Product Category_ID
        If Category_ID.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "Category_ID"
            currentParam.Value = Category_ID
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ' Store Number
        If Store_No.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ' Price Change Type ID
        If PriceChgTypeID.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeID"
            currentParam.Value = PriceChgTypeID
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ds = factory.GetStoredProcedureDataSet("GetESRSItemSearch", paramList)
        Return ds
    End Function

    ' Function to lookup item details based on various criteria
    <WebMethod()> _
Public Function GetESRSItemsByDateRange(ByVal SaleStartDate As String, ByVal SaleEndDate As String, ByVal SubTeam_No As String, ByVal Category_ID As String, ByVal Store_No As String) As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As New DBParam
        Dim ds As DataSet

        ' setup parameters for stored proc
        ' Sale Start Date
        If SaleStartDate.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "SaleStartDate"
            currentParam.Value = SaleStartDate
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ' Sale End Date
        If SaleEndDate.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "SaleEndDate"
            currentParam.Value = SaleEndDate
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ' Sub-Team Number
        If SubTeam_No.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeam_No
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ' Product Category_ID
        If Category_ID.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "Category_ID"
            currentParam.Value = Category_ID
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ' Store Number
        If Store_No.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ds = factory.GetStoredProcedureDataSet("GetESRSItemsByDateRange", paramList)
        Return ds
    End Function

    ' function to get list of batches for a store
    ' Modified by Bryce Bartley on 3/31/08 to add additional vars
    <WebMethod()> _
    Public Function GetESRSBatchList(ByVal PriceBatchStatusID As String, _
    ByVal BatchDescription As String, _
    ByVal StartDate As String, _
    ByVal Store_No As String, _
    ByVal StartDateRangeTop As String, _
    ByVal StartDateRangeBottom As String, _
    ByVal PriceChgTypeID As String) As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As New DBParam
        Dim ds As DataSet

        ' setup parameters for stored proc
        If Store_No.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        If PriceChgTypeID.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeID"
            currentParam.Value = PriceChgTypeID
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        If BatchDescription.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "BatchDescription"
            currentParam.Value = BatchDescription
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        If PriceBatchStatusID.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "PriceBatchStatusID"
            currentParam.Value = PriceBatchStatusID
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        If StartDate.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Value = StartDate
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        If StartDateRangeTop.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "StartDateRangeTop"
            currentParam.Value = StartDateRangeTop
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        If StartDateRangeBottom.Length > 0 Then
            currentParam = New DBParam
            currentParam.Name = "StartDateRangeBottom"
            currentParam.Value = StartDateRangeBottom
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
        End If

        ds = factory.GetStoredProcedureDataSet("GetESRSBatchList", paramList)
        Return ds
    End Function

    ' function to get the details of a batch for a store
    <WebMethod()> _
    Public Function GetESRSBatchDetail(ByVal PriceBatchHeaderID As String, ByVal Store_No As String) As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As New DBParam
        Dim ds As DataSet

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "PriceBatchHeaderID"
        currentParam.Value = PriceBatchHeaderID
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = Store_No
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        ds = factory.GetStoredProcedureDataSet("GetESRSBatchDetail", paramList)
        Return ds
    End Function

    ' a pre-existing web query which could be useful
    ' For Identifier supply a * if null and for all others supply a 0 if null
    <WebMethod()> _
    Public Function GetItemWebQueryStore(ByVal Identifier As String, ByVal Item_Description As String, ByVal SubTeam_No As String, ByVal Brand_ID As String, ByVal Store_No As String) As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As New DBParam
        Dim ds As DataSet

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = Identifier
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Item_Description"
        currentParam.Value = Item_Description
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "SubTeam_No"
        currentParam.Value = SubTeam_No
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Brand_ID"
        currentParam.Value = Brand_ID
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = Store_No
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        ds = factory.GetStoredProcedureDataSet("GetItemWebQueryStore", paramList)
        Return ds
    End Function

    ' Function to get store list
    <WebMethod()> _
    Public Function GetStores() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim ds As DataSet

        ds = factory.GetStoredProcedureDataSet("GetStores")
        Return ds
    End Function

    ' Function to get sub teams
    <WebMethod()> _
    Public Function GetESRSSubTeams() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim ds As DataSet

        ds = factory.GetStoredProcedureDataSet("GetESRSSubTeams")
        Return ds
    End Function

    ' Function to get vendors
    <WebMethod()> _
    Public Function GetVendors() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim ds As DataSet

        ds = factory.GetStoredProcedureDataSet("GetVendors")
        Return ds
    End Function

    ' Function to get national product classes
    <WebMethod()> _
    Public Function GetNatClass() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim ds As DataSet

        ds = factory.GetStoredProcedureDataSet("GetNatClass")
        Return ds
    End Function

    ' Function to get regions
    <WebMethod()> _
    Public Function GetRegions() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim ds As DataSet

        ds = factory.GetStoredProcedureDataSet("GetRegions")
        Return ds
    End Function

    ' function to get Categories By Sub Team Number
    <WebMethod()> _
    Public Function GetCategoriesBySubTeam(ByVal SubTeam_No As Integer) As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As New DBParam
        Dim ds As DataSet

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "SubTeam_No"
        currentParam.Value = SubTeam_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        ds = factory.GetStoredProcedureDataSet("GetCategoriesBySubTeam", paramList)
        Return ds
    End Function

    ' Function to get Teams
    <WebMethod()> _
    Public Function GetTeams() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim ds As DataSet

        ds = factory.GetStoredProcedureDataSet("GetTeams")
        Return ds
    End Function

    ' Function to get Price batch status lists
    <WebMethod()> _
    Public Function GetPriceBatchStatusList() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim ds As DataSet

        ds = factory.GetStoredProcedureDataSet("GetPriceBatchStatusList")
        Return ds
    End Function

    ' Function to get Categories And SubTeams
    <WebMethod()> _
    Public Function GetCategoriesAndSubTeams() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim ds As DataSet

        ds = factory.GetStoredProcedureDataSet("GetCategoriesAndSubTeams")
        Return ds
    End Function
    ' Function to get Price Change Types
    <WebMethod()> _
    Public Function GetPriceTypes() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As New DBParam
        Dim ds As DataSet

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "IncludeReg"
        currentParam.Value = 1
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        ds = factory.GetStoredProcedureDataSet("GetPriceTypes", paramList)
        Return ds
    End Function
End Class