Namespace WholeFoods.IRMA.Pricing.BusinessLogic

    Public Class PriceBatchFailedPrintRequestsBO

        Private _BatchName As String
        Private _StoreNumber As Integer
        Private _SubteamName As String

#Region "Property Access Methods"
        Property BatchName() As String
            Get
                Return _BatchName
            End Get
            Set(ByVal value As String)
                _BatchName = value
            End Set
        End Property

        Property StoreNumber() As Integer
            Get
                Return _StoreNumber
            End Get
            Set(ByVal value As Integer)
                _StoreNumber = value
            End Set
        End Property

        Property SubteamName() As String
            Get
                Return _SubteamName
            End Get
            Set(ByVal value As String)
                _SubteamName = value
            End Set
        End Property

#End Region

    End Class

End Namespace
