Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ScaleLabelFormatBO

        Private _id As Integer
        Private _description As String

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

#End Region
    End Class

End Namespace
