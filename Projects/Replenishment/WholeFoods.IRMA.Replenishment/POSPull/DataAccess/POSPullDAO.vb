Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Object
Imports System.Collections
Imports System.IO
Imports WholeFoods.IRMA.Replenishment.POSPull.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.POSPull.DataAccess
    Public Class POSPullDAO
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.POSPush.DataAccess.POSPullDAO")

        Public Shared Sub InsertPOSItems(ByVal storeNo As Integer, ByVal pathFileName As String)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                If storeNo <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = storeNo
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PathFileName"
                If pathFileName Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = pathFileName
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
                factory.ExecuteStoredProcedure("Replenishment_POSPull_InsertPOSItem", paramList)
            Finally
                factory = Nothing
                paramList = Nothing
                currentParam = Nothing
            End Try
        End Sub

        Public Shared Sub InsertTempPriceAudit(ByVal storeNo As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                If storeNo <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = storeNo
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
                factory.ExecuteStoredProcedure("Replenishment_POSPull_InsertTempPriceAudit", paramList)
            Finally
                factory = Nothing
                paramList = Nothing
                currentParam = Nothing
            End Try
        End Sub
        Public Function GetItemDataset(ByVal strsql As String, ByRef ItemDataset As DataSet, ByVal strFillTableName As String) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As DataSet = Nothing
            Try
                reader = factory.GetDataSet("Exec Replenishment_POSPull_GetIdentifier", ItemDataset, strFillTableName)
            Finally
                factory = Nothing
            End Try

            Return reader

        End Function

        Public Shared Function GetPOSAuditExceptions(ByVal Store_No As Integer) As SqlDataReader

            Logger.LogDebug("GetPOSAuditExceptions: Store_No=" + Store_No.ToString, CLASSTYPE)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList

            ' setup parameters for stored proc
            paramList = New ArrayList

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.CommandTimeout = 0

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("Replenishment_POSAudit_FindExceptions", paramList)

            Logger.LogDebug("GetPOSAuditExceptions exit", CLASSTYPE)

            Return results
        End Function

        Public Shared Function GetNoOfPOSExceptions(ByVal Store_No As Integer) As Long

            Logger.LogDebug("GetNoOfPOSAuditExceptions: Store_No=" + Store_No.ToString, CLASSTYPE)

            Dim count As Int32 = 0
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList

            ' setup parameters for stored proc
            paramList = New ArrayList

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.CommandTimeout = 0

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("Replenishment_POSAudit_GetNoOfExceptions", paramList)

            If (results.HasRows) Then
                While (results.Read())
                    count = results.GetInt32(0)
                End While
            Else
                count = 0
            End If

            Logger.LogDebug("GetNoOfPOSAuditExceptions exit", CLASSTYPE)

            Return count
        End Function

        Public Function GetSubteams(ByVal strsql As String, ByVal ItemDataset As DataSet, ByVal strFillTableName As String) As DataSet

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As DataSet = Nothing

            Try
                ' Execute the stored procedure 
                reader = factory.GetDataSet("Exec GetAllSubTeams", ItemDataset, strFillTableName)
            Finally
                factory = Nothing
            End Try
            Return reader
        End Function

        Public Shared Function GetSubTeamDataTable() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim subteamTable As DataTable = Nothing
            Try
                subteamTable = factory.GetStoredProcedureDataSet("GetSubTeams").Tables.Item(0)

            Catch ex As Exception
                'HandleError(ex)
            Finally
                factory = Nothing
            End Try
            Return subteamTable
        End Function
    End Class
End Namespace
