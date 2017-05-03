Option Strict Off
Option Explicit On

Imports WholeFoods.IRMA.ItemHosting.DataAccess

Friend Class frmLineDrive
	Inherits System.Windows.Forms.Form
	
	Private mbNoClick As Boolean
    Private mbFilling As Boolean
    Private IsInitializing As Boolean
    Private mdtStores As DataTable

	Private Enum geStoreCol
		StoreNo = 0
		StoreName = 1
		ZoneID = 2
		State = 3
		WFMStore = 4
		MegaStore = 5
		CustomerType = 6
	End Enum
	
	Private Sub frmLineDrive_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

		CenterForm(Me)
		
		LoadBrand((Me.cmbBrand))
		
		SetActive(txtField(iPriceEarnedDiscount1), False)
		SetActive(txtField(iPriceEarnedDiscount2), False)
		SetActive(txtField(iPriceEarnedDiscount3), False)
		
		LoadPricingMethod(cmbPricingMethod)
		If cmbPricingMethod.Items.Count > 0 Then cmbPricingMethod.SelectedIndex = 0
		
		LoadZone(cmbZones)

        '-- Fill out the store list
        mdtStores = StoreDAO.GetRetailStoreList()
        ugrdStoreList.DataSource = mdtStores

        Call StoreListGridLoadStatesCombo(mdtStores, cmbStates)

		Call SetCombos()
		
        dtpStartDate.Value = System.DateTime.Today
        dtpEndDate.Value = System.DateTime.Today

    End Sub
	
	Private Function GetConflicts() As ADODB.Recordset
		
        Dim rs As ADODB.Recordset = Nothing
		Dim sBrand As String
		Dim sStores As String
		
		If cmbBrand.SelectedIndex > -1 Then
			sBrand = CStr(VB6.GetItemData(cmbBrand, cmbBrand.SelectedIndex))
		Else
			sBrand = "NULL"
		End If
		
        sStores = StoreListGridGetStoreList(ugrdStoreList)

        ' Convert the start date to US format for db
        Dim sStartDate As String
        sStartDate = CDate(dtpStartDate.Value).ToString(ResourcesIRMA.GetString("DateStringFormat"))

        ' Convert the end date to US format for db
        Dim sEndtDate As String
        sEndtDate = CDate(dtpEndDate.Value).ToString(ResourcesIRMA.GetString("DateStringFormat"))
        Try
            gRSRecordset = SQLOpenRecordSet("EXEC GetLineDriveConflicts '" & Me.txtFamily.Text & "'," & sBrand & ",'" & sStores & "','|','" & sStartDate & "','" & sEndtDate & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            CreateEmptyADORS_FromDAO(gRSRecordset, rs, bAllowNulls:=False)

            If Not IsNothing(rs) Then
                rs.Open()
                Do While Not gRSRecordset.EOF
                    CopyDAORecordToADORecord(gRSRecordset, rs, bAllowNulls:=False)
                    gRSRecordset.MoveNext()
                Loop
            End If

            GetConflicts = rs
        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

    End Function

	Private Sub cmbBrand_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbBrand.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 8 Then cmbBrand.SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub cmdApply_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdApply.Click
		
        Dim rs As dao.Recordset = Nothing
		Dim rsConflicts As ADODB.Recordset
		Dim sBrand_ID As String
		Dim sStores As String
		Dim dblPercent As Double
		
        If MsgBox(ResourcesPricing.GetString("UpdatePrices"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub
		
		If Len(Trim(Me.txtFamily.Text)) = 0 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblFamily.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
			txtFamily.Focus()
			Exit Sub
		End If
		
		If cmbBrand.SelectedIndex > -1 Then
			sBrand_ID = CStr(VB6.GetItemData(cmbBrand, cmbBrand.SelectedIndex))
		Else
			sBrand_ID = "NULL"
		End If
		
		If Len(Trim(Me.txtPct.Text)) = 0 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblPercent.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
			txtPct.Focus()
			Exit Sub
		Else
			dblPercent = CDbl(txtPct.Text) / 100
			If Not (dblPercent < 1) Then
                MsgBox(ResourcesPricing.GetString("PercentLessHundred"), MsgBoxStyle.Critical, Me.Text)
				Exit Sub
			End If
		End If
		
        If dtpStartDate.Value < System.DateTime.Today Then
            MsgBox(ResourcesIRMA.GetString("StartDateNotPast"), MsgBoxStyle.Exclamation, Me.Text)
            dtpStartDate.Focus()
            Exit Sub
        End If

        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        If cmbPricingMethod.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblMethod.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            cmbPricingMethod.Focus()
            Exit Sub
        End If
		
		If CDbl(ComboValue(cmbPricingMethod)) = 2 And CShort(IIf(Len(txtField(iPriceEarnedDiscount1).Text) > 0, txtField(iPriceEarnedDiscount1).Text, 0)) < 1 Then
            MsgBox(ResourcesPricing.GetString("QuantityRegularPriceNotZero"), MsgBoxStyle.Critical, Me.Text)
			txtField(iPriceEarnedDiscount1).Focus()
			Exit Sub
		End If
		
		If CShort(IIf(Len(Trim(txtField(iPriceEarnedDiscount3).Text)) = 0, "0", txtField(iPriceEarnedDiscount3).Text)) < 1 Then
            MsgBox(ResourcesPricing.GetString("EarnedSaleLimitNotZero"), MsgBoxStyle.Critical, Me.Text)
			txtField(iPriceEarnedDiscount3).Focus()
			Exit Sub
		End If
		
        If ugrdStoreList.Selected.Rows.Count = 0 Then
            MsgBox(ResourcesIRMA.GetString("SelectStore"), MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        Else
            sStores = StoreListGridGetStoreList(ugrdStoreList)
        End If
		
		rsConflicts = GetConflicts
        If Not IsNothing(rsConflicts) Then
            If rsConflicts.RecordCount > 0 Then
                rsConflicts.Close()
                rsConflicts = Nothing
                MsgBox(ResourcesPricing.GetString("NonLineDrivePriceConflict"), MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
            rsConflicts.Close()
            rsConflicts = Nothing
        End If

        ' Convert the start date to US format for db
        Dim sStartDate As String
        sStartDate = CDate(dtpStartDate.Value).ToString(ResourcesIRMA.GetString("DateStringFormat"))

        ' Convert the end date to US format for db
        Dim sEndDate As String
        sEndDate = CDate(dtpEndDate.Value).ToString(ResourcesIRMA.GetString("DateStringFormat"))

        Try
            rs = SQLOpenRecordSet("EXEC UpdateLineDrive '" & txtFamily.Text & "'," & sBrand_ID & "," & dblPercent & ",'" & sStartDate & "','" & sEndDate & "','" & sStores & "','|'," & ComboValue(cmbPricingMethod) & ", " & txtField(iPriceEarnedDiscount1).Text & ", " & txtField(iPriceEarnedDiscount2).Text & ", " & txtField(iPriceEarnedDiscount3).Text, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If Not IsNothing(rs) Then
                Dim recordsAffected As Integer = rs.Fields("RecordsAffected").Value
                MsgBox(String.Format(ResourcesIRMA.GetString("UpdateCompleted"), recordsAffected), MsgBoxStyle.Information, Me.Text)
            End If
        Finally
            If rs IsNot Nothing Then
                rs.Close()
                rs = Nothing
            End If
        End Try

    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		Dim rsConflicts As ADODB.Recordset
		
		If Len(Trim(Me.txtFamily.Text)) = 0 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblFamily.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
			txtFamily.Focus()
			Exit Sub
		End If
		
		If Len(Trim(Me.txtPct.Text)) = 0 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblPercent.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
			txtPct.Focus()
			Exit Sub
		End If
		
        rsConflicts = GetConflicts()
		
        If Not IsNothing(rsConflicts) Then
            If rsConflicts.RecordCount > 0 Then
                ' ###########################################################################
                ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
                ' ###########################################################################
                MsgBox("LineDrive.vb  cmdReport_Click(): The Crystal Report LineDriveConflicts.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)
                'crwRpt2.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "LineDriveConflicts.rpt"
                'crwRpt2.SetTablePrivateData(0, 3, rsConflicts)
                'crwRpt2.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
                'crwRpt2.Action = 1
            End If
            rsConflicts.Close()
            rsConflicts = Nothing
        End If

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("LineDrive.vb  cmdReport_Click(): The Crystal Report LineDrivePreUpdate.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

        'crwReport.Reset()
        'crwReport.set_StoredProcParam(0, Me.txtFamily.Text)

        'If cmbBrand.SelectedIndex > -1 Then
        '    crwReport.set_StoredProcParam(1, VB6.GetItemData(cmbBrand, cmbBrand.SelectedIndex))
        'Else
        '    crwReport.set_StoredProcParam(1, "crwnull")
        'End If

        'crwReport.set_StoredProcParam(2, CDbl(txtPct.Text) / 100)
        'crwReport.set_StoredProcParam(3, StoreListGridGetStoreList(ugrdStoreList))
        'crwReport.set_StoredProcParam(4, "|")

        'crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "LineDrivePreUpdate.rpt"
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'crwReport.ReportTitle = String.Format(ResourcesPricing.GetString("LineDrivePreUpdateTitle"), vbCrLf, Me.txtPct.Text)
        'crwReport.Connect = gsCrystal_Connect
        'PrintReport(crwReport)

	End Sub
	
    Private Sub txtFamily_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtFamily.Enter

        HighlightText(txtFamily)

    End Sub
	
	Private Sub txtFamily_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtFamily.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtFamily.Tag), txtFamily, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub txtPct_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtPct.Enter
		
		HighlightText(txtPct)
		
	End Sub
	
	Private Sub txtPct_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtPct.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtPct.Tag), txtPct, 0, 0, 2)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

    Private Sub cmbPricingMethod_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbPricingMethod.SelectedIndexChanged

        If IsInitializing Then Exit Sub

        SetActive(txtField(iPriceEarnedDiscount1), False)
        SetActive(txtField(iPriceEarnedDiscount2), False)
        SetActive(txtField(iPriceEarnedDiscount3), False)

        Select Case ComboValue(cmbPricingMethod)
            Case CStr(0)
                txtField(iPriceEarnedDiscount3).Text = ResourcesIRMA.GetString("NinetyNine")
            Case CStr(1)
                txtField(iPriceEarnedDiscount3).Text = ResourcesIRMA.GetString("NinetyNine")
            Case CStr(2)
                SetActive(txtField(iPriceEarnedDiscount1), True)
                txtField(iPriceEarnedDiscount2).Text = ResourcesIRMA.GetString("One")
                txtField(iPriceEarnedDiscount3).Text = ResourcesIRMA.GetString("NinetyNine")
            Case CStr(4)
                txtField(iPriceEarnedDiscount1).Text = ResourcesIRMA.GetString("Zero")
                txtField(iPriceEarnedDiscount2).Text = ResourcesIRMA.GetString("NinetyNine")
                SetActive(txtField(iPriceEarnedDiscount3), True)
        End Select

    End Sub
	
    Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
        Dim Index As Short = txtField.GetIndex(eventSender)

        HighlightText(txtField(Index))

    End Sub
	Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim Index As Short = txtField.GetIndex(eventSender)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	

    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged

        If mbFilling Or IsInitializing Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)

            Call SetCombos()

            mbFilling = True

            Select Case Index
                Case 0
                    '-- Manual.
                    ugrdStoreList.Selected.Rows.Clear()
                    cmbZones.SelectedIndex = -1
                    cmbStates.SelectedIndex = -1
                Case 1
                    '-- All Stores.
                    Call StoreListGridSelectAll(ugrdStoreList, True)

                Case 2
                    '-- By Zones.
                    If cmbZones.SelectedIndex > -1 Then
                        Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
                    Else
                        ugrdStoreList.Selected.Rows.Clear()
                    End If

                Case 3
                    '-- By State.
                    If cmbStates.SelectedIndex > -1 Then
                        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemData(cmbStates, cmbStates.SelectedIndex))
                    Else
                        ugrdStoreList.Selected.Rows.Clear()
                    End If

                Case 4
                    '-- All WFM.
                    Call StoreListGridSelectAllWFM(ugrdStoreList)

            End Select

            ugrdStoreList.ActiveRow = Nothing

        mbFilling = False

        End If
    End Sub
	
    Private Sub SetCombos()

        mbFilling = True

        'Zones.
        If optSelection(2).Checked = True Then
            cmbZones.Enabled = True
            cmbZones.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbZones.SelectedIndex = -1
            cmbZones.Enabled = False
            cmbZones.BackColor = System.Drawing.SystemColors.Control
        End If

        'States.
        If optSelection(3).Checked = True Then
            cmbStates.Enabled = True
            cmbStates.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbStates.SelectedIndex = -1
            cmbStates.Enabled = False
            cmbStates.BackColor = System.Drawing.SystemColors.Control
        End If

        mbFilling = False

    End Sub
	
	Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged
        Dim iFirstStore As Short

        If IsInitializing Or mbFilling Then Exit Sub

        iFirstStore = -1

        mbFilling = True

        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        mbFilling = False


    End Sub
	
	Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged

        If mbFilling Or IsInitializing Then Exit Sub

        optSelection(2).Checked = True
        OptSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())

    End Sub
	
    Private Sub ugrdStoreList_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdStoreList.AfterSelectChange

        If mbFilling Or IsInitializing Then Exit Sub

        mbFilling = True
        optSelection.Item(0).Checked = True
        mbFilling = False

    End Sub
End Class