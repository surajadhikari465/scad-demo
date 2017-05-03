
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the UploadTypeTemplate db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadTypeTemplateRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _uploadTypeTemplateID As System.Int32
		Private _isUploadTypeTemplateIDNull As Boolean = True
		Private _uploadTypeCode As System.String
		Private _isUploadTypeCodeNull As Boolean = True
		Private _name As System.String
		Private _isNameNull As Boolean = True
		Private _createdByUserID As System.Int32
		Private _isCreatedByUserIDNull As Boolean = True
		Private _createdDateTime As System.DateTime
		Private _isCreatedDateTimeNull As Boolean = True
		Private _modifiedByUserID As System.Int32
		Private _isModifiedByUserIDNull As Boolean = True
		Private _modifiedDateTime As System.DateTime
		Private _isModifiedDateTimeNull As Boolean = True

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
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
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
			Me.UploadTypeTemplateID = UploadTypeTemplate.NextTemporaryId
		End Sub
		
		''' <summary>
        ''' Construct a UploadTypeTemplate by passing in
		''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
		Public Sub New(ByRef inUploadType As UploadType)
		
			Me.UploadTypeTemplateID = UploadTypeTemplate.NextTemporaryId

			Me.UploadType = inUploadType
			
			If Not IsNothing(inUploadType) Then
				Me.UploadTypeCode = inUploadType.UploadTypeCode
				inUploadType.AddUploadTypeTemplate(CType(Me, UploadTypeTemplate))
			End If
			
        End Sub

#End Region

