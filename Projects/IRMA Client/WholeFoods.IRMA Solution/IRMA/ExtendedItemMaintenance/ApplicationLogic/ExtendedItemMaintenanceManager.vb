' Have to turn strict off to use the Excel object model
Option Strict Off

Imports System.IO
Imports System.Text
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.IRMA.ModelLayer.DataAccess
Imports WholeFoods.IRMA.ExtendedItemMaintenance.Resources
Imports WholeFoods.IRMA.Common.DataAccess

Namespace WholeFoods.IRMA.ExtendedItemMaintenance.Logic

    Public Class ExtendedItemMaintenanceManager

#Region "Constant fields"
        Const slimDealDataConst As String = "SLIM Deal Data"
        Const isdealChangeConst As String = "isdealchange"
#End Region

#Region "Fields and Properties"

        Private _extendedItemMaintenanceForm As ExtendedItemMaintenanceForm
        Private _calcValidateManager As CalculatorAndValidatorManager
        Private WithEvents _currentUploadSession As UploadSession = Nothing
        Private _currentUploadRowHolderCollecton As New UploadRowHolderCollection()
        Private _previousUploadSession As UploadSession = Nothing
        Private _attributeNameToColumnIndexMap As New Hashtable
        Private _progressCounter As Integer
        Private _progressComplete As Boolean
        Private _currentFileName As String
        Private _currentExcelWorkSheet As ExcelLibrary.SpreadSheet.Worksheet
        Private _storeJurisdictionErrorCount As Integer = 0
        Private _existingDuplicatePriceChangesCount As Integer = 0
        Private _uploadedDuplicatePriceChangesCount As Integer = 0
        Private _uploadedPrimaryVendorCollisionCount As Integer = 0
        Private _uploadedSecondaryVendorCollisionCount As Integer = 0
        Private _preuploadPriceCollisionCount As Integer = 0
        Private _valueListDataByKeyCollection As New Hashtable
        Private _valueListDataByValueCollection As New Hashtable
        Private _spreadsheetRowCount As Integer = -1
        Private _spreadsheetColumnCount As Integer = -1

        Public Property EIMForm() As ExtendedItemMaintenanceForm
            Get
                Return _extendedItemMaintenanceForm
            End Get
            Set(ByVal value As ExtendedItemMaintenanceForm)
                _extendedItemMaintenanceForm = value
            End Set
        End Property

        Public Property CalcValidateManager() As CalculatorAndValidatorManager
            Get
                Return _calcValidateManager
            End Get
            Set(ByVal value As CalculatorAndValidatorManager)
                _calcValidateManager = value
            End Set
        End Property

        Public Property CurrentUploadSession() As UploadSession
            Get
                If IsNothing(_currentUploadSession) Then
                    _currentUploadSession = New UploadSession()
                    _currentUploadSession.SlimDealDataGroupName = slimDealDataConst
                    _currentUploadSession.IsdealChangeColumnName = isdealChangeConst

                    ' add all  by default
                    _currentUploadSession.SetUploadTypes(UploadTypeDAO.Instance.GetAllUploadTypes())

                    ' default the flag
                    _currentUploadSession.IsNewItemSessionFlag = False

                End If
                Return _currentUploadSession
            End Get
            Set(ByVal value As UploadSession)
                _currentUploadSession = value
            End Set
        End Property

        Private Property PreviousUploadSession() As UploadSession
            Get
                Return _previousUploadSession
            End Get
            Set(ByVal value As UploadSession)
                _previousUploadSession = value
            End Set
        End Property

        Public Property CurrentUploadRowHolderCollecton() As UploadRowHolderCollection
            Get
                Return _currentUploadRowHolderCollecton
            End Get
            Set(ByVal value As UploadRowHolderCollection)
                _currentUploadRowHolderCollecton = value
            End Set
        End Property

        Private ReadOnly Property AttributeNameToColumnIndexMap() As Hashtable
            Get
                Return _attributeNameToColumnIndexMap
            End Get
        End Property

        Public Property ProgressCounter() As Integer
            Get
                Return _progressCounter
            End Get
            Set(ByVal value As Integer)
                _progressCounter = value
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

        Public Property CurrentFileName() As String
            Get
                Return _currentFileName
            End Get
            Set(ByVal value As String)
                _currentFileName = value
            End Set
        End Property

        ''' <summary>
        ''' Used only for importing.
        ''' Refactor to use Infragistics.Excel classes,
        ''' as the export does, when IRMA is upgraded to
        ''' at least Infragistics 2007 Vol 2.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Property CurrentExcelWorksheet() As ExcelLibrary.SpreadSheet.Worksheet
            Get
                If IsNothing(Me._currentExcelWorkSheet) Then

                    Dim theFileStream As Stream = Nothing

                    Try
                        theFileStream = File.OpenRead(Me.CurrentFileName)
                        Dim theExcelWorkBook As ExcelLibrary.SpreadSheet.Workbook = ExcelLibrary.SpreadSheet.Workbook.Load(theFileStream)
                        Me._currentExcelWorkSheet = theExcelWorkBook.Worksheets(0)
                    Finally
                        If Not IsNothing(theFileStream) Then
                            theFileStream.Close()
                        End If
                    End Try
                End If

                Return Me._currentExcelWorkSheet

            End Get
            Set(ByVal value As ExcelLibrary.SpreadSheet.Worksheet)
                Me._currentExcelWorkSheet = value

                If IsNothing(value) Then
                    Me._spreadsheetRowCount = -1
                    Me._spreadsheetColumnCount = -1
                End If
            End Set
        End Property

        ''' <summary>
        ''' Used only for importing.
        ''' Refactor to use Infragistics.Excel classes,
        ''' as the export does, when IRMA is upgraded to
        ''' at least Infragistics 2007 Vol 2.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SpreadSheetRowCount() As Integer
            Get
                If Me._spreadsheetRowCount = -1 Then
                    Me._spreadsheetRowCount = Me.CurrentExcelWorksheet.Cells.LastRowIndex + 1
                End If

                Return Me._spreadsheetRowCount
            End Get
        End Property

        ''' <summary>
        ''' Used only for importing.
        ''' Refactor to use Infragistics.Excel classes,
        ''' as the export does, when IRMA is upgraded to
        ''' at least Infragistics 2007 Vol 2.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SpreadSheetColumnCount() As Integer
            Get
                If Me._spreadsheetColumnCount = -1 Then
                    Me._spreadsheetColumnCount = Me.CurrentExcelWorksheet.Cells.LastColIndex + 1
                End If

                Return Me._spreadsheetColumnCount
            End Get
        End Property

        Public Property PriceChangeErrorCount() As Integer
            Get
                Return _existingDuplicatePriceChangesCount
            End Get
            Set(ByVal value As Integer)
                _existingDuplicatePriceChangesCount = value
            End Set
        End Property

        Public Property StoreJurisdictionErrorCount() As Integer
            Get
                Return _storeJurisdictionErrorCount
            End Get
            Set(ByVal value As Integer)
                _storeJurisdictionErrorCount = value
            End Set
        End Property

        Public Property UploadedDuplicatePriceChangesCount() As Integer
            Get
                Return _uploadedDuplicatePriceChangesCount
            End Get
            Set(ByVal value As Integer)
                _uploadedDuplicatePriceChangesCount = value
            End Set
        End Property

        Public Property UploadedPrimaryVendorCollisionCount() As Integer
            Get
                Return _uploadedPrimaryVendorCollisionCount
            End Get
            Set(ByVal value As Integer)
                _uploadedPrimaryVendorCollisionCount = value
            End Set
        End Property

        Public Property UploadedSecondaryVendorCollisionCount() As Integer
            Get
                Return _uploadedSecondaryVendorCollisionCount
            End Get
            Set(ByVal value As Integer)
                _uploadedSecondaryVendorCollisionCount = value
            End Set
        End Property

        Public Property PreuploadPriceCollisionCount() As Integer
            Get
                Return _preuploadPriceCollisionCount
            End Get
            Set(ByVal value As Integer)
                _preuploadPriceCollisionCount = value
            End Set
        End Property

        Public Property ValueListDataByKeyCollection() As Hashtable
            Get
                Return _valueListDataByKeyCollection
            End Get
            Set(ByVal value As Hashtable)
                _valueListDataByKeyCollection = value
            End Set
        End Property

        Public Property ValueListDataByValueCollection() As Hashtable
            Get
                Return _valueListDataByValueCollection
            End Get
            Set(ByVal value As Hashtable)
                _valueListDataByValueCollection = value
            End Set
        End Property

#End Region

#Region "Shared Methods"

        Public Shared Function GetDataTableFromGrid(ByVal inGrid As UltraGrid, ByVal inUploadTypeCode As String) As DataTable
            Dim theDataTable As DataTable = Nothing

            If Not IsNothing(inGrid.DataSource) Then
                theDataTable = CType(CType(inGrid.DataSource, BindingSource).DataSource, DataSet).Tables(inUploadTypeCode)
            End If

            Return theDataTable
        End Function

        Public Shared Function GetUploadTypeCodeFromGrid(ByVal inGrid As UltraGrid) As String
            Dim theUploadTypeCode As String = Nothing

            If Not IsNothing(inGrid.DataSource) Then
                theUploadTypeCode = CStr(CType(inGrid.DataSource, BindingSource).DataMember)
            End If

            Return theUploadTypeCode
        End Function

#End Region

#Region "Constructors"

        Public Sub New()

            Me.CalcValidateManager = New CalculatorAndValidatorManager(Me)

        End Sub

#End Region

#Region "Public Methods"

        Public Sub BindUploadSessionDataToGrids(ByRef gridCollection As SortableHashlist, ByVal inForImport As Boolean, Optional ByVal isSlimFunctionalityEnabled As Boolean = True)
            BindUploadSessionDataToGrids(gridCollection, inForImport, True, isSlimFunctionalityEnabled)
        End Sub

        Public Sub BindUploadSessionDataToGrids(ByRef gridCollection As SortableHashlist,
                ByVal inForImport As Boolean, ByVal inBuildDataSet As Boolean, Optional ByVal isSlimFunctionalityEnabled As Boolean = True)

            If inBuildDataSet Then
                Me.BuildDataSet(isSlimFunctionalityEnabled)
            End If

            Dim theDataSet As DataSet = Me.CurrentUploadSession.DataSet
            Dim theBindingSource As BindingSource

            ' initialize the grid with the configured columns
            ' and bind the grids to a DataSet created from the imported data
            Dim theGrid As UltraGrid
            For Each theUploadType As UploadType In Me.CurrentUploadSession.UploadTypeCollection

                theGrid = CType(gridCollection.ItemByKey(theUploadType.UploadTypeCode), UltraGrid)

                theGrid.BeginUpdate()

                BuildGridColumns(theGrid, theUploadType.UploadTypeCode, inForImport, isSlimFunctionalityEnabled)

                ' bind the data to the grid
                theGrid.DataSource = theDataSet

                theBindingSource = New BindingSource(theDataSet, theUploadType.UploadTypeCode)
                theGrid.DataSource = theBindingSource

                AutoSizeAllGridColumns(theGrid)

                ' associate the grid rows with their corresponding DataRows to
                ' the UploadRow
                ' the DataRows should already be associated with the UploadRows
                ' when the UploadSession.BuildDataSet() function was called
                Dim theUploadRowId As Integer
                Dim theUploadRowHolder As UploadRowHolder
                Dim theGridAndDataRowHolder As GridAndDataRowHolder

                For Each theGridRow As UltraGridRow In theGrid.Rows

                    theUploadRowId = CInt(theGridRow.Cells(EIM_Constants.UPLOADROW_ID_COLUMN_NAME).Value)
                    theUploadRowHolder = Me.CurrentUploadRowHolderCollecton.GetUploadRowHolderForUploadRowId(theUploadRowId)

                    theGridAndDataRowHolder =
                            theUploadRowHolder.GetGridAndDataRowByUploadType(theUploadType.UploadTypeCode)

                    theGridAndDataRowHolder.GridRow = theGridRow
                Next

                theGrid.EndUpdate()

            Next

        End Sub

        ''' <summary>
        ''' Load items into the current UploadSession from a spreadsheet.
        '''
        ''' Note that importing uses a different class lib
        ''' than exporting. This is because the Infragistics
        ''' version IRMA uses only supports export. Version
        ''' 2007 Vol 2, however, also supports importing. 
        ''' Refactor to use Infragistics.Excel classes,
        ''' as the export does, when IRMA is upgraded to
        ''' at least Infragistics 2007 Vol 2.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SpreadsheetImport()

            Try
                ' there's got to be at least a header and one data row
                If Me.SpreadSheetRowCount < 3 Then
                    MessageBox.Show("The spreadsheet is empty and cannot be imported.", "Extended Item Maintenance Import",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else

                    ' warn the user if the spreadsheet doesn't have all the required columns
                    ' we'll add them to the grid and the underlying DataTables with empty values
                    If Me.CurrentUploadSession.IsNewItemSessionFlag Then
                        CheckForRequiredColumns()
                    End If

                    ' do this to clean up the the rendering if we
                    ' have to show another message box due to unrecognized columns
                    Application.DoEvents()

                    ' warn the user if there are any unrecognized columns
                    ' and don't let the import continue if there isn't
                    ' at least one column
                    If ContainsAtLeastOneRecognizedColumn() Then

                        ' don't allow duplicate columns in the spreadsheet
                        If CheckForDuplicateColumns() Then
                            ImportSpreadsheetIntoUploadSession()
                        End If
                    End If
                End If

            Finally

                ' gotta do the following
                Me.CurrentExcelWorksheet = Nothing
                Me.ProgressComplete = True

            End Try

        End Sub

        Public Sub AddAllRequiredUploadValuesToUploadRows()

            Dim theExistingUploadTypeAttributes As BusinessObjectCollection
            Dim theUploadTypeAttributesToAdd As New ArrayList()

            Dim allUploadTypeAttributes As BusinessObjectCollection = UploadTypeAttributeDAO.Instance.GetAllUploadTypeAttributes()

            ' first let's get a list of all the required UploadAttributes that are not in the Session's
            ' Upload Rows
            For Each theUploadType As UploadType In Me.CurrentUploadSession.UploadTypeCollection
                ' now make sure we also have all the required attributes for the UploadType

                ' get the UploadAttributeTypes and their UploadAttributes for the provided uploadTypeCode
                ' from what's been imported
                theExistingUploadTypeAttributes = Me.CurrentUploadSession.FindUploadTypeAttributesInFirstUploadRowByUploadTypeCode(theUploadType.UploadTypeCode)

                For Each theRequiredUploadTypeAttribute As UploadTypeAttribute In allUploadTypeAttributes

                    If theRequiredUploadTypeAttribute.UploadTypeCode.Equals(theUploadType.UploadTypeCode) AndAlso
                            Me.CurrentUploadSession.IsNewItemSessionFlag And theRequiredUploadTypeAttribute.UploadAttribute.IsRequiredValue Then

                        If Me.CurrentUploadSession.IsAttributeInTemplateForUploadType(theUploadType.UploadTypeCode, theRequiredUploadTypeAttribute.UploadAttribute) Then

                            ' don't add it if it is already there
                            If Not theExistingUploadTypeAttributes.ContainsKey(theRequiredUploadTypeAttribute.PrimaryKey) Then
                                theUploadTypeAttributesToAdd.Add(theRequiredUploadTypeAttribute)
                            End If
                        End If
                    End If
                Next
            Next

            Dim theNewUploadValue As UploadValue

            ' now add the missing UploadValues to each UploadRow
            For Each theUploadRow As UploadRow In Me.CurrentUploadSession.UploadRowCollection
                For Each theRequiredUploadTypeAttribute As UploadTypeAttribute In theUploadTypeAttributesToAdd
                    theNewUploadValue = New UploadValue(theRequiredUploadTypeAttribute.UploadAttribute, theUploadRow)
                Next
            Next

        End Sub

        Public Sub BuildDataSet(Optional ByVal isSlimFunctionalityEnabled As Boolean = True)
            Me.CurrentUploadRowHolderCollecton = Me.CurrentUploadSession.BuildDataSet(isSlimFunctionalityEnabled)
        End Sub

        ''' <summary>
        ''' Load UploadSesion data from the database into the current UploadSession.
        ''' </summary>
        ''' <param name="uploadSessionId"></param>
        ''' <remarks></remarks>
        Public Sub SessionLoad(ByVal uploadSessionId As Integer)

            Me.CurrentUploadSession = UploadSessionDAO.Instance.GetUploadSessionByPK(uploadSessionId)

            ' if we are loading an uploaded session then it cannot be
            ' a new item session
            If Me.CurrentUploadSession.IsUploaded Then
                Me.CurrentUploadSession.IsNewItemSessionFlag = False
            End If

            Me.BuildDataSet()

        End Sub

        ''' <summary>
        ''' Load Items into the current UploadSession from a DataTable of
        ''' items loaded from the database.
        ''' </summary>
        ''' <param name="itemsDataTable"></param>
        ''' <remarks></remarks>
        Public Sub ItemLoad(ByRef itemsDataTable As System.Data.DataTable)

            LoadItemsIntoCurrentUploadSession(itemsDataTable)

        End Sub

        ''' <summary>
        ''' Save the current UploadSession to the database.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SessionSaveMarkedForDelete(ByRef uploadTypeCollection As BusinessObjectCollection)

            ' delete the marked for delete rows and values
            Me.CurrentUploadSession.Delete(True)

        End Sub

        ''' <summary>
        ''' Save the current UploadSession to the database.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SessionSave(ByRef uploadTypeCollection As BusinessObjectCollection)

            ' if the user is saving it we know it hasn't been uploaded yet
            ' because once it is uploaded it will be a new session
            Me.CurrentUploadSession.IsUploaded = False

            ' force the UploadSession to save its
            ' row counts    
            Me.CurrentUploadSession.IsDirty = True
            Me.CurrentUploadSession.Save(giUserID)

        End Sub

        ''' <summary>
        ''' Delete the specified UploadSession from the database.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SessionCascadeDelete(ByVal uploadSessionID As Integer)

            ' this will only load the UploadSession and not all its rows
            ' or values which are only loaded if accessed
            Dim theUploadSession As UploadSession = UploadSessionDAO.Instance.GetUploadSessionByPK(uploadSessionID)

            If Not theUploadSession.IsUploaded Then
                theUploadSession.CascadeDelete()
            End If

        End Sub

        ''' <summary>
        ''' Export items into the current UploadSession to a spreadsheet.
        ''' </summary>
        ''' <param name="gridCollection"></param>
        ''' <param name="uploadTypeCollection"></param>
        ''' <remarks></remarks>
        Public Sub SpreadsheetExport(ByRef gridCollection As SortableHashlist, ByRef uploadTypeCollection As BusinessObjectCollection,
                                     ByRef validationErrors As SortableHashlist, Optional ByVal isSlimFunctionalityEnabled As Boolean = True)

            Dim tempSelectedFile As String = Me.CurrentFileName
            Dim excelBook As Infragistics.Excel.Workbook = Nothing

            Try
                ' export if we have a file name, at least one user selected UploadType,
                ' and at least one item row
                If Not String.IsNullOrEmpty(Me.CurrentFileName) And uploadTypeCollection.Count > 0 And
                        Me.CurrentUploadSession.UploadRowCollection.Count > 0 Then

                    ' this is critical
                    Me.ProgressComplete = False
                    Me.ProgressCounter = 0

                    Me.CurrentUploadSession.LoadFromDataSet(False)

                    excelBook = New Infragistics.Excel.Workbook()
                    excelBook.Worksheets.Add("EIM Export")

                    Dim excelWorksheet As Infragistics.Excel.Worksheet = excelBook.Worksheets(0)

                    ' create a map from the original attribute spreadsheet column postion
                    ' to a new compact position that takes into account that the user
                    ' may not select to not export all upload types
                    Dim theCompactSpreadsheetPositionMap As New Hashtable()
                    Dim theCompactSpreadsheetPosition As Integer = 0
                    Me.CurrentUploadSession.UploadAttributesByName.SortByPropertyValue("SpreadsheetPosition")

                    Dim theUploadAttributes As BusinessObjectCollection

                    ' sort the values by upload type code - Item, Price, Cost
                    uploadTypeCollection.SortByPropertyValue("SortKey")

                    theCompactSpreadsheetPositionMap.Add("Validation Error", 0)

                    'only for the provided UploadTypes
                    For Each theUploadType As UploadType In uploadTypeCollection

                        theUploadAttributes = Me.CurrentUploadSession.FindUploadAttributesInFirstUploadRowByUploadTypeCode(theUploadType.UploadTypeCode, isSlimFunctionalityEnabled)
                        theUploadAttributes.SortByPropertyValue("SpreadsheetPosition")

                        For Each theUploadAttribute As UploadAttribute In theUploadAttributes
                            If Not theCompactSpreadsheetPositionMap.ContainsKey(theUploadAttribute.Key) Then
                                theCompactSpreadsheetPositionMap.Add(theUploadAttribute.Key, theCompactSpreadsheetPosition + 1)
                                theCompactSpreadsheetPosition = theCompactSpreadsheetPosition + 1
                            End If
                        Next
                    Next

                    Dim rowIndex As Integer = 0
                    Dim columnIndex As Integer = 1
                    ' create the header label row
                    ' containing the list of UploadTypes
                    ' this is required since it attributes may be part of
                    ' more than one UploadType and so it cannot 
                    ' always be determined from the attributes what the
                    ' UploadTypes are
                    excelWorksheet.Rows(rowIndex).Cells(0).Value = "Upload Types:"
                    For Each theUploadType As UploadType In Me.CurrentUploadSession.UploadTypeCollection
                        excelWorksheet.Rows(rowIndex).Cells(columnIndex).Value = theUploadType.UploadTypeCode
                        columnIndex = columnIndex + 1
                    Next

                    rowIndex = rowIndex + 1

                    ' create the column label row
                    excelWorksheet.Rows(rowIndex).Cells(0).Value = "Validation Error"
                    Dim allUploadAttributes As BusinessObjectCollection = UploadAttributeDAO.Instance.GetAllUploadAttributes()

                    For Each theUploadAttribute As UploadAttribute In allUploadAttributes

                        If theCompactSpreadsheetPositionMap.ContainsKey(theUploadAttribute.Key) Then
                            theCompactSpreadsheetPosition =
                                    CType(theCompactSpreadsheetPositionMap.Item(theUploadAttribute.Key), Integer)
                            excelWorksheet.Rows(rowIndex).Cells(theCompactSpreadsheetPosition).Value = theUploadAttribute.Name
                        End If
                    Next

                    rowIndex = rowIndex + 1

                    Dim theCellValue As String
                    Dim theValueListData As BusinessObjectCollection
                    Dim theValueListItem As KeyedListItem

                    For Each theUploadRow As UploadRow In Me.CurrentUploadSession.UploadRowCollection
                        Dim theUploadRowHolder As UploadRowHolder = validationErrors.ItemByKey(theUploadRow.UploadRowID)
                        Dim errors As New StringBuilder

                        If Not theUploadRowHolder Is Nothing Then
                            If theUploadRowHolder.ValidationErrors.Count > 0 Then
                                For Each entry As DictionaryEntry In theUploadRowHolder.ValidationErrors
                                    errors.Append(entry.Value).AppendLine()
                                    errors.Append("; ").AppendLine()
                                Next
                            End If
                            excelWorksheet.Rows(rowIndex).Cells(0).Value = errors.ToString()
                        End If

                        For Each theUploadType As UploadType In uploadTypeCollection
                            ' add the data rows with values only
                            ' for the provided UploadTypes
                            If Not theUploadRow.IsMarkedForDelete Then
                                theUploadRow.UploadValueCollection.SortByPropertyValue("SpreadsheetPosition")
                                For Each theUploadValue As UploadValue In theUploadRow.UploadValueCollection
                                    'only for the provided UploadTypes
                                    If Not theUploadValue.IsMarkedForDelete And theUploadValue.IsForUpdateType(theUploadType.UploadTypeCode) And Not theUploadValue.Key = EIM_Constants.UPLOAD_EXCLUSION_COLUMN Then
                                        theCompactSpreadsheetPosition =
                                                CType(theCompactSpreadsheetPositionMap.Item(theUploadValue.Key), Integer)

                                        If theUploadValue.ControlType.ToLower().Equals("valuelist") Then
                                            ' if the control type is a valuelist then
                                            ' export the text value instead of the key value that is stored
                                            ' in the UploadValue.Value property
                                            theValueListData = CType(Me.ValueListDataByKeyCollection.Item(theUploadValue.Key), BusinessObjectCollection)

                                            If Not IsNothing(theValueListData) And Not IsNothing(theUploadValue.Value) Then
                                                If String.IsNullOrEmpty(theUploadValue.Value) Then
                                                    theValueListItem = Nothing
                                                Else
                                                    theValueListItem = CType(theValueListData.ItemByKey(theUploadValue.Value), KeyedListItem)
                                                End If

                                                If Not IsNothing(theValueListItem) Then
                                                    theCellValue = Trim(theValueListItem.Value)
                                                Else
                                                    theCellValue = ""
                                                End If
                                            Else
                                                theCellValue = ""
                                            End If
                                        Else
                                            theCellValue = theUploadValue.TranslateUploadValueForExport()
                                        End If

                                        'If Not theUploadValue.UploadAttribute.IsNumeric() Then
                                        '    ' format as text if not numeric
                                        '    excelWorksheet.Rows(rowIndex).Cells(theCompactSpreadsheetPosition).CellFormat.FormatString = "@"
                                        'End If

                                        ' set the cell value
                                        excelWorksheet.Rows(rowIndex).Cells(theCompactSpreadsheetPosition).Value = theCellValue

                                        'If theUploadValue.Key Then
                                        If theUploadRowHolder.ValidationErrors.Count > 0 Then
                                            For Each entry As DictionaryEntry In theUploadRowHolder.ValidationErrors
                                                If theUploadValue.Key = entry.Key.ToString() Then
                                                    excelWorksheet.Rows(rowIndex).Cells(theCompactSpreadsheetPosition).CellFormat.FillPatternBackgroundColor = Color.Red
                                                    Exit For
                                                End If
                                            Next
                                        End If

                                    End If
                                Next
                            End If
                        Next

                        If Not theUploadRow.IsMarkedForDelete Then
                            rowIndex = rowIndex + 1
                            ' increment the progress count
                            Me.ProgressCounter = Me.ProgressCounter + 1
                        End If

                    Next

                    ' if there is an original file then delete it
                    If File.Exists(Me.CurrentFileName) Then
                        File.Delete(Me.CurrentFileName)
                    End If

                    ' save the new file
                    excelBook.Save(Me.CurrentFileName)
                End If
            Finally
                ' this is critical
                Me.ProgressComplete = True
            End Try

        End Sub

        Public Sub BackUpCurrentUploadSession()

            Me.PreviousUploadSession = Me.CurrentUploadSession

        End Sub

        Public Sub RestorePreviousUploadSession()

            Me.CurrentUploadSession = Me.PreviousUploadSession

        End Sub

        Public Sub AutoSizeAllGridColumns(ByRef inGrid As UltraGrid)

            For Each theGridColumn As UltraGridColumn In inGrid.DisplayLayout.Bands(0).Columns
                Dim theUploadAttribute As UploadAttribute =
                        Me.CurrentUploadSession.FindUploadAttributeByKeyForSessionUploadTypes(theGridColumn.Key)

                If Not IsNothing(theUploadAttribute) AndAlso
                        theUploadAttribute.Size > EIM_Constants.LONG_TEXT_SIZE Then
                    ' fix the log text columns' width
                    theGridColumn.Width = EIM_Constants.LONG_TEXT_COLUMN_WIDTH
                Else
                    theGridColumn.PerformAutoResize()
                End If
            Next

        End Sub

        Public Sub Validate(ByVal inValidationType As ValidationTypes, ByVal inJustStores As Boolean)

            Me.CalcValidateManager.Validate(inValidationType, inJustStores)

        End Sub

        ''' <summary>
        ''' Run validations that must be done just before uploading
        ''' because they take too long to do during data entry.
        ''' These include:
        '''     - Price change collisions (with existing pending)
        '''     - Dupe price changes in grid
        '''     - Invalid primary vendor settings in grid
        '''     - Invalid jurisdiction settings in grid
        ''' </summary>
        ''' <param name="inStoreList"></param>
        ''' <param name="useGridStore"></param>
        ''' <remarks></remarks>
        Public Sub ValidateForUpload(ByVal inStoreList As String, ByVal useGridStore As Boolean)

            Me.CurrentUploadSession.LoadFromDataSet(False)

            'validate for primary vendor collisions
            If Not IsNothing(Me.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.PRICE_UPLOAD_CODE, True)) Then
                'validate for duplicate price changes or no primary vendor
                Me.PriceChangeErrorCount = Me.CalcValidateManager.ValidatePriceChanges(inStoreList)

                'validate pending upload data against itself (other records in the data set)
                Me.UploadedDuplicatePriceChangesCount = Me.CalcValidateManager.ValidateForInGridDuplicatePriceChanges(useGridStore)


                'Error messages are processed in ValidatePriceChanges now
                '    Me.PreuploadPriceCollisionCount = Me.CalcValidateManager.ValidateForPricePreUploadCollision()
                'End If

            End If

            'validate for primary vendor collisions
            If Not IsNothing(Me.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.COST_UPLOAD_CODE, True)) Then
                Me.UploadedPrimaryVendorCollisionCount = Me.CalcValidateManager.ValidateForPrimaryVendor(useGridStore)
            End If

            'validate for secondary vendor collisions 
            'in case of delete_vendor or deauth_store flags checked see if more than one secondary vendor exists
            If Not IsNothing(Me.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.COST_UPLOAD_CODE, True)) Then
                Me.UploadedSecondaryVendorCollisionCount = Me.CalcValidateManager.ValidateForSecondaryVendor()
            End If

            Dim useStoreJurisdictions As Boolean = InstanceDataDAO.IsFlagActive("UseStoreJurisdictions")
            If useStoreJurisdictions Then
                'validate store jurisdiction if the instance data flag is set
                If Me.CurrentUploadSession.IsNewItemSessionFlag Then
                    Me.CalcValidateManager.ValidateNewItemJurisdictions()
                Else
                    Me.CalcValidateManager.ValidateExistingItemJurisdiction()
                End If
            End If

            Me.CurrentUploadSession.DataSet.AcceptChanges()

            Me.CalcValidateManager.ProgressComplete = True

        End Sub

        Public Sub ValidateGridRowsForUploadRow(ByVal inUploadRowHolder As UploadRowHolder, ByVal inValidationType As ValidationTypes)

            Me.CalcValidateManager.ValidateGridRow(inUploadRowHolder, inValidationType, False)

            Me.CurrentUploadSession.DataSet.AcceptChanges()

        End Sub

        ''' <summary>
        ''' Recalculate row values as needed.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RecalculateRowValues(ByRef inGridCell As UltraGridCell)

            ' do the calculation for only certain grid cells
            If inGridCell.Column.Key.Equals(EIM_Constants.PRICE_IS_CHANGE_ATTR_KEY) Then

                Me.CalcValidateManager.UnCheckPromoPlannerCell(inGridCell, Me.CurrentUploadSession.DataSet)

            ElseIf inGridCell.Column.Key.Equals(EIM_Constants.PRICE_MARGIN_ATTR_KEY) Then

                Me.CalcValidateManager.CalculatePrice(inGridCell, Me.CurrentUploadSession.DataSet)

            ElseIf inGridCell.Column.Key.Equals(EIM_Constants.COST_PKG_COST_ATTR_KEY) Or
                                    inGridCell.Column.Key.Equals(EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY) Or
                                    inGridCell.Column.Key.Equals(EIM_Constants.PRICE_ATTR_KEY) Or
                                    inGridCell.Column.Key.Equals(EIM_Constants.PRICE_MULTIPLE_ATTR_KEY) Or
                                    inGridCell.Column.Key.Equals(EIM_Constants.PRICE_PROMO_ATTR_KEY) Or
                                    inGridCell.Column.Key.Equals(EIM_Constants.PRICE_PROMO_MULTIPLE_ATTR_KEY) Or
                                    inGridCell.Column.Key.Equals(EIM_Constants.COST_VEND_PKG_DSCR_ATTR_KEY) Then

                Me.CalcValidateManager.CalculateMargin(inGridCell, Me.CurrentUploadSession.DataSet)

            End If
        End Sub

        Public Sub EmailUploadConfirmation()

            Dim emailClient As WholeFoods.Utility.SMTP.SMTP = Nothing
            Dim emailFrom As String = ConfigurationServices.AppSettings("EIM_FromEmailAddress")
            Dim emailTo As String = ConfigurationServices.AppSettings("EIM_ToEmailAddress")
            Dim emailCC As String = ConfigurationServices.AppSettings("EIM_CCEmailAddress")
            Dim emailMessage As String
            Dim theVersion As String

            If String.IsNullOrEmpty(emailCC) Then
                emailCC = Nothing
            End If

            Dim theMessage As String = String.Format(ResourcesEIM.UploadResults,
                vbCrLf & vbCrLf, Me.CurrentUploadSession.UploadSessionID.ToString(), vbCrLf & vbCrLf, Me.CurrentUploadSession.ItemsProcessedCount, vbCrLf,
                Me.CurrentUploadSession.ErrorsCount, vbCrLf)

            With My.Application.Info.Version
                theVersion = String.Format("{0}.{1}.{2}", .Major, .Minor, .Build) + " " +
                    ConfigurationServices.AppSettings("environment")
            End With

            emailMessage = theMessage + vbCrLf + vbCrLf + String.Format(ResourcesEIM.EmailMessage,
                gsUserName, vbCrLf, Now.ToString("yyyy-MM-dd HH:mm:ss"), vbCrLf & vbCrLf, theVersion, vbCrLf & vbCrLf, emailTo)

            Try
                emailClient = New WholeFoods.Utility.SMTP.SMTP(ConfigurationServices.AppSettings("EmailSMTPServer"))

                emailClient.send(emailMessage, emailTo, emailCC, emailFrom, ResourcesEIM.EmailSubject)

            Catch ex As Exception
                'e-mail confirmation shouldn't be a fatal error!
                ErrorHandler.ProcessError(WholeFoods.Utility.ErrorType.GeneralApplicationError, SeverityLevel.Warning, ex)

                Dim sMsg As String = "Warning!  No confirmation e-mail will be sent due to the following error:"
                If ex.InnerException IsNot Nothing Then
                    sMsg = String.Format("{0}{1}{1} {2} {3}{1} {2} {4}", sMsg, vbCrLf, Chr(149), ex.Message, ex.InnerException.Message)
                    Logger.LogWarning(ex.Message, Me.GetType(), ex.InnerException)
                Else
                    sMsg = String.Format("{0}{1}{1} {2} {3}", sMsg, vbCrLf, Chr(149), ex.Message)
                    Logger.LogWarning(ex.Message, Me.GetType())
                End If
                MsgBox(sMsg, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "ImportData.EmailUploadConfirmation()")

            Finally
                emailClient = Nothing

            End Try

        End Sub

