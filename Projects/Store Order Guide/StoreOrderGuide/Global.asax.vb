Imports System.Web.SessionState
Imports System.Web
Imports System.Data
Imports System.Data.SqlClient

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        Application.Add("SupportEmail", GetAppConfigSetting("SupportEmail"))
        Application.Add("reportingServicesURL", GetAppConfigSetting("reportingServicesURL"))
        Application.Add("region", GetAppConfigSetting("region"))
        Application.Add("environment", GetAppConfigSetting("environment"))
        Application.Add("version", GetAppConfigSetting("version"))
        Application.Add("defaultCommandTimeout", GetAppConfigSetting("defaultCommandTimeout"))
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Application.Set("SupportEmail", GetAppConfigSetting("SupportEmail"))
        Application.Set("reportingServicesURL", GetAppConfigSetting("reportingServicesURL"))
        Application.Set("region", GetAppConfigSetting("region"))
        Application.Set("environment", GetAppConfigSetting("environment"))
        Application.Set("version", GetAppConfigSetting("version"))
        Application.Set("defaultCommandTimeout", GetAppConfigSetting("defaultCommandTimeout"))
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache)
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

End Class