Public Class dlgMultiplePOSelector

    Private dataTable As New Data.DataTable("POSelector")
    Private orderList As ListsExternalOrder()

    Public Sub New(ByVal orderList As ListsExternalOrder())

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.orderList = orderList
        AlignText()
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub dlgMultiplePOSelector_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dataTable.Columns.Add("IRMAPO")
        dataTable.Columns.Add("Source")
        dataTable.Columns.Add("Vendor")

        For Each order As ListsExternalOrder In orderList
            dataTable.Rows.Add(order.OrderHeader_ID, order.Source, order.CompanyName)
        Next

        DataGridPOSelector.DataSource = dataTable
    End Sub

    Private Sub DataGridPOSelector_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridPOSelector.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub AlignText()
        LabelHeader.TextAlign = ContentAlignment.TopCenter
    End Sub
End Class