<%@ Application Language="VB" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<script runat="server">
    
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        ' if you add any new app config keys, you ADD them here  
        Application.Add("LowMargin", GetAppConfigSetting("LowMargin"))
        Application.Add("HighMargin", GetAppConfigSetting("HighMargin"))
        Application.Add("CRV", GetAppConfigSetting("CRV"))
        Application.Add("userAccess", GetAppConfigSetting("userAccess"))
        Application.Add("maxTimeSpanMovement", GetAppConfigSetting("maxTimeSpanMovement"))
        Application.Add("ItemRequestEmail", GetAppConfigSetting("ItemRequestEmail"))
        Application.Add("ItemRequestRejectEmail", GetAppConfigSetting("ItemRequestRejectEmail"))
        Application.Add("VendorRequestEmail", GetAppConfigSetting("VendorRequestEmail"))
        Application.Add("InStoreSpecialEmail", GetAppConfigSetting("InStoreSpecialEmail"))
        Application.Add("InStoreSpecialRejectEmail", GetAppConfigSetting("InStoreSpecialRejectEmail"))
        Application.Add("InStoreSpecialEndSaleEarlyEmail", GetAppConfigSetting("InStoreSpecialEndSaleEarlyEmail"))
        Application.Add("ItemAuthorizationEmail", GetAppConfigSetting("ItemAuthorizationEmail"))
        Application.Add("RetailCostChangeEmail", GetAppConfigSetting("RetailCostChangeEmail"))
        Application.Add("IRMAPushEmail", GetAppConfigSetting("IRMAPushEmail"))
        Application.Add("PLURequestEmail", GetAppConfigSetting("PLURequestEmail"))
        Application.Add("Admin", GetAppConfigSetting("Admin"))
        Application.Add("RegionalTeam", GetAppConfigSetting("RegionalTeam"))
        Application.Add("RegionalCorporateOffice", GetAppConfigSetting("RegionalCorporateOffice"))
        Application.Add("encryptedConnectionStrings", GetAppConfigSetting("encryptedConnectionStrings"))
        Application.Add("CRV-BottleDepositLabel", GetAppConfigSetting("CRV-BottleDepositLabel"))
        Application.Add("CRV-BottleDepositSubteams", GetAppConfigSetting("CRV-BottleDepositSubteams"))
        Application.Add("IIS_PriceChgTypeID", GetAppConfigSetting("IIS_PriceChgTypeID"))
        Application.Add("WebQuery_Show_POSPrice", GetAppConfigSetting("WebQuery_Show_POSPrice"))
        Application.Add("Simple_New_Item_View", GetAppConfigSetting("Simple_New_Item_View"))
        Application.Add("Allow_Unsecure_Excel_Export", GetAppConfigSetting("Allow_Unsecure_Excel_Export"))
        Application.Add("WebQuery_Show_NetUnitCost", GetAppConfigSetting("WebQuery_Show_NetUnitCost"))
        Application.Add("LDAP_Server", GetAppConfigSetting("LDAP_Server"))
        Application.Add("RefreshMessageOn", GetAppConfigSetting("RefreshMessageOn"))
        Application.Add("RefreshMessage", GetAppConfigSetting("RefreshMessage"))
        Application.Add("Check_ISS_AutoUpload", GetAppConfigSetting("Check_ISS_AutoUpload"))
        Application.Add("Check_ISS_Weekend_Proc", GetAppConfigSetting("Check_ISS_Weekend_Proc"))
        Application.Add("Text_ISS_Amount_Off", GetAppConfigSetting("Text_ISS_Amount_Off"))
        Application.Add("Text_ISS_Duration", GetAppConfigSetting("Text_ISS_Duration"))
        Application.Add("Text_ISS_Min_Margin", GetAppConfigSetting("Text_ISS_Min_Margin"))
        Application.Add("Text_ISS_Process_Delay", GetAppConfigSetting("Text_ISS_Process_Delay"))
        Application.Add("Text_ISS_Subteam_Ex", GetAppConfigSetting("Text_ISS_Subteam_Ex"))
        Application.Add("SSI_TitleDescription", GetAppConfigSetting("SSI_TitleDescription"))
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        
        ' if you add any new app config keys, you SET them here    
        Application.Set("LowMargin", GetAppConfigSetting("LowMargin"))
        Application.Set("HighMargin", GetAppConfigSetting("HighMargin"))
        Application.Set("CRV", GetAppConfigSetting("CRV"))
        Application.Set("userAccess", GetAppConfigSetting("userAccess"))
        Application.Set("maxTimeSpanMovement", GetAppConfigSetting("maxTimeSpanMovement"))
        Application.Set("ItemRequestEmail", GetAppConfigSetting("ItemRequestEmail"))
        Application.Set("ItemRequestRejectEmail", GetAppConfigSetting("ItemRequestRejectEmail"))
        Application.Set("VendorRequestEmail", GetAppConfigSetting("VendorRequestEmail"))
        Application.Set("InStoreSpecialEmail", GetAppConfigSetting("InStoreSpecialEmail"))
        Application.Set("InStoreSpecialRejectEmail", GetAppConfigSetting("InStoreSpecialRejectEmail"))
        Application.Set("InStoreSpecialEndSaleEarlyEmail", GetAppConfigSetting("InStoreSpecialEndSaleEarlyEmail"))
        Application.Set("ItemAuthorizationEmail", GetAppConfigSetting("ItemAuthorizationEmail"))
        Application.Set("RetailCostChangeEmail", GetAppConfigSetting("RetailCostChangeEmail"))
        Application.Set("IRMAPushEmail", GetAppConfigSetting("IRMAPushEmail"))
        Application.Set("PLURequestEmail", GetAppConfigSetting("PLURequestEmail"))
        Application.Set("Admin", GetAppConfigSetting("Admin"))
        Application.Set("RegionalTeam", GetAppConfigSetting("RegionalTeam"))
        Application.Set("RegionalCorporateOffice", GetAppConfigSetting("RegionalCorporateOffice"))
        Application.Set("encryptedConnectionStrings", GetAppConfigSetting("encryptedConnectionStrings"))
        Application.Set("CRV-BottleDepositLabel", GetAppConfigSetting("CRV-BottleDepositLabel"))
        Application.Set("CRV-BottleDepositSubteams", GetAppConfigSetting("CRV-BottleDepositSubteams"))
        Application.Set("IIS_PriceChgTypeID", GetAppConfigSetting("IIS_PriceChgTypeID"))
        Application.Set("WebQuery_Show_POSPrice", GetAppConfigSetting("WebQuery_Show_POSPrice"))
        Application.Set("Simple_New_Item_View", GetAppConfigSetting("Simple_New_Item_View"))
        Application.Set("Allow_Unsecure_Excel_Export", GetAppConfigSetting("Allow_Unsecure_Excel_Export"))
        Application.Set("WebQuery_Show_NetUnitCost", GetAppConfigSetting("WebQuery_Show_NetUnitCost"))
        Application.Set("LDAP_Server", GetAppConfigSetting("LDAP_Server"))
        Application.Set("RefreshMessageOn", GetAppConfigSetting("RefreshMessageOn"))
        Application.Set("RefreshMessage", GetAppConfigSetting("RefreshMessage"))
        Application.Set("Check_ISS_AutoUpload", GetAppConfigSetting("Check_ISS_AutoUpload"))
        Application.Set("Check_ISS_Weekend_Proc", GetAppConfigSetting("Check_ISS_Weekend_Proc"))
        Application.Set("Text_ISS_Amount_Off", GetAppConfigSetting("Text_ISS_Amount_Off"))
        Application.Set("Text_ISS_Duration", GetAppConfigSetting("Text_ISS_Duration"))
        Application.Set("Text_ISS_Min_Margin", GetAppConfigSetting("Text_ISS_Min_Margin"))
        Application.Set("Text_ISS_Process_Delay", GetAppConfigSetting("Text_ISS_Process_Delay"))
        Application.Set("Text_ISS_Subteam_Ex", GetAppConfigSetting("Text_ISS_Subteam_Ex"))
        Application.Set("SSI_TitleDescription", GetAppConfigSetting("SSI_TitleDescription"))
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

            Throw New Exception(KeyName & ex.InnerException.Message)

        Finally

            If Not dbConn Is Nothing Then
                If dbConn.State <> ConnectionState.Closed Then
                    dbConn.Close()
                End If
            End If

        End Try

    End Function
      
</script>
