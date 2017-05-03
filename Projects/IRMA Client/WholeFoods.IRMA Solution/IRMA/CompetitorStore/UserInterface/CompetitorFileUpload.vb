Imports System.Data.Common
Imports System.Text
Imports ExcelInterop = Microsoft.Office.Interop.Excel
Imports WholeFoods.IRMA.CompetitorStore.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    Public Class CompetitorFileUpload

#Region "Constants"

        Private Const MAX_ERROR_COUNT As Integer = 10
        Private Const ROWS_PER_BATCH As Integer = 50

#End Region

#Region "Member Variables"

        Private _uploadStart As DateTime
        Private _matchingStart As DateTime
        Private _previewData As CompetitorStoreDataSet
        Private _currentSession As CompetitorStoreDataSet.CompetitorImportSessionRow
        Private WithEvents _progressDialog As ProgressDialog
        Private WithEvents _competitorImportInfoDataObject As CompetitorImportInfo
        Private _selectedCompetitorStore As CompetitorStoreDataSet.CompetitorStoreRow
        Private _selectedFiscalWeekDescription As String

#End Region

#Region "Properties"

        Public ReadOnly Property SelectedCompetitorStore() As CompetitorStoreDataSet.CompetitorStoreRow
            Get
                Return _selectedCompetitorStore
            End Get
        End Property

        Public ReadOnly Property SelectedFiscalWeekDescription() As String
            Get
                Return _selectedFiscalWeekDescription
            End Get
        End Property

        Private ReadOnly Property CompetitorImportInfoDataObject() As CompetitorImportInfo
            Get
                If (_competitorImportInfoDataObject Is Nothing) Then
                    _competitorImportInfoDataObject = New CompetitorImportInfo()
                End If

                Return _competitorImportInfoDataObject
            End Get
        End Property

#End Region

#Region "Helper Methods"

