Namespace WholeFoods.IRMA.Replenishment.Common.BusinessLogic

    Public Class InstanceDataFlagsBO

        Private _storeNo As Integer
        Private _storeName As String
        Private _flagKey As String
        Private _flagValue As Boolean
        Private _canStoreOverride As Boolean

        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property

        Public Property StoreName() As String
            Get
                Return _storeName
            End Get
            Set(ByVal value As String)
                _storeName = value
            End Set
        End Property

        Public Property FlagKey() As String
            Get
                Return _flagKey
            End Get
            Set(ByVal value As String)
                _flagKey = value
            End Set
        End Property

        Public Property FlagValue() As Boolean
            Get
                Return _flagValue
            End Get
            Set(ByVal value As Boolean)
                _flagValue = value
            End Set
        End Property

        Public Property CanStoreOverride() As Boolean
            Get
                Return _canStoreOverride
            End Get
            Set(ByVal value As Boolean)
                _canStoreOverride = value
            End Set
        End Property
    End Class

End Namespace
