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
Imports Microsoft.WindowsCE.Forms
Imports System.Collections.Specialized
Imports System.Reflection

Public Class DefineReport
    Private WithEvents splashScreen As SplashScreenForm
    Public RegionCode As String
    Public ServiceURI As String
    Public ForceClose As Boolean = vbFalse
    'Dim regionsDataSet As New DataSet
    Dim MySettingsDataSet As New DataSet
    Dim fs As IO.FileStream
    ' Dim regionID As String
    Dim regionName As String
    Dim myRegion As String
    Dim myStore As String
    'Dim myStoreName As String
    Dim UsingDefaultStore As Boolean
    ' Dim RegionsFile As String
    Dim MySettingsFile As String
    Private Sub DefineReport_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not ForceClose Then
            If MsgBox("Exit OOS ?", MsgBoxStyle.OkCancel, "Exit?") = MsgBoxResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub
    Private Sub DefineReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Enabled = True
        Me.Visible = True
        Me.Text = String.Format("OOS {0}", AppVersion)
        splashScreen.Close()
    End Sub
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        AppPath()
        ' get assembly information
        AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString()

        ' create and display the splash screen form and make the main form it's owner
        splashScreen = New SplashScreenForm()
        splashScreen.Owner = Me
        splashScreen.Show()
        Application.DoEvents()
        splashScreen.Invalidate()


        splashScreen.UpdateStatus("Intializing", 100, 10)

        ' process the message queue - this is done to allow the splash screen to be painted

        ' disable the main form to avoid the title bar being drawn over the splash screen during initialization
        Me.Enabled = False


        Dim result As DialogResult = DialogResult.OK
        splashScreen.UpdateStatus("Verifying Network Connectivity", 100, 20)
        If Not TcpSocketTest() Then
            Dim noNetworkForm As NoNetwork = New NoNetwork(Me)
            ' noNetworkForm.Parent = Me
            result = noNetworkForm.ShowDialog()
            noNetworkForm.Dispose()
        End If

        If result = DialogResult.Abort Then
            Application.[Exit]()
        Else




            splashScreen.UpdateStatus("Loading Settings", 100, 30)
            MySettingsFile = OOSAppPath & "\MySettings.xml"

            If File.Exists(MySettingsFile) Then
                Try
                    GetMySettings()
                Catch ex As Exception
                    ' Already disposed
                End Try
            Else
                MsgBox("Missing File: MySettings.xml  Please contact nexussupport@wholefoods.com.", MsgBoxStyle.Exclamation, "Error.")
                Me.Close()
                Me.Dispose()
            End If
        End If

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        LoadScanForm()
    End Sub
    Private Sub MenuItem_About_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_About.Click
        Dim iam As String

        Dim x As Notification = New Notification()

        x.Caption = "test"
        x.Critical = False

        Dim html As String = "<html><body>test</body></html>"

        x.Text = html

        x.InitialDuration = 20
        x.Visible = True






        iam = System.Reflection.Assembly.GetExecutingAssembly.FullName
        MsgBox(iam, MsgBoxStyle.Information, "OOS")
    End Sub
    Private Sub MenuItem_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_Exit.Click
        If MsgBox("Exit OOS ?", MsgBoxStyle.OkCancel, "Exit?") = MsgBoxResult.Ok Then
            Application.[Exit]()
            Me.Dispose()
            Me.Close()
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        RefreshStores()
    End Sub

    Private Sub RefreshStores()
        Button2.Enabled = False
        Try
            ComboBox1.DataSource = QueryStoreAbbreviationsFor(ComboBox2.SelectedItem)
        Catch ex As Exception
            ' do nothing - no logging on the client
        End Try
        Button2.Enabled = True
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
            MsgBox(" Please contact nexussupport@wholefoods.com. Unable to Open File: " & MySettingsFile & vbCrLf & err.Message, MsgBoxStyle.Exclamation, "Error.")
            Me.Close()
            Me.Dispose()
        End Try

        Try
            MySettingsDataSet.ReadXml(fs)
        Catch ex As Exception
            MsgBox(" Please contact nexussupport@wholefoods.com. Unable to Read: " & MySettingsFile & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, "Error.")
            Me.Close()
            Me.Dispose()
        Finally
            fs.Close()
        End Try
        splashScreen.UpdateStatus("Setting Defaults", 100, 40)
        For Each DataRow As DataRow In MySettingsDataSet.Tables(0).Rows
            myRegion = DataRow("MyRegion").ToString
            myStore = DataRow("MyStore").ToString
            MaxItems = Convert.ToInt32(DataRow("MaxItems").ToString)
            WcfAddress = DataRow("OOS").ToString
            ScanAddress = DataRow("Scan").ToString


            Try
                Environment = DataRow("Environment").ToString
            Catch ex As Exception
                Environment = String.Empty
            End Try

            ' Ignore if this setting is not present
            Try
                oosUrlSandbox = DataRow("OOSSandbox").ToString
            Catch ex As Exception
                oosUrlSandbox = Nothing
            End Try

        Next
        splashScreen.UpdateStatus("Loading Regions", 100, 75)
        Try
            ComboBox2.DataSource = QueryRegionAbbreviations()
        Catch ex As Exception
            If myStore = "" Then
                MsgBox("OOS is currently not available.  Please contact nexussupport@wholefoods.com.  Error is: " & ex.Message, MsgBoxStyle.Exclamation)
                Me.Close()
                Me.Dispose()
            Else
                ComboBox2.Items.Add(Trim(myRegion))
                ComboBox2.SelectedIndex = 0
            End If
        End Try

        If myRegion <> "" Then
            Dim x As Integer
            x = 0
            For Each item As Object In ComboBox2.Items
                If (item.ToString = Trim(myRegion)) Then
                    ComboBox2.SelectedIndex = x
                End If
                x = x + 1
            Next
            Module1.regionID = Trim(myRegion)
        End If
        If myStore <> "" Then
            ComboBox1.Items.Add(Trim(myStore))
            ComboBox1.SelectedIndex = 0
            Module1.storeBUandName = Trim(myStore)
            UsingDefaultStore = True
            Module1.storeName = Trim(myStore)
        Else
            UsingDefaultStore = False
        End If
        Return True
    End Function

    Function QueryRegionAbbreviations() As String()
        Dim abbreviations As String()
        Using oos As New OOS.ScanOutOfStock.ScanOutOfStock
            With oos
                .Url = ScanAddress
                abbreviations = .RegionAbbreviations()
            End With
        End Using
        Return abbreviations
    End Function
    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            LoadScanForm()
        End If
    End Sub
    Private Sub LoadScanForm()
        If String.IsNullOrEmpty(ComboBox2.SelectedItem) Then
            MsgBox("Select a Region", MsgBoxStyle.Exclamation, "Region Required.")
        ElseIf String.IsNullOrEmpty(ComboBox1.SelectedItem) Then
            MsgBox("Select a Store", MsgBoxStyle.Exclamation, "Store Required.")
        Else
            Dim myXmlTextWriter As XmlTextWriter = New XmlTextWriter(MySettingsFile, System.Text.Encoding.UTF8)
            Try
                Button1.Enabled = False
                Module1.regionID = Trim(Split(ComboBox2.SelectedItem, "|")(0))
                If UsingDefaultStore = False Then
                    Module1.storeName = ComboBox1.SelectedItem
                End If
                myXmlTextWriter.Formatting = System.Xml.Formatting.Indented
                myXmlTextWriter.WriteStartDocument(False)
                'Create the main document element ...
                myXmlTextWriter.WriteStartElement("MySettings")
                myXmlTextWriter.WriteStartElement("MyRegion")
                myXmlTextWriter.WriteString(ComboBox2.SelectedItem)
                myXmlTextWriter.WriteEndElement()
                myXmlTextWriter.WriteStartElement("MyStore")
                myXmlTextWriter.WriteString(storeName)
                myXmlTextWriter.WriteEndElement()
                myXmlTextWriter.WriteStartElement("MaxItems")
                myXmlTextWriter.WriteString(MaxItems)
                myXmlTextWriter.WriteEndElement()
                myXmlTextWriter.WriteStartElement("Scan")
                myXmlTextWriter.WriteString(ScanAddress)
                myXmlTextWriter.WriteEndElement()
                myXmlTextWriter.WriteStartElement("OOS")
                myXmlTextWriter.WriteString(WcfAddress)
                myXmlTextWriter.WriteEndElement()

                If (Not String.IsNullOrEmpty(oosUrlSandbox)) Then
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
                HandHeldDataCollect.Label2.Text = "(" & Module1.regionID & ")" & " " & Module1.storeName
                HandHeldDataCollect.Show()
                Button1.Enabled = True
            Catch ex As Exception
                MsgBox("error starting application " + ex.Message)
            End Try
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        RefreshStores()
    End Sub
End Class