#Region "Excel"

        Private Function OpenWorkbook(ByRef excelApp As ExcelInterop.Application, ByRef workbook As ExcelInterop.Workbook, ByRef path As String) As Boolean
            Dim success As Boolean = True

            Try
                workbook = excelApp.Workbooks.Open(path)
            Catch ex As Exception
                success = False
            End Try

            Return success
        End Function

        ''' <summary>
        ''' Attempts to open and validate the format of the file specified in the text box. Also closes
        ''' the workbook.
        ''' </summary>
        ''' <returns>Is the file valid?</returns>
        Private Function ValidateExcelFile(ByRef workBook As ExcelInterop.Workbook) As Boolean
            Dim excelApp As New ExcelInterop.Application
            Dim success As Boolean = True

            ' Attempt to open the given path as an excel workbook
            If (OpenWorkbook(excelApp, workBook, txtFilePath.Text)) Then
                If (workBook.Sheets.Count > 0) Then
                    Dim workSheet As ExcelInterop.Worksheet = CType(workBook.Worksheets(1), Microsoft.Office.Interop.Excel.Worksheet)
                    Dim workSheetName As String = workSheet.Name

                    ' Close interop access to the workbook now so we can access it through ODBC
                    workBook.Close()
                    workBook = Nothing

                    ' Inner method will set appropriate error message, if any
                    success = LoadDataFromExcel(workSheetName)
                Else
                    workBook.Close()
                    workBook = Nothing


                    SetErrorMessage(My.Resources.CompetitorStore.NoWorksheetFound)
                    success = False
                End If
            Else
                SetErrorMessage(My.Resources.CompetitorStore.UnableToOpenWorkbook)
                success = False
            End If

            lblWait.Visible = False

            Return success
        End Function

        ''' <summary>
        ''' Determines why a row cannot be imported and appends the error message to the given string builder
        ''' </summary>
        Private Sub AddRowErrorMessage(ByVal errorMessageBuilder As StringBuilder, ByVal row As CompetitorStoreDataSet.CompetitorImportInfoRow, ByRef errorCount As Integer, ByVal i As Integer)
            ' 1 for zero-based index, 1 for column headers row
            errorMessageBuilder.AppendFormat(My.Resources.CompetitorStore.ExcelErrorRowIndex, i + 2)
            errorMessageBuilder.AppendLine()

            For Each column As DataColumn In row.GetColumnsInError()
                If errorCount >= MAX_ERROR_COUNT Then
                    Exit For
                End If

                Select Case column.ColumnName
                    Case _previewData.CompetitorImportInfo.CompetitorColumn.ColumnName, _
                        _previewData.CompetitorImportInfo.CompetitorStoreColumn.ColumnName, _
                        _previewData.CompetitorImportInfo.LocationColumn.ColumnName, _
                        _previewData.CompetitorImportInfo.UPCCodeColumn.ColumnName

                        errorMessageBuilder.AppendFormat(My.Resources.CompetitorStore.ImportMissingValue, column)
                    Case _previewData.CompetitorImportInfo.PriceColumn.ColumnName, _
                        _previewData.CompetitorImportInfo.DateCheckedColumn.ColumnName

                        errorMessageBuilder.AppendFormat(My.Resources.CompetitorStore.ImportMissingOrMalformattedValue, column)
                    Case _previewData.CompetitorImportInfo.PriceMultipleColumn.ColumnName, _
                        _previewData.CompetitorImportInfo.SaleColumn.ColumnName, _
                        _previewData.CompetitorImportInfo.SaleMultipleColumn.ColumnName, _
                        _previewData.CompetitorImportInfo.SizeColumn.ColumnName

                        errorMessageBuilder.AppendFormat(My.Resources.CompetitorStore.ImportMalformattedValue, column)
                End Select

                errorMessageBuilder.AppendLine()
                errorCount += 1
            Next
        End Sub

        Private Function LoadDataFromExcel(ByVal worksheetName As String) As Boolean
            Dim excelConnection As String

            ' Connect to the Excel file as an OLEDB data source
            ' The worksheet name is passed in to cover the case that the sheet has been renamed from the default "Sheet1"
            excelConnection = String.Format("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=""{0}"";Extended Properties=""Excel 8.0;HDR=Yes"";", _
                txtFilePath.Text)

            Try
                ' You must use the $ after the object you reference in the spreadsheet
                Using excelAdapter As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(String.Format("SELECT * FROM [{0}$]", worksheetName), excelConnection)
                    Dim tableMapping As DataTableMapping = excelAdapter.TableMappings.Add("Table", _previewData.CompetitorImportInfo.TableName)

                    With tableMapping.ColumnMappings
                        .Add("Store", _previewData.CompetitorImportInfo.CompetitorStoreColumn.ColumnName)
                        .Add("UPC Code", _previewData.CompetitorImportInfo.UPCCodeColumn.ColumnName)
                        .Add("Unit of Measure", _previewData.CompetitorImportInfo.UnitOfMeasureColumn.ColumnName)
                        .Add("Price Multiple", _previewData.CompetitorImportInfo.PriceMultipleColumn.ColumnName)
                        .Add("Sale Multiple", _previewData.CompetitorImportInfo.SaleMultipleColumn.ColumnName)
                        .Add("Date Checked", _previewData.CompetitorImportInfo.DateCheckedColumn.ColumnName)
                    End With
                    
                    excelAdapter.Fill(_previewData)
                End Using

                Return True
            Catch ex As ConstraintException
                Dim errorMessageBuilder As New StringBuilder()
                Dim row As CompetitorStoreDataSet.CompetitorImportInfoRow
                Dim errorCount As Integer = 0

                For i As Integer = 0 To _previewData.CompetitorImportInfo.Count - 1
                    If errorCount >= MAX_ERROR_COUNT Then
                        Exit For
                    End If

                    row = _previewData.CompetitorImportInfo(i)

                    If row.HasErrors Then
                        AddRowErrorMessage(errorMessageBuilder, row, errorCount, i)
                    End If
                Next

                If errorCount >= MAX_ERROR_COUNT Then
                    errorMessageBuilder.AppendLine()
                    errorMessageBuilder.AppendFormat(My.Resources.CompetitorStore.ImportErrorLimit, MAX_ERROR_COUNT)
                    errorMessageBuilder.AppendLine()
                End If

                If errorMessageBuilder.Length = 0 Then
                    SetErrorMessage(My.Resources.CompetitorStore.ImportErrorMessage)
                Else
                    MessageBox.Show(errorMessageBuilder.ToString(), My.Resources.CompetitorStore.ImportErrorTitle)
                End If

                Return False
            Catch ex As Exception
                SetErrorMessage(String.Format(My.Resources.CompetitorStore.ImportErrorGeneric, ex.Message))
                Return False
            End Try
        End Function

#End Region

#Region "Upload"

