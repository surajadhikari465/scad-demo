Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess

Public Class AppConfigValueBO

    Private _environmentID As Guid
    Private _applicationID As Guid
    Private _keyID As Integer
    Private _value As String
    Private _userID As Integer

    ''' <summary>
    ''' The environment GUID for this key/value pair.
    ''' </summary>
    Public Property EnvironmentID() As Guid
        Get
            Return Me._environmentID
        End Get
        Set(ByVal value As Guid)
            Me._environmentID = value
        End Set
    End Property

    ''' <summary>
    ''' The application GUID for this key/value pair.
    ''' </summary>
    Public Property ApplicationID() As Guid
        Get
            Return Me._applicationID
        End Get
        Set(ByVal value As Guid)
            Me._applicationID = value
        End Set
    End Property

    ''' <summary>
    ''' The application configuration key for this key/value pair.
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
    ''' The application configuration key value for this key/value pair.
    ''' </summary>
    Public Property Value() As String
        Get
            Return Me._value
        End Get
        Set(ByVal value As String)
            Me._value = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return Me._userID
        End Get
        Set(ByVal value As Integer)
            Me._userID = value
        End Set
    End Property

    Public Sub Add(ByVal AppConfigValue As AppConfigValueBO, ByVal UpdateExistingKeyValue As Boolean)

        Dim configDAO As ConfigurationDataDAO = New ConfigurationDataDAO
        configDAO.AddAppConfigValue(AppConfigValue, UpdateExistingKeyValue)

    End Sub

    Public Function Update(ByVal AppConfigValue As AppConfigValueBO) As Boolean

        Dim configDAO As ConfigurationDataDAO = New ConfigurationDataDAO
        If configDAO.UpdateKeyValue(AppConfigValue) Then
            Return True
        End If

    End Function

    Public Function Remove(ByVal AppConfigValue As AppConfigValueBO) As Boolean

        Dim configDAO As ConfigurationDataDAO = New ConfigurationDataDAO
        Return configDAO.RemoveAppConfigValue(AppConfigValue)

    End Function

End Class
