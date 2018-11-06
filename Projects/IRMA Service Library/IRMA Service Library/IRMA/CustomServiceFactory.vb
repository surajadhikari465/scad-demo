Imports System.ServiceModel
Imports System.ServiceModel.Activation

Namespace IRMA

    Public Class CustomServiceFactory : Inherits ServiceHostFactory

        Private baseAddressIndex As Integer = 0

        Protected Overrides Function CreateServiceHost(ByVal serviceType As System.Type, ByVal baseAddresses() As System.Uri) As ServiceHost
            Return New ServiceHost(serviceType, baseAddresses(baseAddressIndex))
        End Function

    End Class

End Namespace