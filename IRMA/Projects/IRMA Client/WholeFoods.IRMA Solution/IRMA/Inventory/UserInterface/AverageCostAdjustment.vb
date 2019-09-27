Imports log4net
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class frmAvgCostAdjustment

#Region " Private Fields"

    Private IsLoading As Boolean
    Private mbFilling As Boolean
    Private sValidationMessage As String = ""
    Private bValidSelection As Boolean = True
    Private CurrentItem As AvgCostAdjBO.AvgCostAdjItem
    Private Tolerance As Decimal = CDec(ConfigurationServices.AppSettings("AvgCostTolerance"))
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region " Custom events"

    Private Event AdjustmentChanged()

    Private Sub CalculateVariance() Handles Me.AdjustmentChanged

        ' compare to current avg cost to determine if out of tolerance
        If Me.gridHistory.Rows.Count > 0 Then

            If IsNumeric(Me.txtAmount.Text) Then

                Dim _currAvgCost As Decimal = CDec(Me.gridHistory.Rows(0).Cells("AvgCost").Value)
                Dim _variance As Decimal = ((_currAvgCost - CDec(Me.txtAmount.Text)) / CDec(Me.txtAmount.Text)) * 100

                If _variance < 0 Then _variance = _variance * -1

                If _variance > Me.Tolerance Then
                    If Not Me.lblOutOfTolerance.Visible Then
                        Me.lblOutOfTolerance.Visible = True
                    End If
                    Me.txtAmount.BackColor = Color.MistyRose
                Else
                    Me.lblOutOfTolerance.Visible = False
                    Me.txtAmount.BackColor = Color.White
                End If

            End If

        End If

    End Sub

    Private Sub ResetAdjustmentValues()

        ' reset the values for the adjustment group to defaults

        Me.IsLoading = True

        Me.lblOutOfTolerance.Visible = False
        Me.txtAmount.BackColor = Color.White

        Me.txtAmount.Text = String.Empty
        Me.txtComments.Text = String.Empty
        Me.cmbReason.SelectedIndex = -1

        Me.txtAmount.Focus()

        Me.IsLoading = False

    End Sub

    Private Function ValidateData() As Boolean

        logger.Debug("ValidateData entry")

        ValidateData = True

        If Not IsValidAdjustmentAmount() Then
            ValidateData = False
        End If

        If Not IsValidReasonSelection() Then
            ValidateData = False
        End If

        logger.Debug("ValidateData exit")

    End Function

    Private Function IsValidAdjustmentAmount() As Boolean

        Dim errCount As Integer = 0

        If Me.txtAmount.Text.Length > 0 Then
            If Not IsNumeric(Me.txtAmount.Text) Then
                errCount = errCount + 1
                Me.frmErrorProvider.SetError(Me.txtAmount, "You must enter a new average cost.")
            Else
                Me.CurrentItem.AvgCost = CDec(Me.txtAmount.Text)
            End If
        Else
            errCount = errCount + 1
            Me.frmErrorProvider.SetError(Me.txtAmount, "You must enter a new average cost.")
        End If

        If errCount = 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function IsValidReasonSelection() As Boolean

        Dim errCount As Integer = 0

        ' validate reason selected
        If Not Me.cmbReason.SelectedIndex > -1 Then
            errCount = errCount + 1
            Me.frmErrorProvider.SetError(Me.cmbReason, "You must select a reason for this adjustment.")
        Else
            Me.CurrentItem.Reason = CInt(Me.cmbReason.SelectedValue)
        End If

        If errCount = 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub ValidateStoreSelection()
        'Validations
        Select Case True
            Case optSelectStore.Checked
                If cmbStoreSelection.SelectedIndex = -1 Then
                    sValidationMessage = "Please provide a Store Selection!"
                    bValidSelection = False
                Else
                    sValidationMessage = ""
                    bValidSelection = True
                End If
            Case optSelectZone.Checked
                If cmbZones.SelectedIndex = -1 Then
                    sValidationMessage = "Please provide a Zone Selection!"
                    bValidSelection = False
                Else
                    sValidationMessage = ""
                    bValidSelection = True
                End If
            Case optSelectState.Checked
                If cmbState.SelectedIndex = -1 Then
                    sValidationMessage = "Please provide a State Selection!"
                    bValidSelection = False
                Else
                    sValidationMessage = ""
                    bValidSelection = True
                End If
            Case optSelectALLStores.Checked
                sValidationMessage = ""
                bValidSelection = True
            Case optSelectHFM.Checked
                sValidationMessage = ""
                bValidSelection = True
            Case optSelectWFM.Checked
                sValidationMessage = ""
                bValidSelection = True
        End Select

    End Sub

#End Region

