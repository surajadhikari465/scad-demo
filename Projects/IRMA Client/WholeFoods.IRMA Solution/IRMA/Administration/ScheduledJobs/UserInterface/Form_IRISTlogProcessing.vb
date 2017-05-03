Imports WholeFoods.Utility
Imports WholeFoods.Utility.FTP
Imports WholeFoods.IRMA.Replenishment.TLog
Imports System.IO



Public Class Form_IRISTlogProcessing
    Private _currentStore As StoreFtpInfo
    Private Sub Form_TlogProcessing_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        LoadStores()
    End Sub



    Private Sub LoadStores()

        Dim factory As DataAccess.DataFactory
        factory = New DataAccess.DataFactory(DataAccess.DataFactory.ItemCatalog)
        Dim ds As DataSet = factory.GetStoredProcedureDataSet("GetStoreTlogFTPInfo")


        For Each dr As DataRow In ds.Tables(0).Rows
            Dim store As StoreFtpInfo = New StoreFtpInfo
            store.StoreName = dr("Store_Name").ToString
            store.StoreNo = CInt(dr("Store_No"))
            store.FtpUser = dr("TlogFtpUser").ToString
            store.FtpPass = dr("TlogFtpPassword").ToString
            store.FtpIP = dr("TlogIpAddress").ToString
            store.FtpDirectory = dr("TlogFtpDirectory").ToString
            If CInt(dr("IsSecureTransfer")) = 0 Then store.IsSecure = False Else store.IsSecure = True

            ComboBox_Stores.Items.Add(store)
        Next
        ComboBox_Stores.Items.Insert(0, "All Stores")

        ComboBox_Stores.DisplayMember = "StoreName"
        ComboBox_Stores.ValueMember = "StoreNo"


    End Sub

    Private Sub ProcessTlogs()


        If ComboBox_Stores.SelectedItem Is Nothing Then
            MessageBox.Show("You must select a store.", "Error")
            Exit Sub
        End If

        If ComboBox_Stores.SelectedItem.ToString <> "All Stores" Then
            If ListBox_Files.SelectedItem Is Nothing Then
                MessageBox.Show("You must select a file to process.", "Error")
                Exit Sub
            End If
        End If

        Dim store As StoreFtpInfo = Nothing
        Dim oParser As TlogParserInterface
        Dim TlogFile As String = GetTlogFilenameFromDate(DateTime_FileDate.Value)
        Dim memBuffer As MemoryStream = Nothing
        Dim ftpClient As FTP.FTPclient = Nothing


        oParser = getparser()


        Try
            LockControls()
            Cursor = Cursors.AppStarting
            AddHandler oParser.Notfiy, AddressOf Parser_Notify
            AddHandler oParser.UpdateProgress, AddressOf Parser_UpdateProgress
            AddHandler oParser.Start, AddressOf Parser_Start
            AddHandler oParser.Finished, AddressOf Parser_Finished
            AddHandler oParser.Failure, AddressOf Parser_Failure


            If ComboBox_Stores.SelectedItem.ToString <> "All Stores" Then
                store = CType(ComboBox_Stores.SelectedItem, StoreFtpInfo)
                _currentStore = store
                For Each item As String In ListBox_Files.SelectedItems
                    ftpClient = New FTP.FTPclient(store.FtpIP, store.FtpUser, store.FtpPass, store.IsSecure)
                    ftpClient.Download(store.FtpDirectory & item, memBuffer, True)
                    oParser.ParseDataFromMemoryStream(memBuffer.ToArray)
                    memBuffer.Dispose()
                    ftpClient = Nothing
                Next
            Else
                For Each item As Object In ComboBox_Stores.Items
                    If TypeOf item Is StoreFtpInfo Then
                        Try
                            store = CType(item, StoreFtpInfo)
                            _currentStore = store
                            ftpClient = New FTP.FTPclient(store.FtpIP, store.FtpUser, store.FtpPass, store.IsSecure)
                            ftpClient.Download(store.FtpDirectory & TlogFile, memBuffer, True)
                            oParser.ParseDataFromMemoryStream(memBuffer.ToArray)
                            memBuffer.Dispose()
                        Catch ExD As Exception
                            If ExD.Message.Contains("550") Then
                                WriteToLog("Could not find " & TlogFile & " for " & store.StoreName, True)
                            Else
                                Throw
                            End If
                        Finally
                            If Not ftpClient Is Nothing Then
                                ftpClient = Nothing
                            End If
                        End Try
                    End If
                Next

            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            RemoveHandler oParser.Notfiy, AddressOf Parser_Notify
            RemoveHandler oParser.UpdateProgress, AddressOf Parser_UpdateProgress
            RemoveHandler oParser.Start, AddressOf Parser_Start
            RemoveHandler oParser.Finished, AddressOf Parser_Finished
            RemoveHandler oParser.Failure, AddressOf Parser_Failure
            Cursor = Cursors.Default
            oParser.Dispose()
            UnLockControls()

        End Try



    End Sub

    Private Function GetTlogFilenameFromDate(ByVal d As DateTime) As String
        Dim retval As String
        retval = "EJ"
        retval += d.Year.ToString.Substring(2, 2)
        If d.Month.ToString.Length < 2 Then
            retval += "0" & d.Month.ToString
        Else
            retval += d.Month.ToString
        End If
        If d.Day.ToString.Length < 2 Then
            retval += "0" & d.Day.ToString
        Else
            retval += d.Day.ToString
        End If

        retval += ".LOG"

        Return retval
    End Function
    Private Sub LockControls()
        Button_Process.Enabled = False
        ComboBox_Stores.Enabled = False
    End Sub
    Private Sub UnLockControls()
        Button_Process.Enabled = True
        ComboBox_Stores.Enabled = True
    End Sub
    Private Function getparser() As TlogParserInterface
        Return CType(New Parser(), TlogParserInterface) ' UK Parser.
    End Function

    Private Sub Parser_Notify(ByVal msg As String)
        WriteToLog(msg, True)
    End Sub
    Private Sub Parser_Start()
        Cursor = Cursors.WaitCursor
        WriteToLog("=============================================")
        WriteToLog("Processing Tlog for: " & _currentStore.StoreName, True)
        Label_Msg.Text = ""
        Application.DoEvents()
    End Sub
    Private Sub Parser_Finished()
        Cursor = Cursors.Default
        WriteToLog("Finished Processing.", True)
        ' Label_Msg.Text = "Finished Processing."
    End Sub
    Private Sub Parser_Failure(ByVal Errmsg As String, ByVal InnerMsg As String)
        'MsgBox("ERROR: " & Errmsg & vbCrLf & InnerMsg)
        WriteToLog("ERROR: " & Errmsg & vbCrLf & InnerMsg, True)
    End Sub
    Private Sub Parser_UpdateProgress(ByVal value As Integer, ByVal max As Integer)
        ProgressBar_Progress.Maximum = max
        ProgressBar_Progress.Value = value
        ProgressBar_Progress.Refresh()
        Application.DoEvents()
    End Sub
    Private Sub WriteToLog(ByVal msg As String, Optional ByVal IncludeTimestamp As Boolean = False)
        If IncludeTimestamp Then
            Dim d As DateTime = DateTime.Now
            msg = "[" & d.ToShortDateString & " " & d.ToShortTimeString & "] " & msg
        End If
        ListView_Log.Items.Add(msg).EnsureVisible()
    End Sub

    Private Sub Button_Process_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Process.Click
        ProcessTlogs()
    End Sub

    Private Class StoreFtpInfo
        Sub New()

        End Sub


        Private _StoreName As String
        Public Property StoreName() As String
            Get
                Return _StoreName
            End Get
            Set(ByVal value As String)
                _StoreName = value
            End Set
        End Property

        Private _Store_No As Integer
        Public Property StoreNo() As Integer
            Get
                Return _Store_No
            End Get
            Set(ByVal value As Integer)
                _Store_No = value
            End Set
        End Property


        Private _FtpIP As String
        Public Property FtpIP() As String
            Get
                Return _FtpIP
            End Get
            Set(ByVal value As String)
                _FtpIP = value
            End Set
        End Property


        Private _FtpUser As String
        Public Property FtpUser() As String
            Get
                Return _FtpUser
            End Get
            Set(ByVal value As String)
                _FtpUser = value
            End Set
        End Property



        Private _FtpPass As String
        Public Property FtpPass() As String
            Get
                Return _FtpPass
            End Get
            Set(ByVal value As String)
                _FtpPass = value
            End Set
        End Property

        Private _FtpDirectory As String
        Public Property FtpDirectory() As String
            Get
                Return _FtpDirectory
            End Get
            Set(ByVal value As String)
                _FtpDirectory = value
            End Set
        End Property


        Private _IsSecure As Boolean
        Public Property IsSecure() As Boolean
            Get
                Return _IsSecure
            End Get
            Set(ByVal value As Boolean)
                _IsSecure = value
            End Set
        End Property


    End Class

    Private Sub ComboBox_Stores_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Stores.SelectedIndexChanged
        If Not ComboBox_Stores.SelectedItem Is Nothing Then
            If ComboBox_Stores.SelectedItem.ToString <> "All Stores" Then
                DateTime_FileDate.Enabled = False
                ListBox_Files.Enabled = True
                GetListOfFilesFromStore(CType(ComboBox_Stores.SelectedItem, StoreFtpInfo))
            Else
                DateTime_FileDate.Enabled = True
                ListBox_Files.Enabled = False
            End If

        End If
    End Sub

    Private Sub GetListOfFilesFromStore(ByVal store As StoreFtpInfo)

        Dim ftpClient As FTP.FTPclient = New FTPclient(store.FtpIP, store.FtpUser, store.FtpPass, store.IsSecure)
        Dim fileList As List(Of String)
        ListBox_Files.Items.Clear()
        Try
            fileList = ftpClient.ListDirectory(store.FtpDirectory)
            For Each item As String In fileList
                If (item.IndexOf("EJ") = 0) And (item.IndexOf(".LOG") > 0) Then
                    ListBox_Files.Items.Add(item)
                End If
            Next
        Catch ex As Exception
            ListBox_Files.Items.Add("Could not get a list of files")
            Label_Msg.Text = ex.Message
        End Try

    End Sub

    Private Sub SaveLogMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveLogMenuItem.Click
        Dim f As FolderBrowserDialog = New FolderBrowserDialog()
        f.RootFolder = Environment.SpecialFolder.Desktop
        f.ShowDialog()

        Dim fs As TextWriter = New StreamWriter(f.SelectedPath & "\TlogParsing.Log")
        For Each item As ListViewItem In ListView_Log.Items
            fs.WriteLine(item.ToString())
        Next
        fs.Flush()
        fs.Close()

    End Sub

    Private Sub ClearToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearToolStripMenuItem.Click
        ListView_Log.Items.Clear()
    End Sub

    Private Sub Label_Msg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label_Msg.Click

    End Sub
End Class

