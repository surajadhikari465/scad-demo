Imports System.ComponentModel
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports WholeFoods.IRMA.CompetitorStore.DataAccess
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    Public Class CompetitorPriceGrid

        Public Overloads Sub Initialize(ByVal dataSet As CompetitorStoreDataSet, ByVal currentWeek As CompetitorStoreDataSet.FiscalWeekRow)
            _disablePrimaryKeyColumnEdit = True

            Me.Initialize(ugCompetitorPrices, dataSet, dataSet.CompetitorPrice.DefaultView, False, currentWeek)
        End Sub

        Private Sub ugCompetitorPrices_AfterRowInsert(ByVal sender As Object, ByVal e As RowEventArgs) Handles ugCompetitorPrices.AfterRowInsert
            e.Row.Cells("UpdateUserID").Value = giUserID
            e.Row.Cells("UpdateDateTime").Value = DateTime.Now
        End Sub

    End Class
End Namespace