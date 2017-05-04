Imports System.Collections
Imports System.Data.SqlClient
Imports System.Text
Imports log4net
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.TagPush.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.TagPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.TagPush.BusinessLogic
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.TagPush.Controller

    ''' <summary>
    ''' Subclasses POSProcessor, providing the details for the Promotional Offer ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TagPriceBatchProcessor
        Inherits TagProcessor
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        Public Property ErrorException As Exception = Nothing

        Dim storeFTPBo As StoreFTPConfigBO = Nothing

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="batchId"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal batchId As Integer)
            MyBase.New(batchId)
        End Sub

        ''' <summary>
        ''' Reads the database records from IRMA, adding the results to the POS Push file for each store.
        ''' Executes the Replenishment_POSPush_GetPriceBatchOffers stored procedure to retrieve the change records.
        ''' 
        ''' </summary>
        ''' <param name="batchId"></param>
        ''' <remarks></remarks>
        ''' <exception cref="WholeFoods.Utility.DataAccess.DataFactory" />
        Overrides Sub GetPriceBatchTagDataFromIRMA(ByVal itemList As String, ByVal itemListSeparator As Char, ByVal batchId As Integer, ByVal startLabelPosition As Integer, ByVal store_no As Integer)
            logger.Debug("GetPriceBatchTagDataFromIRMA entry: store_No=" + store_no.ToString + ", batchId=" + batchId.ToString)
            Dim results As SqlDataReader = Nothing
            Dim currentStoreNum As Integer
            Dim itemKey As Integer
            Dim PBD_ID As Integer
            Dim currentStoreUpdate As StoreUpdatesBO
            Dim currentStoreName As String = Nothing
            Dim currentTagExt As String = Nothing
            Dim prev_tagType As Integer = 0
            Dim prev_tagType2 As Integer = 0
            Dim tagType As Integer = 0
            Dim tagType2 As Integer = 0
            Dim tagExt As String = Nothing
            Dim exemptTagExt As String = Nothing
            Dim prevSBTNum As Integer = 0
            Dim subTeamNum As Integer = 0
            Dim RowNumber As Integer = 0
            Dim tagBO As ShelfTagBO = Nothing
            Dim currentTagWriter As TagWriter = Nothing   ' AB: this should be type TagWriter, rename POS to TAG
            Dim exemptTagWriter As TagWriter = Nothing    ' AB: this should be type TagWriter
            Dim tagFileList As ArrayList = New ArrayList
            Dim printExemptTags As Boolean = False
            Dim PosFileWriterKey As Integer
            Dim suffixType As String

            Try
                ' Initialize the storeUpdates hashtable, adding an entry for each store
                storeUpdates = StorePOSConfigDAO.GetShelfTagStoreConfigurations(Constants.FileWriterType_TAG, CType(store_no, String))

                'set filewriter key and pass it in for new query branching
                If (storeUpdates.ContainsKey(store_no)) Then
                    currentStoreUpdate = CType(storeUpdates.Item(store_no), StoreUpdatesBO)
                    PosFileWriterKey = CInt(currentStoreUpdate.FileWriter.POSFileWriterKey.ToString)
                Else
                    PosFileWriterKey = 0
                End If

                ' Read the price batch changes that are ready to be sent to the stores.
                logger.Info("GetPriceBatchTagDataFromIRMA - reading the TAG data from the database: batchID=" + batchId.ToString + ", startLabelPosition=" + startLabelPosition.ToString + ", PosFileWriterKey=" + PosFileWriterKey.ToString + ", itemList=" + itemList)
                results = TagWriterDAO.GetPriceBatchTagData(itemList, itemListSeparator, batchId, startLabelPosition, PosFileWriterKey)

                ' Read instance data flags
                printExemptTags = InstanceDataDAO.IsFlagActive("ExemptShelfTags")

                ' -- First resultset - list of PriceBatchHeader records being updated
                logger.Debug("processing first result set - batch headers")

                While results.Read()

                    ' get the store # for this record
                    currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))
                    subTeamNum = results.GetInt32(results.GetOrdinal("SubTeam_No"))

                    ' verify this store is configured in StorePOSConfig
                    If (storeUpdates.ContainsKey(currentStoreNum)) Then

                        currentStoreUpdate = CType(storeUpdates.Item(currentStoreNum), StoreUpdatesBO)
                        tagType = results.GetInt32(results.GetOrdinal("ShelfTagTypeID"))

                        If (Not results.IsDBNull(results.GetOrdinal("ExemptTagTypeID"))) Then
                            tagType2 = results.GetInt32(results.GetOrdinal("ExemptTagTypeID"))
                        Else
                            tagType2 = 0
                        End If

                        itemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
                        PBD_ID = results.GetInt32(results.GetOrdinal("PriceBatchDetailID"))

                        logger.Info("GetPriceBatchTagDataFromIRMA - Processing Store_No=" + currentStoreNum.ToString + ", subTeamNum=" + subTeamNum.ToString + ", tagType=" + tagType.ToString + ", itemKey=" + itemKey.ToString + ", PBD_ID=" + PBD_ID.ToString)
                        If (prev_tagType <> 0) AndAlso (tagType <> prev_tagType) Then
                            If currentTagWriter IsNot Nothing Then
                                currentTagWriter.CloseTempFile()
                            End If
                            prevSBTNum = 0
                        End If

                        If (prev_tagType2 <> 0) AndAlso (tagType2 <> prev_tagType2) Then
                            If exemptTagWriter IsNot Nothing Then
                                exemptTagWriter.CloseTempFile()
                            End If
                        End If
                        If (tagType = tagType2) Then
                            tagType2 = 0
                        End If

                        If (tagType <> prev_tagType) Then
                            currentStoreName = results.GetString(results.GetOrdinal("Store_Name"))
                            tagExt = results.GetString(results.GetOrdinal("tagExt"))
                            ' Set the Exempt Tag Writer and its file name
                            If (Not results.IsDBNull(results.GetOrdinal("ExemptTagExt"))) Then
                                exemptTagExt = results.GetString(results.GetOrdinal("ExemptTagExt"))
                            End If
                            If tagType <> tagType2 Then
                                ' get the writer instance for the store
                                currentTagWriter = CType(currentStoreUpdate.FileWriter, TagWriter)
                                currentStoreUpdate.StoreName = currentStoreName
                                'DaveStacey - 20080120 - Fix for MA's "P" vs. NA's "B" Filename Configuration
                                'This needs to be turned into a database lookup instead of a code-side switch
                                If currentStoreUpdate.FileWriter.ToString.Contains("PrintLab") Then
                                    suffixType = "B"
                                Else
                                    suffixType = "P"
                                End If
                                currentStoreUpdate.SetShelfTagTempFilenames(tagExt, suffixType, batchId.ToString)
                                currentStoreUpdate.SetExemptTagTempFilenames(exemptTagExt, suffixType, batchId.ToString)
                                currentStoreUpdate.ShelfTagFiles.Add(currentStoreUpdate.BatchFileName)
                            End If
                        End If

                        If printExemptTags Then
                            If (tagType2 > 0 AndAlso tagType2 <> prev_tagType2) Then
                                currentStoreUpdate.ExemptFileWriter = currentStoreUpdate.FileWriter.Copy()
                                exemptTagWriter = CType(currentStoreUpdate.ExemptFileWriter, TagWriter)
                                exemptTagWriter.ExemptTagFile = True
                                currentStoreUpdate.StoreName = currentStoreName
                                'call to currentStoreUpdate.SetExemptTagTempFilenames(exemptTagExt, "B") is now included above
                                'where currentStoreUpdate.SetShelfTagTempFilenames() is called.  This is done to make sure
                                'the exempt tag filenames are set for regions that do not use them.  This is necessary because,
                                'if not set here, the FTP routine will check to see if it exists and cause creation of them
                                'in a way that prevents moving of the local shelf tag files to the archive directory and, as a
                                'result, the tag files keep getting appended to.
                                currentStoreUpdate.ExemptShelfTagFiles.Add(currentStoreUpdate.ExemptTagFileName)
                                If subTeamNum = prevSBTNum Then
                                    If (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                                        exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                                        exemptTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.ExemptTagFileName, results, True)
                                        exemptTagWriter.CloseTempFile()
                                    End If
                                End If
                            End If
                        End If

                        If subTeamNum <> prevSBTNum Then
                            If currentTagWriter IsNot Nothing AndAlso tagType <> tagType2 Then
                                currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                                currentTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.BatchFileName, results, False)
                                currentTagWriter.CloseTempFile()
                            End If
                            If printExemptTags AndAlso (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                                exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                                exemptTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.ExemptTagFileName, results, True)
                                exemptTagWriter.CloseTempFile()
                            End If
                        End If
                        If printExemptTags AndAlso (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                            exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                            WriteExemptTagResultsToFile(currentStoreUpdate, results, ChangeType.shelfTagChange)
                            exemptTagWriter.CloseTempFile()
                        End If
                        If currentTagWriter IsNot Nothing AndAlso tagType <> tagType2 Then
                            currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                            WriteResultsToFile(currentStoreUpdate, results, ChangeType.shelfTagChange)
                            currentTagWriter.CloseTempFile()
                        End If
                        If (Not storeTagFTPs.ContainsKey(currentStoreUpdate.StoreNum)) Then
                            storeTagFTPs.Add(currentStoreUpdate.StoreNum, currentStoreUpdate)
                        End If

                        tagBO = New ShelfTagBO(PBD_ID, itemKey, tagType, tagType2)


                        'If IncludeAllItemIdentifiersInShelfTagPush is TRUE (an MA requirement), 
                        'we must append the RowNumber column to the Item key to make a unique
                        'key for the hashtable.

                        'Matt Potok (FL) - Removing this If statement because there is an error in this logic and we always want to execute this
                        'logic to keep the keys unique.  See Bug 4963
                        itemTagBOs.Add(tagBO.ItemKey.ToString() + ":" + RowNumber.ToString(), tagBO)
                        RowNumber = RowNumber + 1

                        If tagType2 > 0 Then
                            prev_tagType2 = tagType2
                        End If
                        prev_tagType = tagType
                        prevSBTNum = subTeamNum
                    Else
                        ' ERROR PROCESSING ... all stores should be found in the hash
                        logger.Warn("Error processing a record returned by Replenishment_TagPush_GetBatchTagFile.  Store # not configured in StorePOSConfig table: " & currentStoreNum.ToString)

                        'send message about exception
                        Dim args(1) As String
                        args(0) = currentStoreNum.ToString
                        ErrorHandler.ProcessError(ErrorType.POSPush_StoreNotFound, args, SeverityLevel.Warning)
                        Continue While
                    End If

                End While

                logger.Debug("done processing all results")

                If storeTagFTPs.Count > 0 Then
                    Dim transfer As New TransferWriterFiles
                    Dim transferSuccess As Boolean = transfer.TransferStoreFiles(storeTagFTPs)

                    If transferSuccess Then
                        'Update Price Batch Detail with the Item Tag Info
                        logger.Info("GetPriceBatchTagDataFromIRMA - updating PriceBatchDetail with item tag info: itemTagBOs count=" + itemTagBOs.Count.ToString)
                        TagWriterDAO.UpdatePriceBatchDetailWithTagID(itemTagBOs)
                    Else
                        ' update the error message so the user knows what happened
                        logger.Info("Transfer of Tag Writer files did not succeed.  Updates were not applied in IRMA.")
                    End If
                End If

            Catch e As Exception
                'common exceptions would be DataFactory or File I/O exceptions
                logger.Error("GetPriceBatchTagDataFromIRMA - error during processing; the exception is caught & just thrown again", e)
                Throw e
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetPriceBatchTagDataFromIRMA exit")
        End Sub

        ''' <summary>
        ''' Reads the database records from IRMA for the type of change being handled by the subclass, 
        ''' adding the results to the POS Push file for each store.
        ''' </summary>
        ''' <param name="itemList"></param>
        ''' <param name="itemListSeparator"></param>
        ''' <param name="startLabelPosition"></param>
        ''' <param name="store_no"></param>
        ''' <remarks></remarks>
        Public Overrides Sub GetReprintTagDataFromIRMA(ByVal itemList As String, ByVal itemListSeparator As Char, ByVal startLabelPosition As Integer, ByVal store_no As Integer, ByVal blnSortTags As Boolean)
            logger.Debug("GetReprintTagDataFromIRMA entry: store_No=" + store_no.ToString)
            Dim results As SqlDataReader = Nothing
            Dim currentStoreNum As Integer
            Dim currentStoreUpdate As StoreUpdatesBO
            Dim currentStoreName As String = Nothing
            Dim currentTagExt As String = Nothing
            Dim exemptTagExt As String = Nothing
            Dim prev_tagType As Integer = 0
            Dim prev_tagType2 As Integer = 0
            Dim tagType As Integer = 0
            Dim tagType2 As Integer = 0
            Dim tagExt As String = Nothing
            Dim prevSBTNum As Integer = 0
            Dim subTeamNum As Integer = 0
            Dim RowNumber As Integer = 0
            Dim currentTagWriter As TagWriter = Nothing
            Dim exemptTagWriter As TagWriter = Nothing
            Dim itemKey As Integer
            Dim PBD_ID As Integer
            Dim tagBO As ShelfTagBO = Nothing
            Dim printExemptTags As Boolean = False
            Dim PosFileWriterKey As Integer

            Try
                ' Initialize the storeUpdates hashtable, adding an entry for each store
                storeUpdates = StorePOSConfigDAO.GetShelfTagStoreConfigurations(Constants.FileWriterType_TAG, CType(store_no, String))

                If (storeUpdates.ContainsKey(store_no)) Then
                    currentStoreUpdate = CType(storeUpdates.Item(store_no), StoreUpdatesBO)
                    PosFileWriterKey = CInt(currentStoreUpdate.FileWriter.POSFileWriterKey.ToString)
                Else
                    PosFileWriterKey = 0
                End If
                ' Read the price batch changes that are ready to be sent to the stores.
                logger.Info("GetReprintTagDataFromIRMA - reading the TAG data from the database: store_no=" + store_no.ToString + ", startLabelPosition=" + startLabelPosition.ToString + ", PosFileWriterKey=" + PosFileWriterKey.ToString + ", itemList=" + itemList)
                results = TagWriterDAO.GetReprintTagData(itemList, store_no, itemListSeparator, startLabelPosition, PosFileWriterKey, blnSortTags)

                ' Read instance data flags
                printExemptTags = InstanceDataDAO.IsFlagActive("ExemptShelfTags")

                ' -- First resultset - list of PriceBatchHeader records being updated
                logger.Debug("processing first result set - batch headers")

                While results.Read()
                    ' get the store # for this record
                    currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))
                    subTeamNum = results.GetInt32(results.GetOrdinal("SubTeam_No"))

                    ' verify this store is configured in StorePOSConfig
                    If (storeUpdates.ContainsKey(currentStoreNum)) Then
                        currentStoreUpdate = CType(storeUpdates.Item(currentStoreNum), StoreUpdatesBO)
                        'currentTagWriter = CType(currentStoreUpdate.FileWriter, PrintLab_Writer)

                        tagType = results.GetInt32(results.GetOrdinal("ShelfTagTypeID"))

                        If (Not results.IsDBNull(results.GetOrdinal("ExemptTagTypeID"))) Then
                            tagType2 = results.GetInt32(results.GetOrdinal("ExemptTagTypeID"))
                        Else
                            tagType2 = 0
                        End If

                        If (prev_tagType2 <> 0) AndAlso (tagType2 <> prev_tagType2) Then
                            If exemptTagWriter IsNot Nothing Then
                                exemptTagWriter.CloseTempFile()
                            End If
                        End If
                        If (tagType = tagType2) Then
                            tagType2 = 0
                        End If

                        itemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
                        PBD_ID = results.GetInt32(results.GetOrdinal("PriceBatchDetailID"))

                        logger.Info("GetReprintTagDataFromIRMA - Processing Store_No=" + currentStoreNum.ToString + ", subTeamNum=" + subTeamNum.ToString + ", tagType=" + tagType.ToString + ", itemKey=" + itemKey.ToString)
                        If (prev_tagType <> 0) AndAlso (tagType <> prev_tagType) Then
                            If currentTagWriter IsNot Nothing Then
                                currentTagWriter.CloseTempFile()
                            End If
                            prevSBTNum = 0
                        End If
                        If (tagType <> prev_tagType) Then
                            currentStoreName = results.GetString(results.GetOrdinal("Store_Name"))
                            tagExt = results.GetString(results.GetOrdinal("tagExt"))
                            If tagType <> tagType2 Then
                                ' get the writer instance for the store
                                currentTagWriter = CType(currentStoreUpdate.FileWriter, TagWriter)
                                currentStoreUpdate.StoreName = currentStoreName
                                currentStoreUpdate.SetShelfTagTempFilenames(tagExt, "R", batchId.ToString)
                                currentStoreUpdate.ShelfTagFiles.Add(currentStoreUpdate.BatchFileName)
                            End If
                        End If
                        If printExemptTags Then
                            If (tagType2 > 0 AndAlso tagType2 <> prev_tagType2) Then
                                ' Set the Exempt Tag Writer and its file name
                                exemptTagExt = results.GetString(results.GetOrdinal("ExemptTagExt"))
                                currentStoreUpdate.ExemptFileWriter = currentStoreUpdate.FileWriter.Copy()
                                exemptTagWriter = CType(currentStoreUpdate.ExemptFileWriter, TagWriter)
                                exemptTagWriter.ExemptTagFile = True
                                currentStoreUpdate.StoreName = currentStoreName
                                currentStoreUpdate.SetExemptTagTempFilenames(exemptTagExt, "R", batchId.ToString)
                                currentStoreUpdate.ExemptShelfTagFiles.Add(currentStoreUpdate.ExemptTagFileName)
                                If subTeamNum = prevSBTNum Then
                                    If (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                                        exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                                        exemptTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.ExemptTagFileName, results, True)
                                        exemptTagWriter.CloseTempFile()
                                    End If
                                End If
                            End If
                        End If
                        If subTeamNum <> prevSBTNum Then
                            If currentTagWriter IsNot Nothing AndAlso tagType <> tagType2 Then
                                currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                                currentTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.BatchFileName, results, False)
                                currentTagWriter.CloseTempFile()
                            End If
                            If (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                                exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                                exemptTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.ExemptTagFileName, results, True)
                                exemptTagWriter.CloseTempFile()
                            End If
                        End If
                        If (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                            exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                            WriteExemptTagResultsToFile(currentStoreUpdate, results, ChangeType.shelfTagChange)
                            exemptTagWriter.CloseTempFile()
                        End If
                        If currentTagWriter IsNot Nothing AndAlso tagType <> tagType2 Then
                            currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                            WriteResultsToFile(currentStoreUpdate, results, ChangeType.shelfTagChange)
                            currentTagWriter.CloseTempFile()
                        End If
                        If (Not storeTagFTPs.ContainsKey(currentStoreUpdate.StoreNum)) Then
                            storeTagFTPs.Add(currentStoreUpdate.StoreNum, currentStoreUpdate)
                        End If

                        tagBO = New ShelfTagBO(PBD_ID, itemKey, tagType, tagType2)

                        itemTagBOs.Add(tagBO.ItemKey.ToString() + ":" + RowNumber.ToString(), tagBO)
                        RowNumber = RowNumber + 1

                        If tagType2 > 0 Then
                            prev_tagType2 = tagType2
                        End If
                        prev_tagType = tagType
                        prevSBTNum = subTeamNum
                    Else
                        ' ERROR PROCESSING ... all stores should be found in the hash
                        logger.Warn("Error processing a record returned by Replenishment_TagPush_GetReprintTagFile.  Store # not configured in StorePOSConfig table: " & currentStoreNum.ToString)

                        'send message about exception
                        Dim args(1) As String
                        args(0) = currentStoreNum.ToString
                        ErrorHandler.ProcessError(ErrorType.POSPush_StoreNotFound, args, SeverityLevel.Warning)
                        Continue While
                    End If
                End While

                logger.Debug("done processing all results")

                If storeTagFTPs.Count > 0 Then
                    Dim transfer As New TransferWriterFiles
                    Dim transferSuccess As Boolean = transfer.TransferStoreFiles(storeTagFTPs)
                    If Not transferSuccess Then
                        ' update the error message so the user knows what happened
                        logger.Debug("Transfer of Tag Writer files did not succeed.")
                    End If
                End If

                logger.Debug("done transferring non EST files")

            Catch e As Exception
                'common exceptions would be DataFactory or File I/O exceptions
                Throw e
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try
            logger.Debug("GetReprintTagDataFromIRMA exit")
        End Sub

        Public Overrides Sub GetPlanogramTagDataFromIRMA(ByVal itemList As String, ByVal itemListSeparator As Char, _
                                            ByVal store_no As Integer, ByVal isRegular As Boolean, ByVal startDate As Date)
            logger.Debug("GetPlanogramTagDataFromIRMA entry: store_No=" + store_no.ToString + ", isRegular=" + isRegular.ToString + ", startDate=" + startDate.ToString)
            Dim results As SqlDataReader = Nothing
            Dim currentStoreNum As Integer
            Dim currentStoreUpdate As StoreUpdatesBO = Nothing
            Dim currentStoreName As String = Nothing
            Dim currentTagExt As String = Nothing
            Dim exemptTagExt As String = Nothing
            Dim prev_tagType As Integer = 0
            Dim prev_tagType2 As Integer = 0
            Dim tagType As Integer = 0
            Dim tagType2 As Integer = 0
            Dim tagExt As String = Nothing
            Dim prevSBTNum As Integer = 0
            Dim subTeamNum As Integer = 0
            Dim RowNumber As Integer = 0
            Dim currentTagWriter As TagWriter = Nothing
            Dim exemptTagWriter As TagWriter = Nothing
            Dim itemKey As Integer
            Dim PBD_ID As Integer
            Dim tagBO As ShelfTagBO = Nothing
            Dim planoCode As String = String.Empty
            Dim prev_planoCode As String = String.Empty
            Dim planoGroup As String = String.Empty
            Dim prev_planoGroup As String = String.Empty
            Dim planoShelf As String = String.Empty
            Dim prev_planoShelf As String = String.Empty
            Dim onlyStoreNo As Integer = 0
            Dim recCount As Integer
            Dim PlanogramRBXFileExtVal As Boolean = False
            Dim suffixType As String
            Dim printExemptTags As Boolean = False
            Dim PosFileWriterKey As Integer

            recCount = 0
            Try
                'If reg tags are printed there is no date needed in the query, 
                'but because the date does not appear on the form it needs to be normalized
                If CDate(startDate) < Now() Then
                    startDate = CDate("#1/1/1900#")
                End If

                ' Read instance data flags
                printExemptTags = InstanceDataDAO.IsFlagActive("ExemptShelfTags")

                If InstanceDataDAO.IsFlagActive("PlanogramRBXFileExt") Then
                    PlanogramRBXFileExtVal = True
                End If


                ' Initialize the storeUpdates hashtable, adding an entry for each store
                storeUpdates = StorePOSConfigDAO.GetShelfTagStoreConfigurations(Constants.FileWriterType_TAG, CType(store_no, String))

                If (storeUpdates.ContainsKey(store_no)) Then
                    currentStoreUpdate = CType(storeUpdates.Item(store_no), StoreUpdatesBO)
                    PosFileWriterKey = CInt(currentStoreUpdate.FileWriter.POSFileWriterKey.ToString)
                Else
                    PosFileWriterKey = 0
                End If
                ' Read the price batch changes that are ready to be sent to the stores.
                logger.Info("GetPlanogramTagDataFromIRMA - reading the TAG data from the database: store_No=" + store_no.ToString + ", isRegular=" + isRegular.ToString + ", startDate=" + startDate.ToString + ", PosFileWriterKey=" + PosFileWriterKey.ToString)
                results = TagWriterDAO.GetSetRegTagData(itemList, itemListSeparator, store_no, isRegular, CDate(startDate), PosFileWriterKey)

                ' -- First resultset - list of PriceBatchHeader records being updated
                logger.Debug("processing first result set - planogram set reg tags")

                While results.Read()
                    ' get the store # for this record
                    currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))
                    planoCode = results.GetString(results.GetOrdinal("ProductPlanogramCode"))
                    planoGroup = results.GetString(results.GetOrdinal("ProductGroup"))
                    planoShelf = results.GetString(results.GetOrdinal("ShelfIdentifier"))

                    ' verify this store is configured in StorePOSConfig
                    If (storeUpdates.ContainsKey(currentStoreNum)) Then
                        currentStoreUpdate = CType(storeUpdates.Item(currentStoreNum), StoreUpdatesBO)

                        tagType = results.GetInt32(results.GetOrdinal("ShelfTagTypeID"))

                        If (Not results.IsDBNull(results.GetOrdinal("ExemptTagTypeID"))) Then
                            tagType2 = results.GetInt32(results.GetOrdinal("ExemptTagTypeID"))
                        Else
                            tagType2 = 0
                        End If
                        If (prev_tagType2 <> 0) AndAlso (tagType2 <> prev_tagType2) Then
                            If exemptTagWriter IsNot Nothing Then
                                exemptTagWriter.CloseTempFile()
                            End If
                        End If
                        If (tagType = tagType2) Then
                            tagType2 = 0
                        End If

                        itemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
                        PBD_ID = results.GetInt32(results.GetOrdinal("PriceBatchDetailID"))

                        logger.Info("GetPlanogramTagDataFromIRMA - Processing Store_No=" + currentStoreNum.ToString + ", planoCode=" + planoCode + ", planoGroup=" + planoGroup + ", planoShelf=" + planoShelf + ", tagType=" + tagType.ToString + ", itemKey=" + itemKey.ToString)
                        If (prev_tagType <> 0) AndAlso (tagType <> prev_tagType) Then
                            currentTagWriter.CloseTempFile()
                            If exemptTagWriter IsNot Nothing Then
                                exemptTagWriter.CloseTempFile()
                                exemptTagWriter = Nothing
                            End If
                        End If

                        If (tagType <> prev_tagType) Then
                            currentStoreName = results.GetString(results.GetOrdinal("Store_Name"))
                            If PlanogramRBXFileExtVal = True Then
                                tagExt = "S" + CType(store_no, String)
                                suffixType = ""
                            Else
                                tagExt = results.GetString(results.GetOrdinal("tagExt"))
                                suffixType = "P"
                            End If
                            If tagType <> tagType2 Then
                                ' get the writer instance for the store
                                currentTagWriter = CType(currentStoreUpdate.FileWriter, TagWriter)
                                currentStoreUpdate.StoreName = currentStoreName
                                If PlanogramRBXFileExtVal = True Then
                                    currentStoreUpdate.SetShelfTagRBXFilenames(planoCode, tagExt)
                                Else
                                    currentStoreUpdate.SetShelfTagTempFilenames(tagExt, suffixType, batchId.ToString)
                                End If
                                currentStoreUpdate.ShelfTagFiles.Add(currentStoreUpdate.BatchFileName)
                            End If
                        End If
                        If PlanogramRBXFileExtVal = True Then
                            If (prev_planoCode <> planoCode) OrElse (tagType <> prev_tagType) Then
                                If currentTagWriter IsNot Nothing Then
                                    currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                                    If prev_planoCode <> "" Then
                                        ' write the footer record
                                        currentTagWriter.AddPlanogramFooterToFile(recCount)
                                        recCount = 0
                                    End If
                                    ' write the header record
                                    currentTagWriter.AddPlanogramHeaderToFile(currentStoreUpdate.BatchFileName, results)
                                    currentTagWriter.CloseTempFile()
                                End If
                            End If
                        Else
                            If (planoGroup <> prev_planoGroup) OrElse (planoShelf <> prev_planoShelf) OrElse (tagType <> prev_tagType) Then
                                If currentTagWriter IsNot Nothing Then
                                    currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                                    ' write the header record
                                    currentTagWriter.AddPlanogramHeaderToFile(currentStoreUpdate.BatchFileName, results)
                                    currentTagWriter.CloseTempFile()
                                End If
                            End If
                        End If
                        ' Set the Exempt Tag Writer and its file name
                        If printExemptTags Then
                            If (tagType2 > 0 AndAlso tagType2 <> prev_tagType2) Then
                                exemptTagExt = results.GetString(results.GetOrdinal("ExemptTagExt"))
                                currentStoreUpdate.ExemptFileWriter = currentStoreUpdate.FileWriter.Copy()
                                exemptTagWriter = CType(currentStoreUpdate.ExemptFileWriter, TagWriter)
                                exemptTagWriter.ExemptTagFile = True
                                currentStoreUpdate.StoreName = currentStoreName
                                currentStoreUpdate.SetExemptTagTempFilenames(exemptTagExt, "P", batchId.ToString)
                                currentStoreUpdate.ExemptShelfTagFiles.Add(currentStoreUpdate.ExemptTagFileName)
                                exemptTagWriter.CloseTempFile()
                                If (planoGroup = prev_planoGroup) AndAlso (planoShelf = prev_planoShelf) Then
                                    If (Not results.IsDBNull(results.GetOrdinal("ExemptTagExt")) AndAlso exemptTagWriter IsNot Nothing) Then
                                        exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                                        ' write the header record
                                        exemptTagWriter.AddPlanogramHeaderToFile(currentStoreUpdate.ExemptTagFileName, results)
                                        exemptTagWriter.CloseTempFile()
                                    End If
                                End If
                            End If
                        End If
                        If (planoGroup <> prev_planoGroup) OrElse (planoShelf <> prev_planoShelf) Then
                            If printExemptTags AndAlso (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                                exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                                ' write the header record
                                exemptTagWriter.AddPlanogramHeaderToFile(currentStoreUpdate.ExemptTagFileName, results)
                                exemptTagWriter.CloseTempFile()
                            End If
                        End If
                        If printExemptTags AndAlso (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                            exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                            WriteExemptTagResultsToFile(currentStoreUpdate, results, ChangeType.shelfTagChange)
                            exemptTagWriter.CloseTempFile()
                        End If
                        If currentTagWriter IsNot Nothing AndAlso tagType <> tagType2 Then
                            currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                            WriteResultsToFile(currentStoreUpdate, results, ChangeType.shelfTagChange)
                            currentTagWriter.CloseTempFile()
                        End If
                        If onlyStoreNo = 0 Then
                            'only add store once
                            storeTagFTPs.Add(currentStoreUpdate.StoreNum, currentStoreUpdate)
                            onlyStoreNo = currentStoreNum
                        End If

                        tagBO = New ShelfTagBO(PBD_ID, itemKey, tagType, tagType2)
                        itemTagBOs.Add(tagBO.ItemKey.ToString() + ":" + RowNumber.ToString(), tagBO)
                        RowNumber = RowNumber + 1
                        If PlanogramRBXFileExtVal = True Then
                            recCount = recCount + 1
                        End If
                        If tagType2 > 0 Then
                            prev_tagType2 = tagType2
                        End If
                        prev_tagType = tagType
                        prev_planoGroup = planoGroup
                        prev_planoShelf = planoShelf
                        prev_planoCode = planoCode
                    Else
                        ' ERROR PROCESSING ... all stores should be found in the hash
                        logger.Warn("Error processing a record returned by Planogram_GetSetRegTagFile or Planogram_GetNonRegTagFile.  Store # not configured in StorePOSConfig table: " & currentStoreNum.ToString)

                        'send message about exception
                        Dim args(1) As String
                        args(0) = currentStoreNum.ToString
                        ErrorHandler.ProcessError(ErrorType.POSPush_StoreNotFound, args, SeverityLevel.Warning)
                        Continue While
                    End If
                End While

                If currentTagWriter IsNot Nothing Then
                    If PlanogramRBXFileExtVal = True Then
                        currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                        ' write the footer record
                        currentTagWriter.AddPlanogramFooterToFile(recCount)
                        recCount = 0
                    End If
                    currentTagWriter.CloseTempFile()
                End If
                logger.Debug("done processing all results")
            Catch e As Exception
                'common exceptions would be DataFactory or File I/O exceptions
                logger.Error("GetPlanogramTagDataFromIRMA - error during processing; the exception is caught & just thrown again", e)
                Throw e
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
                If exemptTagWriter IsNot Nothing Then
                    exemptTagWriter.CloseTempFile()
                End If
                If currentTagWriter IsNot Nothing Then
                    currentTagWriter.CloseTempFile()
                End If
            End Try

            If storeTagFTPs.Count > 0 Then
                Dim transfer As New TransferWriterFiles
                Dim transferSuccess As Boolean = transfer.TransferStoreFiles(storeTagFTPs)

                If transferSuccess Then
                Else
                    ' update the error message so the user knows what happened
                    logger.Info("Transfer of Planogram files did not succeed.")
                End If
            End If
            logger.Debug("GetPlanogramTagDataFromIRMA exit")
        End Sub

        Public Function ProcessElectronicShelfTags() As Boolean
            Dim results As SqlDataReader = Nothing
            Dim currentStoreUpdate As StoreUpdatesBO
            Dim currentStoreNum As Integer
            Dim currentStoreName As String = Nothing
            Dim currentTagExt As String = Nothing
            Dim exemptTagExt As String = Nothing
            Dim prev_tagType As Integer = 0
            Dim prev_tagType2 As Integer = 0
            Dim tagType As Integer = 0
            Dim tagType2 As Integer = 0
            Dim tagExt As String = Nothing
            Dim prevSBTNum As Integer = 0
            Dim prevStoreNum As Integer = 0
            Dim subTeamNum As Integer = 0
            Dim RowNumber As Integer = 0
            Dim currentTagWriter As ElectronicShelfTagWriter = Nothing
            Dim exemptTagWriter As ElectronicShelfTagWriter = Nothing
            Dim itemKey As Integer
            Dim PBD_ID As Integer
            Dim tagBO As ShelfTagBO = Nothing

            Dim printExemptTags As Boolean = False

            Dim posPushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                storeUpdates = StorePOSConfigDAO.GetElectronicShelfTagStoreConfigurations(Constants.FileWriterType_ELECTRONICSHELFTAG)

                posPushJobTimer = Stopwatch.StartNew()

                results = TagWriterDAO.GetElectronicShelfTagBatchData

                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing Electronic Shelf Tag DB Step in POS Push: " + stepTimeInSeconds.ToString())
                logger.Info("Time taken for processing Electronic Shelf Tag DB Step in POS Push: " + stepTimeInSeconds.ToString())
                posPushJobTimer = Stopwatch.StartNew()

                While results.Read
                    ' get the store # for this record
                    currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))

                    'Reset variables if the current and previous store numbers do not match
                    If currentStoreNum <> prevStoreNum Then
                        prev_tagType = 0
                        prev_tagType2 = 0
                    End If

                    subTeamNum = results.GetInt32(results.GetOrdinal("SubTeam_No"))

                    ' verify this store is configured in StorePOSConfig
                    If (storeUpdates.ContainsKey(currentStoreNum)) Then
                        currentStoreUpdate = CType(storeUpdates.Item(currentStoreNum), StoreUpdatesBO)
                        'currentTagWriter = CType(currentStoreUpdate.FileWriter, PrintLab_Writer)

                        tagType = results.GetInt32(results.GetOrdinal("ShelfTagTypeID"))

                        If (Not results.IsDBNull(results.GetOrdinal("ExemptTagTypeID"))) Then
                            tagType2 = results.GetInt32(results.GetOrdinal("ExemptTagTypeID"))
                        Else
                            tagType2 = 0
                        End If

                        If (prev_tagType2 <> 0) AndAlso (tagType2 <> prev_tagType2) Then
                            If exemptTagWriter IsNot Nothing Then
                                exemptTagWriter.CloseTempFile()
                            End If
                        End If
                        If (tagType = tagType2) Then
                            tagType2 = 0
                        End If
                        itemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
                        PBD_ID = results.GetInt32(results.GetOrdinal("PriceBatchDetailID"))

                        logger.Info("GetReprintTagDataFromIRMA - Processing Store_No=" + currentStoreNum.ToString + ", subTeamNum=" + subTeamNum.ToString + ", tagType=" + tagType.ToString + ", itemKey=" + itemKey.ToString)
                        If (prev_tagType <> 0) AndAlso (tagType <> prev_tagType) Then
                            If currentTagWriter IsNot Nothing Then
                                currentTagWriter.CloseTempFile()
                            End If
                            prevSBTNum = 0
                        End If
                        If (tagType <> prev_tagType) Then
                            currentStoreName = results.GetString(results.GetOrdinal("Store_Name"))
                            tagExt = results.GetString(results.GetOrdinal("tagExt"))
                            If tagType <> tagType2 Then
                                ' get the writer instance for the store
                                currentTagWriter = CType(currentStoreUpdate.FileWriter, ElectronicShelfTagWriter)
                                currentStoreUpdate.StoreName = currentStoreName
                                currentStoreUpdate.SetShelfTagTempFilenames(tagExt, "R", batchId.ToString)
                                currentStoreUpdate.ShelfTagFiles.Add(currentStoreUpdate.BatchFileName)
                            End If
                        End If
                        If printExemptTags Then
                            If (tagType2 > 0 AndAlso tagType2 <> prev_tagType2) Then
                                ' Set the Exempt Tag Writer and its file name
                                exemptTagExt = results.GetString(results.GetOrdinal("ExemptTagExt"))
                                currentStoreUpdate.ExemptFileWriter = currentStoreUpdate.FileWriter.Copy()
                                exemptTagWriter = CType(currentStoreUpdate.ExemptFileWriter, ElectronicShelfTagWriter)
                                exemptTagWriter.ExemptTagFile = True
                                currentStoreUpdate.StoreName = currentStoreName
                                currentStoreUpdate.SetExemptTagTempFilenames(exemptTagExt, "R", batchId.ToString)
                                currentStoreUpdate.ExemptShelfTagFiles.Add(currentStoreUpdate.ExemptTagFileName)
                                If subTeamNum = prevSBTNum Then
                                    If (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                                        exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                                        exemptTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.ExemptTagFileName, results, True)
                                        exemptTagWriter.CloseTempFile()
                                    End If
                                End If
                            End If
                        End If
                        If subTeamNum <> prevSBTNum Then
                            If currentTagWriter IsNot Nothing AndAlso tagType <> tagType2 Then
                                currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                                currentTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.BatchFileName, results, False)
                                currentTagWriter.CloseTempFile()
                            End If
                            If (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                                exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                                exemptTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.ExemptTagFileName, results, True)
                                exemptTagWriter.CloseTempFile()
                            End If
                        End If
                        If (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                            exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                            WriteExemptElectronicShelfTagResultsToFile(currentStoreUpdate, results, ChangeType.ElectronicShelfTagChange)
                            exemptTagWriter.CloseTempFile()
                        End If
                        If currentTagWriter IsNot Nothing AndAlso tagType <> tagType2 Then
                            currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                            WriteElectronicResultsToFile(currentStoreUpdate, results, ChangeType.ElectronicShelfTagChange)
                            currentTagWriter.CloseTempFile()
                        End If
                        If (Not storeTagFTPs.ContainsKey(currentStoreUpdate.StoreNum)) Then
                            storeTagFTPs.Add(currentStoreUpdate.StoreNum, currentStoreUpdate)
                        End If

                        tagBO = New ShelfTagBO(PBD_ID, itemKey, tagType, tagType2)

                        itemTagBOs.Add(tagBO.ItemKey.ToString() + ":" + RowNumber.ToString(), tagBO)
                        RowNumber = RowNumber + 1

                        If tagType2 > 0 Then
                            prev_tagType2 = tagType2
                        End If
                        prev_tagType = tagType
                        prevSBTNum = subTeamNum
                        prevStoreNum = currentStoreNum
                    Else
                        ' ERROR PROCESSING ... all stores should be found in the hash
                        logger.Warn("Error processing a record returned by Replenishment_TagPush_GetReprintTagFile.  Store # not configured in StorePOSConfig table: " & currentStoreNum.ToString)

                        'send message about exception
                        Dim args(1) As String
                        args(0) = currentStoreNum.ToString
                        ErrorHandler.ProcessError(ErrorType.POSPush_StoreNotFound, args, SeverityLevel.Warning)
                        Continue While
                    End If
                End While

                logger.Debug("done processing all results")

                If storeTagFTPs.Count > 0 Then
                    Dim transfer As New TransferWriterFiles
                    Dim transferSuccess As Boolean = transfer.TransferStoreFiles(storeTagFTPs)
                    If Not transferSuccess Then
                        logger.Debug("Transfer of Tag Writer files did not succeed.")
                        Return False
                    End If
                End If

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing Electronic Shelf Tag Application Step in POS Push: " + stepTimeInSeconds.ToString())
                logger.Info("Time taken for processing Electronic Shelf Tag Application Step in POS Push: " + stepTimeInSeconds.ToString())
                logger.Debug("done transferring EST files")
                Return True

            Catch ex As Exception
                Throw ex
                Return False
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try
        End Function
    End Class
End Namespace
