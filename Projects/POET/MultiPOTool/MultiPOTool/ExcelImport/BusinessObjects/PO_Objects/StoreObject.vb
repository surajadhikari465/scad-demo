Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class StoreObject
    Private _sa As String
    Private _ord As Integer

    Sub New(ByVal sa As String, ByVal ord As Integer)
        '_bu = bu
        _sa = sa
        _ord = ord
    End Sub

    Sub New()

    End Sub
    
    Public Property StoreAbbr() As String
        Get
            Return _sa
        End Get
        Set(ByVal value As String)
            _sa = Trim(value)
        End Set
    End Property
    Public Property ColumnOrdinal() As Integer
        Get
            Return _ord
        End Get
        Set(ByVal value As Integer)
            _ord = value
        End Set
    End Property
End Class
