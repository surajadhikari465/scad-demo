
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the UploadSession db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadSessionRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _uploadSessionID As System.Int32
		Private _isUploadSessionIDNull As Boolean = True
		Private _name As System.String
		Private _isNameNull As Boolean = True
		Private _isUploaded As System.Boolean
		Private _isIsUploadedNull As Boolean = True
		Private _itemsProcessedCount As System.Int32
		Private _isItemsProcessedCountNull As Boolean = True
		Private _itemsLoadedCount As System.Int32
		Private _isItemsLoadedCountNull As Boolean = True
		Private _errorsCount As System.Int32
		Private _isErrorsCountNull As Boolean = True
		Private _emailToAddress As System.String
		Private _isEmailToAddressNull As Boolean = True
		Private _createdByUserID As System.Int32
		Private _isCreatedByUserIDNull As Boolean = True
		Private _createdDateTime As System.DateTime
		Private _isCreatedDateTimeNull As Boolean = True
		Private _modifiedByUserID As System.Int32
		Private _isModifiedByUserIDNull As Boolean = True
		Private _modifiedDateTime As System.DateTime
		Private _isModifiedDateTimeNull As Boolean = True
		Private _isNewItemSessionFlag As System.Boolean
        Private _isIsNewItemSessionFlagNull As Boolean = True
        Private _isDeleteItemSessionFlag As System.Boolean
        Private _isIsDeleteItemSessionFlagNull As Boolean = True
		Private _isFromSLIM As System.Boolean
		Private _isIsFromSLIMNull As Boolean = True

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
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
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

		Public Overridable Property Name() As System.String
		    Get
				Return _name
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsNameNull = IsNothing(value)
				If (IsNothing(_name) And Not IsNothing(value)) Or _
						(Not IsNothing(_name) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_name) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _name.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "Name", theOldValue, value)
				End If
				
			
				_name = value
		    End Set
		End Property

		Public Overridable Property IsNameNull() As Boolean
		    Get
				Return _isNameNull
		    End Get
		    Set(ByVal value As Boolean)
				_isNameNull = value
		    End Set
		End Property

		Public Overridable Property IsUploaded() As System.Boolean
		    Get
				Return _isUploaded
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsUploadedNull = False
				If _isUploaded <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsUploaded", theOldValue, value)
				End If
				
			
				_isUploaded = value
		    End Set
		End Property

		Public Overridable Property IsIsUploadedNull() As Boolean
		    Get
				Return _isIsUploadedNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsUploadedNull = value
		    End Set
		End Property

		Public Overridable Property ItemsProcessedCount() As System.Int32
		    Get
				Return _itemsProcessedCount
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsItemsProcessedCountNull = False
				If _itemsProcessedCount <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ItemsProcessedCount", theOldValue, value)
				End If
				
			
				_itemsProcessedCount = value
		    End Set
		End Property

		Public Overridable Property IsItemsProcessedCountNull() As Boolean
		    Get
				Return _isItemsProcessedCountNull
		    End Get
		    Set(ByVal value As Boolean)
				_isItemsProcessedCountNull = value
		    End Set
		End Property

		Public Overridable Property ItemsLoadedCount() As System.Int32
		    Get
				Return _itemsLoadedCount
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsItemsLoadedCountNull = False
				If _itemsLoadedCount <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ItemsLoadedCount", theOldValue, value)
				End If
				
			
				_itemsLoadedCount = value
		    End Set
		End Property

		Public Overridable Property IsItemsLoadedCountNull() As Boolean
		    Get
				Return _isItemsLoadedCountNull
		    End Get
		    Set(ByVal value As Boolean)
				_isItemsLoadedCountNull = value
		    End Set
		End Property

		Public Overridable Property ErrorsCount() As System.Int32
		    Get
				Return _errorsCount
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsErrorsCountNull = False
				If _errorsCount <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ErrorsCount", theOldValue, value)
				End If
				
			
				_errorsCount = value
		    End Set
		End Property

		Public Overridable Property IsErrorsCountNull() As Boolean
		    Get
				Return _isErrorsCountNull
		    End Get
		    Set(ByVal value As Boolean)
				_isErrorsCountNull = value
		    End Set
		End Property

		Public Overridable Property EmailToAddress() As System.String
		    Get
				Return _emailToAddress
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsEmailToAddressNull = IsNothing(value)
				If (IsNothing(_emailToAddress) And Not IsNothing(value)) Or _
						(Not IsNothing(_emailToAddress) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_emailToAddress) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _emailToAddress.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "EmailToAddress", theOldValue, value)
				End If
				
			
				_emailToAddress = value
		    End Set
		End Property

		Public Overridable Property IsEmailToAddressNull() As Boolean
		    Get
				Return _isEmailToAddressNull
		    End Get
		    Set(ByVal value As Boolean)
				_isEmailToAddressNull = value
		    End Set
		End Property

		Public Overridable Property CreatedByUserID() As System.Int32
		    Get
				Return _createdByUserID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsCreatedByUserIDNull = False
				If _createdByUserID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "CreatedByUserID", theOldValue, value)
				End If
				
			
				_createdByUserID = value
		    End Set
		End Property

		Public Overridable Property IsCreatedByUserIDNull() As Boolean
		    Get
				Return _isCreatedByUserIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isCreatedByUserIDNull = value
		    End Set
		End Property

		Public Overridable Property CreatedDateTime() As System.DateTime
		    Get
				Return _createdDateTime
		    End Get
		    Set(ByVal value As System.DateTime)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsCreatedDateTimeNull = False
				If _createdDateTime <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "CreatedDateTime", theOldValue, value)
				End If
				
			
				_createdDateTime = value
		    End Set
		End Property

		Public Overridable Property IsCreatedDateTimeNull() As Boolean
		    Get
				Return _isCreatedDateTimeNull
		    End Get
		    Set(ByVal value As Boolean)
				_isCreatedDateTimeNull = value
		    End Set
		End Property

		Public Overridable Property ModifiedByUserID() As System.Int32
		    Get
				Return _modifiedByUserID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsModifiedByUserIDNull = False
				If _modifiedByUserID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ModifiedByUserID", theOldValue, value)
				End If
				
			
				_modifiedByUserID = value
		    End Set
		End Property

		Public Overridable Property IsModifiedByUserIDNull() As Boolean
		    Get
				Return _isModifiedByUserIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isModifiedByUserIDNull = value
		    End Set
		End Property

		Public Overridable Property ModifiedDateTime() As System.DateTime
		    Get
				Return _modifiedDateTime
		    End Get
		    Set(ByVal value As System.DateTime)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsModifiedDateTimeNull = False
				If _modifiedDateTime <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ModifiedDateTime", theOldValue, value)
				End If
				
			
				_modifiedDateTime = value
		    End Set
		End Property

		Public Overridable Property IsModifiedDateTimeNull() As Boolean
		    Get
				Return _isModifiedDateTimeNull
		    End Get
		    Set(ByVal value As Boolean)
				_isModifiedDateTimeNull = value
		    End Set
		End Property

		Public Overridable Property IsNewItemSessionFlag() As System.Boolean
		    Get
				Return _isNewItemSessionFlag
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsNewItemSessionFlagNull = False
				If _isNewItemSessionFlag <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsNewItemSessionFlag", theOldValue, value)
				End If
				
			
				_isNewItemSessionFlag = value
		    End Set
        End Property
        Public Overridable Property IsDeleteItemSessionFlag() As System.Boolean
            Get
                Return _isDeleteItemSessionFlag
            End Get
            Set(ByVal value As System.Boolean)

                Dim theOldValue As Object = Nothing

                ' set the IsDirty and is null flags
                Me.IsIsDeleteItemSessionFlagNull = False
                If _isDeleteItemSessionFlag <> value Then
                    Me.IsDirty = True
                End If

                If Me.IsDirty Then
                    RaisePropertyChangedEvent(Me, "IsDeleteItemSessionFlag", theOldValue, value)
                End If


                _isDeleteItemSessionFlag = value
            End Set
        End Property

		Public Overridable Property IsIsNewItemSessionFlagNull() As Boolean
		    Get
				Return _isIsNewItemSessionFlagNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsNewItemSessionFlagNull = value
		    End Set
        End Property
        Public Overridable Property IsIsDeleteItemSessionFlagNull() As Boolean
            Get
                Return _isIsDeleteItemSessionFlagNull
            End Get
            Set(ByVal value As Boolean)
                _isIsDeleteItemSessionFlagNull = value
            End Set
        End Property

		Public Overridable Property IsFromSLIM() As System.Boolean
		    Get
				Return _isFromSLIM
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsFromSLIMNull = False
				If _isFromSLIM <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsFromSLIM", theOldValue, value)
				End If
				
			
				_isFromSLIM = value
		    End Set
		End Property

		Public Overridable Property IsIsFromSLIMNull() As Boolean
		    Get
				Return _isIsFromSLIMNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsFromSLIMNull = value
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
			Me.UploadSessionID = UploadSession.NextTemporaryId
		End Sub
		

