
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the UploadSessionUploadTypeStore db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadSessionUploadTypeStoreRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _uploadSessionUploadTypeStoreID As System.Int32
		Private _isUploadSessionUploadTypeStoreIDNull As Boolean = True
		Private _uploadSessionUploadTypeID As System.Int32
		Private _isUploadSessionUploadTypeIDNull As Boolean = True
		Private _storeNo As System.Int32
		Private _isStoreNoNull As Boolean = True

		Public Overridable Property UploadSessionUploadTypeStoreID() As System.Int32
		    Get
				Return _uploadSessionUploadTypeStoreID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadSessionUploadTypeStoreIDNull = False
				If _uploadSessionUploadTypeStoreID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadSessionUploadTypeStoreID", theOldValue, value)
				End If
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
				_uploadSessionUploadTypeStoreID = value
		    End Set
		End Property

		Public Overridable Property IsUploadSessionUploadTypeStoreIDNull() As Boolean
		    Get
				Return _isUploadSessionUploadTypeStoreIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadSessionUploadTypeStoreIDNull = value
		    End Set
		End Property

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

		Public Overridable Property StoreNo() As System.Int32
		    Get
				Return _storeNo
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsStoreNoNull = False
				If _storeNo <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "StoreNo", theOldValue, value)
				End If
				
			
				_storeNo = value
		    End Set
		End Property

		Public Overridable Property IsStoreNoNull() As Boolean
		    Get
				Return _isStoreNoNull
		    End Get
		    Set(ByVal value As Boolean)
				_isStoreNoNull = value
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
			Me.UploadSessionUploadTypeStoreID = UploadSessionUploadTypeStore.NextTemporaryId
		End Sub
		
		''' <summary>
        ''' Construct a UploadSessionUploadTypeStore by passing in
		''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
		Public Sub New(ByRef inUploadSessionUploadType As UploadSessionUploadType)
		
			Me.UploadSessionUploadTypeStoreID = UploadSessionUploadTypeStore.NextTemporaryId

			Me.UploadSessionUploadType = inUploadSessionUploadType
			
			If Not IsNothing(inUploadSessionUploadType) Then
				Me.UploadSessionUploadTypeID = inUploadSessionUploadType.UploadSessionUploadTypeID
				inUploadSessionUploadType.AddUploadSessionUploadTypeStore(CType(Me, UploadSessionUploadTypeStore))
			End If
			
        End Sub

#End Region

#Region "Just-in-time Instantiated Parents"

		Private _isUploadSessionUploadTypeLoaded As Boolean = False
		
		        Public Property IsUploadSessionUploadTypeLoaded() As Boolean
            Get
                Return _isUploadSessionUploadTypeLoaded
            End Get
            Set(ByVal value As Boolean)
                _isUploadSessionUploadTypeLoaded = value
            End Set
        End Property
 
		Private _uploadSessionUploadType As UploadSessionUploadType = Nothing

	    ''' <summary>
        ''' Set this UploadSessionUploadTypeStore UploadSessionUploadType parent
		''' and set the foreign key.
		''' This UploadSessionUploadTypeStore is not added to the parent's UploadSessionUploadTypeStoreCollection.
		''' You will have to do this yourself.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadSessionUploadType() As UploadSessionUploadType
		    Get
		    	If Not _isUploadSessionUploadTypeLoaded Then
				
					' set his once we try to load the parent so we won't keep trying if a parent
					' is not found and the reference remains Nothing
					_isUploadSessionUploadTypeLoaded = True
					
		    		_uploadSessionUploadType = _
		    			UploadSessionUploadTypeDAO.Instance.GetUploadSessionUploadTypeByPK(Me.UploadSessionUploadTypeID)
		    	End If
			Return _uploadSessionUploadType
		    End Get
		    Set(ByVal value As UploadSessionUploadType)
				_uploadSessionUploadType = value
				
				_isUploadSessionUploadTypeLoaded = True

				' set the FKs but DO NOT add this BusinessObject to the parent's corresponding child collection
				' the developer will have to do this
				If Not IsNothing(value) Then
					Me.UploadSessionUploadTypeID = _uploadSessionUploadType.UploadSessionUploadTypeID
				End If
		    End Set
		End Property
		
#End Region

#Region "Just-in-time Instantiated Children Collections"

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this UploadSessionUploadTypeStore
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					UploadSessionUploadTypeStoreDAO.Instance.InsertUploadSessionUploadTypeStore(CType(Me, UploadSessionUploadTypeStore))
					Trace.WriteLine("Inserting a new UploadSessionUploadTypeStore")
				Else
					UploadSessionUploadTypeStoreDAO.Instance.UpdateUploadSessionUploadTypeStore(CType(Me, UploadSessionUploadTypeStore))
					Trace.WriteLine("Updating an existing UploadSessionUploadTypeStore")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
		
		
		End Function

       ''' <summary>
        ''' Delete this UploadSessionUploadTypeStore
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
			Dim childCollectionIndex As Integer = 0
			
            If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
                If Not Me.IsNew Then
                    UploadSessionUploadTypeStoreDAO.Instance.DeleteUploadSessionUploadTypeStore(CType(Me, UploadSessionUploadTypeStore))
                    Trace.WriteLine("Deleting a UploadSessionUploadTypeStore.")
                Else
                    Trace.WriteLine("Removing a new unsaved UploadSessionUploadTypeStore.")
                End If

                Me.IsDeleted = True
                Me.IsMarkedForDelete = False

            End If
		

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this UploadSessionUploadTypeStore  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.UploadSessionUploadTypeStoreID = UploadSessionUploadTypeStore.NextTemporaryId
			Me.IsDirty = True
			Me.IsNew = True
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New UploadSessionUploadTypeStore()
        End Function

#End Region

	End Class

End Namespace

