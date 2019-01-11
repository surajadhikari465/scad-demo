Imports System.IO
Imports System.Linq
Imports System.Net
Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports WholeFoods.Utility.DataAccess

Friend Class frmPricingPrintSigns
    Inherits System.Windows.Forms.Form

  Private factory As DataFactory
  Private isInitializing As Boolean
  Private multipleIdentifiersForm As MultipleValueInput = New MultipleValueInput()
  Private searchIdentifiers As List(Of String) = New List(Of String)
  Private pricingPrintSignsBusinessLogic As PricingPrintSignsBO = New PricingPrintSignsBO()
  Private tableStore, tableCategoryTeam As DataTable
  Private storeID As Integer
  Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

  Private Sub InitilizeData()
    lvStore.Items.Clear()
    cmbSubTeam.DataSource = Nothing

    Try
      factory = New DataFactory(DataFactory.ItemCatalog)
      tableStore = factory.GetStoredProcedureDataTable("GetRetailStores")

      Dim spParam As DBParam = New DBParam With {
        .Name = "bRetail",
        .Value = False,
        .Type = DBParamType.Bit
      }
      tableCategoryTeam = factory.GetStoredProcedureDataTable("GetCategoriesAndSubTeams", New ArrayList From {spParam})

      Dim ar As ListViewItem() = (From x In tableStore.Rows Select New ListViewItem(x!Store_Name.ToString().Trim()) With {.Tag = x}).OrderBy(Function(x) x.Text).ToArray()
      lvStore.Items.AddRange(ar)

      cmbSubTeam.DataSource = tableCategoryTeam.DefaultView.ToTable(True, "SubTeam_Name", "SubTeam_No")
      cmbSubTeam.DisplayMember = "SubTeam_Name"
      cmbSubTeam.ValueMember = "SubTeam_No"
      cmbSubTeam.SelectedIndex = -1

      cmbBrand.DataSource = factory.GetStoredProcedureDataTable("GetBrandAndID")
      cmbBrand.DisplayMember = "Brand_Name"
      cmbBrand.ValueMember = "Brand_ID"
      cmbBrand.SelectedIndex = -1

      chkApplyNoTagLogic.Enabled = (glStore_Limit = 0)
      ugrdSearchResults.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.Default.False 'Prevent data change in the grid
    Catch ex As Exception
      Throw New Exception(String.Format("{0}{1}Please contact your System Administrator.", ex.Message, vbCrLf))
    End Try
  End Sub

  Private Sub frmPricingPrintSigns_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
    isInitializing = True

    Try
      InitilizeData()
      isInitializing = False
    Catch ex As Exception
      MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Me.Close()
    End Try
  End Sub

  Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
    Me.Close()
  End Sub

  Private Sub cmdItemEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemEdit.Click
    If ugrdSearchResults.Selected.Rows.Count > 0 Then
      glItemID = ugrdSearchResults.Selected.Rows(0).Cells("Item_Key").Value
      frmItem.ShowDialog()
      frmItem.Close()
    End If
  End Sub

  Private Sub cmdPrint_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPrint.Click
    If storeID < 1 OrElse ugrdSearchResults.Selected.Rows.Count = 0 OrElse lvStore.SelectedItems.Count = 0 Then Exit Sub

    Dim selectedStoreNumber As Integer = storeID
    Dim businessUnitsToSend As List(Of Integer) = New List(Of Integer)
    Dim selectedSubteamName As String = If(ugrdSearchResults.Selected.Rows.Count > 1, "MultipleSubteams", ugrdSearchResults.Selected.Rows.Item(0).Cells("SubTeam_Name").Value)
    Dim selectedItems As List(Of ItemForAdHocPrint) = New List(Of ItemForAdHocPrint)
    Dim itemsExcluded As Boolean
    Dim batchName As String = txtBatchName.Text.Trim()

    If (String.IsNullOrEmpty(batchName)) Then
      MessageBox.Show("Please specify the batch name.", "System Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
      txtBatchName.Focus()
      Exit Sub
    End If

    For Each row As UltraGridRow In ugrdSearchResults.Selected.Rows
      selectedItems.Add(New ItemForAdHocPrint() With {
                        .Identifier = row.Cells("Identifier").Value,
                        .ItemKey = row.Cells("Item_Key").Value,
                        .SubteamNumber = row.Cells("SubTeam_No").Value,
                        .SubteamName = row.Cells("SubTeam_Name").Value,
                        .RequestedOrder = If(row.Cells("RequestedOrder").Value Is DBNull.Value, 0, row.Cells("RequestedOrder").Value),
                        .IdentifierIsValid = False,
                        .ExcludedByNoTagLogic = False})
    Next


    If SlawPrintBatchConfigurationDAO.SlawPrintRequestsEnabledForRegion() Then
      logger.Info("SLAW is enabled for this region.  Sending print request to SLAW API.")

      For Each item As ListViewItem In lvStore.SelectedItems
        businessUnitsToSend.Add(CInt((CType(item.Tag, DataRow)!BusinessUnit_ID)))
      Next

      If (MessageBox.Show(String.Format("{0}{1}{2}Would you like to proceed  ({3})?", lblInfo.Text, vbCrLf, vbCrLf, If(lvStore.SelectedItems.Count = 1, "Single store", "Multiple Stores")), "User Confirmation Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.OK) Then Exit Sub

      For Each businessUnit As Integer In businessUnitsToSend
        Dim storeNumber As Integer = CInt(tableStore.Select(String.Format("BusinessUnit_ID = {0}", businessUnit)).First()!Store_No)
        Dim validIdentifiers As List(Of String) = pricingPrintSignsBusinessLogic.GetValidTagReprintIdentifiers(storeNumber, selectedItems.Select(Function(i) i.Identifier).ToList())

        ' flag the selected items collection to indicate validated identifiers
        For Each selectedItemWithValidIdentifier As ItemForAdHocPrint In selectedItems.Where(Function(i) validIdentifiers.Contains(i.Identifier))
          selectedItemWithValidIdentifier.IdentifierIsValid = True
        Next

        itemsExcluded = selectedItems.Any(Function(i) i.IdentifierIsValid = False)

        If selectedItems.Any(Function(i) i.IdentifierIsValid = True) Then
          If chkApplyNoTagLogic.Checked Then
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
            pricingPrintSignsBusinessLogic.SendTagReprintPrintBatchRequests(businessUnit, batchName, identifiersToReprint)
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
          printSignsForm.SetReprintSignInfo(selectedItems.Select(Function(i) CInt(i.ItemKey)).ToArray(), "|", selectedStoreNumber)
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
    Try
      If (lvStore.FocusedItem Is Nothing) Then
        MessageBox.Show("Select the store from the list and try agaian.", "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return
      End If

      Cursor = Cursors.WaitCursor
      cmdSearch.Enabled = False
      ugrdSearchResults.Text = "Search Results"
      ugrdSearchResults.DataSource = Nothing
      storeID = CInt((CType(lvStore.FocusedItem.Tag, DataRow)!Store_No))
      Refresh()
      Application.DoEvents()

      Dim identifiers As String = If(searchIdentifiers.Count > 0, String.Join(",", searchIdentifiers), txtField(1).Text.Trim())
      Dim result As DataTable = PriceBatchSearchDAO.PricingPrintSignsSearch(
      storeNumber:=storeID,
      subteamNumber:=If(cmbSubTeam.SelectedIndex < 0, String.Empty, cmbSubTeam.SelectedValue),
      categoryId:=If(cmbCategory.SelectedIndex < 0, String.Empty, cmbCategory.SelectedValue),
      signDescription:=txtField(0).Text.Trim(),
      identifiers:=identifiers,
      brandId:=If(cmbBrand.SelectedIndex < 0, String.Empty, cmbBrand.SelectedValue))

      AddOrderingColumnBasedOnProvidedIdentifiers(identifiers, result)
      If result.Rows.Count() > 0 Then ugrdSearchResults.DataSource = result
      ugrdSearchResults.Text = String.Format("Search Results for {0}.   Total Items: {1}", lvStore.FocusedItem.Text, result.Rows.Count().ToString())
    Catch ex As Exception
      storeID = -1
      MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Finally
      Cursor = Cursors.Default
      cmdSearch.Enabled = True
    End Try
  End Sub

  Private Sub txtBox_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub cmbSubTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged
    If isInitializing Then Exit Sub

    cmbCategory.DataSource = If((cmbSubTeam.SelectedIndex < 0 OrElse tableCategoryTeam Is Nothing), Nothing, New DataView(tableCategoryTeam) With {
        .RowFilter = String.Format("SubTeam_No = {0}", cmbSubTeam.SelectedValue),
        .Sort = "Category_Name"})
    cmbCategory.DisplayMember = "Category_Name"
    cmbCategory.ValueMember = "Category_ID"
    cmbCategory.SelectedIndex = -1
  End Sub

  Private Sub cmbSubTeam_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.TextChanged
    If cmbSubTeam.Text = String.Empty Then cmbSubTeam.SelectedIndex = -1
  End Sub

  Private Sub ButtonIdentifiers_Click(sender As Object, e As EventArgs) Handles btnIdentifiers.Click
    multipleIdentifiersForm.ShowDialog()

    If multipleIdentifiersForm.TextBoxInput.Text.Length > 0 Then
      searchIdentifiers = multipleIdentifiersForm.TextBoxInput.Lines().Where(Function(i) Not String.IsNullOrWhiteSpace(i)).ToList()

      _txtField_1.Text = "<multiple selected>"
      _txtField_1.Enabled = False
    Else
      _txtField_1.Text = String.Empty
      _txtField_1.Enabled = True

      searchIdentifiers.Clear()
    End If
  End Sub

  Private Sub frmPricingPrintSigns_Closed(sender As Object, e As EventArgs) Handles Me.Closed
    multipleIdentifiersForm.Dispose()
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

  Private Sub ugrdSearchResults_KeyDown(sender As Object, e As KeyEventArgs) Handles ugrdSearchResults.KeyDown
    If (e.Control AndAlso e.KeyCode = Keys.A) Then
      ugrdSearchResults.Selected.Rows.AddRange(ugrdSearchResults.Rows.All)
    End If
  End Sub

  Private Sub ugrdSearchResults_InitializeLayout(ByVal sender As Object, ByVal e As InitializeLayoutEventArgs) Handles ugrdSearchResults.InitializeLayout
    ' tell the grid control to include sorting by the hidden column which tracks the order of submitted identifiers
    e.Layout.Bands(0).SortedColumns.Add(e.Layout.Bands(0).Columns("RequestedOrder"), False, False)
  End Sub

  Private Sub ugrdSearchResults_AfterSortChange(ByVal sender As Object, ByVal e As BandEventArgs) Handles ugrdSearchResults.AfterSortChange
    'if the user has manually sorted the columns, disregard the initial sort order column values
    For Each row As UltraGridRow In ugrdSearchResults.Rows
      row.Cells("RequestedOrder").Value = 0
    Next
  End Sub

  Private Sub cmb_KeyUp(sender As Object, e As KeyEventArgs) Handles cmbBrand.KeyUp, cmbSubTeam.KeyUp, cmbCategory.KeyUp
    Select Case e.KeyCode
      Case Keys.Back : CType(sender, ComboBox).SelectedIndex = -1
    End Select
  End Sub

  Private Sub lvStore_KeyDown(sender As Object, e As KeyEventArgs) Handles lvStore.KeyDown
    If (e.Control AndAlso e.KeyCode = Keys.A) Then
      For Each itm As ListViewItem In lvStore.Items
        itm.Selected = True
      Next
    End If
  End Sub

  Private Sub lvStore_DoubleClick(sender As Object, e As EventArgs) Handles lvStore.DoubleClick
    If (lvStore.FocusedItem IsNot Nothing) Then cmdSearch_Click(Nothing, Nothing)
  End Sub

  Private Sub lvStore_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles lvStore.ColumnClick
    lvStore.Sorting = If(lvStore.Sorting = SortOrder.Descending, SortOrder.Ascending, SortOrder.Descending)
    lvStore.Sort()
  End Sub
End Class