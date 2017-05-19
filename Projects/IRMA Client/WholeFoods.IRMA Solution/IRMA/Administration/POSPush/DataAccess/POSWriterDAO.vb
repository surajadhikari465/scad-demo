Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.POSPush.DataAccess
    Public Class POSWriterDAO
        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("POSWriterDAO")

#Region "Read methods"
        Public Overloads Shared Function GetFileWriters(ByVal fileWriterType As String, ByVal scaleWriterTypeDesc1 As String, Optional ByVal scaleWriterTypeDesc2 As String = Nothing) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "FileWriterType"
            If fileWriterType Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = fileWriterType
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ScaleWriterTypeDesc1"
            If scaleWriterTypeDesc1 Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = scaleWriterTypeDesc1
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ScaleWriterTypeDesc2"
            If scaleWriterTypeDesc2 Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = scaleWriterTypeDesc2
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetPOSWriters", paramList)
        End Function

        ''' <summary>
        ''' Reads all of the POSWriter records.
        ''' Executes the POSGetWriters stored procedure.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetFileWriters(ByVal fileWriterType As String) As DataSet
            Return GetFileWriters(fileWriterType, Nothing)
        End Function

        ''' <summary>
        ''' Reads the list of Stores assigned to the POSWriter.
        ''' Executes the POSGetStoresByWriter stored procedure.
        ''' </summary>
        ''' <param name="currentWriter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetStoresAssignedToWriter(ByRef currentWriter As POSWriterBO) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "FileWriterKey"
            currentParam.Value = currentWriter.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FileWriterType"
            currentParam.Value = currentWriter.WriterType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetStoresByWriter", paramList)
        End Function

        ''' <summary>
        ''' Read the number of rows configured in the POSWriterFileConfig for the given file writer and change type.
        ''' Executes the POSGetWriterFileConfigurationRowCount stored procedure.
        ''' </summary>
        ''' <param name="currentWriter"></param>
        ''' <param name="currentChangeType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRowCountForWriter(ByRef currentWriter As POSWriterBO, ByRef currentChangeType As POSChangeTypeBO) As Integer
            Logger.LogDebug("GetRowCountForWriter entry", CLASSTYPE)
            Dim maxRows As Integer = 0
            Dim results As SqlDataReader = Nothing
            Try
                Dim factory As New DataFactory(DataFactory.ItemCatalog)

                ' setup parameters for stored proc
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "FileWriterKey"
                currentParam.Value = currentWriter.POSFileWriterKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ChangeTypeKey"
                currentParam.Value = currentChangeType.POSChangeTypeKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_POSPush_GetPOSWriterFileConfigRowCount", paramList)

                ' Read the max row count from the result set
                While (results.Read())
                    If (Not results.IsDBNull(results.GetOrdinal("MaxRow"))) Then
                        maxRows = results.GetInt32(results.GetOrdinal("MaxRow"))
                    End If
                End While
            Catch ex As Exception
                Throw ex
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try
            Logger.LogDebug("GetRowCountForWriter exit: # rows=" + maxRows.ToString, CLASSTYPE)
            Return maxRows
        End Function

        ''' <summary>
        ''' Read the POS Writer configuration data from the POSWriterFileConfig table for the
        ''' given writer, change type, and row combination.
        ''' Executes the POSGetWriterFileConfigurationForEdit stored procedure.
        ''' </summary>
        ''' <param name="currentWriter"></param>
        ''' <param name="currentChangeType"></param>
        ''' <param name="selectedRowNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetWriterFileConfigurationsByRowAndCol(ByRef currentWriter As POSWriterBO, ByRef currentChangeType As POSChangeTypeBO, ByVal selectedRowNum As Integer, Optional ByVal selectedColumnNum As Integer = 0) As DataSet
            Return GetWriterFileConfigurationsByRowAndCol(currentWriter.POSFileWriterKey, currentChangeType.POSChangeTypeKey, selectedRowNum, selectedColumnNum)
        End Function

        ''' <summary>
        ''' Read the POS Writer configuration data from the POSWriterFileConfig table for the
        ''' given writer, change type, and row combination.
        ''' Executes the POSGetWriterFileConfigurationForEdit stored procedure.
        ''' </summary>
        ''' <param name="fileWriterKey"></param>
        ''' <param name="changeTypeKey"></param>
        ''' <param name="selectedRowNum"></param>
        ''' <param name="selectedColumnNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetWriterFileConfigurationsByRowAndCol(ByVal fileWriterKey As Integer, ByVal changeTypeKey As Integer, ByVal selectedRowNum As Integer, Optional ByVal selectedColumnNum As Integer = 0) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "FileWriterKey"
            currentParam.Value = fileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ChangeTypeKey"
            currentParam.Value = changeTypeKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RowOrder"
            currentParam.Value = selectedRowNum
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ColumnOrder"
            If selectedColumnNum > 0 Then
                currentParam.Value = selectedColumnNum
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetPOSWriterFileConfigurationForEdit", paramList)
        End Function

        ''' <summary>
        ''' gets data from POSWriterEscapeChars for given writer type
        ''' </summary>
        ''' <param name="currentWriter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetWriterEscapeChars(ByRef currentWriter As POSWriterBO) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentWriter.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetPOSWriterEscapeChars", paramList)
        End Function

        ''' <summary>
        ''' gets data from POSWriterBatchIDs for the given writer type
        ''' </summary>
        ''' <param name="currentWriter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBatchIdDefaultsByWriterChangeType(ByRef currentWriter As POSWriterBO) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentWriter.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetDefaultBatchIdByChangeType", paramList)
        End Function

        ''' <summary>
        ''' gets data from POSWriterItemChgBatchId for the given writer type
        ''' </summary>
        ''' <param name="currentWriter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBatchIdDefaultsByItemChangeType(ByRef currentWriter As POSWriterBO) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentWriter.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetDefaultBatchIdByItemChgType", paramList)
        End Function

        ''' <summary>
        ''' gets data from POSWriterPriceChgBatchId for the given writer type
        ''' </summary>
        ''' <param name="currentWriter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBatchIdDefaultsByPriceChangeType(ByRef currentWriter As POSWriterBO) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentWriter.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetDefaultBatchIdByPriceChgType", paramList)
        End Function

        Public Function GetPOSFileWriterClasses(ByVal fileWriterType As String) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim writerClasses As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "FileWriterType"
                currentParam.Value = fileWriterType
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_POSPush_GetFileWriterClass", paramList)

                While results.Read
                    writerClasses.Add(results.GetString(results.GetOrdinal("FileWriterClass")))
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return writerClasses
        End Function

        Public Function GetFileWriterTypes(ByVal isConfigWriter As Boolean) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim writerTypes As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "IsConfigWriter"
                currentParam.Value = isConfigWriter
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_POSPush_GetFileWriterTypes", paramList)

                While results.Read
                    writerTypes.Add(results.GetString(results.GetOrdinal("FileWriterType")))
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return writerTypes
        End Function

        Public Function GetScaleWriterTypes() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetScaleWriterTypes")
        End Function

        Public Function GetAvailableFileWriterTypes(ByVal storeNo As Integer, ByVal isConfigWriter As Boolean) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim writerTypes As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IsConfigWriter"
                currentParam.Value = isConfigWriter
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_POSPush_GetAvailableFileWriterTypeForStore", paramList)

                While results.Read
                    writerTypes.Add(results.GetString(results.GetOrdinal("FileWriterType")))
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return writerTypes
        End Function

#End Region

#Region "Create, update methods"
        ''' <summary>
        ''' Insert a new record into the POSWriter table.
        ''' </summary>
        ''' <param name="currentWriter"></param>
        ''' <returns>new primary key value just inserted for new record</returns>
        ''' <remarks></remarks>
        Public Shared Function AddPOSWriterRecord(ByRef currentWriter As POSWriterBO) As Integer
            Logger.LogDebug("AddPOSWriterRecord entry", CLASSTYPE)
            Return InsertOrUpdateData(True, currentWriter)
            Logger.LogDebug("AddPOSWriterRecord exit", CLASSTYPE)
        End Function

        ''' <summary>
        ''' Update an existing record in the POSWriter table.
        ''' This method updates the properties of the writer - not the details for the output files.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdatePOSWriterRecord(ByRef currentWriter As POSWriterBO)
            Logger.LogDebug("UpdatePOSWriterRecord entry", CLASSTYPE)
            InsertOrUpdateData(False, currentWriter)
            Logger.LogDebug("UpdatePOSWriterRecord exit", CLASSTYPE)
        End Sub

         ''' <summary>
        ''' insert or update data based on passed in parameters
        ''' </summary>
        ''' <param name="isInsert"></param>
        ''' <param name="currentWriter"></param>
        ''' <remarks></remarks>
        Private Shared Function InsertOrUpdateData(ByVal isInsert As Boolean, ByRef currentWriter As POSWriterBO) As Integer
            Logger.LogDebug("InsertOrUpdateData entry", CLASSTYPE)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim newPOSFileWriterKey As Integer = -1

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            If isInsert = False Then
                currentParam = New DBParam
                currentParam.Name = "POSFileWriterKey"
                currentParam.Value = currentWriter.POSFileWriterKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
            End If

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterCode"
            currentParam.Value = currentWriter.POSFileWriterCode
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterClass"
            currentParam.Value = currentWriter.POSFileWriterClass
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ScaleWriterType"
            If currentWriter.ScaleWriterType Is Nothing Or currentWriter.ScaleWriterType = "" Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriter.ScaleWriterType
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "DelimChar"
            currentParam.Value = currentWriter.DelimChar
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OutputByIrmaBatches"
            currentParam.Value = currentWriter.OutputByIrmaBatches
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FixedWidth"
            currentParam.Value = currentWriter.FixedWidth
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LeadingDelim"
            currentParam.Value = currentWriter.LeadingDelimiter
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TrailingDelim"
            currentParam.Value = currentWriter.TrailingDelimiter
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FieldIdDelim"
            currentParam.Value = currentWriter.FieldIdDelim
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TaxFlagTrueChar"
            currentParam.Value = currentWriter.TaxFlagTrueChar
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TaxFlagFalseChar"
            currentParam.Value = currentWriter.TaxFlagFalseChar
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EnforceDictionary"
            currentParam.Value = currentWriter.EnforceDictionary
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AppendToFile"
            currentParam.Value = currentWriter.AppendToFile
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FileWriterType"
            currentParam.Value = currentWriter.WriterType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "BatchIdMin"
            If currentWriter.BatchIdMin Is Nothing Or String.Equals(currentWriter.BatchIdMin, "") Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriter.BatchIdMin
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "BatchIdMax"
            If currentWriter.BatchIdMax Is Nothing Or String.Equals(currentWriter.BatchIdMax, "") Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriter.BatchIdMax
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            If isInsert Then
                ' Execute the stored procedure to insert the new POSWriter record.
                Dim reader As SqlDataReader = factory.GetStoredProcedureDataReader("Administration_POSPush_InsertPOSWriter", paramList)

                Try
                    While reader.Read
                        'get the POSFileWriterKey value assigned to this new POSWriter item
                        newPOSFileWriterKey = CType(reader.GetDecimal(reader.GetOrdinal("POSFileWriterKey")), Integer)
                    End While
                Catch ex As Exception
                    Throw ex
                Finally
                    If reader IsNot Nothing Then
                        'close reader and DB connection
                        reader.Close()
                    End If
                End Try

                Return newPOSFileWriterKey
            Else
                ' Execute the stored procedure to update the new UpdatePOSWriter record.
                factory.ExecuteStoredProcedure("Administration_POSPush_UpdatePOSWriter", paramList)
            End If

            Logger.LogDebug("InsertOrUpdateData exit", CLASSTYPE)
        End Function

        ''' <summary>
        ''' Reorders two records in the POSWriterFileConfig table.
        ''' </summary>
        ''' <param name="currentWriterFileConfig"></param>
        ''' <param name="currentChangeType"></param>
        ''' <param name="moveUp"></param>
        ''' <remarks></remarks>
        Public Shared Sub ReorderPOSWriterFileConfigRecords(ByRef currentWriterFileConfig As POSWriterFileConfigBO, ByRef currentChangeType As POSChangeTypeBO, ByVal moveUp As Boolean)
            Logger.LogDebug("ReorderPOSWriterFileConfigRecords entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentWriterFileConfig.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSChangeTypeKey"
            currentParam.Value = currentChangeType.POSChangeTypeKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RowOrder"
            currentParam.Value = currentWriterFileConfig.RowOrder
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ColumnOrder"
            currentParam.Value = currentWriterFileConfig.ColumnOrder
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MoveUp"
            currentParam.Value = moveUp
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to reorder the records.
            factory.ExecuteStoredProcedure("Administration_POSPush_UpdatePOSWriterFileConfigOrder", paramList)
            Logger.LogDebug("ReorderPOSWriterFileConfigRecords exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Inserts a new record in the POSWriterFileConfig table.
        ''' This method updates the details for a single column in the output file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub AddPOSWriterFileConfigRecord(ByRef currentWriterFileConfig As POSWriterFileConfigBO)
            Logger.LogDebug("AddPOSWriterFileConfigRecord entry", CLASSTYPE)
            InsertOrUpdateData(True, currentWriterFileConfig)
            Logger.LogDebug("AddPOSWriterFileConfigRecord exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Inserts 8 new records for a bit column in the POSWriterFileConfig table.
        ''' This method updates the details for a single column comprised of 8 bits in the output file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub AddPOSWriterFileConfigRecord(ByRef currentWriterFileConfigArray() As POSWriterFileConfigBO)
            Logger.LogDebug("AddPOSWriterFileConfigRecord entry", CLASSTYPE)
            Dim index As Short
            For index = 0 To 7
                InsertOrUpdateData(True, currentWriterFileConfigArray(index))
            Next
            Logger.LogDebug("AddPOSWriterFileConfigRecord exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Update an existing record in the UpdatePOSWriterFileConfig table.
        ''' This method updates the details for a single column in the output file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdatePOSWriterFileConfigRecord(ByRef currentWriterFileConfig As POSWriterFileConfigBO)
            Logger.LogDebug("UpdatePOSWriterFileConfigRecord entry", CLASSTYPE)
            InsertOrUpdateData(False, currentWriterFileConfig)
            Logger.LogDebug("UpdatePOSWriterFileConfigRecord exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Update all 8 existing records for a bit column in the UpdatePOSWriterFileConfig table.
        ''' This method updates the details for a single column comprised of 8 bits in the output file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdatePOSWriterFileConfigRecord(ByRef currentWriterFileConfigArray() As POSWriterFileConfigBO)
            Logger.LogDebug("UpdatePOSWriterFileConfigRecord entry", CLASSTYPE)
            Dim index As Short
            For index = 0 To 7
                InsertOrUpdateData(False, currentWriterFileConfigArray(index))
            Next
            Logger.LogDebug("UpdatePOSWriterFileConfigRecord exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' performs insert or update using data passed in
        ''' </summary>
        ''' <param name="isInsert"></param>
        ''' <param name="currentWriterFileConfig"></param>
        ''' <remarks></remarks>
        Private Shared Sub InsertOrUpdateData(ByVal isInsert As Boolean, ByRef currentWriterFileConfig As POSWriterFileConfigBO)
            Logger.LogDebug("InsertOrUpdateData entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentWriterFileConfig.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSChangeTypeKey"
            currentParam.Value = currentWriterFileConfig.POSChangeTypeKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RowOrder"
            currentParam.Value = currentWriterFileConfig.RowOrder
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ColumnOrder"
            currentParam.Value = currentWriterFileConfig.ColumnOrder
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "BitOrder"
            currentParam.Value = currentWriterFileConfig.BitOrder
            currentParam.Type = DBParamType.SmallInt
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "DataElement"
            currentParam.Value = currentWriterFileConfig.DataElement
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FieldID"
            currentParam.Value = currentWriterFileConfig.FieldId
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MaxFieldWidth"
            If currentWriterFileConfig.MaxFieldWidth Is Nothing _
                Or (currentWriterFileConfig.MaxFieldWidth IsNot Nothing AndAlso currentWriterFileConfig.MaxFieldWidth.Trim().Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriterFileConfig.MaxFieldWidth
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TruncLeft"
            currentParam.Value = currentWriterFileConfig.TruncLeft
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "DefaultValue"
            If currentWriterFileConfig.DefaultValue Is Nothing _
                Or (currentWriterFileConfig.DefaultValue IsNot Nothing AndAlso currentWriterFileConfig.DefaultValue.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriterFileConfig.DefaultValue
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsTaxFlag"
            currentParam.Value = currentWriterFileConfig.IsTaxFlag
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsLiteral"
            currentParam.Value = currentWriterFileConfig.IsLiteral
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsDecimalValue"
            currentParam.Value = currentWriterFileConfig.IsDecimalValue
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsPackedDecimal"
            currentParam.Value = currentWriterFileConfig.IsPackedDecimal
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsBinaryInt"
            currentParam.Value = currentWriterFileConfig.IsBinaryInt
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "DecimalPrecision"
            If currentWriterFileConfig.DecimalPrecision Is Nothing _
                Or (currentWriterFileConfig.DecimalPrecision IsNot Nothing AndAlso currentWriterFileConfig.DecimalPrecision.Trim().Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriterFileConfig.DecimalPrecision
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IncludeDecimal"
            currentParam.Value = currentWriterFileConfig.IncludeDecimal
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PadLeft"
            currentParam.Value = currentWriterFileConfig.PadLeft
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FillChar"
            If currentWriterFileConfig.FillChar Is Nothing _
                Or (currentWriterFileConfig.FillChar IsNot Nothing AndAlso currentWriterFileConfig.FillChar.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriterFileConfig.FillChar
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LeadingChars"
            If currentWriterFileConfig.LeadingChars Is Nothing _
                Or (currentWriterFileConfig.LeadingChars IsNot Nothing AndAlso currentWriterFileConfig.LeadingChars.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriterFileConfig.LeadingChars
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TrailingChars"
            If currentWriterFileConfig.TrailingChars Is Nothing _
                Or (currentWriterFileConfig.TrailingChars IsNot Nothing AndAlso currentWriterFileConfig.TrailingChars.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriterFileConfig.TrailingChars
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsBoolean"
            currentParam.Value = currentWriterFileConfig.IsBoolean
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "BooleanTrueChar"
            If currentWriterFileConfig.BooleanTrueChar Is Nothing _
                Or (currentWriterFileConfig.BooleanTrueChar IsNot Nothing AndAlso currentWriterFileConfig.BooleanTrueChar.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriterFileConfig.BooleanTrueChar
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "BooleanFalseChar"
            If currentWriterFileConfig.BooleanFalseChar Is Nothing _
                Or (currentWriterFileConfig.BooleanFalseChar IsNot Nothing AndAlso currentWriterFileConfig.BooleanFalseChar.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriterFileConfig.BooleanFalseChar
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FixedWidthField"
            currentParam.Value = currentWriterFileConfig.FixedWidthField
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PackLength"
            If currentWriterFileConfig.PackLength Is Nothing _
                Or (currentWriterFileConfig.PackLength IsNot Nothing AndAlso currentWriterFileConfig.PackLength.Trim().Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentWriterFileConfig.PackLength
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            If isInsert Then
                ' Execute the stored procedure to insert the new POSWriterFileConfig record.
                factory.ExecuteStoredProcedure("Administration_POSPush_InsertPOSWriterFileConfig", paramList)
            Else
                ' Execute the stored procedure to update the new UpdatePOSWriterFileConfig record.
                factory.ExecuteStoredProcedure("Administration_POSPush_UpdatePOSWriterFileConfig", paramList)
            End If

            Logger.LogDebug("InsertOrUpdateData exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Save changes to the DataSet to the database (inserts, updates, deletes)
        ''' </summary>
        ''' <param name="dataSet"></param>
        ''' <remarks></remarks>
        Public Sub SaveEscapeCharData(ByRef dataSet As DataSet, ByVal currentWriter As POSWriterBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentWriter.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.UpdateDataSet(dataSet, "Administration_POSPush_GetPOSWriterEscapeChars", True, paramList)
        End Sub

        Public Sub SaveBatchIdDefaultsByWriterChangeType(ByRef currentBatchId As BatchIdDefaultBO)
            Logger.LogDebug("SaveBatchIdDefaultsByWriterChangeType entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentBatchId.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSChangeTypeKey"
            currentParam.Value = currentBatchId.ChangeTypeId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSBatchIdDefault"
            If currentBatchId.BatchIdDefault Is Nothing Or currentBatchId.BatchIdDefault.Equals("") Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentBatchId.BatchIdDefault
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Administration_POSPush_UpdateDefaultBatchIdByChangeType", paramList)
            Logger.LogDebug("SaveBatchIdDefaultsByWriterChangeType exit", CLASSTYPE)
        End Sub

        Public Sub SaveBatchIdDefaultsByItemChangeType(ByRef currentBatchId As BatchIdDefaultBO)
            Logger.LogDebug("SaveBatchIdDefaultsByItemChangeType entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentBatchId.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemChgTypeID"
            currentParam.Value = currentBatchId.ChangeTypeId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSBatchIdDefault"
            If currentBatchId.BatchIdDefault Is Nothing Or currentBatchId.BatchIdDefault.Equals("") Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentBatchId.BatchIdDefault
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Administration_POSPush_UpdateDefaultBatchIdByItemChgType", paramList)
            Logger.LogDebug("SaveBatchIdDefaultsByItemChangeType exit", CLASSTYPE)
        End Sub

        Public Sub SaveBatchIdDefaultsByPriceChangeType(ByRef currentBatchId As BatchIdDefaultBO)
            Logger.LogDebug("SaveBatchIdDefaultsByPriceChangeType entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentBatchId.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeID"
            currentParam.Value = currentBatchId.ChangeTypeId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSBatchIdDefault"
            If currentBatchId.BatchIdDefault Is Nothing Or currentBatchId.BatchIdDefault.Equals("") Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentBatchId.BatchIdDefault
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Administration_POSPush_UpdateDefaultBatchIdByPriceChgType", paramList)
            Logger.LogDebug("SaveBatchIdDefaultsByPriceChangeType exit", CLASSTYPE)
        End Sub
#End Region

#Region "Delete methods"
        ''' <summary>
        ''' Disable the POSWriter record and delete the associated StorePOSConfig records.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeletePOSWriter(ByRef currentWriter As POSWriterBO)
            Logger.LogDebug("DeletePOSWriter entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentWriter.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to disable the POSWriter and delete the associated StorePOSConfig 
            ' records.
            ' The POSWriterFileConfig records are not deleted, but the POSWriter must be re-enabled to access
            ' them.
            factory.ExecuteStoredProcedure("Administration_POSPush_DeletePOSWriter", paramList)
            Logger.LogDebug("DeletePOSWriter exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Delete a record in the POSWriterFileConfig table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeletePOSWriterFileConfig(ByRef currentWriterFileConfig As POSWriterFileConfigBO)
            Logger.LogDebug("DeletePOSWriterFileConfig entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentWriterFileConfig.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSChangeTypeKey"
            currentParam.Value = currentWriterFileConfig.POSChangeTypeKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RowOrder"
            currentParam.Value = currentWriterFileConfig.RowOrder
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ColumnOrder"
            currentParam.Value = currentWriterFileConfig.ColumnOrder
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to delete the POSWriterFileConfig record.
            factory.ExecuteStoredProcedure("Administration_POSPush_DeletePOSWriterFileConfig", paramList)
            Logger.LogDebug("DeletePOSWriterFileConfig exit", CLASSTYPE)
        End Sub

        Public Shared Sub DeletePOSWriterFileConfigRow(ByRef currentWriterFileConfig As POSWriterFileConfigBO)
            Logger.LogDebug("DeletePOSWriterFileConfigRow entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentWriterFileConfig.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSChangeTypeKey"
            currentParam.Value = currentWriterFileConfig.POSChangeTypeKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RowOrder"
            currentParam.Value = currentWriterFileConfig.RowOrder
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to delete the POSWriterFileConfig record.
            factory.ExecuteStoredProcedure("Administration_POSPush_DeletePOSWriterFileConfigRow", paramList)
            Logger.LogDebug("DeletePOSWriterFileConfigRow exit", CLASSTYPE)
        End Sub
#End Region

    End Class
End Namespace
