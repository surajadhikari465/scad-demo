Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ServiceModel
Imports System.ServiceModel.Channels.Binding
Imports System.Security
Imports System.Object
Imports Symbol.Barcode

Public Class HandHeldDataCollect
    Dim UploadSucceeded As Boolean
    'Dim upcCount As Integer = 0
    Dim incomingUPC As String
    Dim mySQLDate As String
    Dim AllowNewItems As Boolean
    Dim MaxError As String = "You have reached the maximum number of items allowed in an Item List: " & MaxItems & vbNewLine & vbNewLine & "Please upload your Item List."
    Public Shared Function IsNSC2(ByVal str As String)
        Return New Regex("^02[0-9]{10}$").Match(str).Success
    End Function
    Public Shared Function ConvertNSC2(ByVal upc As String) As String
        Dim plu As String = upc.Substring(0, 7)
        upc = plu & "00000"
        Return upc
    End Function
    Private Sub HandHeldDataCollect_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Me.Hide()
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AppPath()
        'Label4.Text = ListBox1.Items.Count & " Item(s)"
    End Sub
    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        'Me.Close()
        Me.Hide()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) Then
            incomingUPC = TextBox1.Text
            If IsNSC2(incomingUPC) Then
                incomingUPC = ConvertNSC2(incomingUPC)
            End If
            ' If ListBox1.Items.Contains(incomingUPC) = False Then
            ListBox1.Items.Insert(0, incomingUPC)
            ListBox1.SelectedItem = incomingUPC
            '   End If
            Me.TextBox1.Text = ""
            upcLength = 0
            Me.TextBox1.Focus()
        End If
        'Label4.Text = ListBox1.Items.Count & " Item(s)"
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If ListBox1.SelectedItem = True Then
            ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
        End If
        'Label4.Text = ListBox1.Items.Count & " Item(s)"
        Me.TextBox1.Focus()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If MsgBox("Upload the Item List?", MsgBoxStyle.OkCancel, "Upload?") = MsgBoxResult.Ok Then
            If ListBox1.Items.Count = 0 Then
                MsgBox("Nothing to Upload!", MsgBoxStyle.Information, "Empty Item List")
            Else
                Application.DoEvents()
                ScanInSandbox(GetRawUpcs())
                Try
                    ScanProducts(GetUpcs())
                    ListBox1.Items.Clear()
                    Button2.Enabled = True
                    Me.Hide()
                Catch ex As System.Exception
                    MsgBox("Network Error. Please contact nexussupport@wholefoods.com " & ex.Message, MsgBoxStyle.Exclamation)
                End Try
            End If
        End If
        Me.TextBox1.Focus()
    End Sub

    Private Function GetRawUpcs() As List(Of String)
        Dim upcs As New List(Of String)
        For i As Integer = 0 To ListBox1.Items.Count - 1
            upcs.Add(ListBox1.Items(i).ToString)
        Next
        Return upcs
    End Function
    Private Function GetUpcs() As List(Of String)
        Dim upcs As New List(Of String)
        For i As Integer = 0 To ListBox1.Items.Count - 1
            upcs.Add(ListBox1.Items(i).ToString.PadLeft(13, "0"c))
        Next
        Return upcs
    End Function

    Private Sub ScanInSandbox(ByVal upcs As List(Of String))
        If (String.IsNullOrEmpty(oosUrlSandbox)) Then
            Return
        End If
        Try
            Scan(upcs, oosUrlSandbox)
        Catch ex As Exception
            ' Ignore
        End Try

    End Sub

    Private Sub Scan(ByVal upcs As List(Of String), ByVal url As String)
        If String.IsNullOrEmpty(Module1.storeName) Then
            MsgBox("Store cannot be null or empty", MsgBoxStyle.Exclamation, "Invalid Store")
            Return
        End If

        If String.IsNullOrEmpty(Module1.regionID) Then
            MsgBox("Region cannot be null or empty", MsgBoxStyle.Exclamation, "Invalid Region")
            Return
        End If

        Dim bt As New biztalk.ScanProductsByStoreAbbreviation
        bt.scanDate = DateTime.Now
        bt.scanDateSpecified = True
        bt.storeAbbrev = Module1.storeName
        bt.regionAbbrev = Module1.regionID
        bt.upcs = upcs.ToArray()
        Using bzt As New biztalk.BizTalkServiceInstance
            With bzt
                .Url = url
                bzt.ScanProductsByStoreAbbreviation(bt)
            End With
        End Using
    End Sub

    Private Sub ScanProducts(ByVal upcs As List(Of String))
        Scan(upcs, WcfAddress)
    End Sub

    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        DefineReport.Show()
    End Sub
    Public Shared Function IsNumeric(ByVal str As String)
        Return New Regex("^(([0-9]*[1-9][0-9]*([0-9]+)?)|([0]+[0-9]*[1-9][0-9]*))$").Match(str).Success
    End Function
    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) And e.KeyCode = Keys.Enter Then
            incomingUPC = TextBox1.Text
            If IsNSC2(incomingUPC) Then
                incomingUPC = ConvertNSC2(incomingUPC)
            End If
            'If ListBox1.Items.Contains(incomingUPC) = False Then
            ListBox1.Items.Insert(0, incomingUPC)
            ListBox1.SelectedItem = incomingUPC
            '  Label4.Text = ListBox1.Items.Count & " Item(s)"
            ' End If
            Me.TextBox1.Text = ""
            Me.TextBox1.Focus()
        End If
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text.EndsWith(vbNewLine) Then
            Me.TextBox1.Text = Me.TextBox1.Text.Replace(vbNewLine, "")
            If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) Then
                incomingUPC = TextBox1.Text
                If IsNSC2(incomingUPC) Then
                    incomingUPC = ConvertNSC2(incomingUPC)
                End If
                '    If ListBox1.Items.Contains(incomingUPC) = False Then
                ListBox1.Items.Insert(0, incomingUPC)
                ListBox1.SelectedItem = incomingUPC
                ' End If
                Me.TextBox1.Text = ""
                Me.TextBox1.Focus()
            End If
        End If
    End Sub
    Private Sub ListBox1_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedValueChanged
        If ListBox1.Items.Count > MaxItems Then
            If ListBox1.Items.Contains(incomingUPC) = True Then
                ListBox1.Items.Remove(incomingUPC)
            End If
            MsgBox(MaxError, MsgBoxStyle.Exclamation, "Item List Limit")
        End If
    End Sub
End Class



