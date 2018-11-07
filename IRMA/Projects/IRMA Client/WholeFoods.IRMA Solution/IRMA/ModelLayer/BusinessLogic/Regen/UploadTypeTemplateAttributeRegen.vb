
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the UploadTypeTemplateAttribute db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadTypeTemplateAttributeRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _uploadTypeTemplateAttributeID As System.Int32
		Private _isUploadTypeTemplateAttributeIDNull As Boolean = True
		Private _uploadTypeTemplateID As System.Int32
		Private _isUploadTypeTemplateIDNull As Boolean = True
		Private _uploadTypeAttributeID As System.Int32
		Private _isUploadTypeAttributeIDNull As Boolean = True

		Public Overridable Property UploadTypeTemplateAttributeID() As System.Int32
		    Get
				Return _uploadTypeTemplateAttributeID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadTypeTemplateAttributeIDNull = False
				If _uploadTypeTemplateAttributeID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadTypeTemplateAttributeID", theOldValue, value)
				End If
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
				_uploadTypeTemplateAttributeID = value
		    End Set
		End Property

		Public Overridable Property IsUploadTypeTemplateAttributeIDNull() As Boolean
		    Get
				Return _isUploadTypeTemplateAttributeIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadTypeTemplateAttributeIDNull = value
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
			Me.UploadTypeTemplateAttributeID = UploadTypeTemplateAttribute.NextTemporaryId
		End Sub
		
		''' <summary>
        ''' Construct a UploadTypeTemplateAttribute by passing in
		''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
		Public Sub New(ByRef inUploadTypeAttribute As UploadTypeAttribute, ByRef inUploadTypeTemplate As UploadTypeTemplate)
		
			Me.UploadTypeTemplateAttributeID = UploadTypeTemplateAttribute.NextTemporaryId

			Me.UploadTypeAttribute = inUploadTypeAttribute
			
			If Not IsNothing(inUploadTypeAttribute) Then
				Me.UploadTypeAttributeID = inUploadTypeAttribute.UploadTypeAttributeID
				inUploadTypeAttribute.AddUploadTypeTemplateAttribute(CType(Me, UploadTypeTemplateAttribute))
			End If
			
			Me.UploadTypeTemplate = inUploadTypeTemplate
			
			If Not IsNothing(inUploadTypeTemplate) Then
				Me.UploadTypeTemplateID = inUploadTypeTemplate.UploadTypeTemplateID
				inUploadTypeTemplate.AddUploadTypeTemplateAttribute(CType(Me, UploadTypeTemplateAttribute))
			End If
			
        End Sub

#End Region

#Region "Just-in-time Instantiated Parents"

		Private _isUploadTypeAttributeLoaded As Boolean = False
		
		        Public Property IsUploadTypeAttributeLoaded() As Boolean
            Get
                Return _isUploadTypeAttributeLoaded
            End Get
            Set(ByVal value As Boolean)
                _isUploadTypeAttributeLoaded = value
            End Set
        End Property
 
		Private _uploadTypeAttribute As UploadTypeAttribute = Nothing

	    ''' <summary>
        ''' Set this UploadTypeTemplateAttribute UploadTypeAttribute parent
		''' and set the foreign key.
		''' This UploadTypeTemplateAttribute is not added to the parent's UploadTypeTemplateAttributeCollection.
		''' You will have to do this yourself.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadTypeAttribute() As UploadTypeAttribute
		    Get
		    	If Not _isUploadTypeAttributeLoaded Then
				
					' set his once we try to load the parent so we won't keep trying if a parent
					' is not found and the reference remains Nothing
					_isUploadTypeAttributeLoaded = True
					
		    		_uploadTypeAttribute = _
		    			UploadTypeAttributeDAO.Instance.GetUploadTypeAttributeByPK(Me.UploadTypeAttributeID)
		    	End If
			Return _uploadTypeAttribute
		    End Get
		    Set(ByVal value As UploadTypeAttribute)
				_uploadTypeAttribute = value
				
				_isUploadTypeAttributeLoaded = True

				' set the FKs but DO NOT add this BusinessObject to the parent's corresponding child collection
				' the developer will have to do this
				If Not IsNothing(value) Then
					Me.UploadTypeAttributeID = _uploadTypeAttribute.UploadTypeAttributeID
				End If
		    End Set
		End Property
		
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
        ''' Set this UploadTypeTemplateAttribute UploadTypeTemplate parent
		''' and set the foreign key.
		''' This UploadTypeTemplateAttribute is not added to the parent's UploadTypeTemplateAttributeCollection.
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
		
#End Region

#Region "Just-in-time Instantiated Children Collections"

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this UploadTypeTemplateAttribute
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					UploadTypeTemplateAttributeDAO.Instance.InsertUploadTypeTemplateAttribute(CType(Me, UploadTypeTemplateAttribute))
					Trace.WriteLine("Inserting a new UploadTypeTemplateAttribute")
				Else
					UploadTypeTemplateAttributeDAO.Instance.UpdateUploadTypeTemplateAttribute(CType(Me, UploadTypeTemplateAttribute))
					Trace.WriteLine("Updating an existing UploadTypeTemplateAttribute")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
		
		
		End Function

       ''' <summary>
        ''' Delete this UploadTypeTemplateAttribute
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
			Dim childCollectionIndex As Integer = 0
			
            If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
                If Not Me.IsNew Then
                    UploadTypeTemplateAttributeDAO.Instance.DeleteUploadTypeTemplateAttribute(CType(Me, UploadTypeTemplateAttribute))
                    Trace.WriteLine("Deleting a UploadTypeTemplateAttribute.")
                Else
                    Trace.WriteLine("Removing a new unsaved UploadTypeTemplateAttribute.")
                End If

                Me.IsDeleted = True
                Me.IsMarkedForDelete = False

            End If
		

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this UploadTypeTemplateAttribute  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.UploadTypeTemplateAttributeID = UploadTypeTemplateAttribute.NextTemporaryId
			Me.IsDirty = True
			Me.IsNew = True
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New UploadTypeTemplateAttribute()
        End Function

#End Region

	End Class

End Namespace

