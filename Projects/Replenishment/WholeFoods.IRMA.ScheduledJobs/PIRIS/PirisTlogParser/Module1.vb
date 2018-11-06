Imports WholeFoods.Utility.FTP
Imports WholeFoods.Utility
Imports WholeFoods.Utility.SMTP
Imports WholeFoods.IRMA.Replenishment.Tlog
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration
Imports System.Threading


Module Module1
    Private CmdArgs() As String = System.Environment.GetCommandLineArgs()
    Private _StoreNo As Integer
    Private _ParseDate As DateTime
    Private _DaysOffset As Integer
    Private _Factory As DataAccess.DataFactory
    Private _StoreInformation As List(Of StoreInfo) = New List(Of StoreInfo)
    Private _Logs As List(Of String) = New List(Of String)
    Private _IsDebug As Boolean = False
    Private _LogFileAge As String


    Sub Main()

        Try

            ' download the appSettings document
            Configuration.CreateAppSettings()

            SetConfiguration()

            ValidateArguments()
            CreateDatabaseConnection()
            GetStoreInfo()

            If _StoreNo = -1 Then
                ParseAll()
            Else
                ParseStore()
            End If
            WriteToConsole("Finished Processing.", True)
            WriteToConsole("----------------------------------------------------------------------", True)

        Catch ex As Exception
            WriteToConsole("Fatal: " & ex.Message, True)
            My.Application.Log.WriteException(ex, TraceEventType.Error, String.Empty)
            Thread.Sleep(5000)
        Finally
            Try

                ' check to see if Logs direcotry exists. If not, create it.
                If Not Directory.Exists(".\Logs") Then Directory.CreateDirectory(".\Logs")

                If _Logs.Count > 1 Then
                    Dim txt As TextWriter = New StreamWriter(".\Logs\TlogParser-" & FixDate(DateTime.Now) & ".log", True)
                    txt.WriteLine(String.Join(vbCrLf, _Logs.ToArray()))
                    txt.Close()
                    txt.Dispose()
                End If

                Dim files As String() = Directory.GetFileSystemEntries(".\Logs", "*.log")

                Dim fi As FileInfo
                For Each file As String In files
                    fi = New FileInfo(file)
                    If fi.CreationTime < DateTime.Now.AddDays(Math.Abs(Integer.Parse(_LogFileAge)) * -1) Then
                        fi.Delete()
                    End If
                Next

            Catch ex As Exception
                WriteToConsole("Fatal: " & ex.Message, True)
                My.Application.Log.WriteException(ex, TraceEventType.Error, String.Empty)
            End Try

        End Try

    End Sub

    Private Function FixDate(ByVal d As DateTime) As String
        Dim retval As String
        If d.Day < 10 Then
            retval = "0" & d.Day.ToString
        Else
            retval = d.Day.ToString
        End If

        If d.Month < 10 Then
            retval += "0" & d.Month.ToString
        Else
            retval += d.Month.ToString
        End If
        retval += d.Year.ToString
        Return retval
    End Function
    Private Function ShowUsage() As String
        Return vbCrLf & vbCrLf & "Usage: PirisTlogParser.exe <Store_No> <Date To Parse> <Days Offset>" & vbCrLf & vbCrLf & _
                "Store_No:  A Valid Store_No " & vbCrLf & "Date To Parse: This can be 'today', 'yesterday', or a valid date. " & vbCrLf & "Days Offset: This number of days will be added or subtraced from the Date To Parse" & vbCrLf & _
                "Example: if Date To Parse = 8/24/2006 and Days Offset = -1 then 8/23/2006 will be the effective date"
    End Function
    Private Sub ValidateArguments()

        If CmdArgs.Length < 4 Then
            Throw New Exception("Too Few Arguments." & ShowUsage())
        End If
        If CmdArgs.Length > 5 Then
            Throw New Exception("Too Many Arguments." & ShowUsage())
        End If
        Try
            If CmdArgs(1).ToLower.Equals("all") Then
                _StoreNo = -1
            Else
                _StoreNo = CInt(CmdArgs(1))
            End If

        Catch ex As Exception
            Throw New Exception("Argument 1 does not appear to be a valid store number." & ShowUsage())
        End Try

        Try
            If CmdArgs(2).ToLower.Equals("today") Then
                _ParseDate = DateTime.Now

            ElseIf CmdArgs(2).ToLower.Equals("yesterday") Then
                _ParseDate = DateTime.Now.AddDays(-1)
            Else
                _ParseDate = CDate(CmdArgs(2))
            End If

        Catch ex As Exception
            Throw New Exception("Argument 2 does not appear to be a valid date." & ShowUsage())
        End Try

        Try
            _DaysOffset = CInt(CmdArgs(3))
            _ParseDate = _ParseDate.AddDays(_DaysOffset)
        Catch ex As Exception
            Throw New Exception("Argument 2 does not appear to be a valid integer." & ShowUsage())
        End Try

        If (CmdArgs.Length = 5) Then
            If CmdArgs(4).ToLower.Equals("-debug") Then
                _IsDebug = True
            End If
        End If

        WriteToConsole("Arguments Given: " & (CmdArgs.Length - 1).ToString, True)
        WriteToConsole("StoreNo: " & CmdArgs(1), True)
        WriteToConsole("Date: " & CmdArgs(2), True)
        WriteToConsole("DaysOffset: " & CmdArgs(3), True)
        If _IsDebug Then WriteToConsole("DebugMode: ON", True)





    End Sub

    Private Sub CreateDatabaseConnection()
        _Factory = New DataAccess.DataFactory(DataAccess.DataFactory.ItemCatalog)
    End Sub

    Private Sub ParseStore()
        Dim mystore As StoreInfo = Nothing
        Dim oParser As TlogParserInterface = Nothing
        Dim FileName As String = GetTlogFilenameFromDate(_ParseDate)
        Dim memBuffer As MemoryStream = Nothing
        Dim ftpClient As FTP.FTPclient = Nothing


        For Each store As StoreInfo In _StoreInformation
            If store.StoreNo = _StoreNo.ToString Then
                mystore = store
            End If
        Next

        If mystore Is Nothing Then
            Throw New Exception("Could not find store information for Store_No: " & _StoreNo.ToString)
        Else
            Try
                oParser = New Parser
                If _IsDebug Then
                    oParser.IsDebug = True
                End If
                WriteToConsole("Parsing " & FileName & " for " & mystore.StoreName, True)
                FileName = GetTlogFilenameFromDate(_ParseDate)
                ftpClient = New FTP.FTPclient(mystore.FtpIP, mystore.FtpUser, mystore.FtpPass, mystore.IsSecure)
                ftpClient.Download(mystore.FtpDirectory & FileName, memBuffer, True)
                oparser.ParseDataFromMemoryStream(memBuffer.ToArray)
            Catch ex As Exception
                Throw New Exception("Parsing Failed: " & ex.Message)
            Finally
                If Not memBuffer Is Nothing Then
                    memBuffer.Dispose()
                End If
                If Not ftpClient Is Nothing Then
                    ftpClient = Nothing
                End If
                If Not oParser Is Nothing Then
                    oParser.Dispose()
                End If

            End Try




        End If


    End Sub

    Private Sub WriteToConsole(ByVal msg As String, ByVal log As Boolean)
        Console.WriteLine(DateTime.Now.ToString() & " " & msg)
        If log Then _Logs.Add(DateTime.Now.ToString() & " " & msg)
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


    Private Sub GetStoreInfo()
        Dim ds As DataSet = _Factory.GetStoredProcedureDataSet("GetStoreTlogFTPInfo")

        _StoreInformation.Clear()
        For Each dr As DataRow In ds.Tables(0).Rows
            Dim store As StoreInfo = New StoreInfo
            store.StoreName = dr("Store_Name").ToString
            store.StoreNo = CInt(dr("Store_No"))
            store.FtpUser = dr("TlogFtpUser").ToString
            store.FtpPass = dr("TlogFtpPassword").ToString
            store.FtpIP = dr("TlogIpAddress").ToString
            store.FtpDirectory = dr("TlogFtpDirectory").ToString
            If CInt(dr("IsSecureTransfer")) = 0 Then store.IsSecure = False Else store.IsSecure = True
            _StoreInformation.Add(store)
        Next

    End Sub

    Private Sub ParseAll()
        Dim memBuffer As MemoryStream = Nothing
        Dim ftpClient As FTP.FTPclient = Nothing
        Dim oParser As TlogParserInterface = Nothing

        Dim FileName As String = String.Empty
        FileName = GetTlogFilenameFromDate(_ParseDate)



        For Each store As StoreInfo In _StoreInformation
            Try
                WriteToConsole("Parsing " & FileName & " for " & store.StoreName, True)

                oParser = New Parser
                If _IsDebug Then
                    oParser.IsDebug = True
                End If
                FileName = GetTlogFilenameFromDate(_ParseDate)
                ftpClient = New FTP.FTPclient(store.FtpIP, store.FtpUser, store.FtpPass, store.IsSecure)
                ftpClient.Download(store.FtpDirectory & FileName, memBuffer, True)
                oParser.ParseDataFromMemoryStream(memBuffer.ToArray)

            Catch ex As Exception
                If ex.Message.Contains("550") Then
                    WriteToConsole("Could not find " & FileName & " for " & store.StoreName, True)
                Else
                    WriteToConsole(ex.Message, True)
                End If
            Finally

                oParser.Dispose()
                If Not ftpClient Is Nothing Then
                    ftpClient = Nothing
                End If
            End Try
        Next
    End Sub

    Private Sub SetConfiguration()

        _LogFileAge = ConfigurationServices.AppSettings("LogFileAge")

    End Sub


    Private Class StoreInfo
        Sub New()

        End Sub

        Private _storeNo As Integer
        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property


        Private _StoreName As String
        Public Property StoreName() As String
            Get
                Return _StoreName
            End Get
            Set(ByVal value As String)
                _StoreName = value
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


        Private _IsSecure As Integer
        Public Property IsSecure() As Integer
            Get
                Return _IsSecure
            End Get
            Set(ByVal value As Integer)
                _IsSecure = value
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



    End Class


End Module
