Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Enum LabelSizeEnum
        Undefined = 0
        None = 1
        Small = 4
        Medium = 2
        SmallGlutenFree = 11
        MediumGlutenFree = 12
        SmallDairyFree = 13
        MediumDairyFree = 14
        SmallPrivateLabel = 15
        MediumPrivateLabel = 16
    End Enum

    Public Enum LabelTypeEnum
        Regular = 100
        Sale = 600
        Offer = 700
        Promo = 800
        [New] = 900
    End Enum

    Public Class LabelTypeBO

        Private _labelTypeID As Integer
        Private _labelTypeDesc As String

#Region "Property Access Methods"

        Public Property LabelTypeID() As Integer
            Get
                Return _labelTypeID
            End Get
            Set(ByVal value As Integer)
                _labelTypeID = value
            End Set
        End Property

        Public Property LabelTypeDesc() As String
            Get
                Return _labelTypeDesc
            End Get
            Set(ByVal value As String)
                _labelTypeDesc = value
            End Set
        End Property

#End Region

    End Class

End Namespace
