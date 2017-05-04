
Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic
    Public Class ItemDefaultAttributeBO

#Region "Property Definitions"

        Private _ID As Integer
        Private _AttributeName As String
        Private _AttributeField As String
        Private _Type As Integer
        Private _Active As Boolean
        Private _ControlOrder As Integer
        Private _ControlType As Integer
        Private _PopulateProcedure As String
        Private _IndexField As String
        Private _DescriptionField As String
        Private _ItemDefaultValueID As Integer
        Private _Value As String

#End Region

#Region "Constructors"

        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

#End Region

#Region "Property Access Methods"

        Public Property Active() As Boolean
            Get
                Return _Active
            End Get
            Set(ByVal value As Boolean)
                _Active = value
            End Set
        End Property
        Public Property AttributeField() As String
            Get
                Return _AttributeField
            End Get
            Set(ByVal value As String)
                _AttributeField = value
            End Set
        End Property
        Public Property AttributeName() As String
            Get
                Return _AttributeName
            End Get
            Set(ByVal value As String)
                _AttributeName = value
            End Set
        End Property

        Public Property ControlOrder() As Integer
            Get
                Return _ControlOrder
            End Get
            Set(ByVal value As Integer)
                _ControlOrder = value
            End Set
        End Property
        Public Property ControlType() As Integer
            Get
                Return _ControlType
            End Get
            Set(ByVal value As Integer)
                _ControlType = value
            End Set
        End Property
        Public Property DescriptionField() As String
            Get
                Return _DescriptionField
            End Get
            Set(ByVal value As String)
                _DescriptionField = value
            End Set
        End Property

        Public Property IndexField() As String
            Get
                Return _IndexField
            End Get
            Set(ByVal value As String)
                _IndexField = value
            End Set
        End Property
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Public Property PopulateProcedure() As String
            Get
                Return _PopulateProcedure
            End Get
            Set(ByVal value As String)
                _PopulateProcedure = value
            End Set
        End Property

        Public Property Type() As Integer
            Get
                Return _Type
            End Get
            Set(ByVal value As Integer)
                _Type = value
            End Set
        End Property

        Public Property ItemDefaultValueID() As Integer
            Get
                Return _ItemDefaultValueID
            End Get
            Set(ByVal value As Integer)
                _ItemDefaultValueID = value
            End Set
        End Property

        Public Property Value() As String
            Get
                Return _Value
            End Get
            Set(ByVal value As String)
                _Value = value
            End Set
        End Property

#End Region

#Region "Business Rules"


#End Region

#Region "Data Access"

        ''' <summary>
        ''' Load the Item.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Load() As DataSet

            Return Nothing

        End Function

        ''' <summary>
        ''' Save the Item.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Save() As DataSet

            Return Nothing

        End Function

#End Region

    End Class

End Namespace
