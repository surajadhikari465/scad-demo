Imports System.Text.RegularExpressions

Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win

Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.IRMA.ModelLayer.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Imports WholeFoods.IRMA.ExtendedItemMaintenance.Logic.Utilites

Namespace WholeFoods.IRMA.ExtendedItemMaintenance.Logic

#Region "Public Enums"

    Public Enum ValidationTypes
        GridCell
        UploadValue
    End Enum

#End Region

    Public Class CalculatorAndValidatorManager

#Region "Fields and Properties"

        Private _extendedItemMaintenanceManager As ExtendedItemMaintenanceManager
        Private _progressComplete As Boolean
        Private _progressCounter As Integer
        Private _lastValidatedDateTime As DateTime

        Private Property EIMManager() As ExtendedItemMaintenanceManager
            Get
                Return _extendedItemMaintenanceManager
            End Get
            Set(ByVal value As ExtendedItemMaintenanceManager)
                _extendedItemMaintenanceManager = value
            End Set
        End Property

        Public Property ProgressComplete() As Boolean
            Get
                Return _progressComplete
            End Get
            Set(ByVal value As Boolean)
                _progressComplete = value
            End Set
        End Property

        Public Property ProgressCounter() As Integer
            Get
                Return _progressCounter
            End Get
            Set(ByVal value As Integer)
                _progressCounter = value
            End Set
        End Property

        ''' <summary>
        ''' Used to determine if full validation must be run
        ''' before upload because we're in a new day since the
        ''' last full validation was run.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastValidatedDateTime() As DateTime
            Get
                Return _lastValidatedDateTime
            End Get
            Set(ByVal value As DateTime)
                _lastValidatedDateTime = value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New(ByRef inEIMManager As ExtendedItemMaintenanceManager)

            Me.EIMManager = inEIMManager

        End Sub

#End Region

#Region "Public Methods"

        Public Sub Validate(ByVal inValidationType As ValidationTypes, ByVal inJustStores As Boolean)

            Me.ProgressComplete = False
            Me.ProgressCounter = 0

            Try

                Me.EIMManager.CurrentUploadSession.DataSet.AcceptChanges()

                For Each theUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList

                    ValidateGridRow(theUploadRowHolder, inValidationType, inJustStores)

                    RefreshItemMaintenanceGridRows(theUploadRowHolder)

                    Me.ProgressCounter = Me.ProgressCounter + 1
                Next

                Me.EIMManager.CurrentUploadSession.DataSet.AcceptChanges()

                Me.LastValidatedDateTime = DateTime.Now

            Finally
                Me.ProgressComplete = True
                Me.ProgressCounter = 0
            End Try

        End Sub

        ''' <summary>
        ''' Refreshes the rows in the item grid when validation is done
        ''' so if the user has chosen to hide duplicate item rows (those with
        ''' price rows for more than one store) then the visible item grid row
        ''' is the one with the highest validation level (Error > Warning > Valid).
        ''' </summary>
        ''' <param name="inUploadRowHolder"></param>
        ''' <remarks></remarks>
        Private Sub RefreshItemMaintenanceGridRows(ByRef inUploadRowHolder As UploadRowHolder)
            Dim theUploadRowHoldersForSameItemList As ArrayList = _
                            Me.EIMManager.CurrentUploadRowHolderCollecton.GetUploadRowHolderListForIdentifier(inUploadRowHolder.UploadRow.Identifier)
            Dim theGridAndDataRowHolder As GridAndDataRowHolder

            For Each theOtherUploadRowHolder As UploadRowHolder In theUploadRowHoldersForSameItemList

                theGridAndDataRowHolder = theOtherUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.ITEM_MAINTENANCE_CODE)

                ' only if there are rows in the Item Maintenance grid
                If Not IsNothing(theGridAndDataRowHolder) Then
                    theGridAndDataRowHolder.GridRow.Refresh(RefreshRow.FireInitializeRow)
                End If
            Next

        End Sub

        Public Sub ValidateGridRow(ByRef inUploadRowHolder As UploadRowHolder, ByVal inValidationType As ValidationTypes, _
                ByVal inJustStores As Boolean)

            InternalValidateGridRow(inUploadRowHolder, inValidationType, inJustStores)

        End Sub

        ''' <summary>
        ''' Set the promo planner cell value to false
        ''' if the is price change cell value is false.
        ''' </summary>
        ''' <param name="inGridCell"></param>
        ''' <remarks></remarks>
        Public Sub UnCheckPromoPlannerCell(ByRef inGridCell As UltraGridCell, ByRef inDataSet As DataSet)

            Dim theDataRowArray As DataRow()
            Dim theGridRow As UltraGridRow = inGridCell.Row
            Dim theUploadRowId As Integer = CInt(theGridRow.Cells(EIM_Constants.UPLOADROW_ID_COLUMN_NAME).Value)
            Dim isPriceChange As Boolean = CBool(inGridCell.Value)

            If Not isPriceChange Then

                ' set the promo planner value to false for the row
                ' BTW, we are guaranteed the promo planner column is only in the prices table
                theDataRowArray = inDataSet.Tables(EIM_Constants.PRICE_UPLOAD_CODE).Select(EIM_Constants.UPLOADROW_ID_COLUMN_NAME + " = " + theUploadRowId.ToString())
                theDataRowArray(0).Item(EIM_Constants.PROMO_PLANNER_IS_FOR_PROMO_PLANNER_ATTR_KEY) = False

            End If

        End Sub


        ''' <summary>
        ''' Calculate margin when the cost, price, or multiple changes.
        ''' </summary>
        ''' <param name="inGridCell"></param>
        ''' <remarks></remarks>
        Public Sub CalculateMargin(ByRef inGridCell As UltraGridCell, ByRef inDataSet As DataSet)

            Dim theGridRow As UltraGridRow = inGridCell.Row
            Dim thePriceChangeId As Integer = 0
            Dim thePriceKey As String = Nothing
            Dim theMultipleKey As String = Nothing

            Dim thePrice As Decimal = 0
            Dim theBaseCost As Decimal = 0
            Dim theNetCost As Decimal = 0
            Dim theVendorPackageCount As Decimal = 0
            Dim theMultiple As Decimal = 0
            Dim theMargin As Decimal = 0
            Dim theUnitFreight As Decimal = 0
            Dim theNetDiscount As Decimal = 0

            Dim theUploadRowId As Integer = CInt(theGridRow.Cells(EIM_Constants.UPLOADROW_ID_COLUMN_NAME).Value)
            Dim theCurrentItemIdentifier As String = CStr(theGridRow.Cells(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY).Value)

            Dim theDataRow As DataRow
            Dim theDataRowsWithMarginList As New ArrayList()
            Dim theDataRowsWithNetCostList As New ArrayList()
            Dim hasEnoughDataForCalculation As Boolean = True
            Dim hasPriceChangeType As Boolean = False
            Dim hasBaseCost As Boolean = False
            Dim hasNetCost As Boolean = False
            Dim hasVendorPackageCount As Boolean = False
            Dim hasPrice As Boolean = False
            Dim hasMultiple As Boolean = False
            Dim hasUnitFreight As Boolean = False
            Dim hasNetDiscount As Boolean = False
            Dim theDataRowArray As DataRow()

            hasPriceChangeType = _
                FindIntegerValueInDataSetForUploadRow(inDataSet, theUploadRowId, EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY, thePriceChangeId)

            If hasPriceChangeType Then

                ' use the promo price and multiple if the price change type
                ' is non-reg
                If PriceChgType.IsPromoPriceChgType(thePriceChangeId) Then
                    thePriceKey = EIM_Constants.PRICE_PROMO_ATTR_KEY
                    theMultipleKey = EIM_Constants.PRICE_PROMO_MULTIPLE_ATTR_KEY
                Else
                    thePriceKey = EIM_Constants.PRICE_ATTR_KEY
                    theMultipleKey = EIM_Constants.PRICE_MULTIPLE_ATTR_KEY
                End If

                ' find the price, cost, and multiple values from whichever grids they may be in
                ' we are relying on the values being synced across the grids by now
                hasPrice = _
                    FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, thePriceKey, thePrice)

                hasBaseCost = _
                    FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, EIM_Constants.COST_PKG_COST_ATTR_KEY, theBaseCost)

                hasNetCost = _
                    FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, EIM_Constants.COST_NET_COST_ATTR_KEY, theNetCost)

                hasUnitFreight = _
                    FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, EIM_Constants.COST_UNIT_FREIGHT_ATTR_KEY, theUnitFreight)

                hasNetDiscount = _
                    FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, EIM_Constants.COST_DISCOUNT_ATTR_KEY, theNetDiscount)

                hasVendorPackageCount = _
                    FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, EIM_Constants.COST_VEND_PKG_DSCR_ATTR_KEY, theVendorPackageCount)

                hasMultiple = _
                    FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, theMultipleKey, theMultiple)


                For Each theDataTable As DataTable In inDataSet.Tables
                    ' hold on to all the dataRows with the margin in them to set later
                    If theDataTable.Columns.Contains(EIM_Constants.PRICE_MARGIN_ATTR_KEY) Then
                        ' get the rows from the DataTable for the item the calculation is for
                        theDataRowArray = theDataTable.Select(EIM_Constants.UPLOADROW_ID_COLUMN_NAME + " = " + theUploadRowId.ToString())

                        For Each theDataRow In theDataRowArray
                            theDataRowsWithMarginList.Add(theDataRow)
                        Next
                    End If

                    ' hold on to all the dataRows with the net cost in them to set later
                    If theDataTable.Columns.Contains(EIM_Constants.COST_NET_COST_ATTR_KEY) Then
                        ' get the rows from the DataTable for the item the calculation is for
                        theDataRowArray = theDataTable.Select(EIM_Constants.UPLOADROW_ID_COLUMN_NAME + " = " + theUploadRowId.ToString())

                        For Each theDataRow In theDataRowArray
                            theDataRowsWithNetCostList.Add(theDataRow)
                        Next
                    End If
                Next

                If hasPrice And hasBaseCost And hasNetCost And hasNetDiscount And _
                        hasUnitFreight And hasVendorPackageCount And hasMultiple Then

                    ' calculate the net cost
                    theNetCost = theBaseCost - theNetDiscount + theUnitFreight

                    ' only do the calculation if we have the right data
                    If thePrice = 0 Or theNetCost = 0 Or theVendorPackageCount = 0 Or theMultiple = 0 Then
                        theMargin = 0
                    Else
                        theMargin = (((thePrice / theMultiple) - (theNetCost / theVendorPackageCount)) * 100) / (thePrice / theMultiple)
                    End If

                    ' now update all datarows that have the net margin
                    For Each theDataRowWithMargin As DataRow In theDataRowsWithMarginList
                        theDataRowWithMargin.Item(EIM_Constants.PRICE_MARGIN_ATTR_KEY) = theMargin
                        theDataRowWithMargin.AcceptChanges()
                    Next

                    ' now update all datarows that have the net cost
                    For Each theDataRowWithNetCost As DataRow In theDataRowsWithNetCostList
                        theDataRowWithNetCost.Item(EIM_Constants.COST_NET_COST_ATTR_KEY) = theNetCost
                        theDataRowWithNetCost.AcceptChanges()
                    Next
                End If
            End If

        End Sub

        ''' <summary>
        ''' Calculate margin when the cost, price, or multiple changes.
        ''' </summary>
        ''' <param name="inGridCell"></param>
        ''' <remarks></remarks>
        Public Sub CalculatePrice(ByRef inGridCell As UltraGridCell, ByRef inDataSet As DataSet)

            Dim theGridRow As UltraGridRow = inGridCell.Row
            Dim thePriceChangeId As Integer = 0
            Dim thePriceKey As String = Nothing
            Dim theMultipleKey As String = Nothing

            Dim thePrice As Decimal = 0
            Dim theNetCost As Decimal = 0
            Dim theVendorPackageCount As Decimal = 0
            Dim theMultiple As Decimal = 0
            Dim theMargin As Decimal = 0

            Dim theUploadRowId As Integer = CInt(theGridRow.Cells(EIM_Constants.UPLOADROW_ID_COLUMN_NAME).Value)
            Dim theCurrentItemIdentifier As String = CStr(theGridRow.Cells(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY).Value)

            Dim theDataRow As DataRow
            Dim theDataRowsWithPriceList As New ArrayList()
            Dim hasPriceChangeType As Boolean = False
            Dim hasNetCost As Boolean = False
            Dim hasVendorPackageCount As Boolean = False
            Dim hasMargin As Boolean = False
            Dim hasMultiple As Boolean = False
            Dim theDataRowArray As DataRow()

            hasPriceChangeType = _
                FindIntegerValueInDataSetForUploadRow(inDataSet, theUploadRowId, EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY, thePriceChangeId)

            If hasPriceChangeType Then

                ' use the promo price and multiple if the price change type
                ' is non-reg
                If PriceChgType.IsPromoPriceChgType(thePriceChangeId) Then
                    thePriceKey = EIM_Constants.PRICE_PROMO_ATTR_KEY
                    theMultipleKey = EIM_Constants.PRICE_PROMO_MULTIPLE_ATTR_KEY
                Else
                    thePriceKey = EIM_Constants.PRICE_ATTR_KEY
                    theMultipleKey = EIM_Constants.PRICE_MULTIPLE_ATTR_KEY
                End If

                ' find the price, cost, and multiple values from whichever grids they may be in
                ' we are relying on the values being synced across the grids by now
                hasMargin = _
                    FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, EIM_Constants.PRICE_MARGIN_ATTR_KEY, theMargin)

                hasNetCost = _
                    FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, EIM_Constants.COST_NET_COST_ATTR_KEY, theNetCost)

                hasVendorPackageCount = _
                   FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, EIM_Constants.COST_VEND_PKG_DSCR_ATTR_KEY, theVendorPackageCount)

                hasMultiple = _
                     FindDecimalValueInDataSetForUploadRow(inDataSet, theUploadRowId, theMultipleKey, theMultiple)

                For Each theDataTable As DataTable In inDataSet.Tables
                    ' hold on to all the dataRows with the price in them to set later
                    ' only if the margin column is in the table
                    If theDataTable.Columns.Contains(thePriceKey) Then
                        ' get the rows from the DataTable for the item the calculation is for
                        theDataRowArray = theDataTable.Select(EIM_Constants.UPLOADROW_ID_COLUMN_NAME + " = " + theUploadRowId.ToString())

                        For Each theDataRow In theDataRowArray
                            theDataRowsWithPriceList.Add(theDataRow)
                        Next
                    End If
                Next

                If hasMargin And hasNetCost And hasVendorPackageCount And hasMultiple Then
                    ' only do the calculation if we have the right data
                    If theMargin = 100 Or theNetCost = 0 Or theVendorPackageCount = 0 Or theMultiple = 0 Then
                        thePrice = 0
                    Else
                        thePrice = Math.Round((theNetCost / theVendorPackageCount) / ((1 - (theMargin / 100)) * theMultiple), 2)
                    End If

                    ' now update all datarows that have the margin
                    For Each theDataRowWithPrice As DataRow In theDataRowsWithPriceList
                        theDataRowWithPrice.Item(thePriceKey) = thePrice
                        theDataRowWithPrice.AcceptChanges()
                    Next
                End If
            End If

        End Sub

#End Region

#Region "Shared Public Methods"

        Public Shared Function ValidateDataType(ByVal inValueString As String, ByVal theDbDataTypeName As String) As Boolean

            Dim isValid As Boolean = True

            If Not String.IsNullOrEmpty(inValueString) And Not IsNothing(inValueString) Then
                isValid = Not IsNothing(CalculatorAndValidatorManager.SafeCastByDbDataType(inValueString, theDbDataTypeName))
            End If

            Return isValid

        End Function

        Public Shared Function SafeCastByDbDataType(ByVal inValueString As String, ByVal theDbDataTypeName As String) As Object

            Dim theCastValue As Object = Nothing

            If Not String.IsNullOrEmpty(inValueString) And Not IsNothing(inValueString) Then
                If theDbDataTypeName.ToLower().Equals("char") Or _
                        theDbDataTypeName.ToLower().Equals("varchar") Then
                    theCastValue = inValueString
                ElseIf theDbDataTypeName.ToLower().Equals("smalldatetime") Then
                    Dim theThrowAwayDate As New DateTime()
                    If DateTime.TryParse(inValueString, theThrowAwayDate) Then

                        ' make sure the SQL Server small date is within the 
                        ' correct range of values
                        Dim theEarliestValidSmallDateTime As Date = Date.Parse("01/01/1900")
                        Dim theLatestValidSmallDateTime As Date = Date.Parse("06/06/2079")

                        If Not theThrowAwayDate.CompareTo(theEarliestValidSmallDateTime) < 0 And _
                                Not theThrowAwayDate.CompareTo(theLatestValidSmallDateTime) > 0 Then
                            theCastValue = theThrowAwayDate
                        End If
                    End If
                ElseIf theDbDataTypeName.ToLower().Equals("datetime") Then
                    Dim theThrowAwayDate As New DateTime()
                    If DateTime.TryParse(inValueString, theThrowAwayDate) Then
                        theCastValue = theThrowAwayDate
                    End If
                ElseIf theDbDataTypeName.ToLower().Equals("bit") Then
                    Dim theThrowAwayBoolean As Boolean
                    If Boolean.TryParse(inValueString, theThrowAwayBoolean) Then
                        theCastValue = theThrowAwayBoolean
                    End If
                ElseIf theDbDataTypeName.ToLower().Equals("int") Or _
                        theDbDataTypeName.ToLower().Equals("smallint") Or _
                        theDbDataTypeName.ToLower().Equals("tinyint") Then
                    Dim theThrowAwayInteger As Integer
                    If Integer.TryParse(inValueString, theThrowAwayInteger) Then
                        theCastValue = theThrowAwayInteger
                    End If
                ElseIf theDbDataTypeName.ToLower().Equals("decimal") Or _
                        theDbDataTypeName.ToLower().Equals("smallmoney") Or _
                        theDbDataTypeName.ToLower().Equals("money") Then
                    Dim theThrowAwayDouble As Double
                    If Double.TryParse(inValueString, theThrowAwayDouble) Then
                        theCastValue = theThrowAwayDouble
                    End If
                End If
            End If

            Return theCastValue

        End Function

        Public Shared Function SafeCastCompareValues(ByVal inValueString As String, ByVal inCompareToValueString As String, ByVal inDbDataTypeName As String) As Integer

            Dim theComparisonResult As Integer = 0
            Dim theCastValue As Object = CalculatorAndValidatorManager.SafeCastByDbDataType(inValueString, inDbDataTypeName)
            Dim theCastCompareToValue As Object = CalculatorAndValidatorManager.SafeCastByDbDataType(inCompareToValueString, inDbDataTypeName)

            If Not (IsNothing(theCastValue) Or _
                    IsNothing(theCastCompareToValue)) Then

                If inDbDataTypeName.ToLower().Equals("smalldatetime") Or _
                        inDbDataTypeName.ToLower().Equals("datetime") Then
                    theComparisonResult = CDate(theCastValue).CompareTo(CDate(inCompareToValueString))
                ElseIf inDbDataTypeName.ToLower().Equals("int") Or _
                        inDbDataTypeName.ToLower().Equals("tinyint") Then
                    theComparisonResult = CInt(theCastValue).CompareTo(CInt(inCompareToValueString))
                ElseIf inDbDataTypeName.ToLower().Equals("decimal") Or _
                        inDbDataTypeName.ToLower().Equals("smallmoney") Or _
                        inDbDataTypeName.ToLower().Equals("money") Then
                    theComparisonResult = CDec(theCastValue).CompareTo(CDec(inCompareToValueString))
                End If

            End If

            Return theComparisonResult

        End Function

