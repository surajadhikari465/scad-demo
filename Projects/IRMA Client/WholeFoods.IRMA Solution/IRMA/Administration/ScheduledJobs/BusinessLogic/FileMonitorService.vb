Imports System
Imports System.ServiceModel
Imports IRMAFileMonitor
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.FileMonitor

    Public Class FileMonitor
        Private Shared stEndPointAddress As String

        Public Sub New()
            stEndPointAddress = ConfigurationServices.AppSettings("FileMonitorEndpoint")
        End Sub

        Public Shared Function GetUnprocessedPushFiles(ByVal stRegion As String) As DataTable
            Dim myBinding As New WSHttpBinding
            Dim myEndpoint As New EndpointAddress(stEndPointAddress)
            Dim dtResult As New DataTable

            Dim myChannelFactory As ChannelFactory(Of IFileMonitor) = _
            New ChannelFactory(Of IFileMonitor)(myBinding, myEndpoint)

            'Create a channel.
            Dim wcfClient1 As IFileMonitor = myChannelFactory.CreateChannel()

            dtResult = wcfClient1.GetUnprocessedPushFiles(stRegion)

            Dim clientChannel As IClientChannel = CType(wcfClient1, IClientChannel)
            clientChannel.Close()

            myChannelFactory.Close()
            Return dtResult

        End Function


    End Class
End Namespace


