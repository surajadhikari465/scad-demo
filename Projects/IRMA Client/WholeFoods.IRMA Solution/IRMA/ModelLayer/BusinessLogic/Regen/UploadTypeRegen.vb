
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the UploadType db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadTypeRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _uploadTypeCode As System.String
		Private _isUploadTypeCodeNull As Boolean = True
		Private _name As System.String
		Private _isNameNull As Boolean = True
		Private _description As System.String
		Private _isDescriptionNull As Boolean = True
		Private _isActive As System.Boolean
		Private _isIsActiveNull As Boolean = True

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
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
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

		Public Overridable Property Description() As System.String
		    Get
				Return _description
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsDescriptionNull = IsNothing(value)
				If (IsNothing(_description) And Not IsNothing(value)) Or _
						(Not IsNothing(_description) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_description) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _description.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "Description", theOldValue, value)
				End If
				
			
				_description = value
		    End Set
		End Property

		Public Overridable Property IsDescriptionNull() As Boolean
		    Get
				Return _isDescriptionNull
		    End Get
		    Set(ByVal value As Boolean)
				_isDescriptionNull = value
		    End Set
		End Property

		Public Overridable Property IsActive() As System.Boolean
		    Get
				Return _isActive
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsActiveNull = False
				If _isActive <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsActive", theOldValue, value)
				End If
				
			
				_isActive = value
		    End Set
		End Property

		Public Overridable Property IsIsActiveNull() As Boolean
		    Get
				Return _isIsActiveNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsActiveNull = value
		    End Set
		End Property

		
#End Region

#Region "Non-persistent Fields and Properties"

		
#End Region

#Region "Constructors"

		Public Sub New()
		End Sub
		

#End Region

#Region "Just-in-time Instantiated Parents"

#End Region

