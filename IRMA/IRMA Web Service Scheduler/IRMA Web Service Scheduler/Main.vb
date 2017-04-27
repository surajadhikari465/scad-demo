Imports System.Threading
Imports System.Net
Imports System.Windows.Forms
Module IRMAWebServiceTools

    ' This project is intented to run as a scheduled service but can be run manually.

    ' When running as a scheduled service (Windows Scheduled Task), run with a task switch to bypass the menu.
    ' EXAMPLE:
    '   "C:\Program Files\IRMA Web Service Toolkit\irmawstool.exe" -a runs ALL supported tasks.
    '   "C:\Program Files\IRMA Web Service Toolkit\irmawstool.exe" -tl runs the TLOG Error Pull.
    '
    ' To run different tasks as different times, simply setup a scheduled task for each switch.
    '
    ' Supported Switches:
    '
    '   -wa     All WIMP Extract Tasks
    '   -wc     WIMP Cost file extract
    '   -wi     WIMP Item file extract
    '   -wp     WIMP Planogram file extract
    '   -i      Inventory File Extract
    '   -pl     Automated POS Push Log Pull
    '   -wr     Weekly Sales Rollup
    '   -tl     TLOG Error Pull (added 1/2/2008)
    '   -vim    VIM Supplemental Extract (added 2/12/08)
    '   
    '
    ' Configuration
    '---------------------------------------------------------------------------------
    ' To change the Web Service, change the WebServiceURI in My.Settings (app.config) and recompile
    ' To change the FTP Parameters, change them in FTP.xml.
    ' Error/Success message formatting is stored in My.Resources
    ' The WSTimeOut value in app.config controls how long this app will wait for a response from the service (in milliseconds). Default is 3600000 (1 hour).
    ' If you do not pass a delimiter to the service, it defaults to a pipe.


    Private FTPSettings As DataTable
    Private _email As String = My.Settings.EmailDefault
    Private _tlogDate As String


    Sub Main()

        Dim _ftp As DataTable

        Console.WriteLine("IRMA Web Service Toolkit v" + Application.ProductVersion)
        Console.WriteLine("---------------------------------------")

        ' setup the tlog date for the TLOG extract call
        SetTLOGDate()

        Try
            ' grab the FTP settings from the XML config file
            _ftp = New DataTable
            _ftp = LoadFTPSettings()
            FTPSettings = _ftp
        Catch ex As Exception
            Console.WriteLine("Unable to load FTP settings file: ")
            Console.WriteLine(ex.Message)
            Exit Sub
        End Try

        Dim args() As String = Environment.GetCommandLineArgs()

        ' test for a startup switch
        If args.Length = 1 Then

            ' no switch, started manually - show the menu

            Dim ck As ConsoleKeyInfo

            ' wait for the user to start it
            ' add any additional methods to the menu here
            Console.WriteLine("")
            Console.WriteLine("")
            Console.WriteLine("Choose from the task list below:")
            Console.WriteLine("")
            Console.WriteLine("1. Extract the WIMP ITEM file")
            Console.WriteLine("2. Extract the WIMP COST file")
            Console.WriteLine("3. Extract the WIMP PLANO file")
            Console.WriteLine("4. Generate Store Inventory Service Files")
            Console.WriteLine("5. Pull Automated POS Push Logs and Archived Batches")
            Console.WriteLine("6. Pull TLOG Parser Log Errors")
            Console.WriteLine("7. Extact VIM Supplement File")
            Console.WriteLine("")
            Console.WriteLine("Enter Selection: ")

