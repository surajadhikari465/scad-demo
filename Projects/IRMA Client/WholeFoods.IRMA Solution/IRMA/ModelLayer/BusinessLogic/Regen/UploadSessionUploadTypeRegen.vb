
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the UploadSessionUploadType db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadSessionUploadTypeRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _uploadSessionUploadTypeID As System.Int32
		Private _isUploadSessionUploadTypeIDNull As Boolean = True
		Private _uploadSessionID As System.Int32
		Private _isUploadSessionIDNull As Boolean = True
		Private _uploadTypeCode As System.String
		Private _isUploadTypeCodeNull As Boolean = True
		Private _uploadTypeTemplateID As System.Int32
		Private _isUploadTypeTemplateIDNull As Boolean = True
		Private _storeSelectionType As System.String
		Private _isStoreSelectionTypeNull As Boolean = True
		Private _zoneID As System.Int32
		Private _isZoneIDNull As Boolean = True
		Private _state As System.String
		Private _isStateNull As Boolean = True

		Public Overridable Property UploadSessionUploadTypeID() As System.Int32
		    Get
				Return _uploadSessionUploadTypeID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadSessionUploadTypeIDNull = False
				If _uploadSessionUploadTypeID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadSessionUploadTypeID", theOldValue, value)
				End If
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
				_uploadSessionUploadTypeID = value
		    End Set
		End Property

		Public Overridable Property IsUploadSessionUploadTypeIDNull() As Boolean
		    Get
				Return _isUploadSessionUploadTypeIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadSessionUploadTypeIDNull = value
		    End Set
		End Property

		Public Overridable Property UploadSessionID() As System.Int32
		    Get
				Return _uploadSessionID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadSessionIDNull = False
				If _uploadSessionID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadSessionID", theOldValue, value)
				End If
				
			
				_uploadSessionID = value
		    End Set
		End Property

		Public Overridable Property IsUploadSessionIDNull() As Boolean
		    Get
				Return _isUploadSessionIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadSessionIDNull = value
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

		Public Overridable Property UploadTypeTemplateID() As System.Int32
		    Get
				Return _uploadTypeTemplateID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadTypeTemplateIDNull = False
				If _uploadTypeTemplateID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadTypeTemplateID", theOldValue, value)
				End If
				
			
				_uploadTypeTemplateID = value
		    End Set
		End Property

		Public Overridable Property IsUploadTypeTemplateIDNull() As Boolean
		    Get
				Return _isUploadTypeTemplateIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadTypeTemplateIDNull = value
		    End Set
		End Property

		Public Overridable Property StoreSelectionType() As System.String
		    Get
				Return _storeSelectionType
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsStoreSelectionTypeNull = IsNothing(value)
				If (IsNothing(_storeSelectionType) And Not IsNothing(value)) Or _
						(Not IsNothing(_storeSelectionType) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_storeSelectionType) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _storeSelectionType.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "StoreSelectionType", theOldValue, value)
				End If
				
			
				_storeSelectionType = value
		    End Set
		End Property

		Public Overridable Property IsStoreSelectionTypeNull() As Boolean
		    Get
				Return _isStoreSelectionTypeNull
		    End Get
		    Set(ByVal value As Boolean)
				_isStoreSelectionTypeNull = value
		    End Set
		End Property

		Public Overridable Property ZoneID() As System.Int32
		    Get
				Return _zoneID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsZoneIDNull = False
				If _zoneID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ZoneID", theOldValue, value)
				End If
				
			
				_zoneID = value
		    End Set
		End Property

		Public Overridable Property IsZoneIDNull() As Boolean
		    Get
				Return _isZoneIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isZoneIDNull = value
		    End Set
		End Property

		Public Overridable Property State() As System.String
		    Get
				Return _state
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsStateNull = IsNothing(value)
				If (IsNothing(_state) And Not IsNothing(value)) Or _
						(Not IsNothing(_state) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_state) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _state.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "State", theOldValue, value)
				End If
				
			
				_state = value
		    End Set
		End Property

		Public Overridable Property IsStateNull() As Boolean
		    Get
				Return _isStateNull
		    End Get
		    Set(ByVal value As Boolean)
				_isStateNull = value
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
			Me.UploadSessionUploadTypeID = UploadSessionUploadType.NextTemporaryId
		End Sub
		
		''' <summary>
        ''' Construct a UploadSessionUploadType by passing in
		''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
		Public Sub New(ByRef inUploadTypeTemplate As UploadTypeTemplate, ByRef inUploadType As UploadType, ByRef inUploadSession As UploadSession)
		
			Me.UploadSessionUploadTypeID = UploadSessionUploadType.NextTemporaryId

			Me.UploadTypeTemplate = inUploadTypeTemplate
			
			If Not IsNothing(inUploadTypeTemplate) Then
				Me.UploadTypeTemplateID = inUploadTypeTemplate.UploadTypeTemplateID
				inUploadTypeTemplate.AddUploadSessionUploadType(CType(Me, UploadSessionUploadType))
			End If
			
			Me.UploadType = inUploadType
			
			If Not IsNothing(inUploadType) Then
				Me.UploadTypeCode = inUploadType.UploadTypeCode
				inUploadType.AddUploadSessionUploadType(CType(Me, UploadSessionUploadType))
			End If
			
			Me.UploadSession = inUploadSession
			
			If Not IsNothing(inUploadSession) Then
				Me.UploadSessionID = inUploadSession.UploadSessionID
				inUploadSession.AddUploadSessionUploadType(CType(Me, UploadSessionUploadType))
			End If
			
        End Sub

