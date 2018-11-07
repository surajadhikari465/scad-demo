Namespace WholeFoods.IRMA.Common.BusinessLogic
    Public Class StoreListBO

        Private _StoreNo As Integer
        Private _StoreName As String
        Private _VendorID As Integer
        Private _VendorName As String

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

        Public Property VendorID() As Integer
            Get
                Return _VendorID
            End Get
            Set(ByVal value As Integer)
                _VendorID = value
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

    End Class
End Namespace