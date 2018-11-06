Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports WholeFoods.IRMA.CompetitorStore.DataAccess

Namespace WholeFoods.IRMA.Reporting.UserInterface
    Public Class CompetitorTrendReport

        Public Sub New(ByVal results As DataTable, ByVal title As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            ucTrend.TitleTop.Text = title

            ' Set Chart and Grid control data source
            ugData.DataSource = results
            ucTrend.DataSource = results
            ucTrend.DataBind()

            Dim columns As ColumnsCollection = ugData.DisplayLayout.Bands(0).Columns
            For i As Integer = 1 To columns.Count - 1
                columns(i).CellActivation = Activation.NoEdit
                columns(i).Format = "c"
                columns(i).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            Next
        End Sub
    End Class
End Namespace