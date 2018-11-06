Imports System.Data
Imports System.Text
Imports System.Data.Common
Imports System.Data.SqlClient
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.AverageCostUpdate.DataAccess
    Public Class AverageCostUpdateDAO
        Public Shared Sub UpdateAverageCost(ByVal Store As Integer, ByVal Subteam As Integer)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim retval As ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "InStore_No"
            If Store = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = Store
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InSubteam_No"
            If Subteam = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = Subteam
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InStartDate"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            retval = factory.ExecuteStoredProcedure("UpdateAverageCost", paramList)
        End Sub

        Public Shared Function GetAllStores() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return factory.GetStoredProcedureDataTable("GetAllStores")
        End Function

        Public Shared Function GetAllSubteams() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return factory.GetStoredProcedureDataTable("GetAllSubteams")
        End Function

        Public Shared Function GetAllStoresLoadedSales() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim sql As New StringBuilder
            ' ******* Get List of Stores that have sales loaded for Today - Only these will be allowed to update average Cost ******
            sql.Append("Select Store_No from ItemHistory where Adjustment_ID = 3 and convert(varchar(15), insert_date, 101) = ")
            sql.Append("'" & Today.Date & "' ")
            sql.Append("group by Store_no")
            Return factory.GetDataTable(sql.ToString)
        End Function

    End Class
End Namespace