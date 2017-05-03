Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class VendorBO

        Private _vendorID As Integer
        Private _vendorName As String
        Private _PSVendorId As String
        Private _vendorCurrencyCode As String
        Private _vendorCurrencyID As Integer

        Public Property VendorCurrencyCode() As String
            Get
                Return _vendorCurrencyCode
            End Get
            Set(ByVal value As String)
                _vendorCurrencyCode = value
            End Set
        End Property

        Public Property VendorCurrencyID() As Integer
            Get
                Return _vendorCurrencyID
            End Get
            Set(ByVal value As Integer)
                _vendorCurrencyID = value
            End Set
        End Property

        Public Property VendorID() As Integer
            Get
                Return _vendorID
            End Get
            Set(ByVal value As Integer)
                _vendorID = value
            End Set
        End Property

        Public Property VendorName() As String
            Get
                Return _vendorName
            End Get
            Set(ByVal value As String)
                _vendorName = value
            End Set
        End Property

        Public Property PSVendorId As String
            Get
                Return _PSVendorId
            End Get
            Set(ByVal value As String)
                _PSVendorId = value
            End Set
        End Property

        Sub New()

        End Sub

        Sub New(ByVal id As Integer, ByVal VendorName As String, ByVal PSVendorId As String,
                Optional ByVal VendorCurrencyCode As String = "",
                Optional ByVal VendorCurrencyID As Integer = -1)
            _vendorID = id
            _vendorName = VendorName
            _PSVendorId = PSVendorId
            _vendorCurrencyCode = VendorCurrencyCode
            _vendorCurrencyID = VendorCurrencyID
        End Sub

    End Class

End Namespace