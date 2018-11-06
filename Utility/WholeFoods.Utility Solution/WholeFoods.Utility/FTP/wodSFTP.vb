Imports System.Collections.Generic
Imports System.IO
Imports System.Net

Imports WeOnlyDo.Client.SFTP
Imports WeOnlyDo.Client.SSH
Imports WeOnlyDo.Security.Cryptography.KeyManager


Namespace WholeFoods.Utility.wodSFTP

    Public Class SFTPClient
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

        Private _isconnected As Boolean = False
        Public ReadOnly Property IsConnected() As Boolean
            Get
                Return _isconnected
            End Get

        End Property

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

        Private _ftpClient As WeOnlyDo.Client.SFTP
        Public ReadOnly Property Instance() As WeOnlyDo.Client.SFTP
            Get
                Return _ftpClient
            End Get
        End Property

        Dim WithEvents wodSftp As WeOnlyDo.Client.SFTP

#End Region

#Region "Constructors"
        Sub New(Optional ByVal Port As Integer = 22)
            _port = Port
        End Sub

        Sub New(ByVal Hostname As String, Optional ByVal Port As Integer = 22)
            _hostname = Hostname
            _port = Port
        End Sub

        Sub New(ByVal Hostname As String, ByVal Username As String, ByVal Password As String, Optional ByVal Port As Integer = 22)
            _hostname = Hostname
            _username = Username
            _password = Password
            _port = Port
        End Sub
#End Region

        'creates a new secure/nonsecure ftp object and returns the value by reference
        Private Function CreateFtpConnection(ByRef sftp As WeOnlyDo.Client.SFTP) As Boolean
            Logger.LogDebug("Create Secure FTP Connection entry", Me.GetType)
            sftp = New WeOnlyDo.Client.SFTP()

            sftp.Hostname = _hostname
            sftp.Port = _port
            sftp.Login = _username
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey

            sftp.DebugFile = "sftp_debug.txt"

            sftp.LicenseKey = "7BYU-Z8GN-SZQC-A3PX"

            sftp.Timeout = 0
            sftp.Blocking = True
            sftp.TransferMode = WeOnlyDo.Client.SFTP.TransferModes.Binary

            Dim km As WeOnlyDo.Security.Cryptography.KeyManager = New WeOnlyDo.Security.Cryptography.KeyManager()
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey
            km.Load("IRMA Private Key", "Irma12#")
            sftp.PrivateKey = km.PrivateKey(WeOnlyDo.Security.Cryptography.SSHKeyTypes.RSAKey)

            Dim sError As String = String.Empty

            Try
                sftp.Connect()

            Catch ex1 As WeOnlyDo.Exceptions.SFTP.SFTP
                sError = String.Format("SFTP Exception: {0}", ex1.Message)

            Catch ex As Exception
                sError = String.Format("Exception: {0}", ex.Message)

            Finally
                If Not sError.Equals(String.Empty) Then
                    Logger.LogError(String.Format("wodFTP.CreateFtpConnection() Error: " & sError), Me.GetType)
                End If
            End Try

            If Not sError.Equals(String.Empty) Then
                Logger.LogDebug("Create Secure FTP Connection exit (failure)", Me.GetType)
                Return False
            Else
                Logger.LogDebug("Create Secure FTP Connection exit (success)", Me.GetType)
                Return True
            End If
        End Function

        Private Sub wodSftp_ConnectedEvent(ByVal Sender As Object, ByVal Args As WeOnlyDo.Client.SFTP.ConnectedArgs) Handles wodSftp.ConnectedEvent
            'If (Args.Error Is Nothing) Then
            ' _isconnected = True
            'Else
            '_isconnected = False
            'throwException("Error: " + Args.Error.Message)
            'End If
        End Sub

        Private Sub wodSftp_DoneEvent(ByVal Sender As Object, ByVal Args As WeOnlyDo.Client.SFTP.DoneArgs) Handles wodSftp.DoneEvent

            'wodSftp.Disconnect()
        End Sub