#End Region

#Region "Private Methods"

#Region "Top-level Validation Methods"

        Private Sub InternalValidateGridRow(ByRef inUploadRowHolder As UploadRowHolder, ByVal inValidationType As ValidationTypes, _
                ByVal inJustStores As Boolean)

            Dim FourLevelHierarchyFlag As Boolean = InstanceDataDAO.IsFlagActiveCached("FourLevelHierarchy")
            Dim useStoreJurisdiction As Boolean = InstanceDataDAO.IsFlagActiveCached("UseStoreJurisdictions")

            ' the store only validation is only basic and has no
            ' attribute specific validation
            If Not inJustStores Then

                If useStoreJurisdiction Then
                    HandleDefaultJurisdictionFlag(inUploadRowHolder)
                End If

                HandleIsChangeFlags(inUploadRowHolder)
                HandlePrimaryVendorFlag(inUploadRowHolder)
                HandlePromoPlannerAttributes(inUploadRowHolder)
                HandleRandomWeightAttributes(inUploadRowHolder)
            End If

            For Each theUploadValue As UploadValue In inUploadRowHolder.UploadRow.UploadValueCollection

                If theUploadValue.UploadAttribute.IsAllowedForRegion() Then

                    ' just validate stores column if needed
                    ' this is done only when the user is changing the checked state of the 
                    ' "Upload Prices and Costs Back to Item's Store" check box
                    If Not inJustStores Or (inJustStores And theUploadValue.Key.Equals(EIM_Constants.STORE_NO_ATTR_KEY)) Then

                        ' do not validate level 3 or 4 if the region doesn't use 4 levels
                        If (FourLevelHierarchyFlag) Or _
                                (Not FourLevelHierarchyFlag And _
                                Not theUploadValue.Key.Equals(EIM_Constants.ITEM_LEVEL_3_ATTR_KEY) And _
                                Not theUploadValue.Key.Equals(EIM_Constants.ITEM_LEVEL_4_ATTR_KEY)) Then

                            ValidateBasic(theUploadValue, inUploadRowHolder, inValidationType)

                            If inJustStores Then
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next

            ' the store only validation is only basic and has no
            ' attribute specific validation
            If Not inJustStores Then
                ValidateAttributeSpecific(inUploadRowHolder)
            End If
            SetUploadExclusion(inUploadRowHolder)

            SetRowValidationLevel(inUploadRowHolder)
        End Sub

        ''' <summary>
        ''' Reset the flags that keep the item and linked item identifiers from
        ''' being validated when it is not needed.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ResetIdentifierValidationFlags()

            For Each theUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList
                theUploadRowHolder.UploadRow.HasValidatedIdentifier = False
                theUploadRowHolder.UploadRow.HasValidatedLinkedIdentifier = False
            Next

        End Sub

        Private Sub ValidateBasic(ByRef inUploadValue As UploadValue, ByRef inUploadRowHolder As UploadRowHolder, ByVal inValidationType As ValidationTypes)

            Dim clearCellStyle As Boolean
            Dim isValid As Boolean
            Dim theValidationMessage As String = ""
            Dim theValue As String
            Dim theValidationType As String = ""
            Dim theGridCell As UltraGridCell = Nothing

            For Each theGridAndDataRowHolder As GridAndDataRowHolder In inUploadRowHolder.GridAndDataRowList

                If GridUtilities.TryGetGridCell(inUploadValue.Key, theGridAndDataRowHolder.GridRow, theGridCell) Then

                    ' do not clear the cell error for the identifier cells if there are existing errors
                    ' because the validation that runs to determine if the identifier values
                    ' exist in IRMA only run when the values are first loaded or when they are
                    ' changed through data entry
                    If (inUploadValue.Key.Equals(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY) AndAlso inUploadRowHolder.ContainsValidationKey("Identifier")) Or _
                        (inUploadValue.Key.Equals(EIM_Constants.PRICE_LINKED_ITEM_IDENTIFIER_ATTR_KEY) AndAlso inUploadRowHolder.ContainsValidationKey("LinkedItemIdentifier")) Then

                        clearCellStyle = False
                    Else
                        clearCellStyle = True
                    End If

                    If clearCellStyle Then
                        ' clear any existing error
                        inUploadRowHolder.ClearValidationKey(inUploadValue.Key)

                        ' since basic validation runs first
                        ' the last parameter must be true to
                        ' clear out any previous errors
                        GridUtilities.SetCellStyle(theGridCell, "", EIM_Constants.ValidationLevels.Valid, True)
                    End If


                    ' validate only if the cell is not disabled
                    ' 3/7/2011 if class is uploaded from the spreadsheet 
                    ' and class doesn't match subteam, class is loaded as NULL
                    ' user should get an error and fix the class. Added "Or inUploadValue.ColumnNameOrKey = "category_id""

                    If theGridCell.Activation = Activation.AllowEdit Or inUploadValue.ColumnNameOrKey = "category_id" Then

                        If inValidationType = ValidationTypes.GridCell Then
                            theValue = GridUtilities.GetGridCellStringValue(theGridCell)
                        Else
                            theValue = inUploadValue.Value
                        End If

                        isValid = ValidateValueForValueList(theValidationMessage, theValue, inUploadValue, theGridCell)

                        If isValid Then
                            isValid = ValidateRequired(theValidationMessage, theValue, inUploadValue)
                            If Not isValid Then
                                theValidationType = "Requires Value"

                                ' for cost upload if delete_vendor or deauth_store flag is checked, turn off validation for vendor's item id. Bug 1338

                                If theGridAndDataRowHolder.UploadTypeCode = "COST_UPLOAD" And inUploadValue.ColumnNameOrKey = "item_id" Then

                                    Dim theGridRow As UltraGridRow = theGridCell.Row

                                    Dim Delete_Vendor As Integer = CInt(theGridRow.Cells(EIM_Constants.COST_DELETE_VENDOR).Value)
                                    Dim Deauth_Store As Integer = CInt(theGridRow.Cells(EIM_Constants.COST_DEAUTH_STORE).Value)


                                    If Delete_Vendor Or Deauth_Store Then

                                        isValid = True
                                        theValidationType = ""

                                    End If

                                End If



                            End If
                        Else
                            theValidationType = "Invalid"
                        End If

                        If isValid Then
                            isValid = ValidateStringValueForSize(theValidationMessage, theValue, inUploadValue)
                            If Not isValid Then
                                theValidationType = "Too Large"
                            End If
                        End If

                        If isValid Then
                            isValid = ValidateValueForDataType(theValidationMessage, theValue, inUploadValue)
                            If Not isValid Then
                                theValidationType = "Invalid " + UploadAttribute.MapFromDbDataType(inUploadValue.DbDataType).ToString() + " Value"
                            End If
                        End If

                        If isValid Then
                            isValid = ValidateRange(theValidationMessage, theValue, inUploadValue)
                            If Not isValid Then
                                theValidationType = "Invalid Range"
                            End If
                        End If

                        If Not isValid Then
                            inUploadRowHolder.AddValidationKey(inUploadValue.Key, inUploadValue.Name + " " + theValidationType, EIM_Constants.ValidationLevels.Invalid)
                            GridUtilities.SetCellStyle(theGridCell, theValidationMessage, EIM_Constants.ValidationLevels.Invalid)
                        End If
                    End If
                End If
            Next

        End Sub

        Private Sub ValidateAttributeSpecific(ByRef inUploadRowHolder As UploadRowHolder)

            ValidateItemIdentifier(inUploadRowHolder)
            ValidateLinkedItemIdentifier(inUploadRowHolder)
            ValidateCostDates(inUploadRowHolder)
            ValidateDiscountDates(inUploadRowHolder)
            ValidateAllowanceDates(inUploadRowHolder)
            ValidateByPriceChangeType(inUploadRowHolder)
            ValidateItemChains(inUploadRowHolder)
            ValidateScaleDigits(inUploadRowHolder)
            'ValidateVendorUOM(inUploadRowHolder)
            ValidateCostedByWeight(inUploadRowHolder)
            ValidateAverageUnitWeight(inUploadRowHolder)
        End Sub

#End Region

#Region "Basic Validation Methods"

        Private Function ValidateRequired(ByRef inValidationMessage As String, ByVal inValue As String, ByRef inUploadValue As UploadValue) As Boolean

            Dim isValid As Boolean = True

            If inUploadValue.IsRequiredValue And (IsNothing(inValue) Or String.IsNullOrEmpty(inValue)) Then
                isValid = False
                inValidationMessage = "The " + inUploadValue.Name + " is required."
            End If

            Return isValid

        End Function

        Private Function ValidateRange(ByRef inValidationMessage As String, ByVal inValue As String, ByRef inUploadValue As UploadValue) As Boolean

            Dim isValid As Boolean = True

            If Not inUploadValue.DbDataType.ToLower().Equals("varchar") Then

                If SafeCastCompareValues(inValue, inUploadValue.OptionalMinValue, inUploadValue.DbDataType) < 0 Then
                    isValid = False
                    inValidationMessage = "The " + inUploadValue.Name + " cannot be less than " + inUploadValue.OptionalMinValue
                End If

                If SafeCastCompareValues(inValue, inUploadValue.OptionalMaxValue, inUploadValue.DbDataType) > 0 Then
                    isValid = False

                    If inValidationMessage.Length > 0 Then
                        inValidationMessage = inValidationMessage + " or greater than " + inUploadValue.OptionalMaxValue
                    Else
                        inValidationMessage = "The " + inUploadValue.Name + " cannot be greater than " + inUploadValue.OptionalMaxValue
                    End If
                End If
            End If

            Return isValid

        End Function

        Private Function ValidateStringValueForSize(ByRef inValidationMessage As String, ByVal inValue As String, ByRef inUploadValue As UploadValue) As Boolean

            Dim isValid As Boolean = True

            If Not String.IsNullOrEmpty(inValue) And Not IsNothing(inValue) Then
                If inUploadValue.DbDataType.ToLower().Equals("varchar") Then
                    If inValue.Length > inUploadValue.Size Then
                        isValid = False
                        inValidationMessage = "The value is too large. Please reduce to " + inUploadValue.Size.ToString() + " characters or less."

                    End If
                End If
            End If

            Return isValid

        End Function

        Private Function ValidateValueForDataType(ByRef inValidationMessage As String, ByVal inValue As String, ByRef inUploadValue As UploadValue) As Boolean

            Dim isValid As Boolean = True

            If Not CalculatorAndValidatorManager.ValidateDataType(inValue, inUploadValue.DbDataType) Then
                isValid = False

                inUploadValue.Value = ""

                inValidationMessage = "The value """ + inValue + """ is not a valid " + inUploadValue.Name + " value. Please re-enter."
            End If

            Return isValid

        End Function

        Private Function ValidateValueForValueList(ByRef inValidationMessage As String, ByVal inValue As String, ByRef inUploadValue As UploadValue, _
                ByRef inGridCell As UltraGridCell) As Boolean

            Dim isValid As Boolean = True
            Dim theGrid As UltraGrid = CType(inGridCell.Row.Band.Layout.Grid, UltraGrid)

            If inUploadValue.ControlType.ToLower().Equals("valuelist") Then

                ' validate value is in the list only if it is required and there is a value
                ' or if it isn't and the value is not nothing
                ' or an empty string
                If Not IsNothing(inValue) AndAlso Not String.IsNullOrEmpty(inValue) Then

                    Dim theValueListData As BusinessObjectCollection = _
                            CType(Me.EIMManager.ValueListDataByKeyCollection.Item(inUploadValue.Key), BusinessObjectCollection)

                    If Not IsNothing(theValueListData) Then

                        Dim theValueListItem As KeyedListItem = CType(theValueListData.ItemByKey(inValue), KeyedListItem)
                        isValid = Not IsNothing(theValueListItem)
                    End If

                    If Not isValid Then
                        ' the invalid value needs to be cleared once it has been used for error messaging
                        inUploadValue.Value = ""
                        inValidationMessage = "The value """ + inValue + """ is not a valid " + inUploadValue.Name + " ID value. Please re-enter."
                    End If
                End If
            End If

            Return isValid

        End Function

#End Region

#Region "Handle Attribute Value Changes"

        ''' <summary>
        ''' Handle the enabling and disabling of the price, cost, and deal cells
        ''' according to the value of the corresponding Is Change checkbox cells.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub HandleIsChangeFlags(ByRef inUploadRowHolder As UploadRowHolder)

            For Each theGridAndDataRowHolder As GridAndDataRowHolder In inUploadRowHolder.GridAndDataRowList

                Dim thePriceChangeId As Integer = 0
                Dim theIsPriceChangeCell As UltraGridCell = Nothing

                Dim isPriceChange As Boolean = True
                Dim isCostChange As Boolean = True
                Dim isDealChange As Boolean = True

                ' get the is price change flag value
                If GridUtilities.TryGetGridCell(EIM_Constants.PRICE_IS_CHANGE_ATTR_KEY, theGridAndDataRowHolder.GridRow, theIsPriceChangeCell) Then
                    isPriceChange = GridUtilities.GetGridCellBooleanValue(theIsPriceChangeCell)
                End If

                ' get the is cost change flag value
                If GridUtilities.TryGetGridCell(EIM_Constants.COST_IS_CHANGE_ATTR_KEY, theGridAndDataRowHolder.GridRow, theIsPriceChangeCell) Then
                    isCostChange = GridUtilities.GetGridCellBooleanValue(theIsPriceChangeCell)
                End If

                ' get the is deal change flag value
                If Me.EIMManager.CurrentUploadSession.IsFromSLIM Then
                    If GridUtilities.TryGetGridCell(EIM_Constants.DEAL_IS_CHANGE_ATTR_KEY, theGridAndDataRowHolder.GridRow, theIsPriceChangeCell) Then

                        isDealChange = GridUtilities.GetGridCellBooleanValue(theIsPriceChangeCell)
                    End If
                Else
                    ' can only create deals for SLIM items
                    isDealChange = False
                End If

                HandlePriceChangeAttributes(isPriceChange, thePriceChangeId, theGridAndDataRowHolder.GridRow)
                HandleCostChangeAttributes(isCostChange, theGridAndDataRowHolder.GridRow)
                HandleSLIMVendorDealAttributes(isDealChange, theGridAndDataRowHolder.GridRow)

            Next
        End Sub

        Private Sub HandlePriceChangeAttributes(ByVal inIsPriceChange As Boolean, _
                 ByVal inPriceChangeId As Integer, ByVal inGridRow As UltraGridRow)

            Dim theGridCell As UltraGridCell = Nothing

            If inIsPriceChange Then

                GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY)

                ' enable reg price regardless
                ' of the price change type as it
                ' is required even for promo price changes
                GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_MULTIPLE_ATTR_KEY)

                If PriceChgType.IsPromoPriceChgType(inPriceChangeId) Then
                    ' if the price change type is promo

                    GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_START_DATE_ATTR_KEY)

                    GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_PROMO_ATTR_KEY)
                    GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_PROMO_MULTIPLE_ATTR_KEY)
                    GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_PROMO_MULTIPLE_ATTR_KEY)
                    GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_SALE_START_DATE_ATTR_KEY)
                    GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_SALE_END_DATE_ATTR_KEY)
                    GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_MSRP_PRICE_ATTR_KEY)
                    GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_MSRP_MULTIPLE_ATTR_KEY)
                Else
                    ' if the price change type is regular

                    GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_PROMO_ATTR_KEY)
                    GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_PROMO_MULTIPLE_ATTR_KEY)
                    GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_SALE_START_DATE_ATTR_KEY)
                    GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_SALE_END_DATE_ATTR_KEY)
                    GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_MSRP_PRICE_ATTR_KEY)
                    GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_MSRP_MULTIPLE_ATTR_KEY)

                    GridUtilities.EnableCell(inGridRow, EIM_Constants.PRICE_START_DATE_ATTR_KEY)
                End If
            Else
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_MARGIN_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_MSRP_PRICE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_MSRP_MULTIPLE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_MULTIPLE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_START_DATE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_PROMO_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_PROMO_MULTIPLE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_SALE_START_DATE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.PRICE_SALE_END_DATE_ATTR_KEY)
            End If

        End Sub

        Private Sub HandleCostChangeAttributes(ByVal inIsCostChange As Boolean, ByVal inGridRow As UltraGridRow)

            Dim theGridCell As UltraGridCell = Nothing

            If inIsCostChange Then
                GridUtilities.EnableCell(inGridRow, EIM_Constants.COST_UNIT_FREIGHT_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.COST_FREIGHT_UNIT_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.COST_COST_UNIT_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.COST_VEND_PKG_DSCR_ATTR_KEY)

                GridUtilities.EnableCell(inGridRow, EIM_Constants.COST_PKG_COST_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.COST_START_DATE_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.COST_END_DATE_ATTR_KEY)

                GridUtilities.EnableCell(inGridRow, EIM_Constants.COST_IGNORECASEPACK_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.COST_RETAILCASEPACK_ATTR_KEY)
            Else
                GridUtilities.DisableCell(inGridRow, EIM_Constants.COST_UNIT_FREIGHT_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.COST_FREIGHT_UNIT_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.COST_COST_UNIT_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.COST_VEND_PKG_DSCR_ATTR_KEY)

                GridUtilities.DisableCell(inGridRow, EIM_Constants.COST_PKG_COST_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.COST_START_DATE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.COST_END_DATE_ATTR_KEY)

                GridUtilities.DisableCell(inGridRow, EIM_Constants.COST_IGNORECASEPACK_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.COST_RETAILCASEPACK_ATTR_KEY)

            End If

        End Sub

        Private Sub HandleSLIMVendorDealAttributes(ByVal inIsDealChange As Boolean, ByVal inGridRow As UltraGridRow)

            Dim theGridCell As UltraGridCell = Nothing

            If inIsDealChange Then
                GridUtilities.EnableCell(inGridRow, EIM_Constants.ALLOWANCE_AMOUNT_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.DISCOUNT_AMOUNT_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.ALLOWANCE_START_DATE_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.ALLOWANCE_END_DATE_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.DISCOUNT_START_DATE_ATTR_KEY)
                GridUtilities.EnableCell(inGridRow, EIM_Constants.DISCOUNT_END_DATE_ATTR_KEY)
            Else
                GridUtilities.DisableCell(inGridRow, EIM_Constants.ALLOWANCE_AMOUNT_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.DISCOUNT_AMOUNT_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.ALLOWANCE_START_DATE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.ALLOWANCE_END_DATE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.DISCOUNT_START_DATE_ATTR_KEY)
                GridUtilities.DisableCell(inGridRow, EIM_Constants.DISCOUNT_END_DATE_ATTR_KEY)
            End If

        End Sub

        ''' <summary>
        ''' Handle the enabling and disabling of the non-jurisdictional override cells
        ''' according to the value of the corresponding Is Default Jurisdiction checkbox cell.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub HandleDefaultJurisdictionFlag(ByRef inUploadRowHolder As UploadRowHolder)

            For Each theGridAndDataRowHolder As GridAndDataRowHolder In inUploadRowHolder.GridAndDataRowList

                If theGridAndDataRowHolder.UploadTypeCode.Equals(EIM_Constants.ITEM_MAINTENANCE_CODE) Then
                    Dim theIsDefaultJurisdictionCell As UltraGridCell = Nothing

                    Dim isDefaultJurisdiction As Boolean = True

                    ' get the is price change flag value
                    If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_IS_DEFAULT_JURISDICTION_ATTR_KEY, theGridAndDataRowHolder.GridRow, theIsDefaultJurisdictionCell) Then
                        isDefaultJurisdiction = GridUtilities.GetGridCellBooleanValue(theIsDefaultJurisdictionCell)
                    End If

                    If isDefaultJurisdiction Then
                        EnableAllCells(theGridAndDataRowHolder.UploadTypeCode, theGridAndDataRowHolder)
                    Else
                        DisableItemGridCellsExcept(theGridAndDataRowHolder, _
                            EIM_Constants.JURISDICTION_ATTR_KEY_ARRAY)
                    End If
                End If
            Next

        End Sub

        ''' <summary>
        ''' Handle the enabling and disabling of the primary vendor cells
        ''' according to whether there is a valid store and vendor value.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub HandlePrimaryVendorFlag(ByRef inUploadRowHolder As UploadRowHolder)

            For Each theGridAndDataRowHolder As GridAndDataRowHolder In inUploadRowHolder.GridAndDataRowList

                Dim theStoreCell As UltraGridCell = Nothing
                Dim theVendorCell As UltraGridCell = Nothing
                Dim theStoreNo As Integer = 0
                Dim theVendorId As Integer = 0

                Dim isPriceChange As Boolean = True

                ' get the is price change flag value
                If GridUtilities.TryGetGridCell(EIM_Constants.STORE_NO_ATTR_KEY, theGridAndDataRowHolder.GridRow, theStoreCell) And _
                        GridUtilities.TryGetGridCell(EIM_Constants.COST_VENDOR_ATTR_KEY, theGridAndDataRowHolder.GridRow, theVendorCell) Then
                    If Not IsNothing(theStoreCell.Value) And Not IsNothing(theVendorCell.Value) Then
                        If Not Object.Equals(theStoreCell.Value, DBNull.Value) And Not Object.Equals(theVendorCell.Value, DBNull.Value) Then
                            If Integer.TryParse(CStr(theStoreCell.Value), theStoreNo) And Integer.TryParse(CStr(theVendorCell.Value), theVendorId) Then
                                GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.COST_PRIMARY_VENDOR)
                            Else
                                GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.COST_PRIMARY_VENDOR)
                            End If
                        End If
                    End If
                End If
            Next
        End Sub

        Private Sub HandlePromoPlannerAttributes(ByRef inUploadRowHolder As UploadRowHolder)

            Dim isValid As Boolean = True
            Dim theValidationMessage As String = ""
            Dim thePriceGridRow As UltraGridRow = Nothing
            Dim theCostGridRow As UltraGridRow = Nothing
            Dim theIsForPromoPlannerGridCell As UltraGridCell = Nothing
            Dim theIsPriceChangeCell As UltraGridCell = Nothing
            Dim enablePromoPlannerCells As Boolean = False

            Dim thePriceGridAndDataRowHolder As GridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.PRICE_UPLOAD_CODE)
            Dim theCostGridAndDataRowHolder As GridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.COST_UPLOAD_CODE)

            If Not IsNothing(thePriceGridAndDataRowHolder) Then
                thePriceGridRow = thePriceGridAndDataRowHolder.GridRow
            End If

            If Not IsNothing(theCostGridAndDataRowHolder) Then
                theCostGridRow = theCostGridAndDataRowHolder.GridRow
            End If

            If Not IsNothing(thePriceGridRow) Then
                If Not IsNothing(theCostGridRow) Then

                    Dim isPriceChange As Boolean = True

                    ' get the is price change flag value
                    If GridUtilities.TryGetGridCell(EIM_Constants.PRICE_IS_CHANGE_ATTR_KEY, thePriceGridRow, theIsPriceChangeCell) Then
                        isPriceChange = GridUtilities.GetGridCellBooleanValue(theIsPriceChangeCell)
                    End If

                    ' promo planner requires a price change for the item
                    If isPriceChange Then

                        Dim hasVendorId As Boolean = GridUtilities.GetGridCellIntegerValue(EIM_Constants.COST_VENDOR_ATTR_KEY, theCostGridRow) > 0
                        Dim hasPkgCostId As Boolean = GridUtilities.GetGridCellSingleValue(EIM_Constants.COST_PKG_COST_ATTR_KEY, theCostGridRow) > 0

                        ' promo planner requires a vendor and case cost
                        If hasVendorId And hasPkgCostId Then
                            ' enable the promo planner cells
                            GridUtilities.EnableCell(thePriceGridRow, EIM_Constants.PROMO_PLANNER_IS_FOR_PROMO_PLANNER_ATTR_KEY)

                            If GridUtilities.TryGetGridCell(EIM_Constants.PROMO_PLANNER_IS_FOR_PROMO_PLANNER_ATTR_KEY, thePriceGridRow, theIsForPromoPlannerGridCell) Then

                                ' enable the promo planner cells only if the is for promo planner is checked
                                enablePromoPlannerCells = Not Object.Equals(theIsForPromoPlannerGridCell.Value, DBNull.Value) AndAlso CBool(theIsForPromoPlannerGridCell.Value)
                            Else
                                ' the "is for promo planner" attribute is not present so we
                                ' must disable any other present promo planner attributes
                                enablePromoPlannerCells = False

                            End If
                        Else ' no vendor and or case cost in cost grid
                            ' disable the promo planner cells
                            GridUtilities.DisableCellWithToolTip(thePriceGridRow, EIM_Constants.PROMO_PLANNER_IS_FOR_PROMO_PLANNER_ATTR_KEY, _
                                    "The Vendor and Case Cost must be in the Cost Grid row when uploading to the Promo Planner.")
                            enablePromoPlannerCells = False
                        End If
                    Else ' no price change

                        ' warn the user there is no price change
                        GridUtilities.DisableCellWithToolTip(thePriceGridRow, EIM_Constants.PROMO_PLANNER_IS_FOR_PROMO_PLANNER_ATTR_KEY, _
                                                "A price change is required when uploading to the Promo Planner.")
                    End If
                Else ' no cost grid data
                    ' disable the promo planner cells
                    GridUtilities.DisableCellWithToolTip(thePriceGridRow, EIM_Constants.PROMO_PLANNER_IS_FOR_PROMO_PLANNER_ATTR_KEY, _
                            "The Vendor and Case Cost must be in the Cost Grid row when uploading to the Promo Planner.")
                    enablePromoPlannerCells = False
                End If

                If enablePromoPlannerCells Then
                    GridUtilities.EnableCell(thePriceGridRow, EIM_Constants.PROMO_PLANNER_PROJUNITS)
                    GridUtilities.EnableCell(thePriceGridRow, EIM_Constants.PROMO_PLANNER_COMMENT1)
                    GridUtilities.EnableCell(thePriceGridRow, EIM_Constants.PROMO_PLANNER_COMMENT2)
                    GridUtilities.EnableCell(thePriceGridRow, EIM_Constants.PROMO_PLANNER_BILLBACK)
                Else
                    GridUtilities.DisableCell(thePriceGridRow, EIM_Constants.PROMO_PLANNER_PROJUNITS)
                    GridUtilities.DisableCell(thePriceGridRow, EIM_Constants.PROMO_PLANNER_COMMENT1)
                    GridUtilities.DisableCell(thePriceGridRow, EIM_Constants.PROMO_PLANNER_COMMENT2)
                    GridUtilities.DisableCell(thePriceGridRow, EIM_Constants.PROMO_PLANNER_BILLBACK)
                End If
            End If

        End Sub

        Private Sub HandleRandomWeightAttributes(ByRef inUploadRowHolder As UploadRowHolder)

            Dim theScaleUOMGridCell As UltraGridCell = Nothing
            Dim theScaleUOM As String


            For Each theGridAndDataRowHolder As GridAndDataRowHolder In inUploadRowHolder.GridAndDataRowList

                If GridUtilities.TryGetGridCell(EIM_Constants.ITEMSCALE_SCALE_SCALEUOMUNIT_ID_ATTR_KEY, theGridAndDataRowHolder.GridRow, theScaleUOMGridCell) AndAlso _
                        theScaleUOMGridCell.Activation = Activation.AllowEdit Then

                    Dim index As Integer
                    theScaleUOM = theScaleUOMGridCell.ValueListResolved.GetText(theScaleUOMGridCell.Value, index)

                    If index > -1 Then
                        If Not IsNothing(theScaleUOM) AndAlso theScaleUOM.ToUpper().StartsWith("BY COUNT") Then
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_BYCOUNT_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_FIXEDWEIGHT_ATTR_KEY)
                        ElseIf Not IsNothing(theScaleUOM) AndAlso theScaleUOM.ToUpper().ToUpper().StartsWith("FIXED WEIGHT") Then
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_BYCOUNT_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_FIXEDWEIGHT_ATTR_KEY)
                        Else
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_BYCOUNT_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_FIXEDWEIGHT_ATTR_KEY)
                        End If
                    Else
                        GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_BYCOUNT_ATTR_KEY)
                        GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_FIXEDWEIGHT_ATTR_KEY)
                    End If
                Else
                    GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_BYCOUNT_ATTR_KEY)
                    GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_FIXEDWEIGHT_ATTR_KEY)
                End If
            Next

        End Sub