#End Region

#Region "Private Methods"

        Public Sub LoadValueListData(ByVal inForImport As Boolean)

            Dim theValueListDataByKey As BusinessObjectCollection
            Dim theValueListDataByValue As BusinessObjectCollection

            ' get the UploadAttributeTypes and their UploadAttributes for the provided uploadTypeCode
            Dim theUploadTypeAttributes As BusinessObjectCollection

            For Each theUploadType As UploadType In Me.CurrentUploadSession.UploadTypeCollection

                If inForImport Then
                    ' build the columns based on what's been imported
                    theUploadTypeAttributes = Me.CurrentUploadSession.FindUploadTypeAttributesInFirstUploadRowByUploadTypeCode(theUploadType.UploadTypeCode)
                Else
                    ' build the columns based on what has been loaded from either a session
                    ' or an item load
                    theUploadTypeAttributes = UploadTypeAttributeDAO.Instance.GetUploadTypeAttributesByUploadTypeCode(theUploadType.UploadTypeCode)
                End If

                Dim isIdentifierTypeAttribute As Boolean
                ' load value list data for only those attributes in the grid that
                ' have a control type of "ValueList"`
                For Each theUploadTypeAttribute As UploadTypeAttribute In theUploadTypeAttributes

                    If theUploadTypeAttribute.UploadAttribute.ControlType.ToLower().Equals("valuelist") AndAlso
                        Me.CurrentUploadSession.IsAttributeInTemplateForUploadType(theUploadType.UploadTypeCode, theUploadTypeAttribute.UploadAttribute) Then

                        ' always rebuild the identifier type list
                        isIdentifierTypeAttribute = theUploadTypeAttribute.UploadAttribute.Key.Equals(EIM_Constants.ITEMIDENTIFIER_IDENTIFIERTYPE_ATTR_KEY)
                        If Not Me.ValueListDataByKeyCollection.ContainsKey(theUploadTypeAttribute.UploadAttribute.Key) Or
                                isIdentifierTypeAttribute Then

                            ' and remove the identifier type list from
                            ' the list collection if present
                            If isIdentifierTypeAttribute And Me.ValueListDataByKeyCollection.ContainsKey(EIM_Constants.ITEMIDENTIFIER_IDENTIFIERTYPE_ATTR_KEY) Then
                                Me.ValueListDataByKeyCollection.Remove(EIM_Constants.ITEMIDENTIFIER_IDENTIFIERTYPE_ATTR_KEY)
                                Me.ValueListDataByValueCollection.Remove(EIM_Constants.ITEMIDENTIFIER_IDENTIFIERTYPE_ATTR_KEY)
                            End If

                            ' load the data
                            theValueListDataByKey =
                                UploadAttributeDAO.Instance.GetAttributeValueList(theUploadTypeAttribute.UploadAttribute)

                            ' create the cache by value to be
                            ' used below during importing
                            theValueListDataByValue = New BusinessObjectCollection()

                            ' sort it
                            theValueListDataByKey.SortByPropertyValue("Value")

                            ' additionally map it by value in another BusinessObjectCollection
                            ' for use below during importing
                            For Each theKeyedListItem As KeyedListItem In theValueListDataByKey
                                If Not theValueListDataByValue.ContainsKey(theKeyedListItem.Value.Trim().ToLower()) Then
                                    theValueListDataByValue.Add(theKeyedListItem.Value.Trim().ToLower(), theKeyedListItem)
                                End If
                            Next

                            ' if the region is MA and the value  list is for item identifier type
                            ' and the session is for new items then remove the "S" type
                            If InstanceDataDAO.IsFlagActive("NewItemAutoSku") And Me.CurrentUploadSession.IsNewItemSessionFlag And
                                    isIdentifierTypeAttribute Then

                                ' remove the sku type from the list
                                theValueListDataByKey.RemoveByKey("S")

                            End If

                            ' hold on to them to later assign to the attribute's column
                            Me.ValueListDataByKeyCollection.Add(theUploadTypeAttribute.UploadAttribute.Key, theValueListDataByKey)
                            Me.ValueListDataByValueCollection.Add(theUploadTypeAttribute.UploadAttribute.Key, theValueListDataByValue)
                        End If
                    End If
                Next
            Next

            Dim theMappedValue As Object

            If inForImport Then
                ' now let's loop through the values of the uploadRow and
                ' map the names to ids for all attributes with value lists
                For Each theUploadRow As UploadRow In Me.CurrentUploadSession.UploadRowCollection

                    For Each theUploadValue As UploadValue In theUploadRow.UploadValueCollection

                        If Not IsNothing(theUploadValue.Value) AndAlso Not theUploadValue.IsHierarchyValue() AndAlso
                                theUploadValue.ControlType.ToLower().Equals("valuelist") Then
                            ' if the control type is a valuelist then
                            ' export the text value instead of the key value that is stored
                            ' in the UploadValue.Value property

                            ' we skip over any hierarchy attributes as those require special treatment
                            ' below

                            theMappedValue = theUploadValue.Value.Trim()

                            Dim theValueListData As BusinessObjectCollection =
                                CType(Me.ValueListDataByValueCollection.Item(theUploadValue.Key), BusinessObjectCollection)

                            If Not IsNothing(theValueListData) Then

                                Dim theKeyedListItem As KeyedListItem = CType(theValueListData.ItemByKey(theUploadValue.Value.Trim().ToLower()), KeyedListItem)
                                If Not IsNothing(theKeyedListItem) Then
                                    theMappedValue = theKeyedListItem.Key
                                End If
                            End If

                            theUploadValue.Value = CStr(theMappedValue)
                        End If
                    Next

                    ' ****** Enhancement TFS # 9836 - Match item on VendorID and VIN Number  AZ - Up the Irons! ******

                    If theUploadRow.IsItemKeyNull = True AndAlso
                        Me.CurrentUploadSession.UploadSessionUploadTypeCollection.Count = 1 AndAlso
                        Me.CurrentUploadSession.UploadSessionUploadType.UploadTypeCode = EIM_Constants.COST_UPLOAD_CODE AndAlso
                        Me.CurrentUploadSession.UploadSessionUploadType.UploadTypeTemplateID = EIM_Constants.MATCHVENDORVIN_TEMPLATE Then

                        ' ******* Get ItemIdentifier by VendorID and VIN number *****

                        FindItemsByVendorVIN(theUploadRow)

                    End If

                    ' ***********************************************************************

                    ' now convert any hierarchy name values to ids
                    EIMUtilityDAO.Instance.LookUpHierarchyIds(theUploadRow)
                Next
            End If

        End Sub

        Private Sub FindItemsByVendorVIN(ByRef upRow As UploadRow)

            Dim vendorID As Integer = CInt(upRow.FindUploadValueByAttributeKey(EIM_Constants.COST_VENDOR_ATTR_KEY).Value)

            Dim item_id As String = CStr(upRow.FindUploadValueByAttributeKey(EIM_Constants.COST_ITEM_ID_ATTR_KEY).Value)

            Dim results As New ArrayList

            results = UploadRowDAO.Instance.GetItemByVendorVIN(item_id, vendorID)


            Select Case results.Count

                Case Is > 2
                    upRow.HasValidatedIdentifier = False
                    upRow.IsItemKeyNull = True
                    upRow.IsVendorVINDuplicate = True
                Case Is = 0
                    upRow.HasValidatedIdentifier = False
                    upRow.IsItemKeyNull = True
                    upRow.IsVendorVINDuplicate = False
                Case Else
                    upRow.ItemKey = CInt(results.Item(0))
                    upRow.Identifier = CStr(results.Item(1))
                    upRow.UploadValue.Value = upRow.Identifier
                    upRow.IsItemKeyNull = False
                    upRow.HasValidatedIdentifier = True
                    upRow.IsVendorVINDuplicate = False
            End Select

        End Sub

        Private Sub BuildValidationLevelColumn(ByRef grid As UltraGrid)

            ' create and configure the visible isValidated column
            Dim theUltraGridColumn As UltraGridColumn = grid.DisplayLayout.Bands(0).Columns.
                Add(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME, "Valid")

            theUltraGridColumn.CellActivation = Activation.NoEdit
            theUltraGridColumn.CellClickAction = CellClickAction.CellSelect
            theUltraGridColumn.AutoSizeMode = ColumnAutoSizeMode.AllRowsInBand
            theUltraGridColumn.Header.VisiblePosition = 0
            theUltraGridColumn.Hidden = False
            theUltraGridColumn.Header.Appearance.ImageHAlign = HAlign.Center
            theUltraGridColumn.Header.Appearance.TextHAlign = HAlign.Center
            theUltraGridColumn.Group = grid.DisplayLayout.Bands(0).Groups(" ")

            ' create a ValueList with two items
            ' one for valid and one for invalidated
            Dim theValueList As ValueList = grid.DisplayLayout.ValueLists.Add(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME)
            theValueList.DisplayStyle = ValueListDisplayStyle.Picture
            theValueList.Appearance.ImageHAlign = HAlign.Center

            ' add the validated item
            Dim vli As ValueListItem = theValueList.ValueListItems.Add(EIM_Constants.ValidationLevels.Valid, "Valid")

            ' give it no image
            vli.Appearance.Image = Nothing

            ' add the invalidated item
            vli = theValueList.ValueListItems.Add(EIM_Constants.ValidationLevels.Warning, "Warning")

            ' give it an image
            vli.Appearance.Image = EIMForm.ImageListValidationLevel.Images(0)

            ' add the invalidated item
            vli = theValueList.ValueListItems.Add(EIM_Constants.ValidationLevels.Invalid, "Error")

            ' give it an image
            vli.Appearance.Image = EIMForm.ImageListValidationLevel.Images(1)

            ' attach this ValueList to the "Is Validated" column in the grid
            grid.DisplayLayout.Bands(0).Columns(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME).ValueList = theValueList

        End Sub


        Private Sub BuildNonAttributeGridColumns(ByRef grid As UltraGrid)

            ' set the settings on the grid to support error display
            grid.DisplayLayout.Override.SupportDataErrorInfo = UltraWinGrid.SupportDataErrorInfo.RowsAndCells
            grid.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True
            grid.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.Empty
            grid.DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.Black

            ' clear the value lists
            grid.DisplayLayout.ValueLists.Clear()

            Dim theUltraGridColumn As UltraGridColumn

            ' create and configure the hidden UploadRow_ID column
            theUltraGridColumn = grid.DisplayLayout.Bands(0).Columns.
                Add(EIM_Constants.UPLOADROW_ID_COLUMN_NAME, EIM_Constants.UPLOADROW_ID_COLUMN_NAME)
            theUltraGridColumn.CellActivation = Activation.NoEdit
            theUltraGridColumn.AutoSizeMode = ColumnAutoSizeMode.AllRowsInBand
            theUltraGridColumn.Header.VisiblePosition = -1

