Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class VendorObject

    Private _rowOrdinal As Integer
    Private _VendorName As String
    Private _VendorPSNumber As String

    Sub New()

    End Sub

    Sub New(ByVal ven As String, ByVal ord As Integer)

        VendorPSNumber = ven
        RowOrdinal = ord

    End Sub
    Public Property VendorPSNumber() As String
        Get
            Return _VendorPSNumber
        End Get
        Set(ByVal value As String)
            _VendorPSNumber = value
        End Set
    End Property

    Public Property VendorName() As String
        Get
            Return _VendorName
        End Get
        Set(ByVal value As String)
            _VendorName = value
        End Set
    End Property

    Public Property RowOrdinal() As Integer
        Get
            Return _rowOrdinal
        End Get
        Set(ByVal value As Integer)
            _rowOrdinal = value
        End Set
    End Property
End Class
