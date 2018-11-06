Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ScaleExtraTextBO

        Private _id As Integer
        Private _description As String
        Private _scale_LabelType_ID As Integer
        Private _extraText As String

#Region "Property Access Methods"

        Public Property ID() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property Scale_LabelType_ID() As Integer
            Get
                Return _scale_LabelType_ID
            End Get
            Set(ByVal value As Integer)
                _scale_LabelType_ID = value
            End Set
        End Property

        Public Property ExtraText() As String
            Get
                Return _extraText
            End Get
            Set(ByVal value As String)
                _extraText = value
            End Set
        End Property
#End Region

    End Class

End Namespace