#Region "Callbacks"

        Private Sub CompetitorImportInfo_RowSaved(ByVal index As Integer) Handles _competitorImportInfoDataObject.CompetitorImportInfoRowSaved
            _progressDialog.UpdateProgressDialogValue(index)
        End Sub

        Private Sub UploadComplete()
            _progressDialog.CloseProgressDialog()

            lblWait.Text = My.Resources.CompetitorStore.ImportMatching

            _matchingStart = DateTime.Now

            MatchImportedData(Nothing)
        End Sub

        Private Sub UploadFailure()
            lblWait.Visible = False
            _progressDialog.CloseProgressDialog()

            SetErrorMessage(My.Resources.CompetitorStore.UploadFailed)

            EnableInputControls(True)
        End Sub

#End Region

        Private Sub UploadData(ByVal state As Object)
            If CompetitorImportInfoDataObject.ImportCompetitorData(_previewData, giUserID, _currentSession, ROWS_PER_BATCH) _
                AndAlso _currentSession IsNot Nothing Then

                UploadComplete()
            Else
                UploadFailure()
            End If
        End Sub

        Private Sub MatchImportedData(ByVal state As Object)
            FiscalWeek.List(_previewData)
            ItemUnit.List(_previewData)

            CompetitorImportInfoDataObject.MatchIdentifiers(_currentSession.CompetitorImportSessionID, _previewData)

            MatchingComplete()
        End Sub

        Private Sub MatchingComplete()
            Dim preview As ImportPreview
            Dim uploadDuration As TimeSpan = _matchingStart.Subtract(_uploadStart)
            Dim matchingDuration As TimeSpan = DateTime.Now.Subtract(_matchingStart)

            MessageBox.Show(String.Format(My.Resources.CompetitorStore.UploadStatistics, vbNewLine, uploadDuration.TotalSeconds, matchingDuration.TotalSeconds), "Upload Complete")

            Me.Close()

            preview = New ImportPreview(_previewData, _currentSession)

            Me.DialogResult = preview.ShowDialog()

            _selectedCompetitorStore = preview.SelectedCompetitorStore
            _selectedFiscalWeekDescription = preview.SelectedFiscalWeekDescription
        End Sub

#End Region

        Private Sub SetErrorMessage(ByRef message As String)
            lblError.Visible = True
            lblError.Text = message
        End Sub

        Private Sub EnableInputControls(ByVal enabled As Boolean)
            btnUpload.Enabled = enabled
            btnCancel.Enabled = enabled
            btnBrowse.Enabled = enabled
            txtFilePath.Enabled = enabled
        End Sub

#End Region

#Region "Button Handlers"

        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Me.Close()
        End Sub

        Private Sub btnUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpload.Click
            If (IO.File.Exists(txtFilePath.Text)) Then
                Dim workBook As ExcelInterop.Workbook = Nothing

                Windows.Forms.Cursor.Current = Cursors.WaitCursor

                EnableInputControls(False)

                lblWait.Visible = True
                lblWait.Text = My.Resources.CompetitorStore.ImportValidating

                lblError.Visible = False

                _previewData = New CompetitorStoreDataSet()

                ' Open the file and ensure that it's in the format we're expecting. 
                ' Validation method will set its own error message
                If (ValidateExcelFile(workBook)) Then
                    workBook = Nothing

                    Windows.Forms.Cursor.Current = Cursors.Default

                    lblWait.Text = My.Resources.CompetitorStore.ImportUploadingData
                    lblWait.Visible = True

                    _uploadStart = DateTime.Now

                    _progressDialog = ProgressDialog.OpenProgressDialog(Me, _
                        My.Resources.CompetitorStore.UploadProgressTitle, _
                        My.Resources.CompetitorStore.UploadProgressMessage, _
                        My.Resources.CompetitorStore.UploadProgressItemName, _
                        _previewData.CompetitorImportInfo.Rows.Count, 0)

                    UploadData(Nothing)
                Else
                    EnableInputControls(True)

                    Windows.Forms.Cursor.Current = Cursors.Default
                End If
            Else
                SetErrorMessage(My.Resources.CompetitorStore.FileNotFound)
            End If
        End Sub

        Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
            Dim dialog As OpenFileDialog = New OpenFileDialog()

            ' We only want excel files
            dialog.Filter = "Excel|*.xls"

            If (dialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                txtFilePath.Text = dialog.FileName
            End If
        End Sub

#End Region

    End Class
End Namespace