Imports System.ComponentModel
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports WholeFoods.IRMA.CompetitorStore.DataAccess
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    Public Class CompetitorImportInfoGrid
        Public Overloads Sub Initialize(ByVal dataSet As CompetitorStoreDataSet)
            Me.Initialize(ugPreview, dataSet, dataSet.CompetitorImportInfo.DefaultView)
        End Sub
    End Class
End Namespace