#Region " Form Events"

    Private Sub InventoryCosting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        glItemID = -1

        IsLoading = True

        'Load the facilities that can do adjustments
        Me.cmbStoreSelection.Items.Clear()
        Me.cmbReason.Items.Clear()

        Try

            ' set the current tolerance value
            Me.lblOutOfTolerance.Text = String.Format(Me.lblOutOfTolerance.Text, Me.Tolerance)
            ' what?

            Dim dt As DataTable = AvgCostAdjBO.GetStoreList()

            Dim dr As DataRow = dt.NewRow()
            dr("Store_No") = -1
            dr("Store_Name") = ""
            dt.Rows.InsertAt(dr, 0)

            Me.cmbStoreSelection.DataSource = dt
            Me.cmbStoreSelection.DisplayMember = "Store_Name"
            Me.cmbStoreSelection.ValueMember = "Store_No"
            Me.cmbStoreSelection.SelectedIndex = -1

            If Not glStore_Limit = 0 Then
                Me.cmbStoreSelection.SelectedValue = glStore_Limit
            End If

            Me.cmbStoreSelection.Enabled = (glStore_Limit = 0 And (gbSuperUser Or gbCostAdmin))

            Me.cmbReason.DataSource = AvgCostAdjBO.GetAdjustmentReasons(True, True)
            Me.cmbReason.DisplayMember = "Description"
            Me.cmbReason.ValueMember = "ID"
            Me.cmbReason.SelectedIndex = -1

            Me.cmbStoreSelection.Focus()

        Catch ex As Exception

            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

		Me.cmbSubTeam.DataSource = SubTeamDAO.GetSubteams()
		LoadZone(Me.cmbZones)
        LoadStates(Me.cmbState)

        Me.cmbSubTeam.SelectedIndex = -1
        Me.cmbZones.SelectedIndex = -1
        Me.cmbState.SelectedIndex = -1

        Me.chkCurrentCost.Checked = True

        Me.cmbZones.Enabled = False
        Me.cmbState.Enabled = False

        Me.dtpEndDate.Enabled = False
        Me.dtpStartDate.Enabled = False
        'The default for the StartDate is set to Date component so it uses 00:00:00 for time during seraches
        Me.dtpStartDate.Value = SystemDateTime.Date
        'The default for the EndDate is set to DateTime so it uses current time during seraches
        Me.dtpEndDate.Value = SystemDateTime()

        Me.sValidationMessage = ""

        IsLoading = False

    End Sub


#End Region

