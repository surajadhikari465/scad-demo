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
Imports System.Net


Public Class DefineReport
    Private WithEvents splashScreen As SplashScreenForm
    Public RegionCode As String
    Public ServiceURI As String
    Public ForceClose As Boolean = vbFalse

    Dim MySettingsDataSet As New DataSet
    Dim fs As IO.FileStream
    Dim regionName As String
    Dim myRegion As String
    Dim myStore As String
    Dim UsingDefaultStore As Boolean
    'Dim RegionsFile As String
    Dim MySettingsFile As String

    Private Sub DefineReport_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
        Handles MyBase.Closing
        If Not ForceClose Then
            If MsgBox("Exit OOS ?", MsgBoxStyle.OkCancel, "Exit?") = MsgBoxResult.Cancel Then
                e.Cancel = True
                Return
            End If
        End If
        HandHeldDataCollect.Undo()
    End Sub

    Private Sub DefineReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Enabled = True
        Me.Visible = True
        Me.Text = String.Format("OOS {0}", AppVersion)
        Label_Version.Text = String.Format("OOS {0} {1}", AppVersion, Environment)
        splashScreen.UpdateStatus("Done", 100, 100)
        splashScreen.Close()

    End Sub

    Public Sub New()
        ' get assembly information
        AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString()

        ' create and display the splash screen form and make the main form it's owner
        splashScreen = New SplashScreenForm()
        splashScreen.Owner = Me
        splashScreen.Show()

        splashScreen.UpdateStatus("Intializing", 100, 10)


        ' process the message queue - this is done to allow the splash screen to be painted
        Application.DoEvents()
        ' disable the main form to avoid the title bar being drawn over the splash screen during initialization
        Me.Enabled = False
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        AppPath()

        Dim result As DialogResult = DialogResult.OK


        splashScreen.UpdateStatus("Verifying Network Connectivity", 100, 20)
        If Not TcpSocketTest() Then
            Dim noNetworkForm As NoNetwork = New NoNetwork(Me)
            ' noNetworkForm.Parent = Me
            result = noNetworkForm.ShowDialog()
            noNetworkForm.Dispose()
        End If
        If result = DialogResult.Abort Then
            Application.Exit()
        Else
            Me.Refresh()
            Application.DoEvents()


            splashScreen.UpdateStatus("Loading Settings", 100, 30)
            MySettingsFile = OOSAppPath & "\MySettings.xml"
            If File.Exists(MySettingsFile) Then
                Try
                    GetMySettings()
                Catch ex As Exception
                    ' Already disposed
                End Try
            Else
                MsgBox("Missing File: MySettings.xml", MsgBoxStyle.Exclamation, "Error.")
            End If
        End If
    End Sub

    Private Sub Button_Continue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles Button_Continue.Click
        LoadScanForm()
    End Sub

    Private Sub MenuItem_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_Menu.Click
        If MsgBox("Exit OOS?", MsgBoxStyle.OkCancel, "Exit?") = MsgBoxResult.Ok Then
            HandHeldDataCollect.Undo()
            Application.[Exit]()

            Me.Dispose()
            Me.Close()
        End If
    End Sub

    Private Sub Button_GetStores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles Button_GetStores.Click
        RefreshStores()
    End Sub

    Private Sub RefreshStores()
        Button_GetStores.Enabled = False
        Try
            ComboBox1.DataSource = QueryStoreAbbreviationsFor(ComboBox_Region.SelectedItem)
        Catch ex As Exception
            ' do nothing - no logging on the client
        End Try
        Button_GetStores.Enabled = True
        UsingDefaultStore = False
    End Sub

    Function QueryStoreAbbreviationsFor(ByVal regionAbbrev) As String()
        Dim abbreviations As String() = Nothing
        Using oos As New OOS.ScanOutOfStock.ScanOutOfStock
            With oos
                .Url = ScanAddress
                abbreviations = .StoreAbbreviationsFor(regionAbbrev)
            End With
        End Using
        Return abbreviations
    End Function

    Function GetMySettings() As Boolean
        Try
            fs = New IO.FileStream(MySettingsFile, IO.FileMode.Open)
        Catch err As Exception
            MsgBox( _
                " Please contact nexussupport@wholefoods.com. Unable to Open File: " & MySettingsFile & vbCrLf & _
                err.Message, MsgBoxStyle.Exclamation, "Error.")
            Me.Close()
            Me.Dispose()
        End Try

        Try
            splashScreen.UpdateStatus("Reading Settings", 100, 30)
            MySettingsDataSet.ReadXml(fs)
        Catch ex As Exception
            MsgBox( _
                " Please contact nexussupport@wholefoods.com. Unable to Read: " & MySettingsFile & vbCrLf & ex.Message, _
                MsgBoxStyle.Exclamation, "Error.")
            Me.Close()
            Me.Dispose()
        Finally
            fs.Close()
        End Try
        splashScreen.UpdateStatus("Setting Defaults", 100, 40)
        For Each dataRow As DataRow In MySettingsDataSet.Tables(0).Rows
            myRegion = dataRow("MyRegion").ToString
            myStore = dataRow("MyStore").ToString
            MaxItems = Convert.ToInt32(dataRow("MaxItems").ToString)
            WcfAddress = dataRow("OOS").ToString
            ScanAddress = dataRow("Scan").ToString


            Try
                Environment = dataRow("Environment").ToString
            Catch ex As Exception
                Environment = String.Empty
            End Try

            ' Ignore if this setting is not present
            Try

                oosUrlSandbox = dataRow("OOSSandbox").ToString
            Catch ex As Exception
                oosUrlSandbox = Nothing
            End Try

        Next
        Try
            Using oos As New OOS.ScanOutOfStock.ScanOutOfStock
                splashScreen.UpdateStatus("Loading Regions", 100, 75)

                With oos
                    .Url = ScanAddress
                    ComboBox_Region.DataSource = .RegionAbbreviations()
                End With
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            If myStore = "" Then
                MsgBox( _
                    "OOS is currently not available.  Please contact nexussupport@wholefoods.com.  Error is: " & _
                    ex.Message, MsgBoxStyle.Exclamation)
                Me.Close()
                Me.Dispose()
            Else
                ComboBox_Region.Items.Add(Trim(myRegion))
                ComboBox_Region.SelectedIndex = 0
            End If
        End Try
        If myRegion <> "" Then
            Dim x As Integer
            x = 0
            For Each item As Object In ComboBox_Region.Items
                If (item.ToString = Trim(myRegion)) Then
                    ComboBox_Region.SelectedIndex = x
                End If
                x = x + 1
            Next
            Module1.regionID = Trim(myRegion)
        End If
        If myStore <> "" Then
            ComboBox1.Items.Add(Trim(myStore))
            ComboBox1.SelectedIndex = 0
            storeBUandName = Trim(myStore)
            UsingDefaultStore = True
            storeName = Trim(myStore)
        Else
            UsingDefaultStore = False
        End If
        Return True
    End Function

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            LoadScanForm()
        End If
    End Sub


    Private Sub LoadScanForm()
        If ComboBox_Region.SelectedItem = String.Empty Or ComboBox_Region.SelectedItem Is Nothing Then
            MsgBox("Select a Region", MsgBoxStyle.Exclamation, "Region Required.")
        ElseIf ComboBox1.SelectedItem = String.Empty Or ComboBox1.SelectedItem Is Nothing Then
            MsgBox("Select a Store", MsgBoxStyle.Exclamation, "Store Required.")
        Else
            Dim myXmlTextWriter As XmlTextWriter = New XmlTextWriter(MySettingsFile, System.Text.Encoding.UTF8)
            Try
                Button_Continue.Enabled = False
                Module1.regionID = Trim(Split(ComboBox_Region.SelectedItem, "|")(0))
                If UsingDefaultStore = False Then
                    Module1.storeName = ComboBox1.SelectedItem
                End If
                myXmlTextWriter.Formatting = System.Xml.Formatting.Indented
                myXmlTextWriter.WriteStartDocument(False)
                'Create the main document element ...
                myXmlTextWriter.WriteStartElement("MySettings")
                myXmlTextWriter.WriteStartElement("MyRegion")
                myXmlTextWriter.WriteString(ComboBox_Region.SelectedItem)
                myXmlTextWriter.WriteEndElement()
                myXmlTextWriter.WriteStartElement("MyStore")
                myXmlTextWriter.WriteString(storeName)
                myXmlTextWriter.WriteEndElement()
                myXmlTextWriter.WriteStartElement("MaxItems")
                myXmlTextWriter.WriteString(MaxItems)
                myXmlTextWriter.WriteEndElement()
                myXmlTextWriter.WriteStartElement("Scan")
                myXmlTextWriter.WriteString(WcfAddress)
                myXmlTextWriter.WriteEndElement()
                myXmlTextWriter.WriteStartElement("OOS")
                myXmlTextWriter.WriteString(WcfAddress)
                myXmlTextWriter.WriteEndElement()

                If (String.IsNullOrEmpty(oosUrlSandbox) = False) Then
                    myXmlTextWriter.WriteStartElement("OOSSandbox")
                    myXmlTextWriter.WriteString(oosUrlSandbox)
                    myXmlTextWriter.WriteEndElement()
                End If
                If (Not String.IsNullOrEmpty(Environment)) Then
                    myXmlTextWriter.WriteStartElement("Environment")
                    myXmlTextWriter.WriteString(Environment)
                    myXmlTextWriter.WriteEndElement()
                End If
                myXmlTextWriter.WriteEndDocument()
                myXmlTextWriter.Flush()
            Catch ex As Exception
                MsgBox("error writing xml file" + ex.Message)
            Finally
                myXmlTextWriter.Close()
            End Try

            Try
                HandHeldDataCollect.Label_Store.Text = "(" & Module1.regionID & ")" & " " & Module1.storeName
                HandHeldDataCollect.Show()
                Button_Continue.Enabled = True
            Catch ex As Exception
                MsgBox("error starting application " + ex.Message)
            End Try
        End If
    End Sub

    Private Sub ComboBox_Region_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles ComboBox_Region.SelectedIndexChanged
        RefreshStores()
    End Sub
End Class