Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Administration.UserInterface

    Public Class ComboSelection

#Region "Property Definitions"
        Private _value As Object
        Private _description As String
#End Region

#Region "Methods to manage the object"

        ''' <summary>
        ''' Create a new instance of the object.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal inValue As Object, ByVal inDescription As String)
            _value = inValue
            _description = inDescription
        End Sub
#End Region

#Region "Property access methods"
        Public Property Value() As Object
            Get
                Return _value
            End Get
            Set(ByVal value As Object)
                _value = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

#End Region

    End Class
End Namespace

