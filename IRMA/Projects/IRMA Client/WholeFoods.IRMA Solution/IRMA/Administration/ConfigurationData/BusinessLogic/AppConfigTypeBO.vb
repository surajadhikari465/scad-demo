
Public Class AppConfigTypeBO

    Private _typeID As Integer
    Private _name As String

    ''' <summary>
    ''' The value that uniquely identifies the application configuration type.
    ''' </summary>
    Public Property TypeID() As Integer
        Get
            Return Me._typeID
        End Get
        Set(ByVal value As Integer)
            Me._typeID = value
        End Set
    End Property

    ''' <summary>
    ''' The name of the application configuration type.
    ''' </summary>
    ''' <remarks>Examples include Scheduled Job, Windows Application, Web Application, etc.</remarks>
    Public Property Name() As String
        Get
            Return Me._name
        End Get
        Set(ByVal value As String)
            Me._name = value
        End Set
    End Property

    Public Sub Add()

    End Sub

    Public Sub Remove()

    End Sub

    Public Sub Update()

    End Sub

End Class