#End Region

#Region "Just-in-time Instantiated Parents"

		Private _isUploadTypeTemplateLoaded As Boolean = False
		
		        Public Property IsUploadTypeTemplateLoaded() As Boolean
            Get
                Return _isUploadTypeTemplateLoaded
            End Get
            Set(ByVal value As Boolean)
                _isUploadTypeTemplateLoaded = value
            End Set
        End Property
 
		Private _uploadTypeTemplate As UploadTypeTemplate = Nothing

	    ''' <summary>
        ''' Set this UploadSessionUploadType UploadTypeTemplate parent
		''' and set the foreign key.
		''' This UploadSessionUploadType is not added to the parent's UploadSessionUploadTypeCollection.
		''' You will have to do this yourself.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadTypeTemplate() As UploadTypeTemplate
		    Get
		    	If Not _isUploadTypeTemplateLoaded Then
				
					' set his once we try to load the parent so we won't keep trying if a parent
					' is not found and the reference remains Nothing
					_isUploadTypeTemplateLoaded = True
					
		    		_uploadTypeTemplate = _
		    			UploadTypeTemplateDAO.Instance.GetUploadTypeTemplateByPK(Me.UploadTypeTemplateID)
		    	End If
			Return _uploadTypeTemplate
		    End Get
		    Set(ByVal value As UploadTypeTemplate)
				_uploadTypeTemplate = value
				
				_isUploadTypeTemplateLoaded = True

				' set the FKs but DO NOT add this BusinessObject to the parent's corresponding child collection
				' the developer will have to do this
				If Not IsNothing(value) Then
					Me.UploadTypeTemplateID = _uploadTypeTemplate.UploadTypeTemplateID
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
        ''' Set this UploadSessionUploadType UploadType parent
		''' and set the foreign key.
		''' This UploadSessionUploadType is not added to the parent's UploadSessionUploadTypeCollection.
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
		
		Private _isUploadSessionLoaded As Boolean = False
		
		        Public Property IsUploadSessionLoaded() As Boolean
            Get
                Return _isUploadSessionLoaded
            End Get
            Set(ByVal value As Boolean)
                _isUploadSessionLoaded = value
            End Set
        End Property
 
		Private _uploadSession As UploadSession = Nothing

	    ''' <summary>
        ''' Set this UploadSessionUploadType UploadSession parent
		''' and set the foreign key.
		''' This UploadSessionUploadType is not added to the parent's UploadSessionUploadTypeCollection.
		''' You will have to do this yourself.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadSession() As UploadSession
		    Get
		    	If Not _isUploadSessionLoaded Then
				
					' set his once we try to load the parent so we won't keep trying if a parent
					' is not found and the reference remains Nothing
					_isUploadSessionLoaded = True
					
		    		_uploadSession = _
		    			UploadSessionDAO.Instance.GetUploadSessionByPK(Me.UploadSessionID)
		    	End If
			Return _uploadSession
		    End Get
		    Set(ByVal value As UploadSession)
				_uploadSession = value
				
				_isUploadSessionLoaded = True

				' set the FKs but DO NOT add this BusinessObject to the parent's corresponding child collection
				' the developer will have to do this
				If Not IsNothing(value) Then
					Me.UploadSessionID = _uploadSession.UploadSessionID
				End If
		    End Set
		End Property
		
#End Region

#Region "Just-in-time Instantiated Children Collections"

		Private _uploadSessionUploadTypeStoreCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadSessionUploadType's collection of UploadSessionUploadTypeStores.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadSessionUploadTypeStoreCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadSessionUploadTypeStoreCollection) Then
					If Not Me.IsNew Then
						_uploadSessionUploadTypeStoreCollection = _
							UploadSessionUploadTypeStoreDAO.Instance.GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeID(Me.UploadSessionUploadTypeID)
		    		Else
						_uploadSessionUploadTypeStoreCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore In _
							_uploadSessionUploadTypeStoreCollection
						theUploadSessionUploadTypeStore.UploadSessionUploadType = CType(Me, UploadSessionUploadType)
					Next
									
				' subscribe to the PropertyChanged event that bubbles up from the BusinessObjects through the BusinessObjectCollection
				' this will set this BusinessObject's IsDirty flag when that of a child object changes
				AddHandler _uploadSessionUploadTypeStoreCollection.PropertyChanged, AddressOf Me.PropertyChangedHandler
				
				End If
			Return _uploadSessionUploadTypeStoreCollection
		    End Get
		    Set(ByVal value As BusinessObjectCollection)
				_uploadSessionUploadTypeStoreCollection = value
		    End Set
		End Property
		
		''' <summary>
        ''' Gets the first UploadSessionUploadTypeStore in this UploadSessionUploadType's
        ''' UploadSessionUploadTypeStoreCollection, if there is one.
        ''' This is a convenience for when a one-to-many is really a one-to-one.
        ''' </summary>
        ''' <value></value>
        ''' <returns>uploadSessionUploadTypeStore</returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property UploadSessionUploadTypeStore() As UploadSessionUploadTypeStore
            Get
                Dim theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore = Nothing

                If Me.UploadSessionUploadTypeStoreCollection.Count > 0 Then
                    theUploadSessionUploadTypeStore = CType(Me.UploadSessionUploadTypeStoreCollection.Item(0), UploadSessionUploadTypeStore)
                End If
                Return theUploadSessionUploadTypeStore
            End Get
        End Property
		
		''' <summary>
        ''' Add the provided UploadSessionUploadTypeStore to this UploadSessionUploadType's UploadSessionUploadTypeStoreCollection
        ''' </summary>
        ''' <param name="inUploadSessionUploadTypeStore"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadSessionUploadTypeStore(ByRef inUploadSessionUploadTypeStore As UploadSessionUploadTypeStore)

            inUploadSessionUploadTypeStore.UploadSessionUploadType = Ctype(Me, UploadSessionUploadType)
            inUploadSessionUploadTypeStore.UploadSessionUploadTypeID = Me.UploadSessionUploadTypeID

            Me.UploadSessionUploadTypeStoreCollection.Add(inUploadSessionUploadTypeStore.PrimaryKey, inUploadSessionUploadTypeStore)
			
        End Sub

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this UploadSessionUploadType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					UploadSessionUploadTypeDAO.Instance.InsertUploadSessionUploadType(CType(Me, UploadSessionUploadType))
					Trace.WriteLine("Inserting a new UploadSessionUploadType")
				Else
					UploadSessionUploadTypeDAO.Instance.UpdateUploadSessionUploadType(CType(Me, UploadSessionUploadType))
					Trace.WriteLine("Updating an existing UploadSessionUploadType")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
			' save the UploadSessionUploadTypeStores
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadSessionUploadTypeStoreCollection.SaveProgressCounter = 0
            For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore _
                    In Me.UploadSessionUploadTypeStoreCollection
                theUploadSessionUploadTypeStore.UploadSessionUploadTypeID = Me.UploadSessionUploadTypeID
            	theUploadSessionUploadTypeStore.Save()
				Me.UploadSessionUploadTypeStoreCollection.SaveProgressCounter = Me.UploadSessionUploadTypeStoreCollection.SaveProgressCounter + 1
			Next		
		
			Me.UploadSessionUploadTypeStoreCollection.SaveProgressComplete = True
		
		End Function

       ''' <summary>
        ''' Delete this UploadSessionUploadType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
			Dim childCollectionIndex As Integer = 0
			
			Dim theChild As Object
			' delete the UploadSessionUploadTypeStores
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadSessionUploadTypeStoreCollection.DeleteProgressCounter = 0
			For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore _
					In Me.UploadSessionUploadTypeStoreCollection
				
				' increment the counter only if the UploadSessionUploadTypeStore is deleted
				If Not onlyMarked Or (onlyMarked And (Me.IsMarkedForDelete Or theUploadSessionUploadTypeStore.IsMarkedForDelete)) Then
					Me.UploadSessionUploadTypeStoreCollection.DeleteProgressCounter = Me.UploadSessionUploadTypeStoreCollection.DeleteProgressCounter + 1
				End If
				
				' always delete the children if the parent is deleted
				theUploadSessionUploadTypeStore.Delete(onlyMarked And Not Me.IsMarkedForDelete)
			Next
		
			' remove all deleted children from the UploadSessionUploadTypeStoreCollection
			childCollectionIndex = 0
			Do
				If childCollectionIndex >= Me.UploadSessionUploadTypeStoreCollection.Count Then
					Exit Do
				End If
				theChild = Me.UploadSessionUploadTypeStoreCollection.Item(childCollectionIndex)
				If CType(theChild, UploadSessionUploadTypeStore).IsDeleted Then
					Me.UploadSessionUploadTypeStoreCollection.Remove(theChild)
                Else
					childCollectionIndex = childCollectionIndex + 1
				End If
			Loop
			

			If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
				If Not Me.IsNew Then
					UploadSessionUploadTypeDAO.Instance.DeleteUploadSessionUploadType(CType(Me, UploadSessionUploadType))
					Trace.WriteLine("Deleting a UploadSessionUploadType.")
				Else
					Trace.WriteLine("Removing a new unsaved UploadSessionUploadType.")
				End If
				
				Me.IsDeleted = True
				Me.IsMarkedForDelete = False

			End If
		
			Me.UploadSessionUploadTypeStoreCollection.DeleteProgressComplete = True

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this UploadSessionUploadType  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.UploadSessionUploadTypeID = UploadSessionUploadType.NextTemporaryId
			Me.IsDirty = True
			Me.IsNew = True
		
            For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore _
                    In Me.UploadSessionUploadTypeStoreCollection
                theUploadSessionUploadTypeStore.MakeNew()
			Next
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New UploadSessionUploadType()
        End Function

#End Region

	End Class

End Namespace

