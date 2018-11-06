Imports Microsoft.Build.Utilities
Imports System.IO
Imports System.Net.Mail
Imports System.Text
Imports System.Xml

Namespace WholeFoods.Build.BuildTask
    ''' <summary>
    ''' The BuildDatabaseScript defines a custom build task to create a database script
    ''' from the ItemCatalogDB project for the build.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BuildDatabaseScript
        Inherits Microsoft.Build.Utilities.Task
#Region "Property definitons"
        Private _sourceDir As String
        Private _outputDir As String
        Private _outputFilename As String = Nothing
        Private _outputFilenameJDA As String = Nothing
        Private _outputFilenameConversion As String = Nothing
        Private _archiveDir As String
        Private _validateGO As Boolean = False

        Private _errorFiles As ArrayList
#End Region

#Region "Task definitions"
        ''' <summary>
        ''' The Execute method builds a database scripts for the build and copies it to the output directory.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Execute() As Boolean
            Log().LogMessage("Entering BuildDatabaseScript.Execute method: source directory=" + _sourceDir)
            Dim returnVal As Boolean = True

            ' Concatenate all of the scripts in the source directory into a single file
            _errorFiles = New ArrayList()
            ConcatFilesInDirectory()

            ' If any error were encountered, send out an email notification.
            If _errorFiles.Count >= 1 Then
                SendEmailMessage(_sourceDir)
            End If

            ' If the archive directory is specified, create the directory in the local file system.
            ' This allows the TFS script that actually performs the archive to run without an error.
            If (_archiveDir IsNot Nothing) AndAlso (_archiveDir <> "") Then
                CreateLocalArchiveDirectory()
            End If

            Log().LogMessage("Exiting BuildDatabaseScript.Execute method: " + returnVal.ToString())
            Return returnVal
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ConcatFilesInDirectory()
            Dim outputFile As StreamWriter = Nothing
            Dim inputFile As StreamReader = Nothing

            Try
                ' List all the files in the directory 
                Dim files() As String = Directory.GetFiles(_sourceDir)
                Log().LogMessage("File count for " + _sourceDir + "=" + files.Length.ToString)

                ' TODO: Right now config file includes / in directory name ... check in code instead of assuming there
                If Not Directory.Exists(_outputDir) Then
                    Log().LogMessage("Creating output directory: " + _outputDir)
                    Directory.CreateDirectory(_outputDir)
                End If

                ' Append the contents of each file to the output file
                Dim fileName As String
                Dim fileEnum As IEnumerator = files.GetEnumerator()
                While fileEnum.MoveNext
                    fileName = CStr(fileEnum.Current)
                    ' Open the correct output file
                    Dim outputPath As String
                    If fileName.Contains("JDASync") AndAlso Not _outputFilenameJDA Is Nothing Then
                        outputPath = _outputDir + _outputFilenameJDA
                    Else
                        outputPath = _outputDir + _outputFilename
                    End If

                    ' Create the output file, if it does not exist
                    If Not File.Exists(outputPath) Then
                        Log().LogMessage("Creating output file: " + outputPath)
                        File.Create(outputPath).Close()
                    End If

                    ' Open the input and output files
                    Log().LogMessage("Appending contents to output file: " + outputPath)
                    outputFile = New StreamWriter(outputPath, True)
                    inputFile = File.OpenText(fileName)

                    If ValidateGO Then
                        '' Make sure the file contains the text "DROP" ... "GO" ... "CREATE"
                        '' If not, it is invalid and will not be included in the build.
                        'Dim dropFound As Boolean = False
                        'Dim goFound As Boolean = False
                        'Dim createFound As Boolean = False
                        'Dim currLine As String
                        'currLine = inputFile.ReadLine()
                        'While Not currLine Is Nothing Or (dropFound AndAlso goFound AndAlso createFound)
                        '    If Not dropFound AndAlso currLine.ToUpper.Contains("DROP") Then
                        '        dropFound = True
                        '    ElseIf Not goFound AndAlso currLine.ToUpper.Contains("GO") Then
                        '        goFound = True
                        '    ElseIf Not createFound AndAlso currLine.ToUpper.Contains("CREATE") Then
                        '        createFound = True
                        '    End If
                        '    currLine = inputFile.ReadLine()
                        'End While

                        '' If the file is valid, append the contents of the input file to the output file.
                        'If dropFound AndAlso goFound AndAlso createFound Then
                        '    inputFile.Close()
                        '    inputFile = File.OpenText(fileName)
                        outputFile.Write(inputFile.ReadToEnd())
                        outputFile.WriteLine()
                        '    ' Always append an extra "GO" between files.  This won't hurt, and it will
                        '    ' reduce build errors.
                        '    outputFile.Write("GO")
                        '    outputFile.WriteLine()
                        'Else
                        '    ' The file is not included in the build because it does not follow the IRMA
                        '    ' standards.
                        '    _errorFiles.Add(fileName)
                        'End If
                    Else
                        ' No validation is necessary.
                        ' Append the contents of the input file to the correct output file
                        outputFile.Write(inputFile.ReadToEnd())
                        outputFile.WriteLine()
                    End If

                    ' Close the input and output files
                    outputFile.Close()
                    inputFile.Close()
                End While

            Finally
                ' Close the files
                If outputFile IsNot Nothing Then
                    outputFile.Close()
                End If
                If inputFile IsNot Nothing Then
                    inputFile.Close()
                End If
            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CreateLocalArchiveDirectory()
            If Not Directory.Exists(_archiveDir) Then
                Log().LogMessage("Creating archive directory: " + _archiveDir)
                Directory.CreateDirectory(_archiveDir)
            End If
        End Sub

        ''' <summary>
        ''' Sends a page and/or email to warn about system failure.  SeverityLevel used to 
        ''' determing if page should be sent.  Otherwise only email sent to configured email
        ''' addresses.
        ''' Also logs the error to the application event log.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SendEmailMessage(ByVal fileName As String)
            'send error message using MailMessage
            Dim message As New MailMessage
            Dim mailClient As New SmtpClient
            Dim messageText As New StringBuilder
            Dim appEventLog As New EventLog

            Try
                'get base message based on error type
                messageText.Append("The following scripts were skipped when processing the ")
                messageText.Append(_sourceDir)
                messageText.Append(" source directory as part of the database build.")
                messageText.Append("They did not follow the DROP ... GO ... CREATE ... GO standard. ")
                messageText.Append("These must be corrected to be incluced in a build:")
                messageText.Append(Environment.NewLine)

                Dim fileEnum As IEnumerator = _errorFiles.GetEnumerator
                While fileEnum.MoveNext
                    messageText.Append(fileEnum.Current)
                    messageText.Append(Environment.NewLine)
                End While

                'send email
                'build the message
                message.From = New MailAddress("IRMA.Automated.Build@wholefoods.com")
                message.To.Add(New MailAddress("amanda.black@wholefoods.com"))
                message.Subject = "IRMA Database Build Error Encountered -- Action Needed"
                message.Body = messageText.ToString()

                'deliver the message
                mailClient.Send(message)
            Catch ex As Exception
                ' error processing failed
            End Try
        End Sub


