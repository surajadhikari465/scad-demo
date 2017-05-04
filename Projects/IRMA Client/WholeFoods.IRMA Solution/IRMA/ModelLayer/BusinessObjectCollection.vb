
Imports System.Collections
Imports System.Reflection
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic

Namespace WholeFoods.IRMA.ModelLayer

    ''' <summary>
    ''' This collection class subclasses SortableHashlist, which
    ''' has the characteristics of an ArrayList and a Hashtable, and can be sorted
    ''' on any property of the class of the contained objects.
    ''' This class adds a progress counter (see notes for the progress properties below)
    ''' which is incremented whenever a child collection
    ''' is saved or deleted through a parent. This is done in the parent CRUD method
    ''' and not in this collection.
    ''' This progress counter can be used to update a progress bar if the save or delete is
    ''' executed on its own thread and the form has an progress update timer that polls
    ''' the collections progress counter.
    ''' </summary>
    ''' <remarks>
    ''' Can be sequentially accessed and used in a For Each loop exactly like an ArrayList. 
    ''' Can be randomly accessed by key like a Hashtable 
    ''' Can be sorted by any property of the contained objects 
    ''' </remarks>
    Public Class BusinessObjectCollection
        Inherits SortableHashlist

#Region "Public Events"

        Public Delegate Sub PrimaryKeyChangedEventHandler(ByRef inBusinessObject As BusinessObjectBase, ByVal oldValue As Object, ByVal newValue As Object)
        Public Event PrimaryKeyChanged As PrimaryKeyChangedEventHandler

        Public Delegate Sub PropertyChangedEventHandler(ByRef inBusinessObject As BusinessObjectBase, _
                ByVal inChangedPropertyName As String, ByVal oldValue As Object, ByVal newValue As Object)
        Public Event PropertyChanged As PropertyChangedEventHandler

#End Region

#Region "Fields and Properties"

        Private _isDirty As Boolean = False

        Private _saveProgressCounter As Integer
        Private _saveProgressComplete As Boolean

        Private _deleteProgressCounter As Integer
        Private _deleteProgressComplete As Boolean

        ''' <summary>
        ''' This is a progress counter which is incremented whenever a child collection
        ''' is saved or deleted through a parent. This is done automatically
        ''' in the generated parent CRUD method and not in this collection.
        ''' 
        ''' This progress counter can be used to update a progress bar if the save or delete is
        ''' executed on its own thread and the form has an progress update timer that polls
        ''' the collections progress counter.
        ''' The polling thread will need to do the following:
        ''' 
        ''' 1. Set the BusinessObjectCollection's ProgressComplete property to False.
        ''' 2. Start the polling timer which will poll the BusinessObjectCollection's ProgressCounter
        '''    property and update the progress bar on the form.
        ''' 3. Do the polling in a loop which exits when the BusinessObjectCollection's ProgressComplete property
        '''    is set to true by the parent in which the collection is being saved.
        ''' 
        ''' This is done by polling instead of by events fired from the collection to minimize the amount of overhead
        ''' added to the saving and deleting of collections. It also avoids having a non-UI thread
        ''' accessing form controls or methods running on a UI thread.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SaveProgressCounter() As Integer
            Get
                SyncLock Me
                    Return _saveProgressCounter
                End SyncLock
            End Get
            Set(ByVal value As Integer)
                SyncLock Me
                    _saveProgressCounter = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' This is a progress complete flag which is works with the progress counter 
        ''' above.
        ''' 
        ''' Toether they are used to update a progress bar if the save or delete is
        ''' executed on its own thread and the form has an progress update timer that polls
        ''' the collections progress counter.
        ''' The polling thread will need to do the following:
        ''' 
        ''' 1. Set the BusinessObjectCollection's ProgressComplete property to False.
        ''' 2. Start the polling timer which will poll the BusinessObjectCollection's ProgressCounter
        '''    property and update the progress bar on the form.
        ''' 3. Do the polling in a loop which exits when the BusinessObjectCollection's ProgressComplete property
        '''    is set to true by the parent in which the collection is being saved.
        ''' 
        ''' This is done by polling instead of by events fired from the collection to minimize the amount of overhead
        ''' added to the saving and deleting of collections. It also avoids having a non-UI thread
        ''' accessing form controls or methods running on a UI thread.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SaveProgressComplete() As Boolean
            Get
                SyncLock Me
                    Return _saveProgressComplete
                End SyncLock
            End Get
            Set(ByVal value As Boolean)
                SyncLock Me
                    _saveProgressComplete = value
                End SyncLock
            End Set
        End Property
		
        ''' <summary>
        ''' This is a progress counter which is incremented whenever a child collection
        ''' is saved or deleted through a parent. This is done automatically
        ''' in the generated parent CRUD method and not in this collection.
        ''' 
        ''' This progress counter can be used to update a progress bar if the save or delete is
        ''' executed on its own thread and the form has an progress update timer that polls
        ''' the collections progress counter.
        ''' The polling thread will need to do the following:
        ''' 
        ''' 1. Set the BusinessObjectCollection's ProgressComplete property to False.
        ''' 2. Start the polling timer which will poll the BusinessObjectCollection's ProgressCounter
        '''    property and update the progress bar on the form.
        ''' 3. Do the polling in a loop which exits when the BusinessObjectCollection's ProgressComplete property
        '''    is set to true by the parent in which the collection is being saved.
        ''' 
        ''' This is done by polling instead of by events fired from the collection to minimize the amount of overhead
        ''' added to the saving and deleting of collections. It also avoids having a non-UI thread
        ''' accessing form controls or methods running on a UI thread.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeleteProgressCounter() As Integer
            Get
                SyncLock Me
                    Return _deleteProgressCounter
                End SyncLock
            End Get
            Set(ByVal value As Integer)
                SyncLock Me
                    _deleteProgressCounter = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' This is a progress complete flag which is works with the progress counter 
        ''' above.
        ''' 
        ''' Toether they are used to update a progress bar if the save or delete is
        ''' executed on its own thread and the form has an progress update timer that polls
        ''' the collections progress counter.
        ''' The polling thread will need to do the following:
        ''' 
        ''' 1. Set the BusinessObjectCollection's ProgressComplete property to False.
        ''' 2. Start the polling timer which will poll the BusinessObjectCollection's ProgressCounter
        '''    property and update the progress bar on the form.
        ''' 3. Do the polling in a loop which exits when the BusinessObjectCollection's ProgressComplete property
        '''    is set to true by the parent in which the collection is being saved.
        ''' 
        ''' This is done by polling instead of by events fired from the collection to minimize the amount of overhead
        ''' added to the saving and deleting of collections. It also avoids having a non-UI thread
        ''' accessing form controls or methods running on a UI thread.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeleteProgressComplete() As Boolean
            Get
                SyncLock Me
                    Return _deleteProgressComplete
                End SyncLock
            End Get
            Set(ByVal value As Boolean)
                SyncLock Me
                    _deleteProgressComplete = value
                End SyncLock
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

#End Region

#Region "Overriden Methods"

        Public Overrides Sub Add(ByVal key As Object, ByVal value As Object)
            MyBase.Add(key, value)

            ' can only subscribe to event if the object being added
            ' is a BusinessObjectBase
            If TypeOf value Is BusinessObjectBase Then
                AddHandler CType(value, BusinessObjectBase).PrimaryKeyChanged, AddressOf Me.PrimaryKeyChangedHandler
                AddHandler CType(value, BusinessObjectBase).PropertyChanged, AddressOf Me.PropertyChangedHandler
            End If

        End Sub

#End Region

#Region "Event Handlers"

        ''' <summary>
        ''' Handle the changing of the primary key of a contained BusinessObject
        ''' by additionally indexing the BusinessObject with the new PK value.
        ''' This will allow the BusinessObject to be found from this collection
        ''' by both the new and the old key values.
        ''' </summary>
        ''' <param name="oldValue"></param>
        ''' <param name="NewValue"></param>
        ''' <remarks></remarks>
        Private Sub PrimaryKeyChangedHandler(ByRef inBusinessObject As BusinessObjectBase, ByVal oldValue As Object, ByVal newValue As Object)

            ' add the BusinessObject to the internal Hashtable under the new key
            ' leaving it under the old key
            Me.AddWithNewKey(NewValue, inBusinessObject)

            RaiseEvent PrimaryKeyChanged(inBusinessObject, oldValue, newValue)

        End Sub

        ''' <summary>
        ''' Handle the changing of the IsDirty flag of a contained BusinessObject
        ''' by setting the IsDirty flag of this collection to the same value and
        ''' firing the same event.
        ''' </summary>
        ''' <param name="oldValue"></param>
        ''' <param name="NewValue"></param>
        ''' <remarks></remarks>
        Private Sub PropertyChangedHandler(ByRef inBusinessObject As BusinessObjectBase, _
                ByVal inChangedPropertyName As String, ByVal oldValue As Object, ByVal newValue As Object)

            Me.IsDirty = True

            RaiseEvent PropertyChanged(inBusinessObject, inChangedPropertyName, oldValue, newValue)

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Return a BusinessObjectCollection of BusinessObjects
        ''' from this collection whose
        ''' property identified by the provided property name
        ''' has a value equal to the given property value.
        ''' </summary>
        ''' <param name="inPropertyName"></param>
        ''' <param name="inPropertyValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindByPropertyValue(ByVal inPropertyName As String, ByVal inPropertyValue As Object) As BusinessObjectCollection

            Dim theBusinessObjectCollection As New BusinessObjectCollection()
            If Me.Count = 0 Or String.IsNullOrEmpty(inPropertyName) Or inPropertyValue Is Nothing Then
                Return theBusinessObjectCollection
            End If

            Dim theFirstItem As Object = Me.Item(0)
            Dim theProperty As PropertyInfo = theFirstItem.GetType().GetProperty(inPropertyName)
            Dim thePropertyValue As Object

            If Not IsNothing(theProperty) Then
                For Each theBusinessObject As BusinessObjectBase In Me
                    thePropertyValue = theProperty.GetValue(theBusinessObject, Nothing)
                    If Object.Equals(thePropertyValue, inPropertyValue) Then
                        theBusinessObjectCollection.Add(theBusinessObject.PrimaryKey, theBusinessObject)
                    End If
                Next
            End If

            Return theBusinessObjectCollection
        End Function

        ''' <summary>
        ''' Returns the first BusinessObject in a BusinessObjectCollection of BusinessObjects
        ''' from this collection whose
        ''' property identified by the provided property name
        ''' has a value equal to the given property value.
        ''' This is convenient when you know there will only be one
        ''' BusinessObject in your search results.
        ''' </summary>
        ''' <param name="inPropertyName"></param>
        ''' <param name="inPropertyValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindOneByPropertyValue(ByVal inPropertyName As String, ByVal inPropertyValue As Object) As BusinessObjectBase

            Dim theFoundBusinessObjectBase As BusinessObjectBase = Nothing

            Dim theBusinessObjectCollection As BusinessObjectCollection = _
                FindByPropertyValue(inPropertyName, inPropertyValue)

            If theBusinessObjectCollection.Count > 0 Then
                theFoundBusinessObjectBase = CType(theBusinessObjectCollection.Item(0), BusinessObjectBase)
            End If

            Return theFoundBusinessObjectBase
        End Function

        ''' <summary>
        ''' Create a new copy of this collection with references to its contained
        ''' BusinessObjects.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NewCopy() As BusinessObjectCollection
            Dim theNewBusinessObjectCollection As New BusinessObjectCollection()

            For Each theBusinessObject As BusinessObjectBase In Me
                theNewBusinessObjectCollection.Add(theBusinessObject.PrimaryKey, theBusinessObject)
            Next

            Return theNewBusinessObjectCollection

        End Function

#End Region

    End Class

End Namespace