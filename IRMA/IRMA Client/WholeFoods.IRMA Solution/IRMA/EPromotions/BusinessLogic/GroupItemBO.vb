Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class GroupItemBO
#Region "Property Definitions"
        Private _ItemKey As Integer
        Private _ItemDescription As String
        Private _UserId As Integer
#End Region

#Region "Property Access Methods"

        Public Property Description() As String
            Get
                Return _ItemDescription
            End Get
            Set(ByVal value As String)
                _ItemDescription = value
            End Set
        End Property
        Public Property Key() As Integer
            Get
                Return _ItemKey
            End Get
            Set(ByVal value As Integer)
                _ItemKey = value
            End Set
        End Property
        Public Property UserId() As Integer
            Get
                Return _UserId
            End Get
            Set(ByVal value As Integer)
                _UserId = value
            End Set
        End Property



#End Region

#Region "Constructors"
        ''' <summary>
        ''' empty constuctor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub
#End Region

#Region "Business Rules"

#End Region

    End Class
End Namespace

