Imports Infragistics.Win.UltraWinGrid
Imports System.Text
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports System.Linq

Public Class CancelSalesMultipleItems

    Private maximumNumberOfIdentifiers As Integer
    Private IsInitializing As Boolean
    Private mbFilling As Boolean

    Public Sub New()
        InitializeComponent()
        maximumNumberOfIdentifiers = 500
    End Sub

    Private Sub CancelSalesMultipleItems_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToParent()
        SetupStoreSelectionGrid()
        RadioButton_Manual.Checked = True
        GroupBox_Error.Visible = False
        'load zone drop down
        LoadZone(cmbZones)
        SetCombos()
        ItemIdentifiersTextBox.Clear()
    End Sub

    Private Sub SetupStoreSelectionGrid()
        '-- Fill out the store list
        Dim mdtStores As DataTable = WholeFoods.IRMA.ItemHosting.DataAccess.StoreDAO.GetRetailStoreList
        ugrdStoreList.DataSource = mdtStores

        DisableRowsWithGPMStore(mdtStores.Rows.Count)
    End Sub

    Private Sub DisableRowsWithGPMStore(ByVal rowcount As Integer)
        For currentRowNumber As Integer = 0 To rowcount - 1
            If ugrdStoreList.Rows(currentRowNumber).Cells("IsGPMStore").Value Then
                ugrdStoreList.Rows(currentRowNumber).Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled
            End If
        Next

    End Sub
    Private Sub SetCombos()
        mbFilling = True

        'Zones.
        If Me.RadioButton_Zone.Checked = True Then
            cmbZones.Enabled = True
            cmbZones.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbZones.SelectedIndex = -1
            cmbZones.Enabled = False
            cmbZones.BackColor = System.Drawing.SystemColors.Control
        End If

        mbFilling = False
    End Sub

    Private Sub ApplyChanges()
        Dim identifiers As List(Of String)
        Dim errorMessage As String = String.Empty

        identifiers = getEnteredIdentifiers()
        errorMessage = ValidateDateAndIdentifiers(identifiers)

        If (errorMessage.Length > 0) Then
            MessageBox.Show(errorMessage)
        Else
            SaveData(identifiers)
        End If

    End Sub

    Private Sub SaveData(identifiers As List(Of String))
        Dim cancelSalesMultipleItemsDAO As New CancelSalesMultipleItemsDAO
        Dim storeItemInfoModelList As List(Of StoreItemInfoModel) = New List(Of StoreItemInfoModel)()
        Dim row As UltraGridRow
        Dim storeItemInfoDataTable As DataTable = New DataTable()
        Dim identifier As String

        storeItemInfoDataTable.Columns.Add("StoreNo", GetType(Integer))
        storeItemInfoDataTable.Columns.Add("ItemIdentifier", GetType(String))

        ' create a table with all store no and identifiers combination
        For Each identifier In identifiers
            If (identifier.Length > 13) Then
                identifier = identifier.Substring(0, 13)
            End If

            For Each row In Me.ugrdStoreList.Selected.Rows
                storeItemInfoDataTable.Rows.Add(row.Cells("Store_No").Value.ToString(), identifier)
            Next
        Next

        Dim validatedStoreItemInfoDataSet As DataSet = cancelSalesMultipleItemsDAO.ValidateCancelAllSalesData(storeItemInfoDataTable)

        If (validatedStoreItemInfoDataSet IsNot Nothing _
                      AndAlso validatedStoreItemInfoDataSet.Tables.Count >= 0 _
                     AndAlso validatedStoreItemInfoDataSet.Tables(0).Rows.Count > 0) Then
            'convert table to model
            storeItemInfoModelList = ConvertDataTableToList(validatedStoreItemInfoDataSet.Tables(0))
        Else
            MessageBox.Show("All the entered Item Identifiers are invalid.")
            Return
        End If

        Dim result As DialogResult
        result = MessageBox.Show(ResourcesItemHosting.GetString("msg_confirm_CancelAllSalesForAllItems"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = Windows.Forms.DialogResult.Yes Then
            'save data. Call cancel all sales to save data
            CancelSales(storeItemInfoModelList.Where(Function(si) String.IsNullOrEmpty(si.ErrorDetails)).ToList())
        Else
            Return
        End If

        If (storeItemInfoModelList.Where(Function(si) Not String.IsNullOrEmpty(si.ErrorDetails)).Count > 0) Then
            'all rows without error saved, rows with error will be displayed in error grid
            DisplayErrors(storeItemInfoModelList.Where(Function(si) Not String.IsNullOrEmpty(si.ErrorDetails)).ToList())
            MessageBox.Show("Sales have been Cancelled. Please take a look at Error section below for any errors.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            '' all rows saved. no errors
            GroupBox_Error.Visible = False
            MessageBox.Show("All Sales have been Cancelled.")
        End If
    End Sub

    Private Function ValidateDateAndIdentifiers(identifiers As List(Of String)) As String
        Dim errorMessage As String = String.Empty

        If CType(Me.dtpStartDate.Value, Date) < System.DateTime.Today Then
            errorMessage = "Effective Date must be greater than or equal to Today."
        ElseIf Me.ugrdStoreList.Selected.Rows.Count = 0 Then
            errorMessage = "No Store selected."
        ElseIf Not identifiers.Any() Then
            errorMessage = "No Item Identifiers entered."
        ElseIf identifiers.Count > maximumNumberOfIdentifiers Then
            errorMessage = String.Format("More than the maximum allowed {0} Item Identifiers entered.", maximumNumberOfIdentifiers)
        End If

        Return errorMessage
    End Function

    Private Sub DisplayErrors(storeItemInfoModelList As List(Of StoreItemInfoModel))

        ErrorsDataGridView.DataSource = Nothing
        ErrorsDataGridView.AutoGenerateColumns = False
        Me.ErrorsDataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText

        If (ErrorsDataGridView.ColumnCount = 0) Then
            ErrorsDataGridView.Columns.Add(New DataGridViewTextBoxColumn With {
                                                  .DataPropertyName = "StoreName",
                                                  .HeaderText = "Store",
                                                  .ReadOnly = True,
                                                  .Resizable = True,
                                                  .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                                                })
            ErrorsDataGridView.Columns.Add(New DataGridViewTextBoxColumn With {
                                                   .DataPropertyName = "Identifier",
                                                   .HeaderText = "Identifier",
                                                   .ReadOnly = True,
                                                   .Resizable = True,
                                                   .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                                               })
            ErrorsDataGridView.Columns.Add(New DataGridViewTextBoxColumn With {
                                                   .DataPropertyName = "ErrorDetails",
                                                   .HeaderText = "Error Details",
                                                   .ReadOnly = True,
                                                   .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                                               })

            ErrorsDataGridView.DataSource = storeItemInfoModelList _
                                            .Where(Function(si) Not String.IsNullOrEmpty(si.ErrorDetails)) _
                                            .OrderBy(Function(o) o.ItemKey)
        End If

        ErrorsDataGridView.DataSource = storeItemInfoModelList
        GroupBox_Error.Visible = True
    End Sub

    Private Sub CancelSales(storeItemInfoModelList As List(Of StoreItemInfoModel))
        Dim adjustSaleDAO As New AdjustSaleDataDAO
        Dim effectiveDate As Date
        Dim adjustSaleData As AdjustSaleDataBO
        Dim currentItemKey As String = String.Empty
        Dim previousItemKey As String = String.Empty
        Dim firstTime As Boolean = True
        Dim StoreListSeparator As Char = "|"c
        Dim storeList As New StringBuilder

        Try
            effectiveDate = CType(Me.dtpStartDate.Value, Date)

            For Each StoreItemInfoModel As StoreItemInfoModel In storeItemInfoModelList _
                                                                 .Where(Function(si) String.IsNullOrEmpty(si.ErrorDetails)) _
                                                                 .OrderBy(Function(o) o.ItemKey)

                currentItemKey = StoreItemInfoModel.ItemKey

                If (previousItemKey <> currentItemKey) Then
                    If (Not firstTime) Then
                        Try
                            adjustSaleData.StoreList = storeList.ToString.Substring(0, storeList.ToString.Length - 1)
                            adjustSaleDAO.CancelAllSales(adjustSaleData)
                            UnlockItem(adjustSaleData.ItemKey)
                            storeList.Clear()
                        Catch ex As Exception
                            StoreItemInfoModel.ErrorDetails = String.Concat("Item:", StoreItemInfoModel.Identifier, " has been locked by other user.")
                            Continue For
                        End Try
                    Else
                        firstTime = False
                    End If

                    adjustSaleData = New AdjustSaleDataBO
                    adjustSaleData.ItemKey = currentItemKey
                    adjustSaleData.UserID = giUserID
                    adjustSaleData.StartDate = effectiveDate
                    adjustSaleData.StoreListSeparator = StoreListSeparator
                    adjustSaleData.UserIDDate = LockItem(currentItemKey)
                End If

                storeList.Append(StoreItemInfoModel.Store)
                storeList.Append(StoreListSeparator)
                previousItemKey = StoreItemInfoModel.ItemKey
            Next
            ' call cancel all sales for last item
            If (storeList.Length > 0) Then
                adjustSaleData.StoreList = storeList.ToString.Substring(0, storeList.ToString.Length - 1)
                adjustSaleDAO.CancelAllSales(adjustSaleData)
                UnlockItem(adjustSaleData.ItemKey)
            End If

        Catch ex As Exception
            MessageBox.Show(ResourcesCommon.GetString("msg_dbError"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Function ConvertDataTableToList(dataTable As DataTable) As List(Of StoreItemInfoModel)
        Return dataTable.AsEnumerable().Select(Function(dat) New StoreItemInfoModel(dat.Item(0), dat.Item(1),
                                                                                    dat.Item(2), dat.Item(3),
                                                                                    dat.Item(4)
                                                                                    )).ToList()
    End Function

    Private Function LockItem(ByVal itemKey As String) As String
        Dim itemDAO As New ItemDAO
        Dim dsItemLock As DataSet = itemDAO.LockItem(itemKey, giUserID)

        Return dsItemLock.Tables(0).Rows(0)("User_ID_Date").ToString()
    End Function

    Private Function UnlockItem(ByVal itemKey As String) As Boolean
        Dim itemDAO As New ItemDAO

        Return itemDAO.UnlockItem(itemKey)
    End Function

    ''' <summary>
    ''' get identifiers entered by user
    ''' </summary>
    ''' <returns></returns>
    Private Function getEnteredIdentifiers() As List(Of String)
        Dim identifiers As List(Of String) = ItemIdentifiersTextBox _
                                            .Lines() _
                                            .Where(Function(i) Not String.IsNullOrWhiteSpace(i)) _
                                            .Select(Function(i) i.Trim) _
                                            .ToList()

        Return identifiers
    End Function
#Region "Event Handlers"

    ''' <summary>
    ''' exit form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        Me.Close()
    End Sub

    ''' <summary>
    '''Apply Changes and save data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_ApplyChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ApplyChanges.Click
        'save changes
        Try
            ApplyChanges()
        Catch ex As Exception
            MessageBox.Show(ResourcesCommon.GetString("msg_unexpectedError"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    ''' <summary>
    ''' RadioButton_Manual_CheckedChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RadioButton_Manual_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_Manual.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub

        If CType(sender, RadioButton).Checked Then
            Call SetCombos()
            mbFilling = True
            ugrdStoreList.Selected.Rows.Clear()
            mbFilling = False
        End If

    End Sub

    ''' <summary>
    ''' RadioButton_All_CheckedChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RadioButton_All_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_All.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub

        If CType(sender, RadioButton).Checked Then
            Call SetCombos()
            mbFilling = True
            ugrdStoreList.Selected.Rows.Clear()
            StoreListGridSelectAll(ugrdStoreList, True)
            mbFilling = False
        End If

    End Sub

    ''' <summary>
    ''' RadioButton_Zone_CheckedChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RadioButton_Zone_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_Zone.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub

        If CType(sender, RadioButton).Checked Then
            Call SetCombos()
            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()
            '-- By Zone
            If cmbZones.SelectedIndex > -1 Then Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
            mbFilling = False
        End If

    End Sub

    ''' <summary>
    ''' cmbZones_SelectedIndexChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmbZones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbZones.SelectedIndexChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Me.RadioButton_Zone_CheckedChanged(Me.RadioButton_Zone, New System.EventArgs)
    End Sub

#End Region
End Class