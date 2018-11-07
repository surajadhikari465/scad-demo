<%@ Application Language="VB" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<script runat="server">

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        
        ' if you add any new app config keys, you must declare and set them here       
        
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