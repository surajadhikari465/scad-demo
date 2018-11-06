Imports System.Data.SqlClient
Imports System.Text
Imports log4net
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.Writers
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.POSPush.Controller

    ''' <summary>
    ''' POSProcessor defines the base class for delivering a type of change to the POS
    ''' system for each store.  This class is subclassed to provide processing for each 
    ''' ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class POSProcessor
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        'Date used for all calls to stored procedures
        Protected dStart As Date

        ' Add debug values to help with error processing
        Private _debugSuccessLinesCount As Integer
        Private _debugErrorRowNum As String
        Private _debugErrorColNum As String
        Private _debugStoreNo As Integer
        Private _debugFieldId As String

        Private _posFileWriterList As DataTable = New DataTable


#Region "Property Access Methods"
        Public Property DebugSuccessLinesCount() As Integer
            Get
                Return _debugSuccessLinesCount
            End Get
            Set(ByVal value As Integer)
                _debugSuccessLinesCount = value
            End Set
        End Property

        Public Property DebugErrorRowNum() As String
            Get
                Return _debugErrorRowNum
            End Get
            Set(ByVal value As String)
                _debugErrorRowNum = value
            End Set
        End Property

        Public Property DebugErrorColNum() As String
            Get
                Return _debugErrorColNum
            End Get
            Set(ByVal value As String)
                _debugErrorColNum = value
            End Set
        End Property

        Public Property DebugStoreNo() As Integer
            Get
                Return _debugStoreNo
            End Get
            Set(ByVal value As Integer)
                _debugStoreNo = value
            End Set
        End Property

        Public Property DebugFieldId() As String
            Get
                Return _debugFieldId
            End Get
            Set(ByVal value As String)
                _debugFieldId = value
            End Set
        End Property

        Public Property POSFileWriterList As DataTable
            Get
                Return _posFileWriterList
            End Get
            Set(value As DataTable)
                _posFileWriterList = value
            End Set
        End Property
