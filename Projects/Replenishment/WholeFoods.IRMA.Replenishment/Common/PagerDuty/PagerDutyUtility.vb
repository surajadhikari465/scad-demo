Imports PagerDutyLib
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class PagerDutyUtility

    Public Shared Sub TriggerPagerDutyAlert(clientName As String, description As String, errorMessage As String)
        If AlertsEnabled() Then
            Try
                Dim regionCode As String = GetRegionCode()

                errorMessage = errorMessage.Insert(0, String.Format("Region: {0} - ", regionCode))
                errorMessage = If(errorMessage.Length > 1024, errorMessage.Substring(0, 1023), errorMessage)

                Dim response As EventAPIResponse = PagerDutyTrigger.CreateFromConfig(clientName).TriggerIncident(
                    description,
                    New Dictionary(Of String, String) From {{"Exception", errorMessage}})

                If response.status = "success" Then
                    Logger.LogInfo(String.Format("Successfully sent PagerDuty notification from client: {0}", clientName), GetType(PagerDutyUtility))
                Else
                    Logger.LogError(String.Format("Failed to send PagerDuty notification from client: {0}.  Error: {1}", clientName, response.message), GetType(PagerDutyUtility))
                    Dim response2 As EventAPIResponse = PagerDutyTrigger.CreateFromConfig(clientName).TriggerIncident(
                    String.Format("POS Push Job Errored out. Failed to send PagerDuty notification from client first attempt: {0}", clientName),
                        New Dictionary(Of String, String) From {{"Exception", "Error in sending Page Duty alert"}})
                    If response2.status = "success" Then
                        Logger.LogInfo(String.Format("Successfully resent PagerDuty notification from client: {0}.", clientName), GetType(PagerDutyUtility))
                    Else
                        Logger.LogError(String.Format("Failed to resend PagerDuty notification from client: {0}.  Error: {1}", clientName, response.message), GetType(PagerDutyUtility))
                    End If
                End If
            Catch e As Exception
                Logger.LogError(String.Format("An unexpected error occurred while sending the PagerDuty notification from client: {0}.  Error: ", clientName), GetType(PagerDutyUtility), e)
            End Try
        End If
    End Sub

    Private Shared Function AlertsEnabled() As Boolean
        Try
            Return CBool(ConfigurationServices.AppSettings("EnablePagerDutyAlerts"))
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
            Logger.LogError(String.Format("GetRegionCode failed with the following error: {0}", ex.ToString()), GetType(PagerDutyUtility))
        End Try

        Return regionCode
    End Function
End Class
