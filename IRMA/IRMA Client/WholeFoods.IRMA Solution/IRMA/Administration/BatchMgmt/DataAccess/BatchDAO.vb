Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.BatchMgmt.DataAccess
    Public Class BatchDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Function GetHeaderSearchData(ByRef searchCriteria As BatchSearchBO) As ArrayList
            logger.Debug("GetHeaderSearchData entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim batchList As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "StoreList"
                currentParam.Value = searchCriteria.StoreList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreListSeparator"
                currentParam.Value = searchCriteria.ListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PriceBatchStatusIDList"
                currentParam.Value = searchCriteria.BatchStatusList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PriceBatchStatusIDSeparator"
                currentParam.Value = searchCriteria.ListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "BatchDescription"
                If searchCriteria.BatchDesc IsNot Nothing Then
                    If searchCriteria.BatchDesc.Length > 0 Then
                        currentParam.Value = searchCriteria.BatchDesc
                    Else
                        currentParam.Value = DBNull.Value
                    End If
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.CommandTimeout = 1200
                results = factory.GetStoredProcedureDataReader("Administration_GetPriceBatchHeaderSearch", paramList)

                ' Build the list of batches that match the search criteria
                Dim currentBatch As BatchHeaderBO
                While results.Read
                    currentBatch = New BatchHeaderBO(results)
                    batchList.Add(currentBatch)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetHeaderSearchData exit: batch count=" + batchList.Count.ToString)
            Return batchList
        End Function

        ''' <summary>
        ''' updates PriceBatchHeader.PriceBatchStatusID w/ passed in status value.
        ''' returns new status description (PriceBatchStatus.PriceBatchStatusDesc) to calling form
        ''' </summary>
        ''' <param name="header"></param>
        ''' <returns>PriceBatchStatus.PriceBatchStatusDesc of new status</returns>
        ''' <remarks></remarks>
        Public Shared Function UpdatePriceBatchStatus(ByVal header As BatchHeaderBO) As String
            logger.Debug("UpdatePriceBatchStatus entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim newStatusDesc As String = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "PriceBatchHeaderID"
                currentParam.Value = header.PriceBatchHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PriceBatchStatusID"
                currentParam.Value = header.BatchStatusID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("UpdatePriceBatchStatus", paramList)

                While results.Read
                    newStatusDesc = results.GetString(results.GetOrdinal("PriceBatchStatusDesc"))
                End While

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
            logger.Debug("UpdatePriceBatchStatus exit")
            Return newStatusDesc
        End Function

        ''' <summary>
        ''' Queries the database to get the most current state for a batch.
        ''' </summary>
        ''' <param name="header"></param>
        ''' <returns>PriceBatchHeader.PriceBatchStatusID</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCurrentPriceBatchStatus(ByVal header As BatchHeaderBO) As Integer
            logger.Debug("GetCurrentPriceBatchStatus entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim statusID As Integer = -1

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "PriceBatchHeaderID"
                currentParam.Value = header.PriceBatchHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_GetPriceBatchHeaderStatus", paramList)

                While results.Read
                    statusID = CInt(results.GetValue(results.GetOrdinal("PriceBatchStatusID")))
                End While

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
            logger.Debug("GetCurrentPriceBatchStatus exit")
            Return statusID
        End Function
    End Class
End Namespace
