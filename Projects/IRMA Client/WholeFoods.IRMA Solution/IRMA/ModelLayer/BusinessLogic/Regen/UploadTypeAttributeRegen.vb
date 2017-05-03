
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the UploadTypeAttribute db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadTypeAttributeRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _uploadTypeAttributeID As System.Int32
		Private _isUploadTypeAttributeIDNull As Boolean = True
		Private _uploadTypeCode As System.String
		Private _isUploadTypeCodeNull As Boolean = True
		Private _uploadAttributeID As System.Int32
		Private _isUploadAttributeIDNull As Boolean = True
		Private _isRequiredForUploadTypeForExistingItems As System.Boolean
		Private _isIsRequiredForUploadTypeForExistingItemsNull As Boolean = True
		Private _isReadOnlyForExistingItems As System.Boolean
		Private _isIsReadOnlyForExistingItemsNull As Boolean = True
		Private _isHidden As System.Boolean
		Private _isIsHiddenNull As Boolean = True
		Private _gridPosition As System.Int32
		Private _isGridPositionNull As Boolean = True
		Private _isRequiredForUploadTypeForNewItems As System.Boolean
		Private _isIsRequiredForUploadTypeForNewItemsNull As Boolean = True
		Private _isReadOnlyForNewItems As System.Boolean
		Private _isIsReadOnlyForNewItemsNull As Boolean = True
		Private _groupName As System.String
		Private _isGroupNameNull As Boolean = True

		Public Overridable Property UploadTypeAttributeID() As System.Int32
		    Get
				Return _uploadTypeAttributeID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadTypeAttributeIDNull = False
				If _uploadTypeAttributeID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadTypeAttributeID", theOldValue, value)
				End If
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
				_uploadTypeAttributeID = value
		    End Set
		End Property

		Public Overridable Property IsUploadTypeAttributeIDNull() As Boolean
		    Get
				Return _isUploadTypeAttributeIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadTypeAttributeIDNull = value
		    End Set
		End Property

		Public Overridable Property UploadTypeCode() As System.String
		    Get
				Return _uploadTypeCode
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadTypeCodeNull = IsNothing(value)
				If (IsNothing(_uploadTypeCode) And Not IsNothing(value)) Or _
						(Not IsNothing(_uploadTypeCode) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_uploadTypeCode) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _uploadTypeCode.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadTypeCode", theOldValue, value)
				End If
				
			
				_uploadTypeCode = value
		    End Set
		End Property

		Public Overridable Property IsUploadTypeCodeNull() As Boolean
		    Get
				Return _isUploadTypeCodeNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadTypeCodeNull = value
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

		Public Overridable Property IsRequiredForUploadTypeForExistingItems() As System.Boolean
		    Get
				Return _isRequiredForUploadTypeForExistingItems
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsRequiredForUploadTypeForExistingItemsNull = False
				If _isRequiredForUploadTypeForExistingItems <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsRequiredForUploadTypeForExistingItems", theOldValue, value)
				End If
				
			
				_isRequiredForUploadTypeForExistingItems = value
		    End Set
		End Property

		Public Overridable Property IsIsRequiredForUploadTypeForExistingItemsNull() As Boolean
		    Get
				Return _isIsRequiredForUploadTypeForExistingItemsNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsRequiredForUploadTypeForExistingItemsNull = value
		    End Set
		End Property

		Public Overridable Property IsReadOnlyForExistingItems() As System.Boolean
		    Get
				Return _isReadOnlyForExistingItems
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsReadOnlyForExistingItemsNull = False
				If _isReadOnlyForExistingItems <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsReadOnlyForExistingItems", theOldValue, value)
				End If
				
			
				_isReadOnlyForExistingItems = value
		    End Set
		End Property

		Public Overridable Property IsIsReadOnlyForExistingItemsNull() As Boolean
		    Get
				Return _isIsReadOnlyForExistingItemsNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsReadOnlyForExistingItemsNull = value
		    End Set
		End Property

		Public Overridable Property IsHidden() As System.Boolean
		    Get
				Return _isHidden
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsHiddenNull = False
				If _isHidden <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsHidden", theOldValue, value)
				End If
				
			
				_isHidden = value
		    End Set
		End Property

		Public Overridable Property IsIsHiddenNull() As Boolean
		    Get
				Return _isIsHiddenNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsHiddenNull = value
		    End Set
		End Property

		Public Overridable Property GridPosition() As System.Int32
		    Get
				Return _gridPosition
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsGridPositionNull = False
				If _gridPosition <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "GridPosition", theOldValue, value)
				End If
				
			
				_gridPosition = value
		    End Set
		End Property

		Public Overridable Property IsGridPositionNull() As Boolean
		    Get
				Return _isGridPositionNull
		    End Get
		    Set(ByVal value As Boolean)
				_isGridPositionNull = value
		    End Set
		End Property

		Public Overridable Property IsRequiredForUploadTypeForNewItems() As System.Boolean
		    Get
				Return _isRequiredForUploadTypeForNewItems
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsRequiredForUploadTypeForNewItemsNull = False
				If _isRequiredForUploadTypeForNewItems <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsRequiredForUploadTypeForNewItems", theOldValue, value)
				End If
				
			
				_isRequiredForUploadTypeForNewItems = value
		    End Set
		End Property

		Public Overridable Property IsIsRequiredForUploadTypeForNewItemsNull() As Boolean
		    Get
				Return _isIsRequiredForUploadTypeForNewItemsNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsRequiredForUploadTypeForNewItemsNull = value
		    End Set
		End Property

		Public Overridable Property IsReadOnlyForNewItems() As System.Boolean
		    Get
				Return _isReadOnlyForNewItems
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsReadOnlyForNewItemsNull = False
				If _isReadOnlyForNewItems <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsReadOnlyForNewItems", theOldValue, value)
				End If
				
			
				_isReadOnlyForNewItems = value
		    End Set
		End Property

		Public Overridable Property IsIsReadOnlyForNewItemsNull() As Boolean
		    Get
				Return _isIsReadOnlyForNewItemsNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsReadOnlyForNewItemsNull = value
		    End Set
		End Property

		Public Overridable Property GroupName() As System.String
		    Get
				Return _groupName
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsGroupNameNull = IsNothing(value)
				If (IsNothing(_groupName) And Not IsNothing(value)) Or _
						(Not IsNothing(_groupName) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_groupName) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _groupName.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "GroupName", theOldValue, value)
				End If
				
			
				_groupName = value
		    End Set
		End Property

		Public Overridable Property IsGroupNameNull() As Boolean
		    Get
				Return _isGroupNameNull
		    End Get
		    Set(ByVal value As Boolean)
				_isGroupNameNull = value
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
			Me.UploadTypeAttributeID = UploadTypeAttribute.NextTemporaryId
		End Sub
		
		''' <summary>
        ''' Construct a UploadTypeAttribute by passing in
		''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
		Public Sub New(ByRef inUploadAttribute As UploadAttribute, ByRef inUploadType As UploadType)
		
			Me.UploadTypeAttributeID = UploadTypeAttribute.NextTemporaryId

			Me.UploadAttribute = inUploadAttribute
			
			If Not IsNothing(inUploadAttribute) Then
				Me.UploadAttributeID = inUploadAttribute.UploadAttributeID
				inUploadAttribute.AddUploadTypeAttribute(CType(Me, UploadTypeAttribute))
			End If
			
			Me.UploadType = inUploadType
			
			If Not IsNothing(inUploadType) Then
				Me.UploadTypeCode = inUploadType.UploadTypeCode
				inUploadType.AddUploadTypeAttribute(CType(Me, UploadTypeAttribute))
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
        ''' Set this UploadTypeAttribute UploadAttribute parent
		''' and set the foreign key.
		''' This UploadTypeAttribute is not added to the parent's UploadTypeAttributeCollection.
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
		
		Private _isUploadTypeLoaded As Boolean = False
		
		        Public Property IsUploadTypeLoaded() As Boolean
            Get
                Return _isUploadTypeLoaded
            End Get
            Set(ByVal value As Boolean)
                _isUploadTypeLoaded = value
            End Set
        End Property
 
		Private _uploadType As UploadType = Nothing

	    ''' <summary>
        ''' Set this UploadTypeAttribute UploadType parent
		''' and set the foreign key.
		''' This UploadTypeAttribute is not added to the parent's UploadTypeAttributeCollection.
		''' You will have to do this yourself.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadType() As UploadType
		    Get
		    	If Not _isUploadTypeLoaded Then
				
					' set his once we try to load the parent so we won't keep trying if a parent
					' is not found and the reference remains Nothing
					_isUploadTypeLoaded = True
					
		    		_uploadType = _
		    			UploadTypeDAO.Instance.GetUploadTypeByPK(Me.UploadTypeCode)
		    	End If
			Return _uploadType
		    End Get
		    Set(ByVal value As UploadType)
				_uploadType = value
				
				_isUploadTypeLoaded = True

				' set the FKs but DO NOT add this BusinessObject to the parent's corresponding child collection
				' the developer will have to do this
				If Not IsNothing(value) Then
					Me.UploadTypeCode = _uploadType.UploadTypeCode
				End If
		    End Set
		End Property
		
