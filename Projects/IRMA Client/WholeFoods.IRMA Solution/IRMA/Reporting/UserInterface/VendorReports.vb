Option Strict Off
Option Explicit On

Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports System.Web


Friend Class frmVendorReports
	Inherits System.Windows.Forms.Form

    Private mdtBrand As DataTable

	Private mbNoClick As Boolean
    Private mbFilling As Boolean
    Private IsInitializing As Boolean
    Private mdtStores As DataTable

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Enum geStoreCol
		StoreNo = 0
		StoreName = 1
		ZoneID = 2
		State = 3
		WFMStore = 4
		MegaStore = 5
		CustomerType = 6
    End Enum

    Private Sub frmVendorReports_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmVendorReports_Load Entry")

        optRegionSingleRpt.Checked = True

        Call LoadZone(cmbZoneSingleRpt)
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        If glVendorID <> 0 Then
            'vendor preselected so disallow selecting another vendor

            Call GetVendorName(glVendorID)
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Me.cmdCompanySearch.Visible = False
            Me.txtVendorName.Width = 228
        Else
            'vendor not Pre Selected so allow selecting a vendor
            Me.cmdCompanySearch.Visible = True
            Me.txtVendorName.Width = 202
        End If

        Call SetActive(txtVendorName, False)

        ''Call LoadSubTeamByType(enumSubTeamType.All, cmbSubTeam)
        ''System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        If InstanceDataDAO.IsFlagActive("FourLevelHierarchy") Then
            cmbTeam.Visible = False
            Label1.Visible = False
            HierarchySelector1.Enabled = True
        Else
            cmbTeam.Visible = True
            HierarchySelector1.Enabled = False
        End If

        Call LoadTeam(cmbTeam)

        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        mdtBrand = GetBrandData()
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        cmbBrand.DataSource = mdtBrand
        cmbBrand.DisplayMember = "Brand_Name"
        cmbBrand.ValueMember = "Brand_ID"
        cmbBrand.SelectedIndex = -1

        '-- Fill out the store list
        'Include the distribution centers in the store list TFS 6625
        mdtStores = StoreDAO.GetStoreAndDCList()

        ugrdStoreList.DataSource = mdtStores

        Call LoadZone(cmbZones)

        Call StoreListGridLoadStatesCombo(mdtStores, cmbStates)

        Call SetCombos()

        If Not CheckAllStoreSelectionEnabled() Then
            _optSelection_1.Text = "All 365"
        End If

        CenterForm(Me)

        logger.Debug("frmVendorReports_Load Exit")

    End Sub

    Private Sub SetFormWidth()

        If Me.optEachStore.Checked = True Then
            'Wide.
            Me.Width = 681
            Me.fraButtons.Left = 480
            Me.Height = 477
        Else
            'Narrow.
            Me.Width = 350
            Me.fraButtons.Left = 196
            Me.Height = 477
        End If

    End Sub

    Private Sub cmbTeam_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbTeam.SelectedIndexChanged

        logger.Debug("cmbTeam_SelectedIndexChanged Entry")

        If IsInitializing Then Exit Sub

        ' Commented by sekhara to fix bug 6417.
        'If cmbTeam.SelectedIndex = -1 Then
        '    SetActive((HierarchySelector1), True)
        '    SetActive((cmbSubTeam), True)
        'Else
        '    SetActive((cmbCategory), False)
        '    SetActive((cmbSubTeam), False)
        'End If

        logger.Debug("cmbTeam_SelectedIndexChanged Exit")


    End Sub

    Private Sub cmbTeam_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTeam.KeyPress
        logger.Debug("cmbTeam_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbTeam.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbTeam_KeyPress Exit")
    End Sub

    Private Sub cmdCompanySearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCompanySearch.Click


        logger.Debug("cmdCompanySearch_Click Entry")

        glVendorID = 0
        '-- Set the search type
        giSearchType = iSearchAllVendors
        '-- Open the search form
        frmSearch.Text = "Search for Vendor by Company Name"
        frmSearch.ShowDialog()

        frmSearch.Close()
        frmSearch.Dispose()

        '-- if its not zero, then something was found
        If glVendorID <> 0 Then Call GetVendorName(glVendorID)

        logger.Debug("cmdCompanySearch_Click Entry")


    End Sub

    Private Sub GetVendorName(ByRef lVendorID As Integer)

        logger.Debug("GetVendorName Entry")

        Dim rsVendor As DAO.Recordset = Nothing
        Try
            rsVendor = SQLOpenRecordSet("EXEC GetVendorName " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Me.txtVendorName.Text = rsVendor.Fields("CompanyName").Value
        Finally
            If rsVendor IsNot Nothing Then
                rsVendor.Close()
                rsVendor = Nothing
            End If
        End Try

        logger.Debug("GetVendorName Exit")


    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")

    End Sub

    Private Sub RunReport(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click


        logger.Debug("RunReport Entry")

        Dim oRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim sReportURL As New System.Text.StringBuilder
        Dim sStoreNoList As String = String.Empty

        Const DELIM As Char = ","   'list separator

        'validate input
        If glVendorID = 0 Then
            MsgBox("No vendor was selected.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
            logger.Info("No vendor was selected.")
            logger.Debug("RunReport Exit")
            Exit Sub
        End If

        If optZoneSingleRpt.Checked AndAlso Me.cmbZoneSingleRpt.SelectedIndex = -1 Then
            MsgBox("No zone was selected.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
            logger.Info("No zone was selected.")
            logger.Debug("RunReport Exit")
            Exit Sub
        End If

        '--------------------------
        ' Setup Report Parameters.
        '--------------------------
        'report name
        sReportURL.Append("ItemsByVendor")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Vendor_ID=" & glVendorID)

        ' New code by sekhara to fix 6417(New report Parameter).
        If txtVendorName.Text.Trim.Length > 0 Then
            sReportURL.Append("&Vendor_Name=" & HttpUtility.UrlEncode(txtVendorName.Text.Trim))
        Else
            sReportURL.Append("&Vendor_Name:isnull=true")
        End If

        Select Case True
            Case optRegionSingleRpt.Checked
                sReportURL.Append("&IsRegional=True")

                ' New code by sekhara to fix 6417
                sReportURL.Append("&Zone_ID:isnull=true")
                sReportURL.Append("&Store_No_List:isnull=true")
                sReportURL.Append("&Zone_Name:isnull=true")

            Case optZoneSingleRpt.Checked
                sReportURL.Append("&Zone_ID=" & VB6.GetItemData(cmbZoneSingleRpt, cmbZoneSingleRpt.SelectedIndex))

                ' New code by sekhara to fix 6417
                sReportURL.Append("&Zone_Name=" & cmbZoneSingleRpt.Text.Trim)
                sReportURL.Append("&IsRegional=False")
                sReportURL.Append("&Store_No_List:isnull=true")

            Case optEachStore.Checked
                'Create list of stores for report from those selected in the grid
                For Each oRow In ugrdStoreList.Selected.Rows
                    sStoreNoList += DELIM & oRow.Cells("Store_No").Value.ToString
                Next
                If Len(sStoreNoList) = 0 Then
                    MsgBox("At least one store must be selected.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
                    logger.Info("At least one store must be selected.")
                    logger.Debug("RunReport Exit")
                    Exit Sub
                Else
                    'remove the leading delimiter and append the parameter
                    sReportURL.Append("&Store_No_List=" & Mid(sStoreNoList, Len(DELIM)))

                    ' New code by sekhara to fix 6417
                    sReportURL.Append("&IsRegional=False")
                    sReportURL.Append("&Zone_ID:isnull=true")
                    sReportURL.Append("&Zone_Name:isnull=true")
                End If

        End Select

        If cmbTeam.Text = "" Or cmbTeam.Text = "ALL" Then
            sReportURL.Append("&Team_No:isnull=true")
        Else
            sReportURL.Append("&Team_No=" & VB6.GetItemData(cmbTeam, cmbTeam.SelectedIndex))
        End If

        ' New code by sekhara to fix bug 6417(Using HierarchySelector control).

        If HierarchySelector1.cmbSubTeam.Text = "" Or HierarchySelector1.cmbSubTeam.Text = "ALL" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(HierarchySelector1.cmbSubTeam, HierarchySelector1.cmbSubTeam.SelectedIndex))
        End If

        If HierarchySelector1.cmbCategory.Text = "" Or HierarchySelector1.cmbCategory.Text = "ALL" Then
            sReportURL.Append("&Category_ID:isnull=true")
        Else
            sReportURL.Append("&Category_ID=" & VB6.GetItemData(HierarchySelector1.cmbCategory, HierarchySelector1.cmbCategory.SelectedIndex))
        End If

        If HierarchySelector1.cmbLevel3.Text = "" Or HierarchySelector1.cmbLevel3.Text = "ALL" Then
            sReportURL.Append("&Level3_ID:isnull=true")
        Else
            sReportURL.Append("&Level3_ID=" & VB6.GetItemData(HierarchySelector1.cmbLevel3, HierarchySelector1.cmbLevel3.SelectedIndex))
        End If

        If HierarchySelector1.cmbLevel4.Text = "" Or HierarchySelector1.cmbLevel4.Text = "ALL" Then
            sReportURL.Append("&Level4_ID:isnull=true")
        Else
            sReportURL.Append("&Level4_ID=" & VB6.GetItemData(HierarchySelector1.cmbLevel4, HierarchySelector1.cmbLevel4.SelectedIndex))
        End If


        If cmbBrand.Text.Trim = "" Or cmbBrand.Text = "ALL" Then
            sReportURL.Append("&Brand_ID:isnull=true")
        Else
            ' Changed by sekhara to fix 6714.
            ' sReportURL.Append("&Brand_ID=" & VB6.GetItemData(cmbBrand, cmbBrand.SelectedIndex))
            sReportURL.Append("&Brand_ID=" & cmbBrand.SelectedValue)
        End If


        If Len(txtUPC.Text) <> 0 Then
            If Not (IsNumeric(txtUPC.Text)) Or InStr(1, txtUPC.Text, ".") <> 0 Or InStr(1, txtUPC.Text, "$") <> 0 Then
                MsgBox("UPC may only contain numbers.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
                logger.Info("UPC may only contain numbers.")
                logger.Debug("RunReport Exit")

                Exit Sub
            Else
                sReportURL.Append("&Identifier=" & txtUPC.Text)
            End If
        Else
            ' Added by sekhara to fix 6714.
            sReportURL.Append("&Identifier:isnull=true")
        End If

        '--------------
        ' Print Report.
        '--------------
        Call ReportingServicesReport(sReportURL.ToString)

        logger.Debug("RunReport Exit")

    End Sub

    Private Sub gridStoreList_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

        logger.Debug("gridStoreList_ClickEvent Entry")
        optSelection(0).Checked = True
        logger.Debug("gridStoreList_ClickEvent Exit")

    End Sub

    Private Sub optEachStore_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optEachStore.CheckedChanged
        'If option is selected it resets the tabindex to include the extended window.
        logger.Debug("optEachStore_CheckedChanged Entry")

        fraButtons.TabIndex = 27

        If IsInitializing Then Exit Sub

        If eventSender.Checked Then

            Call SetFormWidth()
            Call SetReportOpts()

        End If

        logger.Debug("optEachStore_CheckedChanged Exit")
    End Sub

    Private Sub optRegionSingleRpt_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optRegionSingleRpt.CheckedChanged

        logger.Debug("optRegionSingleRpt_CheckedChanged Entry")

        fraButtons.TabIndex = 5
        If IsInitializing Then Exit Sub

        If eventSender.Checked Then

            Call SetFormWidth()
            Call SetReportOpts()

        End If
        logger.Debug("optRegionSingleRpt_CheckedChanged Exit")
    End Sub

    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged

        logger.Debug("OptSelection_CheckedChanged Entry")

        fraButtons.TabIndex = 27
        If mbFilling Or IsInitializing Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)

            Call SetCombos()

            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            Select Case Index
                Case 0
                    '-- Manual.
                    cmbZones.SelectedIndex = -1
                    cmbStates.SelectedIndex = -1
                Case 1
                    '-- All Stores or All 365 for RM
                    If CheckAllStoreSelectionEnabled() Then
                        Call StoreListGridSelectAll(ugrdStoreList, True)
                    Else
                        Call StoreListGridSelectAll365(ugrdStoreList)
                    End If
                Case 2
                    '-- By Zone
                    If cmbZones.SelectedIndex > -1 Then Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
                Case 3
                    '-- By State.
                    If cmbStates.SelectedIndex > -1 Then Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemData(cmbStates, cmbStates.SelectedIndex))
                Case 4
                    '-- All WFM.
                    Call StoreListGridSelectAllWFM(ugrdStoreList)
            End Select

            mbFilling = False

        End If

        logger.Debug("OptSelection_CheckedChanged Exit")

    End Sub

    Private Sub SetCombos()

        logger.Debug("SetCombos Entry")


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

        logger.Debug("SetCombos Exit")

    End Sub

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged

        If mbFilling Or IsInitializing Then Exit Sub

        logger.Debug("cmbStates_SelectedIndexChanged Entry")

        mbFilling = True

        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        mbFilling = False

        logger.Debug("cmbStates_SelectedIndexChanged Exit")

    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged

        If mbFilling Or IsInitializing Then Exit Sub

        logger.Debug("cmbZones_SelectedIndexChanged Entry")

        OptSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())

        logger.Debug("cmbZones_SelectedIndexChanged Exit")

    End Sub

    Private Sub optZoneSingleRpt_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optZoneSingleRpt.CheckedChanged
        If IsInitializing Then Exit Sub

        logger.Debug("optZoneSingleRpt_CheckedChanged Entry")

        If eventSender.Checked Then

            Call SetFormWidth()
            Call SetReportOpts()

        End If

        logger.Debug("optZoneSingleRpt_CheckedChanged Exit")
    End Sub

    Private Sub SetReportOpts()
        logger.Debug("SetReportOpts Entry")

        If optZoneSingleRpt.Checked = True Then
            cmbZoneSingleRpt.Enabled = True
            cmbZoneSingleRpt.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbZoneSingleRpt.Enabled = False
            cmbZoneSingleRpt.SelectedIndex = -1
            cmbZoneSingleRpt.BackColor = System.Drawing.SystemColors.Control
        End If
        logger.Debug("SetReportOpts Exit")

    End Sub


    ' Commented by sekhara to fix 6417.
    ''********************************
    ''SubTeam / Catagory Code
    ''********************************
    'Private Sub cmbSubTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    '    If IsInitializing Then Exit Sub

    '    logger.Debug("cmbSubTeam_SelectedIndexChanged Entry")

    '    ''If cmbSubTeam.SelectedIndex = -1 Then
    '    ''    cmbCategory.Items.Clear()
    '    ''    SetActive(cmbTeam, True)
    '    ''Else
    '    ''    SetActive(cmbTeam, False)
    '    ''    Call LoadCategory(cmbCategory, VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
    '    ''    SetActive(cmbCategory, True)
    '    ''End If

    '    logger.Debug("cmbSubTeam_SelectedIndexChanged Exit")

    'End Sub

    'Private Sub cmbSubTeam_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    '    If cmbSubTeam.Text = "" Then cmbSubTeam.SelectedIndex = -1

    'End Sub

    ''Private Sub cmbSubTeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

    ''    logger.Debug("cmbSubTeam_KeyPress Entry")
    ''    Dim KeyAscii As Short = Asc(e.KeyChar)

    ''    If KeyAscii = 8 Then
    ''        cmbSubTeam.SelectedIndex = -1
    ''    End If

    ''    e.KeyChar = Chr(KeyAscii)
    ''    If KeyAscii = 0 Then
    ''        e.Handled = True
    ''    End If
    ''    logger.Debug("cmbSubTeam_KeyPress Exit")
    ''End Sub

    'Private Sub cmbCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

    '    logger.Debug("cmbCategory_KeyPress Entry")
    '    Dim KeyAscii As Short = Asc(e.KeyChar)

    '    If KeyAscii = 8 Then
    '        cmbCategory.SelectedIndex = -1
    '    End If

    '    e.KeyChar = Chr(KeyAscii)
    '    If KeyAscii = 0 Then
    '        e.Handled = True
    '    End If

    '    logger.Debug("cmbCategory_KeyPress Exit")
    'End Sub

    Private Sub cmbBrand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbBrand.KeyPress

        logger.Debug("cmbBrand_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbBrand.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbBrand_KeyPress Exit")
    End Sub

    '*************************************
    'End SubTeam / Catagory Code
    '*************************************

    Private Sub cmbZoneSingleRpt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbZoneSingleRpt.KeyPress

        logger.Debug("cmbZoneSingleRpt_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbZoneSingleRpt.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbZoneSingleRpt_KeyPress Exit")
    End Sub

    Private Sub ugrdStoreList_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdStoreList.AfterSelectChange

        If mbFilling Or IsInitializing Then Exit Sub

        logger.Debug("ugrdStoreList_AfterSelectChange Entry")

        mbFilling = True
        optSelection.Item(0).Checked = True
        mbFilling = False

        logger.Debug("ugrdStoreList_AfterSelectChange Exit")

    End Sub
    
    
End Class
