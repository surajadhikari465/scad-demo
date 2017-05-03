Public Class FTPBO

    Private _ftpip As String
    Public Property FTP_IP() As String
        Get
            Return _ftpip
        End Get
        Set(ByVal value As String)
            _ftpip = value
        End Set
    End Property

    Private _ftpdir As String
    Public Property FTP_DIR() As String
        Get
            Return _ftpdir
        End Get
        Set(ByVal value As String)
            _ftpdir = value
        End Set
    End Property

    Private _ftpuid As String
    Public Property FTP_UID() As String
        Get
            Return _ftpuid
        End Get
        Set(ByVal value As String)
            _ftpuid = value
        End Set
    End Property

    Private _ftppwd As String
    Public Property FTP_PWD() As String
        Get
            Return _ftppwd
        End Get
        Set(ByVal value As String)
            _ftppwd = value
        End Set
    End Property

    Private _ftpsecure As Boolean
    Public Property FTP_SECURE() As Boolean
        Get
            Return _ftpsecure
        End Get
        Set(ByVal value As Boolean)
            _ftpsecure = value
        End Set
    End Property

    Private _ftpdelimiter As String
    Public Property FTP_DELIM() As String
        Get
            Return _ftpdelimiter
        End Get
        Set(ByVal value As String)
            _ftpdelimiter = value
        End Set
    End Property

    Private _ftpport As Integer
    Public Property FTP_PORT() As Integer
        Get
            Return _ftpport
        End Get
        Set(ByVal value As Integer)
            _ftpport = value
        End Set
    End Property

End Class
