Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ScaleLabelTypeBO

        Private _id As Integer
        Private _description As String
        Private _linesPerLabel As Integer
        Private _charsPerLine As Integer

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
        Public Property LinesPerLabel() As Integer
            Get
                Return _linesPerLabel
            End Get
            Set(ByVal value As Integer)
                _linesPerLabel = value
            End Set
        End Property
        Public Property CharsPerLine() As Integer
            Get
                Return _charsPerLine
            End Get
            Set(ByVal value As Integer)
                _charsPerLine = value
            End Set
        End Property
#End Region

    End Class

End Namespace
