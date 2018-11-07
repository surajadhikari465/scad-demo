Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.Pricing.DataAccess


Namespace WholeFoods.IRMA.Pricing.BusinessLogic

    Public Class ItemForAdHocPrint

        Private _identifier As String
        Private _itemKey As Integer
        Private _subTeamNumber As Integer
        Private _subTeamName As String
        Private _requestedOrder As Integer
        Private _identifierIsValid As Boolean
        Private _excludedByNoTagLogic As Boolean

        Public Property Identifier As String
            Get
                Return _identifier
            End Get
            Set(value As String)
                _identifier = value
            End Set
        End Property

        Public Property ItemKey As Integer
            Get
                Return _itemKey
            End Get
            Set(value As Integer)
                _itemKey = value
            End Set
        End Property

        Public Property SubteamNumber As Integer
            Get
                Return _subTeamNumber
            End Get
            Set(value As Integer)
                _subTeamNumber = value
            End Set
        End Property

        Public Property SubteamName As String
            Get
                Return _subTeamName
            End Get
            Set(value As String)
                _subTeamName = value
            End Set
        End Property

        Public Property RequestedOrder As Integer
            Get
                Return _requestedOrder
            End Get
            Set(value As Integer)
                _requestedOrder = value
            End Set
        End Property

        Public Property IdentifierIsValid As Boolean
            Get
                Return _identifierIsValid
            End Get
            Set(value As Boolean)
                _identifierIsValid = value
            End Set
        End Property

        Public Property ExcludedByNoTagLogic As Boolean
            Get
                Return _excludedByNoTagLogic
            End Get
            Set(value As Boolean)
                _excludedByNoTagLogic = value
            End Set
        End Property

    End Class

End Namespace