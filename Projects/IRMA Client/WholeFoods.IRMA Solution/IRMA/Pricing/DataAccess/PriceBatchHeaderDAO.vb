Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Pricing.DataAccess

    Public Class PriceBatchHeaderDAO

        ''' <summary>
        ''' The Admin application provides the ability to define default batch ids to be set when a batch
        ''' is created.  This stored procedure will query the database to determine which default batch id,
        ''' if any, should be set.
        ''' </summary>
        ''' <param name="header"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDefaultPOSBatchID(ByVal header As PriceBatchHeaderBO) As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim defaultBatchId As String = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = header.StoreNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemChgTypeID"
                If header.ItemChgTypeID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.ItemChgTypeID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PriceChgTypeID"
                If header.PriceChgTypeID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.PriceChgTypeID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "POSBatchId"
                If header.POSBatchId = Nothing Or String.Equals(header.POSBatchId, "") Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.POSBatchId
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                reader = factory.GetStoredProcedureDataReader("GetDefaultPOSBatchId", paramList)

                While reader.Read
                    If reader.GetValue(reader.GetOrdinal("DefaultBatchId")).GetType IsNot GetType(DBNull) Then
                        defaultBatchId = reader.GetInt32(reader.GetOrdinal("DefaultBatchId")).ToString
                    End If
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try

            Return defaultBatchId
        End Function

        ''' <summary>
        ''' The Admin application provides the ability to define default batch ids to be set when a batch
        ''' is created.  This stored procedure will query the database to determine which default batch id,
        ''' if any, should be set.
        ''' </summary>
        ''' <param name="storeNo"></param>
        ''' <returns>Integer Array:  {0} = min batch id, if defined  {1} = max batch id, if defined</returns>
        ''' <remarks></remarks>
        Public Shared Function GetDefaultPOSBatchIdRangeByStore(ByVal storeNo As Integer) As Integer()
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim batchIdDefaults(2) As Integer

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                reader = factory.GetStoredProcedureDataReader("GetDefaultPOSBatchIdRangeByStore", paramList)

                While reader.Read
                    If reader.GetValue(reader.GetOrdinal("BatchIdMin")).GetType IsNot GetType(DBNull) Then
                        batchIdDefaults(0) = reader.GetInt32(reader.GetOrdinal("BatchIdMin"))
                    Else
                        batchIdDefaults(0) = -1
                    End If
                    If reader.GetValue(reader.GetOrdinal("BatchIdMax")).GetType IsNot GetType(DBNull) Then
                        batchIdDefaults(1) = reader.GetInt32(reader.GetOrdinal("BatchIdMax"))
                    Else
                        batchIdDefaults(1) = -1
                    End If
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try

            Return batchIdDefaults
        End Function

        Public Function InsertPriceBatchHeader(ByVal header As PriceBatchHeaderBO, ByRef transaction As SqlTransaction) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim intPriceBatchHeaderID As Integer

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemChgTypeID"
                If header.ItemChgTypeID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.ItemChgTypeID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PriceChgTypeID"
                If header.PriceChgTypeID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.PriceChgTypeID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StartDate"
                currentParam.Value = header.StartDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "BatchDescription"
                If (header.BatchDescription Is Nothing) Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.BatchDescription
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "AutoApplyFlag"
                currentParam.Value = header.AutoApplyFlag
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ApplyDate"
                If header.AutoApplyDate = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.AutoApplyDate
                End If
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "POSBatchId"
                If header.POSBatchId = Nothing Or String.Equals(header.POSBatchId, "") Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.POSBatchId
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                reader = factory.GetStoredProcedureDataReader("InsertPriceBatchHeader", paramList, transaction)

                While reader.Read
                    intPriceBatchHeaderID = reader.GetInt32(reader.GetOrdinal("PriceBatchHeaderID"))
                End While
            Catch ex As Exception
                'TODO handle exception
                Throw ex
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try

            Return intPriceBatchHeaderID
        End Function

        Public Function GetPriceBatchDetailIDs(ByVal header As PriceBatchHeaderBO, ByVal itemKeyArray() As Integer) As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim priceBatchDetailIDList As String
            Dim itemKeyList As String
            Dim i As Integer
            priceBatchDetailIDList = String.Empty
            itemKeyList = String.Empty

            Try
                ' create a list of item keys from array
                For i = 0 To itemKeyArray.Length - 1
                    itemKeyList = itemKeyList & "|" & itemKeyArray(i)
                Next i

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "@ItemKeyList"
                currentParam.Value = itemKeyList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "@ItemKeyListSep"
                currentParam.Value = "|"
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetPriceBatchDetailIDs", paramList)

                'create the priceBatchDetailIDList by appending all these together with the "|"
                While results.Read
                    priceBatchDetailIDList = priceBatchDetailIDList & "|" & results.GetInt32(results.GetOrdinal("PriceBatchDetailID")).ToString()
                End While

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return priceBatchDetailIDList

        End Function

        Public Sub UpdatePriceBatchDetails(ByVal header As PriceBatchHeaderBO, ByRef transaction As SqlTransaction)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim newDetailList As StringBuilder = Nothing
            Dim offerIdList As StringBuilder = Nothing

            Try
                'if change type = Offer then update all details for the given Offer_ID w/ the new PriceBatchHeaderID
                '(only 1 PBD record is displayed to the user to represent the entire offer so the user does not have to
                ' explicitly batch all items within an offer definition.  They are batching the entire offer as a whole.)
                If header.ItemChgTypeID = 4 Then
                    Dim sql As StringBuilder
                    Dim offerList() As String = Split(header.PriceBatchDetailIDList, header.DetailIDListSeparator)
                    Dim offerListEnum As IEnumerator = offerList.GetEnumerator
                    Dim currentOfferDetailID As String

                    newDetailList = New StringBuilder

                    'if more than 1 offer being batched together then expand PBD list for each offer in the list
                    'separately and then combine the results into 1 master delimeted list of PBD IDs
                    While offerListEnum.MoveNext
                        currentOfferDetailID = CType(offerListEnum.Current, String)

                        sql = New StringBuilder
                        sql.Append("SELECT dbo.fn_UpdatePriceBatchDetailOffer('")
                        sql.Append(currentOfferDetailID)
                        sql.Append("','")
                        sql.Append(header.DetailIDListSeparator)
                        sql.Append("')")

                        newDetailList.Append(CType(factory.ExecuteScalar(sql.ToString), String))
                        newDetailList.Append(header.DetailIDListSeparator)
                    End While

                    'strip off last list separator
                    newDetailList.Remove(newDetailList.Length - 1, 1)
                End If




                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "DetailIDList"
                If newDetailList IsNot Nothing Then
                    currentParam.Value = newDetailList.ToString
                Else
                    currentParam.Value = header.PriceBatchDetailIDList
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DetailIDListSep"
                currentParam.Value = header.DetailIDListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PriceBatchHeaderID"
                currentParam.Value = header.PriceBatchHeaderId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("UpdatePriceBatchDetailHeader", paramList, transaction)
            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' updates PriceBatchHeader.PriceBatchStatusID w/ passed in status value.
        ''' returns new status description (PriceBatchStatus.PriceBatchStatusDesc) to calling form
        ''' </summary>
        ''' <param name="header"></param>
        ''' <returns>PriceBatchStatus.PriceBatchStatusDesc of new status</returns>
        ''' <remarks></remarks>
        Public Function UpdatePriceBatchStatus(ByVal header As PriceBatchHeaderBO) As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim newStatusDesc As String = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "PriceBatchHeaderID"
                currentParam.Value = header.PriceBatchHeaderId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PriceBatchStatusID"
                currentParam.Value = header.PriceBatchStatusID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("UpdatePriceBatchStatus", paramList)

                While results.Read
                    newStatusDesc = results.GetString(results.GetOrdinal("PriceBatchStatusDesc"))
                End While

                Return newStatusDesc
            Catch ex As Exception
                'TODO handle exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
        End Function

        ''' <summary>
        ''' updates PriceBatchHeader.PriceBatchStatusID as "packaged".
        ''' returns new status description (PriceBatchStatus.PriceBatchStatusDesc) to calling form
        ''' </summary>
        ''' <param name="header"></param>
        ''' <returns>PriceBatchStatus.PriceBatchStatusDesc of new status</returns>
        ''' <remarks></remarks>
        Public Function UpdatePriceBatchPackage(ByVal header As PriceBatchHeaderBO) As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim newStatusDesc As String = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "PriceBatchHeaderID"
                currentParam.Value = header.PriceBatchHeaderId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("UpdatePriceBatchPackage", paramList)

                While results.Read
                    newStatusDesc = results.GetString(results.GetOrdinal("PriceBatchStatusDesc"))
                End While

                Return newStatusDesc
            Catch ex As Exception
                'TODO handle exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
        End Function

        Public Sub InsertMammothItemLocaleEvents(ByVal header As PriceBatchHeaderBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim newStatusDesc As String = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "PriceBatchHeaderID"
                currentParam.Value = header.PriceBatchHeaderId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = header.StoreNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("mammoth.InsertItemLocaleChangeQueueByBatchHeaderAndStore", paramList)

            Catch ex As Exception
                Throw
            End Try
        End Sub

    End Class

End Namespace
