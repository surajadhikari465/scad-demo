Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.Common.DataAccess
    Public Class DataArchiveDAO
#Region "Read Methods"
        ''' <summary>
        ''' Read the complete list of archive Tables.
        ''' </summary>
        ''' <exception cref="DataFactoryException" />
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetArchiveTables() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return factory.GetStoredProcedureDataSet("Administration_DataArchive_GetDataArchiveList")

        End Function

        ''' <summary>
        ''' Populates the DataArchiverBO with the results for the ItemCatalog.dataArchive table.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function PopulateArchiveTableFromValidateResults(ByRef results As SqlDataReader) As DataArchiveBO

            Dim returnArchiveTable As New DataArchiveBO()

            If (Not results.IsDBNull(results.GetOrdinal("DataArchiveID"))) Then
                returnArchiveTable.DataArchiveID = results.GetInt32(results.GetOrdinal("DataArchiveID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("TableName"))) Then
                returnArchiveTable.TableName = results.GetString(results.GetOrdinal("TableName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("StoredProcName"))) Then
                returnArchiveTable.StoredProcName = results.GetString(results.GetOrdinal("StoredProcName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ChangeTypeID"))) Then
                returnArchiveTable.ChangeTypeID = results.GetInt32(results.GetOrdinal("ChangeTypeID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("JobFrequencyID"))) Then
                returnArchiveTable.JobFrequencyID = results.GetInt32(results.GetOrdinal("JobFrequencyID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("RetentionDays"))) Then
                returnArchiveTable.RetentionDays = results.GetInt32(results.GetOrdinal("RetentionDays"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("IsEnabled"))) Then
                returnArchiveTable.RetentionDays = results.GetBoolean(results.GetOrdinal("IsEnabled"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ProcessStatus"))) Then
                returnArchiveTable.RetentionDays = results.GetInt32(results.GetOrdinal("ProcessStatus"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ProcessDate"))) Then
                returnArchiveTable.ProcessDate = results.GetDateTime(results.GetOrdinal("ProcessDate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("LastUpdate"))) Then
                returnArchiveTable.LastUpdate = results.GetDateTime(results.GetOrdinal("LastUpdate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("LastUpdateUserID"))) Then
                returnArchiveTable.LastUpdateUserID = results.GetBoolean(results.GetOrdinal("LastUpdateUSerID"))
            End If

            Return returnArchiveTable

        End Function

        Public Shared Function GetArchiveTable(ByVal DataArchiveID As Integer) As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try

                ' Setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "intDataArchiveID"
                currentParam.Value = DataArchiveID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("Administration_DataArchive_GetDataArchiveTable", paramList)

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function

#End Region

#Region "Create, Update Methods"
        ''' <summary>
        ''' Creates the ArrayList for the parameters that are common between inserts and updates.
        ''' </summary>
        ''' <param name="currentArchiveTable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function DefineArchiveTableParams(ByRef currentArchiveTable As DataArchiveBO) As ArrayList

            ' Setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@strTableName"
            currentParam.Value = currentArchiveTable.TableName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@strStoredProcName"
            currentParam.Value = currentArchiveTable.StoredProcName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@intChangeTypeID"
            currentParam.Value = currentArchiveTable.ChangeTypeID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@intJobFrequencyID"
            currentParam.Value = currentArchiveTable.JobFrequencyID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@intRetentionDays"
            currentParam.Value = currentArchiveTable.RetentionDays
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@bitIsEnabled"
            currentParam.Value = currentArchiveTable.IsEnabled
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@dtmProcessDate"
            currentParam.Value = currentArchiveTable.ProcessDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@dtmLastUpdate"
            currentParam.Value = currentArchiveTable.LastUpdate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@intLastUpdateUserID"
            currentParam.Value = currentArchiveTable.LastUpdateUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Return paramList

        End Function

        ''' <summary>
        ''' Insert a new record into the DataArchive table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub AddDataArchiveRecord(ByRef currentDataArchive As DataArchiveBO)

            Logger.LogDebug("AddDataArchiveRecord entry", Nothing)

            Dim retVal As New ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Setup parameters for stored proc
            Dim paramList As ArrayList = DefineArchiveTableParams(currentDataArchive)

            ' Execute the stored procedure to insert the new Data Archive record.
            factory.ExecuteStoredProcedure("Administration_DataArchive_InsertDataArchiveTable", paramList)

            Logger.LogDebug("AddDataArchiveRecord exit", Nothing)

        End Sub

        ''' <summary>
        ''' Update an existing record in the Users table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdateDataArchiveRecord(ByRef currentDataArchive As DataArchiveBO)

            Logger.LogDebug("UpdateDataArchiveRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Setup parameters for stored proc
            Dim paramList As ArrayList = DefineArchiveTableParams(currentDataArchive)
            Dim currentParam As DBParam

            ' Add the param for the DataArchiveID
            currentParam = New DBParam
            currentParam.Name = "@strDataArchiveID"
            currentParam.Value = currentDataArchive.DataArchiveID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to update a Data Archive record.
            factory.ExecuteStoredProcedure("[Administration_DataArchive_UpdateDataArchiveTable]", paramList)
            Logger.LogDebug("UpdateDataArchiveRecord exit", Nothing)

        End Sub

#End Region

    End Class
End Namespace
