Imports System
Imports System.IO
'Imports System.Net
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports Symbol
Imports Symbol.Barcode
Imports PsionTeklogix.Barcode
Imports Scanner.quiver_tst
Imports Scanner.OOSServiceClient
'Imports System.Media.SystemSounds

Public Class HandHeldDataCollect
    Public Class vimProduct
        Public UPC As String
        Public Brand As String
        Public BrandName As String
        Public ProductName As String
        Public LotCode As String
        Public BestByDate As String
    End Class
    Dim UploadSucceeded As Boolean

    Friend WithEvents ScannerServicesDriver1 As New PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver
    Friend WithEvents Scanner1 As New PsionTeklogix.Barcode.Scanner

    Friend WithEvents MyReader As Symbol.Barcode.Reader = Nothing
    Private MyReaderData As Symbol.Barcode.ReaderData = Nothing


    Dim upcCount As Integer = 0
    Dim incomingUPC As String
    Dim mySQLDate As String
    Dim AllowNewItems As Boolean
    Dim MaxError As String = "You have reached the maximum number of items allowed in an Item List: " & MaxItems & vbNewLine & vbNewLine & "Please upload your Item List."
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' MsgBox("A")

        AppPath()





        Try
            Scanner1.Driver = ScannerServicesDriver1

            If CInt(ScannerServicesDriver1.GetProperty("Barcode\UPCA\ICSP\Transmit Check Digit")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Barcode\UPCA\ICSP\Transmit Check Digit", 0)
                UpdateUPCATransmitCheck = True
            End If
            If CInt(ScannerServicesDriver1.GetProperty("Barcode\UPCE\ICSP\Transmit as UPC-A")) <> 1 Then
                ScannerServicesDriver1.SetProperty("Barcode\UPCE\ICSP\Transmit as UPC-A", 1)
                UpdateUPCE = True
            End If
            If CInt(ScannerServicesDriver1.GetProperty("Barcode\UPCE\ICSP\Transmit Check Digit")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Barcode\UPCE\ICSP\Transmit Check Digit", 0)
                UpdateUPCETransmitCheck = True
            End If
            If CInt(ScannerServicesDriver1.GetProperty("Barcode\EAN8\ICSP\Transmit Check Digit")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Barcode\EAN8\ICSP\Transmit Check Digit", 0)
                UpdateEAN8TransmitCheck = True
            End If
            If CInt(ScannerServicesDriver1.GetProperty("Barcode\EAN13\ICSP\Transmit Check Digit")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Barcode\EAN13\ICSP\Transmit Check Digit", 0)
                UpdateEAN13TransmitCheck = True
            End If
            If CInt(ScannerServicesDriver1.GetProperty("Barcode\UPCA\Scs\Strip Trailing")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Barcode\UPCA\Scs\Strip Trailing", 0)
                UpdateUPCAStripTrailing = True
            End If
            If CInt(ScannerServicesDriver1.GetProperty("Barcode\UPCE\Scs\Strip Trailing")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Barcode\UPCE\Scs\Strip Trailing", 0)
                UpdateUPCEStripTrailing = True
            End If
            If CInt(ScannerServicesDriver1.GetProperty("Barcode\EAN8\Scs\Strip Trailing")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Barcode\EAN8\Scs\Strip Trailing", 0)
                UpdateEAN8StripTrailing = True
            End If
            If CInt(ScannerServicesDriver1.GetProperty("Barcode\EAN13\Scs\Strip Trailing")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Barcode\EAN13\Scs\Strip Trailing", 0)
                UpdateEAN13StripTrailing = True
            End If
            If CInt(ScannerServicesDriver1.GetProperty("Barcode\UPC_EAN\ICSP\GTIN Compliant")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Barcode\UPC_EAN\ICSP\GTIN Compliant", 0)
                UpdateGTCompliant = True
            End If

            ' This is one of the pesky images that appears when an item is scanned.  It was preventing the max error messagebox from displaying
            ' and freezing the application when we hit max items 

            If CInt(ScannerServicesDriver1.GetProperty("Scs\Scan Result")) <> 0 Then
                ScannerServicesDriver1.SetProperty("Scs\Scan Result", 0)
                UpdateScanResult = True
            End If


            If CInt(ScannerServicesDriver1.GetProperty("Barcode\EAN13\Scs\Suffix Char")) <> 0 OrElse CInt(ScannerServicesDriver1.GetProperty("Barcode\EAN8\Scs\Suffix Char")) <> 0 OrElse CInt(ScannerServicesDriver1.GetProperty("Barcode\C39\Scs\Suffix Char")) <> 0 OrElse CInt(ScannerServicesDriver1.GetProperty("Barcode\C128\Scs\Suffix Char")) <> 0 OrElse CInt(ScannerServicesDriver1.GetProperty("Barcode\UPCA\Scs\Suffix Char")) <> 0 OrElse CInt(ScannerServicesDriver1.GetProperty("Barcode\UPCE\Scs\Suffix Char")) <> 0 Then

                UpdateEnterKeys = True

                ScannerServicesDriver1.SetProperty("Barcode\UPCA\Scs\Suffix Char", 0)
                ScannerServicesDriver1.SetProperty("Barcode\UPCE\Scs\Suffix Char", 0)
                ScannerServicesDriver1.SetProperty("Barcode\EAN8\Scs\Suffix Char", 0)
                ScannerServicesDriver1.SetProperty("Barcode\EAN13\Scs\Suffix Char", 0)
                ScannerServicesDriver1.SetProperty("Barcode\C39\Scs\Suffix Char", 0)

                ScannerServicesDriver1.SetProperty("Barcode\C128\Scs\Suffix Char", 0)
            Else
                UpdateEnterKeys = False
            End If


            ScannerServicesDriver1.ApplySettingChanges()



        Catch ex As Exception
            Try
                MyReader = New Symbol.Barcode.Reader
                MyReaderData = New Symbol.Barcode.ReaderData(Symbol.Barcode.ReaderDataTypes.Text, 7905)
                MyReader.Actions.Enable()
                MyReader.Decoders.UPCE0.ConvertToUPCA = True
                MyReader.Decoders.UPCE1.ConvertToUPCA = True
                MyReader.Decoders.UPCA.ReportCheckDigit = False
                AddHandler MyReader.ReadNotify, AddressOf Reader_ReadNotify
                ' specify event handler for reading barcode data
                ' this call activates the scanner's scanning ability
                MyReader.Actions.Read(MyReaderData)

            Catch ex2 As Exception


                MsgBox("Scanner Init Error: " & ex2.Message, MsgBoxStyle.Information, "Scanner Settings Error")
            End Try

            MsgBox("Scanner Settings Error: " & ex.Message, MsgBoxStyle.Information, "Scanner Settings Error")


        End Try


        'Label4.Text = ListBox1.Items.Count & " Item(s)"




    End Sub
    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click


        Me.Close()


    End Sub




    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click



        If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) Then

            incomingUPC = Me.TextBox1.Text.ToString.PadLeft(12, "0"c)


            If IsNSC2(incomingUPC) Then

                incomingUPC = ConvertNSC2(incomingUPC)

            End If


            '    If ListBox1.Items.Contains(incomingUPC) = False Then

            ListBox1.Items.Insert(0, incomingUPC)
            ListBox1.SelectedItem = incomingUPC


            ' End If

            Me.TextBox1.Text = ""

            upcLength = 0

            Me.TextBox1.Focus()


        End If


        'Label4.Text = ListBox1.Items.Count & " Item(s)"


    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click


        If ListBox1.SelectedItem = True Then

            ListBox1.Items.Remove(ListBox1.SelectedItem)

        End If


        'Label4.Text = ListBox1.Items.Count & " Item(s)"

        Me.TextBox1.Focus()

    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If MsgBox("Upload the Item List?", MsgBoxStyle.OkCancel, "Upload?") = MsgBoxResult.Ok Then
            If ListBox1.Items.Count = 0 Then
                MsgBox("Nothing to Upload!", MsgBoxStyle.Information, "Empty Item List")
            Else
                Try
                    Dim upcs As New List(Of String)
                    For i As Integer = 0 To ListBox1.Items.Count - 1
                        upcs.Add(ListBox1.Items(i).ToString.PadLeft(13, "0"c))
                    Next
                    Dim oos As New Scanner.ScanOutOfStock.ScanOutOfStock
                    Dim returncode As String
                    returncode = oos.CreateEventsFor(Module1.storeName, upcs.ToArray())
                    'MsgBox(returncode)
                    If returncode = "True" Then
                        ListBox1.Items.Clear()
                        upcCount = 0
                        Button2.Enabled = True
                        Me.Hide()
                    Else
                        MsgBox("Server Error. Please contact WFM Nexus Support  ")
                    End If
                    oos.Dispose()

                Catch ex As System.Exception
                    MsgBox("Network Error. Please contact WFM Nexus Support " & ex.Message, MsgBoxStyle.Exclamation)
                End Try
            End If
        End If
        Me.TextBox1.Focus()
    End Sub
    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        DefineReport.Show()
    End Sub
    Public Shared Function IsNumeric(ByVal str As String)

        Return New Regex("^(([0-9]*[1-9][0-9]*([0-9]+)?)|([0]+[0-9]*[1-9][0-9]*))$").Match(str).Success

    End Function
    Public Shared Function IsNSC2(ByVal str As String)

        Return New Regex("^02[0-9]{10}$").Match(str).Success

    End Function
    Public Shared Function ConvertNSC2(ByVal upc As String) As String
        Dim plu As String = upc.Substring(0, 7)
        upc = plu & "00000"
        Return upc
    End Function
    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) And e.KeyCode = Keys.Enter Then
            incomingUPC = Me.TextBox1.Text.ToString.PadLeft(13, "0"c)
            If IsNSC2(incomingUPC) Then
                incomingUPC = ConvertNSC2(incomingUPC)
            End If
            'If ListBox1.Items.Contains(incomingUPC) = False Then
            ListBox1.Items.Insert(0, incomingUPC)
            ListBox1.SelectedItem = incomingUPC
            'Label4.Text = ListBox1.Items.Count & " Item(s)"
            'End If
            Me.TextBox1.Text = ""
            Me.TextBox1.Focus()
        End If
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text.EndsWith(vbNewLine) Then
            Me.TextBox1.Text = Me.TextBox1.Text.Replace(vbNewLine, "")
            If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) Then
                incomingUPC = Me.TextBox1.Text.ToString.PadLeft(13, "0"c)
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









    Private Sub Scanner1_ScanCompleteEvent(ByVal sender As Object, ByVal e As PsionTeklogix.Barcode.ScanCompleteEventArgs) Handles Scanner1.ScanCompleteEvent





        If e.Text <> "" And IsNumeric(e.Text) Then



            incomingUPC = e.Text.ToString.PadLeft(13, "0"c)


            If IsNSC2(incomingUPC) Then

                incomingUPC = ConvertNSC2(incomingUPC)

            End If



            ' If ListBox1.Items.Contains(incomingUPC) = False Then


            ListBox1.Items.Insert(0, incomingUPC)
            ListBox1.SelectedItem = incomingUPC


            ' End If


            'Label4.Text = ListBox1.Items.Count & " Item(s)"




            Me.TextBox1.Focus()



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




    Private Sub Reader_ReadNotify(ByVal sender As Object, ByVal e As EventArgs)

        Dim nextReaderData As Symbol.Barcode.ReaderData = MyReader.GetNextReaderData()

        Me.TextBox1.Text = nextReaderData.Text


        If nextReaderData.Text <> "" And IsNumeric(nextReaderData.Text) Then

            'incomingUPC = Me.TextBox1.Text.ToString.Substring(0, Me.TextBox1.Text.ToString.Length - 1)

            incomingUPC = nextReaderData.Text.ToString.PadLeft(12, "0"c)


            If IsNSC2(incomingUPC) Then

                incomingUPC = ConvertNSC2(incomingUPC)

            End If



            ' If ListBox1.Items.Contains(incomingUPC) = False Then


            ListBox1.Items.Insert(0, incomingUPC)
            ListBox1.SelectedItem = incomingUPC


            ' End If


            ' Label4.Text = ListBox1.Items.Count & " Item(s)"


            Me.TextBox1.Text = ""

            Me.TextBox1.Focus()



        End If



        MyReader.Actions.Read(MyReaderData)

    End Sub




    Private Sub Label1_ParentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.ParentChanged

    End Sub
End Class



