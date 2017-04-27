Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class FiscalWeek

        ''' <summary>
        ''' Gets the fiscal weeks between the given start and end date. If either is not given, 
        ''' then it is assumed to be unbounded in that direction.
        ''' </summary>
        ''' <param name="dataSet"></param>
        ''' <remarks></remarks>
        Public Shared Sub List(ByVal dataSet As CompetitorStoreDataSet)
            List(dataSet, Nothing, Nothing)
        End Sub

        ''' <summary>
        ''' Gets the fiscal weeks between the given start and end date. If either is not given, 
        ''' then it is assumed to be unbounded in that direction.
        ''' </summary>
        ''' <param name="dataSet"></param>
        ''' <param name="startDate"></param>
        ''' <param name="endDate"></param>
        ''' <remarks></remarks>
        Public Shared Sub List(ByVal dataSet As CompetitorStoreDataSet, ByVal startDate As Nullable(Of DateTime), ByVal endDate As Nullable(Of DateTime))
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = factory.GetDataCommand("GetFiscalWeeks", Nothing, True)
            Dim param As SqlParameter

            If startDate.HasValue Then
                param = New SqlParameter("StartDate", SqlDbType.DateTime)
                param.Value = startDate.Value
                command.Parameters.Add(param)
            End If

            If endDate.HasValue Then
                param = New SqlParameter("EndDate", SqlDbType.DateTime)
                param.Value = endDate.Value
                command.Parameters.Add(param)
            End If

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(command, Nothing)
                adapter.Fill(dataSet.FiscalWeek)
            End Using
        End Sub

        Public Shared Function FindCurrent(ByVal dataSet As CompetitorStoreDataSet) As CompetitorStoreDataSet.FiscalWeekRow
            Dim view As New DataView(dataSet.FiscalWeek)
            Dim currentWeek As CompetitorStoreDataSet.FiscalWeekRow = Nothing

            view.RowFilter = String.Format("#{0}# >= StartDate AND #{0}# <= EndDate", DateTime.Today)

            If view.Count = 1 Then
                currentWeek = CType(view(0).Row, CompetitorStoreDataSet.FiscalWeekRow)
            End If

            Return currentWeek
        End Function

    End Class
End Namespace