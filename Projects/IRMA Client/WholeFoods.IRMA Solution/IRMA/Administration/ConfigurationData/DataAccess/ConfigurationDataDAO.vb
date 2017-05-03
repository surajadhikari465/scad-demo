Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers.Constants

Namespace WholeFoods.IRMA.Administration.ConfigurationData.DataAccess
    Public Class ConfigurationDataDAO
        ' ************************************************************
        ' THIS CLASS IS DUPLICATED in the IRMA CLIENT project.
        ' until we have a repository for common code.
        ' ************************************************************

        ' The purpose of this class is to manage the ConfigurationData table
        ' The table is made to take data of various types and store them in the same sql_variant field
        ' to be available as values in the application

        ' Rick Kelleher

        Private factory As DataFactory

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            CreateDatabaseConnection()
        End Sub

        ''' <summary>
        ''' CreateDatabaseConnection from the data factory
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CreateDatabaseConnection()
            If factory Is Nothing Then
                factory = New DataFactory(DataFactory.ItemCatalog)
            End If
        End Sub

        ''' <summary>
        ''' GetConfigInfo() returns all configuration data records from the ConfigurationData table
        ''' </summary>
        ''' <returns>Returns a dataset containing a single table</returns>
        ''' <remarks></remarks>
        Public Function GetConfigInfo() As DataSet
            Dim ds As DataSet = factory.GetStoredProcedureDataSet("GetConfigurationData")
            Return ds
        End Function

        ''' <summary>
        ''' GetConfigDocument() returns the string representation of the XML configuration for the
        ''' specified Application and Environment.
        ''' </summary>
        ''' <returns>Returns a dataset containing a single string</returns>
        ''' <remarks></remarks>
        Public Function GetConfigDocument(ByVal AppID As Guid, ByVal EnvID As Guid) As String

            Dim retVal As String = factory.GetConfigDocument(AppID, EnvID)
            Return retVal

        End Function

        ''' <summary>
        ''' GetConfigDocument() returns the string representation of the XML configuration for the
        ''' specified Application and Environment.
        ''' </summary>
        ''' <returns>Returns a dataset containing a single string</returns>
        ''' <remarks></remarks>
        Public Function GetConfigKeyList(ByVal AppID As Guid, ByVal EnvID As Guid) As String

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@EnvironmentID"
            currentParam.Value = EnvID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ApplicationID"
            currentParam.Value = AppID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            Dim dt As DataTable = factory.GetStoredProcedureDataTable("AppConfig_GetConfigKeyList", paramList)
            Dim dr As DataRow = Nothing

            Dim _keyXML As String = String.Empty

            For Each dr In dt.Rows

                _keyXML = _keyXML & dr.Item(0).ToString

            Next

            Return "<configuration><appSettings>" & _keyXML & "</appSettings></configuration>"

        End Function

        ''' <summary>
        ''' GetConfigValue() returns a single configuration data record from the ConfigurationData table
        ''' </summary>
        ''' <param name="sConfigKey">The key (a string) defining the value to retrieve</param>
        ''' <returns>Returns an object of elemental data type (actaully a value of type SQL_VARIANT)</returns>
        ''' <remarks></remarks>
        Public Function GetConfigValue(ByVal sConfigKey As String) As Object
            Dim retval As Object = factory.ExecuteScalar("EXEC GetConfigurationValue '" & sConfigKey & "'")
            Return retval
        End Function

        ''' <summary>
        ''' UpdateConfigInfo is for updating the dataset created by GetConfigInfo(). 
        ''' </summary>
        ''' <param name="ds">The dataset containing a single table containing the modified data to be updated.</param>
        ''' <param name="TblName">The name of the table in the dataset</param>
        ''' <returns></returns>
        ''' <remarks>It updates the value of all records marked as modified back into the ConfigurationData table.
        ''' The table must have the same structure as the one created by GetConfigInfo().
        ''' It attempts to keep the base data type intact of the values being saved back into the SQL_VARIANT field.
        ''' </remarks>
        Public Function UpdateConfigInfo(ByVal ds As DataSet, ByVal TblName As String) As Boolean
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            UpdateConfigInfo = False
            If ds.HasChanges Then
                ' get only the changed rows for updating
                Dim dv As DataView = New DataView(ds.Tables(TblName), "", "", DataViewRowState.ModifiedCurrent)
                Dim rowView As DataRowView

                For Each rowView In dv
                    paramList = New ArrayList

                    ' setup parameters for stored proc
                    currentParam = New DBParam
                    currentParam.Name = "iConfigKey"
                    currentParam.Value = rowView.Item("ConfigKey").ToString
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "iConfigValue"
                    currentParam.Value = rowView.Item("ConfigValue")
                    Select Case rowView.Item("BaseType").ToString
                        Case "bit"
                            currentParam.Type = DBParamType.Bit
                        Case "char"
                            currentParam.Type = DBParamType.Char
                        Case "datetime"
                            currentParam.Type = DBParamType.DateTime
                        Case "decimal", "real"
                            currentParam.Type = DBParamType.Decimal
                        Case "float"
                            currentParam.Type = DBParamType.Float
                        Case "int"
                            currentParam.Type = DBParamType.Int
                        Case "money"
                            currentParam.Type = DBParamType.Money
                        Case "smallint"
                            currentParam.Type = DBParamType.SmallInt
                        Case "varchar"
                            currentParam.Type = DBParamType.String
                    End Select
                    paramList.Add(currentParam)
                    ' Execute the stored procedure 
                    factory.ExecuteStoredProcedure("UpdateConfigurationData", paramList)
                Next
            End If
            UpdateConfigInfo = True
        End Function

        Public Function GetApplicationList(ByVal EnvironmentID As Guid) As DataTable

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@EnvironmentID"
            currentParam.Value = EnvironmentID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            Dim dt As DataTable = factory.GetStoredProcedureDataTable("AppConfig_AppList", paramList)
            Return dt

        End Function

        Public Function GetConfigList() As DataTable

            Dim dt As DataTable = factory.GetStoredProcedureDataTable("AppConfig_GetConfigList")
            Return dt

        End Function

        Public Function GetEnvironmentList() As DataTable
            Dim dt As DataTable = factory.GetStoredProcedureDataTable("AppConfig_EnvList")
            Return dt
        End Function

        Public Function GetApplicationTypeList() As DataTable
            Dim dt As DataTable = factory.GetStoredProcedureDataTable("AppConfig_TypeList")
            Return dt
        End Function

        Public Function GetApplicationKeyList() As DataTable
            Dim dt As DataTable = factory.GetStoredProcedureDataTable("AppConfig_KeyList")
            Return dt
        End Function

        Public Function AddAppConfigApp(ByVal AppConfigApp As AppConfigAppBO) As AppConfigAppBO

            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim retVal As New ArrayList

            paramList = New ArrayList

            ' setup parameters for stored proc

            '//OUTPUT
            currentParam = New DBParam
            currentParam.Name = "@ApplicationID"
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)
            '//

            currentParam = New DBParam
            currentParam.Name = "@EnvironmentID"
            currentParam.Value = AppConfigApp.EnvironmentID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@TypeID"
            currentParam.Value = AppConfigApp.Type
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Name"
            currentParam.Value = AppConfigApp.Name
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@User_ID"
            currentParam.Value = AppConfigApp.UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            retVal = factory.ExecuteStoredProcedure("AppConfig_AddApp", paramList)

            AppConfigApp.ApplicationID = retVal(0)

            Return AppConfigApp

        End Function

        Public Function AddAppConfigEnv(ByVal AppConfigEnv As AppConfigEnvBO) As AppConfigEnvBO

            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim retVal As New ArrayList

            paramList = New ArrayList

            ' setup parameters for stored proc

            '//OUTPUT
            currentParam = New DBParam
            currentParam.Name = "@EnvironmentID"
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)
            '//

            currentParam = New DBParam
            currentParam.Name = "@Name"
            currentParam.Value = AppConfigEnv.Name
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Shortname"
            currentParam.Value = AppConfigEnv.Shortname
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@User_ID"
            currentParam.Value = My.Application.CurrentUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            retVal = factory.ExecuteStoredProcedure("AppConfig_AddEnv", paramList)

            AppConfigEnv.EnvironmentID = retVal(0)

            Return AppConfigEnv

        End Function

        Public Function AddAppConfigKey(ByVal AppConfigKey As AppConfigKeyBO) As AppConfigKeyBO

            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim retVal As New ArrayList

            paramList = New ArrayList

            ' setup parameters for stored proc

            '//OUTPUT
            currentParam = New DBParam
            currentParam.Name = "@KeyID"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            '//

            currentParam = New DBParam
            currentParam.Name = "@Name"
            currentParam.Value = AppConfigKey.Name
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@User_ID"
            currentParam.Value = AppConfigKey.UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            retVal = factory.ExecuteStoredProcedure("AppConfig_AddKey", paramList)

            AppConfigKey.KeyID = retVal(0)

            Return AppConfigKey

        End Function

        Public Function ImportKey(ByVal AppConfigKey As AppConfigKeyBO) As AppConfigKeyBO

            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim retVal As New ArrayList

            paramList = New ArrayList

            ' setup parameters for stored proc

            '//OUTPUT
            currentParam = New DBParam
            currentParam.Name = "@KeyID"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            '//

            currentParam = New DBParam
            currentParam.Name = "@Name"
            currentParam.Value = AppConfigKey.Name
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@User_ID"
            currentParam.Value = AppConfigKey.UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            retVal = factory.ExecuteStoredProcedure("AppConfig_ImportKey", paramList)

            AppConfigKey.KeyID = retVal(0)

            Return AppConfigKey

        End Function

        Public Sub AddAppConfigValue(ByVal AppConfigValue As AppConfigValueBO, ByVal UpdateExistingKeyValue As Boolean)

            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim retVal As New ArrayList

            paramList = New ArrayList

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "@EnvironmentID"
            currentParam.Value = AppConfigValue.EnvironmentID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ApplicationID"
            currentParam.Value = AppConfigValue.ApplicationID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@UpdateExistingKeyValue"
            currentParam.Value = UpdateExistingKeyValue
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@KeyID"
            currentParam.Value = AppConfigValue.KeyID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Value"
            currentParam.Value = AppConfigValue.Value
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@User_ID"
            currentParam.Value = AppConfigValue.UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("AppConfig_AddKeyValue", paramList)

        End Sub

        Public Function UpdateKeyValue(ByVal AppConfigValue As AppConfigValueBO) As Boolean

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            paramList = New ArrayList

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "@EnvironmentID"
            currentParam.Value = AppConfigValue.EnvironmentID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ApplicationID"
            currentParam.Value = AppConfigValue.ApplicationID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@KeyID"
            currentParam.Value = AppConfigValue.KeyID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Value"
            currentParam.Value = AppConfigValue.Value
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@User_ID"
            currentParam.Value = AppConfigValue.UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("AppConfig_UpdateKeyValue", paramList)

            Return True

        End Function

        Public Function RemoveAppConfigEnv(ByVal AppConfigEnv As AppConfigEnvBO) As Boolean

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            paramList = New ArrayList

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "@EnvironmentID"
            currentParam.Value = AppConfigEnv.EnvironmentID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@User_ID"
            currentParam.Value = AppConfigEnv.UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("AppConfig_RemoveEnv", paramList)

            Return True

        End Function

        Public Function RemoveAppConfigApp(ByVal AppConfigApp As AppConfigAppBO) As Boolean

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            paramList = New ArrayList

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "@ApplicationID"
            currentParam.Value = AppConfigApp.ApplicationID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@EnvironmentID"
            currentParam.Value = AppConfigApp.EnvironmentID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@User_ID"
            currentParam.Value = AppConfigApp.UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("AppConfig_RemoveApp", paramList)

            Return True

        End Function

        Public Function RemoveAppConfigValue(ByVal AppConfigValue As AppConfigValueBO) As Boolean

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            paramList = New ArrayList

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "@ApplicationID"
            If AppConfigValue.ApplicationID = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = AppConfigValue.ApplicationID
            End If
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@EnvironmentID"
            If AppConfigValue.EnvironmentID = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = AppConfigValue.EnvironmentID
            End If
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@KeyID"
            currentParam.Value = AppConfigValue.KeyID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@User_ID"
            currentParam.Value = AppConfigValue.UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("AppConfig_RemoveKey", paramList)

            Return True

        End Function

        Public Function Save(ByVal AppConfigApp As AppConfigAppBO) As Boolean

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            paramList = New ArrayList

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "@ApplicationID"
            currentParam.Value = AppConfigApp.ApplicationID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@EnvironmentID"
            currentParam.Value = AppConfigApp.EnvironmentID
            currentParam.Type = DBParamType.UniqueIdentifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Configuration"
            currentParam.Value = AppConfigApp.Configuration.OuterXml
            currentParam.Type = DBParamType.Xml
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@User_ID"
            currentParam.Value = AppConfigApp.UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("AppConfig_SaveBuildConfig", paramList)

            Return True

        End Function


    End Class


End Namespace