#Region " Control Events"

    Private Sub cmdItemSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdItemSearch.Click

        Me.ValidateStoreSelection()

        If bValidSelection Then
			If Not cmbSubTeam.SelectedIndex = -1 Then

				'-- Set glItemid to none found
				frmItemSearch.InitForm()
				frmItemSearch.ShowDialog()

				If glItemID <> 0 Then
					'-- if its not zero, then something was found
					Me.LoadItem()
					' set the form title to show the item selected
					Me.Text = "Inventory Costing - " & Trim(Me.txtItemDesc.Text)
					'load avg cost history or current avg cost based on selections
					LoadHistory()

				End If

				frmItemSearch.Close()
				frmItemSearch.Dispose()
			Else
				MessageBox.Show("Please provide a Subteam Selection!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Else
            MessageBox.Show(sValidationMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub cmdSaveAdjustment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveAdjustment.Click

        If Me.ValidateData() Then

            Dim StoreSelector As New AvgCostStoreSelector
            StoreSelector.ShowDialog()

            If MessageBox.Show(String.Format(My.Resources.Inventory.msg_Confirm_CostAdjustment, _
                                            Me.txtItemDesc.Text, _
                                            Me.CurrentItem.AvgCost) _
                                & vbCrLf & vbCrLf & My.Resources.Inventory.msg_Warning_CostAdjustment _
                                & vbCrLf & vbCrLf & My.Resources.Inventory.msg_Confirm_Action _
                                , Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) _
                                = Windows.Forms.DialogResult.Yes Then

                'Call the Save for each store selected in the store selector form
                For Each StoreNo As Integer In StoreSelector.StoreList

                    Me.CurrentItem.BusinessUnit = StoreNo
                    Me.CurrentItem.AvgCost = CDec(Me.txtAmount.Text)
                    Me.CurrentItem.Comment = Me.txtComments.Text
                    Me.CurrentItem.Reason = CInt(Me.cmbReason.SelectedValue)

                    AvgCostAdjBO.Save(Me.CurrentItem)

                Next

                MessageBox.Show("Adjustment saved for selected stores!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.ResetAdjustmentValues()

                Me.LoadHistory()

            End If

        End If

    End Sub

    Private Sub txtAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAmount.TextChanged
        If Me.IsLoading Then Exit Sub
        RaiseEvent AdjustmentChanged()
    End Sub

    Private Sub cmbReason_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReason.Validated
        Me.frmErrorProvider.SetError(Me.cmbReason, "")
    End Sub

    Private Sub txtAmount_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAmount.Validated
        Me.frmErrorProvider.SetError(Me.txtAmount, "")
    End Sub

    Private Sub optSelectStore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSelectStore.CheckedChanged

        If mbFilling Or IsLoading Then Exit Sub

        mbFilling = True

        cmbZones.SelectedIndex = -1
        cmbState.SelectedIndex = -1
        cmbStoreSelection.Enabled = True
        cmbZones.Enabled = False
        cmbState.Enabled = False

        mbFilling = False

    End Sub

    Private Sub optSelectZone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSelectZone.CheckedChanged

        If mbFilling Or IsLoading Then Exit Sub

        mbFilling = True

        cmbStoreSelection.SelectedIndex = -1
        cmbState.SelectedIndex = -1
        cmbZones.Enabled = True
        cmbStoreSelection.Enabled = False
        cmbState.Enabled = False

        mbFilling = False

    End Sub

    Private Sub optSelectState_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSelectState.CheckedChanged

        If mbFilling Or IsLoading Then Exit Sub

        mbFilling = True

        cmbZones.SelectedIndex = -1
        cmbStoreSelection.SelectedIndex = -1
        cmbState.Enabled = True
        cmbZones.Enabled = False
        cmbStoreSelection.Enabled = False

        mbFilling = False

    End Sub

    Private Sub SetCombo()
        cmbZones.SelectedIndex = -1
        cmbStoreSelection.SelectedIndex = -1
        cmbState.SelectedIndex = -1

        cmbStoreSelection.Enabled = False
        cmbZones.Enabled = False
        cmbState.Enabled = False
    End Sub

    Private Sub optSelectALLStores_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSelectALLStores.CheckedChanged

        If mbFilling Or IsLoading Then Exit Sub

        mbFilling = True
        SetCombo()
        mbFilling = False

    End Sub

    Private Sub optSelectHFM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSelectHFM.CheckedChanged

        If mbFilling Or IsLoading Then Exit Sub

        mbFilling = True
        SetCombo()
        mbFilling = False

    End Sub

    Private Sub optSelectWFM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSelectWFM.CheckedChanged

        If mbFilling Or IsLoading Then Exit Sub

        mbFilling = True
        SetCombo()
        mbFilling = False

    End Sub

    Private Sub chkCurrentCost_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCurrentCost.CheckedChanged

        If mbFilling Or IsLoading Then Exit Sub

        mbFilling = True

        If chkCurrentCost.Checked Then
            dtpEndDate.Enabled = False
            dtpStartDate.Enabled = False
        Else
            dtpEndDate.Enabled = True
            dtpStartDate.Enabled = True
        End If

        mbFilling = False

    End Sub


#End Region

#Region " Refresh Data"

    Private Sub LoadItem()

        Dim sSQL As String

        IsLoading = True

        Try

            If glItemID <> -1 Then
                sSQL = "EXEC GetItemData " & glItemID.ToString
                gRSRecordset = SQLOpenRecordSet(sSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End If

            Me.txtItemDesc.Text = gRSRecordset.Fields("Item_Description").Value.ToString
            Me.txtIdentifier.Text = gRSRecordset.Fields("Identifier").Value.ToString

        Finally

            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If

        End Try

        IsLoading = False

    End Sub

    Private Sub LoadHistory()

        If chkCurrentCost.CheckState = CheckState.Unchecked Then
            If dtpEndDate.Value <> Nothing AndAlso (dtpEndDate.Value < dtpStartDate.Value) Then
                MessageBox.Show("You must select an End Date that is greater than Start Date.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If

        Dim Zone As Integer = 0

        If (cmbZones.SelectedIndex > -1) Then
            Zone = VB6.GetItemData(cmbZones, cmbZones.SelectedIndex)
        End If

		' load the current adjustment history information
		Dim _avgCostItem As New AvgCostAdjBO.AvgCostAdjItem(glItemID,
															Me.cmbStoreSelection.SelectedValue,
															Me.optSelectWFM.Checked,
															Me.optSelectHFM.Checked,
															Me.optSelectALLStores.Checked,
															Zone,
															Me.cmbState.SelectedItem,
															If(cmbSubTeam.SelectedItem Is Nothing, 0, cmbSubTeam.SelectedItem.SubTeamNo),
															Me.chkCurrentCost.Checked,
															dtpStartDate.Value,
															dtpEndDate.Value
															)

		' set the form data
		CurrentItem = _avgCostItem

        ' bind the grid to the history
        Me.gridHistory.DataSource = AvgCostAdjBO.GetAvgCostForStores(CurrentItem)

        If Me.gridHistory.Rows.Count > 0 Then

            Me.gridHistory.ActiveRow = Me.gridHistory.Rows(0)
            Me.gridHistory.Rows(0).Selected = True

            ' set security to allow an adjustment
            Me.grpAdjustment.Enabled = gbCostAdmin OrElse gbSuperUser

            ' enable the data grid group
            Me.grpHistory.Enabled = True
        Else
            MessageBox.Show("There is no average cost history for this item." & vbCrLf & "Receive the item on a PO to begin calculating average cost.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

    End Sub

#End Region

End Class