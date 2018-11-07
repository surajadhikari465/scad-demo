Imports System.Collections.Generic
Imports Jscape.Sftp
Imports System.IO
Imports System.Net





Namespace WholeFoods.Utility.SFTP


    Public Class FTPClient
        Implements IDisposable



        

#Region "Properties"
        Private _hostname As String
        Public Property HostName() As String
            Get
                Return _hostname
            End Get
            Set(ByVal value As String)
                _hostname = value
            End Set
        End Property


        Private _username As String
        Public Property UserName() As String
            Get
                Return _username
            End Get
            Set(ByVal value As String)
                _username = value
            End Set
        End Property



        Private _password As String
        Public Property Password() As String
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                _password = value
            End Set
        End Property


        Private _port As Integer
        Public Property Port() As Integer
            Get
                Return _port
            End Get
            Set(ByVal value As Integer)
                _port = value
            End Set
        End Property


        Private _timeout As Integer
        Public Property ConnectionTimeout() As Integer
            Get
                Return _timeout
            End Get
            Set(ByVal value As Integer)
                _timeout = value
            End Set
        End Property





        Public ReadOnly Property IsConnected() As Boolean
            Get
                Return _ftpClient.IsConnected
            End Get

        End Property


        Private _ftpClient As Jscape.Ftp.Ftp
        Public ReadOnly Property Instance() As Jscape.Ftp.Ftp
            Get
                Return _ftpClient
            End Get
        End Property


#End Region

#Region "Constructors"
        Sub New(Optional ByVal Port As Integer = 21)
            _port = Port
        End Sub

        Sub New(ByVal Hostname As String, Optional ByVal Port As Integer = 21)
            _hostname = Hostname

            _port = Port
        End Sub

        Sub New(ByVal Hostname As String, ByVal Username As String, ByVal Password As String, Optional ByVal Port As Integer = 21)
            _hostname = Hostname
            _username = Username
            _password = Password

            _port = Port
        End Sub
#End Region

        Private Function CreateFtpConnection(ByRef ftp As Jscape.Sftp.Sftp) As Boolean
            'creates a new jscape secure/nonsecure ftp object and returns the value by reference.
            'this code was placed in this sub to make for easy licensekey management.
            Dim ftpParams As New Jscape.Ssh.SshParameters(_hostname, _port, _username, _password)
            ftp = New Jscape.Sftp.Sftp(ftpParams)

            'Trial Key
            ftp.LicenseKey = "SSH Factory for .NET:Evaluation:Evaluation:09-20-2007:eFn7FJeauNGgEeMgEimqZsPKtG/Pd70w7X3C+tKj6gvl+C/q4ETlwg9TwQLrMkblUbpV0ABH+W3pSOFb8x0tKT7ApAvhIKr8YcE80fO352KSCSgHRCgPex0R5dtKr3bl+GpcFIlBxS79JY6ld8Aa7vlC9W8qwUMfZO5YKHDQ2Bg="



            Dim iAttempts As Integer = 1
            Dim sError As String = String.Empty

            Do While (iAttempts <= 3 AndAlso Not ftp.IsConnected)
                sError = String.Empty
                Try
                    ftp.Connect()
                Catch ex1 As Jscape.Sftp.SftpException
                    sError = String.Format("FtpException: {0}", ex1.Message)
                Catch ex As Exception
                    sError = String.Format("Exception: {0}", ex.Message)
                Finally
                    If Not sError.Equals(String.Empty) Then
                        Logger.LogError(String.Format("SFTP.CreateFtpConnection() Error: " & sError & " (attempt {0})", iAttempts.ToString), Me.GetType)
                    End If
                    iAttempts += 1
                End Try
            Loop

            If Not ftp.IsConnected Then
                sError = String.Format("Unable to connect to FTP (Host = '{0}', User = '{1}', Port = {2}).{3}{4}", ftp.Hostname, ftp.Username, ftp.Port, vbCrLf, sError)
                Throw New SftpException(sError)
            End If

            Return ftp.IsConnected()

        End Function
        Public Function ListDirectory(Optional ByVal directory As String = "") As List(Of String)
            'return a simple list of filenames in directory
            Dim result As New List(Of String)
            Dim sftp As Jscape.Sftp.Sftp = Nothing
            CreateFtpConnection(sftp)

            Dim oEnum As IEnumerator = sftp.GetDirListing(directory)

            While oEnum.MoveNext
                Dim info As Jscape.Sftp.Packets.FileInfo = CType(oEnum.Current, Jscape.Sftp.Packets.FileInfo)
                result.Add(info.Name)
            End While

            sftp.Disconnect()
            sftp.Dispose()

            Return result
        End Function

