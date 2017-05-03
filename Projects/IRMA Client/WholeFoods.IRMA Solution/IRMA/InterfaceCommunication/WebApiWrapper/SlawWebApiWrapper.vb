Imports WholeFoods.IRMA.InterfaceCommunication.WebApiModel
Imports WholeFoods.IRMA.Common.DataAccess
Imports System.Linq

Namespace WholeFoods.IRMA.InterfaceCommunication.WebApiWrapper
    Public Class SlawWebApiWrapper
        Inherits BaseWebApiWarapper
        Implements ISlawApi

        Shared storeNumberToBusinessUnit As Lazy(Of Dictionary(Of Integer, Integer)) = New Lazy(Of Dictionary(Of Integer, Integer))(
            Function() StoreListDAO.GetStoreNumberToBusinessUnitCollection())

        Sub New(urlString As String)
            MyBase.New(urlString)
        End Sub

        Public Sub GetSlawData() Implements ISlawApi.GetSlawData

        End Sub

        Public Sub PostPrintHeader(slawPrintBatchModel As SlawPrintBatchModel) Implements ISlawApi.PostPrintHeader
            Dim useSlawIntegratedSecurity As Boolean = SlawApiConfigurationDAO.GetConfiguredUseSlawIntegratedSecurity()
            Dim slawApiUrl As String = SlawApiConfigurationDAO.GetConfiguredSlawApiUrl()
            Dim slawTimeoutValue As Int32 = SlawApiConfigurationDAO.GetConfiguredSlawApiTimeout()
            Dim slawApiVersion As String = SlawApiConfigurationDAO.GetConfiguredSlawApiVersion()

            If (IsLabOrClosedStore(slawPrintBatchModel.BusinessUnitId)) Then
                logger.Info(String.Format("Print Batch Name: {0}, ID: {1}, BusinessUnit: {2} not sent to SLAW.",
                                          slawPrintBatchModel.BatchName,
                                          slawPrintBatchModel.BatchId,
                                          slawPrintBatchModel.BusinessUnitId))
                Exit Sub
            End If

            Dim slawJsonModel = BuildJsonModelByVersion(slawPrintBatchModel, slawApiVersion)

            If (useSlawIntegratedSecurity) Then
                PostAsJson(slawApiUrl, slawTimeoutValue, slawJsonModel, SlawApiConfigurationDAO.GetConfiguredSlawApiUser(), SlawApiConfigurationDAO.GetConfiguredSlawApiPassword(), slawApiVersion)
            Else
                PostAsJson(slawApiUrl, slawTimeoutValue, slawJsonModel, slawApiVersion)
            End If
        End Sub

        Private Function BuildJsonModelByVersion(slawBatchModel As SlawPrintBatchModel, slawApiVersion As String) As Object
            Dim slawJsonModel
            ' Version 1 uses property 'HasPriceChange'
            ' Newer versions will use the property 'BatchChangeType'
            If slawApiVersion = SlawConstants.SlawApiVersionOne Then
                slawJsonModel = New With
                {
                    Key slawBatchModel.BatchId,
                    Key slawBatchModel.BatchName,
                    Key slawBatchModel.BusinessUnitId,
                    Key slawBatchModel.BatchItems,
                    Key slawBatchModel.EffectiveDate,
                    Key slawBatchModel.BatchEvent,
                    Key slawBatchModel.BatchType,
                    Key slawBatchModel.Application,
                    Key slawBatchModel.IsAdHoc,
                    Key slawBatchModel.ItemCount,
                    Key slawBatchModel.HasPriceChange
                }
            Else
                slawJsonModel = New With
                {
                    Key slawBatchModel.BatchId,
                    Key slawBatchModel.BatchName,
                    Key slawBatchModel.BusinessUnitId,
                    Key slawBatchModel.BatchItems,
                    Key slawBatchModel.EffectiveDate,
                    Key slawBatchModel.BatchEvent,
                    Key slawBatchModel.BatchType,
                    Key slawBatchModel.Application,
                    Key slawBatchModel.IsAdHoc,
                    Key slawBatchModel.ItemCount,
                    Key slawBatchModel.BatchChangeType
                }
            End If

            Return slawJsonModel
        End Function

        Private Function IsLabOrClosedStore(businessUnitId As Integer) As Boolean
            Dim labAndClosedStores As String = SlawApiConfigurationDAO.GetConfiguredLabAndClosedStores()
            Dim result As Boolean = False
            Dim businessUnit As Integer

            If (Not String.IsNullOrEmpty(labAndClosedStores)) Then
                result = labAndClosedStores.Split("|") _
                    .Select(Function(s) IIf(storeNumberToBusinessUnit.Value.TryGetValue(s, businessUnit), businessUnit, -1)) _
                    .Any(Function(s) s = businessUnitId)
            End If

            Return result

        End Function
    End Class
End Namespace
