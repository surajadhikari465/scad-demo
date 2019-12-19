Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports PsionTeklogix.Barcode
Imports OOS.quiver_tst
Imports System.Security
Imports System.Object

Public Class HandHeldDataCollect
    Public Class vimProduct
        Public UPC As String
        Public Brand As String
        Public BrandName As String
        Public ProductName As String
        Public LotCode As String
        Public BestByDate As String
    End Class
    '    Dim UploadSucceeded As Boolean
    Friend WithEvents ScannerServicesDriver1 As New PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver
    Friend WithEvents Scanner1 As New PsionTeklogix.Barcode.Scanner
    Dim incomingUPC As String
    '    Dim mySQLDate As String
    '    Dim AllowNewItems As Boolean
    Dim _
        MaxError As String = "You have reached the maximum number of items allowed in an Item List: " & MaxItems & _
                             vbNewLine & vbNewLine & "Please upload your Item List."

    Private _undo As New Dictionary(Of String, Integer)

    Private Const UPCA_SUFFIX_KEY As String = "Barcode\UPCA\Scs\Suffix Char"
    Private Const UPCE_SUFFIX_KEY As String = "Barcode\UPCE\Scs\Suffix Char"
    Private Const EAN8_SUFFIX_KEY As String = "Barcode\EAN8\Scs\Suffix Char"
    Private Const EAN13_SUFFIX_KEY As String = "Barcode\EAN13\Scs\Suffix Char"

    Private Const UPCA_TRANSMIT_CHECK_DIGIT_KEY = "Barcode\UPCA\ICSP\Transmit Check Digit"
    Private Const UPCE_TRANSMIT_CHECK_DIGIT_KEY = "Barcode\UPCE\ICSP\Transmit Check Digit"
    Private Const EAN8_TRANSMIT_CHECK_DIGIT_KEY = "Barcode\EAN8\ICSP\Transmit Check Digit"
    Private Const EAN13_TRANSMIT_CHECK_DIGIT_KEY = "Barcode\EAN13\ICSP\Transmit Check Digit"

    Public Shared Function IsNSC2(ByVal str As String)
        Return New Regex("^02[0-9]{10}$").Match(str).Success
    End Function

    Public Shared Function ConvertNSC2(ByVal upc As String) As String
        Dim plu As String = upc.Substring(0, 7)
        upc = plu & "00000"
        Return upc
    End Function


    Private Sub HandHeldDataCollect_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
        Handles MyBase.Closing
        Me.Hide()
        e.Cancel = False
    End Sub

    Private Sub HandHeldDataCollect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AppPath()
        Try
            Scanner1.Driver = ScannerServicesDriver1
            ' This is one of the pesky images that appears when an item is scanned.  It was preventing the max error messagebox from displaying
            ' and freezing the application when we hit max items 
            If CInt(ScannerServicesDriver1.GetProperty("Scs\Scan Result")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Scs\Scan Result", 0)
                UpdateScanResult = True
            End If
            ScannerServicesDriver1.ApplySettingChanges()

            SetScannerSettings()


        Catch ex As Exception
            'MsgBox("Scanner Settings Error: " & ex.Message, MsgBoxStyle.Information, "Scanner Settings Error")
        End Try
        'Label4.Text = ListBox1.Items.Count & " Item(s)"
    End Sub

    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        Me.Hide()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) Then
            incomingUPC = TextBox1.Text
            If IsNSC2(incomingUPC) Then
                incomingUPC = ConvertNSC2(incomingUPC)
            End If

            ListBox1.Items.Insert(0, incomingUPC)
            ListBox1.SelectedItem = incomingUPC

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
                    MsgBox("Network Error. Please contact nexussupport@wholefoods.com " & ex.Message, _
                           MsgBoxStyle.Exclamation)
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
        If (String.IsNullOrEmpty(Module1.storeName)) Then
            MsgBox("Store cannot be null or empty", MsgBoxStyle.Exclamation, "Invalid Store")
            Return
        End If

        If (String.IsNullOrEmpty(Module1.regionID)) Then
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

    Private Sub MenuItem_ListParameters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles MenuItem_ListParameters.Click
        DefineReport.Show()
    End Sub

    Public Shared Function IsNumeric(ByVal str As String)
        Return New Regex("^(([0-9]*[1-9][0-9]*([0-9]+)?)|([0]+[0-9]*[1-9][0-9]*))$").Match(str).Success
    End Function

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _
        Handles TextBox1.KeyDown
        If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) And e.KeyCode = Keys.Enter Then
            incomingUPC = TextBox1.Text
            If IsNSC2(incomingUPC) Then
                incomingUPC = ConvertNSC2(incomingUPC)
            End If

            ListBox1.Items.Insert(0, incomingUPC)
            ListBox1.SelectedItem = incomingUPC

            Me.TextBox1.Text = ""
            Me.TextBox1.Focus()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text.EndsWith(vbCr) Then
            Me.TextBox1.Text = Me.TextBox1.Text.Replace(vbCr, "")
            If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) Then
                incomingUPC = TextBox1.Text
                If IsNSC2(incomingUPC) Then
                    incomingUPC = ConvertNSC2(incomingUPC)
                End If
                ' If ListBox1.Items.Contains(incomingUPC) = False Then
                ListBox1.Items.Insert(0, incomingUPC)
                ListBox1.SelectedItem = incomingUPC
                ' End If
                Me.TextBox1.Text = ""
                Me.TextBox1.Focus()
            End If
        End If
    End Sub

    Private Sub Scanner1_ScanCompleteEvent(ByVal sender As Object, _
                                           ByVal e As PsionTeklogix.Barcode.ScanCompleteEventArgs) _
        Handles Scanner1.ScanCompleteEvent
        If e.Text.EndsWith(vbCr) Then
            TextBox1.Text = e.Text.Replace(vbCr, "")
            If TextBox1.Text <> "" And IsNumeric(TextBox1.Text) Then
                incomingUPC = TextBox1.Text
                If IsNSC2(incomingUPC) Then
                    incomingUPC = ConvertNSC2(incomingUPC)
                End If
                ' If ListBox1.Items.Contains(incomingUPC) = False Then
                ListBox1.Items.Insert(0, incomingUPC)
                ListBox1.SelectedItem = incomingUPC
                ' End If
                'Label4.Text = ListBox1.Items.Count & " Item(s)"
                TextBox1.Text = ""
                Me.TextBox1.Focus()
            End If
        End If
    End Sub

    Private Sub ListBox1_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles ListBox1.SelectedValueChanged
        If ListBox1.Items.Count > MaxItems Then
            If ListBox1.Items.Contains(incomingUPC) = True Then
                ListBox1.Items.Remove(incomingUPC)
            End If
            MsgBox(MaxError, MsgBoxStyle.Exclamation, "Item List Limit")
        End If
    End Sub

    Public Sub SetScannerSettings()
        ResetMomento()
        SuffixEnterKeys()
        TurnOffCheckDigits()
        ScannerServicesDriver1.ApplySettingChanges()
        ScannerServicesDriver1.Dispose()

    End Sub

    Private Sub ResetMomento()
        _undo.Clear()
    End Sub

    Private Sub SuffixEnterKeys()
        AddMomento(UPCA_SUFFIX_KEY, ScannerServicesDriver1.GetProperty(UPCA_SUFFIX_KEY))
        ScannerServicesDriver1.SetProperty(UPCA_SUFFIX_KEY, CInt(&HD))

        AddMomento(UPCE_SUFFIX_KEY, ScannerServicesDriver1.GetProperty(UPCE_SUFFIX_KEY))
        ScannerServicesDriver1.SetProperty(UPCE_SUFFIX_KEY, CInt(&HD))

        AddMomento(EAN8_SUFFIX_KEY, ScannerServicesDriver1.GetProperty(EAN8_SUFFIX_KEY))
        ScannerServicesDriver1.SetProperty(EAN8_SUFFIX_KEY, CInt(&HD))

        AddMomento(EAN13_SUFFIX_KEY, ScannerServicesDriver1.GetProperty(EAN13_SUFFIX_KEY))
        ScannerServicesDriver1.SetProperty(EAN13_SUFFIX_KEY, CInt(&HD))
    End Sub

    Private Sub AddMomento(ByVal key As String, ByVal value As Integer)
        _undo.Add(key, value)
    End Sub


    Private Sub TurnOffCheckDigits()
        AddMomento(UPCA_TRANSMIT_CHECK_DIGIT_KEY, ScannerServicesDriver1.GetProperty(UPCA_TRANSMIT_CHECK_DIGIT_KEY))
        ScannerServicesDriver1.SetProperty(UPCA_TRANSMIT_CHECK_DIGIT_KEY, 0)

        AddMomento(UPCE_TRANSMIT_CHECK_DIGIT_KEY, ScannerServicesDriver1.GetProperty(UPCE_TRANSMIT_CHECK_DIGIT_KEY))
        ScannerServicesDriver1.SetProperty(UPCE_TRANSMIT_CHECK_DIGIT_KEY, 0)

        AddMomento(EAN8_TRANSMIT_CHECK_DIGIT_KEY, ScannerServicesDriver1.GetProperty(EAN8_TRANSMIT_CHECK_DIGIT_KEY))
        ScannerServicesDriver1.SetProperty(EAN8_TRANSMIT_CHECK_DIGIT_KEY, 0)

        AddMomento(EAN13_TRANSMIT_CHECK_DIGIT_KEY, ScannerServicesDriver1.GetProperty(EAN13_TRANSMIT_CHECK_DIGIT_KEY))
        ScannerServicesDriver1.SetProperty(EAN13_TRANSMIT_CHECK_DIGIT_KEY, 0)
    End Sub


    Public Sub Undo()
        If (_undo Is Nothing Or _undo.Count() = 0) Then
            Return
        End If

        Dim pair As KeyValuePair(Of String, Integer)
        For Each pair In _undo
            ScannerServicesDriver1.SetProperty(pair.Key, pair.Value)
        Next
        ScannerServicesDriver1.ApplySettingChanges()
    End Sub
End Class


