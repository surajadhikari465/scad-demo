Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class StoreList

    Private _NumberOfStores As Integer
    Private _StoreList As List(Of StoreObject)
    Sub New(ByVal stores As List(Of StoreObject))

        StoreList = stores
        _NumberOfStores = stores.Count

    End Sub

    Sub New()

    End Sub
    Public Property StoreList() As List(Of StoreObject)
        Get
            Return _StoreList
        End Get
        Set(ByVal value As List(Of StoreObject))
            _StoreList = value
        End Set
    End Property

    Public ReadOnly Property NumberOfStores() As Integer
        Get
            Return _NumberOfStores
        End Get
    End Property
End Class
