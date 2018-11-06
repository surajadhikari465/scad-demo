Imports System.IO
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Object
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.Writers
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Controller

    ''' <summary>
    ''' ScaleProcessor defines the base class for delivering a type of change to the SCALE
    ''' system for the regionalal host, or to each store.  
    ''' This class is subclassed to provide processing for each ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class ScaleProcessor

        'Date used for all calls to stored procedures
        Protected dStart As Date

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="inDate"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal inDate As Date)
            Logger.LogDebug("New entry: inDate=" & inDate.ToString(), Me.GetType())
            dStart = inDate
            Logger.LogDebug("New exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Reads the database records from IRMA for the type of change being handled by the subclass, 
        ''' adding the results to the SCALE Push file.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub RetrieveChangeRecordsFromIRMA(ByRef StoreUpdatesBO As Hashtable, ByVal isRegionalScaleFile As Boolean, Optional ByVal storeNo As String = Nothing)

        Public Overridable Sub RetrieveStoreChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable)
            'not required to be overriden, but if needed logic will be in overriden method
        End Sub

        Public Overridable Sub RetrieveStoreChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable, ByVal storeNo As String)
            'not required to be overriden, but if needed logic will be in overriden method
        End Sub

        Public Overridable Sub RetrieveCorporateChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable)
            'not required to be overriden, but if needed logic will be in overriden method
        End Sub

        Public Overridable Sub RetrieveCorporateChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable, ByVal storeNo As String)
            'not required to be overriden, but if needed logic will be in overriden method
        End Sub

        Public Overridable Sub RetrieveZoneChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable)
            'not required to be overriden, but if needed logic will be in overriden method
        End Sub

        ''' <summary>
        ''' Processes a result set, adding the records to the SCALE Push file the store associated with the change.
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <param name="results"></param>
        ''' <param name="chgType"></param>
        ''' <remarks></remarks>
        Public Sub WriteResultsToFile(ByRef storeUpdates As Hashtable, ByRef results As SqlDataReader, _
                                        ByVal chgType As ChangeType, ByVal isRegionalScaleFile As Boolean, _
                                        Optional ByVal scaleWriterType As String = Nothing)
            Logger.LogDebug("WriteResultsToFile entry: changeType=" + chgType.ToString(), Me.GetType())
            Dim currentStoreUpdate As StoreUpdatesBO = Nothing
            Dim currentScaleWriter As ScaleWriter = Nothing
            Dim previousStoreNum As Integer = -1
            Dim currentStoreNum As Integer = -1
            Dim previousBatchID As Integer = -1
            Dim currentBatchID As Integer = -1

            Dim headerInfo As New POSBatchHeaderBO  'Now declaring this as a New POSBatchHeaderBO rather than Nothing
            Dim footerInfo As POSBatchFooterBO = Nothing
            Dim isBatchChange As Boolean = False
            Dim isChangeTypeConfigDataError As Boolean
            Dim writeFooter As Boolean
            Dim strChangeType As String = ""
            Dim retailSale As Boolean

            Dim changingStores As Boolean
            Dim changingBatches As Boolean
            Dim changingEffectiveDate As Boolean
            Dim currentEffectiveDate As DateTime
            Dim previousEffectiveDate As DateTime

            Dim corpScaleWriterDefined As Boolean
            Dim storeEmailDelivered As New List(Of Integer)

            'manage store config errors
            Dim storeConfigErrorMsg As New StringBuilder
            Dim currentStoreConfigError As StringBuilder
            Dim configErrorStores As New Hashtable

            Select Case chgType
                Case ChangeType.ItemDataChange
                    If Not isRegionalScaleFile Then
                        isBatchChange = True
                    End If
            End Select

            ' results contains an entry for each change record that should be added to the SCALE Push file
            Logger.LogDebug("adding detail records to the SCALE push file", Me.GetType())
            While results.Read()
                'Logger.LogDebug("there are results to add to the SCALE push file", Me.GetType())
                isChangeTypeConfigDataError = False

                If isRegionalScaleFile Then
                    'get regional business unit as store no.
                    Logger.LogDebug("ScaleWriterType=" + scaleWriterType, Me.GetType())
                    If storeUpdates.Item(scaleWriterType) IsNot Nothing Then
                        currentStoreNum = CType(storeUpdates.Item(scaleWriterType), StoreUpdatesBO).StoreNum
                        corpScaleWriterDefined = True
                    Else
                        ' If the region does not use the Scale Push process, they will not have an entry in the
                        ' hashtable.
                        currentStoreNum = -1
                        corpScaleWriterDefined = False
                    End If
                Else
                    ' get the store # for this record
                    currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))
                End If

                If isBatchChange Then
                    'get current batch ID if one exists
                    currentBatchID = results.GetInt32(results.GetOrdinal("PriceBatchHeaderID"))
                End If

                ' ***** check the effective date of the batch record - if ZonePriceChange *****
                If chgType = ChangeType.ZoneScalePriceChange Then

                    currentEffectiveDate = results.GetDateTime(results.GetOrdinal("Sale_Start_Date"))
                    If currentEffectiveDate <> previousEffectiveDate Then
                        changingEffectiveDate = True
                        headerInfo.EffectiveDate = currentEffectiveDate
                    Else
                        changingEffectiveDate = False
                    End If
                End If

                If results.IsDBNull(results.GetOrdinal("Retail_Sale")) Then
                    retailSale = False
                Else
                    retailSale = CBool(results.GetBoolean(results.GetOrdinal("Retail_Sale")))
                End If

                ' verify this store is configured in StoreScaleConfig
                If (storeUpdates.ContainsKey(currentStoreNum)) Or (isRegionalScaleFile AndAlso corpScaleWriterDefined) Then
                    ' Did we change stores?  If so, complete the processing for the previous store & initialize the
                    ' objects for the current store being processed.  If this change type contains batches, also
                    ' check for existence of new batch
                    changingStores = previousStoreNum <> currentStoreNum
                    changingBatches = isBatchChange AndAlso (previousBatchID <> currentBatchID)

                    If changingStores Or changingBatches Or changingEffectiveDate Then

                        If writeFooter AndAlso _
                            ((changingStores AndAlso previousStoreNum <> -1) Or _
                             (changingBatches AndAlso previousBatchID <> -1 AndAlso currentScaleWriter.OutputByIrmaBatches)) Then
                            ' complete the previous store/batch - add the footer line
                            ' currentPosWriter is still set to the writer for the previousStoreNum and previousBatchID
                            Logger.LogDebug("adding footer to the SCALE push file", Me.GetType())
                            currentScaleWriter.AddFooterToFile(chgType, footerInfo)
                        End If

                        If isRegionalScaleFile Then
                            ' get CORPORATE or ZONE record from store hash -
                            ' the entries in the store has are keyed by scale type here instead of store number since 
                            ' the regional store has more than one writer assigned to it
                            currentStoreUpdate = CType(storeUpdates.Item(scaleWriterType), StoreUpdatesBO)
                        Else
                            ' get the StoreUpdatesBO for the current store being processed -
                            ' the entries in the store has are keyed by store number in this case
                            currentStoreUpdate = CType(storeUpdates(currentStoreNum), StoreUpdatesBO)
                        End If

                        ' get the writer instance for the store
                        currentScaleWriter = CType(currentStoreUpdate.FileWriter, ScaleWriter)

                        'get header and footer info for current store/batch.
                        'also make sure config data is setup for this change type before attempting to add data to file
                        If isRegionalScaleFile Then
                            Select Case chgType
                                Case ChangeType.CorpScaleItemChange
                                    If currentScaleWriter.CorpScaleItemChangeConfig.Count <= 0 Then
                                        strChangeType = "CORPORATE SCALE ITEM CHANGE"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.CorpScaleItemIdAdd
                                    If currentScaleWriter.CorpScaleIdAddConfig.Count <= 0 Then
                                        strChangeType = "CORPORATE SCALE ITEM ID ADD"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.CorpScaleItemIdDelete
                                    If currentScaleWriter.CorpScaleIdDeleteConfig.Count <= 0 Then
                                        strChangeType = "CORPORATE SCALE ITEM ID DELETE"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.CorpScalePriceExceptions
                                    ' the price exceptions included with the corporate scale data are written using the
                                    ' same format and writer definition as the zone price changes
                                    If currentScaleWriter.CorpScalePriceExceptionConfig.Count <= 0 Then
                                        strChangeType = "CORPORATE SCALE PRICE EXCEPTIONS (USES ZONE SCALE PRICE CHANGE CONFIGURATION)"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.ZoneScalePriceChange
                                    If currentScaleWriter.ZoneScalePriceChangeConfig.Count <= 0 Then
                                        strChangeType = "ZONE SCALE PRICE CHANGE"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.ZoneScaleItemAuthPriceChange
                                    ' item authorizations are communicated to corporate scale systems as a zone price
                                    ' change, using the same format and writer definition as the zone price changes
                                    If currentScaleWriter.ZoneScalePriceChangeConfig.Count <= 0 Then
                                        strChangeType = "ZONE SCALE ITEM AUTH PRICE CHANGE (USES ZONE SCALE PRICE CHANGE CONFIGURATION)"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.ZoneScaleItemDeAuthPriceChange
                                    ' item de-authorizations are communicated to corporate scale systems as a zone price
                                    ' change, using the same format and writer definition as the zone price changes
                                    If currentScaleWriter.ZoneScalePriceChangeConfig.Count <= 0 Then
                                        strChangeType = "ZONE SCALE ITEM DE-AUTH PRICE CHANGE (USES ZONE SCALE PRICE CHANGE CONFIGURATION)"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.ZoneScaleSmartXPriceChange
                                    If currentScaleWriter.ZoneScaleSmartXPriceChangeConfig.Count <= 0 Then
                                        strChangeType = "ZONE SCALE SMART X PRICE CHANGE"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.ZoneScaleItemDelete
                                    If currentScaleWriter.ZoneScaleItemDeleteConfig.Count <= 0 Then
                                        strChangeType = "ZONE SCALE ITEM DELETE"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.NutriFact
                                    If currentScaleWriter.NutrifactChangeConfig.Count < 0 Then ' can be zero
                                        strChangeType = "NUTRIFACT CHANGE"
                                        isChangeTypeConfigDataError = False
                                    End If
                                Case ChangeType.ExtraText
                                    If currentScaleWriter.ExtraTextChangeConfig.Count < 0 Then ' can be zero
                                        strChangeType = "EXTRA TEXT CHANGE"
                                        isChangeTypeConfigDataError = False
                                    End If
                            End Select

                            ' A new change type was added to include zone pricing data with all corporate record changes.
                            ' The Scale Writer should be checked to see if these records are included before processing them.
                            If chgType = ChangeType.CorpScalePriceExceptions Then
                                If Not currentScaleWriter.CorpRecordsIncludePricing Then
                                    Continue While
                                End If
                            End If

                        Else
                            Select Case chgType
                                Case ChangeType.CorpScaleItemChange
                                    If currentScaleWriter.CorpScaleItemChangeConfig.Count <= 0 Then
                                        strChangeType = "CORPORATE SCALE ITEM CHANGE"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.CorpScaleItemIdAdd
                                    If currentScaleWriter.CorpScaleIdAddConfig.Count <= 0 Then
                                        strChangeType = "CORPORATE SCALE ITEM ID ADD"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.CorpScaleItemIdDelete
                                    If currentScaleWriter.CorpScaleIdDeleteConfig.Count <= 0 Then
                                        strChangeType = "CORPORATE SCALE ITEM ID DELETE"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.CorpScalePriceExceptions
                                    ' the price exceptions included with the corporate scale data are written using the
                                    ' same format and writer definition as the zone price changes
                                    If currentScaleWriter.CorpScalePriceExceptionConfig.Count <= 0 Then
                                        ' Copy the zone pricing definition to the corporate pricing definition
                                        If currentScaleWriter.ZoneScalePriceChangeConfig.Count <= 0 Then
                                            strChangeType = "CORPORATE SCALE PRICE EXCEPTIONS (USES ZONE SCALE PRICE CHANGE CONFIGURATION)"
                                            isChangeTypeConfigDataError = True
                                        Else
                                            currentScaleWriter.CorpScalePriceExceptionConfig = currentScaleWriter.ZoneScalePriceChangeConfig
                                        End If
                                    End If
                                Case ChangeType.ZoneScalePriceChange
                                    If currentScaleWriter.ZoneScalePriceChangeConfig.Count <= 0 Then
                                        strChangeType = "ZONE SCALE PRICE CHANGE"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.ZoneScaleItemDelete
                                    If currentScaleWriter.ZoneScaleItemDeleteConfig.Count <= 0 Then
                                        strChangeType = "ZONE SCALE ITEM DELETE"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.ItemDataChange
                                    If currentScaleWriter.ItemDataChangeConfig.Count <= 0 Then
                                        strChangeType = "STORE SCALE ITEM DATA CHANGE"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.ItemIdAdd
                                    If currentScaleWriter.ItemIdAddConfig.Count <= 0 Then
                                        strChangeType = "STORE SCALE ITEM ID ADD"
                                        isChangeTypeConfigDataError = True
                                    End If
                                Case ChangeType.ItemIdDelete
                                    If currentScaleWriter.ItemIdDeleteConfig.Count <= 0 Then
                                        strChangeType = "STORE SCALE ITEM ID DELETE"
                                        isChangeTypeConfigDataError = True
                                    End If
                            End Select

                            ' A new change type was added to include zone pricing data with all corporate record changes.
                            ' The Scale Writer should be checked to see if these records are included before processing them.
                            If chgType = ChangeType.CorpScalePriceExceptions Then
                                If Not currentScaleWriter.CorpRecordsIncludePricing Then
                                    Continue While
                                End If
                            End If

                        End If

                        If isChangeTypeConfigDataError Then
                            writeFooter = False

                            'build config error message for store/change type combo so only 1 error message is sent for this Scale Push run
                            'containing info for all stores that need to be configured
                            currentStoreConfigError = New StringBuilder
                            currentStoreConfigError.Append(currentStoreNum.ToString)
                            currentStoreConfigError.Append("::")
                            currentStoreConfigError.Append(strChangeType)

                            'check that store isn't already added to list
                            If Not configErrorStores.ContainsKey(currentStoreConfigError.ToString) Then
                                configErrorStores.Add(currentStoreConfigError.ToString, True)

                                storeConfigErrorMsg.Append("Store: ")
                                storeConfigErrorMsg.Append(currentStoreNum.ToString)
                                storeConfigErrorMsg.Append(" - ")
                                storeConfigErrorMsg.Append(strChangeType)
                                storeConfigErrorMsg.Append(Environment.NewLine)
                            End If

                            Continue While
                        Else
                            writeFooter = True
                        End If


                        ' append the file header info for this change
                        ' store_no is needed for a couple of the writers, add store_no to headerInfo
                        headerInfo.StoreNo = currentStoreNum

                        ' if ePlum and a price change, set the apply date in the header
                        If scaleWriterType = Constants.ScaleWriterType_Corporate And chgType = ChangeType.CorpScalePriceExceptions Then
                            Try
                                headerInfo.ApplyDate = results.GetDateTime(results.GetOrdinal("ApplyDate"))
                            Catch ex As IndexOutOfRangeException
                                ' Ignore the error and move on.  This only has a value for regions using ePLUM.
                            End Try
                            Stop
                        End If

                        Logger.LogDebug("adding header to the SCALE push file", Me.GetType())
                        currentScaleWriter.AddHeaderToFile(chgType, currentStoreUpdate.BatchFileName, headerInfo)

                        'reset record count to get total records for current change type being written
                        currentScaleWriter.RecordCount = 0
                    End If
                Else
                    Logger.LogWarning("Error processing WriteResultsToFile - Store # not configured in StorePOSConfig table: " & currentStoreNum, Me.GetType())

                    'send message about exception if an email has not already been delivered for this store number
                    If Not storeEmailDelivered.Contains(currentStoreNum) Then
                        Dim args(1) As String
                        If currentStoreNum = -1 Then
                            args(0) = "Regional Office"
                        Else
                            args(0) = currentStoreNum.ToString
                        End If
                        ErrorHandler.ProcessError(ErrorType.ScalePush_StoreNotFound, args, SeverityLevel.Warning)
                        storeEmailDelivered.Add(currentStoreNum)
                    End If

                    currentStoreUpdate = Nothing
                    currentStoreNum = -1

                    'skip to next record in result set
                    Continue While
                End If

                If retailSale Then
                    ' Append this type of change to the SCALE file for the store, but only for retail sale items.
                    Logger.LogDebug("adding record to the SCALE push file", Me.GetType())

                    currentScaleWriter.AddRecordToFile(chgType, currentStoreUpdate, results)

                    ' track num of lines written to file
                    currentScaleWriter.RecordCount += 1
                Else
                    Logger.LogDebug("Skipping record because it is not retail sale.", Me.GetType())
                End If

                If Not isRegionalScaleFile Then
                    ' for de-authorization records, we need to populate the ArrayList of de-authorizations that have
                    ' been processed by this run of Scale Push 
                    If chgType = ChangeType.ZoneScaleItemDelete Then
                        If (Not results.IsDBNull(results.GetOrdinal("StoreItemAuthorizationID"))) Then
                            currentStoreUpdate.ScaleDeAuthorizations.Add(results.GetInt32(results.GetOrdinal("StoreItemAuthorizationID")))
                        End If
                    End If
                Else
                    ' for authorization and de-authorization records, we need to populate the ArrayList of records that have
                    ' been processed by this run of Scale Push 
                    If chgType = ChangeType.ZoneScaleItemAuthPriceChange Then
                        If (Not results.IsDBNull(results.GetOrdinal("StoreItemAuthorizationID"))) Then
                            currentStoreUpdate.ScaleAuthorizations.Add(results.GetInt32(results.GetOrdinal("StoreItemAuthorizationID")))
                        End If
                    ElseIf chgType = ChangeType.ZoneScaleItemDeAuthPriceChange Then
                        If (Not results.IsDBNull(results.GetOrdinal("StoreItemAuthorizationID"))) Then
                            currentStoreUpdate.ScaleDeAuthorizations.Add(results.GetInt32(results.GetOrdinal("StoreItemAuthorizationID")))
                        End If
                    End If
                End If

                previousStoreNum = currentStoreNum

                previousEffectiveDate = currentEffectiveDate

                If isBatchChange Then
                    previousBatchID = currentBatchID
                End If
            End While

            ' complete the processing for the final store that was processed - add the footer line
            If writeFooter AndAlso currentScaleWriter IsNot Nothing Then
                currentScaleWriter.AddFooterToFile(chgType, footerInfo)
            End If

            'send message about store config exceptions
            If storeConfigErrorMsg.Length > 0 Then
                Dim args(1) As String
                args(0) = storeConfigErrorMsg.ToString
                ErrorHandler.ProcessError(ErrorType.POSPush_StoreConfig, args, SeverityLevel.Warning)
            End If

            Logger.LogDebug("WriteResultsToFile exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Updates the database records in IRMA for the type of change being handled by the subclass.
        ''' This method should be called after the Scale Push file has been delivered to the Scale system
        ''' for the store.
        ''' Note: Most scale updates do not require this functionality.  Currently, it is only used to reset
        ''' the auth/de-auth flags after successfuly delivering a scale file to a store.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Public Overridable Sub ApplyChangesInIRMA(ByRef StoreUpdatesBO As Hashtable)
            ' Does nothing by default.
        End Sub

        ''' <summary>
        ''' Each record that is added to the Scale Push file by WriteResultsToFile may contain key data,
        ''' such as database IDs, that need to be stored for ApplyChangesInIRMA.  The subclass will 
        ''' parse this data from the result record and add it to the StoreUpdatesBO object.
        ''' Note: Most scale updates do not require this functionality.  Currently, it is only used to reset
        ''' the auth/de-auth flags after successfuly delivering a scale file to a store.
        ''' </summary>
        ''' <param name="currentStore"></param>
        ''' <param name="result"></param>
        ''' <remarks></remarks>
        Public Overridable Sub PopulateApplyChangesData(ByRef currentStore As StoreUpdatesBO, ByRef result As SqlDataReader)
            ' Does nothing by default.
        End Sub

    End Class

End Namespace
