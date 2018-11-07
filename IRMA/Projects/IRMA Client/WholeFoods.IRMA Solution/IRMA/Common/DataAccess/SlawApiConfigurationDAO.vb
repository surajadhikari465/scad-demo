Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Common.DataAccess
    Public Class SlawApiConfigurationDAO

        ''' <summary>
        ''' Gets SlawApiUser
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetConfiguredSlawApiUser() As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As String = String.Empty

            Try
                Dim configObject = factory.ExecuteScalar("SELECT dbo.fn_GetAppConfigValue(" & "'SlawApiUser'" & "," & "'IRMA Client'" & ")")
                If configObject Is Nothing Then
                    returnVal = String.Empty
                Else
                    Dim configValue As String = "0"
                    configValue = CType(configObject, String)
                    Return configValue
                End If
            Catch exception As Exception
                MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly, exception.Message)
            End Try

            Return returnVal
        End Function

        ''' <summary>
        '''Gets SlawApiPassword
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetConfiguredSlawApiPassword() As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As String = String.Empty

            Try
                Dim configObject = factory.ExecuteScalar("SELECT dbo.fn_GetAppConfigValue(" & "'SlawApiPassword'" & "," & "'IRMA Client'" & ")")
                If configObject Is Nothing Then
                    returnVal = String.Empty
                Else
                    returnVal = CType(configObject, String)
                End If
            Catch exception As Exception
                MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly, exception.Message)
            End Try

            Return returnVal
        End Function

        ''' <summary>
        ''' Checks to see if UseSlawIntegratedSecurity is enabled or not
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetConfiguredUseSlawIntegratedSecurity() As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As Boolean = False

            Try
                Dim configObject = factory.ExecuteScalar("SELECT dbo.fn_GetAppConfigValue(" & "'UseSlawIntegratedSecurity'" & "," & "'IRMA Client'" & ")")
                If configObject Is Nothing Then
                    returnVal = False
                Else
                    Dim configValue As String = "0"
                    configValue = CType(configObject, String)
                    If configValue <> "1" Then
                        returnVal = False
                    Else
                        returnVal = True
                    End If
                End If
            Catch exception As Exception
                MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly, exception.Message)
            End Try

            Return returnVal
        End Function

        ''' <summary>
        ''' Gets SlawApiUrl
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetConfiguredSlawApiUrl() As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As String = String.Empty

            Try
                Dim configObject = factory.ExecuteScalar("SELECT dbo.fn_GetAppConfigValue(" & "'SlawApiUrl'" & "," & "'IRMA Client'" & ")")
                If configObject Is Nothing Then
                    returnVal = String.Empty
                Else
                    returnVal = CType(configObject, String)
                End If
            Catch exception As Exception
                MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly, exception.Message)
            End Try

            Return returnVal
        End Function

        Public Shared Function GetConfiguredSlawApiTimeout() As Int32
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As Int32

            Try
                Dim configObject = factory.ExecuteScalar("SELECT dbo.fn_GetAppConfigValue(" & "'SlawApiTimeoutMilliseconds'" & "," & "'IRMA Client'" & ")")
                If configObject Is Nothing Or IsDBNull(configObject) Then
                    'Set default timeout if nothing found
                    returnVal = 60000
                Else
                    returnVal = CType(configObject, Int32)
                End If
            Catch exception As Exception
                MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly, exception.Message)
            End Try

            Return returnVal
        End Function

        Public Shared Function GetConfiguredSlawApiVersion() As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As String = String.Empty

            Try
                Dim configObject = factory.ExecuteScalar("SELECT dbo.fn_GetAppConfigValue(" & "'SlawApiVersion'" & "," & "'IRMA Client'" & ")")
                If configObject Is Nothing Then
                    returnVal = String.Empty
                Else
                    returnVal = CType(configObject, String)
                End If
            Catch exception As Exception
                MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly, exception.Message)
            End Try

            Return returnVal
        End Function

        ''' <summary>
        ''' GetsConfiguredLabAndClosedStores
        ''' </summary>
        ''' <returns>The pipebar delimited list of lab and closed stores.</returns>
        Public Shared Function GetConfiguredLabAndClosedStores() As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As String = String.Empty

            Try
                Dim configObject = factory.ExecuteScalar("SELECT dbo.fn_GetAppConfigValue(" & "'LabAndClosedStoreNo'" & "," & "'IRMA Client'" & ")")
                If configObject Is Nothing Then
                    returnVal = String.Empty
                Else
                    Dim configValue As String = String.Empty
                    configValue = CType(configObject, String)
                    Return configValue
                End If
            Catch exception As Exception
                MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly, exception.Message)
            End Try

            Return returnVal
        End Function
    End Class
End Namespace