#If DEBUG Then
            theUltraGridColumn.Hidden = True
#Else
            theUltraGridColumn.Hidden = True
#End If
            theUltraGridColumn.Width = 0

            ' create and configure the hidden item key column
            theUltraGridColumn = grid.DisplayLayout.Bands(0).Columns.
                Add(EIM_Constants.ITEM_KEY_ATTR_KEY, EIM_Constants.ITEM_KEY_ATTRIBUTE_NAME)
            theUltraGridColumn.CellActivation = Activation.NoEdit
            theUltraGridColumn.AutoSizeMode = ColumnAutoSizeMode.AllRowsInBand
            theUltraGridColumn.Header.VisiblePosition = -1
            theUltraGridColumn.Hidden = True
            theUltraGridColumn.Width = 0

            ' create and configure the hidden is identical column
            theUltraGridColumn = grid.DisplayLayout.Bands(0).Columns.
                Add(EIM_Constants.IS_IDENTICAL_ITEM_ROW_COLUMN_NAME, EIM_Constants.IS_IDENTICAL_ITEM_ROW_COLUMN_NAME)
            theUltraGridColumn.CellActivation = Activation.NoEdit
            theUltraGridColumn.AutoSizeMode = ColumnAutoSizeMode.AllRowsInBand
            theUltraGridColumn.Header.VisiblePosition = -1

