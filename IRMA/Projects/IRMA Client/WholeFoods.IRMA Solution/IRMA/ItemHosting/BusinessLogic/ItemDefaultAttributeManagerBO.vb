Imports System.Text
Imports WholeFoods.IRMA.Common.DataAccess

Public Class ItemDefaultAttributeManagerBO

#Region "Members"


    Private _itemDefaultAttribute_ID As Integer
    Private _attributeName As String
    Private _attributeField As String
    Private _active As Boolean
    Private _controlOrder As Byte
    Private _controlTypeName As String
    Private _controlType As Byte

#End Region

#Region "Properties"

    Public Property ItemDefaultAttribute_ID As Integer
        Get
            Return Me._itemDefaultAttribute_ID
        End Get
        Set(ByVal value As Integer)
            Me._itemDefaultAttribute_ID = value
        End Set
    End Property

    Public Property AttributeName As String
        Get
            Return Me._attributeName
        End Get
        Set(ByVal value As String)
            Me._attributeName = value
        End Set
    End Property

    Public Property AttributeField As String
        Get
            Return Me._attributeField
        End Get
        Set(ByVal value As String)
            Me._attributeField = value
        End Set
    End Property

    Public Property Active As Boolean
        Get
            Return Me._active
        End Get
        Set(ByVal value As Boolean)
            Me._active = value
        End Set
    End Property

    Public Property ControlOrder As Byte
        Get
            Return Me._controlOrder
        End Get
        Set(ByVal value As Byte)
            Me._controlOrder = value
        End Set
    End Property

    Public Property ControlTypeName As String
        Get
            Return Me._controlTypeName
        End Get
        Set(ByVal value As String)
            Me._controlTypeName = value
            SetControlTypeName()
        End Set
    End Property

    Public Property ControlType As Byte
        Get
            Return Me._controlType
        End Get
        Set(ByVal value As Byte)
            Me._controlType = value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        Me._active = False
        Me._attributeField = String.Empty
        Me._attributeName = String.Empty
        Me._controlOrder = 0
        Me._controlType = 1
        Me._controlTypeName = String.Empty
        Me._itemDefaultAttribute_ID = -1
    End Sub

    Public Sub New(ByVal itemDefaultAttribute_ID As Integer _
                   , ByVal attributeName As String _
                   , ByVal attributeField As String _
                   , ByVal active As Boolean _
                   , ByVal controlOrder As Byte _
                   , ByVal controlType As Byte)

        Me._itemDefaultAttribute_ID = itemDefaultAttribute_ID
        Me._attributeName = attributeName
        Me._attributeField = attributeField
        Me._active = active
        Me._controlOrder = controlOrder
        Me._controlType = controlType

        SetControlTypeName()

    End Sub

#End Region

#Region "Methods"

    Private Sub SetControlTypeName()
        ' set control type name by control type
        Select Case Me._controlType
            Case 1
                Me._controlTypeName = "Text Field"
            Case 2
                Me._controlTypeName = "Dropdown List"
            Case 3
                Me._controlTypeName = "Checkbox"
        End Select
    End Sub

#End Region

End Class
