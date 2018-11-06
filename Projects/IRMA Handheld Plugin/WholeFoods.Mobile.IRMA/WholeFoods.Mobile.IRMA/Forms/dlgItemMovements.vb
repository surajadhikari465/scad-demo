Public Class dlgItemMovements

    Private mySession As Session
    Private identifier As String

    Public Sub New(ByVal Session As Session, ByVal _identifier As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.mySession = Session
        Me.identifier = _identifier
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Public Function dlgItemMovement_Load(ByVal vendorId As Integer, ByVal _itemDesc As String, ByVal _itemPackage As String) As Boolean
        Dim results As ListsItemMovement()
        Dim adjustmentID As Integer
        Dim vendorPakcage As Decimal
        Dim j As Integer
        Dim success As Boolean = False

        adjustmentID = 3
        itemDesc.Text = _itemDesc
        itemPackage.Text = _itemPackage

        vendorPakcage = Me.mySession.WebProxyClient.GetVendorPackage(mySession.StoreNo, vendorId, identifier)
        vPackage1.Text = CInt(vendorPakcage)

        results = Me.mySession.WebProxyClient.GetItemMovement(mySession.StoreNo, mySession.SubteamKey, identifier, adjustmentID)

        Dim labels = New Label() {lblWeek1, week1Qty, lblWeek2, week2Qty, lblWeek3, week3Qty, lblWeek4, week4Qty, lblWeek5, week5Qty, lblWeek6, week6Qty}
        If results.Count > 0 Then
            j = 0
            For i As Integer = 0 To results.Length - 1
                If (results(i) IsNot Nothing) Then
                    labels(j).Visible = True
                    labels(j).Text = Format(results(i).MovementDate, "d,MMM").ToString() + ":"
                    labels(j + 1).Visible = True
                    labels(j + 1).Text = results(i).MovementQty.ToString(".00")
                End If
                j = j + 2
            Next
            success = True
        End If
        Return success
    End Function

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
        Me.Close()
    End Sub
End Class