#Region "Upload: File transfer TO ftp server"
        Public Sub Upload(ByVal localFilename As String, ByVal targetFilename As String, Optional ByVal appendToFile As Boolean = False)
            '1. check source
            If Not File.Exists(localFilename) Then
                Throw New Exception("File " & localFilename & " not found")
            End If
            'copy to FI
            Dim fi As New FileInfo(localFilename)
            Upload(fi, targetFilename, appendToFile)
        End Sub

        Public Sub Upload(ByVal localFilename As String, ByVal DC As Boolean)
            '1. check source
            If Not File.Exists(localFilename) Then
                Throw New Exception("File " & localFilename & " not found")
            End If
            'copy to FI
            Dim appendToFile As Boolean = False
            Dim fi As New FileInfo(localFilename)
            Dim target As String
            target = GetFullName(localFilename)
            If DC Then
                DCUpload(fi, localFilename, appendToFile)
            Else
                Upload(fi, target, appendToFile)
            End If

        End Sub



        ''' <param name="fi">Source file</param>
        ''' <param name="targetFilename">Target filename (optional)</param>
        Public Sub DCUpload(ByVal fi As FileInfo, Optional ByVal targetFilename As String = "", Optional ByVal appendToFile As Boolean = False)
            'copy the file specified to target file: target file can be full path or just filename (uses current dir)


            Dim tmpValue As String
            Dim targetTemp As String
            Dim fileExists As Boolean
            Dim targetDir As String
            Dim fileName As String

            fileExists = False

            'value stored denotes the name of the temp file. If _tmp then the file would look like ftpme.txt_tmp
            tmpValue = ""
            fileName = GetFullName(targetFilename)
            targetDir = GetTargetPath(targetFilename)
            targetTemp = fileName + tmpValue


            'Dim sftp As Jscape.Sftp.Sftp = Nothing
            'CreateFtpConnection(sftp)\

            ''This uploads the file with the _tmp extention.
            'sftp.Upload(fi, targetTemp, appendToFile)
            ''This renames the tempfilename to the real filename once the upload is complete.
            'sftp.RenameFile(targetTemp, targetFilename)
            'sftp.Disconnect()
            'sftp.Dispose()

            'Else

            Dim sftp As Jscape.Sftp.Sftp = Nothing
            CreateFtpConnection(sftp)
            'This set the mode to binary
            sftp.SetBinaryMode()
            'If append to file is false, then the file is uploaded as a temp file and then renamed to the original filename
            If appendToFile = False Then
                'Uploads the file with the temp name
                sftp.Upload(fi, targetTemp, appendToFile)
            Else
                'changes to the correct target path passed down from the changeto field.
                If targetDir.Length > 1 Then
                    sftp.RemoteDir() = targetDir
                Else
                End If

                'Gets the directory listing and loops through to see if the target file already exists
                Dim e As IEnumerator = sftp.GetDirListing()
                ' enumerate thru listing printing filename for each entry 
                While e.MoveNext()
                    Dim file As Jscape.Sftp.Packets.FileInfo = CType(e.Current, Jscape.Sftp.Packets.FileInfo)
                    If file.Name = fileName Then
                        fileExists = True
                    End If
                End While

                'If the target file exists, this will rename it to a temp file, append the new uploaded temp file, and then
                'rename the combined temp file to the original target file.
                If fileExists Then
                    Try
                        sftp.RenameFile(fileName, fileName + tmpValue)
                    Catch ex As Exception

                    End Try

                End If
                sftp.Upload(fi, fileName + tmpValue, appendToFile)
                'This renames the tempfilename to the real filename once the upload is complete.
                sftp.RenameFile(fileName + tmpValue, fileName)

            End If

            sftp.Disconnect()
            sftp.Dispose()


        End Sub

        ''' <summary>
        ''' Upload a local file to the FTP server
        ''' </summary>
        ''' <param name="fi">Source file</param>
        ''' <param name="targetFilename">Target filename (optional)</param>
        Public Sub Upload(ByVal fi As FileInfo, Optional ByVal targetFilename As String = "", Optional ByVal appendToFile As Boolean = False)
            'copy the file specified to target file: target file can be full path or just filename (uses current dir)


            Dim tmpValue As String
            Dim targetTemp As String
            Dim fileExists As Boolean
            Dim targetDir As String
            Dim fileName As String

            fileExists = False


            tmpValue = "IRMATEMP_" + System.Guid.NewGuid.ToString() + ".DAT"
            fileName = GetFullName(targetFilename)
            targetDir = GetTargetPath(targetFilename)
            targetTemp = targetDir + tmpValue

            Dim sftp As Jscape.Sftp.Sftp = Nothing
            CreateFtpConnection(sftp)
            'This set the mode to binary
            sftp.SetBinaryMode()

            'If append to file is false, then the file is uploaded as a temp file and then renamed to the original filename
            If appendToFile = False Then
                If targetDir.Length > 1 Then
                    sftp.RemoteDir() = targetDir
                Else
                End If

                'Gets the directory listing and loops through to see if the target file already exists
                Dim e As IEnumerator = sftp.GetDirListing()
                ' enumerate thru listing printing filename for each entry 
                While e.MoveNext()
                    Dim file As Jscape.Sftp.Packets.FileInfo = CType(e.Current, Jscape.Sftp.Packets.FileInfo)
                    If file.Name = fileName Then
                        fileExists = True
                    End If
                End While

                'If the target file exists, this will delete it to avoid conflict when we upload our new version
                If fileExists Then
                    Try
                        'Delete the file. 
                        sftp.DeleteFile(fileName)
                    Catch ex As Exception

                    End Try

                End If

                'Uploads the file with the temp name to repleace the file that was just deleted.
                sftp.Upload(fi, fileName, appendToFile)
                'ftp.Upload(fi, targetTemp, appendToFile)

                'This renames the tempfilename to the real filename once the upload is complete.
                'ftp.RenameFile(targetTemp, targetFilename)
            Else
                'changes to the correct target path passed down from the changeto field.
                If targetDir.Length > 1 Then
                    sftp.RemoteDir() = targetDir
                Else
                End If

                'Gets the directory listing and loops through to see if the target file already exists
                Dim e As IEnumerator = sftp.GetDirListing()
                ' enumerate thru listing printing filename for each entry 
                While e.MoveNext()
                    Dim file As Jscape.Sftp.Packets.FileInfo = CType(e.Current, Jscape.Sftp.Packets.FileInfo)
                    If file.Name = fileName Then
                        fileExists = True
                    End If
                End While

                'If the target file exists, this will rename it to a temp file, append the new uploaded temp file, and then
                'rename the combined temp file to the original target file.
                If fileExists Then
                    Try
                        sftp.RenameFile(fileName, tmpValue)
                    Catch ex As Exception

                    End Try

                End If
                sftp.Upload(fi, tmpValue, appendToFile)
                'This renames the tempfilename to the real filename once the upload is complete.
                sftp.RenameFile(tmpValue, fileName)

            End If

            sftp.Disconnect()
            sftp.Dispose()


        End Sub
#End Region

#Region "Download: File transfer FROM ftp serveR"
        Public Function Download(ByVal sourceFilename As String, ByVal localFilename As String, Optional ByVal PermitOverwrite As Boolean = False) As Boolean
            '2. determine target file
            Dim fi As New FileInfo(localFilename)
            ' Create the local directory if it does not already exist
            If Not fi.Directory.Exists Then
                fi.Directory.Create()
            End If
            Return Me.Download(sourceFilename, fi, PermitOverwrite)
        End Function

        'Version taking an FtpFileInfo
        Public Function Download(ByVal file As Jscape.Sftp.Packets.FileInfo, ByVal localFilename As String, Optional ByVal PermitOverwrite As Boolean = False) As Boolean
            Return Me.Download(file.LongName, localFilename, PermitOverwrite)
        End Function

        'Another version taking FtpFileInfo and FileInfo
        Public Function Download(ByVal file As Jscape.Sftp.Packets.FileInfo, ByVal localFI As FileInfo, Optional ByVal PermitOverwrite As Boolean = False) As Boolean
            ' Create the local directory if it does not already exist
            If Not localFI.Directory.Exists Then
                localFI.Directory.Create()
            End If
            Return Me.Download(file.LongName, localFI, PermitOverwrite)
        End Function

        'Download to a MemoryStream. Initially only supported on a secureFTP connection.
        Public Function Download(ByVal source As String, ByRef mem As MemoryStream, Optional ByVal PermitOverwrite As Boolean = False) As Boolean


            Dim target As String = String.Empty

            'If source.Trim = "" Then
            '    Throw New ApplicationException("File not specified")
            'ElseIf source.Contains("/") Then
            '    'treat as a full path
            '    target = AdjustDir(source)
            'Else
            '    'treat as filename only, use current directory
            '    target = CurrentDirectory & source
            'End If


            'Dim URI As String = HostName & target

            ''3. perform copy
            'Dim ftp As Net.FtpWebRequest = GetRequest(URI)

            ''Set request to download a file in binary mode
            'ftp.Method = Net.WebRequestMethods.Ftp.DownloadFile
            'ftp.UseBinary = True

            ''open request and get response stream
            'Using response As FtpWebResponse = CType(ftp.GetResponse, FtpWebResponse)

            '    Using responseStream As Stream = response.GetResponseStream
            '        If Not mem Is Nothing Then mem.Dispose()
            '        mem = Nothing
            '        If mem Is Nothing Then mem = New MemoryStream()

            '        'loop to read & write to file
            '        Try
            '            Dim buffer(4096) As Byte
            '            Dim read As Integer = 0
            '            Do
            '                read = responseStream.Read(buffer, 0, buffer.Length)
            '                mem.Write(buffer, 0, read)
            '            Loop Until read = 0
            '            responseStream.Close()
            '            mem.Flush()
            '            '.Close()
            '        Catch ex As Exception
            '            'catch error and delete file only partially downloaded
            '            mem.Close()
            '            'delete target file as it's incomplete
            '            'targetFI.Delete()
            '            Throw
            '        End Try

            '        responseStream.Close()
            '    End Using
            '    response.Close()
            'End Using


            'Else
            Dim sftp As Jscape.Sftp.Sftp = Nothing
            CreateFtpConnection(sftp)
            If Not mem Is Nothing Then mem.Dispose()
            If mem Is Nothing Then mem = New MemoryStream()
            sftp.Download(mem, source)

            If sftp.IsConnected Then sftp.Disconnect()
            sftp.Dispose()


        End Function

        'Version taking string/FileInfo
        Public Function Download(ByVal sourceFilename As String, ByVal targetFI As FileInfo, Optional ByVal PermitOverwrite As Boolean = False) As Boolean
            '1. check target
            If targetFI.Exists And Not (PermitOverwrite) Then Throw New ApplicationException("Target file already exists")

            '2. check source
            Dim target As String
            If sourceFilename.Trim = "" Then
                Throw New ApplicationException("File not specified")
            ElseIf sourceFilename.Contains("/") Then
                'treat as a full path
                target = AdjustDir(sourceFilename)
            Else
                'treat as filename only, use current directory
                target = CurrentDirectory & sourceFilename
            End If

            'declare FTP object - use Jscape library
            Dim sftp As Jscape.Sftp.Sftp = Nothing

            'create secure connection
            CreateFtpConnection(sftp)

            'save data to memory buffer before writing to a file
            Dim mem As System.IO.MemoryStream = New System.IO.MemoryStream()
            sftp.Download(mem, target)

            SaveFileToDisk(targetFI.FullName, mem.GetBuffer())
            If sftp.IsConnected Then sftp.Disconnect()

            sftp.Dispose()
            mem.Dispose()

            Return True
        End Function
#End Region

#Region "Other Functions: Delete rename etc."
        Public Function FtpDelete(ByVal filename As String) As Boolean
            'Determine if file or full path


            Dim sftp As Jscape.Sftp.Sftp = Nothing
            CreateFtpConnection(sftp)
            Try
                sftp.DeleteFile(filename)
            Catch ex As Exception
                Return False
            Finally
                sftp.Disconnect()
                sftp.Dispose()
            End Try
            Return True
        End Function

        ''' <summary>
        ''' Determine if file exists on remote FTP site
        ''' </summary>
        ''' <param name="filename">Filename (for current dir) or full path</param>
        ''' <returns></returns>
        ''' <remarks>Note this only works for files</remarks>
        Public Function FtpFileExists(ByVal filename As String) As Boolean
            'Try to obtain filesize: if we get error msg containing "550"
            'the file does not exist
            Dim retval As Boolean = False

            Dim sftp As Jscape.Sftp.Sftp = Nothing
            CreateFtpConnection(sftp)
            Try
                sftp.GetFileSize(filename)
                retval = True
            Catch ex As Exception
                retval = False
            Finally
                sftp.Disconnect()
                sftp.Dispose()
            End Try


            Return retval
        End Function

        ''' <summary>
        ''' Determine size of remote file
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <returns></returns>
        ''' <remarks>Throws an exception if file does not exist</remarks>
        Public Function GetFileSize(ByVal filename As String) As Long


            Dim path As String
            Dim retval As Long
            If filename.Contains("/") Then
                path = AdjustDir(filename)
            Else
                path = Me.CurrentDirectory & filename
            End If

            Dim sftp As Jscape.Sftp.Sftp = Nothing
            CreateFtpConnection(sftp)
            retval = sftp.GetFileSize(filename)
            sftp.Disconnect()
            sftp.Dispose()
            Return retval
        End Function

        Public Function FtpRename(ByVal sourceFilename As String, ByVal newName As String) As Boolean
            Dim retval As Boolean = False
            Dim source As String = GetFullPath(sourceFilename)

            'Does file exist?
            If Not FtpFileExists(source) Then
                Throw New FileNotFoundException("File " & source & " not found")
            End If

            'build target name, ensure it does not exist
            Dim target As String = GetFullPath(newName)
            If target = source Then
                Throw New ApplicationException("Source and target are the same")
            ElseIf FtpFileExists(target) Then
                Throw New ApplicationException("Target file " & target & " already exists")
            End If

            Dim sftp As Jscape.Sftp.Sftp = Nothing


            CreateFtpConnection(sftp)

            'perform rename
            Try
                sftp.RenameFile(sourceFilename, newName)
                retval = True
            Catch ex As Exception
                retval = False
            End Try

            Return retval

        End Function

        Public Function FtpCreateDirectory(ByVal dirpath As String) As Boolean
            'perform create

            Dim sftp As Jscape.Sftp.Sftp = Nothing
            CreateFtpConnection(sftp)
            Try
                sftp.MakeDir(dirpath)
            Catch ex As Exception
                Return False
            Finally
                sftp.Disconnect()
                sftp.Dispose()
            End Try
            Return True
        End Function

        Public Function FtpDeleteDirectory(ByVal dirpath As String) As Boolean
            'perform remove


            Dim sftp As Jscape.Sftp.Sftp = Nothing
            CreateFtpConnection(sftp)
            Try
                sftp.DeleteDir(dirpath, False)
            Catch ex As Exception
                Return False
            Finally
                sftp.Disconnect()
                sftp.Dispose()
            End Try
            Return True
        End Function

