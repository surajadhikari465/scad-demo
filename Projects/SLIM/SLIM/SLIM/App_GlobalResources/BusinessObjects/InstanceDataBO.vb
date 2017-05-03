Namespace WholeFoods.IRMA.Common.BusinessLogic

    Public Class InstanceDataBO

        Private _regionName As String
        Private _regionCode As String
        Private _pluDigitsSentToScale As String

        Public Property RegionName() As String
            Get
                Return _regionName
            End Get
            Set(ByVal value As String)
                _regionName = value
            End Set
        End Property

        Public Property RegionCode() As String
            Get
                Return _regionCode
            End Get
            Set(ByVal value As String)
                _regionCode = value
            End Set
        End Property

        Public Property PluDigitsSentToScale() As String
            Get
                Return _pluDigitsSentToScale
            End Get
            Set(ByVal value As String)
                _pluDigitsSentToScale = value
            End Set
        End Property

    End Class

End Namespace
