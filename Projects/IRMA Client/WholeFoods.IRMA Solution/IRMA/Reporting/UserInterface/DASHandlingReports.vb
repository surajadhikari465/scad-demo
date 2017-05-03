Option Strict Off
Option Explicit On

Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Friend Class DASHandlingChargeReport
    Inherits System.Windows.Forms.Form

    Private _isInitializing As Boolean
    Private _fillingData As Boolean
    Private _tblStores As New DataTable("StoreList")

    'Private Enum geStoreCol
    '    StoreNo = 0
    '    StoreName = 1
    '    ZoneID = 2
    '    State = 3
    '    WFMStore = 4
    '    MegaStore = 5
    '    CustomerType = 6
    'End Enum

    Private Sub Form_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim defaultDate As Date

        Me.Cursor = Cursors.WaitCursor

        Try
            _isInitializing = True

            CenterForm(Me)

            defaultDate = SystemDateTime()

            dtpStartDate.Value = defaultDate
            dtpEndDate.Value = defaultDate

            Call LoadDistSubTeam(cmbSubTeam)

            Call LoadStores()

        Finally
            _isInitializing = False
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub LoadStores()

        Dim viewStores As New DataView()

        Try
            _tblStores = StoreDAO.GetStoreList
            viewStores = _tblStores.DefaultView
            viewStores.AllowEdit = False

            ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended

            ugrdStoreList.DataSource = viewStores

            Call StoreListGridLoadStatesCombo(_tblStores, cmbStates)

            Call LoadZone(cmbZones)

        Finally
            Call SetStoreSelection(True)

        End Try

    End Sub

    Private Sub SetStoreSelection(ByRef bEnabled As Boolean)

        Dim ctrl As System.Windows.Forms.Control

        On Error Resume Next

        For Each ctrl In Me.Controls
            If ctrl.Parent.Name = "fraStores" Then
                ctrl.Enabled = bEnabled
            End If
        Next ctrl

        optSelection(0).Checked = True 'Set store selection back to manual.

        Call SetCombos()

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        Dim storeList As System.Text.StringBuilder
        Dim listDelimiter As String = "|"
        Dim identifier As String = txtIdentifier.Text.Trim
        Dim msg As String = String.Empty

        ' Validate parameters
        If ugrdStoreList.Selected.Rows.Count = 0 Then
            msg = "No stores have been selected."
        ElseIf cmbSubTeam.SelectedIndex = -1 Then
            msg = "A Distribution Sub-Team must be selected."
            cmbSubTeam.Focus()
        ElseIf identifier.Length > 0 AndAlso Not IsNumeric(identifier) Then
            msg = String.Format(ResourcesIRMA.GetString("msg_NumericValue"), ResourcesIRMA.GetString("Identifier"))
            txtIdentifier.SelectAll()
            txtIdentifier.Focus()
        ElseIf dtpEndDate.Value < dtpStartDate.Value Then
            msg = ResourcesIRMA.GetString("EndDateGreaterEqual")
            dtpEndDate.Focus()
        End If

        If msg.Length <> 0 Then
            MsgBox(msg, MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        ' Setup Report URL for Reporting Services
        Me.Text = "Running the DAS Handling Charge Report..."
        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("DASHandlingCharge")

        ' Report display
        'sReportURL.Append("&rs:Command=Render")   'optional
        sReportURL.Append("&rc:Parameters=False")

        ' Add Report Parameters
        ' Distribution SubTeam
        If cmbSubTeam.SelectedIndex <> -1 Then
            If VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex) = -1 Then
                sReportURL.Append("&DistributionSubTeamNo:IsNull=True")
            Else
                sReportURL.AppendFormat("&DistributionSubTeamNo={0}", VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
            End If

        End If

        ' Identifier
        If identifier.Length <> 0 Then
            sReportURL.AppendFormat("&Identifier={0}", identifier)
        End If

        ' StoreList
        storeList = New System.Text.StringBuilder()

        If Me.ugrdStoreList.Selected.Rows.Count <> 0 Then
            For Each row As UltraGridRow In Me.ugrdStoreList.Selected.Rows
                storeList.AppendFormat("{0}{1}", listDelimiter, row.Cells("Store_No").Value)
            Next
        End If

        If storeList.Length <> 0 Then
            'remove the leading delimiter
            storeList.Remove(0, listDelimiter.Length)
        End If

        sReportURL.AppendFormat("&StoreList={0}", storeList)

        ' Store List Separator
        sReportURL.AppendFormat("&ListDelimiter={0}", listDelimiter)

        ' Start Date
        If dtpStartDate.Text = "" Then
            sReportURL.Append("&StartRecvDate:IsNull=True")
        Else
            sReportURL.AppendFormat("&StartRecvDate={0}", dtpStartDate.Value.ToString("yyyy-MM-dd"))
        End If

        ' End Date
        If dtpEndDate.Text = "" Then
            sReportURL.Append("&EndRecvDate:IsNull=True")
        Else
            sReportURL.AppendFormat("&EndRecvDate={0}", dtpEndDate.Value.ToString("yyyy-MM-dd"))
        End If

        Call ReportingServicesReport(sReportURL.ToString)

    End Sub

    Private Sub optSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged

        If _fillingData OrElse _isInitializing Then
            Exit Sub
        End If

        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)

            Call SetCombos()

            _fillingData = True

            ugrdStoreList.Selected.Rows.Clear()

            cmbStates.Enabled = (Index = 3)
            cmbZones.Enabled = (Index = 2)

            Select Case Index
                Case 0
                    '-- Manual.
                    cmbZones.SelectedIndex = -1
                    cmbStates.SelectedIndex = -1
                Case 1
                    '-- All Stores
                    Call StoreListGridSelectAll(ugrdStoreList, True)
                Case 2
                    '-- By Zone
                    cmbZones.BringToFront()
                    cmbZones.Focus()
                    If cmbZones.SelectedIndex > -1 Then
                        Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
                    End If
                Case 3
                    '-- By State.
                    cmbStates.BringToFront()
                    cmbStates.Focus()
                    If cmbStates.SelectedIndex > -1 Then
                        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemData(cmbStates, cmbStates.SelectedIndex))
                    End If
                Case 4
                    '-- All WFM.
                    Call StoreListGridSelectAllWFM(ugrdStoreList)
                Case 5
                    '-- 5 = All Region.
                    StoreListGridSelectAllRegion(ugrdStoreList)
                Case 6
                    '-- 6 = All Region - Retail Only.
                    StoreListGridSelectRetailOnly(ugrdStoreList)
            End Select

            _fillingData = False

        End If

        'Call SetLevelButtons()

    End Sub

    Private Sub SetCombos()

        _fillingData = True

        'Zones.
        If optSelection(2).Checked = True Then
            SetActive(cmbZones, True)
        Else
            cmbZones.SelectedIndex = -1
            SetActive(cmbZones, False)
        End If

        'States.
        If optSelection(3).Checked = True Then
            SetActive(cmbStates, True)
        Else
            cmbStates.SelectedIndex = -1
            SetActive(cmbStates, False)
        End If

        _fillingData = False

    End Sub

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged

        If _fillingData OrElse _isInitializing Then Exit Sub

        _fillingData = True

        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        _fillingData = False

    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged

        If _fillingData OrElse _isInitializing Then Exit Sub

        optSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())

    End Sub

    Private Sub ugrdStoreList_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdStoreList.AfterSelectChange

        If _fillingData OrElse _isInitializing Then Exit Sub

        _fillingData = True
        optSelection.Item(0).Checked = True
        _fillingData = False

    End Sub
End Class