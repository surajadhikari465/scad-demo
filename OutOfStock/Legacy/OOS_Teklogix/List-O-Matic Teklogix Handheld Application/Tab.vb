Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml
Imports System.Collections.Specialized
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports Symbol
Imports Symbol.Barcode
Imports Scanner.quiver_tst

Public Class Tab
    Private splashScreen As SplashScreenForm


    Dim regionsDataSet As New DataSet
    Dim MySettingsDataSet As New DataSet
    Dim fs As IO.FileStream
    Dim regionID As String
    Dim regionName As String
    Dim myRegion As String
    Dim myStore As String
    Dim myStoreName As String
    Dim UsingDefaultStore As Boolean
    Dim RegionsFile As String
    Dim MySettingsFile As String
    Dim UploadSucceeded As Boolean
    Private MyReader As Symbol.Barcode.Reader = Nothing
    Private MyReaderData As Symbol.Barcode.ReaderData = Nothing
    Private MyEventHandler As System.EventHandler = Nothing
    Dim upcCount As Integer = 0
    Dim incomingUPC As String
    Dim mySQLDate As String
    Dim AllowNewItems As Boolean
    Dim MaxError As String = "You have reached the maximum number of items allowed in an Item List: " & MaxItems & vbNewLine & vbNewLine & "Please upload your Item List."
    Friend WithEvents ScannerServicesDriver1 As New PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver
    Friend WithEvents Scanner1 As New PsionTeklogix.Barcode.Scanner
    ' NOT USED IN TEKLOGIK
    Private Sub Reader_ReadNotify(ByVal sender As Object, ByVal e As EventArgs)
        Dim nextReaderData As Symbol.Barcode.ReaderData = MyReader.GetNextReaderData()
        Me.TextBox1.Text = nextReaderData.Text
        If nextReaderData.Text <> "" And IsNumeric(nextReaderData.Text) Then
            'incomingUPC = Me.TextBox1.Text.ToString.Substring(0, Me.TextBox1.Text.ToString.Length - 1)
            incomingUPC = nextReaderData.Text.ToString.PadLeft(12, "0"c)
            If IsNSC2(incomingUPC) Then
                incomingUPC = ConvertNSC2(incomingUPC)
            End If
            '  If ListBox1.Items.Contains(incomingUPC) = False Then
            ListBox1.Items.Insert(0, incomingUPC)
            ListBox1.SelectedItem = incomingUPC
            ' End If
            'Label4.Text = ListBox1.Items.Count & " Item(s)"
            Me.TextBox1.Text = ""
            Me.TextBox1.Focus()
        End If
        MyReader.Actions.Read(MyReaderData)
    End Sub

    Public Shared Function ConvertNSC2(ByVal upc As String) As String
        Dim plu As String = upc.Substring(0, 7)
        upc = plu & "00000"
        Return upc
    End Function
    Public Shared Function IsNSC2(ByVal str As String)
        Return New Regex("^02[0-9]{10}$").Match(str).Success
    End Function
    Private Sub Tab_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        MyReader.Actions.Flush()
        MyReader.Actions.Disable()
        MyReader.Actions.Dispose()
        MyReaderData.Dispose()
    End Sub

    Private Sub Tab_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        MsgBox(e.KeyChar)
    End Sub
    Private Sub Tab_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Enabled = True
        Me.Visible = True

        splashScreen.Close()
        ' MsgBox(RegionsFile)
        If File.Exists(RegionsFile) Then
            BuildRegionList()
        Else
            MsgBox("Missing File: Regions.xml", MsgBoxStyle.Exclamation, "Error.")
        End If

        AppPath()
        'MsgBox("start")
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
            MsgBox("Scanner Settings Error: " & ex.Message, MsgBoxStyle.Information, "Scanner Settings Error")
        End Try

        ' If Module1.storeName.Length > 0 Then
        ' TabControl1.SelectedIndex = 1
        ' End If

        'Label4.Text = ListBox1.Items.Count & " Item(s)"
    End Sub
    Function BuildRegionList() As Boolean
        Dim oos As New Scanner.ScanOutOfStock.ScanOutOfStock

        Regions.DataSource = oos.RegionNames()

        oos.Dispose()
    End Function
    Private Function oldlist()

        Try
            fs = New IO.FileStream(RegionsFile, IO.FileMode.Open)
        Catch err As Exception
            MsgBox("Unable to Open File: " & RegionsFile & err.Message, MsgBoxStyle.Exclamation, "Error.")
        End Try
        Try
            regionsDataSet.ReadXml(fs)
        Catch ex As Exception
            MsgBox("Unable to Read: " & RegionsFile & ex.Message, MsgBoxStyle.Exclamation, "Error.")
        Finally
            If System.IO.File.Exists(RegionsFile) = True Then
                fs.Close()
            End If
        End Try
        For Each DataRow As DataRow In regionsDataSet.Tables(0).Rows
            regionID = DataRow("regionid").ToString
            regionName = DataRow("regionname").ToString
            Regions.Items.Add(regionID)
        Next
        Regions.SelectedItem = myRegion
        Return True
    End Function
    Private Sub Regions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Regions.SelectedIndexChanged
        ' Dim vim As New VimLookup()
        Dim mystorelist As New StoreList
        'GenerateStoreList(ComboBox2.SelectedItem)
        'Stores.DataSource = vim.GetAllStoreNamesByRegion(Regions.SelectedItem)
        Dim oos As New Scanner.ScanOutOfStock.ScanOutOfStock

        Stores.DataSource = oos.StoreNamesFor(Regions.SelectedItem)

        oos.Dispose()
    End Sub
    Public Sub New()

        ' create and display the splash screen form and make the main form it's owner
        splashScreen = New SplashScreenForm()
        splashScreen.Owner = Me
        splashScreen.Show()

        ' process the message queue - this is done to allow the splash screen to be painted
        Application.DoEvents()

        ' disable the main form to avoid the title bar being drawn over the splash screen during initialization
        Me.Enabled = False
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        SmoreAppPath = AppPath()
        '  MsgBox(SmoreAppPath)
        MySettingsFile = SmoreAppPath & "\MySettings.xml"
        RegionsFile = SmoreAppPath & "\Regions.xml"
        If File.Exists(MySettingsFile) Then
            GetMySettings()
        Else
            MsgBox("Missing File: MySettings.xml", MsgBoxStyle.Exclamation, "Error.")
        End If
    End Sub
    Function GetMySettings() As Boolean
        Try
            fs = New IO.FileStream(MySettingsFile, IO.FileMode.Open)
        Catch err As Exception
            MsgBox("Unable to Open File: " & MySettingsFile & err.Message, MsgBoxStyle.Exclamation, "Error.")
        End Try
        Try
            MySettingsDataSet.ReadXml(fs)
        Catch ex As Exception
            MsgBox("Unable to Read: " & MySettingsFile & ex.Message, MsgBoxStyle.Exclamation, "Error.")
        Finally
            fs.Close()
        End Try
        For Each DataRow As DataRow In MySettingsDataSet.Tables(0).Rows
            myRegion = DataRow("MyRegion").ToString
            myStore = DataRow("MyStore").ToString
            MaxItems = Convert.ToInt32(DataRow("MaxItems").ToString)
            WcfAddress = DataRow("ServiceAddress").ToString
        Next
        If myStore <> "" Then
            Stores.Items.Add(Trim(myStore))
            Stores.SelectedItem = Trim(myStore)
            Stores.SelectedValue = Trim(myStore)
            Module1.storeBUandName = myStore
            UsingDefaultStore = True
            'Module1.storeName = Trim(Split(myStore, "|")(1))
            'Module1.storeBU = Trim(Split(myStore, "|")(0))
            Module1.storeName = Trim(myStore)
        Else
            UsingDefaultStore = False
        End If
        Return True
    End Function
    Private Sub Stores_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Stores.SelectedIndexChanged
        If Stores.SelectedIndex > 1 Then
            TabControl1.SelectedIndex = 1
        End If

        Module1.storeName = Stores.SelectedValue
        Label3.Text = "(" & Module1.regionID & ")" & " " & Module1.storeName
        Label3.Text = Module1.storeName
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) Then
            incomingUPC = Me.TextBox1.Text.ToString.PadLeft(12, "0"c)
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
    End Sub
    Private Sub ListBox1_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ListBox1.Items.Count > MaxItems Then
            If ListBox1.Items.Contains(incomingUPC) = True Then
                ListBox1.Items.Remove(incomingUPC)
            End If
            MsgBox(MaxError, MsgBoxStyle.Exclamation, "Item List Limit")
        End If
    End Sub
    Private Sub TextBox1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) And e.KeyCode = Keys.Enter Then
            incomingUPC = Me.TextBox1.Text.ToString.PadLeft(12, "0"c)
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
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If TextBox1.Text.EndsWith(vbNewLine) Then
            Me.TextBox1.Text = Me.TextBox1.Text.Replace(vbNewLine, "")
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
                Me.TextBox1.Focus()
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ListBox1.Items.Count > 0 Then
            Try
                Dim upcs As New List(Of String)
                For i As Integer = 0 To ListBox1.Items.Count - 1
                    upcs.Add(ListBox1.Items(i).ToString.PadLeft(13, "0"c))
                Next
                Dim oos As New Scanner.ScanOutOfStock.ScanOutOfStock
                ' Dim returncode As String
                Dim x As Boolean
                x = True
                'returncode = oos.CreateEventsFor(Module1.storeName, upcs.ToArray())
                '  returncode = oos.ScanProducts(Date.Now, x, Module1.storeName, upcs.ToArray())
                ListBox1.Items.Clear()
                upcCount = 0
                Button2.Enabled = True
                oos.Dispose()
            Catch ex As System.Exception
                MsgBox("Network Error. Your data is saved on this device. Please contact nexussupport@wholefoods.com.  Error is: " & ex.Message, MsgBoxStyle.Exclamation)
            End Try
        End If
        Me.TextBox1.Focus()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) Then
            incomingUPC = Me.TextBox1.Text.ToString.PadLeft(12, "0"c)
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
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If Me.TextBox1.Text <> "" And IsNumeric(Me.TextBox1.Text) Then
            incomingUPC = Me.TextBox1.Text.ToString.PadLeft(12, "0"c)
            If IsNSC2(incomingUPC) Then
                incomingUPC = ConvertNSC2(incomingUPC)
            End If
            ' If ListBox1.Items.Contains(incomingUPC) = False Then
            ListBox1.Items.Insert(0, incomingUPC)
            ListBox1.SelectedItem = incomingUPC
            Me.TextBox1.Text = ""
            upcLength = 0
        End If
        TextBox1.Focus()

    End Sub

    Public Shared Function IsNumeric(ByVal str As String)
        Return New Regex("^(([0-9]*[1-9][0-9]*([0-9]+)?)|([0]+[0-9]*[1-9][0-9]*))$").Match(str).Success
    End Function

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If ListBox1.Items.Count > 0 Then
            Try
                Dim upcs As New List(Of String)
                For i As Integer = 0 To ListBox1.Items.Count - 1
                    upcs.Add(ListBox1.Items(i).ToString.PadLeft(13, "0"c))
                Next
                Dim oos As New Scanner.ScanOutOfStock.ScanOutOfStock
                Dim returncode As String
                returncode = oos.CreateEventsFor(Module1.storeName, upcs.ToArray())
                If returncode = "True" Then
                    ListBox1.Items.Clear()
                    upcCount = 0
                    Button2.Enabled = True
                    '   Me.Hide()
                Else
                    '  MsgBox("Server Error. Please contact WFM Nexus Support  ")
                    ListBox1.Items.Clear()
                    upcCount = 0
                    Button2.Enabled = True
                End If
                oos.Dispose()

            Catch ex As System.Exception
                MsgBox("Network Error. Please contact nexussupport@wholefoods.com.  Error is: " & ex.Message, MsgBoxStyle.Exclamation)
            End Try
        End If
        Me.TextBox1.Focus()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    '  CAN NOT BE RIGHTT
    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyValue = 13 Then
            If ListBox1.Items.Count > 0 Then
                Try
                    Dim upcs As New List(Of String)
                    For i As Integer = 0 To ListBox1.Items.Count - 1
                        upcs.Add(ListBox1.Items(i).ToString.PadLeft(13, "0"c))
                    Next
                    Dim oos As New Scanner.ScanOutOfStock.ScanOutOfStock
                    Dim returncode As String
                    returncode = oos.CreateEventsFor(Module1.storeName, upcs.ToArray())
                    If returncode = "True" Then
                        ListBox1.Items.Clear()
                        upcCount = 0
                        Button2.Enabled = True
                        '   Me.Hide()
                    Else
                        'MsgBox("Server Error. Please contact WFM Nexus Support  ")
                        ListBox1.Items.Clear()
                        upcCount = 0
                        Button2.Enabled = True
                    End If
                    oos.Dispose()

                Catch ex As System.Exception
                    MsgBox("Network Error. Please contact nexussupport@wholefoods.com.  Error is: " & ex.Message, MsgBoxStyle.Exclamation)
                End Try
            End If
            Me.TextBox1.Focus()
        End If
    End Sub

    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If ListBox1.SelectedItem = True Then
            ListBox1.Items.Remove(ListBox1.SelectedItem)
        End If
        TextBox1.Focus()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        TextBox1.Focus()
    End Sub

    Private Sub ScanTab_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ScanTab.GotFocus
        If myStore.Length = 0 Then
            TabControl1.SelectedIndex = 1
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
End Class