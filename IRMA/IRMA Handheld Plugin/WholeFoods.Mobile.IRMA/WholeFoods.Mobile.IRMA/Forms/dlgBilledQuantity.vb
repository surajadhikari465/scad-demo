Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Reflection


Public Class dlgBilledQuantity

    Private mySession As Session
    Private identifier As String

    Public Sub New(ByVal Session As Session, ByVal _identifier As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.mySession = Session
        Me.identifier = _identifier
    End Sub

    Public Function DisplayBilledQuantity(ByVal vendorId As Integer, ByVal _itemDesc As String, ByVal _itemPackage As String) As Boolean
        Dim results As ListsItemBilledQty() = Nothing
        Dim vendorPakcage As Decimal
        Dim success As Boolean = False
        Dim labelHeight As Integer = 28
        Dim xPos As Integer = 5
        Dim yPos As Integer = 100

        itemDesc.Text = _itemDesc
        itemPackage.Text = _itemPackage

        vendorPakcage = Me.mySession.WebProxyClient.GetVendorPackage(mySession.StoreNo, vendorId, identifier)
        vPackage1.Text = CInt(vendorPakcage)

        results = Me.mySession.WebProxyClient.GetItemBilledQuantity(mySession.StoreNo, mySession.SubteamKey, identifier)

        If results.Count > 0 Then
            Dim labels = New Label() {Label6, Label7, Label8, Label9, Label10, Label11, Label12, Label13, Label17, Label16, Label15, Label14}

            Try
                Dim count As Integer = 3
                Dim j As Integer = 0
                For i As Integer = 0 To results.Length - 1
                    If count > 0 Then
                        labels(j).Visible = True
                        labels(j).Text = results(i).OrderDate.ToShortDateString()
                        labels(j + 1).Visible = True
                        labels(j + 1).Text = results(i).InvNum
                        labels(j + 2).Visible = True
                        labels(j + 2).Text = results(i).OrderQty.ToString(".00")
                        labels(j + 3).Visible = True
                        labels(j + 3).Text = results(i).BilledQty.ToString(".00")
                        If Math.Abs(results(i).OrderQty - results(i).BilledQty) > 0.01 Then
                            labels(j).ForeColor = Color.Red
                            labels(j + 1).ForeColor = Color.Red
                            labels(j + 2).ForeColor = Color.Red
                            labels(j + 3).ForeColor = Color.Red
                        End If
                        j = j + 4
                        count = count - 1
                    End If
                Next

                success = True

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "DisplayBilledQuantity")
                success = False
            End Try
        End If

        Return success
    End Function

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
        Me.Close()
    End Sub

End Class