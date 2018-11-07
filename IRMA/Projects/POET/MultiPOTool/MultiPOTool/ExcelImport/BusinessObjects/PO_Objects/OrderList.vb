Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class OrderList




    Public Property NumberOfOrders() As Integer
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public Property SessionHistory() As Integer
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public Property CreatedDate() As DateTime
        Get

        End Get
        Set(ByVal value As DateTime)

        End Set
    End Property

    Public Property Orders() As Collections.ObjectModel.Collection(Of OrderObject)
        Get

        End Get
        Set(ByVal value As Collections.ObjectModel.Collection(Of OrderObject))

        End Set
    End Property




    Public Function GetNumberOfOrders(ByVal Orders As Collections.ObjectModel.Collection(Of OrderObject)) As Integer

    End Function
End Class
