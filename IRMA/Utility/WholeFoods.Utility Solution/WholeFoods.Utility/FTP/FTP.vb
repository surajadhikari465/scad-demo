Option Explicit On
Option Strict On

Imports System.Collections.Generic
Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions
Imports Jscape.Ftp


Namespace WholeFoods.Utility.FTP

#Region "FTP client class"
    ''' <summary>
    ''' A wrapper class for .NET 2.0 FTP
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public Class FTPclient
        '   Public fileinfo As String

#Region "CONSTRUCTORS"
        ''' <summary>
        ''' Blank constructor
        ''' </summary>
        ''' <remarks>Hostname, username and password must be set manually</remarks>
        Sub New(Optional ByVal SecureFtp As Boolean = False, Optional ByVal FTPPort As Integer = 21)
            _secureftp = SecureFtp
            _port = FTPPort

        End Sub

        ''' <summary>
        ''' Constructor just taking the hostname
        ''' </summary>
        ''' <param name="Hostname">in either ftp://ftp.host.com or ftp.host.com form</param>
        ''' <remarks></remarks>
        Sub New(ByVal Hostname As String, Optional ByVal SecureFtp As Boolean = False, Optional ByVal FTPPort As Integer = 21)
            _secureftp = SecureFtp
            _port = FTPPort
            _hostname = Hostname
        End Sub

        ''' <summary>
        ''' Constructor taking hostname, username and password
        ''' </summary>
        ''' <param name="Hostname">in either ftp://ftp.host.com or ftp.host.com form</param>
        ''' <param name="Username">Leave blank to use 'anonymous' but set password to your email</param>
        ''' <param name="Password"></param>
        ''' <remarks></remarks>
        Sub New(ByVal Hostname As String, ByVal Username As String, ByVal Password As String, Optional ByVal SecureFtp As Boolean = False, Optional ByVal FTPPort As Integer = 21)
            _secureftp = SecureFtp
            _hostname = Hostname
            _username = Username
            _password = Password
            _port = FTPPort
        End Sub
#End Region