InvalidKeyPressed:

            ck = Console.ReadKey(True)

            ' if you add additional methods to the menu, be sure to include the ReadKey intercept in the Select Case statement
            Select Case ck.Key

                Case ConsoleKey.NumPad1, ConsoleKey.D1

                    RunTask(IRMAWebServices.SupportedMethod.ITEM_FILE)

                Case ConsoleKey.NumPad2, ConsoleKey.D2

                    RunTask(IRMAWebServices.SupportedMethod.COST_FILE)

                Case ConsoleKey.NumPad3, ConsoleKey.D3

                    RunTask(IRMAWebServices.SupportedMethod.PLANO_FILE)

                Case ConsoleKey.NumPad4, ConsoleKey.D4

                    RunTask(IRMAWebServices.SupportedMethod.INVENTORY_SERVICE_FILE)

                Case ConsoleKey.NumPad5, ConsoleKey.D5
                    ' stop and ask for an email or whether to use default
                    Console.Clear()
                    Console.WriteLine("")
                    Console.WriteLine("Pull Automated POS Push Logs and Archived Batches")
                    Console.WriteLine("---------------------------------------------------")
                    Console.WriteLine("")
                    Console.WriteLine("Enter the email where you would like the files sent.")
                    Console.WriteLine("")
                    Console.WriteLine("        OR")
                    Console.WriteLine("")
                    Console.WriteLine("Press ENTER to use the default (" + My.Settings.EmailDefault + ")")

                    _email = Console.ReadLine().ToString

                    ' if the user didn't enter anything, use the default
                    If _email = String.Empty Then
                        _email = My.Settings.EmailDefault
                    End If

                    RunTask(IRMAWebServices.SupportedMethod.AUTO_PUSH_LOGS)

                Case ConsoleKey.NumPad6, ConsoleKey.D6
                    ' stop and ask for a TLOG date to look for
                    Console.Clear()
                    Console.WriteLine("")
                    Console.WriteLine("Pull TLOG Parser Log Errors")
                    Console.WriteLine("---------------------------------------------------")
                    Console.WriteLine("")
                    Console.WriteLine("Enter the date to pull errors for (ddmmyyyy format: ex. 12/31/2007 = 31122007")
                    Console.WriteLine("")
                    Console.WriteLine("        OR")
                    Console.WriteLine("")
                    Console.WriteLine("Press ENTER to use today's date.")

                    _tlogDate = Console.ReadLine().ToString

                    ' if the user didn't enter anything, use the default
                    If _tlogDate = String.Empty Then

                        SetTLOGDate()

                    End If

                    ' stop and ask for an email or whether to use default
                    Console.Clear()
                    Console.WriteLine("")
                    Console.WriteLine("Pull TLOG Parser Log Errors")
                    Console.WriteLine("---------------------------------------------------")
                    Console.WriteLine("")
                    Console.WriteLine("Enter the email where you would like the files sent.")
                    Console.WriteLine("")
                    Console.WriteLine("        OR")
                    Console.WriteLine("")
                    Console.WriteLine("Press ENTER to use the default (" + My.Settings.EmailDefault + ")")

                    _email = Console.ReadLine().ToString

                    ' if the user didn't enter anything, use the default
                    If _email = String.Empty Then

                        _email = My.Settings.EmailDefault

                    End If

                    RunTask(IRMAWebServices.SupportedMethod.TLOG_ERROR_PULL)

                Case ConsoleKey.NumPad7, ConsoleKey.D7

                    RunTask(IRMAWebServices.SupportedMethod.VIM_SUPPLEMENT_EXTRACT)

                Case ConsoleKey.Escape

                    Exit Sub

                Case Else

                    Console.WriteLine("")
                    Console.WriteLine("Invalid option, try again.")
                    Console.WriteLine("")
                    Console.WriteLine("Enter Selection: ")

                    GoTo InvalidKeyPressed

            End Select
        Else

            ' a switch was found, run the appropriate method
            Select Case args.GetValue(args.GetUpperBound(0)).ToString
                Case "-wa"
                    RunTask(IRMAWebServices.SupportedMethod.COST_FILE)
                    RunTask(IRMAWebServices.SupportedMethod.ITEM_FILE)
                    RunTask(IRMAWebServices.SupportedMethod.PLANO_FILE)
                Case "-wi"
                    RunTask(IRMAWebServices.SupportedMethod.ITEM_FILE)
                Case "-wc"
                    RunTask(IRMAWebServices.SupportedMethod.COST_FILE)
                Case "-wp"
                    RunTask(IRMAWebServices.SupportedMethod.PLANO_FILE)
                Case "-i"
                    RunTask(IRMAWebServices.SupportedMethod.INVENTORY_SERVICE_FILE)
                Case "-pl"
                    RunTask(IRMAWebServices.SupportedMethod.AUTO_PUSH_LOGS)
                Case "-wr"
                    RunTask(IRMAWebServices.SupportedMethod.WEEKLY_ROLLUP)
                Case "-tl"
                    RunTask(IRMAWebServices.SupportedMethod.TLOG_ERROR_PULL)
                Case "-vim"
                    RunTask(IRMAWebServices.SupportedMethod.VIM_SUPPLEMENT_EXTRACT)
            End Select
        End If

    End Sub

    Private Function GetFTPParams(ByVal Task As IRMAWebServices.SupportedMethod) As IRMAWebServices.FTPBO

        ' uses the web service business object provided
        Dim ftpParams As IRMAWebServices.FTPBO

        Try

            ftpParams = New IRMAWebServices.FTPBO()

            ' grab the settings for the task specified...
            Dim expression As String = "name = '" & Task.ToString & "'"
            Dim taskRows() As DataRow

            taskRows = FTPSettings.Select(expression)

            ' grab the individual row values for the task

            Dim currRow As DataRow = taskRows.GetValue(0)

            ftpParams.FTP_IP = currRow.Item("ftpHost").ToString
            ftpParams.FTP_DIR = currRow.Item("ftpDir").ToString
            ftpParams.FTP_UID = currRow.Item("ftpUser").ToString
            ftpParams.FTP_PWD = currRow.Item("ftpPwd").ToString
            ftpParams.FTP_PORT = currRow.Item("ftpPort").ToString
            ftpParams.FTP_SECURE = currRow.Item("secure").ToString
            ftpParams.FTP_DELIM = currRow.Item("delim").ToString

            ' return the FTP info
            Return ftpParams

        Catch ex As Exception

            Console.WriteLine(String.Format("An error ocurred: {0}", ex.Message))
            My.Application.Log.WriteEntry(ex.Message, TraceEventType.Error)

        End Try


    End Function

    Private Sub RunTask(ByVal TaskChoice As IRMAWebServices.SupportedMethod)

        Dim irmaWS As IRMAWebServices.IRMAServices
        Dim response As IRMAWebServices.ServiceResponse

        Try
            ' new web service instance
            irmaWS = New IRMAWebServices.IRMAServices
            irmaWS.Timeout = My.Settings.WSTimeOut

            ' the password here should probably be encrypted
            irmaWS.Credentials = New System.Net.NetworkCredential(My.Settings.WSUser, My.Settings.WSPwd, My.Settings.WSDomain)

            Console.WriteLine("")
            Console.WriteLine(String.Format("Running {0} task...", TaskChoice.ToString))

            ' what choice was passed in? run the choice...
            Select Case TaskChoice

                Case IRMAWebServices.SupportedMethod.INVENTORY_SERVICE_FILE

                    response = irmaWS.GetInventoryFileExtract(GetFTPParams(TaskChoice))

                Case IRMAWebServices.SupportedMethod.VIM_SUPPLEMENT_EXTRACT

                    response = irmaWS.GetVIMExtracts(GetFTPParams(TaskChoice))

                Case IRMAWebServices.SupportedMethod.AUTO_PUSH_LOGS

                    response = irmaWS.GetPOSPushLogs(_email)

                Case IRMAWebServices.SupportedMethod.WEEKLY_ROLLUP

                    response = IRMAWebServices.ServiceResponse.NotSupported

                Case IRMAWebServices.SupportedMethod.TLOG_ERROR_PULL

                    response = irmaWS.GetTlogErrors(_tlogDate, _email)

                Case Else

                    response = irmaWS.GetWIMPExtract(GetFTPParams(TaskChoice), TaskChoice)

            End Select

            ' how did it go? write out the response and log any error
            If response = IRMAWebServices.ServiceResponse.Success Then
                Console.WriteLine(My.Resources.msg_success)
            Else
                Console.WriteLine(String.Format(My.Resources.msg_failed, TaskChoice.ToString, response.ToString))
                My.Application.Log.WriteEntry(String.Format(My.Resources.msg_failed, TaskChoice.ToString, response.ToString), TraceEventType.Error)
            End If

            Console.WriteLine("")
            Console.WriteLine("Task Complete")

        Catch ex As Exception

            Console.WriteLine(String.Format("An error ocurred: {0}", ex.Message))
            My.Application.Log.WriteEntry(ex.Message, TraceEventType.Error)

        End Try

    End Sub

    Private Sub SetTLOGDate()

        Dim _day As String = Today.Day.ToString
        Dim _month As String = Today.Month.ToString
        Dim _year As String = Today.Year.ToString

        ' needs a zero in front of any single-digits
        If Len(_day) = 1 Then _day = "0" + _day
        If Len(_month) = 1 Then _month = "0" + _month

        _tlogDate = (_day + _month + _year).ToString

    End Sub

    Private Function LoadFTPSettings() As DataTable

        Dim doc As Xml.XmlDocument
        Dim dt As DataTable = Nothing
        Dim dr As DataRow = Nothing

        Try

            doc = New Xml.XmlDocument()
            dt = New DataTable

            doc.Load(Application.StartupPath + "\FTP.xml")

            doc.PreserveWhitespace = True

            ' set up the datatable
            dt.Columns.Add("name")
            dt.Columns.Add("ftpHost")
            dt.Columns.Add("ftpPort")
            dt.Columns.Add("ftpUser")
            dt.Columns.Add("ftpPwd")
            dt.Columns.Add("ftpDir")
            dt.Columns.Add("delim")
            dt.Columns.Add("secure")

            ' get ready to iterate through the nodes
            Dim navigator As Xml.XPath.XPathNavigator = doc.CreateNavigator()
            Dim nodes As Xml.XPath.XPathNodeIterator = navigator.Select("/config/task")

            While nodes.MoveNext

                ' create a new row for each task and its settings...
                dr = dt.NewRow
                dr.Item("name") = nodes.Current.GetAttribute("name", navigator.NamespaceURI)
                dr.Item("ftpHost") = nodes.Current.GetAttribute("ftpHost", navigator.NamespaceURI)
                dr.Item("ftpPort") = nodes.Current.GetAttribute("ftpPort", navigator.NamespaceURI)
                dr.Item("ftpUser") = nodes.Current.GetAttribute("ftpUser", navigator.NamespaceURI)
                dr.Item("ftpPwd") = nodes.Current.GetAttribute("ftpPwd", navigator.NamespaceURI)
                dr.Item("ftpDir") = nodes.Current.GetAttribute("ftpDir", navigator.NamespaceURI)
                dr.Item("delim") = nodes.Current.GetAttribute("delim", navigator.NamespaceURI)
                dr.Item("secure") = nodes.Current.GetAttribute("secure", navigator.NamespaceURI)

                dt.Rows.Add(dr)

            End While

            'return the data table with the FTP settings
            Return dt

        Catch ex As Exception

            Console.WriteLine(String.Format("An error ocurred: {0}", ex.Message))
            My.Application.Log.WriteEntry(ex.Message, TraceEventType.Error)

        Finally

            If Not dt Is Nothing Then dt.Dispose()

        End Try

    End Function

End Module