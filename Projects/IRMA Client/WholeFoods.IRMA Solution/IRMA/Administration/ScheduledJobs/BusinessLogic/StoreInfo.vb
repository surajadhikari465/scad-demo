
Public Class StoreInfo
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

    Private _StoreAbbr As String
    Public Property StoreAbbr() As String
        Get
            Return _StoreAbbr
        End Get
        Set(ByVal value As String)
            _StoreAbbr = value
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