#End Region

#Region "Just-in-time Instantiated Children Collections"

		Private _uploadTypeTemplateAttributeCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadTypeAttribute's collection of UploadTypeTemplateAttributes.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadTypeTemplateAttributeCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadTypeTemplateAttributeCollection) Then
					If Not Me.IsNew Then
						_uploadTypeTemplateAttributeCollection = _
							UploadTypeTemplateAttributeDAO.Instance.GetUploadTypeTemplateAttributesByUploadTypeAttributeID(Me.UploadTypeAttributeID)
		    		Else
						_uploadTypeTemplateAttributeCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadTypeTemplateAttribute As UploadTypeTemplateAttribute In _
							_uploadTypeTemplateAttributeCollection
						theUploadTypeTemplateAttribute.UploadTypeAttribute = CType(Me, UploadTypeAttribute)
					Next
									
				' subscribe to the PropertyChanged event that bubbles up from the BusinessObjects through the BusinessObjectCollection
				' this will set this BusinessObject's IsDirty flag when that of a child object changes
				AddHandler _uploadTypeTemplateAttributeCollection.PropertyChanged, AddressOf Me.PropertyChangedHandler
				
				End If
			Return _uploadTypeTemplateAttributeCollection
		    End Get
		    Set(ByVal value As BusinessObjectCollection)
				_uploadTypeTemplateAttributeCollection = value
		    End Set
		End Property
		
		''' <summary>
        ''' Gets the first UploadTypeTemplateAttribute in this UploadTypeAttribute's
        ''' UploadTypeTemplateAttributeCollection, if there is one.
        ''' This is a convenience for when a one-to-many is really a one-to-one.
        ''' </summary>
        ''' <value></value>
        ''' <returns>uploadTypeTemplateAttribute</returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property UploadTypeTemplateAttribute() As UploadTypeTemplateAttribute
            Get
                Dim theUploadTypeTemplateAttribute As UploadTypeTemplateAttribute = Nothing

                If Me.UploadTypeTemplateAttributeCollection.Count > 0 Then
                    theUploadTypeTemplateAttribute = CType(Me.UploadTypeTemplateAttributeCollection.Item(0), UploadTypeTemplateAttribute)
                End If
                Return theUploadTypeTemplateAttribute
            End Get
        End Property
		
		''' <summary>
        ''' Add the provided UploadTypeTemplateAttribute to this UploadTypeAttribute's UploadTypeTemplateAttributeCollection
        ''' </summary>
        ''' <param name="inUploadTypeTemplateAttribute"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadTypeTemplateAttribute(ByRef inUploadTypeTemplateAttribute As UploadTypeTemplateAttribute)

            inUploadTypeTemplateAttribute.UploadTypeAttribute = Ctype(Me, UploadTypeAttribute)
            inUploadTypeTemplateAttribute.UploadTypeAttributeID = Me.UploadTypeAttributeID

            Me.UploadTypeTemplateAttributeCollection.Add(inUploadTypeTemplateAttribute.PrimaryKey, inUploadTypeTemplateAttribute)
			
        End Sub

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this UploadTypeAttribute
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					UploadTypeAttributeDAO.Instance.InsertUploadTypeAttribute(CType(Me, UploadTypeAttribute))
					Trace.WriteLine("Inserting a new UploadTypeAttribute")
				Else
					UploadTypeAttributeDAO.Instance.UpdateUploadTypeAttribute(CType(Me, UploadTypeAttribute))
					Trace.WriteLine("Updating an existing UploadTypeAttribute")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
			' save the UploadTypeTemplateAttributes
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadTypeTemplateAttributeCollection.SaveProgressCounter = 0
            For Each theUploadTypeTemplateAttribute As UploadTypeTemplateAttribute _
                    In Me.UploadTypeTemplateAttributeCollection
                theUploadTypeTemplateAttribute.UploadTypeAttributeID = Me.UploadTypeAttributeID
            	theUploadTypeTemplateAttribute.Save()
				Me.UploadTypeTemplateAttributeCollection.SaveProgressCounter = Me.UploadTypeTemplateAttributeCollection.SaveProgressCounter + 1
			Next		
		
			Me.UploadTypeTemplateAttributeCollection.SaveProgressComplete = True
		
		End Function

       ''' <summary>
        ''' Delete this UploadTypeAttribute
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
			Dim childCollectionIndex As Integer = 0
			
			Dim theChild As Object
			' delete the UploadTypeTemplateAttributes
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadTypeTemplateAttributeCollection.DeleteProgressCounter = 0
			For Each theUploadTypeTemplateAttribute As UploadTypeTemplateAttribute _
					In Me.UploadTypeTemplateAttributeCollection
				
				' increment the counter only if the UploadTypeTemplateAttribute is deleted
				If Not onlyMarked Or (onlyMarked And (Me.IsMarkedForDelete Or theUploadTypeTemplateAttribute.IsMarkedForDelete)) Then
					Me.UploadTypeTemplateAttributeCollection.DeleteProgressCounter = Me.UploadTypeTemplateAttributeCollection.DeleteProgressCounter + 1
				End If
				
				' always delete the children if the parent is deleted
				theUploadTypeTemplateAttribute.Delete(onlyMarked And Not Me.IsMarkedForDelete)
			Next
		
			' remove all deleted children from the UploadTypeTemplateAttributeCollection
			childCollectionIndex = 0
			Do
				If childCollectionIndex >= Me.UploadTypeTemplateAttributeCollection.Count Then
					Exit Do
				End If
				theChild = Me.UploadTypeTemplateAttributeCollection.Item(childCollectionIndex)
				If CType(theChild, UploadTypeTemplateAttribute).IsDeleted Then
					Me.UploadTypeTemplateAttributeCollection.Remove(theChild)
                Else
					childCollectionIndex = childCollectionIndex + 1
				End If
			Loop
			

			If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
				If Not Me.IsNew Then
					UploadTypeAttributeDAO.Instance.DeleteUploadTypeAttribute(CType(Me, UploadTypeAttribute))
					Trace.WriteLine("Deleting a UploadTypeAttribute.")
				Else
					Trace.WriteLine("Removing a new unsaved UploadTypeAttribute.")
				End If
				
				Me.IsDeleted = True
				Me.IsMarkedForDelete = False

			End If
		
			Me.UploadTypeTemplateAttributeCollection.DeleteProgressComplete = True

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this UploadTypeAttribute  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.UploadTypeAttributeID = UploadTypeAttribute.NextTemporaryId
			Me.IsDirty = True
			Me.IsNew = True
		
            For Each theUploadTypeTemplateAttribute As UploadTypeTemplateAttribute _
                    In Me.UploadTypeTemplateAttributeCollection
                theUploadTypeTemplateAttribute.MakeNew()
			Next
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New UploadTypeAttribute()
        End Function

#End Region

	End Class

End Namespace

