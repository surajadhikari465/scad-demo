Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports System.Text
Imports WholeFoods.IRMA.InvoiceControlGroup.BusinessLogic
Imports WholeFoods.IRMA.InvoiceControlGroup.DataAccess
Imports WholeFoods.IRMA.Ordering.BusinessLogic

Public Class InvoiceControlGroup
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ' Images displayed on the invoice grid to show the status of the invoice at the time the control group was closed
    Private Shared bmpError As New Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("redCircleWhiteX.bmp"))
    Private Shared bmpWarning As New Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("yellowTriangleBlackExclamation.bmp"))

    ' ID for the current control group being displayed
    Private _currentControlGroupID As Integer = -1

    ' Business object for the current control group being displayed
    Private currentControlGroup As ControlGroupBO = Nothing

    ' Flags to track changes to the data
    Private dataChanges As Boolean = False
    Private dataLoading As Boolean = False

    ' Flags to keep track of notification prompts that have been displayed to the user
    Private notifyInvoices As Boolean = False
    Private notifyAmount As Boolean = False
    Private notifyBalanced As Boolean = False

    ' Search form
    Dim WithEvents invoiceSearchForm As InvoiceControlGroupSearch

#Region "Property Accessors"
    Public Property CurrentControlGroupID() As Integer
        Get
            Return _currentControlGroupID
        End Get
        Set(ByVal value As Integer)
            _currentControlGroupID = value
        End Set
    End Property
#End Region

#Region "Child Form Events"
    ''' <summary>
    ''' Update the current control group ID on this form when the user selects a control group entry on the
    ''' search form.
    ''' </summary>
    ''' <param name="controlGroupID"></param>
    ''' <remarks></remarks>
    Private Sub InvoiceSearchForm_ControlGroupSelected(ByVal controlGroupID As Integer) Handles invoiceSearchForm.ControlGroupSelected
        logger.Debug("InvoiceSearchForm_ControlGroupSelected entry: controlGroupID=" + controlGroupID.ToString())
        _currentControlGroupID = controlGroupID
        ' Refresh the UI screen with the data from the selected control group
        PopulateControlGroupData(True)
        logger.Debug("InvoiceSearchForm_ControlGroupSelected exit")
    End Sub
#End Region

