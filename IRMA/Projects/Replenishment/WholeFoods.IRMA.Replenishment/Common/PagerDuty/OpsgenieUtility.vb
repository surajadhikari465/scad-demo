Imports OpsgenieAlert
Imports OpsgenieLib
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class OpsGenieUtility

    Public Shared Sub TriggerOpsgenieAlert(clientName As String, description As String, errorMessage As String)
        If AlertsEnabled() Then
            Try
                Dim regionCode As String = GetRegionCode()

                errorMessage = errorMessage.Insert(0, String.Format("Region: {0} - ", regionCode))
                errorMessage = If(errorMessage.Length > 1024, errorMessage.Substring(0, 1023), errorMessage)

                Dim response As OpsgenieResponse = OpsgenieTrigger.CreateFromConfig().TriggerAlert(
                    "POS Push Job Issue",
                    description,
                    New Dictionary(Of String, String) From {{"Exception", errorMessage}})

                If String.IsNullOrEmpty(response.Error) Then
                    Logger.LogInfo(String.Format("Successfully sent Opsgenie alert from client: {0}", clientName), GetType(OpsGenieUtility))
                Else
                    Logger.LogError(String.Format("Failed to send Opsgenie alert from client: {0}.  Error: {1}", clientName, response.Error), GetType(OpsGenieUtility))
                    Dim response2 As OpsgenieResponse = OpsgenieTrigger.CreateFromConfig().TriggerAlert("POS Push Job Issue",
                    String.Format("POS Push Job Errored out. Failed to send Opsgenie alert from client first attempt: {0}", clientName),
                        New Dictionary(Of String, String) From {{"Exception", "Error in sending Opsgenie alert"}})
                    If String.IsNullOrEmpty(response.Error) Then
                        Logger.LogInfo(String.Format("Successfully resent Opsgenie alert from client: {0}.", clientName), GetType(OpsGenieUtility))
                    Else
                        Logger.LogError(String.Format("Failed to resend Opsgenie alert from client: {0}.  Error: {1}", clientName, response.Error), GetType(OpsGenieUtility))
                    End If
                End If
            Catch e As Exception
                Logger.LogError(String.Format("An unexpected error occurred while sending the Opsgenie alert from client: {0}.  Error: ", clientName), GetType(OpsGenieUtility), e)
            End Try
        End If
    End Sub

    Private Shared Function AlertsEnabled() As Boolean
        Try
            Return CBool(ConfigurationServices.AppSettings("EnableErrorAlerts"))
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Shared Function GetRegionCode() As String
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim regionCode As String = String.Empty

        Try
            regionCode = factory.ExecuteScalar("select RegionCode from Region").ToString()
        Catch ex As Exception
            Logger.LogError(String.Format("GetRegionCode failed with the following error: {0}", ex.ToString()), GetType(OpsGenieUtility))
        End Try

        Return regionCode
    End Function
End Class
