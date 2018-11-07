Public Class PrintRequestBatchName

    Private storeNumber As Integer

    Private _batchName As String
    Public ReadOnly Property BatchName() As String
        Get
            Return _batchName
        End Get
    End Property

    Private _applyNoTagLogic As Boolean
    Public ReadOnly Property ApplyNoTagLogic() As Boolean
        Get
            Return _applyNoTagLogic
        End Get
    End Property

    Private _sendToAllStores As Boolean
    Public ReadOnly Property SendToAllStores() As Boolean
        Get
            Return _sendToAllStores
        End Get
    End Property

    Public Sub New(storeNumber As Integer, subteamName As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        TextBoxBatchName.Text = String.Format("{0} - {1} - {2}", DateTime.Now.ToString("MM/dd"), subteamName, "Ad-hoc")

        ' User has access to all stores.
        If glStore_Limit = 0 Then
            CheckBoxApplyNoTagLogic.Enabled = True
            CheckBoxSendToAllStores.Enabled = True
        End If
    End Sub

    Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
        _batchName = TextBoxBatchName.Text
        _sendToAllStores = CheckBoxSendToAllStores.Checked
        _applyNoTagLogic = CheckBoxApplyNoTagLogic.Checked

        DialogResult = DialogResult.OK
    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Me.Close()
    End Sub
End Class