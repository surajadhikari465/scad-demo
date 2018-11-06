Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Tlog
Imports System.Drawing
Imports System.Drawing.Drawing2D


Public Class Form_HouseTlogProcessing
    Private _StoreNo As Integer
    Private _ParseDate As DateTime
    Private _Factory As DataAccess.DataFactory
    Private _StoreInformation As List(Of TlogStoreInfo) = New List(Of WholeFoods.IRMA.Replenishment.TLog.TlogStoreInfo)
    Private _Logs As List(Of String) = New List(Of String)
    Private _BasePath As String = ConfigurationServices.AppSettings("TlogBasePath")
    Private _RequiredFiles As String = ConfigurationServices.AppSettings("RequiredFiles")
    'Private _Region As String = InstanceDataDAO.GetInstanceData.RegionCode
    Private _TlogDBUsername As String = ConfigurationServices.AppSettings("TlogDatabaseUsername")
    Private _TlogDBpassword As String = ConfigurationServices.AppSettings("TlogDatabasePassword")
    Private _TlogProcessUsername As String = ConfigurationServices.AppSettings("TlogProcessUsername")
    Private _TlogProcesspassword As String = ConfigurationServices.AppSettings("TlogProcessPassword")
    Private _TlogDBServer As String = ConfigurationServices.AppSettings("TlogDBServer")
    Private _TlogDatabase As String = ConfigurationServices.AppSettings("TlogDatabase")
    Private _EncryptedConnectionStrings As String = ConfigurationManager.AppSettings("encryptedConnectionStrings")
    Private _TlogLoginInfo As TlogLoginInfo = New WholeFoods.IRMA.Replenishment.TLog.TlogLoginInfo
    Private WithEvents _BackgroundWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker()
    Private _msg As String
    Private UIUpdates As TlogUIUpdate = New TlogUIUpdate

    Private Sub Form_HouseTlogProcessing_Layout(ByVal sender As Object, ByVal e As System.Windows.Forms.LayoutEventArgs) Handles Me.Layout
        ListView_logs.Columns(0).Width = -2
    End Sub

    Private Sub Form_HouseTlogProcessing_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        CreateDatabaseConnection()
        GetStoreInfo()

    End Sub



    Private Sub GetStoreInfo()
        'Dim ds As DataSet = _Factory.GetStoredProcedureDataSet("GetStoreTlogFTPInfo")
        Dim ds As DataSet = _Factory.GetStoredProcedureDataSet("GetStores")

        _StoreInformation.Clear()
        For Each dr As DataRow In ds.Tables(0).Rows
            Dim store As TlogStoreInfo = New TlogStoreInfo
            store.StoreName = dr("Store_Name").ToString
            store.StoreNo = CInt(dr("Store_No"))
            store.StoreAbbr = dr("Storeabbr").ToString
            store.StoreRegion = dr("Region_Code").ToString.Trim
            _StoreInformation.Add(store)
            ComboBox_Stores.Items.Add(store)
        Next

        Dim AllStores As New TlogStoreInfo
        AllStores.StoreNo = -1
        AllStores.StoreName = "All Stores"
        AllStores.StoreAbbr = "All"
        ComboBox_Stores.Items.Insert(0, AllStores)
        ComboBox_Stores.DisplayMember = "StoreName"
        ComboBox_Stores.ValueMember = "StoreNo"
        ComboBox_Stores.SelectedIndex = 0

    End Sub

    Private Sub Button_Parse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Parse.Click
        If DateTime_ParseDate.Value.Date > DateTime.Now.Date Then
            MessageBox.Show("Tlog data files do not exist for dates in the future.", "Warning")
            DateTime_ParseDate.Value = DateTime.Now.Date
        Else
            Try

                CheckLoginInformation()
                ValidateArguments()
                _BackgroundWorker.WorkerReportsProgress = True
                _BackgroundWorker.WorkerSupportsCancellation = True
                Button_Parse.Enabled = False
                _BackgroundWorker.RunWorkerAsync(_StoreNo)

                'If _StoreNo = -1 Then
                '    ParseAll()
                'Else
                '    ParseStore()
                'End If
            Catch ex As Exception
                '   WriteToConsole("ERROR: " & ex.Message & vbCrLf & CType(IIf(Not ex.InnerException Is Nothing, ex.InnerException.Message, ""), String))
                MessageBox.Show(ex.Message)
            Finally

            End Try


        End If
    End Sub


    Private Sub ValidateArguments()
        _StoreNo = CType(ComboBox_Stores.SelectedItem, TlogStoreInfo).StoreNo
        _ParseDate = DateTime_ParseDate.Value.Date
    End Sub
    Private Sub CreateDatabaseConnection()
        If _Factory Is Nothing Then
            _Factory = New DataAccess.DataFactory(DataAccess.DataFactory.ItemCatalog)
        End If
    End Sub

    Private Function ValidateRequiredFiles(ByRef FullPath As String) As Boolean

        Dim IsValid As Boolean = True
        Dim ParsingDirectory As DirectoryInfo = New DirectoryInfo(FullPath)

        For Each rFile As String In _RequiredFiles.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
            If ParsingDirectory.GetFiles(rFile).Length = 0 Then IsValid = False
        Next
        Return IsValid
    End Function

    Private Sub ParseRequiredFiles(ByRef ParsingDirectory As DirectoryInfo, ByRef store As TlogStoreInfo)
        Dim oParser As FLParser = New FLParser(_TlogLoginInfo)
        oParser.ClearLoadTables()

        For Each rFile As String In _RequiredFiles.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
            For Each aFile As FileInfo In ParsingDirectory.GetFiles(rFile)
                WriteToConsole("Parsing " & aFile.Name & " for " & Trim(store.StoreName))
                If rFile.Contains("DWTENDER") Then oParser.ImportDWFile(aFile.FullName, FLParser.DWFileType.DWTENDER)
                If rFile.Contains("DWITEM") Then oParser.ImportDWFile(aFile.FullName, FLParser.DWFileType.DWITEM)
            Next
        Next

        WriteToConsole("Updating Aggregates for " & Trim(store.StoreName))
        oParser.UpdateAggregates(store.StoreRegion, "False")
    End Sub

    Private Sub CheckLoginInformation()
        If _EncryptedConnectionStrings.ToLower().Equals("true") Then
            ' decrypt values.
            Dim dec As WholeFoods.Utility.Encryption.Encryptor = New Encryption.Encryptor
            _TlogLoginInfo.DBUser = dec.Decrypt(_TlogDBUsername)
            _TlogLoginInfo.DBPass = dec.Decrypt(_TlogDBpassword)
            _TlogLoginInfo.ProcessUser = dec.Decrypt(_TlogProcessUsername)
            _TlogLoginInfo.ProcessPass = dec.Decrypt(_TlogProcesspassword)
        Else

            _TlogLoginInfo.DBPass = _TlogDBpassword
            _TlogLoginInfo.DBUser = _TlogDBUsername
            _TlogLoginInfo.ProcessPass = _TlogProcesspassword
            _TlogLoginInfo.ProcessUser = _TlogProcessUsername
            _TlogLoginInfo.DBServer = _TlogDBServer

        End If
        _TlogLoginInfo.DBServer = _TlogDBServer
        _TlogLoginInfo.Database = _TlogDatabase
        _TlogLoginInfo.RunFromClient = True

    End Sub

    Private Delegate Sub WriteToConsoleDelegate(ByVal msg As String)
    Private Sub WriteToConsole(ByVal msg As String)
        UIUpdates.StatusMsg = msg
        _BackgroundWorker.ReportProgress(0)
    End Sub
    Private Function Date2String(ByVal d As DateTime) As String
        'return a string representation of the date as yymmdd
        Dim retval As String
        retval = d.Year.ToString.Substring(2, 2)
        If d.Month < 10 Then
            retval += "0" & d.Month.ToString
        Else
            retval += d.Month.ToString
        End If
        If d.Day < 10 Then
            retval += "0" & d.Day.ToString
        Else
            retval += d.Day.ToString
        End If
        Return retval
    End Function
    Private Sub ParseAll()
        Dim cnt As Integer = 0
        UIUpdates.ClearValues()
        UIUpdates.ProgressMax = _StoreInformation.Count
        UIUpdates.ProgressValue = 0
        _BackgroundWorker.ReportProgress(0)



        For Each store As TlogStoreInfo In _StoreInformation

            Try
                If Not store.StoreAbbr.Equals("") Then
                    Dim FullPath As String = _BasePath & Date2String(_ParseDate) & "\" & store.StoreRegion & store.StoreAbbr.Trim & "\"
                    If File.Exists(FullPath) Then Throw New Exception(FullPath & " appears to be a file. A directory is expected.")
                    If Not Directory.Exists(FullPath) Then Throw New Exception("Cannot find " & FullPath)
                    If Not ValidateRequiredFiles(FullPath) Then Throw New Exception("Not all required files can be found. " & _RequiredFiles)
                    ParseRequiredFiles(New DirectoryInfo(FullPath), store)
                Else
                    WriteToConsole("Skipped " & Trim(store.StoreName) & ". No Store Abbreviation found.")
                End If
            Catch ex As Exception
                Dim InnerMsg As String
                If ex.InnerException Is Nothing Then InnerMsg = "" Else InnerMsg = ex.InnerException.Message

                WriteToConsole("Could Not Parse Tlogs for: " & Trim(store.StoreName) & " " & ex.Message & " " & InnerMsg)
            Finally
                WriteToConsole("Finished Processing.")
                WriteToConsole("--------------------------------------------------")
            End Try
            cnt += 1
            UIUpdates.ClearValues()
            UIUpdates.ProgressValue = cnt
            _BackgroundWorker.ReportProgress(0)
        Next
    End Sub

    Private Sub ParseStore()

        Dim CurrentStore As TlogStoreInfo = Nothing
        Try
            For Each store As TlogStoreInfo In _StoreInformation
                If store.StoreNo = _StoreNo Then
                    CurrentStore = store
                End If
            Next
            If CurrentStore Is Nothing Then Throw New Exception("Could not find the store information for StoreNo: " & _StoreNo.ToString)
            If Not CurrentStore.StoreAbbr.Equals("") Then
                Dim FullPath As String = _BasePath & Date2String(_ParseDate) & "\" & CurrentStore.StoreRegion & CurrentStore.StoreAbbr.Trim & "\"
                If File.Exists(FullPath) Then Throw New Exception(FullPath & " appears to be a file. A directory is expected.")
                If Not Directory.Exists(FullPath) Then Throw New Exception("Cannot find " & FullPath)
                If Not ValidateRequiredFiles(FullPath) Then Throw New Exception("Not all required files can be found. " & _RequiredFiles)
                ParseRequiredFiles(New DirectoryInfo(FullPath), CurrentStore)
            Else
                WriteToConsole("Skipped " & Trim(CurrentStore.StoreName) & ". No Store Abbreviation found.")
            End If
        Catch ex As Exception
            Dim InnerMsg As String
            If ex.InnerException Is Nothing Then InnerMsg = "" Else InnerMsg = ex.InnerException.Message
            WriteToConsole("Could Not Parse Tlogs for: " & Trim(CurrentStore.StoreName) & vbCrLf & ex.Message & vbCrLf & InnerMsg)
        Finally
            WriteToConsole("Finished Processing.")
            WriteToConsole("--------------------------------------------------")
        End Try
    End Sub

    Private Sub ClearToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearToolStripMenuItem.Click
        ListView_logs.Items.Clear()
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Dim f As FolderBrowserDialog = New FolderBrowserDialog()
        f.RootFolder = Environment.SpecialFolder.Desktop
        f.ShowDialog()

        Dim fs As TextWriter = New StreamWriter(f.SelectedPath & "\TlogParsing.Log")
        For Each item As ListViewItem In ListView_logs.Items
            fs.WriteLine(item.ToString())
        Next
        fs.Flush()
        fs.Close()

    End Sub
    Private Sub BackgroundWorker_Dowork(ByVal sedner As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles _BackgroundWorker.DoWork
        If CType(e.Argument, Integer) = -1 Then
            ParseAll()
        Else
            ParseStore()
        End If

    End Sub
    Private Sub BackgroundWorker_WorkDone(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles _BackgroundWorker.RunWorkerCompleted
        Button_Parse.Enabled = True
    End Sub
    Private Sub BackgroundWorker_Progress(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles _BackgroundWorker.ProgressChanged
        UpdateUI()
    End Sub
    Private Sub backgroundworker_Dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles _BackgroundWorker.Disposed

    End Sub

    Private Sub UpdateUI()


        If Not UIUpdates.StatusMsg.Equals(String.Empty) Then
            Dim item As ListViewItem = ListView_logs.Items.Add(DateTime.Now.ToString() & " " & UIUpdates.StatusMsg)
            item.EnsureVisible()
            ListView_logs.Refresh()
        End If

        If UIUpdates.ProgressMax > -1 Then
            'update max on progress bar
            ProgressBar_Tlog.Maximum = UIUpdates.ProgressMax
            ProgressBar_Tlog.Refresh()
        End If

        If UIUpdates.ProgressValue > -1 Then
            'update vlue on progress bar
            With ProgressBar_Tlog
                If UIUpdates.ProgressValue <= .Maximum Then
                    .Value = UIUpdates.ProgressValue
                Else
                    .Value = .Maximum
                End If
                .Refresh()
            End With
        End If

        UIUpdates.ClearValues()
    End Sub

End Class