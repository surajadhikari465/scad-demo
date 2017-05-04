Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess

Public Class AppConfigKeyBO

    Private _keyID As Integer
    Private _name As String
    Private _userID As Integer

    ''' <summary>
    ''' The value the uniquely identifies the application configuration key.
    ''' </summary>
    Public Property KeyID() As Integer
        Get
            Return Me._keyID
        End Get
        Set(ByVal value As Integer)
            Me._keyID = value
        End Set
    End Property

    ''' <summary>
    ''' The name of the application configuration key.
    ''' </summary>
    Public Property Name() As String
        Get
            Return Me._name
        End Get
        Set(ByVal value As String)
            Me._name = value
        End Set
    End Property

    Public Sub New(ByVal Name As String, ByVal KeyID As Integer, ByVal UserID As Integer)
        Me._keyID = KeyID
        Me._name = Name
        Me._userID = UserID
    End Sub

    Public Property UserID() As Integer
        Get
            Return Me._userID
        End Get
        Set(ByVal value As Integer)
            Me._userID = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Function Add(ByVal AppConfigKey As AppConfigKeyBO) As AppConfigKeyBO

        Dim configDAO As ConfigurationDataDAO = New ConfigurationDataDAO
        Return configDAO.AddAppConfigKey(AppConfigKey)

    End Function

End Class
