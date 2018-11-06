Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.POSPush.DataAccess
    Public Class HealthCheckDAO

        Public Shared Function GetIconStagingTableCount() As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim stagingCount As Integer

            Try
                stagingCount = CInt(factory.ExecuteScalar("select count(*) from IConPOSPushStaging"))
                Logger.LogInfo(String.Format("GetIconStagingTableCount succeeded.  Staging count: {0}", stagingCount), GetType(HealthCheckDAO))
            Catch ex As Exception
                Logger.LogError(String.Format("GetIconStagingTableCount failed with the following error: {0}", ex.ToString()), GetType(HealthCheckDAO))
            End Try

            Return stagingCount
        End Function

        Public Shared Function GetBatchesInSentStatus(jobRunDate As Date) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim sentCount As Integer

            Try
                sentCount = CInt(factory.ExecuteScalar(String.Format("select count(*) from PriceBatchHeader where PriceBatchStatusID = 5 and StartDate = '{0}' and SentDate < '{1}'", jobRunDate.Date, jobRunDate)))

                Logger.LogInfo(String.Format("GetBatchesInSentStatus succeeded.  Sent count: {0}", sentCount), GetType(HealthCheckDAO))

            Catch ex As Exception
                Logger.LogError(String.Format("GetBatchesInSentStatus failed with the following error: {0}", ex.ToString()), GetType(HealthCheckDAO))
            End Try

            Return sentCount
        End Function
    End Class
End Namespace