#Region "Just-in-time Instantiated Children Collections"

		Private _uploadSessionUploadTypeCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadType's collection of UploadSessionUploadTypes.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadSessionUploadTypeCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadSessionUploadTypeCollection) Then
					If Not Me.IsNew Then
						_uploadSessionUploadTypeCollection = _
							UploadSessionUploadTypeDAO.Instance.GetUploadSessionUploadTypesByUploadTypeCode(Me.UploadTypeCode)
		    		Else
						_uploadSessionUploadTypeCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadSessionUploadType As UploadSessionUploadType In _
							_uploadSessionUploadTypeCollection
						theUploadSessionUploadType.UploadType = CType(Me, UploadType)
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
        ''' Gets the first UploadSessionUploadType in this UploadType's
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
        ''' Add the provided UploadSessionUploadType to this UploadType's UploadSessionUploadTypeCollection
        ''' </summary>
        ''' <param name="inUploadSessionUploadType"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadSessionUploadType(ByRef inUploadSessionUploadType As UploadSessionUploadType)

            inUploadSessionUploadType.UploadType = Ctype(Me, UploadType)
            inUploadSessionUploadType.UploadTypeCode = Me.UploadTypeCode

            Me.UploadSessionUploadTypeCollection.Add(inUploadSessionUploadType.PrimaryKey, inUploadSessionUploadType)
			
        End Sub

		Private _uploadTypeAttributeCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadType's collection of UploadTypeAttributes.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadTypeAttributeCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadTypeAttributeCollection) Then
					If Not Me.IsNew Then
						_uploadTypeAttributeCollection = _
							UploadTypeAttributeDAO.Instance.GetUploadTypeAttributesByUploadTypeCode(Me.UploadTypeCode)
		    		Else
						_uploadTypeAttributeCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadTypeAttribute As UploadTypeAttribute In _
							_uploadTypeAttributeCollection
						theUploadTypeAttribute.UploadType = CType(Me, UploadType)
					Next
									
				' subscribe to the PropertyChanged event that bubbles up from the BusinessObjects through the BusinessObjectCollection
				' this will set this BusinessObject's IsDirty flag when that of a child object changes
				AddHandler _uploadTypeAttributeCollection.PropertyChanged, AddressOf Me.PropertyChangedHandler
				
				End If
			Return _uploadTypeAttributeCollection
		    End Get
		    Set(ByVal value As BusinessObjectCollection)
				_uploadTypeAttributeCollection = value
		    End Set
		End Property
		
		''' <summary>
        ''' Gets the first UploadTypeAttribute in this UploadType's
        ''' UploadTypeAttributeCollection, if there is one.
        ''' This is a convenience for when a one-to-many is really a one-to-one.
        ''' </summary>
        ''' <value></value>
        ''' <returns>uploadTypeAttribute</returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property UploadTypeAttribute() As UploadTypeAttribute
            Get
                Dim theUploadTypeAttribute As UploadTypeAttribute = Nothing

                If Me.UploadTypeAttributeCollection.Count > 0 Then
                    theUploadTypeAttribute = CType(Me.UploadTypeAttributeCollection.Item(0), UploadTypeAttribute)
                End If
                Return theUploadTypeAttribute
            End Get
        End Property
		
		''' <summary>
        ''' Add the provided UploadTypeAttribute to this UploadType's UploadTypeAttributeCollection
        ''' </summary>
        ''' <param name="inUploadTypeAttribute"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadTypeAttribute(ByRef inUploadTypeAttribute As UploadTypeAttribute)

            inUploadTypeAttribute.UploadType = Ctype(Me, UploadType)
            inUploadTypeAttribute.UploadTypeCode = Me.UploadTypeCode

            Me.UploadTypeAttributeCollection.Add(inUploadTypeAttribute.PrimaryKey, inUploadTypeAttribute)
			
        End Sub

		Private _uploadTypeTemplateCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadType's collection of UploadTypeTemplates.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadTypeTemplateCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadTypeTemplateCollection) Then
					If Not Me.IsNew Then
						_uploadTypeTemplateCollection = _
							UploadTypeTemplateDAO.Instance.GetUploadTypeTemplatesByUploadTypeCode(Me.UploadTypeCode)
		    		Else
						_uploadTypeTemplateCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadTypeTemplate As UploadTypeTemplate In _
							_uploadTypeTemplateCollection
						theUploadTypeTemplate.UploadType = CType(Me, UploadType)
					Next
									
				' subscribe to the PropertyChanged event that bubbles up from the BusinessObjects through the BusinessObjectCollection
				' this will set this BusinessObject's IsDirty flag when that of a child object changes
				AddHandler _uploadTypeTemplateCollection.PropertyChanged, AddressOf Me.PropertyChangedHandler
				
				End If
			Return _uploadTypeTemplateCollection
		    End Get
		    Set(ByVal value As BusinessObjectCollection)
				_uploadTypeTemplateCollection = value
		    End Set
		End Property
		
		''' <summary>
        ''' Gets the first UploadTypeTemplate in this UploadType's
        ''' UploadTypeTemplateCollection, if there is one.
        ''' This is a convenience for when a one-to-many is really a one-to-one.
        ''' </summary>
        ''' <value></value>
        ''' <returns>uploadTypeTemplate</returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property UploadTypeTemplate() As UploadTypeTemplate
            Get
                Dim theUploadTypeTemplate As UploadTypeTemplate = Nothing

                If Me.UploadTypeTemplateCollection.Count > 0 Then
                    theUploadTypeTemplate = CType(Me.UploadTypeTemplateCollection.Item(0), UploadTypeTemplate)
                End If
                Return theUploadTypeTemplate
            End Get
        End Property
		
		''' <summary>
        ''' Add the provided UploadTypeTemplate to this UploadType's UploadTypeTemplateCollection
        ''' </summary>
        ''' <param name="inUploadTypeTemplate"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadTypeTemplate(ByRef inUploadTypeTemplate As UploadTypeTemplate)

            inUploadTypeTemplate.UploadType = Ctype(Me, UploadType)
            inUploadTypeTemplate.UploadTypeCode = Me.UploadTypeCode

            Me.UploadTypeTemplateCollection.Add(inUploadTypeTemplate.PrimaryKey, inUploadTypeTemplate)
			
        End Sub

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this UploadType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					UploadTypeDAO.Instance.InsertUploadType(CType(Me, UploadType))
					Trace.WriteLine("Inserting a new UploadType")
				Else
					UploadTypeDAO.Instance.UpdateUploadType(CType(Me, UploadType))
					Trace.WriteLine("Updating an existing UploadType")
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
                theUploadSessionUploadType.UploadTypeCode = Me.UploadTypeCode
            	theUploadSessionUploadType.Save()
				Me.UploadSessionUploadTypeCollection.SaveProgressCounter = Me.UploadSessionUploadTypeCollection.SaveProgressCounter + 1
			Next		
			' save the UploadTypeAttributes
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadTypeAttributeCollection.SaveProgressCounter = 0
            For Each theUploadTypeAttribute As UploadTypeAttribute _
                    In Me.UploadTypeAttributeCollection
                theUploadTypeAttribute.UploadTypeCode = Me.UploadTypeCode
            	theUploadTypeAttribute.Save()
				Me.UploadTypeAttributeCollection.SaveProgressCounter = Me.UploadTypeAttributeCollection.SaveProgressCounter + 1
			Next		
			' save the UploadTypeTemplates
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadTypeTemplateCollection.SaveProgressCounter = 0
            For Each theUploadTypeTemplate As UploadTypeTemplate _
                    In Me.UploadTypeTemplateCollection
                theUploadTypeTemplate.UploadTypeCode = Me.UploadTypeCode
            	theUploadTypeTemplate.Save()
				Me.UploadTypeTemplateCollection.SaveProgressCounter = Me.UploadTypeTemplateCollection.SaveProgressCounter + 1
			Next		
		
			Me.UploadSessionUploadTypeCollection.SaveProgressComplete = True
			Me.UploadTypeAttributeCollection.SaveProgressComplete = True
			Me.UploadTypeTemplateCollection.SaveProgressComplete = True
		
		End Function

       ''' <summary>
        ''' Delete this UploadType
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
			
			' delete the UploadTypeAttributes
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadTypeAttributeCollection.DeleteProgressCounter = 0
			For Each theUploadTypeAttribute As UploadTypeAttribute _
					In Me.UploadTypeAttributeCollection
				
				' increment the counter only if the UploadTypeAttribute is deleted
				If Not onlyMarked Or (onlyMarked And (Me.IsMarkedForDelete Or theUploadTypeAttribute.IsMarkedForDelete)) Then
					Me.UploadTypeAttributeCollection.DeleteProgressCounter = Me.UploadTypeAttributeCollection.DeleteProgressCounter + 1
				End If
				
				' always delete the children if the parent is deleted
				theUploadTypeAttribute.Delete(onlyMarked And Not Me.IsMarkedForDelete)
			Next
		
			' remove all deleted children from the UploadTypeAttributeCollection
			childCollectionIndex = 0
			Do
				If childCollectionIndex >= Me.UploadTypeAttributeCollection.Count Then
					Exit Do
				End If
				theChild = Me.UploadTypeAttributeCollection.Item(childCollectionIndex)
				If CType(theChild, UploadTypeAttribute).IsDeleted Then
					Me.UploadTypeAttributeCollection.Remove(theChild)
                Else
					childCollectionIndex = childCollectionIndex + 1
				End If
			Loop
			
			' delete the UploadTypeTemplates
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadTypeTemplateCollection.DeleteProgressCounter = 0
			For Each theUploadTypeTemplate As UploadTypeTemplate _
					In Me.UploadTypeTemplateCollection
				
				' increment the counter only if the UploadTypeTemplate is deleted
				If Not onlyMarked Or (onlyMarked And (Me.IsMarkedForDelete Or theUploadTypeTemplate.IsMarkedForDelete)) Then
					Me.UploadTypeTemplateCollection.DeleteProgressCounter = Me.UploadTypeTemplateCollection.DeleteProgressCounter + 1
				End If
				
				' always delete the children if the parent is deleted
				theUploadTypeTemplate.Delete(onlyMarked And Not Me.IsMarkedForDelete)
			Next
		
			' remove all deleted children from the UploadTypeTemplateCollection
			childCollectionIndex = 0
			Do
				If childCollectionIndex >= Me.UploadTypeTemplateCollection.Count Then
					Exit Do
				End If
				theChild = Me.UploadTypeTemplateCollection.Item(childCollectionIndex)
				If CType(theChild, UploadTypeTemplate).IsDeleted Then
					Me.UploadTypeTemplateCollection.Remove(theChild)
                Else
					childCollectionIndex = childCollectionIndex + 1
				End If
			Loop
			

			If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
				If Not Me.IsNew Then
					UploadTypeDAO.Instance.DeleteUploadType(CType(Me, UploadType))
					Trace.WriteLine("Deleting a UploadType.")
				Else
					Trace.WriteLine("Removing a new unsaved UploadType.")
				End If
				
				Me.IsDeleted = True
				Me.IsMarkedForDelete = False

			End If
		
			Me.UploadSessionUploadTypeCollection.DeleteProgressComplete = True
			Me.UploadTypeAttributeCollection.DeleteProgressComplete = True
			Me.UploadTypeTemplateCollection.DeleteProgressComplete = True

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this UploadType  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.IsDirty = True
			Me.IsNew = True
		
            For Each theUploadSessionUploadType As UploadSessionUploadType _
                    In Me.UploadSessionUploadTypeCollection
                theUploadSessionUploadType.MakeNew()
			Next
		
            For Each theUploadTypeAttribute As UploadTypeAttribute _
                    In Me.UploadTypeAttributeCollection
                theUploadTypeAttribute.MakeNew()
			Next
		
            For Each theUploadTypeTemplate As UploadTypeTemplate _
                    In Me.UploadTypeTemplateCollection
                theUploadTypeTemplate.MakeNew()
			Next
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New UploadType()
        End Function

#End Region

	End Class

End Namespace

