<%@ Application Language="VB" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<script runat="server">
    
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' code that runs when the application is started
        
        ' if you add any new app config keys, you must declare them here
        Application.Add("reportingServicesURL", GetAppConfigSetting("reportingServicesURL"))
        Application.Add("environment", GetAppConfigSetting("environment"))
        Application.Add("market", GetAppConfigSetting("market"))
        Application.Add("region", GetAppConfigSetting("region"))
        
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        
        ' if you add any new app config keys, you must set them here
        ' this will ensure that the settings refresh with each new session started in case a change was made and rebuilt
        ' using the configuration manager in the IRMA Client
        Application.Set("reportingServicesURL", GetAppConfigSetting("reportingServicesURL"))
        Application.Set("environment", GetAppConfigSetting("environment"))
        Application.Set("market", GetAppConfigSetting("market"))
        Application.Set("region", GetAppConfigSetting("region"))
        
    End Sub
    
    Private Shared ReadOnly Property DatabaseConnectionString() As String
        Get
            Return ConfigurationManager.ConnectionStrings(ConfigurationManager.AppSettings("ConnectionStringName").ToString).ToString
        End Get
    End Property

    Private Shared ReadOnly Property ApplicationID() As Guid
        Get
            Return New Guid(ConfigurationManager.AppSettings("ApplicationGUID").ToString)
        End Get
    End Property

    Private Shared ReadOnly Property EnvironmentID() As Guid
        Get
            Return New Guid(ConfigurationManager.AppSettings("EnvironmentGUID").ToString)
        End Get
    End Property
    
    Private Shared Function GetAppConfigSetting(ByVal KeyName As String) As String

        Dim dbConn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing

        Try

            dbConn = New SqlConnection()
            dbConn.ConnectionString = DatabaseConnectionString

            cmd = New SqlCommand()
            cmd.Connection = dbConn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "AppConfig_GetAppConfigSetting"

            Dim _envIDParam As New SqlParameter("@EnvironmentID", EnvironmentID.ToString("D").ToUpper)
            Dim _appIDParam As New SqlParameter("@ApplicationID", ApplicationID.ToString("D").ToUpper)
            Dim _keynameParam As New SqlParameter("@KeyName", KeyName)

            cmd.Parameters.Add(_envIDParam)
            cmd.Parameters.Add(_appIDParam)
            cmd.Parameters.Add(_keynameParam)

            dbConn.Open()
            
            Dim retVal As String = CStr(cmd.ExecuteScalar)
          
            Return retVal

        Catch ex As Exception

            Throw ex

        Finally

            If Not dbConn Is Nothing Then
                If dbConn.State <> ConnectionState.Closed Then
                    dbConn.Close()
                End If
            End If

        End Try

    End Function
      
</script>