#Region "Directory functions"
        ''' <summary>
        ''' Return a simple directory listing
        ''' </summary>
        ''' <param name="directory">Directory to list, e.g. /pub</param>
        ''' <returns>A list of filenames and directories as a List(of String)</returns>
        ''' <remarks>For a detailed directory listing, use ListDirectoryDetail</remarks>
        Public Function ListDirectory(Optional ByVal directory As String = "") As List(Of String)
            'return a simple list of filenames in directory

            Dim result As New List(Of String)
            If _secureftp Then
                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateSecureFtp(ftp)
                For Each item As String In ftp.GetNameListing(directory)
                    result.Add(item)
                Next

                ftp.Disconnect()
                ftp.Dispose()
            Else

                Dim ftp As Net.FtpWebRequest = GetRequest(GetDirectory(directory))
                'Set request to do simple list
                ftp.Method = Net.WebRequestMethods.Ftp.ListDirectory

                Dim str As String = GetStringResponse(ftp)
                'replace CRLF to CR, remove last instance
                str = str.Replace(vbCrLf, vbCr).TrimEnd(Chr(13))
                'split the string into a list

                result.AddRange(str.Split(Chr(13)))
            End If
            Return result
        End Function

        ''' <summary>
        ''' Return a detailed directory listing
        ''' </summary>
        ''' <param name="directory">Directory to list, e.g. /pub/etc</param>
        ''' <returns>An FTPDirectory object</returns>
        Public Function ListDirectoryDetail(Optional ByVal directory As String = "") As FTPdirectory
            If _secureftp Then
                Throw New Exception("ListDirectoryDetail is not support by SecureFTP connections at this time")
            End If
            Dim ftp As Net.FtpWebRequest = GetRequest(GetDirectory(directory))
            'Set request to do simple list
            ftp.Method = Net.WebRequestMethods.Ftp.ListDirectoryDetails

            Dim str As String = GetStringResponse(ftp)
            'replace CRLF to CR, remove last instance
            str = str.Replace(vbCrLf, vbCr).TrimEnd(Chr(13))
            'split the string into a list
            Return New FTPdirectory(str, _lastDirectory)
        End Function

#End Region

#Region "Issue Command on FTP server"
        'Public Function CreateCommandConnection(ByVal Command As String) As Boolean

        '    Dim response As String = String.Empty
        '    Dim ftp As Jscape.Ftp.Ftp = Nothing

        '    If _secureftp Then
        '        CreateSecureFtp(ftp)
        '    Else
        '        CreateNonSecureFtp(ftp)
        '    End If

        'End Function

        ''' <summary>
        ''' 	Issues a command to the FTP server.
        ''' </summary>
        ''' <param name="Command">Command to run on the server</param>
        ''' <remarks>The command syntax is the same as if using the "quote"
        '''  command on the FTP server.</remarks>
        Public Function IssueCommand(ByVal Command As String) As String

            Dim response As String = String.Empty
            Dim ftp As Jscape.Ftp.Ftp = Nothing

            If Command.Length = 0 Then
                response = "'Command' parameter was not specified!"
            Else
                If _secureftp Then
                    CreateFtpConnection(ftp, True)
                Else
                    CreateFtpConnection(ftp, False)
                End If

                If ftp.Connected Then
                    Try
                        response = ftp.IssueCommand(Command)

                        If Len(response) <> 0 Then
                            Debug.Print("FTP.IssueCommand(""" & Command & """) return value: " & response)
                        End If

                    Catch ex As Exception
                        response = ex.Message
                        Logger.LogError(String.Format("FTP.IssueCommand(""{0}"") return value: {1}", Command, response), Me.GetType)

                    Finally
                        ftp.Disconnect()
                        ftp.Dispose()

                    End Try
                End If
            End If

            Return response

        End Function

#End Region

#Region "Upload: File transfer TO ftp server"
        ''' <summary>
        ''' Copy a local file to the FTP server
        ''' </summary>
        ''' <param name="localFilename">Full path of the local file</param>
        ''' <param name="targetFilename">Target filename, if required</param>
        ''' <remarks>If the target filename is blank, the source filename is used
        ''' (assumes current directory). Otherwise use a filename to specify a name
        ''' or a full path and filename if required.</remarks>
        ''' 
        Public Sub Upload(ByVal localFilename As String, ByVal targetFilename As String, Optional ByVal appendToFile As Boolean = False)
            '1. check source
            If Not File.Exists(localFilename) Then
                throwException("File " & localFilename & " not found")
            End If
            'copy to FI
            Dim fi As New FileInfo(localFilename)
            Upload(fi, targetFilename, appendToFile)
        End Sub

        Public Sub Upload(ByVal localFilename As String, ByVal DC As Boolean)
            '1. check source
            If Not File.Exists(localFilename) Then
                throwException("File " & localFilename & " not found")
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


        ''' <summary>
        ''' This is a temporary hack to deal specifically with dataconversion. This needs to be removed and rewritten during the next release
        ''' </summary>
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

            If _secureftp Then
                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateSecureFtp(ftp)
                'This uploads the file with the _tmp extention.
                ftp.Upload(fi, targetTemp, appendToFile)
                'This renames the tempfilename to the real filename once the upload is complete.
                ftp.RenameFile(targetTemp, targetFilename)
                ftp.Disconnect()
                ftp.Dispose()

            Else

                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateFtp(ftp)
                'This set the mode to binary
                ftp.TransferMode = Jscape.Ftp.TransferModes.Auto
                'If append to file is false, then the file is uploaded as a temp file and then renamed to the original filename
                If appendToFile = False Then
                    'Uploads the file with the temp name
                    ftp.Upload(fi, targetTemp, appendToFile)
                    'This renames the tempfilename to the real filename once the upload is complete.
                    'ftp.RenameFile(targetTemp, targetFilename)
                Else
                    'changes to the correct target path passed down from the changeto field.
                    If targetDir.Length > 1 Then
                        ftp.RemoteDir() = targetDir
                    Else
                    End If

                    'Gets the directory listing and loops through to see if the target file already exists
                    Dim e As IEnumerator = ftp.GetDirListing()
                    ' enumerate thru listing printing filename for each entry 
                    While e.MoveNext()
                        Dim file As FtpFile = CType(e.Current, FtpFile)
                        If file.Filename = fileName Then
                            fileExists = True
                        End If
                    End While

                    'If the target file exists, this will rename it to a temp file, append the new uploaded temp file, and then
                    'rename the combined temp file to the original target file.
                    If fileExists Then
                        Try
                            ftp.RenameFile(fileName, fileName + tmpValue)
                        Catch ex As Exception

                        End Try

                    End If
                    ftp.Upload(fi, fileName + tmpValue, appendToFile)
                    'This renames the tempfilename to the real filename once the upload is complete.
                    ftp.RenameFile(fileName + tmpValue, fileName)

                End If

                ftp.Disconnect()
                ftp.Dispose()

            End If
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

            If _secureftp Then
                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateSecureFtp(ftp)

                'This uploads the file with the _tmp extention.
                ftp.Upload(fi, targetTemp, appendToFile)
                'This renames the tempfilename to the real filename once the upload is complete.
                ftp.RenameFile(targetTemp, targetFilename)
                ftp.Disconnect()
                ftp.Dispose()

            Else
                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateFtp(ftp)
                'This set the mode to binary
                ftp.TransferMode = Jscape.Ftp.TransferModes.Auto
                'If append to file is false, then the file is uploaded as a temp file and then renamed to the original filename
                If appendToFile = False Then
                    If targetDir.Length > 1 Then
                        ftp.RemoteDir() = targetDir
                    Else
                    End If

                    'Gets the directory listing and loops through to see if the target file already exists
                    Dim e As IEnumerator = ftp.GetDirListing()
                    ' enumerate thru listing printing filename for each entry 
                    While e.MoveNext()
                        Dim file As FtpFile = CType(e.Current, FtpFile)
                        If file.Filename = fileName Then
                            fileExists = True
                        End If
                    End While

                    'If the target file exists, this will delete it to avoid conflict when we upload our new version
                    If fileExists Then
                        Try
                            'Delete the file. 
                            ftp.DeleteFile(fileName)
                        Catch ex As Exception

                        End Try

                    End If

                    'Uploads the file with the temp name to repleace the file that was just deleted.
                    ftp.Upload(fi, fileName, appendToFile)
                    'ftp.Upload(fi, targetTemp, appendToFile)

                    'This renames the tempfilename to the real filename once the upload is complete.
                    'ftp.RenameFile(targetTemp, targetFilename)
                Else
                    'changes to the correct target path passed down from the changeto field.
                    If targetDir.Length > 1 Then
                        ftp.RemoteDir() = targetDir
                    Else
                    End If

                    'Gets the directory listing and loops through to see if the target file already exists
                    Dim e As IEnumerator = ftp.GetDirListing()
                    ' enumerate thru listing printing filename for each entry 
                    While e.MoveNext()
                        Dim file As FtpFile = CType(e.Current, FtpFile)
                        If file.Filename = fileName Then
                            fileExists = True
                        End If
                    End While

                    'If the target file exists, this will rename it to a temp file, append the new uploaded temp file, and then
                    'rename the combined temp file to the original target file.
                    If fileExists Then
                        Try
                            ftp.RenameFile(fileName, tmpValue)
                        Catch ex As Exception

                        End Try

                    End If
                    ftp.Upload(fi, tmpValue, appendToFile)
                    'This renames the tempfilename to the real filename once the upload is complete.
                    ftp.RenameFile(tmpValue, fileName)

                End If

                ftp.Disconnect()
                ftp.Dispose()

            End If
        End Sub
#End Region

        Private Sub CreateSecureFtp(ByRef ftp As Jscape.Ftp.Ftp)
            'creates a new jscape secure ftp object and returns the value by reference.
            'this code was placed in this sub to make for easy licensekey management.
            'ftp = New Jscape.Ftp.Ftp
            'ftp.LicenseKey = "Secure FTP Factory for .NET:Enterprise Developer:Registered User:01-01-3999:UxVOwssHXYfFJ0TseU/xjy8ZOqRSmU/z/4YIyJwGB5KFT9t/HCWwEtUzHmAc99c253JGOrj9wwuWOZ+zbYnuL6Mc0lkAhZ42QZlP8L5uYfVDNpUqpRNG04JC6adapNvdURgj98qGL0vbIbqgQIK9yGxB2mmdsWTlgosx8SxPloI="
            CreateFtpConnection(ftp, True)
        End Sub

        Private Sub CreateFtp(ByRef ftp As Jscape.Ftp.Ftp)
            'creates a new jscape secure ftp object and returns the value by reference.
            'this code was placed in this sub to make for easy licensekey management.
            ftp = New Jscape.Ftp.Ftp
            ftp.LicenseKey = "Secure FTP Factory for .NET:Enterprise Developer:Registered User:01-01-3999:UxVOwssHXYfFJ0TseU/xjy8ZOqRSmU/z/4YIyJwGB5KFT9t/HCWwEtUzHmAc99c253JGOrj9wwuWOZ+zbYnuL6Mc0lkAhZ42QZlP8L5uYfVDNpUqpRNG04JC6adapNvdURgj98qGL0vbIbqgQIK9yGxB2mmdsWTlgosx8SxPloI="

            'Trial Key
            'ftp.LicenseKey = "Secure FTP Factory for .NET:Evaluation:Evaluation:09-20-2006:IdDwFOvrlVgQkVy7nOKU8+WIkkD/AB+ex8clnMKFDcjm+Ue3gR2QnXY58r2B2WtbIogxHSHiqlZYEgv+kUPkfpXj1jaTyCaHazZ5xHr3wAKQi7iV0xkSVpnE8PZC0xfPkJk1kdhhk0AG2QglAgK1h41I/UPcDfck42cC1p9yD1Q="
            ftp.ConnectionType = Jscape.Ftp.Ftp.DEFAULT
            ftp.HostName = _hostname
            ftp.User = _username
            ftp.Password = _password
            ftp.Port = _port
            ftp.Connect()
        End Sub

        Private Function CreateFtpConnection(ByRef ftp As Jscape.Ftp.Ftp, Optional ByVal SecureConnection As Boolean = False) As Boolean
            'creates a new jscape secure/nonsecure ftp object and returns the value by reference.
            'this code was placed in this sub to make for easy licensekey management.
            ftp = New Jscape.Ftp.Ftp
            'ftp = New Jscape.Ftp.Ftp(_hostname, _username, _password, _port)

            ftp.LicenseKey = "Secure FTP Factory for .NET:Enterprise Developer:Registered User:01-01-3999:UxVOwssHXYfFJ0TseU/xjy8ZOqRSmU/z/4YIyJwGB5KFT9t/HCWwEtUzHmAc99c253JGOrj9wwuWOZ+zbYnuL6Mc0lkAhZ42QZlP8L5uYfVDNpUqpRNG04JC6adapNvdURgj98qGL0vbIbqgQIK9yGxB2mmdsWTlgosx8SxPloI="
            'Trial Key
            'ftp.LicenseKey = "Secure FTP Factory for .NET:Evaluation:Evaluation:09-20-2006:IdDwFOvrlVgQkVy7nOKU8+WIkkD/AB+ex8clnMKFDcjm+Ue3gR2QnXY58r2B2WtbIogxHSHiqlZYEgv+kUPkfpXj1jaTyCaHazZ5xHr3wAKQi7iV0xkSVpnE8PZC0xfPkJk1kdhhk0AG2QglAgK1h41I/UPcDfck42cC1p9yD1Q="

            If SecureConnection Then
                ftp.ConnectionType = Jscape.Ftp.Ftp.AUTH_TLS
            End If
            ftp.HostName = _hostname
            ftp.User = _username
            ftp.Password = _password
            ftp.Port = _port

            Dim iAttempts As Integer = 1
            Dim sError As String = String.Empty

            Do While (iAttempts <= 3 AndAlso Not ftp.Connected)
                sError = String.Empty
                Try
                    ftp.Connect()
                Catch ex1 As Jscape.Ftp.FtpException
                    sError = String.Format("FtpException: {0}", ex1.Message)
                Catch ex As Exception
                    sError = String.Format("Exception: {0}", ex.Message)
                Finally
                    If Not sError.Equals(String.Empty) Then
                        Logger.LogError(String.Format("FTP.CreateFtpConnection() Error: " & sError & " (attempt {0})", iAttempts.ToString), Me.GetType)
                    End If
                    iAttempts += 1
                End Try
            Loop

            If Not ftp.Connected Then
                sError = String.Format("Unable to connect to FTP (Host = '{0}', User = '{1}', Port = {2}).{3}{4}", ftp.HostName, ftp.User, ftp.Port, vbCrLf, sError)
                Throw New FTPException(sError)
            End If

            Return ftp.Connected()

        End Function


#Region "Download: File transfer FROM ftp server"
        ''' <summary>
        ''' Copy a file from FTP server to local
        ''' </summary>
        ''' <param name="sourceFilename">Target filename, if required</param>
        ''' <param name="localFilename">Full path of the local file</param>
        ''' <returns></returns>
        ''' <remarks>Target can be blank (use same filename), or just a filename
        ''' (assumes current directory) or a full path and filename</remarks>
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
        Public Function Download(ByVal file As FTPfileInfo, ByVal localFilename As String, Optional ByVal PermitOverwrite As Boolean = False) As Boolean
            Return Me.Download(file.FullName, localFilename, PermitOverwrite)
        End Function

        'Another version taking FtpFileInfo and FileInfo
        Public Function Download(ByVal file As FTPfileInfo, ByVal localFI As FileInfo, Optional ByVal PermitOverwrite As Boolean = False) As Boolean
            ' Create the local directory if it does not already exist
            If Not localFI.Directory.Exists Then
                localFI.Directory.Create()
            End If
            Return Me.Download(file.FullName, localFI, PermitOverwrite)
        End Function

        'Download to a MemoryStream. Initially only supported on a secureFTP connection.
        Public Function Download(ByVal source As String, ByRef mem As MemoryStream, Optional ByVal PermitOverwrite As Boolean = False) As Boolean
            If Not _secureftp Then
                'Throw New Exception("Downloading to memory is only supported on Secure FTP (SFTP) connections at this time.")
                Dim target As String
                If source.Trim = "" Then
                    Throw New ApplicationException("File not specified")
                ElseIf source.Contains("/") Then
                    'treat as a full path
                    target = AdjustDir(source)
                Else
                    'treat as filename only, use current directory
                    target = CurrentDirectory & source
                End If


                Dim URI As String = Hostname & target

                '3. perform copy
                Dim ftp As Net.FtpWebRequest = GetRequest(URI)

                'Set request to download a file in binary mode
                ftp.Method = Net.WebRequestMethods.Ftp.DownloadFile
                ftp.UseBinary = True

                'open request and get response stream
                Using response As FtpWebResponse = CType(ftp.GetResponse, FtpWebResponse)

                    Using responseStream As Stream = response.GetResponseStream
                        If Not mem Is Nothing Then mem.Dispose()
                        mem = Nothing
                        If mem Is Nothing Then mem = New MemoryStream()

                        'loop to read & write to file
                        Try
                            Dim buffer(4096) As Byte
                            Dim read As Integer = 0
                            Do
                                read = responseStream.Read(buffer, 0, buffer.Length)
                                mem.Write(buffer, 0, read)
                            Loop Until read = 0
                            responseStream.Close()
                            mem.Flush()
                            '.Close()
                        Catch ex As Exception
                            'catch error and delete file only partially downloaded
                            mem.Close()
                            'delete target file as it's incomplete
                            'targetFI.Delete()
                            Throw
                        End Try

                        responseStream.Close()
                    End Using
                    response.Close()
                End Using


            Else
                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateSecureFtp(ftp)
                ' resize the memory allocation.
                If Not mem Is Nothing Then mem.Dispose()
                If mem Is Nothing Then mem = New MemoryStream(CInt(ftp.GetFileSize(source)))
                ftp.Download(mem, source)
                ftp.DataConnection.Close()
                If ftp.IsConnected Then ftp.Disconnect()
                ftp.Dispose()

            End If
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
            Dim ftp As Jscape.Ftp.Ftp = Nothing

            If _secureftp Then
                'create secure connection
                CreateSecureFtp(ftp)

                'save data to memory buffer before writing to a file
                Dim mem As System.IO.MemoryStream = New System.IO.MemoryStream(CInt(ftp.GetFileSize(target)))
                ftp.Download(mem, target)
                ftp.DataConnection.Close()

                SaveFileToDisk(targetFI.FullName, mem.GetBuffer())
                If ftp.IsConnected Then ftp.Disconnect()

                ftp.Dispose()
                mem.Dispose()
            Else
                'create non-secure connection
                CreateFtp(ftp)

                ftp.LocalDir = targetFI.DirectoryName
                'write directly to local file from remote file
                ftp.Download(targetFI.Name, ftp.RemoteDir + target)
            End If

            Return True
        End Function
#End Region

#Region "Other functions: Delete rename etc."
        ''' <summary>
        ''' Delete remote file
        ''' </summary>
        ''' <param name="filename">filename or full path</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FtpDelete(ByVal filename As String) As Boolean
            'Determine if file or full path

            If _secureftp Then
                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateSecureFtp(ftp)
                Try
                    ftp.DeleteFile(filename)
                Catch ex As Exception
                    Return False
                Finally
                    ftp.Disconnect()
                    ftp.Dispose()
                End Try
            Else



                Dim URI As String = Me.Hostname & GetFullPath(filename)

                Dim ftp As Net.FtpWebRequest = GetRequest(URI)
                'Set request to delete
                ftp.Method = Net.WebRequestMethods.Ftp.DeleteFile
                Try
                    'get response but ignore it
                    Dim str As String = GetStringResponse(ftp)
                Catch ex As Exception
                    Return False
                End Try
            End If
            Return True
        End Function
        Public Sub SaveFileToDisk(ByVal FullPath As String, ByVal data As Byte())

            ' Delete the file if it already exists
            If File.Exists(FullPath) Then File.Delete(FullPath)
            ' Open the file
            File.WriteAllBytes(FullPath, data)
            ' Close the file
        End Sub
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
            If _secureftp Then
                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateSecureFtp(ftp)
                Try
                    ftp.GetFileSize(filename)
                    retval = True
                Catch ex As Exception
                    retval = False
                Finally
                    ftp.Disconnect()
                    ftp.Dispose()
                End Try



            Else

                Try
                    Dim size As Long = GetFileSize(filename)
                    retval = True

                Catch ex As Exception
                    'only handle expected not-found exception
                    If TypeOf ex Is System.Net.WebException Then
                        'file does not exist/no rights error = 550
                        If ex.Message.Contains("550") Then
                            'clear 
                            retval = False
                        Else
                            Throw
                        End If
                    Else
                        Throw
                    End If
                End Try
            End If
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
            If _secureftp Then
                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateSecureFtp(ftp)
                retval = ftp.GetFileSize(filename)
                ftp.Disconnect()
                ftp.Dispose()
            Else
                Dim URI As String = Me.Hostname & path
                Dim ftp As Net.FtpWebRequest = GetRequest(URI)
                'Try to get info on file/dir?
                ftp.Method = Net.WebRequestMethods.Ftp.GetFileSize
                Dim tmp As String = Me.GetStringResponse(ftp)
                retval = GetSize(ftp)
            End If
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

            Dim ftp As Jscape.Ftp.Ftp = Nothing

            If _secureftp Then
                CreateSecureFtp(ftp)
            Else
                CreateFtp(ftp)
            End If

            'perform rename
            Try
                ftp.RenameFile(sourceFilename, newName)
                retval = True
            Catch ex As Exception
                retval = False
            End Try

            Return retval

        End Function

        Public Function FtpCreateDirectory(ByVal dirpath As String) As Boolean
            'perform create
            If _secureftp Then
                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateSecureFtp(ftp)
                Try
                    ftp.MakeDir(dirpath)
                Catch ex As Exception
                    Return False
                Finally
                    ftp.Disconnect()
                    ftp.Dispose()
                End Try
            Else

                Dim URI As String = Me.Hostname & AdjustDir(dirpath)
                Dim ftp As Net.FtpWebRequest = GetRequest(URI)
                'Set request to MkDir
                ftp.Method = Net.WebRequestMethods.Ftp.MakeDirectory
                Try
                    'get response but ignore it
                    Dim str As String = GetStringResponse(ftp)
                Catch ex As Exception
                    Return False
                End Try
            End If
            Return True
        End Function

        Public Function FtpDeleteDirectory(ByVal dirpath As String) As Boolean
            'perform remove

            If _secureftp Then
                Dim ftp As Jscape.Ftp.Ftp = Nothing
                CreateSecureFtp(ftp)
                Try
                    ftp.DeleteDir(dirpath, False)
                Catch ex As Exception
                    Return False
                Finally
                    ftp.Disconnect()
                    ftp.Dispose()
                End Try
            Else
                Dim URI As String = Me.Hostname & AdjustDir(dirpath)
                Dim ftp As Net.FtpWebRequest = GetRequest(URI)
                'Set request to RmDir
                ftp.Method = Net.WebRequestMethods.Ftp.RemoveDirectory
                Try
                    'get response but ignore it
                    Dim str As String = GetStringResponse(ftp)
                Catch ex As Exception
                    Return False
                End Try
            End If
            Return True
        End Function

        ''' <summary>
        ''' Log an error and throw a new FTPException.
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="innerException"></param>
        ''' <remarks></remarks>
        Protected Sub throwException(ByVal message As String, Optional ByVal innerException As Exception = Nothing)
            Dim newException As FTPException
            If innerException IsNot Nothing Then
                Logger.LogError(message, Me.GetType(), innerException)
                newException = New FTPException(message, innerException)
            Else
                Logger.LogError(message, Me.GetType())
                newException = New FTPException(message)
            End If

            ' Throw the exception
            Throw newException
        End Sub


