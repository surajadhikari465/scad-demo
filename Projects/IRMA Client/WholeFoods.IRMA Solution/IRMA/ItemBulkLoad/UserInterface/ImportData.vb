Option Strict Off
Imports Microsoft.Office.Interop.Excel
Imports System
Imports System.Data.DataTable
Imports Infragistics.Win
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports system.Data.SqlClient
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
Imports WholeFoods.IRMA.ItemBulkLoad.DataAccess

Public Class ImportData
    Private SendMailClient As System.Net.Mail.SmtpClient

    Dim WithEvents xlBook As Workbook
    Dim excelWorksheet As Worksheet
    Dim excelSheets As Sheets
    Dim selectedFile As String
    Dim mdt As System.Data.DataTable
    Dim taxClassArray() As Integer
    Dim nationalClassArray() As Integer
    Dim validRows As Integer
    Dim mipriceBatchHeaderId As Integer
    Dim miItemUploadHeaderId As Integer
    Dim itemBulkLoadDAO As ItemMaintenanceBulkLoadDAO
    Dim hasChanged As Boolean
    Dim enableValidateButton As Boolean
    Dim enableUploadButton As Boolean



    Private Sub ImportData_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If (hasChanged = True) Then
            If DataChanged() <> MsgBoxResult.Yes Then
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    Private Function DataChanged() As MsgBoxResult
        Dim response As MsgBoxResult

        response = MsgBox(ResourcesItemBulkLoad.GetString("DataChanged_WarningMessage"), MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, Me.Text)
        If response = MsgBoxResult.Yes Then
            hasChanged = False
            ' Call stored procedure to delete the item upload header and its detail records
            ' as the user decided not to upload.
            Dim headerBO As New ItemUploadHeaderBO
            headerBO.ItemUploadHeaderID = miItemUploadHeaderId
            headerBO.Delete()
        End If
        Return response

    End Function

    Private Sub frmImportData_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        validRows = 0
        CenterForm(Me)
        hasChanged = False

        ' Set the enabled property to false to make sure the validate and upload buttons are
        ' enabled only after the grid paints itself. This will be set to true when the user
        ' imports the spreadsheet.
        ugrdBulkLoad.Enabled = False
    End Sub


    Private Sub btnSelectFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelectFile.Click
        Me.Enabled = False

        If (hasChanged = True) Then
            ' The user must have already opened a file and is reopening another one
            ' without uploading
            If DataChanged() <> MsgBoxResult.Yes Then
                Exit Sub
            Else
                ' clear the grid while the next file is being selected and imported
                ugrdBulkLoad.DataSource = Nothing
            End If
        End If
        txtFile.Text = Nothing
        selectFileDialog.InitialDirectory = My.Application.Info.DirectoryPath & ConfigurationServices.AppSettings("localItemBulkLoadDirectory")
        selectFileDialog.ShowDialog()
        selectedFile = selectFileDialog.FileName()
        Dim oldCursor As Cursor = Me.Cursor

        If Not String.IsNullOrEmpty(selectedFile) Then
            txtFile.Text = CStr(StripFile(selectedFile))
            Dim lastIndex As Integer = selectedFile.LastIndexOf(".")
            Dim extension As String = Mid$(selectedFile, lastIndex + 2, Len(selectedFile))
            If Not StrComp(extension, "xls", CompareMethod.Text) = 0 Then
                MsgBox("Only Excel Spreadsheets (.xls) can be uploaded!", MsgBoxStyle.OkOnly, Me.Text)
            Else
                Me.Cursor = Cursors.WaitCursor

                xlBook = CType(GetObject(selectedFile), Workbook)

                excelSheets = xlBook.Worksheets
                excelWorksheet = CType(excelSheets.Item(1), Worksheet)

                If excelWorksheet.UsedRange.Rows.Count < 2 Then
                    MsgBox("Worksheet is empty - cannot continue", vbCritical, xlBook.Name)
                    Exit Sub
                Else
                    DisplaySpreadsheetData(excelWorksheet)
                    ' Set the enabled property to true to enable the validate and upload
                    ' buttons.
                    ugrdBulkLoad.Enabled = True
                End If

                xlBook.Close()

                hasChanged = True
                Me.Cursor = oldCursor
            End If
        End If
        Me.Enabled = True

    End Sub


    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub


    Private Sub btnValidate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValidate.Click
        ValidateGridData()
    End Sub


    Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpload.Click
        Dim row As DataRow
        Dim i As Integer
        'Dim itemCount As Integer
        'Dim itemKeyArray(validRows - 1) As Integer
        Dim itemKeyArrayList As New ArrayList
        Dim itemBO As ItemMaintenanceBulkLoadBO
        Dim detailBO As ItemUploadDetailBO

        Me.Enabled = False

        ' Validate before uploading
        ValidateGridData()

        Try
            If (mdt IsNot Nothing) Then
                For i = 0 To mdt.Rows.Count - 1
                    row = mdt.Rows(i)
                    'Added condition to skip any deleted rows from the grid from being validated.
                    If Not row.RowState = DataRowState.Deleted Then
                        detailBO = New ItemUploadDetailBO
                        detailBO.ItemUploadDetail_ID = row(ResourcesItemBulkLoad.GetString("ItemUploadDetailID_Column"))

                        If Not row.HasErrors Then
                            '' As we need to create a batch for every subteam, we need to create an array
                            '' of items for each subteam to be passed for price batch creation
                            'If subTeamNo = Nothing Then
                            '    subTeamNo = row(ResourcesItemBulkLoad.GetString("SubTeamNo_Column"))
                            'ElseIf subTeamNo <> row(ResourcesItemBulkLoad.GetString("SubTeamNo_Column")) Then
                            '    ' Automatically creating a price batch header
                            '    BatchItemChanges(itemKeyArrayList)
                            '    itemKeyArrayList = New ArrayList
                            '    'itemCount = itemCount + 1
                            '    'itemKeyArray(itemCount) = itemBulkLoadDAO.UpdateItemAndPrice(itemBO)
                            'End If

                            itemBO = New ItemMaintenanceBulkLoadBO

                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("Identifier_Column"))) Then
                                itemBO.ItemIdentifier = -1
                            Else
                                itemBO.ItemIdentifier = row(ResourcesItemBulkLoad.GetString("Identifier_Column"))
                            End If

                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("POSDescription_Column"))) Then
                                itemBO.PosDescription = Nothing
                            Else
                                itemBO.PosDescription = ConvertQuotes(row(ResourcesItemBulkLoad.GetString("POSDescription_Column")))
                            End If


                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("ItemDescription_Column"))) Then
                                itemBO.ItemDescription = Nothing
                            Else
                                itemBO.ItemDescription = ConvertQuotes(row(ResourcesItemBulkLoad.GetString("ItemDescription_Column")))
                            End If




                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("Discontinued_Column"))) Then
                                itemBO.DiscontinueItem = -1
                            Else
                                itemBO.DiscontinueItem = Math.Abs(CInt(row(ResourcesItemBulkLoad.GetString("Discontinued_Column")))).ToString
                            End If

                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("Discountable_Column"))) Then
                                itemBO.Discountable = -1
                            Else
                                itemBO.Discountable = Math.Abs(CInt(row(ResourcesItemBulkLoad.GetString("Discountable_Column")))).ToString
                            End If

                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("FoodStamps_Column"))) Then
                                itemBO.FoodStamps = -1
                            Else
                                itemBO.FoodStamps = Math.Abs(CInt(row(ResourcesItemBulkLoad.GetString("FoodStamps_Column")))).ToString
                            End If

                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("NationalClass_Column"))) Then
                                itemBO.NationalClassID = Nothing
                            Else
                                itemBO.NationalClassID = row(ResourcesItemBulkLoad.GetString("NationalClass_Column"))
                            End If

                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("TaxClass_Column"))) Then
                                itemBO.TaxClassId = Nothing
                            Else
                                itemBO.TaxClassId = row(ResourcesItemBulkLoad.GetString("TaxClass_Column"))
                            End If

                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("RestrictedHrs_Column"))) Then
                                itemBO.RestrictedHours = -1
                            Else
                                itemBO.RestrictedHours = Math.Abs(CInt(row(ResourcesItemBulkLoad.GetString("RestrictedHrs_Column")))).ToString
                            End If

                            ' Call UpdateItemUploadDetail to update the detail record                       
                            detailBO.ItemIdentifier = itemBO.ItemIdentifier
                            detailBO.PosDescription = itemBO.PosDescription
                            detailBO.Description = itemBO.ItemDescription
                            detailBO.TaxClassID = itemBO.TaxClassId
                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("FoodStamps_Column"))) Then
                                detailBO.FoodStamps = Nothing
                            Else
                                detailBO.FoodStamps = Math.Abs(CInt(row(ResourcesItemBulkLoad.GetString("FoodStamps_Column")))).ToString
                            End If
                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("RestrictedHrs_Column"))) Then
                                detailBO.RestrictedHours = Nothing
                            Else
                                detailBO.RestrictedHours = Math.Abs(CInt(row(ResourcesItemBulkLoad.GetString("RestrictedHrs_Column")))).ToString
                            End If
                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("Discountable_Column"))) Then
                                detailBO.EmployeeDiscountable = Nothing
                            Else
                                detailBO.EmployeeDiscountable = Math.Abs(CInt(row(ResourcesItemBulkLoad.GetString("Discountable_Column")))).ToString
                            End If
                            If IsDBNull(row(ResourcesItemBulkLoad.GetString("Discontinued_Column"))) Then
                                detailBO.Discontinued = Nothing
                            Else
                                detailBO.Discontinued = Math.Abs(CInt(row(ResourcesItemBulkLoad.GetString("Discontinued_Column")))).ToString
                            End If
                            detailBO.NationalClassID = itemBO.NationalClassID
                            detailBO.Uploaded = 1

                            itemKeyArrayList.Add(itemBulkLoadDAO.UpdateItemAndPrice(itemBO))
                            validRows = validRows + 1

                            ' Update the Item Upload Detail object with any changes that the
                            ' user might have made.
                            detailBO.Update()
                        Else
                            detailBO.Uploaded = 0
                        End If
                    End If
                Next

                Dim resultsMessage As String
                resultsMessage = UpdateItemUploadHeader()

                MsgBox(resultsMessage, MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)

                ' Email results
                EmailUploadConfirmation(resultsMessage)

                hasChanged = False

            End If
        Catch ex As Exception
            Throw ex
        End Try

        Me.Enabled = True

        Dim msgBoxResult As MsgBoxResult
        msgBoxResult = MsgBox("Do you want to upload another spreadsheet?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text)

        If msgBoxResult = msgBoxResult.Yes Then
            ClearForm()
        Else
            Me.Close()
        End If
    End Sub


    Private Sub ugrdBulkLoad_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdBulkLoad.EnabledChanged
        If ugrdBulkLoad.Enabled = True Then
            If enableValidateButton Then
                Me.btnValidate.Enabled = True
            End If

            If enableUploadButton Then
                Me.btnUpload.Enabled = True
            End If
        End If
    End Sub


    ' Function to strip the file to be used to upload of all slashes
    Private Function StripFile(ByVal sFullFileName As String) As String
        Dim iLastPos As Integer, iPos As Integer
        iPos = InStr(1, sFullFileName, "\")
        Do Until iPos = 0
            iLastPos = iPos
            iPos = InStr(iPos + 1, sFullFileName, "\")
        Loop
        StripFile = Mid$(sFullFileName, iLastPos + 1, Len(sFullFileName) - iLastPos)
    End Function


    ' Function to find the column in the worksheet
    Private Function Findcol(ByVal sh As Worksheet, ByVal sSearch As String) As Long
        On Error Resume Next
        Findcol = sh.Cells.Find(What:=sSearch, _
                                MatchCase:=True, _
                                LookAt:=XlLookAt.xlWhole).Column
        On Error GoTo 0
    End Function


    ' Sub routine to populate the grid with data from worksheet
    Private Function DisplaySpreadsheetData(ByVal excelWorksheet As Worksheet) As Boolean
        Dim lSubTeamNameCol As Long, lIdentifierCol As Long, lPOSDescCol As Long, lItemDescCol As Long, _
        lTaxClassCol As Long, lFoodStampsCol As Long, lRestrictedHrsCol As Long, _
        lEmployeeDiscCol As Long, lDiscontCol As Long, lNationalClassCol As Long
        Dim sErrMsg As String

        itemBulkLoadDAO = New ItemMaintenanceBulkLoadDAO()

        lSubTeamNameCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("SubTeamName_Column"))
        lIdentifierCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("Identifier_Column"))
        lPOSDescCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("POSDescription_Column"))
        lItemDescCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("ItemDescription_Column"))
        lTaxClassCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("TaxClass_Column"))
        lFoodStampsCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("FoodStamps_Column"))
        lRestrictedHrsCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("RestrictedHrs_Column"))
        lEmployeeDiscCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("Discountable_Column"))
        lDiscontCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("Discontinued_Column"))
        lNationalClassCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("NationalClass_Column"))

        sErrMsg = ""
        If lIdentifierCol = 0 Then
            sErrMsg = vbTab & Space(3) & ResourcesItemBulkLoad.GetString("Required_Identifier") & vbCrLf
        Else
            ' Insert a record into ItemUploadHeader and ItemUploadDetails table
            InsertItemUploadHeaderAndDetails(excelWorksheet)

            Dim itemUploadBO As New ItemBulkLoadDisplayBO
            Dim bulkDAO As New ItemBulkLoadDisplayDAO

            ' Get the just inserted details record to be displayed in the Grid
            itemUploadBO.ItemUploadHeaderID = miItemUploadHeaderId
            itemUploadBO.ValidItems = bulkDAO.GetValidItems(miItemUploadHeaderId)

            '' Create the data table
            Call SetupDataTable()

            ''Get the list of tax classes
            GetTaxClass()

            ''Get the list of national classes
            GetNationalClass()

            PopulateGrid(itemUploadBO)

            mdt.AcceptChanges()

            ' Set the datasource on the grid to the newly created data table
            Me.ugrdBulkLoad.DataSource = mdt

            ' Set the settings on the grid to support error display
            ugrdBulkLoad.DisplayLayout.Override.SupportDataErrorInfo = UltraWinGrid.SupportDataErrorInfo.RowsAndCells
            ugrdBulkLoad.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True

            ' Based on validation, change the back colors and the tool tip text
            ValidateSpreadsheetData()

            Return True

        End If

    End Function

    ' This sub routine populates the grid with the information passed in by the ItemBulkLoadDisplay object
    Private Sub PopulateGrid(ByVal obj As ItemBulkLoadDisplayBO)
        Dim row As DataRow
        Dim exceptionMessage As String

        exceptionMessage = ""

        For Each oneInst As ItemUploadDetailBO In obj.ValidItems
            row = mdt.NewRow

            exceptionMessage = GetExceptionMessage(oneInst, row)

            If Not String.IsNullOrEmpty(exceptionMessage) Then
                exceptionMessage = exceptionMessage & vbCrLf & vbCrLf & ResourcesItemBulkLoad.GetString("Correct_Spreadsheet")
                MsgBox(exceptionMessage, MsgBoxStyle.OkOnly, "Upload Error")
                Continue For
            End If

            ' Validate the POS Description
            If Not String.IsNullOrEmpty(oneInst.PosDescription) Then
                If Trim(oneInst.PosDescription.Length) > 26 Then
                    If (row.HasErrors) Then
                        row.RowError = row.RowError & ",2"
                    Else
                        row.RowError = "2"
                    End If
                End If
            End If

            ' Validate the Item Description
            If Not String.IsNullOrEmpty(oneInst.Description) Then
                If Trim(oneInst.Description.Length) > 60 Then
                    If (row.HasErrors) Then
                        row.RowError = row.RowError & ",3"
                    Else
                        row.RowError = "3"
                    End If
                End If
            End If

            ' Validate the tax class id
            Dim taxClassIndex As Integer
            If Not oneInst.TaxClassID Is Nothing Then
                taxClassIndex = Array.IndexOf(taxClassArray, Integer.Parse(oneInst.TaxClassID))
                If taxClassIndex = -1 Then
                    If (row.HasErrors) Then
                        row.RowError = row.RowError & ",4"
                    Else
                        row.RowError = "4"
                    End If
                End If
            End If


            ' Validate the national class id
            Dim nationalClassIndex As Integer
            If Not oneInst.NationalClassID Is Nothing Then
                nationalClassIndex = Array.IndexOf(nationalClassArray, Integer.Parse(oneInst.NationalClassID))
                If nationalClassIndex = -1 Then
                    If (row.HasErrors) Then
                        row.RowError = row.RowError & ",9"
                    Else
                        row.RowError = "9"
                    End If
                End If
            End If

            ' Populate the columns with the respective values
            If oneInst.SubTeamName Is Nothing Or String.IsNullOrEmpty(oneInst.SubTeamName) Then
                row(ResourcesItemBulkLoad.GetString("SubTeamName_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("SubTeamName_Column")) = oneInst.SubTeamName
            End If

            If oneInst.ItemIdentifier Is Nothing Or String.IsNullOrEmpty(oneInst.ItemIdentifier) Then
                row(ResourcesItemBulkLoad.GetString("Identifier_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("Identifier_Column")) = oneInst.ItemIdentifier
            End If

            If Trim(oneInst.PosDescription) Is Nothing Or String.IsNullOrEmpty(oneInst.PosDescription) Then
                row(ResourcesItemBulkLoad.GetString("POSDescription_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("POSDescription_Column")) = Trim(oneInst.PosDescription)
            End If

            If Trim(oneInst.Description) Is Nothing Or String.IsNullOrEmpty(oneInst.Description) Then
                row(ResourcesItemBulkLoad.GetString("ItemDescription_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("ItemDescription_Column")) = Trim(oneInst.Description)
            End If

            If oneInst.TaxClassID Is Nothing Then
                row(ResourcesItemBulkLoad.GetString("TaxClass_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("TaxClass_Column")) = oneInst.TaxClassID
            End If

            If oneInst.FoodStamps Is Nothing Then
                row(ResourcesItemBulkLoad.GetString("FoodStamps_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("FoodStamps_Column")) = CBool(oneInst.FoodStamps)
            End If

            If oneInst.RestrictedHours Is Nothing Then
                row(ResourcesItemBulkLoad.GetString("RestrictedHrs_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("RestrictedHrs_Column")) = CBool(oneInst.RestrictedHours)
            End If

            If oneInst.EmployeeDiscountable Is Nothing Then
                row(ResourcesItemBulkLoad.GetString("Discountable_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("Discountable_Column")) = CBool(oneInst.EmployeeDiscountable)
            End If

            If oneInst.Discontinued Is Nothing Then
                row(ResourcesItemBulkLoad.GetString("Discontinued_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("Discontinued_Column")) = CBool(oneInst.Discontinued)
            End If

            If oneInst.NationalClassID Is Nothing Then
                row(ResourcesItemBulkLoad.GetString("NationalClass_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("NationalClass_Column")) = oneInst.NationalClassID
            End If

            row(ResourcesItemBulkLoad.GetString("ItemUploadDetailID_Column")) = oneInst.ItemUploadDetail_ID

            If oneInst.SubTeamNo = Nothing Then
                row(ResourcesItemBulkLoad.GetString("SubTeamNo_Column")) = DBNull.Value
            Else
                row(ResourcesItemBulkLoad.GetString("SubTeamNo_Column")) = oneInst.SubTeamNo
            End If

            row(ResourcesItemBulkLoad.GetString("ItemIdentifierValid_Column")) = oneInst.ItemIdentifierValid

            row(ResourcesItemBulkLoad.GetString("SubTeamAllowed_Column")) = oneInst.SubTeamAllowed

            If oneInst.ItemIdentifierValid = 0 Then
                'row.RowError = "1"
                If (row.HasErrors) Then
                    row.RowError = row.RowError & ",1"
                Else
                    row.RowError = "1"
                End If
            ElseIf oneInst.ItemIdentifierValid = 1 And oneInst.SubTeamAllowed = 0 Then
                'row.RowError = "10"
                If (row.HasErrors) Then
                    row.RowError = row.RowError & ",10"
                Else
                    row.RowError = "10"
                End If
            End If

            mdt.Rows.Add(row)
        Next
    End Sub


    ' This function returns any exceptions that might be thrown during the validation of
    ' FoodStamps, Restricted Hours, Employee Discount or Discontinued
    Private Function GetExceptionMessage(ByVal oneInst As ItemUploadDetailBO, ByVal row As DataRow) As String

        Dim exceptionMessage As String
        exceptionMessage = String.Empty

        ' Validate the Food Stamps value
        Try
            If (CInt(oneInst.FoodStamps) < 0 Or CInt(oneInst.FoodStamps) > 1) Then
                exceptionMessage = "Errors for Identifier, " & oneInst.ItemIdentifier & ":" & vbCrLf & vbCrLf
                exceptionMessage = exceptionMessage & ResourcesItemBulkLoad.GetString("Invalid_FoodStamp")
                If (row.HasErrors) Then
                    row.RowError = row.RowError & ",5"
                Else
                    row.RowError = "5"
                End If
            End If

        Catch
            exceptionMessage = "Errors for Identifier, " & oneInst.ItemIdentifier & ":" & vbCrLf & vbCrLf
            exceptionMessage = exceptionMessage & vbCrLf & ResourcesItemBulkLoad.GetString("Invalid_FoodStamp")
        End Try

        ' Validate the Restricted Hours value
        Try
            If (oneInst.RestrictedHours < 0 Or oneInst.RestrictedHours > 1) Then
                If String.IsNullOrEmpty(exceptionMessage) Then
                    exceptionMessage = "Errors for Identifier, " & oneInst.ItemIdentifier & ":" & vbCrLf & vbCrLf
                End If
                exceptionMessage = exceptionMessage & vbCrLf & ResourcesItemBulkLoad.GetString("Invalid_RestrictedHrs")
                If (row.HasErrors) Then
                    row.RowError = row.RowError & ",6"
                Else
                    row.RowError = "6"
                End If
            End If
        Catch
            If String.IsNullOrEmpty(exceptionMessage) Then
                exceptionMessage = "Errors for Identifier, " & oneInst.ItemIdentifier & ":" & vbCrLf & vbCrLf
            End If
            exceptionMessage = exceptionMessage & vbCrLf & ResourcesItemBulkLoad.GetString("Invalid_RestrictedHrs")
        End Try

        ' Validate the Employee Discount value
        Try
            If (oneInst.EmployeeDiscountable < 0 Or oneInst.EmployeeDiscountable > 1) Then
                If String.IsNullOrEmpty(exceptionMessage) Then
                    exceptionMessage = "Errors for Identifier, " & oneInst.ItemIdentifier & ":" & vbCrLf & vbCrLf
                End If
                exceptionMessage = exceptionMessage & vbCrLf & ResourcesItemBulkLoad.GetString("Invalid_EmployeeDiscount")
                If (row.HasErrors) Then
                    row.RowError = row.RowError & ",7"
                Else
                    row.RowError = "7"
                End If
            End If
        Catch
            If String.IsNullOrEmpty(exceptionMessage) Then
                exceptionMessage = "Errors for Identifier, " & oneInst.ItemIdentifier & ":" & vbCrLf & vbCrLf
            End If
            exceptionMessage = exceptionMessage & vbCrLf & ResourcesItemBulkLoad.GetString("Invalid_EmployeeDiscount")
        End Try

        ' Validate the Discontinued value
        Try
            If (oneInst.Discontinued < 0 Or oneInst.Discontinued > 1) Then
                If String.IsNullOrEmpty(exceptionMessage) Then
                    exceptionMessage = "Errors for Identifier, " & oneInst.ItemIdentifier & ":" & vbCrLf & vbCrLf
                End If
                exceptionMessage = exceptionMessage & vbCrLf & ResourcesItemBulkLoad.GetString("Invalid_Discontinued")
                If (row.HasErrors) Then
                    row.RowError = row.RowError & ",8"
                Else
                    row.RowError = "8"
                End If
            End If
        Catch
            If String.IsNullOrEmpty(exceptionMessage) Then
                exceptionMessage = "Errors for Identifier, " & oneInst.ItemIdentifier & ":" & vbCrLf & vbCrLf
            End If
            exceptionMessage = exceptionMessage & vbCrLf & ResourcesItemBulkLoad.GetString("Invalid_Discontinued")
        End Try

        Return exceptionMessage

    End Function


    ' Sub routine to setup the data table to be used in the grid
    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New System.Data.DataTable(ResourcesItemBulkLoad.GetString("Screen_Title"))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("Identifier_Column"), GetType(String)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("POSDescription_Column"), GetType(String)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("ItemDescription_Column"), GetType(String)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("TaxClass_Column"), GetType(Integer)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("FoodStamps_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("RestrictedHrs_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("Discountable_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("Discontinued_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("NationalClass_Column"), GetType(Integer)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("ItemUploadDetailID_Column"), GetType(Integer)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("SubTeamNo_Column"), GetType(Integer)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("ItemIdentifierValid_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("SubTeamAllowed_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("SubTeamName_Column"), GetType(String)))

    End Sub


    ' Sub routine to retrieve national class for the drop down list
    Private Sub GetNationalClass()
        Dim nationalClassList As New ArrayList
        Dim itemBulkDAO As New ItemMaintenanceBulkLoadDAO
        Dim nationalClassBO As New NationalClassBO
        Dim i, id, k As Integer

        k = 0

        nationalClassList = itemBulkDAO.GetNationalClass()
        ReDim nationalClassArray(nationalClassList.Count - 1)

        Me.ugrdBulkLoad.DisplayLayout.ValueLists("NationalClass").ValueListItems.Clear()

        For i = 0 To nationalClassList.Count - 1
            nationalClassBO = nationalClassList.Item(i)
            id = nationalClassBO.ClassID
            Me.ugrdBulkLoad.DisplayLayout.ValueLists("NationalClass").ValueListItems.Add(id, nationalClassBO.ClassName)
            nationalClassArray(k) = id
            k = k + 1
        Next

    End Sub


    ' Sub routine to retrieve tax classes for the drop down list
    Private Sub GetTaxClass()
        Dim taxClassList As New ArrayList
        Dim itemBulkDAO As New ItemMaintenanceBulkLoadDAO
        Dim taxClassBO As New TaxClassBO
        Dim i, id, k As Integer

        k = 0

        taxClassList = itemBulkDAO.GetTaxClass()
        ReDim taxClassArray(taxClassList.Count - 1)

        Me.ugrdBulkLoad.DisplayLayout.ValueLists("TaxClass").ValueListItems.Clear()

        For i = 0 To taxClassList.Count - 1
            taxClassBO = taxClassList.Item(i)
            id = taxClassBO.TaxClassID
            Me.ugrdBulkLoad.DisplayLayout.ValueLists("TaxClass").ValueListItems.Add(id, taxClassBO.TaxClassDesc)
            taxClassArray(k) = id
            k = k + 1
        Next
    End Sub


    ' Sub routine to change the back colors and the tool tip text based on validation results
    Private Sub ValidateSpreadsheetData()
        Dim i As Integer
        Dim k As Integer

        enableValidateButton = False
        enableUploadButton = False

        k = 0
        For i = 0 To mdt.Rows.Count - 1
            If mdt.Rows(i).HasErrors Then
                Dim errorText As String
                Dim errors() As String
                Dim s As String

                errorText = mdt.Rows(i).RowError  'set the string from the input
                errors = errorText.Split(",")
                For Each s In errors
                    If s = "1" Then
                        ugrdBulkLoad.Rows(i).Cells(0).Appearance.BackColor = Color.Red
                        ugrdBulkLoad.Rows(i).Cells(0).ToolTipText = "Identifier " & ugrdBulkLoad.Rows(i).Cells(0).Value & ", does not exist!"
                        ugrdBulkLoad.Rows(i).Activation = UltraWinGrid.Activation.NoEdit
                        ugrdBulkLoad.Rows(i).Appearance.BackColor = Color.LightGray
                    ElseIf s = "2" Then
                        ugrdBulkLoad.Rows(i).Cells(1).Appearance.BackColor = Color.Yellow
                        ugrdBulkLoad.Rows(i).Cells(1).ToolTipText = ResourcesItemBulkLoad.GetString("POSDesc_MaxLength")
                    ElseIf s = "3" Then
                        ugrdBulkLoad.Rows(i).Cells(2).Appearance.BackColor = Color.Yellow
                        ugrdBulkLoad.Rows(i).Cells(2).ToolTipText = ResourcesItemBulkLoad.GetString("ItemDesc_MaxLength")
                    ElseIf s = "4" Then
                        ugrdBulkLoad.Rows(i).Cells(3).Appearance.BackColor = Color.Yellow
                        ugrdBulkLoad.Rows(i).Cells(3).ToolTipText = ResourcesItemBulkLoad.GetString("Invalid_TaxClass")
                    ElseIf s = "5" Then
                        ugrdBulkLoad.Rows(i).Cells(4).Appearance.BackColor = Color.Yellow
                        ugrdBulkLoad.Rows(i).Cells(4).ToolTipText = ResourcesItemBulkLoad.GetString("Invalid_FoodStamp")
                    ElseIf s = "6" Then
                        ugrdBulkLoad.Rows(i).Cells(5).Appearance.BackColor = Color.Yellow
                        ugrdBulkLoad.Rows(i).Cells(5).ToolTipText = ResourcesItemBulkLoad.GetString("Invalid_RestrictedHrs")
                    ElseIf s = "7" Then
                        ugrdBulkLoad.Rows(i).Cells(6).Appearance.BackColor = Color.Yellow
                        ugrdBulkLoad.Rows(i).Cells(6).ToolTipText = ResourcesItemBulkLoad.GetString("Invalid_EmployeeDiscount")
                    ElseIf s = "8" Then
                        ugrdBulkLoad.Rows(i).Cells(7).Appearance.BackColor = Color.Yellow
                        ugrdBulkLoad.Rows(i).Cells(7).ToolTipText = ResourcesItemBulkLoad.GetString("Invalid_Discontinued")
                    ElseIf s = "9" Then
                        ugrdBulkLoad.Rows(i).Cells(8).Appearance.BackColor = Color.Yellow
                        ugrdBulkLoad.Rows(i).Cells(8).ToolTipText = ResourcesItemBulkLoad.GetString("Invalid_NationalClass")
                    ElseIf s = "10" Then
                        ugrdBulkLoad.Rows(i).Cells(13).Appearance.BackColor = Color.Orange
                        ugrdBulkLoad.Rows(i).Cells(13).ToolTipText = ResourcesItemBulkLoad.GetString("SubTeam_NotAllowed")
                        ugrdBulkLoad.Rows(i).Activation = UltraWinGrid.Activation.NoEdit
                        ugrdBulkLoad.Rows(i).Appearance.BackColor = Color.LightGray
                    End If

                Next s

                'Tax Class is required for ALL items
                If ugrdBulkLoad.Rows(i).Cells(3).Value Is DBNull.Value Then
                    ugrdBulkLoad.Rows(i).Cells(3).Appearance.BackColor = Color.Red
                    ugrdBulkLoad.Rows(i).Cells(3).ToolTipText = String.Format(ResourcesCommon.GetString("msg_validation_required"), ResourcesItemBulkLoad.GetString("TaxClass_Column"))
                End If

                ' Enable the validate button only if the identifier is valid (and subteam allowed).  This is because
                ' if the identifier is invalid (or subteam disallowed), the user has to go back to the spreadsheet to
                ' correct it.
                If ugrdBulkLoad.Rows(i).Cells(0).Appearance.BackColor.Equals(Color.Empty) And _
                        ugrdBulkLoad.Rows(i).Cells(13).Appearance.BackColor.Equals(Color.Empty) Then
                    enableValidateButton = True
                End If
            Else
                k = k + 1
            End If
        Next

        If Not enableValidateButton Then
            If k > 0 And k <= mdt.Rows.Count Then
                enableValidateButton = True
                enableUploadButton = True
            End If
        Else
            enableValidateButton = True
            If k > 0 Then
                enableUploadButton = True
            End If
        End If

    End Sub


    ' Sub routine to create price batch records -- NOT CURRENTLY BEING USED
    Private Sub BatchItemChanges(ByVal itemKeyArrayList As ArrayList)
        Dim header As PriceBatchHeaderBO
        Dim headerDAO As New PriceBatchHeaderDAO
        Dim factory As DataFactory = Nothing
        Dim transaction As SqlTransaction = Nothing
        Dim priceBatchDetailIDs As String
        Dim itemKeyArray(itemKeyArrayList.Count) As Integer
        Dim i As Integer

        i = 0
        Dim myEnumerator As System.Collections.IEnumerator = itemKeyArrayList.GetEnumerator()
        While myEnumerator.MoveNext()
            itemKeyArray(i) = myEnumerator.Current
            i += 1
        End While

        priceBatchDetailIDs = String.Empty

        'open connection
        factory = New DataFactory(DataFactory.ItemCatalog)
        'start database transaction
        transaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

        header = New PriceBatchHeaderBO
        header.ItemChgTypeID = 2
        header.PriceChgTypeID = 0
        header.StartDate = Now.Date
        header.BatchDescription = String.Empty
        header.AutoApplyFlag = 0
        header.AutoApplyDate = Now.Date

        'storeList = itemBulkLoadDAO.GetStoreList()
        'For i = 0 To storeList.Count - 1
        'divide details by subteam

        'create new batch (new PriceBatchHeader record)
        mipriceBatchHeaderId = headerDAO.InsertPriceBatchHeader(header, transaction)

        'go get the list of pricebatchdetailIDs for the uploaded items
        priceBatchDetailIDs = headerDAO.GetPriceBatchDetailIDs(header, itemKeyArray)

        ' Add to update list
        header.PriceBatchDetailIDList = priceBatchDetailIDs
        header.DetailIDListSeparator = "|"
        header.PriceBatchHeaderId = mipriceBatchHeaderId

        'update all details now associated w/ this PriceBatchHeader ID
        headerDAO.UpdatePriceBatchDetails(header, transaction)

        transaction.Commit()
        'Next i

    End Sub


    ' Sub routine to clear the form so that the user can import another spreadsheet
    Private Sub ClearForm()
        ' Clear the data from the grid
        If Not mdt Is Nothing Then
            mdt.Clear()
        End If

        ' Clear the file selected
        txtFile.Clear()

        ' Disable the buttons
        btnValidate.Enabled = False
        btnUpload.Enabled = False

        ugrdBulkLoad.Enabled = False
    End Sub


    ' Sub routine to prepare an email to be sent after the upload has been completed
    Private Sub EmailUploadConfirmation(ByVal uploadMessage As String)

        Dim emailClient As WholeFoods.Utility.SMTP.SMTP = Nothing
        Dim emailFrom As String = ConfigurationServices.AppSettings("ItemBulkLoad_FromEmailAddress")
        Dim emailTo As String = ConfigurationServices.AppSettings("ItemBulkLoad_ToEmailAddress")
        Dim emailCC As String = ConfigurationServices.AppSettings("ItemBulkLoad_CCEmailAddress")
        Dim emailMessage As String
        Dim sVersion As String

        With My.Application.Info.Version
            sVersion = String.Format("{0}.{1}.{2}", .Major, .Minor, .Build)
        End With

        emailMessage = uploadMessage + vbCrLf + vbCrLf + String.Format(ResourcesItemBulkLoad.GetString("EmailMessage"), _
            gsUserName, vbCrLf, Now.ToString("yyyy-MM-dd HH:mm:ss"), vbCrLf & vbCrLf, sVersion, vbCrLf & vbCrLf, emailTo)

        Try
            emailClient = New WholeFoods.Utility.SMTP.SMTP(ConfigurationServices.AppSettings("EmailSMTPServer"))

            emailClient.send(emailMessage, emailTo, emailCC, emailFrom, ResourcesItemBulkLoad.GetString("EmailSubject"))

        Catch ex As Exception
            'e-mail confirmation shouldn't be a fatal error!
            ErrorHandler.ProcessError(ErrorType.GeneralApplicationError, SeverityLevel.Warning, ex)

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


    ' Sub routine to update the item upload header after the upload has been completed
    Private Function UpdateItemUploadHeader() As String
        Dim itemUploadHeaderBO As New ItemUploadHeaderBO
        Dim uploadResults As String
        uploadResults = String.Empty

        itemUploadHeaderBO.ItemUploadHeaderID = miItemUploadHeaderId
        itemUploadHeaderBO.ItemProcessedCount = ugrdBulkLoad.Rows.Count
        itemUploadHeaderBO.ItemLoadedCount = validRows
        itemUploadHeaderBO.ErrorsCount = ugrdBulkLoad.Rows.Count - validRows

        If itemUploadHeaderBO.Update() = True Then
            uploadResults = itemUploadHeaderBO.UploadResults
        Else
            uploadResults = ResourcesItemBulkLoad.GetString("EmailErrorMessage")
        End If

        Return uploadResults
    End Function


    ' Sub routine to insert Item Upload Header and details.  This is called when the
    ' user clicks on the Select File button to import a spreadsheet.
    Private Sub InsertItemUploadHeaderAndDetails(ByVal excelWorksheet As Worksheet)
        Dim itemUploadHeaderBO As New ItemUploadHeaderBO
        Dim itemUploadDetailBO As New ItemUploadDetailBO
        Dim headerDAO As New ItemMaintenanceBulkLoadDAO
        Dim i As Integer
        Dim detailColl As New Collection
        Dim lIdentifierCol As Long, lPOSDescCol As Long, lItemDescCol As Long, _
            lTaxClassCol As Long, lFoodStampsCol As Long, lRestrictedHrsCol As Long, _
            lEmployeeDiscCol As Long, lDiscontCol As Long, lNationalClassCol As Long

        itemUploadHeaderBO.ItemUploadTypeID = CInt(ResourcesItemBulkLoad.GetString("ItemUpload_Type"))
        itemUploadHeaderBO.ItemProcessedCount = excelWorksheet.UsedRange.Rows.Count
        itemUploadHeaderBO.EmailToAddress = ConfigurationServices.AppSettings("ItemBulkLoad_ToEmailAddress")
        itemUploadHeaderBO.UserID = giUserID

        lIdentifierCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("Identifier_Column"))
        lPOSDescCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("POSDescription_Column"))
        lItemDescCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("ItemDescription_Column"))
        lTaxClassCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("TaxClass_Column"))
        lFoodStampsCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("FoodStamps_Column"))
        lRestrictedHrsCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("RestrictedHrs_Column"))
        lEmployeeDiscCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("Discountable_Column"))
        lDiscontCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("Discontinued_Column"))
        lNationalClassCol = Findcol(excelWorksheet, ResourcesItemBulkLoad.GetString("NationalClass_Column"))

        For i = 2 To excelWorksheet.UsedRange.Rows.Count
            itemUploadDetailBO = New ItemUploadDetailBO

            ' Populate the columns with the respective values
            itemUploadDetailBO.ItemIdentifier = IIf(excelWorksheet.Cells(i, lIdentifierCol).Value Is Nothing, Nothing, excelWorksheet.Cells(i, lIdentifierCol).Value)
            itemUploadDetailBO.PosDescription = IIf(Trim(excelWorksheet.Cells(i, lPOSDescCol).Value) Is Nothing, Nothing, Trim(excelWorksheet.Cells(i, lPOSDescCol).Value))
            itemUploadDetailBO.Description = IIf(Trim(excelWorksheet.Cells(i, lItemDescCol).Value) Is Nothing, Nothing, Trim(excelWorksheet.Cells(i, lItemDescCol).Value))
            itemUploadDetailBO.TaxClassID = IIf(excelWorksheet.Cells(i, lTaxClassCol).Value Is Nothing, Nothing, CStr(excelWorksheet.Cells(i, lTaxClassCol).Value))
            itemUploadDetailBO.FoodStamps = IIf(excelWorksheet.Cells(i, lFoodStampsCol).Value Is Nothing, Nothing, CStr(excelWorksheet.Cells(i, lFoodStampsCol).Value))
            itemUploadDetailBO.RestrictedHours = IIf(excelWorksheet.Cells(i, lRestrictedHrsCol).Value Is Nothing, Nothing, CStr(excelWorksheet.Cells(i, lRestrictedHrsCol).Value))
            itemUploadDetailBO.EmployeeDiscountable = IIf(excelWorksheet.Cells(i, lEmployeeDiscCol).Value Is Nothing, Nothing, CStr(excelWorksheet.Cells(i, lEmployeeDiscCol).Value))
            itemUploadDetailBO.Discontinued = IIf(excelWorksheet.Cells(i, lDiscontCol).Value Is Nothing, Nothing, CStr(excelWorksheet.Cells(i, lDiscontCol).Value))
            itemUploadDetailBO.NationalClassID = IIf(excelWorksheet.Cells(i, lNationalClassCol).Value Is Nothing, Nothing, CStr(excelWorksheet.Cells(i, lNationalClassCol).Value))

            detailColl.Add(itemUploadDetailBO)

        Next

        itemUploadHeaderBO.DetailColl = detailColl

        itemUploadHeaderBO.Insert()

        miItemUploadHeaderId = itemUploadHeaderBO.ItemUploadHeaderID

    End Sub


    ' Sub routine to validate the grid data
    Private Sub ValidateGridData()
        Dim i As Integer
        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim hasErrors As Boolean
        Dim taxClassIndex As Integer
        Dim nationalClassIndex As Integer

        For i = 0 To ugrdBulkLoad.Rows.Count - 1
            hasErrors = False
            row = ugrdBulkLoad.Rows(i)
            If ugrdBulkLoad.DataSource.rows(i).hasErrors Then
                ' Check if the identifier is invalid or subteam not allowed.  If so, skip validation as the row
                ' is read-only
                If row.Appearance.BackColor.Equals(Color.LightGray) Then
                    hasErrors = True
                    Continue For
                Else
                    ' Validate POS Description
                    If Trim(row.Cells(1).Value.ToString.Length) > 26 Then
                        row.Cells(1).Appearance.BackColor = Color.Yellow
                        row.Cells(1).ToolTipText = ResourcesItemBulkLoad.GetString("POSDesc_MaxLength")
                        hasErrors = True
                    Else
                        row.Cells(1).Appearance.BackColor = Color.Empty
                        row.Cells(1).ToolTipText = ""
                    End If

                    ' Validate Item Description
                    If Trim(row.Cells(2).Value.ToString.Length) > 60 Then
                        row.Cells(2).Appearance.BackColor = Color.Yellow
                        row.Cells(2).ToolTipText = ResourcesItemBulkLoad.GetString("ItemDesc_MaxLength")
                        hasErrors = True
                    Else
                        row.Cells(2).Appearance.BackColor = Color.Empty
                        row.Cells(2).ToolTipText = ""
                    End If

                    ' Validate Tax Class
                    If Not IsDBNull(row.Cells(3).Value) Then
                        taxClassIndex = Array.IndexOf(taxClassArray, Integer.Parse(row.Cells(3).Value))
                        If taxClassIndex = -1 Then
                            row.Cells(3).Appearance.BackColor = Color.Yellow
                            row.Cells(3).ToolTipText = ResourcesItemBulkLoad.GetString("Invalid_TaxClass")
                            hasErrors = True
                        Else
                            row.Cells(3).Appearance.BackColor = Color.Empty
                            row.Cells(3).ToolTipText = ""
                        End If
                    End If

                    ' Validate Food Stamps.  As the check box can be either checked or un-checked, 
                    ' remove the tool tip text and change the back color
                    If row.Cells(4).Appearance.BackColor = Color.Yellow Then
                        row.Cells(4).Appearance.BackColor = Color.Empty
                        row.Cells(4).ToolTipText = ""
                    End If

                    ' Validate Restricted Hours. As the check box can be either checked or un-checked, 
                    ' remove the tool tip text and change the back color
                    If row.Cells(5).Appearance.BackColor = Color.Yellow Then
                        row.Cells(5).Appearance.BackColor = Color.Empty
                        row.Cells(5).ToolTipText = ""
                    End If

                    ' Validate Employee Discount. As the check box can be either checked or un-checked, 
                    ' remove the tool tip text and change the back color
                    If row.Cells(6).Appearance.BackColor = Color.Yellow Then
                        row.Cells(6).Appearance.BackColor = Color.Empty
                        row.Cells(6).ToolTipText = ""
                    End If

                    ' Validate Discontinued. As the check box can be either checked or un-checked, 
                    ' remove the tool tip text and change the back color
                    If row.Cells(7).Appearance.BackColor = Color.Yellow Then
                        row.Cells(7).Appearance.BackColor = Color.Empty
                        row.Cells(7).ToolTipText = ""
                    End If

                    ' Validate the national class id
                    If Not IsDBNull(row.Cells(8).Value) Then
                        nationalClassIndex = Array.IndexOf(nationalClassArray, Integer.Parse(row.Cells(8).Value))
                        If nationalClassIndex = -1 Then
                            row.Cells(8).Appearance.BackColor = Color.Yellow
                            row.Cells(8).ToolTipText = ResourcesItemBulkLoad.GetString("Invalid_NationalClass")
                            hasErrors = True
                        Else
                            row.Cells(8).Appearance.BackColor = Color.Empty
                            row.Cells(8).ToolTipText = ""
                        End If
                    Else
                        row.Cells(8).Appearance.BackColor = Color.Empty
                        row.Cells(8).ToolTipText = ""
                    End If
                End If
            End If

            If (hasErrors = False) Then
                'validRows = validRows + 1
                ugrdBulkLoad.DataSource.rows(i).RowError = ""
                btnUpload.Enabled = True
            End If
        Next
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class
