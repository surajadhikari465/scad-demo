
Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic
    Public Class ItemDefaultValueBO

#Region "Property Definitions"

        Private _ID As Integer
        Private _ItemDefaultAttributeID As Integer
        Private _prodHierarchyLevel4ID As Integer
        Private _categoryID As Integer
        Private _value As String
        Private _fieldName As String
        Private _dbDataType As Integer

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

        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Public Property ItemDefaultAttributeID() As Integer
            Get
                Return _ItemDefaultAttributeID
            End Get
            Set(ByVal value As Integer)
                _ItemDefaultAttributeID = value
            End Set
        End Property

        Public Property ProdHierarchyLevel4ID() As Integer
            Get
                Return _prodHierarchyLevel4ID
            End Get
            Set(ByVal value As Integer)
                _prodHierarchyLevel4ID = value
            End Set
        End Property

        Public Property CategoryID() As Integer
            Get
                Return _categoryID
            End Get
            Set(ByVal value As Integer)
                _categoryID = value
            End Set
        End Property

        Public Property Value() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property


        Public Property FieldName() As String
            Get
                Return _fieldName
            End Get
            Set(ByVal value As String)
                _fieldName = value
            End Set
        End Property

        Public Property DbDataType() As Integer
            Get
                Return _dbDataType
            End Get
            Set(ByVal value As Integer)
                _dbDataType = value
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
