Option Explicit On
Option Strict On

Imports System.ComponentModel

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic.Common
    Public Class BO_IRMABase
        Implements System.ComponentModel.INotifyPropertyChanged

#Region " Enums"
        Public Enum EntityStateEnum
            UNCHANGED
            ADDED
            DELETED
            MODIFIED
        End Enum
        Public Enum OfferChangeType
            [New] = 1
            Add = 2
            Delete = 3
        End Enum
#End Region

#Region " Properties"
        Private _EntityState As EntityStateEnum
        ''' <summary>
        ''' State of the object data
        ''' </summary>
        ''' <value></value>
        ''' <returns>Entity state</returns>
        Public Property EntityState() As EntityStateEnum
            Get
                Return _EntityState
            End Get
            Set(ByVal value As EntityStateEnum)
                _EntityState = value
            End Set
        End Property

        ''' <summary>
        ''' Indicates if the object has been changed in any way
        ''' </summary>
        ''' <value></value>
        ''' <returns>Entity state</returns>
        Public ReadOnly Property IsDirty() As Boolean
            Get
                Return Me.EntityState <> _
                                EntityStateEnum.UNCHANGED
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the object represents a new, unsaved record
        ''' </summary>
        ''' <value></value>
        ''' <returns>Entity state</returns>
        Public ReadOnly Property IsNew() As Boolean
            Get
                Return Me.EntityState = EntityStateEnum.ADDED
            End Get
        End Property
        Private _Loading As Boolean = False
        ''' <summary>
        ''' Indicates if the object is being populated by "hydration from data source" 
        ''' If true, this prevents modifications to properties from trigger a change 
        ''' in Entity State.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Boolean</returns>
        Public Property Loading() As Boolean
            Get
                Return _Loading
            End Get
            Set(ByVal value As Boolean)
                _Loading = value
            End Set
        End Property

        Public ReadOnly Property isDeleted() As Boolean
            Get
                Return Me.EntityState = EntityStateEnum.DELETED
            End Get
        End Property

#End Region

#Region " Events"
        Public Event PropertyChanged(ByVal sender As Object, _
            ByVal e As System.ComponentModel.PropertyChangedEventArgs) _
            Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region

#Region " Public Methods"
        Public Sub MarkClean()
            Me._EntityState = EntityStateEnum.UNCHANGED
        End Sub

        Public Sub MarkDeleted()
            Me._EntityState = EntityStateEnum.DELETED
        End Sub

        Public Sub MarkNew()
            Me._EntityState = EntityStateEnum.ADDED
        End Sub

        Public Sub MarkDirty()
            Me._EntityState = EntityStateEnum.MODIFIED
        End Sub
#End Region

#Region " Private Methods"

#Region " DataStateChanged"
        ''' <summary>
        ''' Defines that a property is changed and that the system needs to be notified
        ''' </summary>
        ''' <param name="dataState"></param>
        ''' <param name="propertyName"></param>
        ''' <remarks>This code generates the PropertyChanged event</remarks>
        Protected Sub DataStateChanged(ByVal dataState As EntityStateEnum, ByVal propertyName As String)

            ' If the object is being loaded, do not perform Data State Change
            If _Loading Then Exit Sub

            ' Raise the event
            If Not String.IsNullOrEmpty(propertyName) Then
                RaiseEvent PropertyChanged(Me, _
                   New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If

            ' If the state is deleted, mark it as deleted
            If dataState = EntityStateEnum.DELETED Then
                Me.EntityState = dataState
            End If

            ' Only set other data states if the existing state is unchanged
            If Me.EntityState = EntityStateEnum.UNCHANGED Then
                Me.EntityState = dataState
            End If
        End Sub
#End Region

#End Region


    End Class
End Namespace