#End Region

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="inDate"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal inDate As Date)
            logger.Debug("New entry: inDate=" & inDate.ToString())
            dStart = inDate
            logger.Debug("New exit")
        End Sub

        ''' <summary>
        ''' Reads the database records from IRMA for the type of change being handled by the subclass, 
        ''' adding the results to the POS Push file for each store.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <param name="isAuditReport"></param>
        ''' <param name="storeNo">only used by Audit report function - to generate a full store file for a specific store</param>
        ''' <remarks></remarks>
        Public MustOverride Sub RetrieveChangeRecordsFromIRMA(ByRef StoreUpdatesBO As Hashtable, ByVal isAuditReport As Boolean, Optional ByVal storeNo As String = Nothing)
         ''' <summary>
        ''' Processes a result set, adding the results to the Tax Hosting data for the store.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        Public Sub PopulateTaxHostingData(ByRef StoreUpdatesBO As Hashtable, ByRef results As SqlDataReader)
            logger.Debug("PopulateTaxHostingData entry")
            Dim currentStoreUpdate As StoreUpdatesBO = Nothing
            Dim currentStoreNum As Integer = -1
            Dim currentItemKey As Integer = -1
            Dim currentTaxFlag As TaxFlagBO

            ' results contains an entry for each item being added to the POS Push file that 
            ' is configured in the tax hosting tables
            While results.Read()
                ' get the store # for this record
                currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))
                ' get the item key for the record
                currentItemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
                ' verify this store is configured in StorePOSConfig
                If (StoreUpdatesBO.ContainsKey(currentStoreNum)) Then
                    ' get the StoreUpdatesBO for the current store being processed
                    currentStoreUpdate = CType(StoreUpdatesBO(currentStoreNum), StoreUpdatesBO)
                    ' populate a TaxFlagBO for this store item
                    currentTaxFlag = New TaxFlagBO(results)
                    ' store the TaxFlagBO in the hashtable for the store
                    currentStoreUpdate.AddItemTaxFlagData(currentItemKey, currentTaxFlag)
                Else
                    ' Ignore - an error notification does not need to be delivered if the store # is not configured
                    ' in the StorePOSConfig because the message is already being sent by the WriteResultsToFile method
                End If
            End While
            logger.Debug("PopulateTaxHostingData exit")
        End Sub

        ''' <summary>
        ''' Processes a result set, adding the records to the POS Push file the store associated with
        ''' the change.
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <param name="results"></param>
        ''' <param name="chgType"></param>
        ''' <remarks></remarks>
        Public Sub WriteResultsToFile(ByRef storeUpdates As Hashtable, ByRef results As SqlDataReader, ByVal chgType As ChangeType, Optional ByVal IsItemNonBatchableChange As Boolean = False)
            logger.Debug("WriteResultsToFile entry: changeType=" + chgType.ToString())

            Dim currentStoreUpdate As StoreUpdatesBO = Nothing
            Dim currentPosWriter As POSWriter = Nothing
            Dim previousStoreNum As Integer = -1
            Dim currentStoreNum As Integer = -1
            Dim previousBatchID As Integer = -1
            Dim currentBatchID As Integer = -1
            Dim headerInfo As POSBatchHeaderBO = Nothing
            Dim footerInfo As POSBatchFooterBO = Nothing
            Dim isBatchChange As Boolean = False
            Dim isChangeTypeConfigDataError As Boolean
            Dim writeFooter As Boolean
            Dim strChangeType As String = String.Empty
            Dim changingStores As Boolean
            Dim changingBatches As Boolean

            'manage store config errors
            Dim storeConfigErrorMsg As New StringBuilder
            Dim currentStoreConfigError As StringBuilder
            Dim configErrorStores As New Hashtable

            'IRMA to Icon specific variables
            Dim iConStaging As DataTable = GetIconPOSPushStagingTable()
            Dim iConChangeTypeList As DataTable = GetIconChangeTypeTable()
            Dim iConPOSChangeType As String = String.Empty
            Dim currentPOSFileWriterClass As String = String.Empty
            Dim isR10Writer As Boolean = False
            Dim counter As Integer = 0
            Dim writeToStaging As Boolean = False

            ' From POS Push Datareader:
            Dim priceBatchHeaderID As Integer               ' PriceBatchHeaderID
            Dim priceBatchDetailID As Integer               ' PriceBatchDetailID
            Dim store_No As Integer                         ' Store_No
            Dim item_Key As Integer                         ' Item_Key
            Dim identifier As String                        ' Identifier
            Dim posChangeType As ChangeType = chgType
            Dim insertDate As Date = Now
            Dim retailSize? As Decimal                      ' Package_Desc2
            Dim retailUom As String                         ' Package_Unit_Abbr
            Dim tmDiscountEligible? As Boolean              ' Discountable? 
            Dim caseDiscount? As Boolean                    ' Case_Discount
            Dim ageCode? As Integer                         ' AgeCode
            Dim recallFlag? As Boolean                      ' Recall_Flag
            Dim restrictedHours? As Boolean                 ' Restricted_Hours
            Dim soldByWeight? As Boolean                    ' Sold_By_Weight
            Dim scaleForcedTare? As Boolean                 ' ScaleForcedTare
            Dim quantityRequired? As Boolean                ' Quantity_Required
            Dim priceRequired? As Boolean                   ' Price_Required
            Dim qtyProhibit? As Boolean                     ' QtyProhibit_Boolean
            Dim visualVerify? As Boolean                    ' VisualVerify
            Dim restrictSale? As Boolean                    ' NotAuthorizedForSale
            Dim price? As Decimal                           ' Price
            Dim salePrice? As Decimal                       ' Sale_Price
            Dim saleStartDate? As Date                      ' Sale_Start_Date
            Dim saleEndDate? As Date                        ' Sale_End_Date
            Dim onSale As Boolean                           ' On_Sale
            Dim newItem As Boolean                          ' New_Item
            Dim itemChange As Boolean                       ' Item_Change
            Dim priceChange As Boolean                      ' Price_Change
            Dim retailSale As Boolean                       ' Retail_Sale
            Dim multiple As Integer                         ' Multiple
            Dim sale_Multiple As Integer                    ' Sale_Multiple
            Dim linkCode_ItemIdentifier As String           ' LinkCode_Identifier
            Dim posTare? As Integer                         ' POS Tare
            Dim cancelAllSales As Boolean                   ' CancelAllSales
            Dim hasTprAndRegChange As Boolean               ' Has TPR + REG Price change
            Dim newRegPrice? As Decimal                     ' New REG Price for TPR+REG price change

            'POS change types
            Select Case chgType
                Case ChangeType.ItemDataChange
                    If IsItemNonBatchableChange Then
                        isBatchChange = False
                    Else
                        isBatchChange = True
                    End If
                Case ChangeType.ItemDataDelete
                    isBatchChange = True
                Case ChangeType.PromoOffer
                    isBatchChange = True
            End Select

            ' Retrieve store pos writers
            POSFileWriterList = POSWriterDAO.GetPOSFileWriterClass()

            ' results contains an entry for each change record that should be added to the POS Push file
            logger.Debug("WriteResultsToFile - adding detail records to the POS push file: changeType=" + chgType.ToString)
            While results.Read()
                isChangeTypeConfigDataError = False

                ' get the store # for this record
                currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))

                ' get file writer class for the current store
                currentPOSFileWriterClass = GetPOSFileWriterClass(currentStoreNum)

                If isBatchChange Then
                    'get current batch ID if one exists
                    currentBatchID = results.GetInt32(results.GetOrdinal("PriceBatchHeaderID"))
                End If

                ' Vendor ID Adds do not include item-level information in the result set, so the check for retail sale would be invalid.
                If chgType <> ChangeType.VendorIDAdd Then
                    If results.IsDBNull(results.GetOrdinal("Retail_Sale")) Then
                        retailSale = False
                    Else
                        retailSale = CBool(results.GetBoolean(results.GetOrdinal("Retail_Sale")))
                    End If
                End If

                ' verify this store is configured in StorePOSConfig
                If (storeUpdates.ContainsKey(currentStoreNum)) Then
                    ' Did we change stores?  If so, complete the processing for the previous store & initialize the
                    ' objects for the current store being processed.  If this change type contains batches, also
                    ' check for existence of new batch.
                    changingStores = previousStoreNum <> currentStoreNum
                    changingBatches = isBatchChange AndAlso (previousBatchID <> currentBatchID)

                    currentStoreUpdate = CType(storeUpdates(currentStoreNum), StoreUpdatesBO)

                    If changingStores OrElse changingBatches Then
                        If writeFooter AndAlso
                            ((changingStores AndAlso previousStoreNum <> -1) OrElse
                             (changingBatches AndAlso previousBatchID <> -1 AndAlso currentPosWriter.OutputByIrmaBatches)) Then
                            ' complete the previous store/batch - add the footer line
                            ' currentPosWriter is still set to the writer for the previousStoreNum and previousBatchID
                            If currentPosWriter.SupportsChangeType(chgType) Then
                                logger.Debug("WriteResultsToFile - adding footer to file: changingStores=" + changingStores.ToString + ", changingBatches=" + changingBatches.ToString)

                                currentPosWriter.AddFooterToFile(chgType, footerInfo)
                            End If
                        End If

                        ' get the writer instance for the store
                        currentPosWriter = CType(currentStoreUpdate.FileWriter, POSWriter)

                        ' **** IRMA POS Push to ICON related changes - Begin

                        ' Only the following change types will be included in the staging table:
                        ' ChangeType.ItemIdAdd
                        ' ChangeType.ItemIdDelete
                        ' ChangeType.ItemDataDelete
                        ' ChangeType.ItemDataChange
                        ' ChangeType.ItemDataDeAuth
                        ' ChangeType.ItemRefresh
                        Dim isR10ChangeType As Boolean

                        Select Case chgType
                            Case ChangeType.ItemDataDeAuth,
                                 ChangeType.ItemIdAdd,
                                 ChangeType.ItemIdDelete,
                                 ChangeType.ItemRefresh,
                                 ChangeType.ItemDataChange,
                                 ChangeType.ItemDataDelete
                                isR10ChangeType = True
                            Case Else
                                isR10ChangeType = False
                        End Select

                        ' is currentPOSFileWriterClass a R10 writer?
                        If InStr(currentPOSFileWriterClass, "R10") > 0 Then
                            isR10Writer = True
                        Else
                            isR10Writer = False
                        End If

                        writeToStaging = isR10Writer And isR10ChangeType And Not IsItemNonBatchableChange

                        If writeToStaging Then
                            Try
                                Select Case chgType
                                    Case ChangeType.ItemDataChange
                                        hasTprAndRegChange = False
                                        newRegPrice = 0D

                                        If results.IsDBNull(results.GetOrdinal("On_Sale")) Then
                                            onSale = False
                                        Else
                                            onSale = CBool(results.GetBoolean(results.GetOrdinal("On_Sale")))

                                            If onSale = True Then
                                                If Not results.IsDBNull(results.GetOrdinal("NewRegPrice")) Then
                                                    hasTprAndRegChange = True
                                                    newRegPrice = results.GetDecimal(results.GetOrdinal("NewRegPrice"))
                                                End If
                                            End If
                                        End If

                                        If results.IsDBNull(results.GetOrdinal("New_Item")) Then
                                            newItem = False
                                        Else
                                            newItem = CBool(results.GetByte(results.GetOrdinal("New_Item")))
                                        End If

                                        If results.IsDBNull(results.GetOrdinal("Item_Change")) Then
                                            itemChange = False
                                        Else
                                            itemChange = CBool(results.GetByte(results.GetOrdinal("Item_Change")))
                                        End If

                                        If results.IsDBNull(results.GetOrdinal("Price_Change")) Then
                                            priceChange = False
                                        Else
                                            priceChange = CBool(results.GetByte(results.GetOrdinal("Price_Change")))
                                        End If

                                        If results.IsDBNull(results.GetOrdinal("CancelAllSales")) Then
                                            cancelAllSales = False
                                        Else
                                            cancelAllSales = CBool(results.GetBoolean(results.GetOrdinal("CancelAllSales")))
                                        End If

                                        ' For a new item, New_Item = 1
                                        If newItem Then
                                            iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.ItemLocaleAttributeChange))
                                        End If

                                        ' Check to see if an item is on sale (non-regular price change)
                                        If onSale Then
                                            iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.NonRegularPriceChange))
                                            If hasTprAndRegChange = True Then
                                                iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.RegularPriceChange))
                                            End If
                                        Else
                                            ' If priceChange = 1, then it's regular price change
                                            If priceChange Then
                                                If cancelAllSales Then
                                                    iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.CancelAllSales))

                                                    Dim regPriceChanging As Boolean = CBool(results.GetBoolean(results.GetOrdinal("RegPriceChanging")))
                                                    If regPriceChanging Then
                                                        iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.RegularPriceChange))
                                                    End If
                                                Else
                                                    iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.RegularPriceChange))
                                                End If
                                            End If
                                        End If

                                        ' For item locale attribute changes, Item_Change = 1
                                        If itemChange Then
                                            iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.ItemLocaleAttributeChange))
                                        End If

                                    Case ChangeType.ItemIdAdd
                                        iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.ScanCodeAdd))

                                    Case ChangeType.ItemIdDelete
                                        iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.ScanCodeDelete))

                                    Case ChangeType.ItemDataDeAuth
                                        iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.ScanCodeDeauthorization))

                                    Case ChangeType.ItemDataDelete
                                        iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.ScanCodeDelete))

                                    Case ChangeType.ItemRefresh
                                        iConChangeTypeList.Rows.Add([Enum].GetName(GetType(IconChangeType), IconChangeType.ScanCodeAdd))
                                End Select

                                If results.IsDBNull(results.GetOrdinal("Multiple")) Then
                                    multiple = Nothing
                                Else
                                    multiple = CInt(results.GetByte(results.GetOrdinal("Multiple")))
                                End If

                                If results.IsDBNull(results.GetOrdinal("Sale_Multiple")) Then
                                    sale_Multiple = Nothing
                                Else
                                    sale_Multiple = CInt(results.GetByte(results.GetOrdinal("Sale_Multiple")))
                                End If

                                'Adding POS data to temp table
                                If results.IsDBNull(results.GetOrdinal("PriceBatchHeaderID")) Then
                                    priceBatchHeaderID = 0
                                Else
                                    priceBatchHeaderID = results.GetInt32(results.GetOrdinal("PriceBatchHeaderID"))
                                End If

                                If results.IsDBNull(results.GetOrdinal("PriceBatchDetailID")) Then
                                    priceBatchDetailID = 0
                                Else
                                    priceBatchDetailID = results.GetInt32(results.GetOrdinal("PriceBatchDetailID"))
                                End If

                                store_No = results.GetInt32(results.GetOrdinal("Store_No"))
                                item_Key = results.GetInt32(results.GetOrdinal("Item_Key"))
                                identifier = results.GetString(results.GetOrdinal("Identifier"))
                                retailSize = results.GetDecimal(results.GetOrdinal("Package_Desc2"))
                                retailUom = results.GetString(results.GetOrdinal("Package_Unit_Abbr"))

                                If results.IsDBNull(results.GetOrdinal("Price")) Then
                                    price = Nothing
                                Else
                                    price = results.GetDecimal(results.GetOrdinal("Price"))
                                End If

                                If results.IsDBNull(results.GetOrdinal("Discountable")) Then
                                    tmDiscountEligible = False
                                Else
                                    tmDiscountEligible = results.GetBoolean(results.GetOrdinal("Discountable"))
                                End If

                                If results.IsDBNull(results.GetOrdinal("IBM_Discount")) Then
                                    caseDiscount = False
                                Else
                                    caseDiscount = results.GetBoolean(results.GetOrdinal("IBM_Discount"))
                                End If

                                If results.IsDBNull(results.GetOrdinal("AgeCode")) Then
                                    ageCode = Nothing
                                Else
                                    ageCode = results.GetInt32(results.GetOrdinal("AgeCode"))

                                    If ageCode = 0 Then
                                        ageCode = Nothing
                                    End If
                                End If

                                If results.IsDBNull(results.GetOrdinal("Recall_Flag")) Then
                                    recallFlag = Nothing
                                Else
                                    recallFlag = results.GetBoolean(results.GetOrdinal("Recall_Flag"))
                                End If

                                If results.IsDBNull(results.GetOrdinal("Restricted_Hours")) Then
                                    restrictedHours = False
                                Else
                                    restrictedHours = results.GetBoolean(results.GetOrdinal("Restricted_Hours"))
                                End If

                                ' Evaulate the RetailUnit_WeightUnit field to determine if an item is sold by weight
                                ' (sending By Weight to R10)
                                If results.IsDBNull(results.GetOrdinal("RetailUnit_WeightUnit")) Then
                                    soldByWeight = False
                                Else

                                    Select Case results.GetFieldType(results.GetOrdinal("RetailUnit_WeightUnit"))
                                        Case GetType(Boolean)
                                            soldByWeight = results.GetBoolean(results.GetOrdinal("RetailUnit_WeightUnit"))
                                        Case GetType(Integer)
                                            If results.GetInt32(results.GetOrdinal("RetailUnit_WeightUnit")) = 1 Then
                                                soldByWeight = True
                                            Else
                                                soldByWeight = False
                                            End If
                                        Case Else
                                            soldByWeight = False
                                    End Select
                                End If

                                Select Case chgType
                                    Case ChangeType.ItemDataDeAuth,
                                        ChangeType.ItemIdAdd,
                                        ChangeType.ItemIdDelete,
                                        ChangeType.ItemRefresh

                                        If results.IsDBNull(results.GetOrdinal("ForceTare")) Then
                                            scaleForcedTare = False
                                        Else
                                            scaleForcedTare = results.GetBoolean(results.GetOrdinal("ForceTare"))
                                        End If
                                    Case Else
                                        If results.IsDBNull(results.GetOrdinal("ScaleForcedTare")) Then
                                            scaleForcedTare = Nothing
                                        Else
                                            If results.GetString(results.GetOrdinal("ScaleForcedTare")) = "Y" Then
                                                scaleForcedTare = True
                                            Else
                                                scaleForcedTare = False
                                            End If
                                        End If
                                End Select

                                If results.IsDBNull(results.GetOrdinal("Quantity_Required")) Then
                                    quantityRequired = False
                                Else
                                    quantityRequired = results.GetBoolean(results.GetOrdinal("Quantity_Required"))
                                End If

                                If results.IsDBNull(results.GetOrdinal("Price_Required")) Then
                                    priceRequired = False
                                Else
                                    priceRequired = results.GetBoolean(results.GetOrdinal("Price_Required"))
                                End If

                                If results.IsDBNull(results.GetOrdinal("QtyProhibit_Boolean")) Then
                                    qtyProhibit = False
                                Else
                                    qtyProhibit = results.GetBoolean(results.GetOrdinal("QtyProhibit_Boolean"))
                                End If

                                If results.IsDBNull(results.GetOrdinal("VisualVerify")) Then
                                    visualVerify = False
                                Else
                                    If results.GetString(results.GetOrdinal("VisualVerify")) = "Y" Then
                                        visualVerify = True
                                    Else
                                        visualVerify = False
                                    End If
                                End If

                                If results.IsDBNull(results.GetOrdinal("NotAuthorizedForSale")) Then
                                    restrictSale = Nothing
                                Else
                                    If results.GetString(results.GetOrdinal("NotAuthorizedForSale")) = "Y" Then
                                        restrictSale = True
                                    Else
                                        restrictSale = False
                                    End If
                                End If

                                If results.IsDBNull(results.GetOrdinal("Sale_Price")) Then
                                    salePrice = Nothing
                                Else
                                    salePrice = results.GetDecimal(results.GetOrdinal("Sale_Price"))
                                    If salePrice = 0.0 Then
                                        salePrice = Nothing
                                    End If
                                End If

                                Select Case chgType
                                    Case ChangeType.ItemDataDeAuth,
                                        ChangeType.ItemIdAdd,
                                        ChangeType.ItemIdDelete,
                                        ChangeType.ItemRefresh

                                        If results.IsDBNull(results.GetOrdinal("StartDate")) Then
                                            saleStartDate = Nothing
                                        Else
                                            saleStartDate = results.GetDateTime(results.GetOrdinal("StartDate"))
                                        End If
                                    Case Else
                                        If results.IsDBNull(results.GetOrdinal("Sale_Start_Date")) Then
                                            saleStartDate = Nothing
                                        Else
                                            saleStartDate = results.GetDateTime(results.GetOrdinal("Sale_Start_Date"))
                                        End If
                                End Select

                                If results.IsDBNull(results.GetOrdinal("Sale_End_Date")) Then
                                    saleEndDate = Nothing
                                Else
                                    saleEndDate = results.GetDateTime(results.GetOrdinal("Sale_End_Date"))
                                End If

                                If results.IsDBNull(results.GetOrdinal("LinkCode_ItemIdentifier")) Then
                                    linkCode_ItemIdentifier = Nothing
                                Else
                                    linkCode_ItemIdentifier = results.GetString(results.GetOrdinal("LinkCode_ItemIdentifier"))
                                End If

                                If results.IsDBNull(results.GetOrdinal("POSTare")) Then
                                    posTare = Nothing
                                Else
                                    posTare = results.GetInt32(results.GetOrdinal("POSTare"))
                                End If

                                For counter = 0 To iConChangeTypeList.Rows.Count - 1
                                    iConPOSChangeType = iConChangeTypeList.Rows(counter).Item("IconChangeType").ToString


                                    ' If TPR & REG are combined, newRegPrice is used. In addition, any TPR info will be removed for the 
                                    ' RegularPriceChange type.
                                    If hasTprAndRegChange And iConPOSChangeType = "RegularPriceChange" Then
                                        iConStaging.Rows.Add(priceBatchHeaderID, priceBatchDetailID, store_No, item_Key, identifier, iConPOSChangeType,
                                                insertDate, retailSize, retailUom, tmDiscountEligible,
                                                caseDiscount, ageCode, recallFlag, restrictedHours,
                                                soldByWeight, scaleForcedTare, quantityRequired, priceRequired,
                                                qtyProhibit, visualVerify, restrictSale, newRegPrice, multiple,
                                                sale_Multiple, Nothing, Nothing, Nothing, linkCode_ItemIdentifier,
                                                posTare)
                                    ElseIf hasTprAndRegChange And iConPOSChangeType = "NonRegularPriceChange" Then
                                        iConStaging.Rows.Add(priceBatchHeaderID, priceBatchDetailID, store_No, item_Key, identifier, iConPOSChangeType,
                                                    insertDate, retailSize, retailUom, tmDiscountEligible,
                                                    caseDiscount, ageCode, recallFlag, restrictedHours,
                                                    soldByWeight, scaleForcedTare, quantityRequired, priceRequired,
                                                    qtyProhibit, visualVerify, restrictSale, newRegPrice, multiple,
                                                    sale_Multiple, salePrice, saleStartDate, saleEndDate, linkCode_ItemIdentifier,
                                                    posTare)
                                    Else
                                        iConStaging.Rows.Add(priceBatchHeaderID, priceBatchDetailID, store_No, item_Key, identifier, iConPOSChangeType,
                                                insertDate, retailSize, retailUom, tmDiscountEligible,
                                                caseDiscount, ageCode, recallFlag, restrictedHours,
                                                soldByWeight, scaleForcedTare, quantityRequired, priceRequired,
                                                qtyProhibit, visualVerify, restrictSale, price, multiple,
                                                sale_Multiple, salePrice, saleStartDate, saleEndDate, linkCode_ItemIdentifier,
                                                posTare)
                                    End If

                                Next

                                ' Remove all Change Type from the list  
                                iConChangeTypeList.Clear()

                                ' Push the data to the staging table for every 2000 records added. 
                                If iConStaging.Rows.Count > 2000 Then
                                    POSWriterDAO.PopulateIconPOSPushStagingTable(iConStaging)
                                    iConStaging.Clear()
                                End If

                            Catch ex As Exception
                                logger.Error("PopulateIconPOSPushStaging Error in POSProcessor: Adding rows to temp table.", ex)
                                Throw
                            End Try
                        End If
                        ' **** IRMA POS Push to ICON related changes - End

                        logger.Debug("WriteResultsToFile - processing currentStoreNum=" + currentStoreNum.ToString + ", currentPosWriter.POSFileWriterKey=" + currentPosWriter.POSFileWriterKey.ToString)

                        If Not currentPosWriter.SupportsChangeType(chgType) Or isR10Writer Then
                            'change type by this writer or the write type is not supported, so don't create an error
                            logger.Debug("WriteResultsToFile - record being skipped because the writer does not support this change type: chgType=" + chgType.ToString + ", currentPosWriter.POSFileWriterKey=" + currentPosWriter.POSFileWriterKey.ToString)
                            writeFooter = False
                            Continue While
                        End If

                        'get header and footer info for current store/batch.
                        'also make sure config data is setup for this change type before attempting to add data to file
                        Select Case chgType
                            Case ChangeType.ItemDataChange
                                If IsItemNonBatchableChange Then
                                    headerInfo = New POSBatchHeaderBO
                                    headerInfo.StoreNo = currentStoreNum
                                    headerInfo.BatchDate = Now
                                    headerInfo.BatchDesc = "ITEM DATA CHANGE" + headerInfo.BatchDate.ToString("MM/dd")
                                Else
                                    headerInfo = CType(currentStoreUpdate.POSPriceChangeHeaders.Item(currentBatchID), POSBatchHeaderBO)
                                End If

                                If currentPosWriter.ItemDataChangeConfig Is Nothing OrElse currentPosWriter.ItemDataChangeConfig.Count <= 0 Then
                                    strChangeType = "ITEM DATA CHANGE"
                                    isChangeTypeConfigDataError = True
                                End If
                            Case ChangeType.ItemDataDelete
                                headerInfo = CType(currentStoreUpdate.POSDeleteItemHeaders.Item(currentBatchID), POSBatchHeaderBO)

                                If currentPosWriter.ItemDataDeleteConfig Is Nothing OrElse currentPosWriter.ItemDataDeleteConfig.Count <= 0 Then
                                    strChangeType = "ITEM DATA DELETE"
                                    isChangeTypeConfigDataError = True
                                End If

                            Case ChangeType.ItemDataDeAuth
                                headerInfo = New POSBatchHeaderBO
                                headerInfo.StoreNo = currentStoreNum
                                headerInfo.BatchDate = Now
                                headerInfo.BatchDesc = "ITEM DE-AUTH " + headerInfo.BatchDate.ToString("MM/dd")

                                ' de-auth records use the configuration for item deletes
                                If currentPosWriter.ItemDataDeleteConfig Is Nothing OrElse currentPosWriter.ItemDataDeleteConfig.Count <= 0 Then
                                    strChangeType = "ITEM DE-AUTH (USES ITEM DATA DELETE CONFIGURATION)"
                                    isChangeTypeConfigDataError = True
                                End If

                            Case ChangeType.ItemIdAdd
                                headerInfo = New POSBatchHeaderBO
                                headerInfo.StoreNo = currentStoreNum
                                headerInfo.BatchDate = Now
                                headerInfo.BatchDesc = "ITEM ID ADD " + headerInfo.BatchDate.ToString("MM/dd")

                                If currentPosWriter.ItemIdAddConfig Is Nothing OrElse currentPosWriter.ItemIdAddConfig.Count <= 0 Then
                                    strChangeType = "ITEM ID ADD"
                                    isChangeTypeConfigDataError = True
                                End If

                            Case ChangeType.ItemRefresh
                                headerInfo = New POSBatchHeaderBO
                                headerInfo.StoreNo = currentStoreNum
                                headerInfo.BatchDate = Now
                                headerInfo.BatchDesc = "ITEM REFRESH " + headerInfo.BatchDate.ToString("MM/dd")
                                If currentPosWriter.ItemIdAddConfig Is Nothing OrElse currentPosWriter.ItemIdAddConfig.Count <= 0 Then
                                    strChangeType = "ITEM REFRESH"
                                    isChangeTypeConfigDataError = True
                                End If

                            Case ChangeType.ItemIdDelete
                                headerInfo = New POSBatchHeaderBO
                                headerInfo.StoreNo = currentStoreNum
                                headerInfo.BatchDate = Now
                                headerInfo.BatchDesc = "ITEM ID DELETE " + headerInfo.BatchDate.ToString("MM/dd")

                                If currentPosWriter.ItemIdDeleteConfig Is Nothing OrElse currentPosWriter.ItemIdDeleteConfig.Count <= 0 Then
                                    strChangeType = "ITEM ID DELETE"
                                    isChangeTypeConfigDataError = True
                                End If

                            Case ChangeType.PromoOffer
                                headerInfo = CType(currentStoreUpdate.POSPromoOfferHeaders.Item(currentBatchID), POSBatchHeaderBO)

                                If currentPosWriter.PromoOfferConfig Is Nothing OrElse currentPosWriter.PromoOfferConfig.Count <= 0 Then
                                    strChangeType = "PROMO OFFER"
                                    isChangeTypeConfigDataError = True
                                End If

                                If footerInfo Is Nothing Then
                                    footerInfo = New POSBatchFooterBO

                                    'load up PIRUS_Writer specific value if contained within result set
                                    If results.GetValue(results.GetOrdinal("PIRUS_StartDate")).GetType IsNot GetType(DBNull) Then
                                        footerInfo.PIRUS_StartDate = results.GetInt32(results.GetOrdinal("PIRUS_StartDate"))
                                    End If
                                End If

                            Case ChangeType.VendorIDAdd
                                headerInfo = New POSBatchHeaderBO
                                headerInfo.StoreNo = currentStoreNum
                                headerInfo.BatchDate = Now
                                headerInfo.BatchDesc = "VENDOR ID ADD " + headerInfo.BatchDate.ToString("MM/dd")

                                If currentPosWriter.VendorIdAddConfig Is Nothing OrElse currentPosWriter.VendorIdAddConfig.Count <= 0 Then
                                    strChangeType = "VENDOR ID ADD"
                                    isChangeTypeConfigDataError = True
                                End If
                        End Select

                        ' Check to see if the default POS Batch ID needs to be set based on the config data
                        headerInfo.PopulateDefaultPOSBatchId(currentPosWriter.POSFileWriterKey, chgType)

                        If isChangeTypeConfigDataError Then
                            writeFooter = False

                            'build config error message for store/change type combo so only 1 error message is sent for this POSPush run
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

                        Try
                            ' append the file header info for this change
                            logger.Debug("WriteResultsToFile - adding header record to the file")
                            currentPosWriter.AddHeaderToFile(chgType, currentStoreUpdate.BatchFileName, headerInfo)
                        Catch ex As Exception
                            ' If an exception is thrown during processing, make sure that the file handler is closed
                            ' before throwing the exception on up for further error handling
                            '  Get Price batch header ID
                            logger.Error("WriteResultsToFile - error when adding the header record to the file PriceBatchHeaderID=" & currentBatchID.ToString(), ex)
                            currentPosWriter.CloseTempFile()
                            Throw
                        End Try

                        'reset record count to get total records for current change type being written
                        currentPosWriter.RecordCount = 0
                    End If
                Else
                    logger.Warn("Error processing WriteResultsToFile - Store # not configured in StorePOSConfig table: " & currentStoreNum.ToString)

                    'send message about exception
                    Dim args(1) As String
                    args(0) = currentStoreNum.ToString
                    ErrorHandler.ProcessError(ErrorType.POSPush_StoreNotFound, args, SeverityLevel.Warning)

                    currentStoreUpdate = Nothing
                    currentStoreNum = -1

                    Continue While
                End If

                ' track ID data for updates to DB in ApplyChangesInIRMA
                Select Case chgType
                    Case ChangeType.ItemIdAdd
                        PopulateApplyChangesData(currentStoreUpdate, results)
                    Case ChangeType.ItemIdDelete
                        PopulateApplyChangesData(currentStoreUpdate, results)
                    Case ChangeType.VendorIDAdd
                        PopulateApplyChangesData(currentStoreUpdate, results)
                    Case ChangeType.ItemDataDelete
                        PopulateApplyChangesData(currentStoreUpdate, results)
                    Case ChangeType.ItemDataDeAuth
                        PopulateApplyChangesData(currentStoreUpdate, results)
                    Case ChangeType.ItemRefresh
                        PopulateApplyChangesData(currentStoreUpdate, results)
                End Select

                ' Append this type of change to the POS file for the store, but only for retail sale items.
                Try
                    If retailSale Or chgType = ChangeType.VendorIDAdd Then
                        logger.Debug("Adding item record to the file.")

                        If Not currentPosWriter.IsOpen() Then
                            currentPosWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                            currentPosWriter.AddRecordToFile(chgType, currentStoreUpdate, results)
                            currentPosWriter.CloseTempFile()
                        Else
                            currentPosWriter.AddRecordToFile(chgType, currentStoreUpdate, results)
                        End If

                        ' track num of lines written to file
                        currentPosWriter.RecordCount += 1
                    Else
                        logger.Debug("Skipping item record because it is not retail sale.")
                    End If

                Catch ex As Exception
                    ' If an exception is thrown during processing, make sure that the file handler is closed
                    ' before throwing the exception on up for further error handling
                    ' Update the debug statements for the exception
                    _debugStoreNo = currentStoreUpdate.StoreNum
                    _debugSuccessLinesCount = currentPosWriter.RecordCount
                    _debugErrorColNum = currentPosWriter.ProcessingCol
                    _debugErrorRowNum = currentPosWriter.ProcessingRow
                    _debugFieldId = currentPosWriter.ProcessingFieldId

                    ' Add more logging information
                    Dim errorStr As New StringBuilder("** POS PUSH FILE ERROR: Current Store = ")
                    errorStr.Append(currentStoreUpdate.StoreNum)
                    errorStr.Append(", Successful Lines Written = ")
                    errorStr.Append(currentPosWriter.RecordCount)
                    errorStr.Append(", Processing Row = ")
                    errorStr.Append(currentPosWriter.ProcessingRow)
                    errorStr.Append(", Processing Col = ")
                    errorStr.Append(currentPosWriter.ProcessingCol)
                    errorStr.Append(", Processing Field Id = ")
                    errorStr.Append(currentPosWriter.ProcessingFieldId)

                    logger.Error(errorStr.ToString, ex)
                    currentPosWriter.CloseTempFile()
                    Throw
                End Try

                previousStoreNum = currentStoreNum

                If isBatchChange Then
                    previousBatchID = currentBatchID
                End If
            End While

            'Populating the IconStaging table.
            Try
                If iConStaging.Rows.Count > 0 Then
                    POSWriterDAO.PopulateIconPOSPushStagingTable(iConStaging)
                End If
                iConStaging = Nothing
                POSFileWriterList = Nothing
            Catch ex As Exception
                logger.Error("PopulateIconDenormTable Error in POSProcessor", ex)
                Throw
            End Try

            ' complete the processing for the final store that was processed - add the footer line
            If writeFooter AndAlso currentPosWriter IsNot Nothing Then
                Try
                    currentPosWriter.AddFooterToFile(chgType, footerInfo)
                Catch ex As Exception
                    ' If an exception is thrown during processing, make sure that the file handler is closed
                    ' before throwing the exception on up for further error handling
                    currentPosWriter.CloseTempFile()
                    Throw
                End Try
            End If

            'send message about store config exceptions
            If storeConfigErrorMsg.Length > 0 Then
                Dim args(1) As String
                args(0) = storeConfigErrorMsg.ToString
                ErrorHandler.ProcessError(ErrorType.POSPush_StoreConfig, args, SeverityLevel.Warning)
            End If

            logger.Debug("WriteResultsToFile exit")
        End Sub

        ''' <summary>
        ''' Updates the database records in IRMA for the type of change being handled by the subclass.
        ''' This method should be called after the POS Push file has been delivered to the POS system
        ''' for the store.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub ApplyChangesInIRMA(ByRef StoreUpdatesBO As Hashtable)

        ''' <summary>
        ''' Each record that is added to the POS Push file by WriteResultsToFile will contain key data,
        ''' such as database IDs, that need to be stored for ApplyChangesInIRMA.  The subclass will 
        ''' parse this data from the result record and add it to the StoreUpdatesBO object.
        ''' </summary>
        ''' <param name="currentStore"></param>
        ''' <param name="result"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub PopulateApplyChangesData(ByRef currentStore As StoreUpdatesBO, ByRef result As SqlDataReader)

        Public Function GetIconPOSPushStagingTable() As DataTable
            Dim TempTable As New DataTable

            With TempTable.Columns
                .Add("PriceBatchHeaderID", GetType(Integer))
                .Add("PriceBatchDetailID", GetType(Integer))
                .Add("Store_No", GetType(Integer))
                .Add("Item_Key", GetType(Integer))
                .Add("Identifier", GetType(String))
                .Add("ChangeType", GetType(String))
                .Add("InsertDate", GetType(Date))
                .Add("RetailSize", GetType(Decimal))
                .Add("RetailUOM", GetType(String))
                .Add("TMDiscountEligible", GetType(Boolean))
                .Add("Case_Discount", GetType(Boolean))
                .Add("AgeCode", GetType(Integer))
                .Add("Recall_Flag", GetType(Boolean))
                .Add("Restricted_Hours", GetType(Boolean))
                .Add("Sold_By_Weight", GetType(Boolean))
                .Add("ScaleForcedTare", GetType(Boolean))
                .Add("Quantity_Required", GetType(Boolean))
                .Add("Price_Required", GetType(Boolean))
                .Add("QtyProhibit", GetType(Boolean))
                .Add("VisualVerify", GetType(Boolean))
                .Add("RestrictSale", GetType(Boolean))
                .Add("Price", GetType(Double))
                .Add("Multiple", GetType(Integer))
                .Add("Sale_Multiple", GetType(Integer))
                .Add("Sale_Price", GetType(Double))
                .Add("Sale_Start_Date", GetType(Date))
                .Add("Sale_End_Date", GetType(Date))
                .Add("LinkCode_ItemIdentifier", GetType(String))
                .Add("POSTare", GetType(Integer))
            End With

            Return TempTable
        End Function

        Public Function GetIconChangeTypeTable() As DataTable
            Dim TempTable As New DataTable

            With TempTable.Columns
                .Add("IconChangeType", GetType(String))
            End With

            Return TempTable
        End Function

        Public Function GetPOSFileWriterClass(ByRef Store_No As Integer) As String
            Dim dr() As System.Data.DataRow
            Dim FileWriterClass As String = Nothing

            dr = POSFileWriterList.Select("Store_No=" & Store_No.ToString)

            If dr.Length > 0 Then
                FileWriterClass = dr(0)("POSFileWriterCode").ToString
            End If

            Return FileWriterClass
        End Function

        Public Function DuplicateRecordFound(ByRef StagingTable As DataTable, ByRef Store_No As Integer, ByRef Item_Key As Integer, ByRef Identifier As String, ByRef ChangeType As String) As Boolean
            Dim foundRows() As System.Data.DataRow
            Dim searchExpression As String

            searchExpression = "Store_No=" & Store_No.ToString & " AND Item_Key=" & Item_Key.ToString & " AND Identifier='" & Identifier.ToString & "' AND ChangeType ='" & ChangeType.ToString & "'"

            foundRows = StagingTable.Select(searchExpression)

            If foundRows.Length > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