#Region "Upload: File transfer TO ftp server"

        'copy the file specified to target file: target file can be full path or just filename (uses current dir)
        Public Overloads Sub Upload(ByVal localFilename As String, ByVal targetFilename As String, Optional ByVal appendToFile As Boolean = False)
            Logger.LogDebug("Create Secure FTP Upload entry", Me.GetType)

            '1. check source
            If Not File.Exists(localFilename) Then
                Throw New Exception("File " & localFilename & " not found")
            End If

            'copy to FI
            Dim fi As New FileInfo(localFilename)
            Dim tmpValue As String
            Dim targetTemp As String
            Dim fileExists As Boolean = False
            Dim targetDir As String
            Dim fileName As String

            Dim fileNames As String()


            fileName = GetFullName(targetFilename)
            targetDir = GetTargetPath(targetFilename)
            If (targetDir = "") Then
                targetDir = "/"
            End If

            Dim sftp As WeOnlyDo.Client.SFTP = Nothing
            'CreateFtpConnection(sftp)

            sftp = New WeOnlyDo.Client.SFTP()

            sftp.Hostname = _hostname
            sftp.Port = _port
            sftp.Login = _username
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey

            sftp.DebugFile = "sftp_debug.txt"
            sftp.LicenseKey = "7BYU-Z8GN-SZQC-A3PX"

            sftp.Timeout = 0
            sftp.Blocking = True
            sftp.TransferMode = WeOnlyDo.Client.SFTP.TransferModes.Binary

            Dim km As WeOnlyDo.Security.Cryptography.KeyManager = New WeOnlyDo.Security.Cryptography.KeyManager()
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey
            km.Load(My.Application.Info.DirectoryPath & "\IRMA Private Key", "Irma12#")
            sftp.PrivateKey = km.PrivateKey(WeOnlyDo.Security.Cryptography.SSHKeyTypes.RSAKey)

            sftp.Connect()

            If appendToFile = False Then
                'Gets the directory listing and loops through to see if the target file already exists
                sftp.ListNames(targetDir)
                fileNames = sftp.ListItem.Split(New [Char]() {ControlChars.Cr, ControlChars.Lf})
                For j As Integer = 0 To fileNames.Length - 1
                    If fileNames(j).ToUpper() = fileName.ToUpper() Then
                        fileExists = True
                        Exit For
                    End If
                Next j

                If fileExists Then
                    sftp.DeleteFile(targetFilename)
                End If

                'Uploads the file with the temp name to repleace the file that was just deleted.
                sftp.PutFile(localFilename, targetDir + fileName)

            Else
                'Gets the directory listing and loops through to see if the target file already exists
                sftp.ListNames(targetDir)
                fileNames = sftp.ListItem.Split(New [Char]() {ControlChars.Cr, ControlChars.Lf})
                For j As Integer = 0 To fileNames.Length - 1
                    If fileNames(j).ToUpper() = fileName.ToUpper() Then
                        fileExists = True
                        Exit For
                    End If
                Next j

                Dim random As New Random(Now.TimeOfDay.Milliseconds)
                tmpValue = "WFM_" + random.Next(1, 9999).ToString() + ".DAT"
                targetTemp = targetDir + tmpValue

                'If the target file exists, this will rename it to a temp file, append the new uploaded temp file, and then
                'rename the combined temp file to the original target file.
                If fileExists Then
                    Try
                        sftp.Rename(targetTemp, targetFilename)
                    Catch ex As Exception
                    End Try

                End If

                If (fileExists) Then
                    sftp.AppendFile(localFilename, targetTemp)
                Else
                    sftp.PutFile(localFilename, targetTemp)
                End If

                'This renames the tempfilename to the real filename once the upload is complete.
                targetFilename = AdjustDir(targetFilename)
                sftp.Rename(targetFilename, targetTemp)

            End If

            sftp.Disconnect()
            sftp.Dispose()

            Logger.LogDebug("Create Secure FTP Upload exit", Me.GetType)
        End Sub

        'copy the file specified to target file: target file can be full path or just filename (keyPath param if supplied, if not it defaults to current directory for key)
        Public Overloads Sub Upload(ByVal localFilename As String, ByVal targetFilename As String, ByVal keyPath As String, Optional ByVal appendToFile As Boolean = False)
            Logger.LogDebug("Create Secure FTP Upload entry", Me.GetType)

            '1. check source
            If Not File.Exists(localFilename) Then
                Throw New Exception("File " & localFilename & " not found")
            End If

            'copy to FI
            Dim fi As New FileInfo(localFilename)
            Dim tmpValue As String
            Dim targetTemp As String
            Dim fileExists As Boolean = False
            Dim targetDir As String
            Dim fileName As String

            Dim fileNames As String()


            fileName = GetFullName(targetFilename)
            targetDir = GetTargetPath(targetFilename)
            If (targetDir = "") Then
                targetDir = "/"
            End If

            Dim sftp As WeOnlyDo.Client.SFTP = Nothing
            'CreateFtpConnection(sftp)

            sftp = New WeOnlyDo.Client.SFTP()

            sftp.Hostname = _hostname
            sftp.Port = _port
            sftp.Login = _username
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey

            sftp.DebugFile = "sftp_debug.txt"
            sftp.LicenseKey = "7BYU-Z8GN-SZQC-A3PX"

            sftp.Timeout = 0
            sftp.Blocking = True
            sftp.TransferMode = WeOnlyDo.Client.SFTP.TransferModes.Binary

            Dim km As WeOnlyDo.Security.Cryptography.KeyManager = New WeOnlyDo.Security.Cryptography.KeyManager()
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey

            If keyPath <> "" Then
                keyPath = keyPath & "IRMA Private Key"
            Else
                keyPath = "IRMA Private Key"
            End If

            km.Load(keyPath, "Irma12#")
            sftp.PrivateKey = km.PrivateKey(WeOnlyDo.Security.Cryptography.SSHKeyTypes.RSAKey)

            sftp.Connect()

            If appendToFile = False Then
                'Gets the directory listing and loops through to see if the target file already exists
                sftp.ListNames(targetDir)
                fileNames = sftp.ListItem.Split(New [Char]() {ControlChars.Cr, ControlChars.Lf})
                For j As Integer = 0 To fileNames.Length - 1
                    If fileNames(j).ToUpper() = fileName.ToUpper() Then
                        fileExists = True
                        Exit For
                    End If
                Next j

                If fileExists Then
                    sftp.DeleteFile(targetFilename)
                End If

                'Uploads the file with the temp name to repleace the file that was just deleted.
                sftp.PutFile(localFilename, targetDir + fileName)

            Else
                'Gets the directory listing and loops through to see if the target file already exists
                sftp.ListNames(targetDir)
                fileNames = sftp.ListItem.Split(New [Char]() {ControlChars.Cr, ControlChars.Lf})
                For j As Integer = 0 To fileNames.Length - 1
                    If fileNames(j).ToUpper() = fileName.ToUpper() Then
                        fileExists = True
                        Exit For
                    End If
                Next j

                Dim random As New Random(Now.TimeOfDay.Milliseconds)
                tmpValue = "WFM_" + random.Next(1, 9999).ToString() + ".DAT"
                targetTemp = targetDir + tmpValue

                'If the target file exists, this will rename it to a temp file, append the new uploaded temp file, and then
                'rename the combined temp file to the original target file.
                If fileExists Then
                    Try
                        sftp.Rename(targetTemp, targetFilename)
                    Catch ex As Exception
                    End Try

                End If

                If (fileExists) Then
                    sftp.AppendFile(localFilename, targetTemp)
                Else
                    sftp.PutFile(localFilename, targetTemp)
                End If

                'This renames the tempfilename to the real filename once the upload is complete.
                targetFilename = AdjustDir(targetFilename)
                sftp.Rename(targetFilename, targetTemp)

            End If

            sftp.Disconnect()
            sftp.Dispose()

            Logger.LogDebug("Create Secure FTP Upload exit", Me.GetType)
        End Sub
