Namespace WholeFoods.IRMA.Common.BusinessLogic

    Public Class InstanceDataBO

        Private _regionName As String
        Private _regionCode As String
        Private _pluDigitsSentToScale As String
        '20100215 - Dave Stacey - Add Culture and DateMask to global variable collection
        Private _UG_Culture As String
        Private _UG_DateMask As String

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

        Public Property UG_Culture() As String
            Get
                Return _UG_Culture
            End Get
            Set(ByVal value As String)
                _UG_Culture = value
            End Set
        End Property

        Public Property UG_DateMask() As String
            Get
                Return _UG_DateMask
            End Get
            Set(ByVal value As String)
                _UG_DateMask = value
            End Set
        End Property

    End Class

End Namespace
