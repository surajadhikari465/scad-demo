Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class CompetitorTrend

#Region "Helper Methods"

        Private Shared Sub AddValueParameter(ByVal parameterValue As Object, ByVal parameterType As SqlDbType, ByVal parameterName As String, ByVal command As SqlCommand)
            Dim param As New SqlParameter(parameterName, parameterType)
            param.Value = parameterValue
            command.Parameters.Add(param)
        End Sub

#End Region

        Public Shared Sub RunReport(ByVal results As DataTable, ByVal store_NoList As String, ByVal competitorStoreIDs As String, _
            ByVal item_Key As Integer, ByVal regularPrice As Boolean, ByVal startFiscalWeek As CompetitorStoreDataSet.FiscalWeekRow, _
            ByVal endFiscalWeek As CompetitorStoreDataSet.FiscalWeekRow)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = factory.GetDataCommand("Reporting_CompetitorTrend", Nothing, True)

            If Not String.IsNullOrEmpty(store_NoList) Then
                AddValueParameter(store_NoList, SqlDbType.VarChar, "Store_NoList", command)
            End If

            If Not String.IsNullOrEmpty(competitorStoreIDs) Then
                AddValueParameter(competitorStoreIDs, SqlDbType.VarChar, "CompetitorStoreIDs", command)
            End If

            AddValueParameter(item_Key, SqlDbType.Int, "Item_Key", command)
            AddValueParameter(IIf(regularPrice, 1, 0), SqlDbType.Bit, "RegularPrice", command)
            AddValueParameter(startFiscalWeek.FiscalYear, SqlDbType.SmallInt, "StartFiscalYear", command)
            AddValueParameter(startFiscalWeek.FiscalPeriod, SqlDbType.TinyInt, "StartFiscalPeriod", command)
            AddValueParameter(startFiscalWeek.PeriodWeek, SqlDbType.TinyInt, "StartPeriodWeek", command)
            AddValueParameter(endFiscalWeek.FiscalYear, SqlDbType.SmallInt, "EndFiscalYear", command)
            AddValueParameter(endFiscalWeek.FiscalPeriod, SqlDbType.TinyInt, "EndFiscalPeriod", command)
            AddValueParameter(endFiscalWeek.PeriodWeek, SqlDbType.TinyInt, "EndPeriodWeek", command)

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(command, Nothing)
                adapter.Fill(results)
            End Using
        End Sub

    End Class
End Namespace