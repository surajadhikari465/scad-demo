Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.POSPush.DataAccess
    Public Class POSConfigTypeDAO
        ''' <summary>
        ''' _configTypeList contains constant configuration data that only needs to be populated once.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _configTypeList As ArrayList

        ''' <summary>
        ''' This method populates the selection of config types.
        ''' They are hard-coded in the application.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetConfigTypes() As ArrayList
            Logger.LogDebug("GetConfigTypes entry", Nothing)
            Dim configEntry As POSConfigTypeBO

            If (_configTypeList Is Nothing) Then
                ' This list is static.  It is only populated the first time the method is accessed.
                _configTypeList = New ArrayList
                configEntry = New POSConfigTypeBO("direct", "direct")
                _configTypeList.Add(configEntry)
                configEntry = New POSConfigTypeBO("sent", "sent")
                _configTypeList.Add(configEntry)
            End If

            Logger.LogDebug("GetConfigTypes exit: # types=" + _configTypeList.Count.ToString, Nothing)
            Return _configTypeList
        End Function

    End Class
End Namespace