#End Region

#Region "Download: File transfer FROM ftp server"
        Public Function Download(ByVal sourceFilename As String, ByVal localFilename As String, Optional ByVal PermitOverwrite As Boolean = False) As Boolean
            Logger.LogDebug("Create Secure Download entry", Me.GetType)

            'Determine target file
            Dim fi As New FileInfo(localFilename)
            If fi.Exists And Not (PermitOverwrite) Then Throw New ApplicationException("Target file already exists")

            'Create the local directory if it does not already exist
            If Not fi.Directory.Exists Then
                fi.Directory.Create()
            End If

            'check source
            Dim target As String
            If sourceFilename.Trim = "" Then
                Throw New ApplicationException("File not specified")
            ElseIf sourceFilename.Contains("/") Then
                target = AdjustDir(sourceFilename)
            Else
                'treat as filename only, use current directory
                target = CurrentDirectory & sourceFilename
            End If

            'declare Secure FTP object 
            Dim sftp As WeOnlyDo.Client.SFTP = Nothing
            'CreateFtpConnection(sftp)

            sftp = New WeOnlyDo.Client.SFTP()

            sftp.Hostname = _hostname
            sftp.Port = _port
            sftp.Login = _username
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey

            sftp.DebugFile = "sftp_debug.txt"
            sftp.LicenseKey = "7BYU-Z8GN-SZQC-A3PX"

            sftp.Timeout = 0
            sftp.Blocking = True
            sftp.TransferMode = WeOnlyDo.Client.SFTP.TransferModes.Binary

            Dim km As WeOnlyDo.Security.Cryptography.KeyManager = New WeOnlyDo.Security.Cryptography.KeyManager()
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey
            km.Load("IRMA Private Key", "Irma12#")
            sftp.PrivateKey = km.PrivateKey(WeOnlyDo.Security.Cryptography.SSHKeyTypes.RSAKey)

            sftp.Connect()

            'download the file
            Try
                sftp.GetFile(localFilename, target)
            Catch ex As Exception
                Return False
            Finally
                sftp.Disconnect()
                sftp.Dispose()
            End Try

            Logger.LogDebug("Create Secure FTP Download exit", Me.GetType)

            Return True
        End Function