#End Region

#Region "Just-in-time Instantiated Parents"

#End Region

#Region "Just-in-time Instantiated Children Collections"

		Private _uploadRowCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadSession's collection of UploadRows.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadRowCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadRowCollection) Then
					If Not Me.IsNew Then
						_uploadRowCollection = _
							UploadRowDAO.Instance.GetUploadRowsByUploadSessionID(Me.UploadSessionID)
		    		Else
						_uploadRowCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadRow As UploadRow In _
							_uploadRowCollection
						theUploadRow.UploadSession = CType(Me, UploadSession)
					Next
									
				' subscribe to the PropertyChanged event that bubbles up from the BusinessObjects through the BusinessObjectCollection
				' this will set this BusinessObject's IsDirty flag when that of a child object changes
				AddHandler _uploadRowCollection.PropertyChanged, AddressOf Me.PropertyChangedHandler
				
				End If
			Return _uploadRowCollection
		    End Get
		    Set(ByVal value As BusinessObjectCollection)
				_uploadRowCollection = value
		    End Set
		End Property
		
		''' <summary>
        ''' Gets the first UploadRow in this UploadSession's
        ''' UploadRowCollection, if there is one.
        ''' This is a convenience for when a one-to-many is really a one-to-one.
        ''' </summary>
        ''' <value></value>
        ''' <returns>uploadRow</returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property UploadRow() As UploadRow
            Get
                Dim theUploadRow As UploadRow = Nothing

                If Me.UploadRowCollection.Count > 0 Then
                    theUploadRow = CType(Me.UploadRowCollection.Item(0), UploadRow)
                End If
                Return theUploadRow
            End Get
        End Property
		
		''' <summary>
        ''' Add the provided UploadRow to this UploadSession's UploadRowCollection
        ''' </summary>
        ''' <param name="inUploadRow"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadRow(ByRef inUploadRow As UploadRow)

            inUploadRow.UploadSession = Ctype(Me, UploadSession)
            inUploadRow.UploadSessionID = Me.UploadSessionID

            Me.UploadRowCollection.Add(inUploadRow.PrimaryKey, inUploadRow)
			
        End Sub

		Private _uploadSessionUploadTypeCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadSession's collection of UploadSessionUploadTypes.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadSessionUploadTypeCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadSessionUploadTypeCollection) Then
					If Not Me.IsNew Then
						_uploadSessionUploadTypeCollection = _
							UploadSessionUploadTypeDAO.Instance.GetUploadSessionUploadTypesByUploadSessionID(Me.UploadSessionID)
		    		Else
						_uploadSessionUploadTypeCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadSessionUploadType As UploadSessionUploadType In _
							_uploadSessionUploadTypeCollection
						theUploadSessionUploadType.UploadSession = CType(Me, UploadSession)
					Next
									
				' subscribe to the PropertyChanged event that bubbles up from the BusinessObjects through the BusinessObjectCollection
				' this will set this BusinessObject's IsDirty flag when that of a child object changes
				AddHandler _uploadSessionUploadTypeCollection.PropertyChanged, AddressOf Me.PropertyChangedHandler
				
				End If
			Return _uploadSessionUploadTypeCollection
		    End Get
		    Set(ByVal value As BusinessObjectCollection)
				_uploadSessionUploadTypeCollection = value
		    End Set
		End Property
		
		''' <summary>
        ''' Gets the first UploadSessionUploadType in this UploadSession's
        ''' UploadSessionUploadTypeCollection, if there is one.
        ''' This is a convenience for when a one-to-many is really a one-to-one.
        ''' </summary>
        ''' <value></value>
        ''' <returns>uploadSessionUploadType</returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property UploadSessionUploadType() As UploadSessionUploadType
            Get
                Dim theUploadSessionUploadType As UploadSessionUploadType = Nothing

                If Me.UploadSessionUploadTypeCollection.Count > 0 Then
                    theUploadSessionUploadType = CType(Me.UploadSessionUploadTypeCollection.Item(0), UploadSessionUploadType)
                End If
                Return theUploadSessionUploadType
            End Get
        End Property
		
		''' <summary>
        ''' Add the provided UploadSessionUploadType to this UploadSession's UploadSessionUploadTypeCollection
        ''' </summary>
        ''' <param name="inUploadSessionUploadType"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadSessionUploadType(ByRef inUploadSessionUploadType As UploadSessionUploadType)

            inUploadSessionUploadType.UploadSession = Ctype(Me, UploadSession)
            inUploadSessionUploadType.UploadSessionID = Me.UploadSessionID

            Me.UploadSessionUploadTypeCollection.Add(inUploadSessionUploadType.PrimaryKey, inUploadSessionUploadType)
			
        End Sub

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this UploadSession
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					UploadSessionDAO.Instance.InsertUploadSession(CType(Me, UploadSession))
					Trace.WriteLine("Inserting a new UploadSession")
				Else
					UploadSessionDAO.Instance.UpdateUploadSession(CType(Me, UploadSession))
					Trace.WriteLine("Updating an existing UploadSession")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
			' save the UploadRows
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadRowCollection.SaveProgressCounter = 0
            For Each theUploadRow As UploadRow _
                    In Me.UploadRowCollection
                theUploadRow.UploadSessionID = Me.UploadSessionID
            	theUploadRow.Save()
				Me.UploadRowCollection.SaveProgressCounter = Me.UploadRowCollection.SaveProgressCounter + 1
			Next		
			' save the UploadSessionUploadTypes
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadSessionUploadTypeCollection.SaveProgressCounter = 0
            For Each theUploadSessionUploadType As UploadSessionUploadType _
                    In Me.UploadSessionUploadTypeCollection
                theUploadSessionUploadType.UploadSessionID = Me.UploadSessionID
            	theUploadSessionUploadType.Save()
				Me.UploadSessionUploadTypeCollection.SaveProgressCounter = Me.UploadSessionUploadTypeCollection.SaveProgressCounter + 1
			Next		
		
			Me.UploadRowCollection.SaveProgressComplete = True
			Me.UploadSessionUploadTypeCollection.SaveProgressComplete = True
		
		End Function

       ''' <summary>
        ''' Delete this UploadSession
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
			Dim childCollectionIndex As Integer = 0
			
			Dim theChild As Object
			' delete the UploadRows
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadRowCollection.DeleteProgressCounter = 0
			For Each theUploadRow As UploadRow _
					In Me.UploadRowCollection
				
				' increment the counter only if the UploadRow is deleted
				If Not onlyMarked Or (onlyMarked And (Me.IsMarkedForDelete Or theUploadRow.IsMarkedForDelete)) Then
					Me.UploadRowCollection.DeleteProgressCounter = Me.UploadRowCollection.DeleteProgressCounter + 1
				End If
				
				' always delete the children if the parent is deleted
				theUploadRow.Delete(onlyMarked And Not Me.IsMarkedForDelete)
			Next
		
			' remove all deleted children from the UploadRowCollection
			childCollectionIndex = 0
			Do
				If childCollectionIndex >= Me.UploadRowCollection.Count Then
					Exit Do
				End If
				theChild = Me.UploadRowCollection.Item(childCollectionIndex)
				If CType(theChild, UploadRow).IsDeleted Then
					Me.UploadRowCollection.Remove(theChild)
                Else
					childCollectionIndex = childCollectionIndex + 1
				End If
			Loop
			
			' delete the UploadSessionUploadTypes
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadSessionUploadTypeCollection.DeleteProgressCounter = 0
			For Each theUploadSessionUploadType As UploadSessionUploadType _
					In Me.UploadSessionUploadTypeCollection
				
				' increment the counter only if the UploadSessionUploadType is deleted
				If Not onlyMarked Or (onlyMarked And (Me.IsMarkedForDelete Or theUploadSessionUploadType.IsMarkedForDelete)) Then
					Me.UploadSessionUploadTypeCollection.DeleteProgressCounter = Me.UploadSessionUploadTypeCollection.DeleteProgressCounter + 1
				End If
				
				' always delete the children if the parent is deleted
				theUploadSessionUploadType.Delete(onlyMarked And Not Me.IsMarkedForDelete)
			Next
		
			' remove all deleted children from the UploadSessionUploadTypeCollection
			childCollectionIndex = 0
			Do
				If childCollectionIndex >= Me.UploadSessionUploadTypeCollection.Count Then
					Exit Do
				End If
				theChild = Me.UploadSessionUploadTypeCollection.Item(childCollectionIndex)
				If CType(theChild, UploadSessionUploadType).IsDeleted Then
					Me.UploadSessionUploadTypeCollection.Remove(theChild)
                Else
					childCollectionIndex = childCollectionIndex + 1
				End If
			Loop
			

			If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
				If Not Me.IsNew Then
					UploadSessionDAO.Instance.DeleteUploadSession(CType(Me, UploadSession))
					Trace.WriteLine("Deleting a UploadSession.")
				Else
					Trace.WriteLine("Removing a new unsaved UploadSession.")
				End If
				
				Me.IsDeleted = True
				Me.IsMarkedForDelete = False

			End If
		
			Me.UploadRowCollection.DeleteProgressComplete = True
			Me.UploadSessionUploadTypeCollection.DeleteProgressComplete = True

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this UploadSession  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.UploadSessionID = UploadSession.NextTemporaryId
			Me.IsDirty = True
			Me.IsNew = True
		
            For Each theUploadRow As UploadRow _
                    In Me.UploadRowCollection
                theUploadRow.MakeNew()
			Next
		
            For Each theUploadSessionUploadType As UploadSessionUploadType _
                    In Me.UploadSessionUploadTypeCollection
                theUploadSessionUploadType.MakeNew()
			Next
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New UploadSession()
        End Function

#End Region

	End Class

End Namespace

