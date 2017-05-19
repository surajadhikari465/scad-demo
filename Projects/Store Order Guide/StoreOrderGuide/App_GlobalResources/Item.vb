Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<[ReadOnly](True)> _
Public Class Item

    Private _Item_Key As Integer
    Private _Identifier As String
    Private _Item_Description As String
    Private _Cost As Integer
    Private _Discontinue_Item As Boolean
    Private _Distribution_Unit As String
    Private _Not_Available As Boolean
    Private _Retail_Unit As String
    Private _SubTeam_No As Integer

    Public ReadOnly Property Item_Key() As Integer
        Get
            Return _Item_Key
        End Get
    End Property

    Public ReadOnly Property Identifier() As String
        Get
            Return _Identifier
        End Get
    End Property

    Public ReadOnly Property Item_Description() As String
        Get
            Return _Item_Description
        End Get
    End Property

    Public ReadOnly Property Cost() As Integer
        Get
            Return _Cost
        End Get
    End Property

    Public ReadOnly Property Discontinue_Item() As Boolean
        Get
            Return _Discontinue_Item
        End Get
    End Property

    Public ReadOnly Property Distribution_Unit() As String
        Get
            Return _Distribution_Unit
        End Get
    End Property

    Public ReadOnly Property Not_Available() As Boolean
        Get
            Return _Not_Available
        End Get
    End Property

    Public ReadOnly Property Retail_Unit() As String
        Get
            Return _Retail_Unit
        End Get
    End Property

    Public ReadOnly Property SubTeam_No() As Integer
        Get
            Return _SubTeam_No
        End Get
    End Property

    Public Sub Read()

    End Sub

End Class