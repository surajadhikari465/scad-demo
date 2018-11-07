Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Administration.POSPush.BusinessLogic
    ''' <summary>
    ''' The POSConfigTypeBO is used to manage the available values for the StorePOSConfig.ConfigType column.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class POSConfigTypeBO

#Region "Property Definitions"
        Private _value As String
        Private _description As String
#End Region

#Region "Constructor"
        ''' <summary>
        ''' Create a new instance of the object.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal value As String, ByVal description As String)
            _value = value
            _description = description
        End Sub
#End Region

#Region "Property access methods"
        Public Property Value() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
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
