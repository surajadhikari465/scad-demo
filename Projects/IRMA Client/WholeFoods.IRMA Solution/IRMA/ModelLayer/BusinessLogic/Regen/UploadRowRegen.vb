
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the UploadRow db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadRowRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _uploadRowID As System.Int32
		Private _isUploadRowIDNull As Boolean = True
		Private _itemKey As System.Int32
		Private _isItemKeyNull As Boolean = True
		Private _uploadSessionID As System.Int32
		Private _isUploadSessionIDNull As Boolean = True
		Private _identifier As System.String
		Private _isIdentifierNull As Boolean = True
		Private _validationLevel As System.Int32
		Private _isValidationLevelNull As Boolean = True
		Private _itemRequestID As System.Int32
        Private _isItemRequestIDNull As Boolean = True
        Private _isVendorVINDuplicate As Boolean = False
        Private _isUploadExclusion As Boolean = False
        Private _isUploadExclusionWarningChecked As Boolean = False

        Public Overridable Property isUploadExclusion() As Boolean
            Get
                Return _isUploadExclusion
            End Get
            Set(ByVal value As Boolean)
                _isUploadExclusion = value
            End Set
        End Property

        Public Overridable Property isUploadExclusionWarningChecked() As Boolean
            Get
                Return _isUploadExclusionWarningChecked
            End Get
            Set(ByVal value As Boolean)
                _isUploadExclusionWarningChecked = value
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
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
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

		Public Overridable Property ItemKey() As System.Int32
		    Get
				Return _itemKey
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsItemKeyNull = False
				If _itemKey <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ItemKey", theOldValue, value)
				End If
				
			
				_itemKey = value
		    End Set
		End Property

		Public Overridable Property IsItemKeyNull() As Boolean
		    Get
				Return _isItemKeyNull
		    End Get
		    Set(ByVal value As Boolean)
				_isItemKeyNull = value
		    End Set
        End Property
        Public Overridable Property IsVendorVINDuplicate() As Boolean
            Get
                Return _isVendorVINDuplicate
            End Get
            Set(ByVal value As Boolean)
                _isVendorVINDuplicate = value
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

		Public Overridable Property Identifier() As System.String
		    Get
				Return _identifier
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIdentifierNull = IsNothing(value)
				If (IsNothing(_identifier) And Not IsNothing(value)) Or _
						(Not IsNothing(_identifier) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_identifier) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _identifier.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "Identifier", theOldValue, value)
				End If
				
			
				_identifier = value
		    End Set
		End Property

		Public Overridable Property IsIdentifierNull() As Boolean
		    Get
				Return _isIdentifierNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIdentifierNull = value
		    End Set
		End Property

		Public Overridable Property ValidationLevel() As System.Int32
		    Get
				Return _validationLevel
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsValidationLevelNull = False
				If _validationLevel <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ValidationLevel", theOldValue, value)
				End If
				
			
				_validationLevel = value
		    End Set
		End Property

		Public Overridable Property IsValidationLevelNull() As Boolean
		    Get
				Return _isValidationLevelNull
		    End Get
		    Set(ByVal value As Boolean)
				_isValidationLevelNull = value
		    End Set
		End Property

		Public Overridable Property ItemRequestID() As System.Int32
		    Get
				Return _itemRequestID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsItemRequestIDNull = False
				If _itemRequestID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ItemRequestID", theOldValue, value)
				End If
				
			
				_itemRequestID = value
		    End Set
		End Property

		Public Overridable Property IsItemRequestIDNull() As Boolean
		    Get
				Return _isItemRequestIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isItemRequestIDNull = value
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
			Me.UploadRowID = UploadRow.NextTemporaryId
		End Sub
		
		''' <summary>
        ''' Construct a UploadRow by passing in
		''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
		Public Sub New(ByRef inUploadSession As UploadSession)
		
			Me.UploadRowID = UploadRow.NextTemporaryId

			Me.UploadSession = inUploadSession
			
			If Not IsNothing(inUploadSession) Then
				Me.UploadSessionID = inUploadSession.UploadSessionID
				inUploadSession.AddUploadRow(CType(Me, UploadRow))
			End If
			
        End Sub

#End Region

