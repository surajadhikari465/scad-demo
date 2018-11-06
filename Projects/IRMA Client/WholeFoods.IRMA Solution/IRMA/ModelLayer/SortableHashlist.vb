
Imports System.Collections
Imports System.Reflection

Namespace WholeFoods.IRMA.ModelLayer

    ''' <summary>
    ''' This collection class has the charactoristics of an ArrayList and a Hashtable, and can be sorted
    ''' on any property of the class of the contained objects.
    ''' </summary>
    ''' <remarks>
    ''' Can be sequentially accessed and used in a For Each loop exactly like an ArrayList. 
    ''' Can be randomly accessed by key like a Hashtable 
    ''' Can be sorted by any property of the contained objects 
    ''' </remarks>
    Public Class SortableHashlist
        Inherits CollectionBase

#Region "Inner Private Comparer Class"

        Private Class HashlistComparer
            Implements IComparer

            Public Enum SortItems
                Value
                PropertyValue
            End Enum

            Public Enum SortOrders
                ASC
                DESC
            End Enum
            Private SortBy As SortItems = SortItems.Value
            Private OrderBy As SortOrders = SortOrders.ASC

            Private _sortByPropertyName As String = Nothing

            Public Property SortByPropertyName() As String
                Get
                    Return _sortByPropertyName
                End Get
                Set(ByVal value As String)
                    _sortByPropertyName = value
                End Set
            End Property

            Public Property Sort() As SortItems
                Get
                    Return Me.SortBy
                End Get
                Set(ByVal Value As SortItems)
                    Me.SortBy = Value
                End Set
            End Property

            Public Property Order() As SortOrders
                Get
                    Return Me.OrderBy
                End Get
                Set(ByVal Value As SortOrders)
                    Me.OrderBy = Value
                End Set
            End Property

            Public Sub New(Optional ByVal SortItem As SortItems = SortItems.Value, Optional ByVal SortOrder As SortOrders = SortOrders.ASC)
                Me.SortBy = SortItem
            End Sub

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim iRetVal As Integer
                Dim ValueOfX As IComparable = Nothing, ValueOfY As IComparable = Nothing

                Select Case Me.SortBy
                    Case SortItems.Value
                        ValueOfX = DirectCast(x, IComparable)
                        ValueOfY = DirectCast(y, IComparable)

                    Case SortItems.PropertyValue
                        ValueOfX = DirectCast(GetPropertyValue(x, Me.SortByPropertyName), IComparable)
                        ValueOfY = DirectCast(GetPropertyValue(y, Me.SortByPropertyName), IComparable)

                End Select

                If Not IsNothing(ValueOfX) Then
                    iRetVal = ValueOfX.CompareTo(ValueOfY)
                End If

                If Me.OrderBy = SortOrders.DESC Then
                    iRetVal = -iRetVal
                End If

                Return iRetVal
            End Function

            Public Function GetPropertyValue(ByRef inObject As Object, ByVal propertyName As String) As Object

                Dim thePropertyValue As Object = Nothing
                Dim thePropertyInfo As PropertyInfo = inObject.GetType().GetProperty(propertyName)

                If IsNothing(thePropertyInfo) Then
                    Throw New Exception("There is not Property named " + propertyName + " in class " + inObject.GetType().FullName)
                Else
                    thePropertyValue = thePropertyInfo.GetValue(inObject, Nothing)
                End If

                Return thePropertyValue

            End Function

        End Class

#End Region

#Region "Fields and Properties"

        Private Comparer As New HashlistComparer

        Private InnerHashtable As New Hashtable()

        Public ReadOnly Property Item(ByVal Index As Integer) As Object
            Get
                Return Me.InnerList.Item(Index)
            End Get
        End Property

        Public ReadOnly Property ItemByKey(ByVal Key As Object) As Object
            Get
                Return Me.InnerHashtable.Item(Key)
            End Get
        End Property

        Public ReadOnly Property Keys() As ICollection
            Get
                Return Me.InnerHashtable.Keys
            End Get
        End Property

#End Region

#Region "Public Methods"

        Public Overridable Sub AddWithNewKey(ByVal key As Object, ByVal value As Object)
            'If Not Me.InnerList.Contains(value) Then
            '    Throw New Exception("BusinessObjectCollection.Add: The item is not already in the BusinessObjectCollection.")
            '    Exit Sub
            'End If
            If Me.ContainsKey(key) Then
                Throw New Exception("BusinessObjectCollection.Add: An item with the same key, " + key.ToString() + ", is already in the BusinessObjectCollection.")
                Exit Sub
            End If
          Me.InnerHashtable.Add(key, value)
        End Sub

        Public Overridable Sub Add(ByVal key As Object, ByVal value As Object)
            If Me.ContainsKey(key) Then
                Throw New Exception("BusinessObjectCollection.Add: An item with the same key, " + key.ToString() + ", is already in the BusinessObjectCollection.")
                Exit Sub
            End If
            Me.InnerList.Add(value)
            Me.InnerHashtable.Add(key, value)
        End Sub

        Public Overridable Sub SortByValue(ByVal DESC As Boolean)
            Me.Comparer.Sort = HashlistComparer.SortItems.Value
            If DESC = True Then
                Me.Comparer.Order = HashlistComparer.SortOrders.DESC
            Else
                Me.Comparer.Order = HashlistComparer.SortOrders.ASC
            End If

            Me.InnerList.Sort(Me.Comparer)
        End Sub

        Public Overridable Sub SortByPropertyValue(ByVal propertyName As String)
            SortByPropertyValue(propertyName, False)
        End Sub

        Public Overridable Sub SortByPropertyValue(ByVal propertyName As String, ByVal DESC As Boolean)
            Me.Comparer.Sort = HashlistComparer.SortItems.PropertyValue
            Me.Comparer.SortByPropertyName = propertyName
            If DESC = True Then
                Me.Comparer.Order = HashlistComparer.SortOrders.DESC
            Else
                Me.Comparer.Order = HashlistComparer.SortOrders.ASC
            End If

            Me.InnerList.Sort(Me.Comparer)
        End Sub

        Public Function ContainsKey(ByVal Key As Object) As Boolean
            Return Me.InnerHashtable.ContainsKey(Key)
        End Function

        Public Overloads Sub Clear()

            Me.InnerHashtable.Clear()
            Me.InnerList.Clear()

        End Sub

        Public Overridable Sub Remove(ByVal item As Object)

            If Not IsNothing(item) Then
                Me.InnerHashtable.Remove(item)
                Me.InnerList.Remove(item)
            End If

        End Sub

        Public Overridable Sub RemoveByKey(ByVal Key As Object)
            Dim item As Object = Me.InnerHashtable.Item(Key)
			
			If Not IsNothing(item) Then
				Me.InnerHashtable.Remove(Key)				
				Me.InnerList.Remove(item)				
			End If
			
        End Sub

#End Region

    End Class

#Region "KeyedListItem Class"

    ''' <summary>
    ''' Can be optionally used to hold the objects in the BusinessObjectCollection to give
	''' ready access to the objects' keys.
	''' This is not used by the model layer generator.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public Class KeyedListItem

        Private _key As Object
        Private _value As Object

        Public Property Key() As Object
            Get
                Return _key
            End Get
            Set(ByVal value As Object)
                _key = value
            End Set
        End Property

        Public Property Value() As Object
            Get
                Return _value
            End Get
            Set(ByVal value As Object)
                _value = value
            End Set
        End Property

        Public Sub New(ByVal inKey As Object, ByVal inValue As Object)

            Me.Key = inKey
            Me.Value = inValue

        End Sub

    End Class

#End Region

End Namespace
