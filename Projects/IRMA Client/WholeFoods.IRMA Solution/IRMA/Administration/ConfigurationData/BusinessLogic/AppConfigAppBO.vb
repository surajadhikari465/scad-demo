Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess

Public Class AppConfigAppBO

    Private _name As String
    Private _environmentID As Guid
    Private _typeID As Integer
    Private _applicationID As Guid
    Private _configuration As System.Xml.XmlDocument
    Private _userID As Integer

    ''' <summary>
    ''' The name of the application configuration application.
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
    ''' The GUID that uniquely identifies the application configuration enviroment.
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
    ''' The GUID that uniquely identifies the application configuration application.
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
    ''' The type of the application configuration application.
    ''' </summary>
    Public Property Type() As Integer
        Get
            Return Me._typeID
        End Get
        Set(ByVal value As Integer)
            Me._typeID = value
        End Set
    End Property

    ''' <summary>
    ''' The XML configuration document for the application enviroment.
    ''' </summary>
    Public Property Configuration() As Xml.XmlDocument
        Get
            Return Me._configuration
        End Get
        Set(ByVal value As Xml.XmlDocument)
            Me._configuration = value
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
    ''' Adds a new IRMA application to the AppConfigApp table. 
    ''' </summary>
    ''' <param name="AppConfigApp">The AppConfigAppBO object containing the application information.</param>
    ''' <returns>AppConfigAppBO</returns>
    ''' <remarks></remarks>
    Public Function Add(ByVal AppConfigApp As AppConfigAppBO) As AppConfigAppBO

        Dim configDAO As ConfigurationDataDAO = New ConfigurationDataDAO
        Return configDAO.AddAppConfigApp(AppConfigApp)

    End Function

    ''' <summary>
    ''' Updates an existing IRMA application as Deleted in the AppConfigApp table.
    ''' </summary>
    ''' <param name="AppConfigApp">The AppConfigAppBO object containing the application information.</param>
    ''' <returns>AppConfigAppBO</returns>
    ''' <remarks>Marking an application as Deleted with also mark all the Key/Pair value relationships associated
    ''' with this application and envinroment Deleted as well.</remarks>
    Public Function Remove(ByVal AppConfigApp As AppConfigAppBO) As Boolean

        Dim configDAO As ConfigurationDataDAO = New ConfigurationDataDAO
        Return configDAO.RemoveAppConfigApp(AppConfigApp)

    End Function

End Class
