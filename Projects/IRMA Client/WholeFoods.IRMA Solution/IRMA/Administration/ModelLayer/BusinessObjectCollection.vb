


Imports System.Collections
Imports System.Reflection

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

#Region "Fields and Properties"


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
#End Region

#Region "Public Methods"

#End Region

    End Class

End Namespace

