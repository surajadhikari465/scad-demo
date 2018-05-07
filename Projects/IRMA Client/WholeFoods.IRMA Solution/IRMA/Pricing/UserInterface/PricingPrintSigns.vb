Imports System.IO
Imports System.Linq
Imports System.Net
Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess

Friend Class frmPricingPrintSigns
    Inherits System.Windows.Forms.Form

    Private isInitializing As Boolean
    Private isLoading As Boolean
    Private multipleIdentifiersInputForm As MultipleValueInput = New MultipleValueInput()
    Private searchIdentifiers As List(Of String) = New List(Of String)
    Private pricingPrintSignsBusinessLogic As PricingPrintSignsBO = New PricingPrintSignsBO()
    Private storeNumberToBusinessUnit As Dictionary(Of Integer, Integer)

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub LoadDataTable(ByVal storeNumber As Integer, ByVal subteamNumber As String, ByVal categoryId As String,
                              ByVal signDescription As String, ByVal identifiers As String, ByVal brandId As String)
        logger.Debug("LoadDataTable Enter")

        Dim PriceBatchSearchDAO As New PriceBatchSearchDAO()
        Dim resultsTable As New DataTable()

        resultsTable = PriceBatchSearchDAO.PricingPrintSignsSearch(storeNumber, subteamNumber, categoryId, signDescription, identifiers, brandId)

        AddOrderingColumnBasedOnProvidedIdentifiers(identifiers, resultsTable)

        If resultsTable.Rows.Count > 0 Then
            ugrdSearchResults.DataSource = resultsTable
        Else
            MessageBox.Show("No results found.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        logger.Debug("LoadDataTable Exit")
    End Sub

    Private Sub frmPricingPrintSigns_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmPricingPrintSigns_Load Enter")

        isLoading = True

        CenterForm(Me)

        LoadRetailStore(cmbStore)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = 0
        End If

        LoadBrand(cmbBrand)

        storeNumberToBusinessUnit = StoreListDAO.GetStoreNumberToBusinessUnitCollection()

        isLoading = False

        logger.Debug("frmPricingPrintSigns_Load Exit")
    End Sub

    Private Sub cmbBrand_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbBrand.KeyPress
        logger.Debug("cmbBrand_KeyPress Enter")

        Dim keyAscii As Short = Asc(eventArgs.KeyChar)

        If keyAscii = 8 Then cmbBrand.SelectedIndex = -1

        eventArgs.KeyChar = Chr(keyAscii)

        If keyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("cmbBrand_KeyPress Exit")
    End Sub

    Private Sub cmbStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStore.SelectedIndexChanged
        logger.Debug("cmbStore_SelectedIndexChanged Enter")

        If isInitializing Then Exit Sub

        cmbSubTeam.Items.Clear()
        cmbCategory.Items.Clear()

        Call LoadSubTeamByType(Global_Renamed.enumSubTeamType.Retail, cmbSubTeam)

        cmbSubTeam.SelectedIndex = -1

        logger.Debug("cmbStore_SelectedIndexChanged Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdItemEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemEdit.Click
        logger.Debug("cmdItemEdit_Click Enter")

        If ugrdSearchResults.Selected.Rows.Count = 1 Then
            Me.Enabled = False

            glItemID = ugrdSearchResults.Selected.Rows(0).Cells("Item_Key").Value

            frmItem.ShowDialog()
            frmItem.Close()

            Me.Enabled = True
        Else
            MessageBox.Show(ResourcesIRMA.GetString("SelectSingleRow"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        logger.Debug("cmdItemEdit_Click Exit")
    End Sub

    Private Sub cmdPrintLabel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPrintLabel.Click
        logger.Debug("cmdPrintLabel_Click Enter")

        If ugrdSearchResults.Selected.Rows.Count = 0 Then Exit Sub

        Dim selectedStoreNumber As Integer = VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)
        Dim businessUnitsToSend As List(Of Integer) = New List(Of Integer)
        Dim applyNoTagLogic As Boolean
        Dim selectedSubteamName As String = If(ugrdSearchResults.Selected.Rows.Count > 0, "MultipleSubteams", ugrdSearchResults.Selected.Rows.Item(0).Cells("SubTeam_Name").Value)
        Dim selectedItems As List(Of ItemForAdHocPrint) = New List(Of ItemForAdHocPrint)
        Dim itemsExcluded As Boolean

        For Each selectedRow As UltraGridRow In ugrdSearchResults.Selected.Rows
            selectedItems.Add(New ItemForAdHocPrint() With {
                              .Identifier = selectedRow.Cells("Identifier").Value,
                              .ItemKey = selectedRow.Cells("Item_Key").Value,
                              .SubteamNumber = selectedRow.Cells("SubTeam_No").Value,
                              .SubteamName = selectedRow.Cells("SubTeam_Name").Value,
                              .RequestedOrder = selectedRow.Cells("RequestedOrder").Value,
                              .IdentifierIsValid = False,
                              .ExcludedByNoTagLogic = False})
        Next

        If SlawPrintBatchConfigurationDAO.SlawPrintRequestsEnabledForRegion() Then
            logger.Info("SLAW is enabled for this region.  Sending print request to SLAW API.")

            Dim printRequestBatchName As String

            Using printRequestBatchNameForm As New PrintRequestBatchName(selectedStoreNumber, selectedSubteamName)
                printRequestBatchNameForm.ShowDialog()

                If printRequestBatchNameForm.DialogResult = DialogResult.OK Then
                    printRequestBatchName = printRequestBatchNameForm.BatchName

                    If printRequestBatchNameForm.SendToAllStores Then
                        For Each store As Object In cmbStore.Items
                            businessUnitsToSend.Add(storeNumberToBusinessUnit(store.ItemData))
                        Next
                    Else
                        businessUnitsToSend.Add(storeNumberToBusinessUnit(selectedStoreNumber))
                    End If

                    If printRequestBatchNameForm.ApplyNoTagLogic Then
                        applyNoTagLogic = True
                    End If
                Else
                    logger.Info("No batch name chosen.  Cancelling the print request.")
                    Exit Sub
                End If
            End Using

            For Each businessUnit As Integer In businessUnitsToSend
                Dim storeNumber As Integer = storeNumberToBusinessUnit.Single(Function(sn) sn.Value = businessUnit).Key
                Dim validIdentifiers As List(Of String) = pricingPrintSignsBusinessLogic.GetValidTagReprintIdentifiers(storeNumber, selectedItems.Select(Function(i) i.Identifier).ToList())

                ' flag the selected items collection to indicate validated identifiers
                For Each selectedItemWithValidIdentifier As ItemForAdHocPrint In selectedItems.Where(Function(i) validIdentifiers.Contains(i.Identifier))
                    selectedItemWithValidIdentifier.IdentifierIsValid = True
                Next

                itemsExcluded = selectedItems.Any(Function(i) i.IdentifierIsValid = False)

                If selectedItems.Any(Function(i) i.IdentifierIsValid = True) Then
                    If applyNoTagLogic Then
                        Cursor = Cursors.WaitCursor

                        Dim distinctSubteams As List(Of Integer) = selectedItems.Where(Function(i) i.IdentifierIsValid = True).Select(Function(i) i.SubteamNumber).Distinct().ToList()

                        For Each subteamNumber As Integer In distinctSubteams
                            Dim selectedItemsBySubteam As List(Of ItemForAdHocPrint) = selectedItems.Where(Function(i) i.SubteamNumber = subteamNumber).ToList()
                            Dim subteamName As String = selectedItemsBySubteam.First().SubteamName.ToString()

                            Dim excludedNoTagItemKeys As List(Of Integer) = pricingPrintSignsBusinessLogic.GetNoTagLogicExcludedItems(
                                selectedItemsBySubteam.Select(Function(i) New ItemKeyIdentifierModel() With {.ItemKey = i.ItemKey, .Identifier = i.Identifier}).ToList(),
                                subteamNumber,
                                subteamName,
                                storeNumber)

                            itemsExcluded = excludedNoTagItemKeys.Any()

                            ' flag the selected items collection to indicate any excluded by NoTag logic
                            For Each selectedItemExcludedByNoTag As ItemForAdHocPrint In selectedItems.Where(Function(i) excludedNoTagItemKeys.Contains(i.ItemKey))
                                selectedItemExcludedByNoTag.ExcludedByNoTagLogic = True
                            Next
                        Next

                        Cursor = Cursors.Default

                        If Not selectedItems.Any(Function(i) i.ExcludedByNoTagLogic = False And i.IdentifierIsValid = True) Then
                            logger.Info(String.Format("All items were excluded from the entire print request by no-tag logic for store {0}.", storeNumber))
                            Continue For
                        End If
                    End If

                    ' gather the identifers for valid, non-excluded items, preserving the user's submitted ordering
                    Dim identifiersToReprint As List(Of String) = selectedItems _
                        .Where(Function(i) i.IdentifierIsValid = True And i.ExcludedByNoTagLogic = False) _
                        .OrderBy(Function(i) i.RequestedOrder) _
                        .Select(Function(i) i.Identifier).ToList()

                    Cursor = Cursors.WaitCursor

                    Try
                        pricingPrintSignsBusinessLogic.SendTagReprintPrintBatchRequests(businessUnit, printRequestBatchName, identifiersToReprint)
                        logger.Info(String.Format("Successfully sent ad-hoc print batch request to the SLAW API for store number {0}.", storeNumber))
                    Catch ex As Exception
                        Dim slawJsonResponseText As String = Nothing
                        Dim apiWebException = TryCast(ex, WebException)

                        Dim response As HttpWebResponse = TryCast(apiWebException.Response, HttpWebResponse)
                        If response IsNot Nothing Then
                            Using responseReader As StreamReader = New StreamReader(response.GetResponseStream())
                                slawJsonResponseText = responseReader.ReadToEnd()
                            End Using
                            logger.Info(String.Format("Error in sending sign reprint, print batch to SLAW API: Error: {0}. Slaw Response: {1}",
                                   ex.Message, slawJsonResponseText))
                        End If
                        logger.Info(String.Format("Error in sending ad-hoc print batch request to the SLAW API for store number {0}: {1}", storeNumber, ex.ToString()))
                        MessageBox.Show(String.Format("An error occurred while transmitting the print request to SLAW for store number {0}: {1}", storeNumber, ex.Message),
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Finally
                        Cursor = Cursors.Default
                    End Try
                Else
                    logger.Info(String.Format("No valid items were found in the print request for store {0}.", storeNumber))
                End If
            Next
        Else
            logger.Info("SLAW is not enabled for this region.  Sending print request to legacy tag batch processor.")

            Try
                Cursor = Cursors.WaitCursor

                Using printSignsForm As New frmPricingPrintSignInfo
                    printSignsForm.SetReprintSignInfo(selectedItems.Select(Function(i) CInt(i.ItemKey)).ToArray(), "|", VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
                    printSignsForm.ShowDialog()

                    If printSignsForm.DialogResult = DialogResult.Cancel Then
                        Cursor = Cursors.Default
                        Exit Sub
                    End If
                End Using

                logger.Info(String.Format("Successfully sent ad-hoc print batch request to the legacy tag processor for store number {0}.", selectedStoreNumber))
            Catch ex As Exception
                logger.Info(String.Format("Error in sending ad-hoc print batch request to the legacy tag processor for store number {0}: {1}", selectedStoreNumber, ex.ToString()))
                MessageBox.Show(String.Format("An error occurred while transmitting the print request to the legacy tag batch processor for store number {0}: {1}", selectedStoreNumber, ex.Message),
                                Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            Finally
                Cursor = Cursors.Default
            End Try
        End If

        If itemsExcluded Then
            MessageBox.Show("All print requests have been transmitted.  Some items were excluded due to factors such as authorization, validation, deleted status, or no-tag logic.",
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("All print requests have been transmitted.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        logger.Debug("cmdPrintLabel_Click Exit")
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        logger.Debug("cmdSearch_Click Start")

        RunSearch()

        logger.Debug("cmdSearch_Click End")
    End Sub

    Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
        logger.Debug("txtField_Enter Start")

        Dim index As Short = txtField.GetIndex(eventSender)
        HighlightText(txtField(index))

        logger.Debug("txtField_Enter End")
    End Sub

    Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
        logger.Debug("txtField_KeyPress Start")

        Dim keyAscii As Short = Asc(eventArgs.KeyChar)
        Dim index As Short = txtField.GetIndex(eventSender)

        keyAscii = ValidateKeyPressEvent(keyAscii, txtField(index).Tag, txtField(index), 0, 0, 0)

        eventArgs.KeyChar = Chr(keyAscii)

        If keyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("txtField_KeyPress End")
    End Sub

    Public Sub RunSearch()
        logger.Debug("RunSearch Enter")

        Dim signDescription As String
        Dim identifiers As String
        Dim storeNumber As Integer
        Dim categoryId As String
        Dim brandId As String
        Dim subteamNumber As String

        storeNumber = VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)

        If Trim(txtField(0).Text) <> String.Empty Then
            signDescription = Trim(txtField(0).Text)
        Else
            signDescription = String.Empty
        End If

        If searchIdentifiers.Count > 0 Then
            identifiers = String.Join(",", searchIdentifiers)
        Else
            If Trim(txtField(1).Text) <> String.Empty Then
                identifiers = Trim(txtField(1).Text)
            Else
                identifiers = String.Empty
            End If
        End If

        If cmbSubTeam.SelectedIndex > -1 Then
            subteamNumber = CStr(VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        Else
            subteamNumber = String.Empty
        End If

        If cmbBrand.SelectedIndex > -1 Then
            brandId = CStr(VB6.GetItemData(cmbBrand, cmbBrand.SelectedIndex))
        Else
            brandId = String.Empty
        End If

        If cmbCategory.SelectedIndex > -1 Then
            categoryId = CStr(VB6.GetItemData(cmbCategory, cmbCategory.SelectedIndex))
        Else
            categoryId = String.Empty
        End If

        Cursor = Cursors.WaitCursor

        LoadDataTable(storeNumber, subteamNumber, categoryId, signDescription, identifiers, brandId)

        Cursor = Cursors.Default

        lblTotal.Text = "Total Items: " & ugrdSearchResults.Rows.Count.ToString()

        logger.Debug("RunSearch Exit")
    End Sub

    Private Sub cmbCategory_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbCategory.KeyPress
        logger.Debug("cmbCategory_KeyPress Enter")

        Dim keyAscii As Short = Asc(eventArgs.KeyChar)

        If keyAscii = 8 And cmbSubTeam.BackColor <> COLOR_INACTIVE Then Me.cmbCategory.SelectedIndex = -1

        eventArgs.KeyChar = Chr(keyAscii)

        If keyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("cmbCategory_KeyPress Exit")
    End Sub

    Private Sub cmbSubTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged
        logger.Debug("cmbSubTeam_SelectedIndexChanged Enter")

        If isInitializing Then Exit Sub

        If cmbSubTeam.SelectedIndex = -1 Then
            cmbCategory.Items.Clear()
        Else
            LoadCategory(cmbCategory, VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

        logger.Debug("cmbSubTeam_SelectedIndexChanged Exit")
    End Sub

    Private Sub cmbSubTeam_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.TextChanged
        logger.Debug("cmbSubTeam_TextChanged Enter")

        If cmbSubTeam.Text = String.Empty Then cmbSubTeam.SelectedIndex = -1

        logger.Debug("cmbSubTeam_TextChanged Exit")
    End Sub

    Private Sub cmbSubTeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
        logger.Debug("cmbSubTeam_KeyPress Enter")

        If Asc(e.KeyChar) = 8 And cmbSubTeam.BackColor <> COLOR_INACTIVE Then cmbSubTeam.SelectedIndex = -1

        logger.Debug("cmbSubTeam_KeyPress Exit")
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        If chkSelectAll.Checked Then
            ugrdSearchResults.Selected.Rows.AddRange(ugrdSearchResults.Rows.All)
        Else
            ugrdSearchResults.Selected.Rows.Clear()
        End If
    End Sub

    Private Sub ButtonIdentifiers_Click(sender As Object, e As EventArgs) Handles ButtonIdentifiers.Click
        multipleIdentifiersInputForm.ShowDialog()

        If multipleIdentifiersInputForm.TextBoxInput.Text.Length > 0 Then
            searchIdentifiers = multipleIdentifiersInputForm.TextBoxInput.Lines().Where(Function(i) Not String.IsNullOrWhiteSpace(i)).ToList()

            _txtField_1.Text = "<multiple selected>"
            _txtField_1.Enabled = False
        Else
            _txtField_1.Text = String.Empty
            _txtField_1.Enabled = True

            searchIdentifiers.Clear()
        End If
    End Sub

    Private Sub frmPricingPrintSigns_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        multipleIdentifiersInputForm.Dispose()
    End Sub

    Private Sub ugrdSearchResults_AfterRowActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdSearchResults.AfterRowActivate
        chkSelectAll.Checked = False
    End Sub

    Private Sub AddOrderingColumnBasedOnProvidedIdentifiers(ByVal identifiers As String, ByRef resultsTable As DataTable)
        ' PBI 25130: if the user entered a list of identifiers, provide results in the same order that they were presented 

        ' add a column to the data table (results) to keep track of the order of the identifiers provided by the user
        resultsTable.Columns.Add("RequestedOrder", GetType(Integer))

        If identifiers <> String.Empty Then
            ' split the user-provided identifiers (a comma-delimited string) into an array
            Dim identifiersArray As String() = identifiers.Split(New Char() {","c})
            '.Select(Function(s) s.Trim())

            If (identifiersArray.Length > 0) Then
                ' build a dictionary to associate an order value with each identifier
                Dim identifiersDictionary As New Dictionary(Of String, Integer)
                For i As Integer = 0 To identifiersArray.Length - 1
                    identifiersDictionary.Add(identifiersArray(i).Trim(), i + 1)
                Next

                Dim sortColumnCellValue As Integer = 0
                ' iterate the results data and populate values for the ordering column
                For Each dataRow As DataRow In resultsTable.Rows
                    ' assign the ordering cell value by looking up the identifier cell value in the dictionary
                    If identifiersDictionary.TryGetValue(dataRow("Identifier"), sortColumnCellValue) Then
                        dataRow("RequestedOrder") = sortColumnCellValue
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub ugrdSearchResults_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugrdSearchResults.InitializeLayout
        ' tell the grid control to include sorting by the hidden column which tracks the order of submitted identifiers
        e.Layout.Bands(0).SortedColumns.Add(e.Layout.Bands(0).Columns("RequestedOrder"), False, False)
    End Sub
End Class
