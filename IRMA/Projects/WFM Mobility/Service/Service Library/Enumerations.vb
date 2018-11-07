Namespace UniversalHandheldServiceLibrary

    Public Class Enumerations

#Region " Enumerations"

        ''' <summary>
        ''' Possible responses returned by the Authenticate service method.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum AuthenticationResponse
            Authenticated
            BadPassword
            AccountNotFound
            NotInRegionADGroup
            NotInPluginADGroup
            RegionNotSpecified
            PluginNotSpecified
            NoPluginsAvailable
            RegionNotConfigured
            UnknownError
        End Enum

#End Region

    End Class

End Namespace