#End Region

#Region "private supporting fns"
        'Get the basic FtpWebRequest object with the
        'common settings and security
        Private Function GetRequest(ByVal URI As String) As FtpWebRequest
            'create request
            Dim result As FtpWebRequest = CType(FtpWebRequest.Create(URI), FtpWebRequest)
            'Set the login details
            result.Credentials = GetCredentials()
            'keep state alive (stateful mode, although this does not seem to work the way i thought)
            result.KeepAlive = True
            Return result

        End Function


        ''' <summary>
        ''' Get the credentials from username/password
        ''' </summary>
        Private Function GetCredentials() As Net.ICredentials
            Return New Net.NetworkCredential(Username, Password)
        End Function

        ''' <summary>
        ''' returns a full path using CurrentDirectory for a relative file reference
        ''' </summary>
        Private Function GetFullPath(ByVal file As String) As String
            If file.Contains("/") Then
                Return AdjustDir(file)
            Else
                Return Me.CurrentDirectory & file
            End If
        End Function

        ''' <summary>
        ''' Amend an FTP path so that it always starts with /
        ''' </summary>
        ''' <param name="path">Path to adjust</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function AdjustDir(ByVal path As String) As String
            Return CStr(IIf(path.StartsWith("/"), "", "/")) & path
        End Function

        Private Function GetDirectory(Optional ByVal directory As String = "") As String
            Dim URI As String
            If directory = "" Then
                'build from current
                URI = Hostname & Me.CurrentDirectory
                _lastDirectory = Me.CurrentDirectory
            Else
                If Not directory.StartsWith("/") Then Throw New ApplicationException("Directory should start with /")
                URI = Me.Hostname & directory
                _lastDirectory = directory
            End If
            Return URI
        End Function

        'stores last retrieved/set directory
        Private _lastDirectory As String = ""

        ''' <summary>
        ''' Obtains a response stream as a string
        ''' </summary>
        ''' <param name="ftp">current FTP request</param>
        ''' <returns>String containing response</returns>
        ''' <remarks>FTP servers typically return strings with CR and
        ''' not CRLF. Use respons.Replace(vbCR, vbCRLF) to convert
        ''' to an MSDOS string</remarks>
        Private Function GetStringResponse(ByVal ftp As FtpWebRequest) As String
            'Get the result, streaming to a string
            Dim result As String = ""
            Using response As FtpWebResponse = CType(ftp.GetResponse, FtpWebResponse)
                Dim size As Long = response.ContentLength
                Using datastream As Stream = response.GetResponseStream
                    Using sr As New StreamReader(datastream)
                        result = sr.ReadToEnd()
                        sr.Close()
                    End Using
                    datastream.Close()
                End Using
                response.Close()
            End Using
            Return result
        End Function

        ''' <summary>
        ''' Gets the size of an FTP request
        ''' </summary>
        ''' <param name="ftp"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetSize(ByVal ftp As FtpWebRequest) As Long
            Dim size As Long

            Using response As FtpWebResponse = CType(ftp.GetResponse, FtpWebResponse)
                size = response.ContentLength
                response.Close()
            End Using
            Return size
        End Function

        'Removes path from a filename
        'C:\Folder\file.exe - > file.exe
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
#End Region

