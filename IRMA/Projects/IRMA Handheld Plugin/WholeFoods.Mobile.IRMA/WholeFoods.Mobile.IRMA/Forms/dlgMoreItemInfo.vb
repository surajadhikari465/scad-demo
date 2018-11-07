Public Class dlgMoreItemInfo
    Private mySession As Session
    Private identifier As String

    Public Sub New(ByVal Session As Session, ByVal _identifier As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.mySession = Session
        Me.identifier = _identifier
    End Sub

    Public Sub DisplayItemMove()

        Dim results As ListsItemMovement()
        Dim adjustmentID As Integer
        adjustmentID = 3

        results = Me.mySession.WebProxyClient.GetItemMovement(mySession.StoreBU, mySession.SubteamKey, identifier, adjustmentID)

        If results.Count > 0 Then
            Dim labels = New Label() {lblWeek1, lblWeek2, lblWeek3, lblWeek4, lblWeek5, lblWeek6}
            For i As Integer = 0 To results.Length - 1
                If (results(i) IsNot Nothing) Then
                    labels(i).Visible = True
                    labels(i).Text = Format(results(i).MovementDate, "MMM,d").ToString() + ": " + results(i).MovementQty.ToString(".00")
                End If
            Next
        Else
            lblWeek1.Text = "No item movement"
        End If

    End Sub

    Private Sub DisplayEInvoiceQuantity()
        Dim results As ListsItemBilledQty()

        results = Me.mySession.WebProxyClient.GetItemBilledQuantity(mySession.StoreBU, mySession.SubteamKey, identifier)
        If results.Count > 0 Then
            For i As Integer = 0 To results.Length - 1
                If results(i).InvoiceExist Then
                    Dim llabel = New Label
                    llabel.Location = New Point(60, 300 + i * 30)
                    'llabel.AutoSize = False
                    llabel.Size = New Size(100, 23)
                    ' llabel.BackColor = Color.GreenYellow
                    '  llabel.ForeColor = Color.Black


                    llabel.Text = results(i).BilledQty.ToString(".00") + " " + "Units"
                    Me.Controls.Add(llabel)
                End If
            Next
        Else
            Dim llabel = New Label
            llabel.Location = New Point(60, 300)
            llabel.Text = "Invoice is not loaded!"
            Me.Controls.Add(llabel)
        End If
    End Sub
    Private Sub btnClose_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class