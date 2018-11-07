
Imports System.Reflection

Namespace WholeFoods.IRMA.ModelLayer

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


#Region "Public Events"

        Public Delegate Sub PrimaryKeyChangedEventHandler(ByRef inBusinessObject As BusinessObjectBase, ByVal oldValue As Object, ByVal newValue As Object)
        Public Event PrimaryKeyChanged As PrimaryKeyChangedEventHandler

        Public Delegate Sub PropertyChangedEventHandler(ByRef inBusinessObject As BusinessObjectBase, _
                ByVal inChangedPropertyName As String, ByVal oldValue As Object, ByVal newValue As Object)
        Public Event PropertyChanged As PropertyChangedEventHandler

#End Region

#Region "Fields and Properties"

        Private _isNew As Boolean = True
		Private _isDirty As Boolean = False
        Private _isMarkedForDelete As Boolean = False
        Private _isDeleted As Boolean = False
        Private _primaryKey As Object = Nothing

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

        Public Overridable Property IsMarkedForDelete() As Boolean
            Get
                Return _isMarkedForDelete
            End Get
            Set(ByVal value As Boolean)
                _isMarkedForDelete = value
            End Set
        End Property

        Public Property PrimaryKey() As Object
            Get
                Return _primaryKey
            End Get
            Set(ByVal value As Object)

                ' fire off the primary key changed event
                ' this is usually subscribed to by a BusinessObjectCollection
                ' that will need to change the key under which this BusinessObject
                ' is referenced
                If Not Object.Equals(_primaryKey, value) Then

                    Dim theOldValue As Object = _primaryKey

                    _primaryKey = value

                    RaiseEvent PrimaryKeyChanged(Me, theOldValue, value)
                End If

            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Event Handlers"

        ''' <summary>
        ''' Handle the changing of the IsDirty flag of a child BusinessObject
        ''' by setting the IsDirty flag of this BusinessObject to the same value and
        ''' firing the same event.
        ''' </summary>
        ''' <param name="oldValue"></param>
        ''' <param name="NewValue"></param>
        ''' <remarks></remarks>
        Protected Sub PropertyChangedHandler(ByRef inBusinessObject As BusinessObjectBase, _
                ByVal inChangedPropertyName As String, ByVal oldValue As Object, ByVal newValue As Object)

            Me.IsDirty = True

            RaiseEvent PropertyChanged(inBusinessObject, inChangedPropertyName, oldValue, newValue)

        End Sub

        ''' <summary>
        ''' Raise the PropertyChanged event through a method since a derived class cannot raise
		''' an event of a base class directly.
        ''' </summary>
        ''' <param name="oldValue"></param>
        ''' <param name="NewValue"></param>
        ''' <remarks></remarks>
        Protected Sub RaisePropertyChangedEvent(ByRef inBusinessObject As BusinessObjectBase, _
                ByVal inChangedPropertyName As String, ByVal oldValue As Object, ByVal newValue As Object)

            RaiseEvent PropertyChanged(inBusinessObject, inChangedPropertyName, oldValue, newValue)

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Return a copy of this BusinessObject.
        ''' You will need to set its parents, if any.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNewCopy() As BusinessObjectBase

            Dim theNewBusinessObject As BusinessObjectBase = Me.GetNewInstance()

            ' use reflection to copy all property values
            For Each theProperty As PropertyInfo In Me.GetType().GetProperties()

                If theProperty.CanWrite Then
                    If theProperty.PropertyType Is System.Type.GetType("BusinessObjectCollection") Then
                        theProperty.SetValue(theNewBusinessObject, CType(theProperty.GetValue(Me, Nothing), BusinessObjectCollection).NewCopy(), Nothing)
                    Else
                        theProperty.SetValue(theNewBusinessObject, theProperty.GetValue(Me, Nothing), Nothing)
                    End If
                End If

            Next

            Return theNewBusinessObject

        End Function

#End Region

#Region "Private Methods"

        Protected Overridable Function GetNewInstance() As BusinessObjectBase
            Return Nothing
        End Function

#End Region

    End Class

End Namespace