#If DEBUG Then
            theUltraGridColumn.Hidden = True
#Else
            theUltraGridColumn.Hidden = True
#End If
            theUltraGridColumn.Width = 0

            ' create and configure the hidden is hidden column
            theUltraGridColumn = grid.DisplayLayout.Bands(0).Columns.
                Add(EIM_Constants.IS_HIDDEN_COLUMN_NAME, EIM_Constants.IS_HIDDEN_COLUMN_NAME)
            theUltraGridColumn.CellActivation = Activation.NoEdit
            theUltraGridColumn.AutoSizeMode = ColumnAutoSizeMode.AllRowsInBand
            theUltraGridColumn.Header.VisiblePosition = -1

#If DEBUG Then
            theUltraGridColumn.Hidden = True
#Else
            theUltraGridColumn.Hidden = True
#End If
            theUltraGridColumn.Width = 0

            ' create and configure the visible "Is Validated" column
            BuildValidationLevelColumn(grid)

        End Sub

        Private Sub BuildGridColumn(ByRef grid As UltraGrid, ByVal theUploadTypeAttribute As UploadTypeAttribute)

            Dim theUltraGridColumn As UltraGridColumn

            Dim theUploadAttribute As UploadAttribute
            Dim columnKey As String
            Dim columnName As String
            Dim columnPosition As Integer
            Dim columnDataType As String
            Dim columnSize As Integer
            Dim isHidden As Boolean

            ' get the column's configuration
            theUploadAttribute = Me.CurrentUploadSession.FindUploadAttributeByID(theUploadTypeAttribute.UploadAttributeID)
            If theUploadAttribute IsNot Nothing Then
                If theUploadAttribute.IsAllowedForRegion() Then

                    columnKey = theUploadAttribute.Key
                    columnName = theUploadAttribute.Name

                    If theUploadAttribute.IsRequiredValue Then
                        columnName = columnName + "*"
                    End If

                    ' add one to the position because the non-attribute IsValidated column is always first
                    columnPosition = theUploadTypeAttribute.GridPosition + 1
                    columnDataType = theUploadAttribute.DbDataType
                    columnSize = theUploadAttribute.Size
                    isHidden = theUploadTypeAttribute.IsHidden

                    ' create and configure the column for the attribute
                    Try
                        theUltraGridColumn = grid.DisplayLayout.Bands(0).Columns.Add(columnKey, columnName)
                    Catch ex As Exception
                        Throw New Exception("The attribute " + columnName + " is configured to be in the " _
                                + theUploadTypeAttribute.UploadTypeCode + " more than once. This is not allowed. " +
                                "EIM will not be operable until this situation is resolved.")
                    End Try

                    theUltraGridColumn.Header.Appearance.TextHAlign = HAlign.Center
                    theUltraGridColumn.DataType = UploadAttribute.MapFromDbDataType(columnDataType)
                    theUltraGridColumn.NullText = ""
                    theUltraGridColumn.Nullable = Nullable.EmptyString

                    theUltraGridColumn.Header.VisiblePosition = columnPosition
                    theUltraGridColumn.Hidden = isHidden

                    ' set the header group
                    If IsNothing(theUploadTypeAttribute.GroupName) Or String.IsNullOrEmpty(theUploadTypeAttribute.GroupName) Then
                        theUltraGridColumn.Group = grid.DisplayLayout.Bands(0).Groups(" ")
                    Else
                        theUltraGridColumn.Group = grid.DisplayLayout.Bands(0).Groups(theUploadTypeAttribute.GroupName)
                    End If

                    'Set grid culture
                    Dim gridculture As CultureInfo = CultureInfo.CreateSpecificCulture(gsUG_Culture)

                    'set the configured format or a default if none provided
                    If Not IsNothing(theUploadAttribute.DisplayFormatString) And
                            Not String.IsNullOrEmpty(theUploadAttribute.DisplayFormatString) Then
                        theUltraGridColumn.Format = theUploadAttribute.DisplayFormatString
                    Else
                        ' set int format
                        If columnDataType.Equals("int", StringComparison.CurrentCultureIgnoreCase) Then
                            theUltraGridColumn.Format = "###,###,###,###,###"
                        End If

                        ' set decimal format
                        If columnDataType.Equals("decimal", StringComparison.CurrentCultureIgnoreCase) Then
                            theUltraGridColumn.Format = "###,###,###,###,###.##"
                        End If

                        ' set money format
                        If columnDataType.Equals("money", StringComparison.CurrentCultureIgnoreCase) Then
                            theUltraGridColumn.Format = "###,###,###,###,##0.0000"
                        End If

                        ' set datetime format
                        If columnDataType.Equals("smalldatetime", StringComparison.CurrentCultureIgnoreCase) Or
                                columnDataType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase) Then
                            '20100216 - Dave Stacey - remove hard-coded date mask string w/configured one
                            theUltraGridColumn.Format = CStr(gsUG_DateMask)
                        End If
                    End If

                    ' right justify numbers and dates
                    If theUploadAttribute.IsNumeric Or
                            theUploadAttribute.DbDataType.ToLower().Equals("smalldatetime") Or
                            theUploadAttribute.DbDataType.ToLower().Equals("datetime") Then
                        theUltraGridColumn.CellAppearance.TextHAlign = HAlign.Right
                    End If

                    ' make all long text fields read-only
                    ' the LongTextForm will be used for editing
                    If theUploadTypeAttribute.UploadAttribute.Size > EIM_Constants.LONG_TEXT_SIZE Or
                            Me.CurrentUploadSession.IsReadOnly(theUploadTypeAttribute) Then

                        theUltraGridColumn.CellActivation = Activation.NoEdit
                        theUltraGridColumn.CellAppearance.BackColor = EIM_Constants.GRID_CELL_BACKGROUND_COLOR_DISABLED
                        theUltraGridColumn.CellAppearance.AlphaLevel = 0
                    Else
                        theUltraGridColumn.CellActivation = Activation.AllowEdit
                        theUltraGridColumn.CellClickAction = CellClickAction.EditAndSelectText
                        theUltraGridColumn.MaxLength = columnSize
                    End If

                    ' set up the value list if required for the UploadAttribute
                    If theUploadAttribute.IsValueListControl Then

                        grid.DisplayLayout.ValueLists.Add(columnKey)
                        grid.DisplayLayout.ValueLists(columnKey).DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText
                        theUltraGridColumn.Style = ColumnStyle.DropDown
                        theUltraGridColumn.NullText = ""
                        theUltraGridColumn.CellAppearance.TextHAlign = HAlign.Left
                        theUltraGridColumn.ValueList = grid.DisplayLayout.ValueLists.Item(columnKey)

                        theUltraGridColumn.DataType = GetType(System.String)

                        Dim theValueListData As BusinessObjectCollection = CType(Me.ValueListDataByKeyCollection.Item(columnKey), BusinessObjectCollection)

                        If Not IsNothing(theValueListData) Then
                            For Each theKeyedListItem As KeyedListItem In theValueListData
                                grid.DisplayLayout.ValueLists(columnKey).ValueListItems.Add(theKeyedListItem.Key, CStr(theKeyedListItem.Value))
                            Next
                        End If
                    ElseIf theUploadAttribute.ControlType.ToLower().Equals("currency") Or
                            theUploadAttribute.ControlType.ToLower().Equals("decimal") Then
                        ' assign Double editor
                        theUltraGridColumn.Style = ColumnStyle.Edit

                    ElseIf theUploadAttribute.ControlType.ToLower().Equals("integer") Then
                        ' assign Integer editor
                        theUltraGridColumn.Style = ColumnStyle.Integer

                    ElseIf theUploadAttribute.ControlType.ToLower().Equals("smalldatetime") Or
                            theUploadAttribute.ControlType.ToLower().Equals("datetime") Then
                        '20100216 - Dave Stacey - apply configured grid culture to column style
                        ' set datetime format
                        theUltraGridColumn.FormatInfo = gridculture
                        theUltraGridColumn.Format = theUploadAttribute.DisplayFormatString

                        theUltraGridColumn.Style = ColumnStyle.Date

                        ' unfortunately the mask format chars for month are not the same for the display format
                        If Not IsNothing(theUploadAttribute.DisplayFormatString) Then
                            theUltraGridColumn.MaskInput = theUploadAttribute.DisplayFormatString.Replace("MM", "mm")
                        End If

                    ElseIf theUploadAttribute.ControlType.ToLower().Equals("checkbox") Then
                        ' assign checkbox editor
                        theUltraGridColumn.Style = ColumnStyle.CheckBox
                    Else
                        ' assign default editor
                        theUltraGridColumn.Style = ColumnStyle.Edit
                    End If

                    ' set the background color of the custom editor columns
                    If theUploadTypeAttribute.GroupName.Equals(EIM_Constants.GROUP_HIERARCHY_DATA_KEY) OrElse
                            theUploadAttribute.Key.Equals(EIM_Constants.ITEM_CHAINS_ATTR_KEY) OrElse
                            theUploadAttribute.Key.Equals(EIM_Constants.COST_DISCOUNT_ATTR_KEY) OrElse
                            theUploadAttribute.Size > EIM_Constants.LONG_TEXT_SIZE Then

                        theUltraGridColumn.CellAppearance.BackColor = EIM_Constants.GRID_CELL_BACKGROUND_COLOR_CUSTOMPOPUP
                        theUltraGridColumn.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
                        theUltraGridColumn.CellAppearance.ForeColor = Color.Blue
                        theUltraGridColumn.CellAppearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True
                        theUltraGridColumn.CellAppearance.Cursor = Cursors.Hand

                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Add column groups to the grid for a specific upload type.
        ''' </summary>
        ''' <param name="inGrid"></param>
        ''' <param name="inUploadTypeCode"></param>
        ''' <remarks></remarks>
        Private Sub BuildGridColumnGroups(ByRef inGrid As UltraGrid, ByVal inUploadTypeCode As String,
                ByVal inForImport As Boolean, Optional ByVal isSlimFunctionalityEnabled As Boolean = True)

            ' create default group
            inGrid.DisplayLayout.Bands(0).Groups.Add(" ")

            ' get the UploadAttributeTypes and their UploadAttributes for the provided uploadTypeCode
            Dim theUploadTypeAttributes As BusinessObjectCollection

            If inForImport Then
                ' build the groups based on what's been imported
                theUploadTypeAttributes = Me.CurrentUploadSession.FindUploadTypeAttributesInFirstUploadRowByUploadTypeCode(inUploadTypeCode)
            Else
                ' build the bands based on what has been loaded from either a session
                ' or an item load
                theUploadTypeAttributes = UploadTypeAttributeDAO.Instance.GetUploadTypeAttributesByUploadTypeCode(inUploadTypeCode)

            End If

            theUploadTypeAttributes.SortByPropertyValue("GridPosition")

            ' create, configure, and add the configured band to the grid
            For Each theUploadTypeAttribute As UploadTypeAttribute In theUploadTypeAttributes

                If (Not isSlimFunctionalityEnabled AndAlso (theUploadTypeAttribute.GroupName = slimDealDataConst _
                                                            OrElse theUploadTypeAttribute.UploadAttribute.ColumnNameOrKey = isdealChangeConst)) Then
                    Continue For
                End If

                If theUploadTypeAttribute.UploadAttribute.IsAllowedForRegion() AndAlso
                        Me.CurrentUploadSession.IsAttributeInTemplateForUploadType(inUploadTypeCode, theUploadTypeAttribute.UploadAttribute) Then
                    If Not inGrid.DisplayLayout.Bands(0).Groups.Exists(theUploadTypeAttribute.GroupName) Then
                        inGrid.DisplayLayout.Bands(0).Groups.Add(theUploadTypeAttribute.GroupName)
                        ' inGrid.DisplayLayout.Bands(0).Groups(theUploadTypeAttribute.GroupName).CellAppearance.BackColor = Color.PaleGoldenrod
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Add columns to the grid for a specific upload type.
        ''' </summary>
        ''' <param name="inGrid"></param>
        ''' <param name="inUploadTypeCode"></param>
        ''' <remarks></remarks>
        Private Sub BuildGridColumns(ByRef inGrid As UltraGrid, ByVal inUploadTypeCode As String,
                ByVal inForImport As Boolean, Optional ByVal isSlimFunctionalityEnabled As Boolean = True)

            ' clear out any existing grid columns
            inGrid.DataSource = Nothing
            inGrid.DataBind()
            inGrid.DisplayLayout.Bands(0).Columns.ClearUnbound()
            inGrid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.None
            Me.AttributeNameToColumnIndexMap.Clear()

            BuildGridColumnGroups(inGrid, inUploadTypeCode, inForImport, isSlimFunctionalityEnabled)
            BuildNonAttributeGridColumns(inGrid)

            ' get the UploadAttributeTypes and their UploadAttributes for the provided uploadTypeCode
            Dim theUploadTypeAttributes As BusinessObjectCollection

            If inForImport Then
                ' build the columns based on what's been imported
                theUploadTypeAttributes = Me.CurrentUploadSession.FindUploadTypeAttributesInFirstUploadRowByUploadTypeCode(inUploadTypeCode)

            Else
                ' build the columns based on what has been loaded from either a session
                ' or an item load
                theUploadTypeAttributes = UploadTypeAttributeDAO.Instance.GetUploadTypeAttributesByUploadTypeCode(inUploadTypeCode)
            End If

            ' got to do this to reliably order the columns in the grid
            ' what the #$@%?
            theUploadTypeAttributes.SortByPropertyValue("GridPosition")

            ' create, configure, and add the configured columns to the grid
            For Each theUploadTypeAttribute As UploadTypeAttribute In theUploadTypeAttributes

                If (Not isSlimFunctionalityEnabled AndAlso (theUploadTypeAttribute.GroupName = slimDealDataConst _
                                                            OrElse theUploadTypeAttribute.UploadAttribute.ColumnNameOrKey = isdealChangeConst)) Then
                    Continue For
                End If

                If theUploadTypeAttribute.UploadAttribute.IsAllowedForRegion() AndAlso
                        Me.CurrentUploadSession.IsAttributeInTemplateForUploadType(inUploadTypeCode, theUploadTypeAttribute.UploadAttribute) Then

                    BuildGridColumn(inGrid, theUploadTypeAttribute)

                End If

            Next

            inGrid.DisplayLayout.BandsSerializer.Add(inGrid.DisplayLayout.Bands(0))

        End Sub


        ''' <summary>
        ''' Returns the number of columns whose header labels are configured
        ''' as upload attributes in the database and displays a dialog to the user
        ''' listing any unrecognized columns.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ContainsAtLeastOneRecognizedColumn() As Boolean

            Dim validColumnCount As Integer = 0

            Dim nonconfiguredColumnNames As New ArrayList()
            Dim theUploadType As UploadType
            Dim theUploadTypeAttribute As UploadTypeAttribute
            Dim theUploadAttribute As UploadAttribute
            Dim columnNameFound As Boolean = False
            Dim columnIndex As Integer = 0
            Dim allUploadTypes As BusinessObjectCollection = UploadTypeDAO.Instance.GetAllUploadTypes()

            Dim theSpreadsheetColumnHeaderLabels As ArrayList = GetSpreadsheetColumnHeaderLabels()

            For Each theSpreadsheetColumnHeaderLabel As String In theSpreadsheetColumnHeaderLabels

                columnNameFound = False

                ' loop through all the upload types not selected by the user and their UploadAttributes to see
                ' if the column header label has been configured for upload.
                For Each theUploadType In allUploadTypes
                    For Each theUploadTypeAttribute In theUploadType.UploadTypeAttributeCollection
                        theUploadAttribute = Me.CurrentUploadSession.FindUploadAttributeByID(theUploadTypeAttribute.UploadAttributeID)

                        If theUploadAttribute.Name.ToLower().Equals(theSpreadsheetColumnHeaderLabel.ToLower()) Then

                            ' hold on to the column index for the column/attribute name
                            ' for use later when we are importing the data
                            If Not Me.AttributeNameToColumnIndexMap.ContainsKey(columnIndex) Then
                                Me.AttributeNameToColumnIndexMap.Add(columnIndex, theSpreadsheetColumnHeaderLabel)
                            End If

                            columnNameFound = True
                            ' exit the inner loop
                            Exit For
                        End If
                    Next
                    If columnNameFound Then
                        ' found the column so exit the outer loop
                        ' and stop looking
                        Exit For
                    End If
                Next

                If columnNameFound Then
                    validColumnCount = validColumnCount + 1
                Else
                    If Not nonconfiguredColumnNames.Contains(theSpreadsheetColumnHeaderLabel) Then
                        nonconfiguredColumnNames.Add(theSpreadsheetColumnHeaderLabel)
                    End If
                End If

                columnIndex = columnIndex + 1
            Next

            If nonconfiguredColumnNames.Count > 0 Then

                ' build and display the message
                Dim nonconfiguredColumnNamesMessage As String = ""

                For Each nonconfiguredColumnName As String In nonconfiguredColumnNames
                    nonconfiguredColumnNamesMessage = nonconfiguredColumnNamesMessage + nonconfiguredColumnName + ControlChars.NewLine
                Next

                MessageBox.Show("The following spreadsheet columns are not configured for any upload" + ControlChars.NewLine +
                    "type and will not be imported." +
                    ControlChars.NewLine + ControlChars.NewLine +
                    nonconfiguredColumnNamesMessage, "EIM - Unrecognized Columns",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            Return validColumnCount > 0

        End Function

        Private Function GetSpreadsheetColumnHeaderLabels() As ArrayList

            Dim theSpreadsheetColumnHeaderLabels As New ArrayList()
            Dim columnIndex As Integer
            Dim theColumnHeaderLabel As String
            For columnIndex = 0 To Me.SpreadSheetColumnCount - 1

                ' get the column header label from the second row
                ' skip over the UploadType code row
                theColumnHeaderLabel = Me.CurrentExcelWorksheet.Cells(1, columnIndex).Value

                If Not String.IsNullOrEmpty(theColumnHeaderLabel) Then
                    theSpreadsheetColumnHeaderLabels.Add(theColumnHeaderLabel)
                End If

            Next

            Return theSpreadsheetColumnHeaderLabels

        End Function

        ''' <summary>
        ''' Checks the spreadsheet the user wants to import for duplicate columns
        ''' and alerts the user if found.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckForDuplicateColumns() As Boolean

            Dim duplicateColumnNamesString As String = ""

            Dim theSpreadsheetColumnHeaderLabels As ArrayList = GetSpreadsheetColumnHeaderLabels()
            Dim theTableForDupeCheck As New Hashtable()

            Dim theSpreadsheetsUploadTypeCollection As BusinessObjectCollection =
                    GetUploadTypesFromSpreadsheet(False)

            For Each theSpreadsheetColumnHeaderLabel As String In theSpreadsheetColumnHeaderLabels

                If theTableForDupeCheck.ContainsKey(theSpreadsheetColumnHeaderLabel) Then
                    duplicateColumnNamesString = duplicateColumnNamesString + theSpreadsheetColumnHeaderLabel +
                        ControlChars.NewLine
                Else
                    theTableForDupeCheck.Add(theSpreadsheetColumnHeaderLabel, theSpreadsheetColumnHeaderLabel)
                End If
            Next

            If duplicateColumnNamesString.Length > 0 Then

                ' display the message
                MessageBox.Show("The following columns are have been found in the spreadsheet more than once:" + ControlChars.NewLine +
                    "Please remove them and try your import again." +
                    ControlChars.NewLine + ControlChars.NewLine +
                    duplicateColumnNamesString, "EIM - Spreadsheet Import",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            Return duplicateColumnNamesString.Length = 0

        End Function


        Private Function CheckForRequiredColumns() As Boolean

            Dim validColumnCount As Integer = 0

            Dim missingRequiredColumnNamesString As String = ""
            Dim requiredColumnHeaderLabelFound As Boolean = False
            Dim theIgnoreMessage As String = ""

            Dim allUploadTypeAttributes As BusinessObjectCollection = UploadTypeAttributeDAO.Instance.GetAllUploadTypeAttributes()
            Dim theSpreadsheetColumnHeaderLabels As ArrayList = GetSpreadsheetColumnHeaderLabels()

            Dim theSpreadsheetsUploadTypeCollection As BusinessObjectCollection =
                    GetUploadTypesFromSpreadsheet(False)

            For Each theUploadType As UploadType In theSpreadsheetsUploadTypeCollection

                For Each theUploadTypeAttribute As UploadTypeAttribute In allUploadTypeAttributes

                    requiredColumnHeaderLabelFound = False

                    If theUploadTypeAttribute.UploadTypeCode.Equals(theUploadType.UploadTypeCode) AndAlso
                            theUploadTypeAttribute.UploadAttribute.IsRequiredValue AndAlso
                            theUploadTypeAttribute.UploadAttribute.IsAllowedForRegion() Then

                        For Each theSpreadsheetColumnHeaderLabel As String In theSpreadsheetColumnHeaderLabels

                            If theUploadTypeAttribute.UploadAttribute.Name.ToLower().Equals(theSpreadsheetColumnHeaderLabel.ToLower()) Then
                                requiredColumnHeaderLabelFound = True
                                Exit For
                            End If
                        Next

                        If Not requiredColumnHeaderLabelFound Then
                            missingRequiredColumnNamesString = missingRequiredColumnNamesString + theUploadType.UploadTypeCode + " - " +
                                    theUploadTypeAttribute.UploadAttribute.Name + ControlChars.NewLine
                        End If

                    End If
                Next
            Next

            If missingRequiredColumnNamesString.Length > 0 Then

                ' display the message
                MessageBox.Show("The following columns are required but are not found in the spread sheet:" + ControlChars.NewLine +
                    "They will be added with empty values to the grids." +
                    ControlChars.NewLine + ControlChars.NewLine +
                    missingRequiredColumnNamesString, "EIM - Spreadsheet Import",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            Return missingRequiredColumnNamesString.Length = 0

        End Function

        ''' <summary>
        ''' Get the upload types (up to three) for the spreadsheet.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUploadTypesFromSpreadsheet(ByVal showErrorMessage As Boolean) As BusinessObjectCollection

            Dim theUploadTypeCollection As New BusinessObjectCollection()
            Dim theUploadCode As String
            Dim theErrorMessage As String = ""
            Dim theInvalidUploadTypesMessage As String = ""

            Dim rowIndex As Integer = 0
            Dim columnIndex As Integer = 1

            theUploadTypeCollection = New BusinessObjectCollection()
            Dim theUploadType As UploadType
            For columnIndex = 1 To 3
                theUploadCode = Me.CurrentExcelWorksheet.Cells(rowIndex, columnIndex).Value

                ' look for valid UploadType codes
                If Not IsNothing(theUploadCode) And Not String.IsNullOrEmpty(theUploadCode) Then
                    theUploadType = UploadTypeDAO.Instance.GetUploadTypeByPK(theUploadCode)

                    If Not IsNothing(theUploadType) Then
                        theUploadTypeCollection.Add(theUploadType.PrimaryKey, theUploadType)
                    Else
                        theInvalidUploadTypesMessage = theInvalidUploadTypesMessage + theUploadCode
                        Exit For
                    End If
                End If
            Next

            If showErrorMessage Then
                ' if no valid upload type codes were found or there were any invalid upload types then
                ' tell the user

                If theUploadTypeCollection.Count = 0 Then
                    theErrorMessage = "No valid upload types were found."
                ElseIf theInvalidUploadTypesMessage.Length > 0 Then
                    theErrorMessage = "Invalid upload types were found."
                End If

                ' if there are any errors
                If theErrorMessage.Length > 0 Then

                    theErrorMessage = theErrorMessage + ControlChars.NewLine + ControlChars.NewLine +
                        "Up to three valid Upload Types must be found in the 2nd, 3rd, and 4th cells of the first row " + ControlChars.NewLine +
                        "of an EIM importable spreadsheet." +
                        ControlChars.NewLine

                    If theInvalidUploadTypesMessage.Length > 0 Then
                        theErrorMessage = theErrorMessage +
                                ControlChars.NewLine + "The following invalid Upload Type(s) were found: " + ControlChars.NewLine + ControlChars.NewLine +
                                theInvalidUploadTypesMessage
                    End If

                    MessageBox.Show(theErrorMessage, "EIM - Spreadsheet Import", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    ' clear out any valid UploadTypes if any invalid ones were found
                    theUploadTypeCollection.Clear()
                End If
            End If

            Return theUploadTypeCollection

        End Function

        ''' <summary>
        ''' Loop through the rows and coluns of the spreadsheet and import
        ''' its cell values into this UploadSession's data structure
        ''' of UploadRows and UploadValues.
        ''' 
        ''' Note that importing uses a different class lib
        ''' than exporting. This is because the Infragistics
        ''' version IRMA uses only supports export. Version
        ''' 2007 Vol 2, however, also supports importing. 
        ''' Refactor to use Infragistics.Excel classes,
        ''' as the export does, when IRMA is upgraded to
        ''' at least Infragistics 2007 Vol 2.
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ImportSpreadsheetIntoUploadSession()
            Dim newUploadRow As UploadRow
            Dim newUploadValue As UploadValue = Nothing
            Dim theUploadAttribute As UploadAttribute
            ' set the progress properties
            ' which are used by the form to show a progress dialog
            Me.ProgressComplete = False
            Me.ProgressCounter = 0

            Dim rowIndex As Integer = 0
            Dim columnIndex As Integer = 0

            Dim theUploadTypeErrorMessage As String = ""

            ' loop through the rows and colunms of the spreadsheet
            ' skip over the UploadType code and column header rows
            For rowIndex = 2 To Me.SpreadSheetRowCount - 1
                newUploadRow = New UploadRow(Me.CurrentUploadSession)

                For columnIndex = 0 To Me.SpreadSheetColumnCount - 1
                    ' find the UploadAttribute
                    theUploadAttribute =
                        Me.CurrentUploadSession.
                            FindUploadAttributeByName(CStr(Me.AttributeNameToColumnIndexMap(columnIndex)))

                    ' now create a new UploadValue for the UploadTypeAttribute and UploadRow
                    ' but only if the spreadsheet column is configured
                    If Not IsNothing(theUploadAttribute) AndAlso theUploadAttribute.IsAllowedForRegion() Then
                        If Me.CurrentUploadSession.IsAttributeInTemplateForAttributeUploadTypes(theUploadAttribute) Then

                            newUploadValue = New UploadValue(theUploadAttribute, newUploadRow)

                            If IsNothing(Me.CurrentExcelWorksheet.Cells(rowIndex, columnIndex).Value) Then
                                newUploadValue.Value = theUploadAttribute.DefaultValue
                            Else
                                If theUploadAttribute.IsNumeric Then
                                    'strip any commas from numeric attributes
                                    ' but not from int key values for value lists since
                                    ' at this point we are importing the text value which will
                                    ' be later converted into the corresponding int key value
                                    If Not theUploadAttribute.ControlType.ToLower().Equals("valuelist") Then
                                        newUploadValue.Value = Me.CurrentExcelWorksheet.Cells(rowIndex, columnIndex).Value.ToString().Replace(",", "")
                                    Else
                                        newUploadValue.Value = Me.CurrentExcelWorksheet.Cells(rowIndex, columnIndex).Value
                                    End If

                                    newUploadValue.Value = newUploadValue.Value.Trim()
                                Else
                                    newUploadValue.Value = Me.CurrentExcelWorksheet.Cells(rowIndex, columnIndex).Value
                                    newUploadValue.Value = newUploadValue.TranslateUploadValueFromImport()
                                End If
                            End If
                        End If

                        ' get the item key if it is the identifier attribute
                        If theUploadAttribute.Key.Equals(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY) Then
                            newUploadRow.Identifier = newUploadValue.Value

                            If Not IsNothing(newUploadRow.Identifier) And Not String.IsNullOrEmpty(newUploadRow.Identifier) Then
                                newUploadRow.ItemKey = UploadRowDAO.Instance.GetItemKeyByIdentifier(newUploadRow.Identifier)
                            Else
                                newUploadRow.ItemKey = -1
                            End If

                            If newUploadRow.ItemKey = -1 Then
                                newUploadRow.IsItemKeyNull = True
                            End If


                        End If
                    End If
                Next

                ' increment the progress counter
                Me.ProgressCounter = Me.ProgressCounter + 1
            Next

        End Sub

        ''' <summary>
        ''' Loop through the rows and columns of the items DataTable and load
        ''' the data into this UploadSession's data structure
        ''' of UploadRows and UploadValues.
        ''' </summary>
        ''' <param name="theDataTable"></param>
        ''' <remarks></remarks>
        Private Sub LoadItemsIntoCurrentUploadSession(ByVal theDataTable As System.Data.DataTable)

            Dim newUploadRow As UploadRow
            Dim newUploadValue As UploadValue
            Dim theUploadAttribute As UploadAttribute
            Dim theColumnName As String

            ' make sure to finalize any row deletes by the user
            theDataTable.AcceptChanges()

            Try

                ' init the progress properties
                Me.ProgressCounter = 0
                Me.ProgressComplete = False

                ' loop through the rows and columns of the DataTable
                For Each theDataRow As DataRow In theDataTable.Rows
                    newUploadRow = New UploadRow(Me.CurrentUploadSession)
                    For Each theDataColumn As DataColumn In theDataTable.Columns

                        theColumnName = theDataColumn.ColumnName.ToLower()

                        ' if column contains the item key then set its value in the UploadRow
                        If theDataColumn.ColumnName.ToLower().Equals(EIM_Constants.ITEM_KEY_ATTR_KEY) Then
                            newUploadRow.ItemKey = Integer.Parse(theDataRow.Item(theColumnName).ToString())
                        End If

                        ' If the item data is from SLIM we need to hold onto the ItemRequest_ID
                        ' to update the ItemRequest row status after a successful upload.
                        ' The ItemRequest_ID va;ue comes in as the item key, however the item key
                        ' is overwritten with the actual item key value when the item is created
                        ' during upload.
                        If Me.CurrentUploadSession.IsFromSLIM Then
                            newUploadRow.ItemRequestID = newUploadRow.ItemKey
                        End If

                        ' same for the identifier
                        If theColumnName.ToLower().Equals(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY) Then
                            newUploadRow.Identifier = theDataRow.Item(theDataColumn.ColumnName).ToString()
                        End If

                        ' find the UploadAttribute
                        theUploadAttribute =
                            Me.CurrentUploadSession.
                                FindUploadAttributeByKeyForSessionUploadTypes(theColumnName)

                        ' now create a new UploadValue for the UploadAttribute and UploadRow
                        ' but only if the column is configured
                        If Not IsNothing(theUploadAttribute) AndAlso theUploadAttribute.IsAllowedForRegion() Then
                            If Me.CurrentUploadSession.IsAttributeInTemplateForAttributeUploadTypes(theUploadAttribute) Then
                                newUploadValue = New UploadValue(theUploadAttribute, newUploadRow)

                                If Not IsNothing(theDataRow.Item(theColumnName)) And
                                        Not theDataRow.Item(theColumnName).Equals(DBNull.Value) Then
                                    newUploadValue.Value = theDataRow.Item(theColumnName).ToString()
                                Else
                                    newUploadValue.Value = theUploadAttribute.DefaultValue
                                End If
                            End If
                        End If
                    Next

                    ' make sure we also have all the attributes not just those in the DataRow
                    Dim allUploadAttributes As BusinessObjectCollection = UploadAttributeDAO.Instance.GetAllUploadAttributes()
                    Dim theUploadValue As UploadValue = Nothing

                    ' loop through all the UploadAttributes
                    For Each theUploadAttribute In allUploadAttributes
                        If theUploadAttribute.IsAllowedForRegion() Then
                            ' only add those in the current templates for the current UploadTypes
                            If Me.CurrentUploadSession.IsAttributeInTemplateForAttributeUploadTypes(theUploadAttribute) Then
                                ' add the UploadAttribute only if it didn't load from the DataRow

                                theUploadValue = newUploadRow.FindUploadValueByAttributeKey(theUploadAttribute.Key)

                                If IsNothing(theUploadValue) Then

                                    theUploadValue = New UploadValue(theUploadAttribute, newUploadRow)

                                    ' set the default value
                                    theUploadValue.Value = theUploadAttribute.DefaultValue
                                End If

                                If Not Me.CurrentUploadSession.IsFromSLIM Then
                                    If theUploadAttribute.Key.Equals(EIM_Constants.DEAL_IS_CHANGE_ATTR_KEY) Then

                                        ' only allow vendor deal creation for SLIM sessions
                                        theUploadValue.Value = "False"
                                    End If
                                Else

                                    If theUploadAttribute.Key.Equals(EIM_Constants.PRICE_IS_CHANGE_ATTR_KEY) Then
                                        ' if the upload attribute is "Is Price Change" and the load is from SLIM
                                        ' set it to true if there is a price
                                        Dim theRegPriceAmount As String = newUploadRow.FindValueByAttributeKey(EIM_Constants.PRICE_ATTR_KEY)
                                        Dim thePromoPriceAmount As String = newUploadRow.FindValueByAttributeKey(EIM_Constants.PRICE_PROMO_ATTR_KEY)

                                        theUploadValue.Value = (Not IsNothing(theRegPriceAmount) AndAlso
                                                Not String.IsNullOrEmpty(theRegPriceAmount)) Or
                                                (Not IsNothing(thePromoPriceAmount) AndAlso
                                                Not String.IsNullOrEmpty(thePromoPriceAmount))

                                    ElseIf theUploadAttribute.Key.Equals(EIM_Constants.COST_IS_CHANGE_ATTR_KEY) Then
                                        ' if the upload attribute is "Is Cost Change" and the load is from SLIM
                                        ' set it to true if there is a cost
                                        Dim theCostAmount As String = newUploadRow.FindValueByAttributeKey(EIM_Constants.COST_ATTR_KEY)

                                        theUploadValue.Value = Not IsNothing(theCostAmount) AndAlso
                                                Not String.IsNullOrEmpty(theCostAmount)

                                    ElseIf theUploadAttribute.Key.Equals(EIM_Constants.DEAL_IS_CHANGE_ATTR_KEY) Then
                                        ' if the upload attribute is "Is Deal Change" and the load is from SLIM
                                        ' set it to true if there is a discount or an allowance
                                        Dim theDiscountAmount As String = newUploadRow.FindValueByAttributeKey(EIM_Constants.DISCOUNT_AMOUNT_ATTR_KEY)
                                        Dim theAllowanceAmount As String = newUploadRow.FindValueByAttributeKey(EIM_Constants.ALLOWANCE_AMOUNT_ATTR_KEY)

                                        theUploadValue.Value = (Not IsNothing(theDiscountAmount) AndAlso
                                                Not String.IsNullOrEmpty(theDiscountAmount)) Or
                                                (Not IsNothing(theAllowanceAmount) AndAlso
                                                Not String.IsNullOrEmpty(theAllowanceAmount))
                                    End If
                                End If
                            End If
                        End If
                    Next

                    ' increment the progress counter
                    Me.ProgressCounter = Me.ProgressCounter + 1

                Next

            Finally

                'this is critical
                Me.ProgressComplete = True

            End Try

        End Sub


        ''' <summary>
        ''' Returns True if the attribute maps to either the Item or the
        ''' ItemAttribute tables.
        ''' </summary>
        Public Function CopyToOtherRowsForItem(ByRef inUploadValue As UploadValue) As Boolean

            Dim doCopyToOtherRowsForItem As Boolean = inUploadValue.TableName.ToLower().Equals("item") Or
                inUploadValue.TableName.ToLower().Equals("itemattribute") Or
                inUploadValue.TableName.ToLower().Equals("itemscale")

            Return doCopyToOtherRowsForItem

        End Function

        ''' <summary>
        ''' Returns True if the attribute maps to either the Item or the
        ''' ItemAttribute tables.
        ''' </summary>
        Public Function CopyToOtherRowsForItemAndStore(ByRef inUploadValue As UploadValue) As Boolean

            Dim doCopyToOtherRowsForItemAndStore As Boolean = Not inUploadValue.ColumnNameOrKey.ToLower().Equals("margin") And
                Not inUploadValue.ColumnNameOrKey.ToLower().Equals("unitcost")

            Return doCopyToOtherRowsForItemAndStore

        End Function

#End Region

    End Class

#Region "Helper Classes for associating an UploadRow with its Grid and Data Rows"

    ''' <summary>
    ''' This class lets us associate an UploadRow with its correspoonding DataRows and GridRows
    ''' and, for Item Maintenance, other UploadRowHolders that are for the same item and have
    ''' identical Item Maintenance values
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadRowHolderCollection

        Private _uploadRowHolderListByIdentifier As New SortableHashlist()
        Private _uploadRowHolderListByItemKeyAndStoreNo As New SortableHashlist()
        Private _uploadRowHoldersByUploadRowId As New SortableHashlist()

        Public ReadOnly Property UploadRowHolderList() As SortableHashlist
            Get
                Return _uploadRowHoldersByUploadRowId
            End Get
        End Property

        Public Function Add(ByRef inUploadRow As UploadRow) As UploadRowHolder

            Dim theUploadRowHolder As UploadRowHolder = GetUploadRowHolderForUploadRowId(inUploadRow.UploadRowID)

            If IsNothing(theUploadRowHolder) Then
                theUploadRowHolder = New UploadRowHolder(inUploadRow)

                AddByItemKey(theUploadRowHolder)
                ' don't think this is needed
                ' will there ever be an item supplied to a store by two or mor vendors?
                ' AddByItemKeyAndStoreNo(theUploadRowHolder)
                AddByUploadRowId(theUploadRowHolder)
            End If

            Return theUploadRowHolder

        End Function

        Public Function GetUploadRowHolderForUploadRowId(ByVal inUploadRowId As Integer) As UploadRowHolder

            ' look for an existing collection of UploadRowHolders for the provided id
            Dim theUploadRowHolder As UploadRowHolder =
                    CType(Me._uploadRowHoldersByUploadRowId.ItemByKey(inUploadRowId), UploadRowHolder)

            Return theUploadRowHolder

        End Function

        Public Sub RemoveUploadRowHolderForUploadRowId(ByVal inUploadRowId As Integer)

            Me._uploadRowHoldersByUploadRowId.RemoveByKey(inUploadRowId)

        End Sub

        Public Function GetUploadRowHolderListForIdentifier(ByVal inIdentifier As String) As ArrayList

            ' look for an existing collection of UploadRowHolders for the provided key
            Dim theUploadRowHolderForIdentifierList As New ArrayList

            inIdentifier = Trim(inIdentifier)

            If Not IsNothing(inIdentifier) AndAlso Not String.IsNullOrEmpty(inIdentifier) Then

                theUploadRowHolderForIdentifierList = CType(Me._uploadRowHolderListByIdentifier.ItemByKey(inIdentifier), ArrayList)

                If IsNothing(theUploadRowHolderForIdentifierList) Then
                    theUploadRowHolderForIdentifierList = New ArrayList()
                End If

            End If

            Return theUploadRowHolderForIdentifierList

        End Function

        ''' <summary>
        ''' Add the UploadRowHolder to the list of lists of UploadRowHolder by item key.
        ''' </summary>
        ''' <param name="inUploadRowHolder"></param>
        ''' <remarks></remarks>
        Private Sub AddByItemKey(ByRef inUploadRowHolder As UploadRowHolder)

            ' look for an existing collection of UploadRowHolders for the provided key
            Dim theUploadRowHolderForIdentifierList As ArrayList =
                    CType(Me._uploadRowHolderListByIdentifier.ItemByKey(Trim(inUploadRowHolder.UploadRow.Identifier)), ArrayList)

            ' create a new one if it cannot be found
            If IsNothing(theUploadRowHolderForIdentifierList) Then
                theUploadRowHolderForIdentifierList = New ArrayList()
                ' add it to the list of lists of UploadRowHolders for the item key
                Me._uploadRowHolderListByIdentifier.Add(Trim(inUploadRowHolder.UploadRow.Identifier), theUploadRowHolderForIdentifierList)
            End If

            ' add the UploadRowHolder to it
            theUploadRowHolderForIdentifierList.Add(inUploadRowHolder)

        End Sub

        ''' <summary>
        ''' Add the UploadRowHolder by UploadRowID.
        ''' </summary>
        ''' <param name="inUploadRowHolder"></param>
        ''' <remarks></remarks>
        Private Sub AddByUploadRowId(ByRef inUploadRowHolder As UploadRowHolder)

            If Not Me._uploadRowHoldersByUploadRowId.ContainsKey(inUploadRowHolder.UploadRow.UploadRowID) Then
                Me._uploadRowHoldersByUploadRowId.Add(inUploadRowHolder.UploadRow.UploadRowID, inUploadRowHolder)
            End If
        End Sub

        ''' <summary>
        ''' Update the UploadRowIds when they change by either the saving of new UploadRows
        ''' or if they were made new by setting their IDs to temporary id values.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateForNewUploadRowIDs()

            For Each theUploadRowHolder As UploadRowHolder In Me._uploadRowHoldersByUploadRowId

                Me._uploadRowHoldersByUploadRowId.AddWithNewKey(theUploadRowHolder.UploadRow.UploadRowID, theUploadRowHolder)
            Next

        End Sub

        Public Function HideGridRowInItemMaintenance(ByRef inUploadRowHolder As UploadRowHolder,
                ByRef inOutAreAllEqual As Boolean) As Boolean

            Dim theUploadRowHoldersForSameItemList As ArrayList =
                Me.GetUploadRowHolderListForIdentifier(inUploadRowHolder.UploadRow.ItemKey)

            Dim hasLowestValidationLevel As Boolean = True
            inOutAreAllEqual = True

            For Each theOtherUploadRowHolder As UploadRowHolder In theUploadRowHoldersForSameItemList

                If inUploadRowHolder.UploadRow.UploadRowID <> theOtherUploadRowHolder.UploadRow.UploadRowID Then

                    If theOtherUploadRowHolder.UploadRow.ValidationLevel < inUploadRowHolder.UploadRow.ValidationLevel Then
                        hasLowestValidationLevel = False
                        inOutAreAllEqual = False
                    ElseIf theOtherUploadRowHolder.UploadRow.ValidationLevel = inUploadRowHolder.UploadRow.ValidationLevel Then
                        ' if the row shares the status of having the lowest validation
                        ' level with another row
                        If hasLowestValidationLevel Then
                            hasLowestValidationLevel = hasLowestValidationLevel And
                                inUploadRowHolder.UploadRow.UploadRowID < theOtherUploadRowHolder.UploadRow.UploadRowID
                        End If
                    ElseIf theOtherUploadRowHolder.UploadRow.ValidationLevel > inUploadRowHolder.UploadRow.ValidationLevel Then
                        inOutAreAllEqual = False
                    End If
                End If
            Next

            Return hasLowestValidationLevel

        End Function

        Public Function HideGridRowInPriceUpload(ByRef inUploadRowHolder As UploadRowHolder,
                ByRef inOutAreAllEqual As Boolean) As Boolean

            Dim theUploadRowHoldersForSameItemList As ArrayList =
                Me.GetUploadRowHolderListForIdentifier(inUploadRowHolder.UploadRow.ItemKey)

            Dim hasLowestValidationLevel As Boolean = True
            inOutAreAllEqual = True

            For Each theOtherUploadRowHolder As UploadRowHolder In theUploadRowHoldersForSameItemList

                If inUploadRowHolder.UploadRow.UploadRowID <> theOtherUploadRowHolder.UploadRow.UploadRowID Then

                    If theOtherUploadRowHolder.UploadRow.ValidationLevel < inUploadRowHolder.UploadRow.ValidationLevel Then
                        hasLowestValidationLevel = False
                        inOutAreAllEqual = False
                    ElseIf theOtherUploadRowHolder.UploadRow.ValidationLevel = inUploadRowHolder.UploadRow.ValidationLevel Then
                        ' if the row shares the status of having the lowest validation
                        ' level with another row
                        If hasLowestValidationLevel Then
                            hasLowestValidationLevel = hasLowestValidationLevel And
                                inUploadRowHolder.UploadRow.UploadRowID < theOtherUploadRowHolder.UploadRow.UploadRowID
                        End If
                    ElseIf theOtherUploadRowHolder.UploadRow.ValidationLevel > inUploadRowHolder.UploadRow.ValidationLevel Then
                        inOutAreAllEqual = False
                    End If
                End If
            Next

            Return hasLowestValidationLevel

        End Function

    End Class

    ''' <summary>
    ''' This class lets us associate an UploadRow with its correspoonding DataRows and GridRows
    ''' and, for Item Maintenance, other UploadRowHolders that are for the same item and have
    ''' identical Item Maintenance values
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadRowHolder

        Private _uploadRow As UploadRow
        Private _gridAndDataRowList As SortableHashlist
        Private _validationErrors As New Hashtable()
        Private _validationWarnings As New Hashtable()

        Public Property GridAndDataRowList() As SortableHashlist
            Get
                Return _gridAndDataRowList
            End Get
            Set(ByVal value As SortableHashlist)
                _gridAndDataRowList = value
            End Set
        End Property

        Public Property UploadRow() As UploadRow
            Get
                Return _uploadRow
            End Get
            Set(ByVal value As UploadRow)
                _uploadRow = value
            End Set
        End Property

        Public ReadOnly Property ValidationWarnings() As Hashtable
            Get
                Return _validationWarnings
            End Get
        End Property

        Public ReadOnly Property ValidationErrors() As Hashtable
            Get
                Return _validationErrors
            End Get
        End Property

        Public ReadOnly Property ValidationLevel() As EIM_Constants.ValidationLevels
            Get
                Dim theValidationLevel As EIM_Constants.ValidationLevels = EIM_Constants.ValidationLevels.Valid

                If Me.ValidationErrors.Count > 0 Then
                    theValidationLevel = EIM_Constants.ValidationLevels.Invalid
                ElseIf Me.ValidationWarnings.Count > 0 Then
                    theValidationLevel = EIM_Constants.ValidationLevels.Warning
                End If

                Return theValidationLevel
            End Get
        End Property

        Public Sub New(ByRef inUploadRow As UploadRow)
            Me.UploadRow = inUploadRow
            Me.GridAndDataRowList = New SortableHashlist()
        End Sub

        Public Sub AddValidationKey(ByVal inKey As String, ByVal inDescription As String, ByVal inValidationLevel As EIM_Constants.ValidationLevels)

            If inValidationLevel = EIM_Constants.ValidationLevels.Warning Then
                Me.ValidationWarnings.Add(inKey, inDescription)
            ElseIf inValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                Me.ValidationErrors.Add(inKey, inDescription)
            End If

        End Sub

        Public Sub ClearValidationKey(ByVal inName As String)

            Me.ValidationWarnings.Remove(inName)
            Me.ValidationErrors.Remove(inName)

        End Sub

        Public Function ContainsValidationKey(ByVal inName As String) As Boolean

            Return Me.ValidationWarnings.ContainsKey(inName) Or Me.ValidationErrors.ContainsKey(inName)

        End Function

        Public Sub ClearAllValidationLevels()

            Me.ValidationWarnings.Clear()
            Me.ValidationErrors.Clear()

        End Sub

        Public Function GetGridAndDataRowByUploadType(ByVal inUploadTypeCode As String) As GridAndDataRowHolder

            Dim theGridAndDataRowHolder As GridAndDataRowHolder =
                    CType(Me.GridAndDataRowList.ItemByKey(inUploadTypeCode), GridAndDataRowHolder)

            Return theGridAndDataRowHolder

        End Function

    End Class

    Public Class GridAndDataRowHolder

        Private _uploadRowId As Integer
        Private _uploadTypeCode As String
        Private _gridRow As UltraGridRow
        Private _DataRow As DataRow

        Public Property UploadRowId() As Integer
            Get
                Return _uploadRowId
            End Get
            Set(ByVal value As Integer)
                _uploadRowId = value
            End Set
        End Property

        Public Property UploadTypeCode() As String
            Get
                Return _uploadTypeCode
            End Get
            Set(ByVal value As String)
                _uploadTypeCode = value
            End Set
        End Property

        Public Property GridRow() As UltraGridRow
            Get
                Return _gridRow
            End Get
            Set(ByVal value As UltraGridRow)
                _gridRow = value
            End Set
        End Property

        Public Property DataRow() As DataRow
            Get
                Return _DataRow
            End Get
            Set(ByVal value As DataRow)
                _DataRow = value
            End Set
        End Property

        Public Sub New(ByVal inUploadRowId As Integer,
                ByVal inUploadTypeCode As String,
                ByRef inGridRow As UltraGridRow, ByRef inDataRow As DataRow)

            Me.UploadRowId = inUploadRowId
            Me.UploadTypeCode = inUploadTypeCode
            Me.GridRow = inGridRow
            Me.DataRow = inDataRow
        End Sub

    End Class

#End Region

End Namespace
