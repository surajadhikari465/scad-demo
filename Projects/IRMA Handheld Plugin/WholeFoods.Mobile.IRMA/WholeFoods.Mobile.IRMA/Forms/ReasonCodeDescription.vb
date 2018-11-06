Public Class ReasonCodeDescription

    Private dataTable As New Data.DataTable("ReasonCodes")
    Private reasonCodes As ReasonCode()

    Public Sub New(ByVal reasonCodes As ReasonCode())

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.reasonCodes = reasonCodes
        AlignText()
    End Sub

    Private Sub ReasonCodeDescription_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dataTable.Columns.Add("Code")
        dataTable.Columns.Add("Description")

        For Each reasonCode As ReasonCode In reasonCodes
            dataTable.Rows.Add(reasonCode.ReasonCodeAbbreviation, reasonCode.ReasonCodeDescription)
        Next

        DataGridReasonCode.DataSource = dataTable
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub AlignText()
        LabelHeader.TextAlign = ContentAlignment.TopCenter
    End Sub
End Class