#End Region

#Region "Attribute Specific Validation Methods"

        Private Sub ValidateItemIdentifier(ByRef inUploadRowHolder As UploadRowHolder)

            Dim isValid As Boolean = True
            Dim isWarning As Boolean = False
            Dim theValidationMessage As String = ""
            Dim theUploadRow As UploadRow = inUploadRowHolder.UploadRow
            Dim theItemIdentifierGridCell As UltraGridCell
            Dim theIdentifier As String = ""

            ' only validate the identifier when we need to
            ' this flag is false only in the beginning or if
            ' the identifier value has changed
            If Not theUploadRow.HasValidatedIdentifier Then

                ' set the flag so we only validate again when the value changes
                theUploadRow.HasValidatedIdentifier = True

                For Each theGridAndDataRowHolder As GridAndDataRowHolder In inUploadRowHolder.GridAndDataRowList

                    theItemIdentifierGridCell = theGridAndDataRowHolder.GridRow.Cells(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY)
                    theIdentifier = GetItemIdentifier(theGridAndDataRowHolder.GridRow)

                    ' only clear a previous error if the error is not one from basic validation
                    ' those errors have the attribute's key value as the validation error key
                    Dim clearPreviousError As Boolean = _
                            Not inUploadRowHolder.ContainsValidationKey(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY)

                    GridUtilities.SetCellStyle(theItemIdentifierGridCell, "", EIM_Constants.ValidationLevels.Valid, clearPreviousError)
                    inUploadRowHolder.ClearValidationKey("Identifier")

                    Dim reg As New Regex("^[0-9]+$") 'regular expression for numeric characters

                    If Not reg.IsMatch(theIdentifier) Then
                        isValid = False
                        theValidationMessage = "This Identifier contains non-numeric character(s). Please delete this row or correct in a spreadsheet and re-import."
                    End If

                    If Not Me.EIMManager.CurrentUploadSession.IsNewItemSessionFlag Then
                        If theUploadRow.ItemKey <= 0 Then
                            ' for an existing item session validate the identifiers exist in IRMA
                            isValid = False
                            theValidationMessage = "This Identifier does not exist in IRMA. Please delete this row or correct in a spreadsheet and re-import."
                        End If
                    Else

                        Dim theItemKey As Integer = UploadRowDAO.Instance.GetItemKeyByIdentifier(theIdentifier)

                        If theItemKey > 0 Then
                            ' for a new item session validate the identifiers don't exist in IRMA
                            isValid = False
                            theValidationMessage = "This Identifier exists in IRMA. Please correct or delete this row."
                        End If
                    End If

                    If theUploadRow.IsVendorVINDuplicate = True And Me.EIMManager.CurrentUploadSession.UploadSessionUploadType.UploadTypeTemplateID = EIM_Constants.MATCHVENDORVIN_TEMPLATE Then
                        ' flag duplicate VIN numbers if MatchVendorVIN upload
                        isValid = False
                        theValidationMessage = "Duplicate Vendor Item Numbers exist in IRMA. Please delete this row or correct in a spreadsheet and re-import."
                    End If
                    ' *********** THIS IS CODE THAT SHOWS ERRROR MESSAGE FOR TECH SPEC SCREENSHOT ********
                    ' ***** NEED TO ADD ACTUAL CODE HERE ********* EVENTUALLY *********
                    ' ************** Added Code - Alex Z 01.20.2009 *********

                    If Me.EIMManager.CurrentUploadSession.IsDeleteItemSessionFlag = True Then
                        ' ***** validate delete session *****
                        Dim dt As New DataTable
                        dt = EIMUtilityDAO.Instance.ValidateItemDelete(theUploadRow.ItemKey)

                        For Each dr As DataRow In dt.Rows
                            If Not CInt(dr.Item(0)) = 0 And CInt(dr.Item(1)) = 0 Then
                                isValid = False
                                theValidationMessage = dr.Item(2).ToString
                            ElseIf Not CInt(dr.Item(0)) = 0 And CBool(dr.Item(1)) = True And _
                            isValid = True Then
                                isWarning = True
                                theValidationMessage = dr.Item(2).ToString
                            End If
                        Next
                    End If


                    ' ************************************************************************************

                    If Not isValid Then

                        GridUtilities.SetCellStyle(theItemIdentifierGridCell, theValidationMessage, EIM_Constants.ValidationLevels.Invalid)
                        inUploadRowHolder.AddValidationKey("Identifier", "Invalid Identifier", EIM_Constants.ValidationLevels.Invalid)
                        SetRowValidationLevel(inUploadRowHolder)
                    ElseIf isWarning Then
                        GridUtilities.SetCellStyle(theItemIdentifierGridCell, theValidationMessage, EIM_Constants.ValidationLevels.Warning)
                    Else
                        GridUtilities.SetCellStyle(theItemIdentifierGridCell, "", EIM_Constants.ValidationLevels.Valid, False)
                        ' disable any scale data cells if the identifier is not
                        ' a scale identifier
                        If ItemIdentifierDAO.Instance.IsScaleIdentifier(theIdentifier) Then
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_SCALEDESC1_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_SCALEDESC2_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_SCALEDESC3_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_SCALEDESC4_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_NUTRIFACT_ID_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_EXTRATEXT_LABEL_TYPE_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_EXTRATEXT_ID_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_FORCEDTARE_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_TARE_ID_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_ALTERNATE_TARE_ID_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_LABELSTYLE_ID_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_EATBY_ID_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_RANDOMWEIGHTTYPE_ID_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_SCALEUOMUNIT_ID_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_FIXEDWEIGHT_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_BYCOUNT_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKSHELFLIFE_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKPACKDATE_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKUNITPRICE_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKEATBY_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKWEIGHT_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKTOTALPRICE_ATTR_KEY)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.SCALE_EXTRATEXT_EXTRATEXT)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.SCALE_STORAGEDATA_STORAGEDATA)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.SCALE_ALLERGEN_ALLERGENS)
                            GridUtilities.EnableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.SCALE_INGREDIENT_INGREDIENTS)
                        Else
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_SCALEDESC1_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_SCALEDESC2_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_SCALEDESC3_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_SCALEDESC4_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_NUTRIFACT_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_EXTRATEXT_LABEL_TYPE_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_EXTRATEXT_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_FORCEDTARE_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_TARE_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_ALTERNATE_TARE_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_LABELSTYLE_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_EATBY_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_GRADE_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SHELFLIFE_LENGTH_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_RANDOMWEIGHTTYPE_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_SCALEUOMUNIT_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_FIXEDWEIGHT_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_SCALE_BYCOUNT_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKSHELFLIFE_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKPACKDATE_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKUNITPRICE_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKEATBY_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKWEIGHT_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEMSCALE_PRINTBLANKTOTALPRICE_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.SCALE_EXTRATEXT_EXTRATEXT)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.SCALE_STORAGEDATA_STORAGEDATA)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.SCALE_ALLERGEN_ALLERGENS)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.SCALE_INGREDIENT_INGREDIENTS)
                        End If

                        Dim enforceCanonicalFieldLocking As Boolean = ItemIdentifierDAO.Instance.IsValidatedItemInIcon(theUploadRow.ItemKey)

                        If enforceCanonicalFieldLocking Then
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_ITEM_DESCRIPTION_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_POS_DESCRIPTION_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_PACKAGE_DESC1_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_FOOD_STAMPS_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_BRAND_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_TAXCLASS_ID_ATTR_KEY)
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_ORGANIC_ATTR_KEY)
                        End If

                        Dim enforceSubteamLocking As Boolean = InstanceDataDAO.IsFlagActive("UKIPS")
                        Dim itemHasAlignedSubteam As Boolean = ItemIdentifierDAO.Instance.HasAlignedSubteam(theUploadRow.ItemKey)
                        Dim retailSale As UploadValue = theUploadRow.FindUploadValueByAttributeKey(EIM_Constants.ITEM_RETAIL_SALE)
                        Dim itemIsRetailSale As Boolean
                        If (IsNothing(retailSale)) Then
                            itemIsRetailSale = ItemIdentifierDAO.Instance.IsRetailSaleItem(theUploadRow.ItemKey)
                        Else
                            itemIsRetailSale = retailSale.Value
                        End If

                        If enforceSubteamLocking And itemHasAlignedSubteam And itemIsRetailSale Then
                            GridUtilities.DisableCell(theGridAndDataRowHolder.GridRow, EIM_Constants.ITEM_SUBTEAM_NO_ATTR_KEY)
                        End If
                    End If
                Next
            End If
        End Sub

        Private Sub ValidateLinkedItemIdentifier(ByRef inUploadRowHolder As UploadRowHolder)

            Dim isValid As Boolean = True
            Dim theValidationMessage As String = ""
            Dim theUploadRow As UploadRow = inUploadRowHolder.UploadRow
            Dim theLinkedItemIdentifierGridCell As UltraGridCell = Nothing
            Dim theLinkedItemIdentifier As String

            ' only validate the identifier when we need to
            ' this flag is false only in the beginning or if
            ' the identifier value has changed
            If Not theUploadRow.HasValidatedLinkedIdentifier Then

                ' set the flag so we only validate again when the value changes
                theUploadRow.HasValidatedLinkedIdentifier = True

                For Each theGridAndDataRowHolder As GridAndDataRowHolder In inUploadRowHolder.GridAndDataRowList

                    If GridUtilities.TryGetGridCell(EIM_Constants.PRICE_LINKED_ITEM_IDENTIFIER_ATTR_KEY, theGridAndDataRowHolder.GridRow, theLinkedItemIdentifierGridCell) Then

                        theLinkedItemIdentifier = GridUtilities.GetGridCellStringValue(theLinkedItemIdentifierGridCell)

                        ' only clear a previous error if the error is not one from basic validation
                        ' those errors have the attribute's key value as the validation error key
                        Dim clearPreviousError As Boolean = _
                                Not inUploadRowHolder.ContainsValidationKey(EIM_Constants.PRICE_LINKED_ITEM_IDENTIFIER_ATTR_KEY)

                        GridUtilities.SetCellStyle(theLinkedItemIdentifierGridCell, "", EIM_Constants.ValidationLevels.Valid, clearPreviousError)
                        inUploadRowHolder.ClearValidationKey("LinkedItemIdentifier")

                        If Not (IsNothing(theLinkedItemIdentifier) Or String.IsNullOrEmpty(theLinkedItemIdentifier)) Then

                            Dim theLinkedItemKey As Integer = UploadRowDAO.Instance.GetItemKeyByIdentifier(theLinkedItemIdentifier)

                            If theLinkedItemKey <= 0 Then
                                isValid = False
                                theValidationMessage = "This Identifier does not exist in IRMA. Please correct."
                            End If

                            If Not isValid Then

                                GridUtilities.SetCellStyle(theLinkedItemIdentifierGridCell, theValidationMessage, EIM_Constants.ValidationLevels.Invalid)

                                inUploadRowHolder.AddValidationKey("LinkedItemIdentifier", "Invalid Linked Item Identifier", EIM_Constants.ValidationLevels.Invalid)
                                SetRowValidationLevel(inUploadRowHolder)

                            Else
                                GridUtilities.SetCellStyle(theLinkedItemIdentifierGridCell, "", EIM_Constants.ValidationLevels.Valid, False)
                            End If
                        End If
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' This function validates the price upload attributes by
        ''' price change type id value.
        ''' It consists of hardcoded validation that overrides any configured validation.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ValidateByPriceChangeType(ByRef inUploadRowHolder As UploadRowHolder)

            inUploadRowHolder.ClearValidationKey("PriceChange")

            Dim theValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid

            Dim isPriceValid As Boolean = True
            Dim isPriceMultipleValid As Boolean = True
            Dim isPromoPriceValid As Boolean = True
            Dim isPromoPriceMultipleValid As Boolean = True
            Dim isMSRPPriceValid As Boolean = True
            Dim isPromoStartDateValid As Boolean = True
            Dim isPromoEndDateValid As Boolean = True

            Dim thePriceChangeId As Integer = 0
            Dim thePrice As Decimal = 0
            Dim thePriceMultiple As Integer = 0
            Dim theRegPriceStartDate As DateTime
            Dim thePromoPrice As Decimal = 0
            Dim thePromoPriceMultiple As Integer = 0
            Dim theMSRPPrice As Decimal = 0
            Dim thePromoStartDate As DateTime
            Dim thePromoEndDate As DateTime

            Dim theRegStartDateValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid

            Dim thePriceCell As UltraGridCell = Nothing
            Dim thePriceMultipleCell As UltraGridCell = Nothing
            Dim thePriceChangeTypeCell As UltraGridCell = Nothing
            Dim thePromoPriceCell As UltraGridCell = Nothing
            Dim thePromoPriceMultipleCell As UltraGridCell = Nothing
            Dim theMSRPPriceCell As UltraGridCell = Nothing
            Dim theRegStartDateCell As UltraGridCell = Nothing
            Dim thePromoStartDateCell As UltraGridCell = Nothing
            Dim thePromoEndDateCell As UltraGridCell = Nothing
            Dim theIsPriceChangeCell As UltraGridCell = Nothing
            Dim theCellToolTipText As String = ""

            Dim isPriceChange As Boolean = True

            Dim thePriceGridRow As UltraGridRow = Nothing

            Dim theGridAndDataRowHolder As GridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.PRICE_UPLOAD_CODE)
            If Not IsNothing(theGridAndDataRowHolder) Then

                thePriceGridRow = theGridAndDataRowHolder.GridRow

                If GridUtilities.TryGetGridCell(EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY, thePriceGridRow, thePriceChangeTypeCell) AndAlso _
                           Integer.TryParse(GridUtilities.GetGridCellStringValue(thePriceChangeTypeCell), thePriceChangeId) AndAlso thePriceChangeId > 0 Then

                    ' get the is price change flag value
                    If GridUtilities.TryGetGridCell(EIM_Constants.PRICE_IS_CHANGE_ATTR_KEY, thePriceGridRow, theIsPriceChangeCell) Then
                        isPriceChange = Boolean.Parse(GridUtilities.GetGridCellStringValue(theIsPriceChangeCell))
                    End If

                    HandlePriceChangeAttributes(isPriceChange, thePriceChangeId, thePriceGridRow)

                    If isPriceChange Then

                        GridUtilities.SetCellStyle(thePriceChangeTypeCell, "", EIM_Constants.ValidationLevels.Valid, False)

                        ' look for all the required price upload attribute grid cells
                        GridUtilities.TryGetGridCell(EIM_Constants.PRICE_ATTR_KEY, thePriceGridRow, thePriceCell)
                        GridUtilities.TryGetGridCell(EIM_Constants.PRICE_MULTIPLE_ATTR_KEY, thePriceGridRow, thePriceMultipleCell)
                        GridUtilities.TryGetGridCell(EIM_Constants.PRICE_START_DATE_ATTR_KEY, thePriceGridRow, theRegStartDateCell)

                        GridUtilities.TryGetGridCell(EIM_Constants.PRICE_PROMO_ATTR_KEY, thePriceGridRow, thePromoPriceCell)
                        GridUtilities.TryGetGridCell(EIM_Constants.PRICE_PROMO_MULTIPLE_ATTR_KEY, thePriceGridRow, thePromoPriceMultipleCell)
                        GridUtilities.TryGetGridCell(EIM_Constants.PRICE_SALE_START_DATE_ATTR_KEY, thePriceGridRow, thePromoStartDateCell)
                        GridUtilities.TryGetGridCell(EIM_Constants.PRICE_SALE_END_DATE_ATTR_KEY, thePriceGridRow, thePromoEndDateCell)
                        GridUtilities.TryGetGridCell(EIM_Constants.PRICE_MSRP_PRICE_ATTR_KEY, thePriceGridRow, theMSRPPriceCell)

                        ' if present, always validate the reg price and multiple as a
                        ' non zero value is required for reg and promo price creation

                        Dim allowZeroRegPrice As Boolean = InstanceDataDAO.IsFlagActiveCached("AllowZeroRegPrice")
                        Dim allowZeroSalePrice As Boolean = InstanceDataDAO.IsFlagActiveCached("AllowZeroSalePrice")

                        If Not IsNothing(thePriceCell) AndAlso thePriceCell.Activation = Activation.AllowEdit Then

                            GridUtilities.SetCellStyle(thePriceCell, "", EIM_Constants.ValidationLevels.Valid, False)

                            If Decimal.TryParse(GridUtilities.GetGridCellStringValue(thePriceCell), thePrice) Then
                                If Not allowZeroRegPrice And thePrice <= 0.01 Then
                                    isPriceValid = False
                                    theCellToolTipText = "The Regular Price must be greater than 0.01."
                                    GridUtilities.SetCellStyle(thePriceCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                                End If
                            Else
                                isPriceValid = False
                                theCellToolTipText = "The Regular Price is required."
                                GridUtilities.SetCellStyle(thePriceCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                            End If
                        End If

                        If Not IsNothing(thePriceMultipleCell) AndAlso thePriceMultipleCell.Activation = Activation.AllowEdit Then

                            GridUtilities.SetCellStyle(thePriceMultipleCell, "", EIM_Constants.ValidationLevels.Valid, False)

                            If Integer.TryParse(GridUtilities.GetGridCellStringValue(thePriceMultipleCell), thePriceMultiple) Then
                                If thePriceMultiple <= 0 Then
                                    isPriceMultipleValid = False
                                    theCellToolTipText = "The Regular Price Multiple must be greater than zero."
                                    GridUtilities.SetCellStyle(thePriceMultipleCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                                End If
                            Else
                                isPriceMultipleValid = False
                                theCellToolTipText = "The Regular Price Multiple is required."
                                GridUtilities.SetCellStyle(thePriceMultipleCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                            End If
                        End If

                        If PriceChgType.IsPromoPriceChgType(thePriceChangeId) Then
                            ' if the price change type is promo

                            ' make sure the required columns are present for a promo price type change
                            If IsNothing(thePromoPriceCell) Or IsNothing(thePromoStartDateCell) Or _
                                    IsNothing(thePromoEndDateCell) Then

                                theValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                theCellToolTipText = "A Promotion Price type cannot be selected because one or more required price attributes is missing."
                                GridUtilities.SetCellStyle(thePriceChangeTypeCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)

                            Else

                                ' validate the MSRP Price only for EDV
                                If PriceChgType.IsEDVPriceChgType(thePriceChangeId) And IsNothing(theMSRPPriceCell) Then

                                    theValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                    theCellToolTipText = "An EDV Price type cannot be selected because the required MSRP price attribute is missing."
                                    GridUtilities.SetCellStyle(thePriceChangeTypeCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                                Else

                                    ' validate the promo price
                                    If thePromoPriceCell.Activation = Activation.AllowEdit Then

                                        GridUtilities.SetCellStyle(thePromoPriceCell, "", EIM_Constants.ValidationLevels.Valid, False)

                                        If Decimal.TryParse(GridUtilities.GetGridCellStringValue(thePromoPriceCell), thePromoPrice) Then
                                            If Not allowZeroSalePrice And thePromoPrice <= 0.01 Then
                                                isPromoPriceValid = False
                                                theCellToolTipText = "The Promotion Price must be greater than 0.01."
                                                GridUtilities.SetCellStyle(thePromoPriceCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                                            End If
                                        Else
                                            isPromoPriceValid = False
                                            theCellToolTipText = "The Promotion Price is required."
                                            GridUtilities.SetCellStyle(thePromoPriceCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                                        End If
                                    End If

                                    ' validate the promo price multiple
                                    If thePromoPriceMultipleCell.Activation = Activation.AllowEdit Then

                                        GridUtilities.SetCellStyle(thePromoPriceMultipleCell, "", EIM_Constants.ValidationLevels.Valid, False)

                                        If Integer.TryParse(GridUtilities.GetGridCellStringValue(thePromoPriceMultipleCell), thePromoPriceMultiple) Then
                                            If thePromoPriceMultiple <= 0 Then
                                                isPromoPriceMultipleValid = False
                                                theCellToolTipText = "The Promotion Price Multiple must be greater than 0.01."
                                                GridUtilities.SetCellStyle(thePromoPriceMultipleCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                                            End If
                                        Else
                                            isPromoPriceMultipleValid = False
                                            theCellToolTipText = "The Promotion Price Multiple is required."
                                            GridUtilities.SetCellStyle(thePromoPriceMultipleCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                                        End If

                                        ' validate the promo start date
                                        If thePromoStartDateCell.Activation = Activation.AllowEdit Then

                                            GridUtilities.SetCellStyle(thePromoStartDateCell, "", EIM_Constants.ValidationLevels.Valid, False)

                                            If DateTime.TryParse(GridUtilities.GetGridCellStringValue(thePromoStartDateCell), thePromoStartDate) Then
                                                If Not thePromoStartDate.CompareTo(DateTime.Today) >= 0 Then
                                                    isPromoStartDateValid = False
                                                    theCellToolTipText = "The Promotion Start Date should be today or later."
                                                End If
                                            Else
                                                isPromoStartDateValid = False
                                                theCellToolTipText = "A valid Promotion Start Date must be entered for this Price Change Type."
                                            End If
                                        End If

                                        If Not isPromoStartDateValid Then
                                            GridUtilities.SetCellStyle(thePromoStartDateCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                                        End If

                                        ' validate the promo end date
                                        If thePromoEndDateCell.Activation = Activation.AllowEdit Then

                                            GridUtilities.SetCellStyle(thePromoEndDateCell, "", EIM_Constants.ValidationLevels.Valid, False)

                                            If DateTime.TryParse(GridUtilities.GetGridCellStringValue(thePromoEndDateCell), thePromoEndDate) Then
                                                If Not thePromoEndDate.CompareTo(DateTime.Today) > 0 Or Not thePromoEndDate.CompareTo(thePromoStartDate) > 0 Then
                                                    isPromoEndDateValid = False
                                                    theCellToolTipText = "The Promotion End Date must be in the future and after the Start Date."
                                                End If
                                            Else
                                                isPromoEndDateValid = False
                                                theCellToolTipText = "A valid Promotion End Date must be entered for this Price Change Type."
                                            End If

                                            If Not isPromoEndDateValid Then
                                                GridUtilities.SetCellStyle(thePromoEndDateCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                                            End If
                                        End If

                                        ' validate the MSRP Price only for EDV
                                        If PriceChgType.IsEDVPriceChgType(thePriceChangeId) Then
                                            ' validate the MSRP price
                                            If theMSRPPriceCell.Activation = Activation.AllowEdit Then

                                                GridUtilities.SetCellStyle(theMSRPPriceCell, "", EIM_Constants.ValidationLevels.Valid, False)

                                                If Decimal.TryParse(GridUtilities.GetGridCellStringValue(theMSRPPriceCell), theMSRPPrice) Then
                                                    If theMSRPPrice <= 0.01 Then
                                                        isMSRPPriceValid = False
                                                        theCellToolTipText = "The MSRP Price must be greater than 0.01 for an EDV price change type."
                                                        GridUtilities.SetCellStyle(theMSRPPriceCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            ' if the price change type is reg
                            ' make sure the required columns are present for a reg price type change
                            If IsNothing(thePriceCell) Or IsNothing(theRegStartDateCell) Then

                                theValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                theCellToolTipText = "A Regular Price type cannot be selected because one or more required price attributes is missing."
                                GridUtilities.SetCellStyle(thePriceChangeTypeCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)

                            Else

                                ' validate the reg price start date
                                Dim theStartDateString As String = GridUtilities.GetGridCellStringValue(theRegStartDateCell)
                                If theRegStartDateCell.Activation = Activation.AllowEdit Then

                                    GridUtilities.SetCellStyle(theRegStartDateCell, "", EIM_Constants.ValidationLevels.Valid, False)

                                    If IsNothing(theStartDateString) OrElse String.IsNullOrEmpty(theStartDateString) Then

                                        theRegStartDateValidationLevel = EIM_Constants.ValidationLevels.Warning
                                        theCellToolTipText = "This will default to today when uploaded if you do not enter a Reg Price Start Date."
                                    Else
                                        If DateTime.TryParse(theStartDateString, theRegPriceStartDate) Then
                                            If Not (theRegPriceStartDate.CompareTo(DateTime.Today) >= 0) Then
                                                theRegStartDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                                theCellToolTipText = "The Reg Price Start Date must be today or later."
                                            End If
                                        Else
                                            theRegStartDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                            theCellToolTipText = "You must enter a valid Reg Price Start Date."
                                        End If
                                    End If

                                    GridUtilities.SetCellStyle(theRegStartDateCell, theCellToolTipText, theRegStartDateValidationLevel)
                                End If
                            End If
                        End If
                    End If

                    If theValidationLevel <> EIM_Constants.ValidationLevels.Invalid Then
                        If Not isPromoStartDateValid Or Not isPriceValid Or _
                                Not isPriceMultipleValid Or Not isPromoEndDateValid Or Not isPromoPriceValid Or Not isPromoPriceMultipleValid Then
                            theValidationLevel = EIM_Constants.ValidationLevels.Invalid
                        Else
                            theValidationLevel = theRegStartDateValidationLevel
                        End If
                    End If
                End If
            End If

            inUploadRowHolder.AddValidationKey("PriceChange", "Invalid Price Change Values", theValidationLevel)

        End Sub

        ''' <summary>
        ''' This function validates the chain list attribute for an item.
        ''' It consists of hardcoded validation that overrides any configured validation.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ValidateItemChains(ByRef inUploadRowHolder As UploadRowHolder)

            inUploadRowHolder.ClearValidationKey("ItemChains")

            Dim theValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid
            Dim theCellToolTipText As String = ""

            Dim theChainsCell As UltraGridCell = Nothing

            Dim theItemGridRow As UltraGridRow = Nothing

            Dim theGridAndDataRowHolder As GridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.ITEM_MAINTENANCE_CODE)
            If Not IsNothing(theGridAndDataRowHolder) Then

                theItemGridRow = theGridAndDataRowHolder.GridRow

                If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_CHAINS_ATTR_KEY, theItemGridRow, theChainsCell) AndAlso _
                           theChainsCell.Activation = Activation.AllowEdit Then

                    Dim theChains As String = Nothing

                    theChains = GridUtilities.GetGridCellStringValue(theChainsCell)

                    If Not IsNothing(theChains) AndAlso _
                           theChains.IndexOf("-1") > -1 Then

                        theCellToolTipText = "One or more imported chain names cannot be found."
                        GridUtilities.SetCellStyle(theChainsCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)

                        theValidationLevel = EIM_Constants.ValidationLevels.Invalid
                    Else
                        GridUtilities.SetCellStyle(theChainsCell, "", EIM_Constants.ValidationLevels.Valid, False)
                    End If
                End If
            End If

            inUploadRowHolder.AddValidationKey("ItemChains", "Invalid Item Chains", theValidationLevel)

        End Sub

        ''' <summary>
        ''' This function validates the chain list attribute for an item.
        ''' It consists of hardcoded validation that overrides any configured validation.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ValidateScaleDigits(ByRef inUploadRowHolder As UploadRowHolder)

            ' identifier columns are read-only for 
            ' existing item sessions
            If Me.EIMManager.CurrentUploadSession.IsNewItemSessionFlag Then

                inUploadRowHolder.ClearValidationKey("ScaleDigits")

                Dim theValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid
                Dim theCellToolTipText As String = ""
                Dim theIsScaleIdentCell As UltraGridCell = Nothing
                Dim theIdentifierTypeCell As UltraGridCell = Nothing
                Dim theNumPluDigitsCell As UltraGridCell = Nothing

                Dim theItemGridRow As UltraGridRow = Nothing

                Dim theGridAndDataRowHolder As GridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.ITEM_MAINTENANCE_CODE)
                If Not IsNothing(theGridAndDataRowHolder) Then

                    theItemGridRow = theGridAndDataRowHolder.GridRow

                    If GridUtilities.TryGetGridCell(EIM_Constants.ITEMIDENTIFIER_IS_SCALE_IDENT_ATTR_KEY, theItemGridRow, theIsScaleIdentCell) AndAlso _
                            theIsScaleIdentCell.Activation = Activation.AllowEdit AndAlso _
                            GridUtilities.TryGetGridCell(EIM_Constants.ITEMIDENTIFIER_IDENTIFIERTYPE_ATTR_KEY, theItemGridRow, theIdentifierTypeCell) Then

                        Dim theNumDigits As Integer = 0
                        Dim theIdentifierType As String = Nothing
                        Dim theIdentifier As String = GetItemIdentifier(theItemGridRow)

                        theIdentifierType = GridUtilities.GetGridCellStringValue(EIM_Constants.ITEMIDENTIFIER_IDENTIFIERTYPE_ATTR_KEY, theItemGridRow)
                        theNumDigits = GridUtilities.GetGridCellIntegerValue(EIM_Constants.ITEMIDENTIFIER_NUM_DIGITS_ATTR_KEY, theItemGridRow)

                        If theIdentifierType = "O" And IsNewItemScaleIdentifier(theIdentifier) Then
                            Dim useSmartXPriceData As Boolean = InstanceDataDAO.IsFlagActiveCached("UseSmartXPriceData")

                            If useSmartXPriceData And theNumDigits <> 5 Then
                                theCellToolTipText = "All PLUs must have 5 digits. Please correct."

                                GridUtilities.TryGetGridCell(EIM_Constants.ITEMIDENTIFIER_NUM_DIGITS_ATTR_KEY, theItemGridRow, theNumPluDigitsCell)

                                GridUtilities.SetCellStyle(theNumPluDigitsCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)

                                theValidationLevel = EIM_Constants.ValidationLevels.Invalid

                            ElseIf theNumDigits = 4 Then

                                Dim theSubTeamIdString As String = GridUtilities.GetGridCellStringValue(EIM_Constants.ITEM_SUBTEAM_NO_ATTR_KEY, theItemGridRow)
                                Dim theSubTeamId As Integer = -1

                                If Integer.TryParse(theSubTeamIdString, theSubTeamId) Then

                                    'get any existing items with the same ScalePLU for the same SubTeam.ScaleDept for the selected subteam
                                    Dim pluConflicts As ArrayList = ScaleDAO.GetScalePluItemConflicts(theIdentifier, theNumDigits, theSubTeamId)

                                    If pluConflicts.Count > 0 Then

                                        theCellToolTipText = "The 4 digits of this PLU already exists for the selected sub-team. Please correct."
                                        GridUtilities.SetCellStyle(theIsScaleIdentCell, theCellToolTipText, EIM_Constants.ValidationLevels.Invalid)

                                        theValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                    Else
                                        GridUtilities.SetCellStyle(theIsScaleIdentCell, "", EIM_Constants.ValidationLevels.Valid, False)
                                    End If
                                End If
                            End If
                        End If

                        inUploadRowHolder.AddValidationKey("ScaleDigits", "Invalid Scale Digits", theValidationLevel)
                    End If
                End If
            End If

        End Sub

        Private Sub ValidateCostDates(ByRef inUploadRowHolder As UploadRowHolder)

            inUploadRowHolder.ClearValidationKey("CostDates")

            Dim isCostChange As Boolean = True
            Dim isStartDateValid As Boolean = True
            Dim isEndDateValid As Boolean = True

            Dim thePriceChangeType As Integer = 0
            Dim theStartDate As DateTime
            Dim theEndDate As DateTime
            Dim theStartDateCellToolTipText As String = ""
            Dim theEndDateCellToolTipText As String = ""

            Dim theStartDateValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid
            Dim theEndDateValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid
            Dim theValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid

            Dim theIsCostChangeCell As UltraGridCell = Nothing
            Dim theStartDateCell As UltraGridCell = Nothing
            Dim theEndDateCell As UltraGridCell = Nothing

            Dim theCostGridRow As UltraGridRow = Nothing

            Dim theGridAndDataRowHolder As GridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.COST_UPLOAD_CODE)
            If Not IsNothing(theGridAndDataRowHolder) Then

                theCostGridRow = theGridAndDataRowHolder.GridRow

                ' get the is cost change flag value
                If GridUtilities.TryGetGridCell(EIM_Constants.COST_IS_CHANGE_ATTR_KEY, theCostGridRow, theIsCostChangeCell) Then
                    isCostChange = Boolean.Parse(GridUtilities.GetGridCellStringValue(theIsCostChangeCell))
                End If

                If isCostChange Then

                    If GridUtilities.TryGetGridCell(EIM_Constants.COST_START_DATE_ATTR_KEY, theCostGridRow, theStartDateCell) And _
                            GridUtilities.TryGetGridCell(EIM_Constants.COST_END_DATE_ATTR_KEY, theCostGridRow, theEndDateCell) Then

                        ' validate the cost start date
                        ' TFS 1861:
                        ' Modify IRMA to allow costs to be entered with a past start date.
                        ' Developer: Denis Ng
                        ' Date: 08/08/2011
                        ' Comment: The following lines of code will be commented to prevent cost start date validation:
                        ' If Not theStartDate.CompareTo(DateTime.Today) >= 0 Then
                        '    isStartDateValid = False
                        '    theStartDateCellToolTipText = "The Cost Start Date should be today or later."
                        '    theStartDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                        ' End If

                        If theStartDateCell.Activation = Activation.AllowEdit Then
                            If DateTime.TryParse(GridUtilities.GetGridCellStringValue(theStartDateCell), theStartDate) Then
                                'If Not theStartDate.CompareTo(DateTime.Today) >= 0 Then
                                'isStartDateValid = False
                                'theStartDateCellToolTipText = "The Cost Start Date should be today or later."
                                'theStartDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                'End If
                            Else
                                isStartDateValid = False
                                theStartDateCellToolTipText = "The Cost Start Date must be a valid Date."
                                theStartDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                            End If
                        End If

                        ' validate the cost end date is after the start date
                        If theEndDateCell.Activation = Activation.AllowEdit Then
                            If Not DateTime.TryParse(GridUtilities.GetGridCellStringValue(theEndDateCell), theEndDate) Then
                                isEndDateValid = False
                                theEndDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                theStartDateCellToolTipText = "The Cost End Date must be a valid Date."
                            Else
                                If Not theEndDate.CompareTo(DateTime.Today) > 0 Or Not theEndDate.CompareTo(theStartDate) > 0 Then
                                    isEndDateValid = False
                                    theEndDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                    theEndDateCellToolTipText = "The Cost End Date must be in the future and after the Start Date."
                                End If
                            End If
                        End If

                        If isStartDateValid Then
                            GridUtilities.SetCellStyle(theStartDateCell, "", EIM_Constants.ValidationLevels.Valid, False)
                        Else
                            GridUtilities.SetCellStyle(theStartDateCell, theStartDateCellToolTipText, theStartDateValidationLevel)
                        End If

                        If isEndDateValid Then
                            GridUtilities.SetCellStyle(theEndDateCell, "", EIM_Constants.ValidationLevels.Valid, False)
                        Else
                            GridUtilities.SetCellStyle(theEndDateCell, theEndDateCellToolTipText, theEndDateValidationLevel)
                        End If

                        If theStartDateValidationLevel > theEndDateValidationLevel Then
                            theValidationLevel = theStartDateValidationLevel
                        Else
                            theValidationLevel = theEndDateValidationLevel
                        End If
                    End If
                End If
            End If

            inUploadRowHolder.AddValidationKey("CostDates", "Invalid Cost Dates", theValidationLevel)

        End Sub

        Private Sub ValidateDiscountDates(ByRef inUploadRowHolder As UploadRowHolder)

            inUploadRowHolder.ClearValidationKey("DiscountDates")

            Dim isStartDateValid As Boolean = True
            Dim isEndDateValid As Boolean = True

            Dim thePriceChangeType As Integer = 0
            Dim theStartDate As DateTime
            Dim theEndDate As DateTime
            Dim theStartDateCellToolTipText As String = ""
            Dim theEndDateCellToolTipText As String = ""

            Dim theStartDateValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid
            Dim theEndDateValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid
            Dim theValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid

            Dim theAmountCell As UltraGridCell = Nothing
            Dim theStartDateCell As UltraGridCell = Nothing
            Dim theEndDateCell As UltraGridCell = Nothing

            Dim theCostGridRow As UltraGridRow = Nothing

            Dim theGridAndDataRowHolder As GridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.COST_UPLOAD_CODE)
            If Not IsNothing(theGridAndDataRowHolder) Then

                theCostGridRow = theGridAndDataRowHolder.GridRow

                If GridUtilities.TryGetGridCell(EIM_Constants.DISCOUNT_AMOUNT_ATTR_KEY, theCostGridRow, theAmountCell) AndAlso _
                         Not IsNothing(theAmountCell.Value) AndAlso Not theAmountCell.Value.Equals(DBNull.Value) AndAlso Not String.IsNullOrEmpty(CStr(theAmountCell.Value)) AndAlso _
                         CDec(theAmountCell.Value) > 0 Then

                    If GridUtilities.TryGetGridCell(EIM_Constants.DISCOUNT_START_DATE_ATTR_KEY, theCostGridRow, theStartDateCell) And _
                        GridUtilities.TryGetGridCell(EIM_Constants.DISCOUNT_END_DATE_ATTR_KEY, theCostGridRow, theEndDateCell) Then

                        If theStartDateCell.Activation = Activation.AllowEdit Then
                            ' validate the Discount start date
                            If theStartDateCell.Activation = Activation.AllowEdit And _
                                    DateTime.TryParse(GridUtilities.GetGridCellStringValue(theStartDateCell), theStartDate) Then
                                If Not theStartDate.CompareTo(DateTime.Today) >= 0 Then
                                    isStartDateValid = False
                                    theStartDateCellToolTipText = "The Discount Start Date should be today or later."
                                    theStartDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                End If
                            Else
                                isStartDateValid = False
                                theStartDateCellToolTipText = "The Discount Start Date must be a valid Date."
                                theStartDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                            End If
                        End If

                        If theEndDateCell.Activation = Activation.AllowEdit Then
                            ' validate the Discount end date is after the start date
                            If Not DateTime.TryParse(GridUtilities.GetGridCellStringValue(theEndDateCell), theEndDate) Then
                                isEndDateValid = False
                                theEndDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                theStartDateCellToolTipText = "The Discount End Date must be a valid Date."
                            Else
                                If Not theEndDate.CompareTo(DateTime.Today) > 0 Or Not theEndDate.CompareTo(theStartDate) > 0 Then
                                    isEndDateValid = False
                                    theEndDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                    theEndDateCellToolTipText = "The Discount End Date must be in the future and after the Start Date."
                                End If
                            End If
                        End If

                        If isStartDateValid Then
                            GridUtilities.SetCellStyle(theStartDateCell, "", EIM_Constants.ValidationLevels.Valid, False)
                        Else
                            GridUtilities.SetCellStyle(theStartDateCell, theStartDateCellToolTipText, theStartDateValidationLevel)
                        End If

                        If isEndDateValid Then
                            GridUtilities.SetCellStyle(theEndDateCell, "", EIM_Constants.ValidationLevels.Valid, False)
                        Else
                            GridUtilities.SetCellStyle(theEndDateCell, theEndDateCellToolTipText, theEndDateValidationLevel)
                        End If

                        If theStartDateValidationLevel > theEndDateValidationLevel Then
                            theValidationLevel = theStartDateValidationLevel
                        Else
                            theValidationLevel = theEndDateValidationLevel
                        End If
                    End If
                End If
            End If

            inUploadRowHolder.AddValidationKey("DiscountDates", "Invalid Discount Dates", theValidationLevel)

        End Sub

        Private Sub ValidateAllowanceDates(ByRef inUploadRowHolder As UploadRowHolder)

            inUploadRowHolder.ClearValidationKey("AllowanceDates")

            Dim isStartDateValid As Boolean = True
            Dim isEndDateValid As Boolean = True

            Dim thePriceChangeType As Integer = 0
            Dim theStartDate As DateTime
            Dim theEndDate As DateTime
            Dim theStartDateCellToolTipText As String = ""
            Dim theEndDateCellToolTipText As String = ""

            Dim theStartDateValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid
            Dim theEndDateValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid
            Dim theValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid

            Dim theAmountCell As UltraGridCell = Nothing
            Dim theStartDateCell As UltraGridCell = Nothing
            Dim theEndDateCell As UltraGridCell = Nothing

            Dim theCostGridRow As UltraGridRow = Nothing

            Dim theGridAndDataRowHolder As GridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.COST_UPLOAD_CODE)
            If Not IsNothing(theGridAndDataRowHolder) Then

                theCostGridRow = theGridAndDataRowHolder.GridRow

                If GridUtilities.TryGetGridCell(EIM_Constants.ALLOWANCE_AMOUNT_ATTR_KEY, theCostGridRow, theAmountCell) AndAlso _
                         Not IsNothing(theAmountCell.Value) AndAlso Not theAmountCell.Value.Equals(DBNull.Value) AndAlso Not String.IsNullOrEmpty(CStr(theAmountCell.Value)) AndAlso _
                         CDec(theAmountCell.Value) > 0 Then

                    If GridUtilities.TryGetGridCell(EIM_Constants.ALLOWANCE_START_DATE_ATTR_KEY, theCostGridRow, theStartDateCell) And _
                    GridUtilities.TryGetGridCell(EIM_Constants.ALLOWANCE_END_DATE_ATTR_KEY, theCostGridRow, theEndDateCell) Then

                        If theStartDateCell.Activation = Activation.AllowEdit Then
                            ' validate the Allowance start date
                            If DateTime.TryParse(GridUtilities.GetGridCellStringValue(theStartDateCell), theStartDate) Then
                                If Not theStartDate.CompareTo(DateTime.Today) >= 0 Then
                                    isStartDateValid = False
                                    theStartDateCellToolTipText = "The Allowance Start Date should be today or later."
                                    theStartDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                End If
                            Else
                                isStartDateValid = False
                                theStartDateCellToolTipText = "The Allowance Start Date must be a valid Date."
                                theStartDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                            End If
                        End If

                        If theEndDateCell.Activation = Activation.AllowEdit Then
                            ' validate the Allowance end date is after the start date
                            If Not DateTime.TryParse(GridUtilities.GetGridCellStringValue(theEndDateCell), theEndDate) Then
                                isEndDateValid = False
                                theEndDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                theStartDateCellToolTipText = "The Allowance End Date must be a valid Date."
                            Else
                                If Not theEndDate.CompareTo(DateTime.Today) > 0 Or Not theEndDate.CompareTo(theStartDate) > 0 Then
                                    isEndDateValid = False
                                    theEndDateValidationLevel = EIM_Constants.ValidationLevels.Invalid
                                    theEndDateCellToolTipText = "The Allowance End Date must be in the future and after the Start Date."
                                End If
                            End If
                        End If

                        If isStartDateValid Then
                            GridUtilities.SetCellStyle(theStartDateCell, "", EIM_Constants.ValidationLevels.Valid, False)
                        Else
                            GridUtilities.SetCellStyle(theStartDateCell, theStartDateCellToolTipText, theStartDateValidationLevel)
                        End If

                        If isEndDateValid Then
                            GridUtilities.SetCellStyle(theEndDateCell, "", EIM_Constants.ValidationLevels.Valid, False)
                        Else
                            GridUtilities.SetCellStyle(theEndDateCell, theEndDateCellToolTipText, theEndDateValidationLevel)
                        End If

                        If theStartDateValidationLevel > theEndDateValidationLevel Then
                            theValidationLevel = theStartDateValidationLevel
                        Else
                            theValidationLevel = theEndDateValidationLevel
                        End If
                    End If
                End If
            End If

            inUploadRowHolder.AddValidationKey("AllowanceDates", "Invalid Allowance Dates", theValidationLevel)

        End Sub

        Private Sub ValidateAverageUnitWeight(ByRef inUploadRowHolder As UploadRowHolder)

            'Business Rules - IsValid if:
            '#1. AverageUnitWeight > 0.0 if Costed By Weight Item is sold as each     

            inUploadRowHolder.ClearValidationKey("AverageUnitWeight")

            Dim theValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid

            Dim isCostedByWeight As Boolean = False
            Dim theAverageUnitWeight As Decimal = 0.0

            Dim theCostedByWeightCell As UltraGridCell = Nothing
            Dim theStoreNoCell As UltraGridCell = Nothing

            Dim theItemIdentifier As String = ""
            Dim theAverageUnitWeightCell As UltraGridCell = Nothing
            Dim _isSoldAsEachCostedByWeightItem As Boolean = False

            Dim theIdentifierCell As UltraGridCell = Nothing

            Dim theItemGridRow As UltraGridRow = Nothing

            Dim theGridAndDataRowHolder As GridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.COST_UPLOAD_CODE)

            ' get if the item is costed by weight and its retail UOM
            ' if available then they will be found in the Item grid
            theGridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.ITEM_MAINTENANCE_CODE)

            If Not IsNothing(theGridAndDataRowHolder) Then

                theItemGridRow = theGridAndDataRowHolder.GridRow

                ' get average unit weight
                If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_AVERAGE_UNIT_WEIGHT, theItemGridRow, theAverageUnitWeightCell) Then
                    theAverageUnitWeight = GridUtilities.GetGridCellDecimalValue(theAverageUnitWeightCell)
                End If

                ' get identifier
                If GridUtilities.TryGetGridCell(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY, theItemGridRow, theIdentifierCell) Then
                    theItemIdentifier = GridUtilities.GetGridCellStringValue(theIdentifierCell)
                End If

                ' get costed by weight
                If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_COSTEDBYWEIGHT, theItemGridRow, theCostedByWeightCell) Then
                    isCostedByWeight = GridUtilities.GetGridCellBooleanValue(theCostedByWeightCell)
                End If

                If isCostedByWeight Then

                    _isSoldAsEachCostedByWeightItem = WholeFoods.IRMA.ItemHosting.DataAccess.ItemDAO.IfSoldAsEachInRetail(theItemIdentifier)

                    If _isSoldAsEachCostedByWeightItem = True Then
                        If Not theAverageUnitWeight > 0 Then
                            theValidationLevel = EIM_Constants.ValidationLevels.Invalid
                            GridUtilities.SetCellStyle(theAverageUnitWeightCell, "For costed by weight items that are sold as each, Average Unit Weight is required.", theValidationLevel)
                        Else
                            GridUtilities.SetCellStyle(theAverageUnitWeightCell, "", EIM_Constants.ValidationLevels.Valid, False)
                        End If
                    End If
                End If
            End If

            inUploadRowHolder.AddValidationKey("AverageUnitWeight", "Average Unit Weight must be > 0 for Costed-By-Weight Items sold as each in Retail", theValidationLevel)

        End Sub


        Private Sub ValidateCostedByWeight(ByRef inUploadRowHolder As UploadRowHolder)

            'Business Rules - IsValid if:
            '------------------------------------------------
            '#1. Item.Package_Unit_ID IsPackageUnit = True or Weight_Unit = True
            '#2. VendorCostHistory.Cost_Unit_ID IsPackageUnit = True or Weight_Unit = True
            '#3. Item.Vendor_Unit_ID & Item.Distribution_Unit_ID & Manufacturing_Unit_ID IsPackageUnit = True or Weight_Unit = True
            '#4. Item.Retail_Unit_ID = Item.Package_Unit_ID
            '#5. VendorCostHistory.Cost_Unit_ID = VendorCostHistory.Freight_Unit_ID

            inUploadRowHolder.ClearValidationKey("CostedByWeight")

            Dim theValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid

            Dim isCostChange As Boolean = True
            Dim isCostedByWeight As Boolean = False
            Dim theStoreList As String = String.Empty

            Dim thePackageUomID As Integer = -1
            Dim theRetailUomID As Integer = -1
            Dim theFreightUomID As Integer = -1
            Dim theCostUomID As Integer = -1
            Dim theManufacturingUomID As Integer = -1
            Dim theDistributionUomID As Integer = -1
            Dim theVendorUomID As Integer = -1

            Dim theVendorUomCellToolTipText As String = ""
            Dim thePackageUomCellToolTipText As String = ""
            Dim theFreightUomCellToolTipText As String = ""
            Dim theCostUomCellToolTipText As String = ""
            Dim theManufacturingUomCellToolTipText As String = ""
            Dim theDistributionUomCellToolTipText As String = ""
            Dim theRetailUomCellToolTipText As String = ""

            Dim theIsCostChangeCell As UltraGridCell = Nothing
            Dim theCostedByWeightCell As UltraGridCell = Nothing
            Dim theStoreNoCell As UltraGridCell = Nothing

            Dim theVendorUomCell As UltraGridCell = Nothing
            Dim theRetailUomCell As UltraGridCell = Nothing
            Dim thePackageUomCell As UltraGridCell = Nothing
            Dim theCostUomCell As UltraGridCell = Nothing
            Dim theFreightUomCell As UltraGridCell = Nothing
            Dim theManufacturingUomCell As UltraGridCell = Nothing
            Dim theDistributionUomCell As UltraGridCell = Nothing

            Dim theItemGridRow As UltraGridRow = Nothing
            Dim theCostGridRow As UltraGridRow = Nothing

            Dim theGridAndDataRowHolder As GridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.COST_UPLOAD_CODE)

            If Not IsNothing(theGridAndDataRowHolder) Then

                theCostGridRow = theGridAndDataRowHolder.GridRow

            End If

            If Not IsNothing(theCostGridRow) Then

                ' get if the item is costed by weight and its retail UOM
                ' if available then they will be found in the Item grid
                theGridAndDataRowHolder = inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.ITEM_MAINTENANCE_CODE)

                If Not IsNothing(theGridAndDataRowHolder) Then

                    theItemGridRow = theGridAndDataRowHolder.GridRow

                    ' get costed by weight
                    If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_COSTEDBYWEIGHT, theItemGridRow, theCostedByWeightCell) Then
                        isCostedByWeight = GridUtilities.GetGridCellBooleanValue(theCostedByWeightCell)
                    End If

                    If isCostedByWeight Then

                        ' get the vendor UOM
                        If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_VENDOR_UNIT_ID_ATTR_KEY, theItemGridRow, theVendorUomCell) Then
                            theVendorUomID = GridUtilities.GetGridCellIntegerValue(theVendorUomCell)
                        End If

                        ' get the retail UOM
                        If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_RETAIL_UNIT_ID_ATTR_KEY, theItemGridRow, theRetailUomCell) Then
                            theRetailUomID = GridUtilities.GetGridCellIntegerValue(theRetailUomCell)
                        End If

                        ' get the manufacturing UOM
                        If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_MANUFACTURING_UNIT_ID_ATTR_KEY, theItemGridRow, theManufacturingUomCell) Then
                            theManufacturingUomID = GridUtilities.GetGridCellIntegerValue(theManufacturingUomCell)
                        End If

                        ' get the package UOM
                        If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_PACKAGE_UNIT_ID_ATTR_KEY, theItemGridRow, thePackageUomCell) Then
                            thePackageUomID = GridUtilities.GetGridCellIntegerValue(thePackageUomCell)
                        End If

                        ' get the distribution UOM
                        If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_DISTRIBUTION_UNIT_ID_ATTR_KEY, theItemGridRow, theDistributionUomCell) Then
                            theDistributionUomID = GridUtilities.GetGridCellIntegerValue(theDistributionUomCell)
                        End If

                        ' get the cost UOM 
                        If GridUtilities.TryGetGridCell(EIM_Constants.COST_COST_UNIT_ATTR_KEY, theCostGridRow, theCostUomCell) Then
                            theCostUomID = GridUtilities.GetGridCellIntegerValue(theCostUomCell)
                        End If

                        ' get the freight UOM 
                        If GridUtilities.TryGetGridCell(EIM_Constants.COST_FREIGHT_UNIT_ATTR_KEY, theCostGridRow, theFreightUomCell) Then
                            theFreightUomID = GridUtilities.GetGridCellIntegerValue(theFreightUomCell)
                        End If

                        ' get the stores to be uploaded to
                        If Me.EIMManager.EIMForm.CheckBoxUploadToItemStore.Checked Then
                            If GridUtilities.TryGetGridCell(EIM_Constants.STORE_NO_ATTR_KEY, theCostGridRow, theStoreNoCell) Then
                                theStoreList = GridUtilities.GetGridCellStringValue(theStoreNoCell)
                            End If
                        Else
                            theStoreList = Me.EIMManager.EIMForm.StoreSelectorCostUpload.SelectedStoreIdString
                        End If

                        ' we cannot do the validation if there are no stores being uploaded to
                        If Not (IsNothing(theStoreList) Or String.IsNullOrEmpty(theStoreList)) Then

                            ' now do the validation - rule #1 - Item.Package_Unit_ID IsPackageUnit = True or Weight_Unit = True
                            theValidationLevel = IsCBWUnit(thePackageUomID, False)

                            If theValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                                GridUtilities.SetCellStyle(thePackageUomCell, "For costed by weight items, Package UOM must be a valid CostedByWeight UOM.", theValidationLevel)
                            Else
                                GridUtilities.SetCellStyle(thePackageUomCell, "", EIM_Constants.ValidationLevels.Valid, False)
                            End If
                            'End If

                            ' now do the validation rule #2  - VendorCostHistory.Cost_Unit_ID IsPackageUnit = True or Weight_Unit = True
                            theValidationLevel = IsCBWUnit(theCostUomID, False)

                            If theValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                                GridUtilities.SetCellStyle(theCostUomCell, "For costed by weight items, Cost UOM must be a valid CostedByWeight UOM.", theValidationLevel)
                            Else
                                GridUtilities.SetCellStyle(theCostUomCell, "", EIM_Constants.ValidationLevels.Valid, False)
                            End If

                            theValidationLevel = IsCBWUnit(theFreightUomID, False)

                            If theValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                                GridUtilities.SetCellStyle(theFreightUomCell, "For costed by weight items, Freight UOM must be a valid CostedByWeight UOM.", theValidationLevel)
                            Else
                                GridUtilities.SetCellStyle(theFreightUomCell, "", EIM_Constants.ValidationLevels.Valid, False)
                            End If

                            ' now do the validation rule #3 - Item.Vendor_Unit_ID & Item.Distribution_Unit_ID & Manufacturing_Unit_ID IsPackageUnit = True or Weight_Unit = True

                            'Rule #3.0
                            theValidationLevel = IsCBWUnit(theVendorUomID, False)

                            If theValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                                GridUtilities.SetCellStyle(theVendorUomCell, "For costed by weight items, Vendor UOM must be a valid CostedByWeight UOM.", theValidationLevel)
                            Else
                                GridUtilities.SetCellStyle(theVendorUomCell, "", EIM_Constants.ValidationLevels.Valid, False)
                            End If

                            'Rule #3.1
                            theValidationLevel = IsCBWUnit(theDistributionUomID, False)

                            If theValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                                GridUtilities.SetCellStyle(theDistributionUomCell, "For costed by weight items, Distribution UOM must be a valid CostedByWeight UOM.", theValidationLevel)
                            Else
                                GridUtilities.SetCellStyle(theDistributionUomCell, "", EIM_Constants.ValidationLevels.Valid, False)
                            End If

                            'Rule #3.2
                            theValidationLevel = IsCBWUnit(theManufacturingUomID, False)

                            If theValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                                GridUtilities.SetCellStyle(theManufacturingUomCell, "For costed by weight items, Manufacturing UOM must be a valid CostedByWeight UOM.", theValidationLevel)
                            Else
                                GridUtilities.SetCellStyle(theManufacturingUomCell, "", EIM_Constants.ValidationLevels.Valid, False)
                            End If

                            ' now do the validation rule #4 - Item.Retail_Unit_ID = Item.Package_Unit_ID
                            If theRetailUomID <> thePackageUomID Then
                                GridUtilities.SetCellStyle(theRetailUomCell, "For costed by weight items, Retail UOM must be the same as the Package UOM.", theValidationLevel)
                            Else
                                GridUtilities.SetCellStyle(theRetailUomCell, "", EIM_Constants.ValidationLevels.Valid, False)
                            End If

                            ' now do the validation rule #5 - VendorCostHistory.Cost_Unit_ID = VendorCostHistory.Freight_Unit_ID
                            If theCostUomID <> theFreightUomID Then
                                GridUtilities.SetCellStyle(theFreightUomCell, "For costed by weight items, Freight UOM must be the same as the Cost UOM.", theValidationLevel)
                            Else
                                GridUtilities.SetCellStyle(theFreightUomCell, "", EIM_Constants.ValidationLevels.Valid, False)
                            End If

                        End If

                    End If

                End If

            End If

            inUploadRowHolder.AddValidationKey("CostedByWeight", "Invalid CostedByWeight UOM Values", theValidationLevel)

        End Sub

        Private Function IsCBWUnit(ByVal inUomID As Integer, ByVal inCheckWeightedUnit As Boolean) As EIM_Constants.ValidationLevels

            Dim _packageUom As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Invalid

            Return EIMUtilityDAO.Instance.ValidateCostedByWeightUOM(inUomID, inCheckWeightedUnit)

        End Function

