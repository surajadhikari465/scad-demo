Namespace WholeFoods.IRMA.Planogram.BusinessLogic

    Public Class PlanogramSetBO

        Private _description As String

#Region "Property Access Methods"

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