#Region "Helper methods for data population and form display"
    ''' <summary>
    ''' Refresh all of the values on the UI screen with the data for the currently selected control group.
    ''' This refreshes the control group header data along with the grid of invoices associated with the
    ''' control group.
    ''' </summary>
    ''' <param name="refreshBO"></param>
    ''' <remarks></remarks>
    Private Sub PopulateControlGroupData(Optional ByVal refreshBO As Boolean = True)
        logger.Debug("PopulateControlGroupData entry")

        ' Set the data loading flag to true so that the data change events are not processed during initialization
        dataLoading = True

        ' Retrieve the control group data from the database
        If _currentControlGroupID <> -1 AndAlso refreshBO Then
            logger.Info("Retrieving the control group data, including the associated purchase orders: ControlGroupID=" + _currentControlGroupID.ToString())
            currentControlGroup = ControlGroupDAO.GetControlGroupDetails(_currentControlGroupID, True)

            ' Reset the notification prompts because the data has been refreshed
            notifyAmount = False
            notifyInvoices = False
            notifyBalanced = False
        ElseIf _currentControlGroupID = -1 Then
            currentControlGroup = Nothing
        End If

        If currentControlGroup IsNot Nothing Then
            ' Update the control group data
            PopulateControlGroupHeaderData()
            ' Update the invoices
            PopulateDataGrid()
            ' Enable/Disable buttons based on the status of the control group
            If currentControlGroup.Status = ControlGroupStatus.Open Then
                EnableActionButtons()
            Else
                DisableActionButtons()
            End If
        Else
            ' Clear any existing UI values
            ClearControlGroupData()
            ' Disable the action buttons
            DisableActionButtons()
        End If

        ' Default the data changes flag to false
        dataChanges = False
        dataLoading = False

        ' If there was a data refresh, check to see if balance notifications should be shown to the user.
        If refreshBO AndAlso currentControlGroup IsNot Nothing Then
            ' Update the textboxes above the grid that display the running totals and differences
            Me.TextBox_EnteredNumInvoices.Text = currentControlGroup.Orders.Count.ToString()
            ResetNumInvoicesDifference()
            Me.TextBox_EnteredGrossAmt.Text = currentControlGroup.OrdersInvoiceTotal.ToString("C")
            ResetGrossAmtDifferences()
        End If
        logger.Debug("PopulateControlGroupData exit")
    End Sub

    ''' <summary>
    ''' Populate the control group header fields on the UI form with the 
    ''' data for the currently selected control group.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateControlGroupHeaderData()
        logger.Debug("PopulateControlGroupHeaderData entry")
        ' Populate the header data
        Me.TextBox_ControlGroupID.Text = currentControlGroup.ControlGroupId.ToString()
        Select Case currentControlGroup.Status
            Case ControlGroupStatus.Open
                Me.TextBox_ControlGroupStatus.Text = "Open"
            Case ControlGroupStatus.Closed
                Me.TextBox_ControlGroupStatus.Text = "Closed"
        End Select
        If currentControlGroup.ExpectedInvoiceCount <> -1 Then
            Me.UltraNumericEditor_ExpectedNumInvoices.Value = currentControlGroup.ExpectedInvoiceCount
        End If
        If currentControlGroup.ExpectedGrossAmt <> -1 Then
            Me.UltraNumericEditor_ExpectedGrossAmt.Value = currentControlGroup.ExpectedGrossAmt
        End If
        logger.Debug("PopulateControlGroupHeaderData exit")
    End Sub

    ''' <summary>
    ''' Enable the action buttons on the UI form for invoices that are in the OPEN status.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnableActionButtons()
        logger.Debug("EnableActionButtons entry")
        Me.Button_AddInvoice.Enabled = True
        Me.Button_EditInvoice.Enabled = True
        Me.Button_DeleteInvoice.Enabled = True
        Me.Button_CloseControlGroup.Enabled = True
        Me.UltraNumericEditor_ExpectedNumInvoices.ReadOnly = False
        Me.UltraNumericEditor_ExpectedGrossAmt.ReadOnly = False
        logger.Debug("EnableActionButtons exit")
    End Sub

    ''' <summary>
    ''' Disable the actions buttons on the UI form for invoices that are in the CLOSED status or
    ''' when no control group is displayed.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisableActionButtons()
        logger.Debug("DisableActionButtons entry")
        Me.Button_AddInvoice.Enabled = False
        Me.Button_EditInvoice.Enabled = False
        Me.Button_DeleteInvoice.Enabled = False
        Me.Button_CloseControlGroup.Enabled = False
        Me.UltraNumericEditor_ExpectedNumInvoices.ReadOnly = True
        Me.UltraNumericEditor_ExpectedGrossAmt.ReadOnly = True
        logger.Debug("DisableActionButtons exit")
    End Sub

    ''' <summary>
    ''' Clear all of the control group data on the UI form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearControlGroupData()
        logger.Debug("ClearControlGroupData entry")
        ' Clear the textbox values
        Me.TextBox_ControlGroupID.Text = ""
        Me.TextBox_ControlGroupStatus.Text = ""
        Me.UltraNumericEditor_ExpectedNumInvoices.Value = Nothing
        Me.TextBox_EnteredNumInvoices.Text = ""
        Me.TextBox_DiffNumInvoices.Text = ""
        Me.UltraNumericEditor_ExpectedGrossAmt.Value = Nothing
        Me.TextBox_EnteredGrossAmt.Text = ""
        Me.TextBox_DiffGrossAmt.Text = ""

        ' Clear the data grid
        Me.UltraGrid_Invoices.DataSource = New ArrayList()
        logger.Debug("ClearControlGroupData exit")
    End Sub

    ''' <summary>
    ''' Populate the invoices data grid with the orders for the selected control group.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateDataGrid()
        logger.Debug("PopulateDataGrid entry: orderList.count=" + currentControlGroup.Orders.Count.ToString)
        Me.UltraGrid_Invoices.DisplayLayout.Bands(0).Reset()
        Me.UltraGrid_Invoices.DataSource = currentControlGroup.Orders
        FormatDataGrid()
        logger.Debug("PopulateDataGrid exit")
    End Sub

    ''' <summary>
    ''' Format the data in the orders grid.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatDataGrid()
        logger.Debug("FormatDataGrid entry")
        If UltraGrid_Invoices.DisplayLayout.Bands(0).Columns.Count > 0 Then
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceType").Hidden = True
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("VendorName").Hidden = True
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceCost").Hidden = True
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceFreight").Hidden = True
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceStatus").Hidden = True
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceStatusMsg").Hidden = True
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("UpdateUserId").Hidden = True

            ' If this is a closed order, add a status image column.
            If currentControlGroup.Status = ControlGroupStatus.Closed Then
                UltraGrid_Invoices.DisplayLayout.Bands(0).Columns.Add("StatusImage")
                UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("StatusImage").Header.VisiblePosition = 0
                UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("StatusImage").Header.Caption = ""
                UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("StatusImage").DataType = GetType(Bitmap)
                UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("StatusImage").CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center
                UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("StatusImage").CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle
            End If
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceDate").Header.VisiblePosition = 1
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceDate").Header.Caption = "Inv Date"
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceNum").Header.VisiblePosition = 2
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceNum").Header.Caption = "Inv #"
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceTotal").Header.VisiblePosition = 3
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceTotal").Header.Caption = "Inv Total"
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("InvoiceTotal").Format = "$###,###,##0.00"
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("PONum").Header.VisiblePosition = 4
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("PONum").Header.Caption = "PO #"
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("VendorID").Header.VisiblePosition = 5
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("VendorID").Header.Caption = "Vendor ID"
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("VendorKey").Header.VisiblePosition = 6
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("VendorKey").Header.Caption = "Vendor Key"
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("CreditInv").Header.VisiblePosition = 7
            UltraGrid_Invoices.DisplayLayout.Bands(0).Columns("CreditInv").Header.Caption = "Credit Inv"
        End If

        ' If this is a closed order, populate the status image column and display the status message as rollover text.
        If currentControlGroup.Status = ControlGroupStatus.Closed Then
            Dim currentRow As UltraGridRow
            Dim currentStatus As ControlGroupInvoiceStatus
            For Each currentRow In UltraGrid_Invoices.Rows
                ' Highlight each row based on the status value
                currentStatus = CType(currentRow.Cells("InvoiceStatus").Value, ControlGroupInvoiceStatus)
                Select Case currentStatus
                    Case ControlGroupInvoiceStatus.InvoiceSuccess
                        currentRow.Appearance.BackColor = Color.Green
                        currentRow.ToolTipText = "SUCCESS"
                        currentRow.Cells("StatusImage").Value = Nothing
                    Case ControlGroupInvoiceStatus.InvoiceWarning
                        currentRow.Appearance.BackColor = Color.Yellow
                        currentRow.ToolTipText = currentRow.Cells("InvoiceStatusMsg").Value.ToString()
                        currentRow.Cells("StatusImage").Value = bmpWarning
                    Case ControlGroupInvoiceStatus.InvoiceError
                        currentRow.Appearance.BackColor = Color.Red
                        currentRow.ToolTipText = currentRow.Cells("InvoiceStatusMsg").Value.ToString()
                        currentRow.Cells("StatusImage").Value = bmpError
                End Select
            Next
        End If

        ' Format the grid control options
        UltraGrid_Invoices.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
        UltraGrid_Invoices.DisplayLayout.Bands(0).Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False
        UltraGrid_Invoices.DisplayLayout.Bands(0).Override.AllowAddNew = AllowAddNew.No
        If currentControlGroup.Status = ControlGroupStatus.Closed Then
            ' Do not allow the user to select the rows for closed orders.
            UltraGrid_Invoices.DisplayLayout.Bands(0).Override.MaxSelectedRows = 0
            UltraGrid_Invoices.DisplayLayout.Bands(0).Override.SelectTypeCell = SelectType.None
            UltraGrid_Invoices.DisplayLayout.Bands(0).Override.SelectTypeRow = SelectType.None
            UltraGrid_Invoices.DisplayLayout.Bands(0).Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False
        Else
            ' Allow the user to select a row and default to the first row being selected
            UltraGrid_Invoices.DisplayLayout.Bands(0).Override.MaxSelectedRows = 1
            UltraGrid_Invoices.DisplayLayout.Bands(0).Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True
            UltraGrid_Invoices.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect

            ' Default the first row to selected
            If UltraGrid_Invoices.Rows.Count > 0 Then
                UltraGrid_Invoices.Rows(0).Selected = True
            End If
        End If

        logger.Debug("FormatDataGrid exit")
    End Sub

    ''' <summary>
    ''' When the control group has reached a balanced status, the user is notified and they are 
    ''' prompted to close the control group.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BalancedControlGroupPrompt()
        logger.Debug("BalancedControlGroupPrompt entry: dataLoading=" + dataLoading.ToString)
        If Not dataLoading AndAlso (currentControlGroup.Status = ControlGroupStatus.Open) AndAlso Not notifyBalanced Then
            If MsgBox("The control group is now in balance." + Environment.NewLine + "Do you want to close this control group?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                ' Close the control group
                CloseControlGroup()
            Else
                ' The user has been notified that the control group is in balance.  Don't display
                ' the notification again with the same data.
                notifyBalanced = True
            End If
        End If
        logger.Debug("BalancedControlGroupPrompt exit")
    End Sub

    ''' <summary>
    ''' Each time an invoice is added to the control group or the expected number of invoices is changed,
    ''' the number of differences are recalculated.  The user is notified if the change causes the control group
    ''' to now be in balance or the expected invoice total has now been reached.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ResetNumInvoicesDifference()
        logger.Debug("ResetNumInvoicesDifference entry: dataLoading=" + dataLoading.ToString)
        Dim numInvDiff As Integer = currentControlGroup.ExpectedInvoiceCount - currentControlGroup.Orders.Count
        Me.TextBox_DiffNumInvoices.Text = numInvDiff.ToString()

        If Not dataLoading Then
            ' Display a prompt to the user if this is an OPEN invoice and the control group invoice total has been reached
            ' or the control group is now in balance.
            If (currentControlGroup.Status = ControlGroupStatus.Open) AndAlso currentControlGroup.IsControlGroupBalanced() Then
                BalancedControlGroupPrompt()
            ElseIf (currentControlGroup.Status = ControlGroupStatus.Open) AndAlso currentControlGroup.Orders.Count >= 1 AndAlso numInvDiff = 0 AndAlso Not notifyInvoices Then
                MsgBox("# Invoices Expected now equals # Invoices Entered.", MsgBoxStyle.OkOnly, Me.Text)
                ' The user has been notified that the number of invoices are equal.  Don't display
                ' the notification again with the same data.
                notifyInvoices = True
            End If
        End If

        logger.Debug("ResetNumInvoicesDifference exit")
    End Sub

    ''' <summary>
    ''' Each time an invoice is added to the control group or the expected gross amount is changed,
    ''' the differences are recalculated.  The user is notified if the change causes the control group
    ''' to now be in balance or the expected amount has now been reached.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ResetGrossAmtDifferences()
        logger.Debug("ResetGrossAmtDifferences entry: dataLoading=" + dataLoading.ToString)
        Dim grossAmtDiff As Double = currentControlGroup.ExpectedGrossAmt - currentControlGroup.OrdersInvoiceTotal
        Me.TextBox_DiffGrossAmt.Text = grossAmtDiff.ToString("C")

        If Not dataLoading Then
            ' Display a prompt to the user if this is an OPEN invoice and the control group amount has been reached
            ' or the control group is now in balance.
            If (currentControlGroup.Status = ControlGroupStatus.Open) AndAlso currentControlGroup.IsControlGroupBalanced() Then
                BalancedControlGroupPrompt()
            ElseIf (currentControlGroup.Status = ControlGroupStatus.Open) AndAlso currentControlGroup.Orders.Count >= 1 AndAlso grossAmtDiff = 0 AndAlso Not notifyAmount Then
                MsgBox("Gross Amt Expected now equals Gross Amt Entered.", MsgBoxStyle.OkOnly, Me.Text)
                ' The user has been notified that the amounts are equal.  Don't display
                ' the notification again with the same data.
                notifyAmount = True
            End If
        End If

        logger.Debug("ResetNumInvoicesDifference exit")
    End Sub
#End Region

#Region "Form Actions"
    ''' <summary>
    ''' Initial load of the UI form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InvoiceControlGroup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        logger.Debug("InvoiceControlGroup_Load entry")

        ' Update the control group data
        PopulateControlGroupData()

        logger.Debug("InvoiceControlGroup_Load exit")
    End Sub

    ''' <summary>
    ''' Exit button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        logger.Debug("Button_Exit_Click entry")
        ' Close the form.  The user is prompted to save any changes when the form is closed.
        Me.Close()
        logger.Debug("Button_Exit_Click exit")
    End Sub

    ''' <summary>
    ''' Form closing.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InvoiceControlGroup_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("InvoiceControlGroup_FormClosing entry")
        ' Are there any changes that need to be saved to the current control group before the form is closed?
        If dataChanges Then
            'prompt the user to save changes
            If MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
            End If
        End If
        logger.Debug("InvoiceControlGroup_FormClosing exit")
    End Sub
#End Region

#Region "Control Group Actions"
    ''' <summary>
    ''' Save the changes for the current control group to the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ApplyChanges()
        logger.Debug("ApplyChanges entry")
        ' Populate the user id with the current user before performing the save
        currentControlGroup.UpdateUserId = giUserID
        logger.Info("Saving updates to an invoice control group: OrderInvoice_ControlGroup_ID=" + currentControlGroup.ControlGroupId.ToString() + ", UpdateUser_ID=" + currentControlGroup.UpdateUserId.ToString())
        ControlGroupDAO.UpdateControlGroup(currentControlGroup)
        ' Reset the data changes flag since all changes have been saved
        dataChanges = False
        logger.Debug("ApplyChanges exit")
    End Sub

    ''' <summary>
    ''' Save any pending changes for the control group to the database and move the control group to the 
    ''' closed status.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CloseControlGroup()
        logger.Debug("CloseControlGroup entry")
        ' Close the control group
        currentControlGroup.UpdateUserId = giUserID
        logger.Info("Closing an invoice control group: OrderInvoice_ControlGroup_ID=" + currentControlGroup.ControlGroupId.ToString() + ", UpdateUser_ID=" + currentControlGroup.UpdateUserId.ToString())
        ControlGroupDAO.CloseControlGroup(currentControlGroup)
        ' Verify a DB error was not encountered during the closure
        If currentControlGroup.CloseStatus <> 0 Then
            logger.Error("Error received when closing a control group invoice: OrderInvoice_ControlGroup_ID=" + currentControlGroup.ControlGroupId.ToString() + ".  An entry has been logged in the TryCatch_ErrorLog table." + Environment.NewLine + "Close Status = " + currentControlGroup.CloseStatus.ToString() + Environment.NewLine + "Close Message = " + currentControlGroup.CloseErrorMsg)
            MsgBox("An error was encountered when closing the control group.  Your updates were not applied." + Environment.NewLine + "Database Status = " + currentControlGroup.CloseStatus.ToString() + Environment.NewLine + "Database Error Message = " + currentControlGroup.CloseErrorMsg, MsgBoxStyle.OkOnly, Me.Text)
        Else
            ' Refresh the UI, which will show the status of each invoice as the control group was closed.
            PopulateControlGroupData(True)
            ' Launch the 3 way matching detailed summary report for this control group.
            Dim sReportURL As New StringBuilder
            sReportURL.Append("3WayMatchDetailSummary")
            ' Report Parameters
            sReportURL.Append("&rs:Command=Render")
            sReportURL.Append("&rc:Parameters=False")
            sReportURL.Append("&ControlGroup_ID=")
            sReportURL.Append(currentControlGroup.ControlGroupId)
            ' Open the report
            Call ReportingServicesReport(sReportURL.ToString)
        End If
        logger.Debug("CloseControlGroup exit")
    End Sub

    ''' <summary>
    ''' Create a new control group.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_AddControlGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddControlGroup.Click
        logger.Debug("Button_AddControlGroup_Click entry")
        ' Are there any changes that need to be saved to the current control group before the create?
        If dataChanges Then
            'prompt the user to save changes
            If MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
            End If
        Else
            ' The user elected not to save their changes.  Reset the flag so they are not prompted for the save again.
            dataChanges = False
        End If

        ' Create a new control group 
        currentControlGroup = New ControlGroupBO()
        currentControlGroup.UpdateUserId = giUserID
        logger.Info("Creating a new invoice control group: UpdateUser_ID=" + currentControlGroup.UpdateUserId.ToString())
        ControlGroupDAO.CreateControlGroup(currentControlGroup)
        _currentControlGroupID = currentControlGroup.ControlGroupId()

        ' Set the values in the business object from the new db entry
        currentControlGroup.Status = ControlGroupStatus.Open
        currentControlGroup.ExpectedGrossAmt = 0
        currentControlGroup.ExpectedInvoiceCount = 0

        ' Refresh the UI data
        PopulateControlGroupData(False)

        logger.Debug("Button_AddControlGroup_Click exit")
    End Sub

    ''' <summary>
    ''' Move a control group from the OPEN to the CLOSED state.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_CloseControlGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_CloseControlGroup.Click
        logger.Debug("Button_CloseControlGroup_Click entry")
        ' Confirm the closure, checking the balance status and displaying the appropriate message for confirmation.
        Dim confimClose As MsgBoxResult
        If currentControlGroup.IsControlGroupBalanced Then
            confimClose = MsgBox("The control group is in balance. " + Environment.NewLine + "Are you sure you want to close the control group?", MsgBoxStyle.YesNo, Me.Text)
        Else
            confimClose = MsgBox("WARNING: the control group is not in balance. " + Environment.NewLine + "Are you sure you want to close the control group?", MsgBoxStyle.YesNo, Me.Text)
        End If

        If confimClose = MsgBoxResult.Yes Then
            ' Close the control group.
            CloseControlGroup()
        End If
        logger.Debug("Button_CloseControlGroup_Click exit")
    End Sub

    ''' <summary>
    ''' Search for an existing control group to populate the UI screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_SearchControlGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SearchControlGroup.Click
        logger.Debug("Button_SearchControlGroup_Click entry")
        ' Are there any changes that need to be saved to the current control group before the search?
        If dataChanges Then
            'prompt the user to save changes
            If MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
            End If
        Else
            ' The user elected not to save their changes.  Reset the flag so they are not prompted for the save again.
            dataChanges = False
        End If

        ' Search for an existing control group
        ClearControlGroupData()
        invoiceSearchForm = New InvoiceControlGroupSearch
        invoiceSearchForm.ShowDialog(Me)
        invoiceSearchForm.Dispose()
        logger.Debug("Button_SearchControlGroup_Click exit")
    End Sub
#End Region

#Region "Invoice Actions"
    ''' <summary>
    ''' A new invoice is being added to the control group.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_AddInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddInvoice.Click
        logger.Debug("Button_AddInvoice_Click entry")

        ' Save any pending changes to the control data before opening the child form to prevent data
        ' loss on refresh
        If dataChanges Then
            ApplyChanges()
        End If

        ' Open child form
        Dim editInvoiceForm As New EditControlGroupInvoice()
        editInvoiceForm.CurrentControlGroupID = _currentControlGroupID
        editInvoiceForm.ShowDialog(Me)
        editInvoiceForm.Close()
        editInvoiceForm.Dispose()

        ' Refresh the data grid to reflect any changes made on the child form.
        PopulateControlGroupData(True)

        logger.Debug("Button_AddInvoice_Click exit")
    End Sub

    ''' <summary>
    ''' An existing invoice is being edited.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditInvoice()
        logger.Debug("EditInvoice entry")
        If UltraGrid_Invoices.Selected.Rows.Count = 1 Then
            ' Save any pending changes to the control data before opening the child form to prevent data
            ' loss on refresh
            If dataChanges Then
                ApplyChanges()
            End If

            ' Open the edit form for the selected invoice.
            Dim editInvoiceForm As New EditControlGroupInvoice()
            editInvoiceForm.CurrentControlGroupID = _currentControlGroupID
            editInvoiceForm.CurrentInvoice = New ControlGroupInvoiceBO(UltraGrid_Invoices.Selected.Rows.Item(0))
            editInvoiceForm.ShowDialog(Me)
            editInvoiceForm.Close()
            editInvoiceForm.Dispose()

            ' Refresh the data grid to reflect any changes made on the child form.
            PopulateControlGroupData(True)
        Else
            ' Alert the user that they must select at least one invoice
            MsgBox("An invoice from the list must be selected.", MsgBoxStyle.Exclamation, Me.Text)
        End If
        logger.Debug("EditInvoice exit")
    End Sub

    ''' <summary>
    ''' Click the edit button to edit an existing invoice.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_EditInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EditInvoice.Click
        logger.Debug("Button_EditInvoice_Click entry")
        EditInvoice()
        logger.Debug("Button_EditInvoice_Click exit")
    End Sub

    ''' <summary>
    ''' Double clicking a row in the grid performs the same action as the Edit button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGrid_Invoices_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid_Invoices.DoubleClickRow
        logger.Debug("UltraGrid_Invoices_DoubleClickRow entry")
        EditInvoice()
        logger.Debug("UltraGrid_Invoices_DoubleClickRow exit")
    End Sub

    ''' <summary>
    ''' An invoice is being deleted from the control group.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_DeleteInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_DeleteInvoice.Click
        logger.Debug("Button_DeleteInvoice_Click entry")
        If UltraGrid_Invoices.Selected.Rows.Count = 1 Then
            ' Prompt the user to delete the selected invoice.
            If MsgBox("Delete the selected invoice from the control group?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                ' Save any pending changes to the control data before opening the child form to prevent data
                ' loss on refresh
                If dataChanges Then
                    ApplyChanges()
                End If

                ' Delete the invoice
                Dim selectedInvoice As New ControlGroupInvoiceBO(UltraGrid_Invoices.Selected.Rows.Item(0))
                ControlGroupDAO.DeleteControlGroupInvoice(selectedInvoice, _currentControlGroupID)

                ' Refresh the data grid to reflect the invoice deletion.
                PopulateControlGroupData(True)
            End If
        Else
            ' Alert the user that they must select at least one invoice
            MsgBox("An invoice from the list must be selected.", MsgBoxStyle.Exclamation, Me.Text)
        End If

        logger.Debug("Button_DeleteInvoice_Click exit")
    End Sub
#End Region

#Region "Data change events"
    ''' <summary>
    ''' Handle changes to the expected number of invoices data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraNumericEditor_ExpectedNumInvoices_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraNumericEditor_ExpectedNumInvoices.ValueChanged
        logger.Debug("UltraNumericEditor_ExpectedNumInvoices_ValueChanged entry: dataLoading=" + dataLoading.ToString)
        If Not dataLoading Then
            ' Reset the notification prompts because the data has changed
            notifyInvoices = False
            notifyBalanced = False
            ' Set the data changes to true so the changes are persisted
            dataChanges = True
            If UltraNumericEditor_ExpectedNumInvoices.Value Is DBNull.Value Then
                UltraNumericEditor_ExpectedNumInvoices.Value = 0
            End If
            ' Update the buisness object so the business rule calculations are performed correctly
            currentControlGroup.ExpectedInvoiceCount = CInt(UltraNumericEditor_ExpectedNumInvoices.Value)
            ' Recalculate the differences and update the UI
            ResetNumInvoicesDifference()
        End If
        logger.Debug("UltraNumericEditor_ExpectedNumInvoices_ValueChanged exit")
    End Sub

    ''' <summary>
    ''' Handle changes to the expected gross amount data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraNumericEditor_ExpectedGrossAmt_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraNumericEditor_ExpectedGrossAmt.ValueChanged
        logger.Debug("UltraNumericEditor_ExpectedGrossAmt_ValueChanged entry: dataLoading=" + dataLoading.ToString)
        If Not dataLoading Then
            ' Reset the notification prompts because the data has changed
            notifyAmount = False
            notifyBalanced = False
            ' Set the data changes to true so the changes are persisted
            dataChanges = True
            If UltraNumericEditor_ExpectedGrossAmt.Value Is DBNull.Value Then
                UltraNumericEditor_ExpectedGrossAmt.Value = 0
            End If
            ' Update the buisness object so the business rule calculations are performed correctly
            currentControlGroup.ExpectedGrossAmt = CDec(UltraNumericEditor_ExpectedGrossAmt.Value)
            ' Recalculate the differences and update the UI
            ResetGrossAmtDifferences()
        End If
        logger.Debug("UltraNumericEditor_ExpectedGrossAmt_ValueChanged exit")
    End Sub
#End Region

End Class