#End Region

#Region "Property access methods"
        ''' <summary>
        ''' Source directory.  The latest version of the files should already be retrieved from TFS for this directory.
        ''' All the files in the source directory are combined into one script.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceDir() As String
            Get
                Return _sourceDir
            End Get
            Set(ByVal value As String)
                _sourceDir = value
            End Set
        End Property

        ''' <summary>
        ''' The name of the script that is generated.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OutputFilename() As String
            Get
                Return _outputFilename
            End Get
            Set(ByVal value As String)
                _outputFilename = value
            End Set
        End Property

        ''' <summary>
        ''' The name of the script that is generated for JDA data sync scripts.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OutputFilenameJDA() As String
            Get
                Return _outputFilenameJDA
            End Get
            Set(ByVal value As String)
                _outputFilenameJDA = value
            End Set
        End Property

        ''' <summary>
        ''' The name of the script that is generated for Conversion scripts.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OutputFilenameConversion() As String
            Get
                Return _outputFilenameConversion
            End Get
            Set(ByVal value As String)
                _outputFilenameConversion = value
            End Set
        End Property

        ''' <summary>
        ''' The output directory for the script that is generated.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OutputDir() As String
            Get
                Return _outputDir
            End Get
            Set(ByVal value As String)
                _outputDir = value
            End Set
        End Property

        ''' <summary>
        ''' If the build script move the files to an archive directory in TFS, the archive directory must already
        ''' exist in the local file system.  This optional parameter creates the archive directory.  It does not
        ''' perform the archive task.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ArchiveDir() As String
            Get
                Return _archiveDir
            End Get
            Set(ByVal value As String)
                _archiveDir = value
            End Set
        End Property

        ''' <summary>
        ''' If this flag is TRUE, the build process will validate that the script contains the word "GO".
        ''' This is to verify the user included the "GO" between the DROP and CREATE statements.  If it is 
        ''' missing, the script is not included in the build.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValidateGO() As Boolean
            Get
                Return _validateGO
            End Get
            Set(ByVal value As Boolean)
                _validateGO = value
            End Set
        End Property
#End Region

    End Class
End Namespace