#Region "Properties"
        Private _hostname As String
        ''' <summary>
        ''' Hostname
        ''' </summary>
        ''' <value></value>
        ''' <remarks>Hostname can be in either the full URL format
        ''' ftp://ftp.myhost.com or just ftp.myhost.com
        ''' </remarks>
        Public Property Hostname() As String
            Get
                If _hostname.StartsWith("ftp://") Or _secureftp Then
                    Return _hostname
                Else
                    Return "ftp://" & _hostname & ":" & _port.ToString()
                End If


            End Get
            Set(ByVal value As String)
                _hostname = value
            End Set
        End Property
        Private _username As String
        ''' <summary>
        ''' Username property
        ''' </summary>
        ''' <value></value>
        ''' <remarks>Can be left blank, in which case 'anonymous' is returned</remarks>
        Public Property Username() As String
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
        Private _port As Integer = 21
        Public Property Port() As Integer
            Get
                Return _port
            End Get
            Set(ByVal value As Integer)
                _port = value
            End Set
        End Property
        Private _secureftp As Boolean = False
        Public Property SecureFtp() As Boolean
            Get
                Return _secureftp
            End Get
            Set(ByVal value As Boolean)
                _secureftp = True
            End Set
        End Property

        ''' <summary>
        ''' The CurrentDirectory value
        ''' </summary>
        ''' <remarks>Defaults to the root '/'</remarks>
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


#End Region

    End Class
#End Region

End Namespace