#Region "Just-in-time Instantiated Parents"

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
        ''' Set this UploadRow UploadSession parent
		''' and set the foreign key.
		''' This UploadRow is not added to the parent's UploadRowCollection.
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

		Private _uploadValueCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadRow's collection of UploadValues.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadValueCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadValueCollection) Then
					If Not Me.IsNew Then
						_uploadValueCollection = _
							UploadValueDAO.Instance.GetUploadValuesByUploadRowID(Me.UploadRowID)
		    		Else
						_uploadValueCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadValue As UploadValue In _
							_uploadValueCollection
						theUploadValue.UploadRow = CType(Me, UploadRow)
					Next
									
				' subscribe to the PropertyChanged event that bubbles up from the BusinessObjects through the BusinessObjectCollection
				' this will set this BusinessObject's IsDirty flag when that of a child object changes
				AddHandler _uploadValueCollection.PropertyChanged, AddressOf Me.PropertyChangedHandler
				
				End If
			Return _uploadValueCollection
		    End Get
		    Set(ByVal value As BusinessObjectCollection)
				_uploadValueCollection = value
		    End Set
		End Property
		
		''' <summary>
        ''' Gets the first UploadValue in this UploadRow's
        ''' UploadValueCollection, if there is one.
        ''' This is a convenience for when a one-to-many is really a one-to-one.
        ''' </summary>
        ''' <value></value>
        ''' <returns>uploadValue</returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property UploadValue() As UploadValue
            Get
                Dim theUploadValue As UploadValue = Nothing

                If Me.UploadValueCollection.Count > 0 Then
                    theUploadValue = CType(Me.UploadValueCollection.Item(0), UploadValue)
                End If
                Return theUploadValue
            End Get
        End Property
		
		''' <summary>
        ''' Add the provided UploadValue to this UploadRow's UploadValueCollection
        ''' </summary>
        ''' <param name="inUploadValue"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadValue(ByRef inUploadValue As UploadValue)

            inUploadValue.UploadRow = Ctype(Me, UploadRow)
            inUploadValue.UploadRowID = Me.UploadRowID

            Me.UploadValueCollection.Add(inUploadValue.PrimaryKey, inUploadValue)
			
        End Sub

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this UploadRow
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					UploadRowDAO.Instance.InsertUploadRow(CType(Me, UploadRow))
					Trace.WriteLine("Inserting a new UploadRow")
				Else
					UploadRowDAO.Instance.UpdateUploadRow(CType(Me, UploadRow))
					Trace.WriteLine("Updating an existing UploadRow")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
			' save the UploadValues
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadValueCollection.SaveProgressCounter = 0
            For Each theUploadValue As UploadValue _
                    In Me.UploadValueCollection
                theUploadValue.UploadRowID = Me.UploadRowID
            	theUploadValue.Save()
				Me.UploadValueCollection.SaveProgressCounter = Me.UploadValueCollection.SaveProgressCounter + 1
			Next		
		
			Me.UploadValueCollection.SaveProgressComplete = True
		
		End Function

       ''' <summary>
        ''' Delete this UploadRow
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
			Dim childCollectionIndex As Integer = 0
			
			Dim theChild As Object
			' delete the UploadValues
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadValueCollection.DeleteProgressCounter = 0
			For Each theUploadValue As UploadValue _
					In Me.UploadValueCollection
				
				' increment the counter only if the UploadValue is deleted
				If Not onlyMarked Or (onlyMarked And (Me.IsMarkedForDelete Or theUploadValue.IsMarkedForDelete)) Then
					Me.UploadValueCollection.DeleteProgressCounter = Me.UploadValueCollection.DeleteProgressCounter + 1
				End If
				
				' always delete the children if the parent is deleted
				theUploadValue.Delete(onlyMarked And Not Me.IsMarkedForDelete)
			Next
		
			' remove all deleted children from the UploadValueCollection
			childCollectionIndex = 0
			Do
				If childCollectionIndex >= Me.UploadValueCollection.Count Then
					Exit Do
				End If
				theChild = Me.UploadValueCollection.Item(childCollectionIndex)
				If CType(theChild, UploadValue).IsDeleted Then
					Me.UploadValueCollection.Remove(theChild)
                Else
					childCollectionIndex = childCollectionIndex + 1
				End If
			Loop
			

			If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
				If Not Me.IsNew Then
					UploadRowDAO.Instance.DeleteUploadRow(CType(Me, UploadRow))
					Trace.WriteLine("Deleting a UploadRow.")
				Else
					Trace.WriteLine("Removing a new unsaved UploadRow.")
				End If
				
				Me.IsDeleted = True
				Me.IsMarkedForDelete = False

			End If
		
			Me.UploadValueCollection.DeleteProgressComplete = True

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this UploadRow  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.UploadRowID = UploadRow.NextTemporaryId
			Me.IsDirty = True
			Me.IsNew = True
		
            For Each theUploadValue As UploadValue _
                    In Me.UploadValueCollection
                theUploadValue.MakeNew()
			Next
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New UploadRow()
        End Function

#End Region

	End Class

End Namespace

