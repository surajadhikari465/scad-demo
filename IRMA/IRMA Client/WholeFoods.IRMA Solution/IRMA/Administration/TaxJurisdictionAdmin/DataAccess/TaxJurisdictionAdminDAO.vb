Imports System.Data.SqlClient
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Public Class TaxJurisdictionAdminDAO
    Inherits WholeFoods.IRMA.TaxHosting.DataAccess.TaxJurisdictionDAO

    ''' <summary>
    ''' Calls stored procedure to insert clone Tax Jurisdiction
    ''' </summary>
    ''' <param name="newTaxJur"></param>
    ''' <remarks></remarks>
    Public Function InsertCloneTaxJurisdiction(ByVal newTaxJur As TaxJurisdictionAdminBO) As TaxJurisdictionAdminBO
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim outParamList As New ArrayList
        Dim currentParam As DBParam

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "OldTaxJurisdictionID"
        currentParam.Value = newTaxJur.OldTaxJurisdicitonID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "NewTaxJurisdictionID"
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "TaxJurisdictionDesc"
        currentParam.Value = newTaxJur.TaxJurisdictionDesc
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "LastUpdateUserID"
        currentParam.Value = newTaxJur.LastUpdateUserID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "newLastUpdate"
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)

        outParamList = factory.ExecuteStoredProcedure("InsertCloneTaxJurisdiction", paramList)

        'need the output params
        newTaxJur.TaxJurisdictionId = outParamList.Item(0)

        Return newTaxJur
    End Function

    Public Sub DeleteJurisdiction(ByVal remTaxJur As TaxJurisdictionAdminBO)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "TaxJurisdictionID"
        currentParam.Value = remTaxJur.TaxJurisdictionId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        factory.ExecuteStoredProcedure("TaxHosting_DeleteTaxJurisdiction", paramList)

    End Sub

    Public Sub UpdateJurisdiction(ByVal upTaxJur As TaxJurisdictionAdminBO)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "TaxJurisdictionID"
        currentParam.Value = upTaxJur.TaxJurisdictionId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "TaxJurisdictionDesc"
        currentParam.Value = upTaxJur.TaxJurisdictionDesc
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "LastUpdate"
        currentParam.Value = upTaxJur.LastUpdate
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "LastUpdateUserID"
        currentParam.Value = upTaxJur.LastUpdateUserID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "newLastUpdate"
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)

        factory.ExecuteStoredProcedure("TaxHosting_UpdateTaxJurisdiction", paramList)

    End Sub
End Class

