
Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess

Public Class AppConfigEnvBO

    Private _environmentID As Guid
    Private _name As String
    Private _shortname As String
    Private _userID As Integer

    ''' <summary>
    ''' The GUID value that uniquely identifies the application configuration enviroment.
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
    ''' The long name of the application configuration enviroment.
    ''' </summary>
    Public Property Name() As String
        Get
            Return Me._name
        End Get
        Set(ByVal value As String)
            Me._name = value
        End Set
    End Property

    ''' <summary>
    ''' The shortname of the application configuration environment.
    ''' </summary>
    ''' <remarks>Examples include PRD, DEV, QA, etc.</remarks>
    Public Property Shortname() As String
        Get
            Return Me._shortname
        End Get
        Set(ByVal value As String)
            Me._shortname = value
        End Set
    End Property

    ''' <summary>
    ''' The IRMA user id of the current logged in user.
    ''' </summary>
    Public Property UserID() As Integer
        Get
            Return Me._userID
        End Get
        Set(ByVal value As Integer)
            Me._userID = value
        End Set
    End Property

    ''' <summary>
    ''' Adds a new IRMA environment to the AppConfigEnv table. 
    ''' </summary>
    ''' <param name="AppConfigEnv">The AppConfigEnvBO object containing the environment information.</param>
    ''' <returns>AppConfigEnvBO</returns>
    ''' <remarks></remarks>
    Public Function Add(ByVal AppConfigEnv As AppConfigEnvBO) As AppConfigEnvBO

        Dim configDAO As ConfigurationDataDAO = New ConfigurationDataDAO
        Return configDAO.AddAppConfigEnv(AppConfigEnv)

    End Function

    ''' <summary>
    ''' Updates an existing IRMA environment as Deleted in the AppConfigEnv table.
    ''' </summary>
    ''' <param name="AppConfigEnv">The AppConfigEnvBO object containing the enviromment information.</param>
    ''' <returns>AppConfigEnvBO</returns>
    ''' <remarks>Marking an enviroment as Deleted with also mark all applications and all Key/Pair value relationships associated
    ''' with the applications and envinroment Deleted as well.</remarks>
    Public Function Remove(ByVal AppConfigEnv As AppConfigEnvBO) As Boolean

        Dim configDAO As ConfigurationDataDAO = New ConfigurationDataDAO
        Return configDAO.RemoveAppConfigEnv(AppConfigEnv)

    End Function

End Class
