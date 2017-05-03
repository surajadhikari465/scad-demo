
Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Business Object for the UploadRow db table.
	'''
    ''' This class acts as a base class of all
    ''' the generated business objects.
	'''
    ''' DO NOT MODIFY THIS CLASS.
	'''
	''' Created By:	David Marine
	''' Created   :	Feb 12, 2007
	''' </summary>
	''' <remarks></remarks>
    Public Class BusinessObjectBase

#Region "Fields and Properties"

        Private _isNew As Boolean = True
		Private _isDirty As Boolean = False
        Private _isMarkedForDelete As Boolean = False
        Private _isDeleted As Boolean = False

        Public Property IsNew() As Boolean
            Get
                Return _isNew
            End Get
            Set(ByVal value As Boolean)
                _isNew = value
            End Set
        End Property

        Public Property IsDeleted() As Boolean
            Get
                Return _isDeleted
            End Get
            Set(ByVal value As Boolean)
                _isDeleted = value
            End Set
        End Property

        Public Property IsDirty() As Boolean
            Get
                Return _isDirty
            End Get
            Set(ByVal value As Boolean)
                _isDirty = value
            End Set
        End Property

        Public Property IsMarkedForDelete() As Boolean
            Get
                Return _isMarkedForDelete
            End Get
            Set(ByVal value As Boolean)
                _isMarkedForDelete = value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Public Methods"

#End Region

#Region "Private Methods"

#End Region

    End Class

End Namespace

