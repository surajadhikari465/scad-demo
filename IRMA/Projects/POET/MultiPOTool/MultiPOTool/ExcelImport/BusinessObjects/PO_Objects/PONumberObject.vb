Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class PONumberObject

    Private _poNumber As Integer
    Private _poNumberID As Integer
    Private _poType As Integer
    Private _regionID As Integer
    Public Property PONumber() As Integer
        Get
            Return _poNumber
        End Get
        Set(ByVal value As Integer)
            _poNumber = value
        End Set
    End Property

    Public Property PONumberID() As Integer
        Get
            Return _poNumberID
        End Get
        Set(ByVal value As Integer)
            _poNumberID = value
        End Set
    End Property

    Public Property RegionID() As Integer
        Get
            Return _regionID
        End Get
        Set(ByVal value As Integer)
            _regionID = value
        End Set
    End Property

    Public Property POType() As Integer
        Get
            Return _poType
        End Get
        Set(ByVal value As Integer)
            _poType = value
        End Set
    End Property
End Class
