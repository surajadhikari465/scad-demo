Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
    Public Class StoreAddDAO
        Public Shared Function GetSalePriceChgTypeList() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return factory.GetDataTable("Administration_GetSalePriceChgType")
        End Function

        Public Shared Function GetSourceStoreList(ByVal iPOSWriterKey As Int16) As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            If iPOSWriterKey = -1 Then
                Return factory.GetDataTable("GetStores")
            Else
                Return factory.GetDataTable("Administration_GetSourceStores @POSFileWriterKey = " & iPOSWriterKey)
            End If
        End Function

        Public Shared Function GetPOSSubteamList(ByVal iPOSWriterKey As Int16) As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return factory.GetDataTable("Administration_GetPOSSubTeams @POSFileWriterKey = " & iPOSWriterKey)
        End Function

        Public Shared Function GetSubStoreList(ByVal iSourceStore_No As Integer, _
                                                ByVal iSubteam_No As Integer, _
                                                ByVal iPOSWriterKey As Integer) As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim sql As String

            sql = "Administration_GetSubStores " & _
                    "@SourceStore_No = " & iSourceStore_No & _
                    " ,@Subteam_No = " & iSubteam_No & _
                    " ,@POSFileWriterKey = " & iPOSWriterKey

            Return factory.GetDataTable(sql)

        End Function

        Public Shared Function CheckDuplicateBusinessUnitID(ByVal iBuzUnitId) As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim sql As String
            Dim sResult As String = ""

            sql = "select Store_Name from Store  " & _
                    "where BusinessUnit_ID = " & iBuzUnitId 

            sResult = CType(factory.ExecuteScalar(sql), String)
            Return sResult
        End Function

        Public Shared Sub AddCloneStore(ByVal store As StoreAddBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "NewStoreNo"
            currentParam.Value = store.StoreID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreAbbr"
            currentParam.Value = store.StoreAbbr
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "NewStoreName"
            currentParam.Value = store.StoreName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreJurisdiction"
            currentParam.Value = store.StoreJurisdictionID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ZoneID"
            currentParam.Value = store.ZoneID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TaxJurisdiction"
            currentParam.Value = store.TaxJurisdictionID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "BusinessUnit_Id"
            currentParam.Value = store.BusinessUnit_Id
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PSI_Store_No"
            currentParam.Value = store.PSIStoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Plum_Store_No"
            currentParam.Value = store.PlumStoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OldStoreNo"
            currentParam.Value = store.SourceStoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ISSPriceChgTypeID"
            currentParam.Value = store.ISSPriceChgTypeID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreSubTeamSubstitutions"
            currentParam.Value = store.StoreSubTeamSubstitutions
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorName"
            currentParam.Value = store.VendorName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorAddress"
            currentParam.Value = store.VendorAddress
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorCity"
            currentParam.Value = store.VendorCity
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorState"
            currentParam.Value = store.VendorState
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorZipCode"
            currentParam.Value = store.VendorZipCode
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PeopleSoftVendorID"
            currentParam.Value = store.PeopleSoftVendorID
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IncSlim"
            currentParam.Value = store.IncSlim
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IncFutureSale"
            currentParam.Value = store.IncFutureSale
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IncPromoPlanner"
            currentParam.Value = store.IncPromoPlanner
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GeoCode"
            currentParam.Value = store.GeoCode
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsSourceStoreOnGpm"
            currentParam.Value = store.IsSourceStoreOnGpm
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("InsertCloneStore", paramList)
        End Sub
    End Class

End Namespace