#End Region
        Public Sub Connect()
            If Not _ftpClient.IsConnected Then
                _ftpClient.Connect()

            End If
        End Sub

        Public Sub Disconnect()
            If _ftpClient.IsConnected Then
                _ftpClient.Disconnect()
            End If
        End Sub

        Sub Dispose() Implements IDisposable.Dispose
            If Not _ftpClient Is Nothing Then
                If _ftpClient.IsConnected Then
                    _ftpClient.Disconnect()
                    _ftpClient.Dispose()
                End If
            End If
        End Sub


        Public Function GetFullName(ByVal Full As String) As String
            Return Full.Substring(GetTargetPath(Full).Length) 'Cut off everything up to the path
        End Function

        ''' <summary>
        ''' returns a full path using CurrentDirectory for a relative file reference
        ''' Retrieves the path part of a full filename
        ''' C:\Folder\file.exe -> C:\Folder\
        ''' </summary>
        Public Function GetTargetPath(ByVal Full As String) As String
            For i As Integer = Full.Length - 1 To 0 Step -1
                If Full.Substring(i, 1) = "\" OrElse Full.Substring(i, 1) = "/" Then 'Find the rightmost \ or /, which should be cut off the file part
                    Return Full.Substring(0, i) & "/"
                End If
            Next
            Return ""
        End Function
        Private Function AdjustDir(ByVal path As String) As String
            Return CStr(IIf(path.StartsWith("/"), "", "/")) & path
        End Function

        Private _currentDirectory As String = "/"
        Public Property CurrentDirectory() As String
            Get
                'return directory, ensure it ends with /
                Return _currentDirectory & CStr(IIf(_currentDirectory.EndsWith("/"), "", "/"))
            End Get
            Set(ByVal value As String)
                If Not value.StartsWith("/") Then Throw New ApplicationException("Directory should start with /")
                _currentDirectory = value
            End Set
        End Property

        Protected Sub throwException(ByVal message As String, Optional ByVal innerException As Exception = Nothing)
            Dim newException As Jscape.Sftp.SftpException
            If innerException IsNot Nothing Then
                Logger.LogError(message, Me.GetType(), innerException)
                newException = New Jscape.Sftp.SftpException(message, innerException)
            Else
                Logger.LogError(message, Me.GetType())
                newException = New Jscape.Sftp.SftpException(message)
            End If

            ' Throw the exception
            Throw newException
        End Sub

        Public Sub SaveFileToDisk(ByVal FullPath As String, ByVal data As Byte())

            ' Delete the file if it already exists
            If File.Exists(FullPath) Then File.Delete(FullPath)
            ' Open the file
            File.WriteAllBytes(FullPath, data)
            ' Close the file
        End Sub
        Private Function GetFullPath(ByVal file As String) As String
            If file.Contains("/") Then
                Return AdjustDir(file)
            Else
                Return Me.CurrentDirectory & file
            End If
        End Function
    End Class


End Namespace
