
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the UploadValue db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadValueRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _uploadValueID As System.Int32
		Private _isUploadValueIDNull As Boolean = True
		Private _uploadAttributeID As System.Int32
		Private _isUploadAttributeIDNull As Boolean = True
		Private _uploadRowID As System.Int32
		Private _isUploadRowIDNull As Boolean = True
		Private _value As System.String
		Private _isValueNull As Boolean = True

		Public Overridable Property UploadValueID() As System.Int32
		    Get
				Return _uploadValueID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadValueIDNull = False
				If _uploadValueID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadValueID", theOldValue, value)
				End If
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
				_uploadValueID = value
		    End Set
		End Property

		Public Overridable Property IsUploadValueIDNull() As Boolean
		    Get
				Return _isUploadValueIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadValueIDNull = value
		    End Set
		End Property

		Public Overridable Property UploadAttributeID() As System.Int32
		    Get
				Return _uploadAttributeID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadAttributeIDNull = False
				If _uploadAttributeID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadAttributeID", theOldValue, value)
				End If
				
			
				_uploadAttributeID = value
		    End Set
		End Property

		Public Overridable Property IsUploadAttributeIDNull() As Boolean
		    Get
				Return _isUploadAttributeIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadAttributeIDNull = value
		    End Set
		End Property

		Public Overridable Property UploadRowID() As System.Int32
		    Get
				Return _uploadRowID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadRowIDNull = False
				If _uploadRowID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadRowID", theOldValue, value)
				End If
				
			
				_uploadRowID = value
		    End Set
		End Property

		Public Overridable Property IsUploadRowIDNull() As Boolean
		    Get
				Return _isUploadRowIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadRowIDNull = value
		    End Set
		End Property

		Public Overridable Property Value() As System.String
		    Get
				Return _value
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsValueNull = IsNothing(value)
				If (IsNothing(_value) And Not IsNothing(value)) Or _
						(Not IsNothing(_value) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_value) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _value.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "Value", theOldValue, value)
				End If
				
			
				_value = value
		    End Set
		End Property

		Public Overridable Property IsValueNull() As Boolean
		    Get
				Return _isValueNull
		    End Get
		    Set(ByVal value As Boolean)
				_isValueNull = value
		    End Set
		End Property

		
#End Region

#Region "Non-persistent Fields and Properties"

		Private Shared _nextTemporaryId As Integer = 0

		Public Shared Property NextTemporaryId() As Integer
		    Get
				_nextTemporaryId = _nextTemporaryId - 1
				Return _nextTemporaryId
		    End Get
		    Set(ByVal value As Integer)
				_nextTemporaryId = value
		    End Set
		End Property
		
#End Region

#Region "Constructors"

		Public Sub New()
			Me.UploadValueID = UploadValue.NextTemporaryId
		End Sub
		
		''' <summary>
        ''' Construct a UploadValue by passing in
		''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
		Public Sub New(ByRef inUploadAttribute As UploadAttribute, ByRef inUploadRow As UploadRow)
		
			Me.UploadValueID = UploadValue.NextTemporaryId

			Me.UploadAttribute = inUploadAttribute
			
			If Not IsNothing(inUploadAttribute) Then
				Me.UploadAttributeID = inUploadAttribute.UploadAttributeID
				inUploadAttribute.AddUploadValue(CType(Me, UploadValue))
			End If
			
			Me.UploadRow = inUploadRow
			
			If Not IsNothing(inUploadRow) Then
				Me.UploadRowID = inUploadRow.UploadRowID
				inUploadRow.AddUploadValue(CType(Me, UploadValue))
			End If
			
        End Sub

#End Region

#Region "Just-in-time Instantiated Parents"

		Private _isUploadAttributeLoaded As Boolean = False
		
		        Public Property IsUploadAttributeLoaded() As Boolean
            Get
                Return _isUploadAttributeLoaded
            End Get
            Set(ByVal value As Boolean)
                _isUploadAttributeLoaded = value
            End Set
        End Property
 
		Private _uploadAttribute As UploadAttribute = Nothing

	    ''' <summary>
        ''' Set this UploadValue UploadAttribute parent
		''' and set the foreign key.
		''' This UploadValue is not added to the parent's UploadValueCollection.
		''' You will have to do this yourself.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadAttribute() As UploadAttribute
		    Get
		    	If Not _isUploadAttributeLoaded Then
				
					' set his once we try to load the parent so we won't keep trying if a parent
					' is not found and the reference remains Nothing
					_isUploadAttributeLoaded = True
					
		    		_uploadAttribute = _
		    			UploadAttributeDAO.Instance.GetUploadAttributeByPK(Me.UploadAttributeID)
		    	End If
			Return _uploadAttribute
		    End Get
		    Set(ByVal value As UploadAttribute)
				_uploadAttribute = value
				
				_isUploadAttributeLoaded = True

				' set the FKs but DO NOT add this BusinessObject to the parent's corresponding child collection
				' the developer will have to do this
				If Not IsNothing(value) Then
					Me.UploadAttributeID = _uploadAttribute.UploadAttributeID
				End If
		    End Set
		End Property
		
		Private _isUploadRowLoaded As Boolean = False
		
		        Public Property IsUploadRowLoaded() As Boolean
            Get
                Return _isUploadRowLoaded
            End Get
            Set(ByVal value As Boolean)
                _isUploadRowLoaded = value
            End Set
        End Property
 
		Private _uploadRow As UploadRow = Nothing

	    ''' <summary>
        ''' Set this UploadValue UploadRow parent
		''' and set the foreign key.
		''' This UploadValue is not added to the parent's UploadValueCollection.
		''' You will have to do this yourself.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadRow() As UploadRow
		    Get
		    	If Not _isUploadRowLoaded Then
				
					' set his once we try to load the parent so we won't keep trying if a parent
					' is not found and the reference remains Nothing
					_isUploadRowLoaded = True
					
		    		_uploadRow = _
		    			UploadRowDAO.Instance.GetUploadRowByPK(Me.UploadRowID)
		    	End If
			Return _uploadRow
		    End Get
		    Set(ByVal value As UploadRow)
				_uploadRow = value
				
				_isUploadRowLoaded = True

				' set the FKs but DO NOT add this BusinessObject to the parent's corresponding child collection
				' the developer will have to do this
				If Not IsNothing(value) Then
					Me.UploadRowID = _uploadRow.UploadRowID
				End If
		    End Set
		End Property
		
#End Region

#Region "Just-in-time Instantiated Children Collections"

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this UploadValue
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					UploadValueDAO.Instance.InsertUploadValue(CType(Me, UploadValue))
					Trace.WriteLine("Inserting a new UploadValue")
				Else
					UploadValueDAO.Instance.UpdateUploadValue(CType(Me, UploadValue))
					Trace.WriteLine("Updating an existing UploadValue")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
		
		
		End Function

       ''' <summary>
        ''' Delete this UploadValue
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
			Dim childCollectionIndex As Integer = 0
			
			If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
				If Not Me.IsNew Then
					UploadValueDAO.Instance.DeleteUploadValue(CType(Me, UploadValue))
					Trace.WriteLine("Deleting a UploadValue.")
				Else
					Trace.WriteLine("Removing a new unsaved UploadValue.")
				End If
				
				Me.IsDeleted = True
				Me.IsMarkedForDelete = False

			End If
		

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this UploadValue  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.UploadValueID = UploadValue.NextTemporaryId
			Me.IsDirty = True
			Me.IsNew = True
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New UploadValue()
        End Function

#End Region

	End Class

End Namespace

