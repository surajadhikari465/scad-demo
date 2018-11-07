Namespace WholeFoods.IRMA.InterfaceCommunication.WebApiModel
    Public Class SlawPrintBatchItemModel
        Private _identifier As String
        Private _printOrder As String
        Private _template As String
        Private _slawId As Integer
        Private _startDate As String
        Private _effectiveDateEnd As String
        Private _tprHasRegPriceChange As Boolean

        Property Identifier() As String
            Get
                Return _identifier
            End Get

            Set(ByVal Value As String)
                _identifier = Value
            End Set
        End Property

        Property StartDate() As String
            Get
                Return _startDate
            End Get

            Set(ByVal Value As String)
                _startDate = Value
            End Set
        End Property

        Property TprHasRegPriceChange() As Boolean
            Get
                Return _tprHasRegPriceChange
            End Get

            Set(ByVal Value As Boolean)
                _tprHasRegPriceChange = Value
            End Set
        End Property

        Property PrintOrder() As String
            Get
                Return _printOrder
            End Get

            Set(ByVal Value As String)
                _printOrder = Value
            End Set
        End Property
        Property Template() As String
            Get
                Return _template
            End Get

            Set(ByVal Value As String)
                _template = Value
            End Set
        End Property

    End Class
End Namespace