Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Pricing.DataAccess

    Public Class SlawPrintBatchConfigurationDAO

        Public Shared Function SlawPrintRequestsEnabledForRegion() As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim isSlawEnabled As Boolean

            Dim result = factory.ExecuteScalar("SELECT dbo.fn_GetAppConfigValue(" & "'EnableSlawPrintBatching'" & "," & "'IRMA Client'" & ")")

            If result Is Nothing Then
                isSlawEnabled = False
            Else
                Dim configValue As String = "0"

                configValue = CType(result, String)

                If configValue = "1" Then
                    isSlawEnabled = True
                Else
                    isSlawEnabled = False
                End If
            End If

            Return isSlawEnabled
        End Function
    End Class
End Namespace
