Namespace WholeFoods.IRMA.Common.BusinessLogic

    Public Class InstanceDataOverrideStoreCountsBO

        Private _flagKey As String
        Private _regionalFlagValue As Boolean
        Private _numStoresWithOverrides As Integer
        Private _numStoresTotal As Integer

        Public Property FlagKey() As String
            Get
                Return _flagKey
            End Get
            Set(ByVal value As String)
                _flagKey = value
            End Set
        End Property

        Public Property RegionalFlagValue() As Boolean
            Get
                Return _regionalFlagValue
            End Get
            Set(ByVal value As Boolean)
                _regionalFlagValue = value
            End Set
        End Property

        Public Property NumStoresWithOverrides() As Integer
            Get
                Return _numStoresWithOverrides
            End Get
            Set(ByVal value As Integer)
                _numStoresWithOverrides = value
            End Set
        End Property

        Public Property NumStoresTotal() As Integer
            Get
                Return _numStoresTotal
            End Get
            Set(ByVal value As Integer)
                _numStoresTotal = value
            End Set
        End Property
    End Class

End Namespace
