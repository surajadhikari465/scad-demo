Namespace IRMA

    <DataContract()> _
    Public Class StoreFTPConfig

        Private _storeNo As Integer
        Private _fileWriterType As String
        Private _ipAddress As String
        Private _ftpUser As String
        Private _ftpPassword As String
        Private _changeDirectory As String
        Private _port As String  'port set as String so as not to default to 0
        Private _isSecureTransfer As Boolean
        Private _posSystemType As String


        <DataMember()> _
        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property

        <DataMember()> _
        Public Property FileWriterType() As String
            Get
                Return _fileWriterType
            End Get
            Set(ByVal value As String)
                _fileWriterType = value
            End Set
        End Property

        <DataMember()> _
        Public Property IPAddress() As String
            Get
                Return _ipAddress
            End Get
            Set(ByVal value As String)
                _ipAddress = value
            End Set
        End Property

        <DataMember()> _
        Public Property FTPUser() As String
            Get
                Return _ftpUser
            End Get
            Set(ByVal value As String)
                _ftpUser = value
            End Set
        End Property

        <DataMember()> _
        Public Property FTPPassword() As String
            Get
                Return _ftpPassword
            End Get
            Set(ByVal value As String)
                _ftpPassword = value
            End Set
        End Property

        <DataMember()> _
        Public Property ChangeDirectory() As String
            Get
                Return _changeDirectory
            End Get
            Set(ByVal value As String)
                _changeDirectory = value
            End Set
        End Property

        <DataMember()> _
        Public Property Port() As String
            Get
                Return _port
            End Get
            Set(ByVal value As String)
                _port = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsSecureTransfer() As Boolean
            Get
                Return _isSecureTransfer
            End Get
            Set(ByVal value As Boolean)
                _isSecureTransfer = value
            End Set
        End Property

        <DataMember()> _
        Public Property POSSystemType() As String
            Get
                Return _posSystemType
            End Get
            Set(ByVal value As String)
                _posSystemType = value
            End Set
        End Property


        Sub New()

        End Sub

        Sub New(ByRef results As SqlClient.SqlDataReader)
            Try
                If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                    _storeNo = results.GetInt32(results.GetOrdinal("Store_No"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("FileWriterType"))) Then
                    _fileWriterType = results.GetString(results.GetOrdinal("FileWriterType"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("IP_Address"))) Then
                    _ipAddress = results.GetString(results.GetOrdinal("IP_Address"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("FTP_User"))) Then
                    _ftpUser = results.GetString(results.GetOrdinal("FTP_User"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("FTP_Password"))) Then
                    _ftpPassword = results.GetString(results.GetOrdinal("FTP_Password"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("ChangeDirectory"))) Then
                    _changeDirectory = results.GetString(results.GetOrdinal("ChangeDirectory"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("Port"))) Then
                    _port = results.GetInt32(results.GetOrdinal("Port")).ToString
                End If

                If (Not results.IsDBNull(results.GetOrdinal("IsSecureTransfer"))) Then
                    _isSecureTransfer = results.GetBoolean(results.GetOrdinal("IsSecureTransfer"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("POSSystemType"))) Then
                    _posSystemType = results.GetString(results.GetOrdinal("POSSystemType"))
                End If

            Catch ex As Exception
                Throw ex
            End Try


        End Sub
    End Class
End Namespace
Public Class StoreFTPConfig

End Class