#Region "Just-in-time Instantiated Parents"

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
        ''' Set this UploadTypeTemplate UploadType parent
		''' and set the foreign key.
		''' This UploadTypeTemplate is not added to the parent's UploadTypeTemplateCollection.
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

		Private _uploadSessionUploadTypeCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadTypeTemplate's collection of UploadSessionUploadTypes.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadSessionUploadTypeCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadSessionUploadTypeCollection) Then
					If Not Me.IsNew Then
						_uploadSessionUploadTypeCollection = _
							UploadSessionUploadTypeDAO.Instance.GetUploadSessionUploadTypesByUploadTypeTemplateID(Me.UploadTypeTemplateID)
		    		Else
						_uploadSessionUploadTypeCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadSessionUploadType As UploadSessionUploadType In _
							_uploadSessionUploadTypeCollection
						theUploadSessionUploadType.UploadTypeTemplate = CType(Me, UploadTypeTemplate)
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
        ''' Gets the first UploadSessionUploadType in this UploadTypeTemplate's
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
        ''' Add the provided UploadSessionUploadType to this UploadTypeTemplate's UploadSessionUploadTypeCollection
        ''' </summary>
        ''' <param name="inUploadSessionUploadType"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadSessionUploadType(ByRef inUploadSessionUploadType As UploadSessionUploadType)

            inUploadSessionUploadType.UploadTypeTemplate = Ctype(Me, UploadTypeTemplate)
            inUploadSessionUploadType.UploadTypeTemplateID = Me.UploadTypeTemplateID

            Me.UploadSessionUploadTypeCollection.Add(inUploadSessionUploadType.PrimaryKey, inUploadSessionUploadType)
			
        End Sub

		Private _uploadTypeTemplateAttributeCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadTypeTemplate's collection of UploadTypeTemplateAttributes.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadTypeTemplateAttributeCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadTypeTemplateAttributeCollection) Then
					If Not Me.IsNew Then
						_uploadTypeTemplateAttributeCollection = _
							UploadTypeTemplateAttributeDAO.Instance.GetUploadTypeTemplateAttributesByUploadTypeTemplateID(Me.UploadTypeTemplateID)
		    		Else
						_uploadTypeTemplateAttributeCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadTypeTemplateAttribute As UploadTypeTemplateAttribute In _
							_uploadTypeTemplateAttributeCollection
						theUploadTypeTemplateAttribute.UploadTypeTemplate = CType(Me, UploadTypeTemplate)
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
        ''' Gets the first UploadTypeTemplateAttribute in this UploadTypeTemplate's
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
        ''' Add the provided UploadTypeTemplateAttribute to this UploadTypeTemplate's UploadTypeTemplateAttributeCollection
        ''' </summary>
        ''' <param name="inUploadTypeTemplateAttribute"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadTypeTemplateAttribute(ByRef inUploadTypeTemplateAttribute As UploadTypeTemplateAttribute)

            inUploadTypeTemplateAttribute.UploadTypeTemplate = Ctype(Me, UploadTypeTemplate)
            inUploadTypeTemplateAttribute.UploadTypeTemplateID = Me.UploadTypeTemplateID

            Me.UploadTypeTemplateAttributeCollection.Add(inUploadTypeTemplateAttribute.PrimaryKey, inUploadTypeTemplateAttribute)
			
        End Sub

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this UploadTypeTemplate
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					UploadTypeTemplateDAO.Instance.InsertUploadTypeTemplate(CType(Me, UploadTypeTemplate))
					Trace.WriteLine("Inserting a new UploadTypeTemplate")
				Else
					UploadTypeTemplateDAO.Instance.UpdateUploadTypeTemplate(CType(Me, UploadTypeTemplate))
					Trace.WriteLine("Updating an existing UploadTypeTemplate")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
			' save the UploadSessionUploadTypes
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadSessionUploadTypeCollection.SaveProgressCounter = 0
            For Each theUploadSessionUploadType As UploadSessionUploadType _
                    In Me.UploadSessionUploadTypeCollection
                theUploadSessionUploadType.UploadTypeTemplateID = Me.UploadTypeTemplateID
            	theUploadSessionUploadType.Save()
				Me.UploadSessionUploadTypeCollection.SaveProgressCounter = Me.UploadSessionUploadTypeCollection.SaveProgressCounter + 1
			Next		
			' save the UploadTypeTemplateAttributes
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadTypeTemplateAttributeCollection.SaveProgressCounter = 0
            For Each theUploadTypeTemplateAttribute As UploadTypeTemplateAttribute _
                    In Me.UploadTypeTemplateAttributeCollection
                theUploadTypeTemplateAttribute.UploadTypeTemplateID = Me.UploadTypeTemplateID
            	theUploadTypeTemplateAttribute.Save()
				Me.UploadTypeTemplateAttributeCollection.SaveProgressCounter = Me.UploadTypeTemplateAttributeCollection.SaveProgressCounter + 1
			Next		
		
			Me.UploadSessionUploadTypeCollection.SaveProgressComplete = True
			Me.UploadTypeTemplateAttributeCollection.SaveProgressComplete = True
		
		End Function

       ''' <summary>
        ''' Delete this UploadTypeTemplate
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
			Dim childCollectionIndex As Integer = 0
			
			Dim theChild As Object
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
					UploadTypeTemplateDAO.Instance.DeleteUploadTypeTemplate(CType(Me, UploadTypeTemplate))
					Trace.WriteLine("Deleting a UploadTypeTemplate.")
				Else
					Trace.WriteLine("Removing a new unsaved UploadTypeTemplate.")
				End If
				
				Me.IsDeleted = True
				Me.IsMarkedForDelete = False

			End If
		
			Me.UploadSessionUploadTypeCollection.DeleteProgressComplete = True
			Me.UploadTypeTemplateAttributeCollection.DeleteProgressComplete = True

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this UploadTypeTemplate  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.UploadTypeTemplateID = UploadTypeTemplate.NextTemporaryId
			Me.IsDirty = True
			Me.IsNew = True
		
            For Each theUploadSessionUploadType As UploadSessionUploadType _
                    In Me.UploadSessionUploadTypeCollection
                theUploadSessionUploadType.MakeNew()
			Next
		
            For Each theUploadTypeTemplateAttribute As UploadTypeTemplateAttribute _
                    In Me.UploadTypeTemplateAttributeCollection
                theUploadTypeTemplateAttribute.MakeNew()
			Next
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New UploadTypeTemplate()
        End Function

#End Region

	End Class

End Namespace