#End Region

#Region "Upload-time Validation"

        ''' <summary>
        ''' Check the price changes in the upload grid against
        ''' existing pending changes in the database for duplicates.
        ''' </summary>
        ''' <param name="inStoreList">This is a comma delimeted list of store no
        ''' generated from the user's  selection in the price upload store selector.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidatePriceChanges(ByVal inStoreList As String) As Integer

            Me.ProgressCounter = 0

            Dim theErrorCount As Integer = 0
            Dim theStoreListForItem As String = Nothing
            Dim theIsPriceChangeUploadValue As UploadValue
            Dim theUploadValueForStoreNo As UploadValue

            Try

                For Each theUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList

                    theIsPriceChangeUploadValue = theUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_IS_CHANGE_ATTR_KEY)

                    ' check rows for which the is price change flag is either
                    ' not present or true
                    If IsNothing(theIsPriceChangeUploadValue) OrElse CBool(theIsPriceChangeUploadValue.Value) Then

                        theUploadValueForStoreNo = theUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.STORE_NO_ATTR_KEY)

                        ' if inStoreList is Nothing then the user wants to create price changes
                        ' for the store in the UplodRow
                        If IsNothing(inStoreList) Or String.IsNullOrEmpty(inStoreList) Then

                            ' make sure there is a store in the UplodRow
                            If Not IsNothing(theUploadValueForStoreNo) Then
                                theStoreListForItem = theUploadValueForStoreNo.Value
                            End If
                        Else
                            ' use the store selector store list
                            theStoreListForItem = inStoreList
                        End If

                        ' there's got to be a store!
                        If Not IsNothing(theStoreListForItem) Or String.IsNullOrEmpty(inStoreList) Then
                            theErrorCount = theErrorCount + InternalValidatePriceChanges(theUploadRowHolder, theStoreListForItem, IsNothing(inStoreList) Or String.IsNullOrEmpty(inStoreList))
                        End If
                    End If
                Next

            Finally
                Me.ProgressCounter = 0
            End Try

            Return theErrorCount

        End Function

        ''' <summary>
        ''' Check the price changes in the upload grid against each other for duplicates.
        ''' Only concerned about price changes with the same store/start date/price change type
        ''' </summary>
        ''' <param name="useGridStore">True if user has checked the 'Upload Prices and Costs Back to Item’s Store' checkbox [bottom right of form]. 
        ''' False if the user is loading data to the stores selected in the list box.  TFS IS HOSED</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateForInGridDuplicatePriceChanges(ByVal useGridStore As Boolean) As Integer

            Me.ProgressCounter = 0

            Dim theErrorCount As Integer = 0
            Dim outerRowIndex As Integer = 0
            Dim innerRowIndex As Integer = 0
            Dim theStoreListForItem As String = Nothing

            Dim theUploadValueForStoreNo_OUTER As UploadValue
            Dim theUploadValueForItemIdentifier_OUTER As UploadValue
            Dim theUploadValueForPriceChgType_OUTER As UploadValue
            Dim theUploadValueForStartDate_OUTER As UploadValue

            Dim theUploadValueForStoreNo_INNER As UploadValue
            Dim theUploadValueForItemIdentifier_INNER As UploadValue
            Dim theUploadValueForPriceChgType_INNER As UploadValue
            Dim theUploadValueForStartDate_INNER As UploadValue

            Dim isErrorRow As Boolean = False
            Dim gridAndDataRowHolder As GridAndDataRowHolder = Nothing
            Dim priceChangeTypeGridCell As UltraGridCell = Nothing
            Dim theIsPriceChangeUploadValue As UploadValue

            Try

                For Each currentUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList
                    'increment outer row index and reset inner row index
                    outerRowIndex += 1
                    innerRowIndex = 0

                    'reset error row indicator
                    isErrorRow = False

                    theIsPriceChangeUploadValue = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_IS_CHANGE_ATTR_KEY)

                    ' check rows for which the is price change flag not present or is true
                    If IsNothing(theIsPriceChangeUploadValue) OrElse CBool(theIsPriceChangeUploadValue.Value) Then

                        'get the grid row for the current data set row
                        gridAndDataRowHolder = currentUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.PRICE_UPLOAD_CODE)

                        priceChangeTypeGridCell = Nothing

                        ' do not continue checkinf rows if there is no price change type  column
                        If Not GridUtilities.TryGetGridCell(EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY, gridAndDataRowHolder.GridRow, priceChangeTypeGridCell) Then
                            Exit For
                        End If

                        'clear any current error
                        ' don't clear the price change type cell style, however since
                        ' it has already been done by the InternalValidatePriceChanges that
                        ' executed prior to this sub
                        currentUploadRowHolder.ClearValidationKey("DupePriceChange")

                        'get values from current outer row
                        theUploadValueForItemIdentifier_OUTER = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY)
                        theUploadValueForPriceChgType_OUTER = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY)
                        theUploadValueForStoreNo_OUTER = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.STORE_NO_ATTR_KEY)

                        'grab the appropriate start date based on the price change type - reg start or promo start
                        If Not IsNothing(theUploadValueForPriceChgType_OUTER) AndAlso Not IsNothing(theUploadValueForPriceChgType_OUTER.Value) _
                            AndAlso PriceChgType.IsPromoPriceChgType(CType(theUploadValueForPriceChgType_OUTER.Value, Integer)) Then
                            'grab promo start date
                            theUploadValueForStartDate_OUTER = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_SALE_START_DATE_ATTR_KEY)
                        Else
                            'grab reg start date
                            theUploadValueForStartDate_OUTER = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_START_DATE_ATTR_KEY)
                        End If

                        'check current row against all other rows in upload data set
                        For Each otherUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList
                            innerRowIndex += 1

                            'don't validate against the same row in the data set
                            If innerRowIndex <> outerRowIndex Then
                                'check for duplicates in data set for the same item/price_chg_type/start_date/store [store is conditional based on useGridStore]

                                'get values from current inner row
                                theUploadValueForItemIdentifier_INNER = otherUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY)
                                theUploadValueForPriceChgType_INNER = otherUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY)
                                theUploadValueForStoreNo_INNER = otherUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.STORE_NO_ATTR_KEY)

                                'check for same item
                                If Not IsNothing(theUploadValueForItemIdentifier_INNER) AndAlso Not IsNothing(theUploadValueForItemIdentifier_OUTER) _
                                    AndAlso theUploadValueForItemIdentifier_INNER.Value.Equals(theUploadValueForItemIdentifier_OUTER.Value) Then

                                    'check for same price chg type
                                    If Not IsNothing(theUploadValueForPriceChgType_INNER) AndAlso Not IsNothing(theUploadValueForPriceChgType_OUTER) _
                                        AndAlso theUploadValueForPriceChgType_INNER.Value.Equals(theUploadValueForPriceChgType_OUTER.Value) Then

                                        'grab the appropriate start date based on the price change type - reg start or promo start
                                        If Not IsNothing(theUploadValueForPriceChgType_INNER.Value) AndAlso _
                                            PriceChgType.IsPromoPriceChgType(CType(theUploadValueForPriceChgType_INNER.Value, Integer)) Then
                                            'grab promo start date
                                            theUploadValueForStartDate_INNER = otherUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_SALE_START_DATE_ATTR_KEY)
                                        Else
                                            'grab reg start date
                                            theUploadValueForStartDate_INNER = otherUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_START_DATE_ATTR_KEY)
                                        End If 'grab the appropriate start date based on the price change type - reg start or promo start

                                        'check for same start date
                                        If Not IsNothing(theUploadValueForStartDate_INNER) AndAlso Not IsNothing(theUploadValueForStartDate_OUTER) _
                                            AndAlso Not IsNothing(theUploadValueForStartDate_INNER.Value) AndAlso Not IsNothing(theUploadValueForStartDate_OUTER.Value) _
                                            AndAlso theUploadValueForStartDate_INNER.Value.Equals(theUploadValueForStartDate_OUTER.Value) Then

                                            'do we need to check the store on this row as well?
                                            If useGridStore Then
                                                'check for the same store list
                                                If Not IsNothing(theUploadValueForStoreNo_INNER) AndAlso Not IsNothing(theUploadValueForStoreNo_OUTER) _
                                                    AndAlso theUploadValueForStoreNo_INNER.Value.Equals(theUploadValueForStoreNo_OUTER.Value) Then
                                                    isErrorRow = True
                                                End If
                                            Else
                                                isErrorRow = True
                                            End If 'do we need to check the store on this row as well?
                                        End If 'check for same start date
                                    End If 'check for same price chg type
                                End If 'check for same item
                            End If 'don't validate against the same row in the data set

                            If isErrorRow Then
                                'this outer row is the same as another row
                                theErrorCount += 1

                                'mark outer row as an error row and skip out of inner for loop
                                currentUploadRowHolder.AddValidationKey("DupePriceChange", "In-grid Duplocate Price Change", EIM_Constants.ValidationLevels.Invalid)

                                SetRowValidationLevel(currentUploadRowHolder)

                                'get the grid row for the current data set row
                                gridAndDataRowHolder = currentUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.PRICE_UPLOAD_CODE)

                                'update the price chg type cell to inform user of error
                                GridUtilities.SetCellStyle(priceChangeTypeGridCell, ResourcesItemHosting.GetString("OverlappingSamePromoPriceUploaded"), EIM_Constants.ValidationLevels.Invalid)

                                Exit For
                            End If ' isErrorRow
                        Next
                    End If ' check rows for which the is price change flag not present or is true
                Next

            Finally
                Me.ProgressCounter = 0
            End Try

            Return theErrorCount

        End Function

        ''' <summary>
        ''' Validate store jurisdictions for an existing item session.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateExistingItemJurisdiction() As Integer

            Me.ProgressCounter = 0

            Dim theErrorCount As Integer = 0

            Try

                For Each theUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList

                    ' the jurisdiction column must be in the item grid to do any validation!
                    If Not IsNothing(theUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.ITEM_JURISDICTION_ID_ATTR_KEY)) Then
                        Exit For
                    End If

                    theErrorCount = InternalValidateExistingItemJurisdiction(theUploadRowHolder)
                Next

            Finally
                Me.ProgressCounter = 0
            End Try

            Return theErrorCount

        End Function

        Public Function ValidateNewItemJurisdictions() As Integer

            Me.ProgressCounter = 0

            Dim theErrorCount As Integer = 0
            Dim outerRowIndex As Integer = 0
            Dim innerRowIndex As Integer = 0

            Dim theIsDefaultJurisdiction_OUTER As Boolean
            Dim theItemIdentifier_OUTER As String
            Dim theStoreJurisdictionId_OUTER As String

            Dim theIsDefaultJurisdiction_INNER As Boolean
            Dim theItemIdentifier_INNER As String
            Dim theStoreJurisdictionId_INNER As String

            Dim itemDefaultJurisdictionSetMoreThanOnce As Boolean = False

            Dim theErrorMessage As String = ""

            Dim gridAndDataRowHolder As GridAndDataRowHolder = Nothing
            Dim theIsDefaultJurisdictionGridCell As UltraGridCell = Nothing
            Dim theStoreJurisdictionIdGridCell As UltraGridCell = Nothing

            Try

                For Each currentUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList
                    'increment outer row index and reset inner row index
                    outerRowIndex += 1
                    innerRowIndex = 0

                    'reset flag
                    'get the grid row for the current data set row
                    gridAndDataRowHolder = currentUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.ITEM_MAINTENANCE_CODE)

                    If Not IsNothing(gridAndDataRowHolder) Then

                        If Not GridUtilities.TryGetGridCell(EIM_Constants.ITEM_IS_DEFAULT_JURISDICTION_ATTR_KEY, gridAndDataRowHolder.GridRow, theIsDefaultJurisdictionGridCell) Or _
                            Not GridUtilities.TryGetGridCell(EIM_Constants.ITEM_JURISDICTION_ID_ATTR_KEY, gridAndDataRowHolder.GridRow, theStoreJurisdictionIdGridCell) Then
                            Exit For
                        End If

                        'clear previous error
                        currentUploadRowHolder.ClearValidationKey("NewItemJurisdiction")
                        GridUtilities.SetCellStyle(theIsDefaultJurisdictionGridCell, "", EIM_Constants.ValidationLevels.Valid, False)
                        GridUtilities.SetCellStyle(theStoreJurisdictionIdGridCell, "", EIM_Constants.ValidationLevels.Valid, False)

                        itemDefaultJurisdictionSetMoreThanOnce = False

                        'get values from current outer row
                        theItemIdentifier_OUTER = currentUploadRowHolder.UploadRow.FindValueByAttributeKey(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY)
                        theIsDefaultJurisdiction_OUTER = CBool(currentUploadRowHolder.UploadRow.FindValueByAttributeKey(EIM_Constants.ITEM_IS_DEFAULT_JURISDICTION_ATTR_KEY))
                        theStoreJurisdictionId_OUTER = currentUploadRowHolder.UploadRow.FindValueByAttributeKey(EIM_Constants.ITEM_JURISDICTION_ID_ATTR_KEY)

                        'check current row against all other rows in upload data set
                        For Each otherUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList
                            innerRowIndex += 1

                            'get values from current inner row
                            theItemIdentifier_INNER = otherUploadRowHolder.UploadRow.FindValueByAttributeKey(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY)
                            theIsDefaultJurisdiction_INNER = CBool(otherUploadRowHolder.UploadRow.FindValueByAttributeKey(EIM_Constants.ITEM_IS_DEFAULT_JURISDICTION_ATTR_KEY))
                            theStoreJurisdictionId_INNER = otherUploadRowHolder.UploadRow.FindValueByAttributeKey(EIM_Constants.ITEM_JURISDICTION_ID_ATTR_KEY)

                            'check for same item
                            If Not IsNothing(theItemIdentifier_INNER) AndAlso Not IsNothing(theItemIdentifier_OUTER) _
                                AndAlso theItemIdentifier_INNER.Equals(theItemIdentifier_OUTER) Then

                                'check for items that have their default jurisdiction set to more than one value
                                If theIsDefaultJurisdiction_OUTER = True AndAlso theIsDefaultJurisdiction_INNER = True AndAlso _
                                    Not theStoreJurisdictionId_OUTER.Equals(theStoreJurisdictionId_INNER) Then

                                    itemDefaultJurisdictionSetMoreThanOnce = True
                                    theErrorMessage = "This item has its default jurisdiction set to more than one value in this session."
                                    Exit For
                                End If

                                'check for items that have the same jurisdiction set to both the default and alternate
                                If theIsDefaultJurisdiction_OUTER <> theIsDefaultJurisdiction_INNER AndAlso _
                                    theStoreJurisdictionId_OUTER.Equals(theStoreJurisdictionId_INNER) Then

                                    itemDefaultJurisdictionSetMoreThanOnce = True
                                    theErrorMessage = "This item has a jurisdiction set both the default and alternate jursdiction in this session."
                                    Exit For
                                End If
                            End If
                        Next

                        ' set the validation errors in the grid
                        If itemDefaultJurisdictionSetMoreThanOnce Then
                            'this outer row is the same as another row
                            theErrorCount += 1

                            'mark outer row as an error row and skip out of inner for loop
                            currentUploadRowHolder.AddValidationKey("NewItemJurisdiction", "Invalid Jurisdiction Assignment", EIM_Constants.ValidationLevels.Invalid)
                            SetRowValidationLevel(currentUploadRowHolder)

                            'update the is default jurisdiction cell to inform user of error
                            GridUtilities.SetCellStyle(theIsDefaultJurisdictionGridCell, theErrorMessage, EIM_Constants.ValidationLevels.Invalid)

                            'update the jurisdiction id cell to inform user of error
                            GridUtilities.SetCellStyle(theStoreJurisdictionIdGridCell, theErrorMessage, EIM_Constants.ValidationLevels.Invalid)
                        End If
                    End If
                Next

            Finally
                Me.ProgressCounter = 0
            End Try

            Return theErrorCount

        End Function

        ''' <summary>
        ''' Verify that each item/store combination has only one vendor
        ''' </summary>
        ''' <param name="useGridStore">True if the user has chosen
        ''' to upload to the store on the grid rows.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateForPrimaryVendor(ByVal useGridStore As Boolean) As Integer

            Me.ProgressComplete = False
            Me.ProgressCounter = 0

            Dim theErrorCount As Integer = 0
            Dim outerRowIndex As Integer = 0
            Dim innerRowIndex As Integer = 0
            Dim theStoreListForItem As String = Nothing

            Dim theUploadValueForStoreNo_OUTER As UploadValue
            Dim theUploadValueForItemIdentifier_OUTER As UploadValue
            Dim theUploadValueForVendorId_OUTER As UploadValue
            Dim theUploadValueForPrimaryVendorFlag_OUTER As UploadValue

            Dim theUploadValueForStoreNo_INNER As UploadValue
            Dim theUploadValueForItemIdentifier_INNER As UploadValue
            Dim theUploadValueForVendorId_INNER As UploadValue
            Dim theUploadValueForPrimaryVendorFlag_INNER As UploadValue

            Dim isErrorRow As Boolean = False
            Dim gridAndDataRowHolder As GridAndDataRowHolder = Nothing
            Dim thePrimaryVendorGridCell As UltraGridCell = Nothing

            Try

                For Each currentUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList
                    'increment outer row index and reset inner row index
                    outerRowIndex += 1
                    innerRowIndex = 0

                    'reset error row indicator
                    isErrorRow = False

                    'get the grid row for the current data set row
                    gridAndDataRowHolder = currentUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.COST_UPLOAD_CODE)

                    thePrimaryVendorGridCell = Nothing

                    ' do not continue checkinf rows if there is no primary vendor column
                    If Not GridUtilities.TryGetGridCell(EIM_Constants.COST_PRIMARY_VENDOR, gridAndDataRowHolder.GridRow, thePrimaryVendorGridCell) Then
                        Exit For
                    End If

                    ' clear any previous error
                    currentUploadRowHolder.ClearValidationKey("PrimaryVendors")
                    GridUtilities.SetCellStyle(thePrimaryVendorGridCell, "", EIM_Constants.ValidationLevels.Valid, False)

                    'get values from current outer row
                    theUploadValueForItemIdentifier_OUTER = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY)
                    theUploadValueForVendorId_OUTER = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.COST_VENDOR_ATTR_KEY)
                    theUploadValueForStoreNo_OUTER = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.STORE_NO_ATTR_KEY)
                    theUploadValueForPrimaryVendorFlag_OUTER = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.COST_PRIMARY_VENDOR)

                    ' do not check rows where the primary vendor flag is not set
                    If Not IsNothing(theUploadValueForPrimaryVendorFlag_OUTER) AndAlso CBool(theUploadValueForPrimaryVendorFlag_OUTER.Value) Then

                        'check current row against all other rows in upload data set
                        For Each otherUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList
                            innerRowIndex += 1

                            'don't validate against the same row in the data set
                            If innerRowIndex <> outerRowIndex Then
                                'check for the same item/store having more than one
                                ' vendor being set as primary [the store check is conditional based on useGridStore]

                                'get values from current inner row
                                theUploadValueForItemIdentifier_INNER = otherUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY)
                                theUploadValueForVendorId_INNER = otherUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.COST_VENDOR_ATTR_KEY)
                                theUploadValueForStoreNo_INNER = otherUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.STORE_NO_ATTR_KEY)
                                theUploadValueForPrimaryVendorFlag_INNER = otherUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.COST_PRIMARY_VENDOR)

                                ' only compare with rows where the primary vendor flag is set to true
                                If Not IsNothing(theUploadValueForPrimaryVendorFlag_INNER) AndAlso CBool(theUploadValueForPrimaryVendorFlag_INNER.Value) Then

                                    'check for same item
                                    If Not IsNothing(theUploadValueForItemIdentifier_INNER) AndAlso Not IsNothing(theUploadValueForItemIdentifier_OUTER) _
                                        AndAlso theUploadValueForItemIdentifier_INNER.Value.Equals(theUploadValueForItemIdentifier_OUTER.Value) Then

                                        'check for different vendors
                                        If Not Object.Equals(theUploadValueForVendorId_INNER.Value, theUploadValueForVendorId_OUTER.Value) Then
                                            'do we need to check the store on this row as well?
                                            If useGridStore Then
                                                'check for same store
                                                If Not IsNothing(theUploadValueForStoreNo_INNER) AndAlso Not IsNothing(theUploadValueForStoreNo_OUTER) _
                                                    AndAlso theUploadValueForStoreNo_INNER.Value.Equals(theUploadValueForStoreNo_OUTER.Value) Then

                                                    isErrorRow = True
                                                End If
                                            Else
                                                ' if the user is not uploading to the store on the grid row
                                                ' then they are uploading all rows to the same set
                                                ' of stores they selected in the store selector
                                                ' this being the case, we do not need to compare stores
                                                ' at the grid row level
                                                isErrorRow = True
                                            End If 'do we need to check the store on this row as well?
                                        End If 'check for different vendors
                                    End If 'check for same item
                                End If ' only compare with rows where the primary vendor flag is set to true
                            End If 'don't validate against the same row in the data set

                            If isErrorRow Then
                                'this outer row is the same as another row
                                theErrorCount += 1

                                'mark outer row as an error row and skip out of inner for loop
                                currentUploadRowHolder.AddValidationKey("PrimaryVendor", "Multiple Primary Vendors", EIM_Constants.ValidationLevels.Invalid)
                                SetRowValidationLevel(currentUploadRowHolder)

                                'update the primary vendor cell to inform user of error
                                GridUtilities.SetCellStyle(thePrimaryVendorGridCell, "Only one vendor can be set as primary for an item/store in the grid.", EIM_Constants.ValidationLevels.Invalid)

                                Exit For
                            End If ' isErrorRow
                        Next
                    End If ' do not check rows where the primary vendor flag is not set
                Next

            Finally
                Me.ProgressComplete = True
                Me.ProgressCounter = 0
            End Try

            Return theErrorCount

        End Function

        ''' <summary>
        ''' Verify that when delete_vendor or deauth_store is flagged for primary vendor, 
        ''' each item/store combination has no more than one secondary vendor
        ''' </summary>
        ''' <param name="inStoreList">This is a comma delimeted list of store no
        ''' generated from the user's  selection in the cost upload store selector.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Public Function ValidateForSecondaryVendor() As Integer


            Dim inStoreList As String = Me.EIMManager.EIMForm.StoreSelectorCostUpload.SelectedStoreIdString

            Me.ProgressComplete = False
            Me.ProgressCounter = 0

            Dim theErrorCount As Integer = 0
            Dim Delete_Vendor As Integer = 0
            Dim Deauth_Store As Integer = 0

            Dim theStoreListForItem As String = Nothing

            Dim theUploadValueForStoreNo As UploadValue
            'Dim theUploadValueForItemKey As UploadValue
            Dim theUploadValueForVendorId As UploadValue
            Dim theUploadValueForDeleteVendorFlag As UploadValue
            Dim theUploadValueForDeauthStoreFlag As UploadValue

            Dim isErrorRow As Boolean = False
            Dim isWarningDeauthRow As Boolean = False
            Dim isWarningSwapRow As Boolean = False

            ' warnings below are not for the individual rows but for the whole session to appear in the message box "EIM - Upload Session"
            ' in Private sub SessionUpload() in EIMForm.vb
            Me.EIMManager.CurrentUploadSession.WarningCountItemDeauthForAllStores = 0
            Me.EIMManager.CurrentUploadSession.WarningCountItemDeauthForStore = 0
            Me.EIMManager.CurrentUploadSession.WarningCountPrimaryVendorSwap = 0


            Dim ReturnWarningsError(2) As Integer

            Try

                For Each currentUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList

                    ' clear any previous error
                    currentUploadRowHolder.ClearValidationKey("DeauthorizeVendor")
                    currentUploadRowHolder.ClearValidationKey("SwapVendor")
                    currentUploadRowHolder.ClearValidationKey("SecondaryVendor")
                    SetRowValidationLevel(currentUploadRowHolder)

                    'reset error row indicator
                    isErrorRow = False
                    isWarningDeauthRow = False
                    isWarningSwapRow = False

                    Delete_Vendor = 0
                    Deauth_Store = 0

                    theUploadValueForDeleteVendorFlag = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.COST_DELETE_VENDOR)
                    theUploadValueForDeauthStoreFlag = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.COST_DEAUTH_STORE)

                    If Not IsNothing(theUploadValueForDeleteVendorFlag) AndAlso CBool(theUploadValueForDeleteVendorFlag.Value) Then
                        Delete_Vendor = 1
                    End If

                    If Not IsNothing(theUploadValueForDeauthStoreFlag) AndAlso CBool(theUploadValueForDeauthStoreFlag.Value) Then
                        Deauth_Store = 1
                    End If

                    ' only check rows where the delete vendor flag or deauth store flag is set to true

                    If Delete_Vendor = 1 Or Deauth_Store = 1 Then

                        'get values from current row
                        theUploadValueForVendorId = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.COST_VENDOR_ATTR_KEY)
                        theUploadValueForStoreNo = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.STORE_NO_ATTR_KEY)


                        ' if inStoreList is Nothing then the user wants to delete vendor (deauth store) 
                        ' for the store in the UplodRow
                        If IsNothing(inStoreList) Or String.IsNullOrEmpty(inStoreList) Then

                            ' make sure there is a store in the UplodRow
                            If Not IsNothing(theUploadValueForStoreNo) Then
                                theStoreListForItem = theUploadValueForStoreNo.Value
                            End If
                        Else
                            ' use the store selector store list
                            theStoreListForItem = inStoreList
                        End If

                        ' there's got to be a store!
                        If Not IsNothing(theStoreListForItem) Or String.IsNullOrEmpty(inStoreList) Then
                            'call stored procedure here to check if duplicate secondary vendors exist for item/store

                            ReturnWarningsError = EIMUtilityDAO.Instance.ValidateDeleteVendor(currentUploadRowHolder.UploadRow.ItemKey, theUploadValueForVendorId.Value, theStoreListForItem, Delete_Vendor)

                            If ReturnWarningsError(0) > 0 Then
                                isWarningDeauthRow = True

                                If Delete_Vendor = 1 Then
                                    Me.EIMManager.CurrentUploadSession.WarningCountItemDeauthForAllStores += 1
                                Else
                                    Me.EIMManager.CurrentUploadSession.WarningCountItemDeauthForStore += 1
                                End If

                            End If

                            If ReturnWarningsError(1) > 0 Then
                                isWarningSwapRow = True
                                Me.EIMManager.CurrentUploadSession.WarningCountPrimaryVendorSwap += 1
                            End If

                            If ReturnWarningsError(2) > 0 Then
                                isErrorRow = True
                            End If


                        End If

                    End If ' only check rows where the delete vendor flag or deauth store flag is set to true

                    If isErrorRow Then
                        theErrorCount += 1
                        'mark row as an error row
                        currentUploadRowHolder.AddValidationKey("SecondaryVendor", "Multiple Secondary Vendors", EIM_Constants.ValidationLevels.Invalid)
                        SetRowValidationLevel(currentUploadRowHolder)
                    End If

                    If isWarningDeauthRow Then
                        'mark row as an warning row
                        currentUploadRowHolder.AddValidationKey("DeauthorizeVendor", "Item Will Be Deauthorized", EIM_Constants.ValidationLevels.Warning)
                        SetRowValidationLevel(currentUploadRowHolder)
                    End If

                    If isWarningSwapRow Then
                        'mark row as an warning row
                        currentUploadRowHolder.AddValidationKey("SwapVendor", "Primary Vendor Will Be Swapped", EIM_Constants.ValidationLevels.Warning)
                        SetRowValidationLevel(currentUploadRowHolder)

                    End If

                Next

            Finally
                Me.ProgressComplete = True
                Me.ProgressCounter = 0
            End Try

            Return theErrorCount

        End Function


        ''' <summary>
        ''' verify if some price collision exists right before the upload
        ''' </summary>
        ''' <param name=""></param>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Public Function ValidateForPricePreUploadCollision() As Integer

            Me.ProgressComplete = False
            Me.ProgressCounter = 0

            'depends on the upload type. Here it's price so it's storeSelectorPriceUpload...
            Dim inStoreList As String = Me.EIMManager.EIMForm.StoreSelectorPriceUpload.SelectedStoreIdString

            Dim theUploadValueForStoreNo As UploadValue

            Dim Item_Key As Integer
            Dim Identifier As String = String.Empty
            Dim theStoreListForItem As String = String.Empty
            Dim PriceChgTypeID As UploadValue = Nothing
            Dim PriceStartDate As UploadValue = Nothing
            Dim Sale_Start_Date As UploadValue = Nothing
            Dim Sale_End_Date As UploadValue = Nothing
            Dim Multiple As UploadValue = Nothing
            Dim POSPrice As UploadValue = Nothing
            Dim MSRPPrice As UploadValue = Nothing
            Dim MSRPMultiple As UploadValue = Nothing
            Dim POSSale_Price As UploadValue = Nothing
            Dim Sale_Multiple As UploadValue = Nothing
            Dim isPriceChange As UploadValue = Nothing

            Dim ReturnErrorMessage As String = ""
            Dim theErrorCount As Integer = 0

            Try
                For Each currentUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList

                    ' clear any previous error
                    currentUploadRowHolder.ClearValidationKey("PriceUploadCollision")
                    SetRowValidationLevel(currentUploadRowHolder)

                    Item_Key = currentUploadRowHolder.UploadRow.ItemKey
                    Identifier = currentUploadRowHolder.UploadRow.Identifier
                    theUploadValueForStoreNo = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.STORE_NO_ATTR_KEY)
                    PriceChgTypeID = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY)
                    PriceStartDate = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_START_DATE_ATTR_KEY)
                    Sale_Start_Date = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_SALE_START_DATE_ATTR_KEY)
                    Sale_End_Date = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_SALE_END_DATE_ATTR_KEY)
                    Multiple = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_MULTIPLE_ATTR_KEY)
                    POSPrice = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_ATTR_KEY)
                    MSRPPrice = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_MSRP_PRICE_ATTR_KEY)
                    MSRPMultiple = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_MSRP_MULTIPLE_ATTR_KEY)
                    POSSale_Price = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_PROMO_ATTR_KEY)
                    Sale_Multiple = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_PROMO_MULTIPLE_ATTR_KEY)
                    isPriceChange = currentUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_IS_CHANGE_ATTR_KEY)


                    Dim PriceStartDateValue As String = String.Empty
                    If Not IsNothing(PriceStartDate) Then
                        PriceStartDateValue = PriceStartDate.Value
                    End If

                    Dim Sale_Start_DateValue As String = String.Empty
                    If Not IsNothing(Sale_Start_Date) Then
                        Sale_Start_DateValue = Sale_Start_Date.Value
                    End If

                    Dim Sale_End_DateValue As String = String.Empty
                    If Not IsNothing(Sale_End_Date) Then
                        Sale_End_DateValue = Sale_End_Date.Value
                    End If

                    Dim MultipleValue As Integer
                    If Not IsNothing(Multiple) Then
                        MultipleValue = Multiple.Value
                    End If

                    Dim POSPriceValue As String = String.Empty
                    If Not IsNothing(POSPrice) Then
                        POSPriceValue = POSPrice.Value
                    End If

                    Dim MSRPPriceValue As String = String.Empty
                    If Not IsNothing(MSRPPrice) Then
                        MSRPPriceValue = MSRPPrice.Value
                    End If

                    Dim MSRPMultipleValue As Integer
                    If Not IsNothing(MSRPMultiple) Then
                        MSRPMultipleValue = MSRPMultiple.Value
                    End If

                    Dim POSSale_PriceValue As String = String.Empty
                    If Not IsNothing(POSSale_Price) Then
                        POSSale_PriceValue = POSSale_Price.Value
                    End If

                    Dim Sale_MultipleValue As Integer
                    If Not IsNothing(Sale_Multiple) Then
                        Sale_MultipleValue = Sale_Multiple.Value
                    End If

                    Dim isPriceChangeValue As Boolean
                    If Not IsNothing(isPriceChange) Then
                        isPriceChangeValue = isPriceChange.Value
                    End If


                    ' if inStoreList is Nothing then the user wants to make changes for the store in the UplodRow
                    If IsNothing(inStoreList) Or String.IsNullOrEmpty(inStoreList) Then

                        ' make sure there is a store in the UplodRow
                        If Not IsNothing(theUploadValueForStoreNo) Then
                            theStoreListForItem = theUploadValueForStoreNo.Value
                        End If
                    Else
                        ' use the store selector store list
                        theStoreListForItem = inStoreList
                    End If

                    ' there's got to be a store!
                    If Not IsNothing(theStoreListForItem) Or Not String.IsNullOrEmpty(inStoreList) Then

                        ReturnErrorMessage = EIMUtilityDAO.Instance.ValidateForPriceUploadCollision(Item_Key, _
                                                            Identifier, _
                                                            theStoreListForItem, _
                                                            PriceChgTypeID.Value, _
                                                            PriceStartDateValue, _
                                                            Sale_Start_DateValue, _
                                                            Sale_End_DateValue, _
                                                            MultipleValue, _
                                                            POSPriceValue, _
                                                            MSRPPriceValue, _
                                                            MSRPMultipleValue, _
                                                            POSSale_PriceValue, _
                                                            Sale_MultipleValue, _
                                                            isPriceChangeValue)


                    End If

                    If Not String.IsNullOrEmpty(ReturnErrorMessage) Then

                        theErrorCount += 1
                        'mark row as an error row
                        currentUploadRowHolder.AddValidationKey("PriceUploadCollision", ReturnErrorMessage, EIM_Constants.ValidationLevels.Invalid)
                        SetRowValidationLevel(currentUploadRowHolder)

                    End If

                Next

            Catch ex As Exception

            Finally
                Me.ProgressComplete = True
                Me.ProgressCounter = 0
            End Try

            Return theErrorCount

        End Function



        Private Function InternalValidatePriceChanges(ByRef inUploadRowHolder As UploadRowHolder, _
                 ByVal inStoreList As String, _
                 ByVal useItemsStoreInRow As Boolean) As Integer

            Dim isValid As Boolean = True
            Dim theValidationMessage As String = ""
            Dim theUploadRow As UploadRow = inUploadRowHolder.UploadRow
            Dim theErrorCount As Integer = 0

            Dim isExistingUnprocessedBatch As Boolean = False
            Dim theValidationErrorCode As Integer = 0
            Dim theValidationErrorMessage As String = ""
            Dim hasPrimaryVendorSetInUpload As Boolean = False
            Dim isNoPrimaryVendorError As Boolean = False

            Dim thePricesGridAndDataRowHolder As GridAndDataRowHolder = _
                 inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.PRICE_UPLOAD_CODE)

            Dim theCostsGridAndDataRowHolder As GridAndDataRowHolder = _
                inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.COST_UPLOAD_CODE)

            If Not IsNothing(thePricesGridAndDataRowHolder) Then

                Dim thePriceChangeTypeGridCell As UltraGridCell = Nothing
                Dim thePrimaryVendorGridCell As UltraGridCell = Nothing

                If Not IsNothing(theCostsGridAndDataRowHolder) AndAlso _
                        GridUtilities.TryGetGridCell(EIM_Constants.COST_PRIMARY_VENDOR, _
                        theCostsGridAndDataRowHolder.GridRow, thePrimaryVendorGridCell) Then

                    hasPrimaryVendorSetInUpload = CBool(thePrimaryVendorGridCell.Value)
                End If

                If GridUtilities.TryGetGridCell(EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY, _
                        thePricesGridAndDataRowHolder.GridRow, thePriceChangeTypeGridCell) Then

                    ' clear any existing error
                    GridUtilities.SetCellStyle(thePriceChangeTypeGridCell, "", EIM_Constants.ValidationLevels.Valid, False)
                    inUploadRowHolder.ClearValidationKey("PriceChange")

                    Dim theOutputList As ArrayList = EIMUtilityDAO.Instance.ValidatePriceChange(inUploadRowHolder.UploadRow, _
                            inStoreList, theValidationErrorCode, theValidationErrorMessage)

                    Dim theValidationLevelValue As Integer = CInt(theOutputList(0))
                    Dim inOutValidationCode As Integer = CInt(theOutputList(1))
                    Dim inOutValidationMessage As String = CStr(theOutputList(2))

                    Dim theValidationLevel As EIM_Constants.ValidationLevels = _
                            CType([Enum].Parse(GetType(EIM_Constants.ValidationLevels), theValidationLevelValue.ToString()), EIM_Constants.ValidationLevels)

                    If theValidationLevelValue > 0 Then

                        isNoPrimaryVendorError = theValidationErrorCode = EIM_Constants.REG_PRICE_VALIDATION_CODE_NO_PRIMARY_VENDOR Or _
                                theValidationErrorCode = EIM_Constants.PROMO_PRICE_VALIDATION_CODE_NO_PRIMARY_VENDOR

                        If Not isNoPrimaryVendorError Or _
                            (isNoPrimaryVendorError And Not hasPrimaryVendorSetInUpload) Then

                            theErrorCount = theErrorCount + 1

                            ' set the error on the price change type cell
                            GridUtilities.SetCellStyle(thePriceChangeTypeGridCell, theValidationErrorMessage, theValidationLevel)

                            'inUploadRowHolder.AddValidationKey("PriceChange", "Pending Price Change Issue", theValidationLevel)
                            inUploadRowHolder.AddValidationKey("PriceChange", inOutValidationMessage, theValidationLevel)

                            SetRowValidationLevel(inUploadRowHolder)

                        End If
                    End If
                End If
            End If

            Return theErrorCount

        End Function

        Private Function InternalValidateExistingItemJurisdiction(ByRef inUploadRowHolder As UploadRowHolder) As Integer

            Dim isValid As Boolean = True
            Dim theValidationMessage As String = ""
            Dim theUploadRow As UploadRow = inUploadRowHolder.UploadRow
            Dim theErrorCount As Integer = 0

            Dim theValidationErrorCode As Integer = 0
            Dim theValidationErrorMessage As String = ""

            Dim theItemGridAndDataRowHolder As GridAndDataRowHolder = _
                 inUploadRowHolder.GetGridAndDataRowByUploadType(EIM_Constants.ITEM_MAINTENANCE_CODE)

            If Not IsNothing(theItemGridAndDataRowHolder) Then

                Dim theIsDefaultJurisdictionGridCell As UltraGridCell = Nothing
                Dim theJurisdictionIdGridCell As UltraGridCell = Nothing

                Dim theJurisdictionId As Integer
                Dim isDefaultJurisdiction As Boolean

                ' the jurisdiction column must be in the item grid to do any validation!
                If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_JURISDICTION_ID_ATTR_KEY, _
                        theItemGridAndDataRowHolder.GridRow, theJurisdictionIdGridCell) Then

                    ' clear any previous error
                    inUploadRowHolder.ClearValidationKey("ExistingJurisdiction")
                    GridUtilities.SetCellStyle(theIsDefaultJurisdictionGridCell, "", _
                        EIM_Constants.ValidationLevels.Valid, False)
                    GridUtilities.SetCellStyle(theJurisdictionIdGridCell, "", _
                        EIM_Constants.ValidationLevels.Valid, False)

                    theJurisdictionId = GridUtilities.GetGridCellIntegerValue(theJurisdictionIdGridCell)

                    ' try to get the default jurisdiction flag value
                    If GridUtilities.TryGetGridCell(EIM_Constants.ITEM_IS_DEFAULT_JURISDICTION_ATTR_KEY, _
                            theItemGridAndDataRowHolder.GridRow, theIsDefaultJurisdictionGridCell) Then

                        isDefaultJurisdiction = GridUtilities.GetGridCellBooleanValue(theIsDefaultJurisdictionGridCell)
                    Else
                        ' if the default jurisdiction column is not in the item grid default
                        ' the flag to true
                        isDefaultJurisdiction = True
                    End If

                    If isDefaultJurisdiction AndAlso EIMUtilityDAO.Instance.ValidateJurisdiction(inUploadRowHolder.UploadRow.ItemKey, _
                            isDefaultJurisdiction, theJurisdictionId) = EIM_Constants.ValidationLevels.Invalid Then

                        theErrorCount = theErrorCount + 1

                        theValidationErrorMessage = "You cannot change the default of an item."

                        GridUtilities.SetCellStyle(theIsDefaultJurisdictionGridCell, theValidationErrorMessage, _
                            EIM_Constants.ValidationLevels.Invalid)

                        GridUtilities.SetCellStyle(theJurisdictionIdGridCell, theValidationErrorMessage, _
                            EIM_Constants.ValidationLevels.Invalid)

                        inUploadRowHolder.AddValidationKey("ExistingJurisdiction", "Invalid Jurisdiction Assignment", EIM_Constants.ValidationLevels.Invalid)
                        SetRowValidationLevel(inUploadRowHolder)

                    End If
                End If
            End If

            Return theErrorCount

        End Function

#End Region

#Region "Utility Methods"

        Private Function GetItemIdentifier(ByRef inGridRow As UltraGridRow) As String

            Dim theIdentifier As String = GridUtilities.GetGridCellStringValue(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY, inGridRow)

            Return theIdentifier
        End Function

        ''' <summary>
        ''' This replicates logic in the db to avoid having to make
        ''' a time consuming db trip for each row in the session.
        ''' </summary>
        ''' <param name="inIdentifier"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsNewItemScaleIdentifier(ByVal inIdentifier As String) As Boolean

            Dim isScaleIdentifier As Boolean = False

            Dim theRegex As New Regex("2[\d]{5}00000")

            Return theRegex.Match(inIdentifier).Success

        End Function

        Private Sub EnableAllCells(ByVal inUploadTypeCode As String, _
                ByRef inGridAndDataRowHolder As GridAndDataRowHolder)

            Dim theUploadRow As UploadRow = Me.EIMManager.CurrentUploadRowHolderCollecton. _
                    GetUploadRowHolderForUploadRowId(inGridAndDataRowHolder.UploadRowId).UploadRow

            Dim theUploadTypeAttribute As UploadTypeAttribute = Nothing

            For Each theGridCell As UltraGridCell In inGridAndDataRowHolder.GridRow.Cells

                theUploadTypeAttribute = theUploadRow.FindUploadTypeAttribute(inUploadTypeCode, theGridCell.Column.Key)

                ' only enable cells configured to be not read-only
                If Not IsNothing(theUploadTypeAttribute) Then
                    If theUploadTypeAttribute.GroupName.Equals(EIM_Constants.GROUP_HIERARCHY_DATA_KEY) OrElse _
                          theGridCell.Column.Key.Equals(EIM_Constants.ITEM_CHAINS_ATTR_KEY) OrElse _
                           theGridCell.Column.Key.Equals(EIM_Constants.COST_DISCOUNT_ATTR_KEY) OrElse _
                           theUploadTypeAttribute.UploadAttribute.Size > EIM_Constants.LONG_TEXT_SIZE Then

                        theGridCell.Appearance.BackColor = EIM_Constants.GRID_CELL_BACKGROUND_COLOR_CUSTOMPOPUP
                        theGridCell.Activation = Activation.NoEdit
                        theGridCell.Appearance.ForeColor = Color.Blue
                        theGridCell.Appearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True
                        theGridCell.Appearance.AlphaLevel = 0
                        theGridCell.Appearance.Cursor = Cursors.Hand
                    ElseIf Me.EIMManager.CurrentUploadSession.IsReadOnly(theUploadTypeAttribute) Then
                        theGridCell.Activation = Activation.NoEdit
                        theGridCell.Appearance.BackColor = EIM_Constants.GRID_CELL_BACKGROUND_COLOR_DISABLED
                        theGridCell.Appearance.AlphaLevel = 0
                    Else
                        GridUtilities.EnableCell(inGridAndDataRowHolder.GridRow, theGridCell.Column.Key)
                    End If
                End If
            Next

        End Sub

        Private Sub DisableItemGridCellsExcept(ByRef inGridAndDataRowHolder As GridAndDataRowHolder, _
        ByVal inAttributeKeys As String())

            Dim theUploadRow As UploadRow = Me.EIMManager.CurrentUploadRowHolderCollecton. _
                    GetUploadRowHolderForUploadRowId(inGridAndDataRowHolder.UploadRowId).UploadRow

            Dim doDisable As Boolean

            For Each theGridCell As UltraGridCell In inGridAndDataRowHolder.GridRow.Cells

                ' don't change the identifier cell
                If Not theGridCell.Column.Key.Equals(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY) Then
                    doDisable = True
                    For Each theKey As String In inAttributeKeys
                        If theGridCell.Column.Key.Equals(theKey) Then
                            doDisable = False
                            Exit For
                        End If
                    Next

                    If doDisable Then
                        ' only disable cells that map to configured UploadAttributes
                        If Not IsNothing(theUploadRow.FindUploadValueByAttributeKey(theGridCell.Column.Key)) Then
                            GridUtilities.DisableCell(inGridAndDataRowHolder.GridRow, theGridCell.Column.Key)
                        End If
                    End If
                End If
            Next

        End Sub


        ''' <summary>
        ''' Set the upload_exclusion column based on validation level of a Upload and grid row.
        ''' </summary>
        ''' <param name="inUploadRowHolder"></param>
        ''' <remarks></remarks>
        Private Sub SetUploadExclusion(ByRef inUploadRowHolder As UploadRowHolder)

            For Each theGridAnddataRowHolder As GridAndDataRowHolder In inUploadRowHolder.GridAndDataRowList

                'Dim upload_exclusionGridCell As UltraGridCell = theGridAnddataRowHolder.GridRow.Cells(EIM_Constants.UPLOAD_EXCLUSION_COLUMN)

                If inUploadRowHolder.ValidationLevel = EIM_Constants.ValidationLevels.Invalid Then

                    inUploadRowHolder.UploadRow.isUploadExclusion = True


                    'GridUtilities.SetUploadExclusionAppearance(upload_exclusionGridCell, "", EIM_Constants.ValidationLevels.Invalid)


                ElseIf inUploadRowHolder.ValidationLevel = EIM_Constants.ValidationLevels.Warning Then

                    inUploadRowHolder.UploadRow.isUploadExclusion = False


                ElseIf inUploadRowHolder.ValidationLevel = EIM_Constants.ValidationLevels.Valid Then

                    inUploadRowHolder.UploadRow.isUploadExclusion = False

                End If

                'Dim value As UploadValue
                'value = inUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.UPLOAD_EXCLUSION_COLUMN)

                'inUploadRowHolder.UploadRow.SetUploadValueByAttributeKey(EIM_Constants.UPLOAD_EXCLUSION_COLUMN)
                'inUploadRowHolder.UploadRow.Save()
                'inUploadRowHolder.UploadRow.MakeNew()

                'value = inUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(EIM_Constants.UPLOAD_EXCLUSION_COLUMN)

            Next

        End Sub




        ''' <summary>
        ''' Set the validation level of a Upload and grid row.
        ''' </summary>
        ''' <param name="inUploadRowHolder"></param>
        ''' <remarks></remarks>
        Private Sub SetRowValidationLevel(ByRef inUploadRowHolder As UploadRowHolder)

            Dim theRowsExistingValidationLevel As Integer
            Dim theValidationLevel As EIM_Constants.ValidationLevels = inUploadRowHolder.ValidationLevel

            ' iterate through all the grid and data rows accross and set the validaton level for each
            For Each theGridAndDataRowHolder As GridAndDataRowHolder In inUploadRowHolder.GridAndDataRowList

                ' only escalate the validation level from warning to error
                ' but not the other direction unless we explicity want to reset
                ' the validation level
                If Not theGridAndDataRowHolder.DataRow.RowState = DataRowState.Deleted And _
                    Not theGridAndDataRowHolder.DataRow.RowState = DataRowState.Detached Then
                    theRowsExistingValidationLevel = CInt(theGridAndDataRowHolder.DataRow.Item(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME))

                    theGridAndDataRowHolder.DataRow.Item(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME) = theValidationLevel

                    inUploadRowHolder.UploadRow.ValidationLevel = theValidationLevel

                    Dim theValidationLevelGridCell As UltraGridCell = theGridAndDataRowHolder.GridRow.Cells(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME)
                    ' clear any existing tooltip
                    theValidationLevelGridCell.ToolTipText = ""

                    ' set the multi line tooltip telling the user's what the warnings and errors are
                    If theValidationLevel <> EIM_Constants.ValidationLevels.Valid Then
                        theValidationLevelGridCell.ToolTipText = "This item has the following validation issues: "

                        If inUploadRowHolder.ValidationWarnings.Count > 0 Then
                            theValidationLevelGridCell.ToolTipText = theValidationLevelGridCell.ToolTipText + Environment.NewLine + _
                                Environment.NewLine + "Warnings:" + Environment.NewLine + "--------------"
                        End If

                        For Each theValidationWarningDescription As String In inUploadRowHolder.ValidationWarnings.Values
                            theValidationLevelGridCell.ToolTipText = theValidationLevelGridCell.ToolTipText + Environment.NewLine + " - " + theValidationWarningDescription
                        Next

                        If inUploadRowHolder.ValidationErrors.Count > 0 Then
                            theValidationLevelGridCell.ToolTipText = theValidationLevelGridCell.ToolTipText + Environment.NewLine + _
                                Environment.NewLine + "Errors:" + Environment.NewLine + "--------------"
                        End If

                        For Each theValidationErrorDescription As String In inUploadRowHolder.ValidationErrors.Values
                            theValidationLevelGridCell.ToolTipText = theValidationLevelGridCell.ToolTipText + Environment.NewLine + " - " + theValidationErrorDescription
                        Next

                        theValidationLevelGridCell.ToolTipText = theValidationLevelGridCell.ToolTipText + Environment.NewLine + Environment.NewLine + "------------------------------"
                    End If

                End If
            Next

        End Sub

        Private Function FindIntegerValueInDataSetForUploadRow(ByRef inDataSet As DataSet, ByVal inUploadRowId As Integer, _
                 ByVal inAttributeKey As String, ByRef inOutValue As Integer) As Boolean

            Dim theValue As Object = FindValueInDataSetForUploadRow(inDataSet, inUploadRowId, inAttributeKey)

            If theValue Is DBNull.Value Then
                inOutValue = 0
            Else
                inOutValue = CInt(theValue)
            End If

            Return Not IsNothing(inOutValue)

        End Function

        Private Function FindDecimalValueInDataSetForUploadRow(ByRef inDataSet As DataSet, ByVal inUploadRowId As Integer, _
                ByVal inAttributeKey As String, ByRef inOutValue As Decimal) As Boolean

            Dim theValue As Object = FindValueInDataSetForUploadRow(inDataSet, inUploadRowId, inAttributeKey)

            If theValue Is DBNull.Value Then
                inOutValue = 0
            Else
                inOutValue = CDec(theValue)
            End If

            Return Not IsNothing(inOutValue)

        End Function

        Private Function FindValueInDataSetForUploadRow(ByRef inDataSet As DataSet, ByVal inUploadRowId As Integer, _
                ByVal inAttributeKey As String) As Object

            Dim theValue As Object = Nothing
            Dim theDataRow As DataRow

            ' find the price change type id from whichever grids it may be in
            ' we are relying on the values being synced across the grids by now
            For Each theDataTable As DataTable In inDataSet.Tables

                theDataRow = theDataTable.Rows.Find(inUploadRowId)

                If theDataTable.Columns.Contains(inAttributeKey) Then
                    theValue = theDataRow.Item(inAttributeKey)
                    Exit For
                End If
            Next

            Return theValue

        End Function

#End Region

#End Region

    End Class

End Namespace