#End Region

#Region "Delete file from IBM controller"
        Public Function DeleteFile(ByVal sourceFilename As String) As Boolean
            Logger.LogDebug("Enter [wodSFTP].[DeleteFile]", Me.GetType)

            'declare Secure FTP object 
            Dim sftp As WeOnlyDo.Client.SFTP = Nothing
            'CreateFtpConnection(sftp)

            sftp = New WeOnlyDo.Client.SFTP()

            sftp.Hostname = _hostname
            sftp.Port = _port
            sftp.Login = _username
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey

            sftp.DebugFile = "sftp_debug.txt"
            sftp.LicenseKey = "7BYU-Z8GN-SZQC-A3PX"

            sftp.Timeout = 0
            sftp.Blocking = True
            sftp.TransferMode = WeOnlyDo.Client.SFTP.TransferModes.Binary

            Dim km As WeOnlyDo.Security.Cryptography.KeyManager = New WeOnlyDo.Security.Cryptography.KeyManager()
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey
            km.Load("IRMA Private Key", "Irma12#")
            sftp.PrivateKey = km.PrivateKey(WeOnlyDo.Security.Cryptography.SSHKeyTypes.RSAKey)

            sftp.Connect()

            Dim status As Boolean = False

            'delete the file
            Try
                sftp.DeleteFile(sourceFilename)
                status = True

            Catch ex As Exception
                status = False

            Finally
                sftp.Disconnect()
                sftp.Dispose()

            End Try

            Return status

            Logger.LogDebug("   Exit [wodSFTP].[DeleteFile]", Me.GetType)
        End Function
#End Region

#Region "Check if file exists on IBM controller"
        Public Function FileExists(ByVal sourceFilename As String) As Boolean
            Logger.LogDebug("Enter [wodSFTP].[FileExists]", Me.GetType)

            'declare Secure FTP object 
            Dim sftp As WeOnlyDo.Client.SFTP = Nothing
            'CreateFtpConnection(sftp)

            sftp = New WeOnlyDo.Client.SFTP()

            sftp.Hostname = _hostname
            sftp.Port = _port
            sftp.Login = _username
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey

            sftp.DebugFile = "sftp_debug.txt"
            sftp.LicenseKey = "7BYU-Z8GN-SZQC-A3PX"

            sftp.Timeout = 0
            sftp.Blocking = True
            sftp.TransferMode = WeOnlyDo.Client.SFTP.TransferModes.Binary

            Dim km As WeOnlyDo.Security.Cryptography.KeyManager = New WeOnlyDo.Security.Cryptography.KeyManager()
            sftp.Authentication = WeOnlyDo.Client.SFTP.Authentications.PublicKey
            km.Load("IRMA Private Key", "Irma12#")
            sftp.PrivateKey = km.PrivateKey(WeOnlyDo.Security.Cryptography.SSHKeyTypes.RSAKey)

            sftp.Connect()

            Dim status As Boolean = False

            'check for the file
            Try
                sftp.GetAttributes(sourceFilename)
                status = True

            Catch ex As Exception
                status = False

            Finally
                sftp.Disconnect()
                sftp.Dispose()

            End Try

            Return status

            Logger.LogDebug("   Exit [wodSFTP].[FileExists]", Me.GetType)
        End Function
#End Region

#Region "Other Functions: Delete rename etc."

        Sub Dispose() Implements IDisposable.Dispose
            If Not _ftpClient Is Nothing Then
                _ftpClient.Dispose()
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

        Private Function GetFullPath(ByVal file As String) As String
            If file.Contains("/") Then
                Return AdjustDir(file)
            Else
                Return Me.CurrentDirectory & file
            End If
        End Function

#End Region

    End Class


End Namespace
