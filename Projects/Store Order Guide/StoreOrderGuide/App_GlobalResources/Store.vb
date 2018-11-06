Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class Store
    Private _StoreNo As Integer
    Private _StoreName As String
    Private _StoreAbbr As String

    Public Property StoreNo() As Integer
        Get
            Return _StoreNo
        End Get
        Set(ByVal value As Integer)
            _StoreNo = value
        End Set
    End Property

    Public Property StoreName() As String
        Get
            Return _StoreName
        End Get
        Set(ByVal value As String)
            _StoreName = value
        End Set
    End Property

    Public Property StoreAbbr() As String
        Get
            Return _StoreAbbr
        End Get
        Set(ByVal value As String)
            _StoreAbbr = value
        End Set
    End Property

End Class
